using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class implicitGPU : MonoBehaviour
{
    #region simulation parameters
    float mass = 1;
    float damping = 0.99f;
    float spring_k = 8000;

    Vector3 gravity = new Vector3(0f, -9.8f, 0f);
    float r = 2.7f;   //hard-code the radius
    //float omega = 0f;

    Mesh mesh;
    public Transform Sphere;
    public ComputeShader SolverShader;

    int totalEdges;
    int totalVerts;  //for the dispatch return
    int res = 21;
    #endregion

    #region kernels
    int kernel_gradient_v;
    int kernel_gradient_e;
    int kernel_gradient_a;   //add to the final G
    int kernel_initSetup;
    int kernel_updateV;
    int kernel_updateX;
    int kernel_collision;
    #endregion

    #region compute buffers
    ComputeBuffer EO_com;
    ComputeBuffer ED_com;
    ComputeBuffer L_com;
    ComputeBuffer X_com;
    ComputeBuffer X_hat_com;
    ComputeBuffer V_com;
    ComputeBuffer G_com;
    ComputeBuffer G_Delta_com;
    #endregion

    #region Simulation and Model
    [SerializeField]
    public uint iterationSteps = 10;
    [SerializeField]
    public float deltaTimeStep = 0.03f;
    [SerializeField]
    public bool startSimulationOnPlay = false;
    [SerializeField]
    public float timePassed = 0f;

    int[] EO;   //for the cloth model structure
    int[] ED;
    float[] L;
    Vector3[] pos;
    Vector3[] vel;
    Vector3[] G;
    Vector3 sphere_center;

    struct int3
    {
        public int x;
        public int y;
        public int z;
        public int3(int ix, int iy, int iz)
        {
            x = ix;
            y = iy;
            z = iz;
        }
    };
    int threadGroups;    //for the thread group dispatching
    int threadGroups_GE;
    #endregion

    #region Set GPU Variables
    void SetGPUParameters()
    {
        //set up constants
        SolverShader.SetFloat("t", deltaTimeStep);
        SolverShader.SetFloat("mass", mass);
        SolverShader.SetFloat("damping", damping);
        SolverShader.SetFloat("spring_k", spring_k);
        SolverShader.SetFloats("gravity", new float[] { gravity.x, gravity.y, gravity.z });
        SolverShader.SetFloats("r", r);
        SolverShader.SetInt("totalVerts", totalVerts);
        SolverShader.SetInt("totalEdges", totalEdges);

        print("Total Verts: " + totalVerts);
        print("Total Edges: " + totalEdges);
    }


    void SetGPUBuffer()   //set once should be enough
    {
        //set buffers for kernels accordingly
        //1. apply force and initial setup
        SolverShader.SetBuffer(kernel_initSetup, "X", X_com);
        SolverShader.SetBuffer(kernel_initSetup, "X_hat", X_hat_com);   //save to the previous 
        SolverShader.SetBuffer(kernel_initSetup, "V", V_com);
        SolverShader.SetBuffer(kernel_initSetup, "G", G_com);

        //2. set the gradient part V and E
        SolverShader.SetBuffer(kernel_gradient_v, "G", G_com);
        SolverShader.SetBuffer(kernel_gradient_v, "X", X_com);
        SolverShader.SetBuffer(kernel_gradient_v, "X_hat", X_hat_com);
        SolverShader.SetBuffer(kernel_gradient_v, "G_Edelta", G_Delta_com);

        SolverShader.SetBuffer(kernel_gradient_e, "G", G_com);
        SolverShader.SetBuffer(kernel_gradient_e, "EO", EO_com);
        SolverShader.SetBuffer(kernel_gradient_e, "ED", ED_com);
        SolverShader.SetBuffer(kernel_gradient_e, "X", X_com);
        SolverShader.SetBuffer(kernel_gradient_e, "L", L_com);
        SolverShader.SetBuffer(kernel_gradient_e, "G_Edelta", G_Delta_com);

        SolverShader.SetBuffer(kernel_gradient_a, "G", G_com);
        SolverShader.SetBuffer(kernel_gradient_a, "G_Edelta", G_Delta_com);

        //3. Update Position
        SolverShader.SetBuffer(kernel_updateX, "X", X_com);
        SolverShader.SetBuffer(kernel_updateX, "G", G_com);

        //4. Update Velocity
        SolverShader.SetBuffer(kernel_updateV, "V", V_com);
        SolverShader.SetBuffer(kernel_updateV, "X", X_com);
        SolverShader.SetBuffer(kernel_updateV, "X_hat", X_hat_com);

        //5. Collision Handle
        sphere_center = Sphere.position;
        SolverShader.SetBuffer(kernel_collision, "V", V_com);
        SolverShader.SetBuffer(kernel_collision, "X", X_com);

        //6. Assign the initial values
        pos = new Vector3[totalVerts];
        vel = new Vector3[totalVerts];
        Vector3[] X_hat = new Vector3[totalVerts];
        G = new Vector3[totalVerts];
        int3 []G_delta = new int3[totalVerts];
        for(int i=0; i< totalVerts; i++)
        {
            vel[i] = Vector3.zero;
            X_hat[i] = Vector3.zero;
            pos[i] = new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z);
            G[i] = Vector3.zero;
            G_delta[i] = new int3(0, 0, 0);
        }

        X_com.SetData(pos);
        X_hat_com.SetData(X_hat);
        G_com.SetData(G);
        G_Delta_com.SetData(G_delta);
        V_com.SetData(vel);
        L_com.SetData(L);
        EO_com.SetData(EO);
        ED_com.SetData(ED);

        //lastly setup the thread group size
        uint threadnum = 0;
        SolverShader.GetKernelThreadGroupSizes(kernel_initSetup, out threadnum, out _, out _);
        threadGroups = (int)((res * res + (threadnum - 1)) / threadnum);
        SolverShader.GetKernelThreadGroupSizes(kernel_gradient_e, out threadnum, out _, out _);
        threadGroups_GE = (int)((EO.Length + (threadnum - 1)) / threadnum);

    }
    #endregion

    #region ModelStructure
    void initModelStructure()   //define the model structure like verts and edges
    {
        //Resize the mesh to n*n
        mesh = GetComponent<MeshFilter>().mesh;
        int n = res = 21;
        Vector3[] X = new Vector3[n * n];
        Vector2[] UV = new Vector2[n * n];
        int[] triangles = new int[(n - 1) * (n - 1) * 6];
        for (int j = 0; j < n; j++)
            for (int i = 0; i < n; i++)
            {
                X[j * n + i] = new Vector3(5 - 10.0f * i / (n - 1), 0, 5 - 10.0f * j / (n - 1));
                UV[j * n + i] = new Vector3(i / (n - 1.0f), j / (n - 1.0f));    // assign [0 - 1]
            }
        int t = 0;
        for (int j = 0; j < n - 1; j++)
            for (int i = 0; i < n - 1; i++)
            {
                triangles[t * 6 + 0] = j * n + i;
                triangles[t * 6 + 1] = j * n + i + 1;
                triangles[t * 6 + 2] = (j + 1) * n + i + 1;
                triangles[t * 6 + 3] = j * n + i;
                triangles[t * 6 + 4] = (j + 1) * n + i + 1;
                triangles[t * 6 + 5] = (j + 1) * n + i;
                t++;
            }
        mesh.vertices = X;
        mesh.triangles = triangles;
        mesh.uv = UV;
        mesh.RecalculateNormals();
        totalVerts = X.Length;

        //Construct the original E, edge index pair
        int[] _E = new int[triangles.Length * 2];
        for (int i = 0; i < triangles.Length; i += 3)
        {
            _E[i * 2 + 0] = triangles[i + 0];    // two es for one edges
            _E[i * 2 + 1] = triangles[i + 1];
            _E[i * 2 + 2] = triangles[i + 1];
            _E[i * 2 + 3] = triangles[i + 2];
            _E[i * 2 + 4] = triangles[i + 2];
            _E[i * 2 + 5] = triangles[i + 0];
        }
        //Reorder the original edge list
        for (int i = 0; i < _E.Length; i += 2)
            if (_E[i] > _E[i + 1])
                Swap(ref _E[i], ref _E[i + 1]);
        //Sort the original edge list using quicksort
        Quick_Sort(ref _E, 0, _E.Length / 2 - 1);

        int e_number = 0;
        for (int i = 0; i < _E.Length; i += 2)
            if (i == 0 || _E[i + 0] != _E[i - 2] || _E[i + 1] != _E[i - 1])
                e_number++;

        EO = new int[e_number];
        ED = new int[e_number];
        totalEdges = e_number;  //assign the total edges
        for (int i = 0, e = 0; i < _E.Length; i += 2)
            if (i == 0 || _E[i + 0] != _E[i - 2] || _E[i + 1] != _E[i - 1])
            {
                EO[e] = _E[i + 0];
                ED[e] = _E[i + 1];
                e++;
            }

        L = new float[e_number];
        for (int e = 0; e < e_number; e++)
        {
            int v0 = EO[e];
            int v1 = ED[e];
            L[e] = (X[v0] - X[v1]).magnitude;
            //print("L: " + L[e]);
        }


    }
    void Quick_Sort(ref int[] a, int l, int r)
    {
        int j;
        if (l < r)
        {
            j = Quick_Sort_Partition(ref a, l, r);
            Quick_Sort(ref a, l, j - 1);
            Quick_Sort(ref a, j + 1, r);
        }
    }

    int Quick_Sort_Partition(ref int[] a, int l, int r)
    {
        int pivot_0, pivot_1, i, j;
        pivot_0 = a[l * 2 + 0];
        pivot_1 = a[l * 2 + 1];
        i = l;
        j = r + 1;
        while (true)
        {
            do ++i; while (i <= r && (a[i * 2] < pivot_0 || a[i * 2] == pivot_0 && a[i * 2 + 1] <= pivot_1));
            do --j; while (a[j * 2] > pivot_0 || a[j * 2] == pivot_0 && a[j * 2 + 1] > pivot_1);
            if (i >= j) break;
            Swap(ref a[i * 2], ref a[j * 2]);
            Swap(ref a[i * 2 + 1], ref a[j * 2 + 1]);
        }
        Swap(ref a[l * 2 + 0], ref a[j * 2 + 0]);
        Swap(ref a[l * 2 + 1], ref a[j * 2 + 1]);
        return j;
    }

    void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }
    #endregion
    #region GPU Kernel and Buffer Initialization
    void InitKernels()
    {

        kernel_initSetup = SolverShader.FindKernel("Initial_SetUp");
        kernel_gradient_e = SolverShader.FindKernel("Get_Gradient_E");
        kernel_gradient_a = SolverShader.FindKernel("AddtoFinalG");
        kernel_gradient_v = SolverShader.FindKernel("Get_Gradient_V");
        kernel_collision = SolverShader.FindKernel("CollisionHandle");
        kernel_updateV = SolverShader.FindKernel("Update_Velocity");
        kernel_updateX = SolverShader.FindKernel("Update_Position");

        //print("Kernel Index", )
    }

    //init compute buffer size
    void InitGPUBuffers()
    {
        EO_com = new ComputeBuffer(totalEdges, sizeof(int));
        ED_com = new ComputeBuffer(totalEdges, sizeof(int));
        L_com = new ComputeBuffer(totalEdges, sizeof(float));
        X_com = new ComputeBuffer(totalVerts, sizeof(float) * 3);
        V_com = new ComputeBuffer(totalVerts, sizeof(float) * 3);
        X_hat_com = new ComputeBuffer(totalVerts, sizeof(float) * 3);
        G_com = new ComputeBuffer(totalVerts, sizeof(float) * 3);
        G_Delta_com = new ComputeBuffer(totalVerts, sizeof(int) * 3);

    }

    #endregion


    void SimulationOneStep()
    {
        //int vertGroupsize = (totalVerts + )
        #region Initial Setup
        SolverShader.Dispatch(kernel_initSetup, threadGroups, 1, 1);
        #endregion

        for (int i = 0; i < iterationSteps; i++) {
            #region Compute Gradient V
            SolverShader.Dispatch(kernel_gradient_v, threadGroups, 1, 1);
            SolverShader.Dispatch(kernel_gradient_e, threadGroups_GE, 1, 1);
            SolverShader.Dispatch(kernel_gradient_a, threadGroups, 1, 1);

            #endregion
            /*
            #region Compute Gradient E
            //have to do with the sequential way for now
            G_com.GetData(G);
            X_com.GetData(pos);
            for(int e=0; e<EO.Length; e++)
            {
                int vi = EO[e];
                int vj = ED[e];
                float len = (pos[vj] - pos[vi]).magnitude;
                G[vi] += spring_k * (1.0f - L[e] / len) * (pos[vi] - pos[vj]);
                G[vj] -= spring_k * (1.0f - L[e] / len) * (pos[vi] - pos[vj]);
            }
            G_com.SetData(G);
            #endregion
            */

            //var fence = Graphics.CreateGraphicsFence(UnityEngine.Rendering.GraphicsFenceType.AsyncQueueSynchronisation, UnityEngine.Rendering.SynchronisationStageFlags.ComputeProcessing);
            //Graphics.WaitOnAsyncGraphicsFence(fence);
            #region Update Position
            SolverShader.Dispatch(kernel_updateX, threadGroups, 1, 1);
            #endregion

           

        }
        // G_com.GetData(vel);
        //print("G[10]: " + vel[100]);
        #region Update Velocity
        SolverShader.Dispatch(kernel_updateV, threadGroups, 1, 1);
        #endregion

        //V_com.GetData(vel);
        //print("V[10]: " + vel[100]);
        #region Collision Handle
        sphere_center = Sphere.position;
        SolverShader.SetFloats("sphere_center", new float[] { sphere_center.x, sphere_center.y, sphere_center.z });
        SolverShader.Dispatch(kernel_collision, threadGroups, 1, 1);
        #endregion

    }

    void UpdateDataToMesh()
    {
        // utilize global position (Vector3) to update vertices position
        X_com.GetData(pos);
        mesh.vertices = pos;
        //V_com.GetData(vel);
        //print("V[10]: " + vel[10]);
        mesh.RecalculateNormals();
    }

    private void Awake()
    {
        startSimulationOnPlay = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitKernels();
        initModelStructure();
        InitGPUBuffers();
        SetGPUBuffer();
        SetGPUParameters();   //only need to be called once

        startSimulationOnPlay = true;   //initialization is finished and let's go
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= deltaTimeStep) timePassed = 0.0f;
        if (startSimulationOnPlay && timePassed == 0.0f)
        {
            SimulationOneStep();
            UpdateDataToMesh();
        }

    }

    private void OnDestroy()
    {
        if(X_com!=null) X_com.Dispose();
        if (V_com != null) V_com.Dispose();
        if (G_com != null) G_com.Dispose();
        if (EO_com != null) EO_com.Dispose();
        if (ED_com != null) ED_com.Dispose();
        if (L_com != null) L_com.Dispose();
        if (X_hat_com != null) X_hat_com.Dispose();

        print("End");
    }

    
}
