using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FVMGPU : MonoBehaviour
{
    #region Simulation Parameters
    float dt = 0.003f;
    float mass = 1f;
    float stiffness_0 = 20000.0f;
    float stiffness_1 = 5000.0f;
    float damp = 0.999f;
    #endregion

    #region Model
    int tet_number;
    int total_verts;
    Mesh mesh;
    #endregion

    #region External Parameters
    SVD svd = new SVD();
    Vector3 gravity;
    Vector3 N;
    Vector3 P;  //N and P are defined for representing the plane for collision
    #endregion
    #region Public Parameters
    public ComputeShader SolverShader;
    //define the plane for simple collision detection
    public float u_T = 0.5f;   //for the collision impulse-based
    public float u_N = 0.1f;
    public float blend_a = 0.3f;
    #endregion

    #region Compute Buffer
    ComputeBuffer Tet_com;
    ComputeBuffer Force_com;
    ComputeBuffer Force_delta;
    ComputeBuffer V_com;
    ComputeBuffer X_com;  //for Update Mesh
    ComputeBuffer inv_Dm_com;
    ComputeBuffer vertices_com;
    

    //For Laplacian Smoothing
    ComputeBuffer V_sum_com;
    ComputeBuffer V_sum_delta;
    ComputeBuffer V_num_com;
    #endregion

    #region Kernels
    int Build_InvDm;
    int Laplacian_Smoothing_v1;
    int Laplacian_Smoothing_v2;    //v2 - interlocked adding 
    int Laplacian_Smoothing_v3;    //v3 - value beck to the v_sum  
    int Laplacian_Smoothing_v4;    //v4 - perform the blending
    int Jump_up;   //if key down, X - values up
    int Force_Init;  //include deform_grad, Green_Strain, Second_PK_Stress
    int Force_Comp;
    int Force_Final;   //final addition of force
    int VX_Update;   //include collision detection already
    int init_invDm;  // just in case for the zero determinant happened? need to figure out it then
    Vector3[] Force;
    #endregion


    #region SimulationStep and ModelStructure
    [SerializeField]
    public uint iterationSteps = 15;
    [SerializeField]
    public float deltaTimeStep = 0.03f;
    [SerializeField]
    public bool startSimulationOnPlay = false;
    [SerializeField]
    public float timePassed = 0f;

    int threadGroups_X;    //for the thread group dispatching
    int threadGroups_Tet;

    int[] Tet;
    
    Vector3[] vertices;   //global parts for Rendering Mesh
    Vector3[] X;
    Matrix4x4 [] inv_Dm;

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

   
    public void InitModelStructure()
    {
        
        //setup gravity parameter
        gravity = new Vector3(0f, -9.8f, 0f);
        N = new Vector3(0, 1f, 0);   //N for the Plane, collision detection
        P = new Vector3(0, -2.999f, 0);

          
        // FILO IO: Read the house model from files.
        // The model is from Jonathan Schewchuk's Stellar lib.
        {
            string fileContent = File.ReadAllText("Assets/hw3/house2.ele");
            string[] Strings = fileContent.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            tet_number = int.Parse(Strings[0]);
            Tet = new int[tet_number * 4];

            for (int tet = 0; tet < tet_number; tet++)
            {
                Tet[tet * 4 + 0] = int.Parse(Strings[tet * 5 + 4]) - 1;
                Tet[tet * 4 + 1] = int.Parse(Strings[tet * 5 + 5]) - 1;
                Tet[tet * 4 + 2] = int.Parse(Strings[tet * 5 + 6]) - 1;
                Tet[tet * 4 + 3] = int.Parse(Strings[tet * 5 + 7]) - 1;
            }
        }

        {
            string fileContent = File.ReadAllText("Assets/hw3/house2.node");
            string[] Strings = fileContent.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            total_verts = int.Parse(Strings[0]);
            X = new Vector3[total_verts];
            for (int i = 0; i < total_verts; i++)
            {
                X[i].x = float.Parse(Strings[i * 5 + 5]) * 0.4f;
                X[i].y = float.Parse(Strings[i * 5 + 6]) * 0.4f;
                X[i].z = float.Parse(Strings[i * 5 + 7]) * 0.4f;
            }
            //Centralize the model.
            Vector3 center = Vector3.zero;
            for (int i = 0; i < total_verts; i++) center += X[i];
            center = center / total_verts;
            for (int i = 0; i < total_verts; i++)
            {
                X[i] -= center;
                float temp = X[i].y;
                X[i].y = X[i].z;
                X[i].z = temp;   // xzy to xyz coord
            }
        }

        //Create triangle mesh.
        vertices = new Vector3[tet_number * 12];   //as a global one
        int vertex_number = 0;
        for (int tet = 0; tet < tet_number; tet++)   // 3 * 4
        {
            vertices[vertex_number++] = X[Tet[tet * 4 + 0]]; // follow counter clock-wise order
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];

            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];

            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];

            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];
        }

        int[] triangles = new int[tet_number * 12];
        for (int t = 0; t < tet_number * 4; t++)
        {
            triangles[t * 3 + 0] = t * 3 + 0;
            triangles[t * 3 + 1] = t * 3 + 1;
            triangles[t * 3 + 2] = t * 3 + 2;
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        inv_Dm = new Matrix4x4[tet_number];
        float min_v = float.MaxValue;
        for (int i = 0; i < tet_number; i++)
        {
            inv_Dm[i] = new Matrix4x4();
            inv_Dm[i] = Build_Edge_Matrix(i).inverse;
            if (inv_Dm[i].determinant < min_v)
                min_v = inv_Dm[i].determinant;
            if ((inv_Dm[i].determinant - 0f) < 1e-1f) print("Error");
        }
        print("Minimum Determinant Value: (GPU Version)" + min_v);
    }

    Matrix4x4 Build_Edge_Matrix(int tet)
    {
        Matrix4x4 ret =  Matrix4x4.zero;
        //TODO: Need to build edge matrix here.
        // [x10, x20, x30]
        Vector3 X0 = X[Tet[4 * tet]];
        Vector3 X1 = X[Tet[4 * tet + 1]];
        Vector3 X2 = X[Tet[4 * tet + 2]];
        Vector3 X3 = X[Tet[4 * tet + 3]];
        ret.SetColumn(0, X1 - X0);
        ret.SetColumn(1, X2 - X0);
        ret.SetColumn(2, X3 - X0);

        ret[3, 3] = 1.0f;  //homogeneous matrix
        return ret;
    }
    #endregion
    #region Kernals and Buffer Initialization
    public void GPUPartInitialization()
    {
        //Find all the Kernels
        //Build_InvDm = SolverShader.FindKernel("Build_InvDm");
        Laplacian_Smoothing_v1 = SolverShader.FindKernel("Laplacian_Smoothing_v1");
        Laplacian_Smoothing_v2 = SolverShader.FindKernel("Laplacian_Smoothing_v2");    //v2 - interlocked adding 
        Laplacian_Smoothing_v3 = SolverShader.FindKernel("Laplacian_Smoothing_v3");     //v3 - value beck to the v_sum  
        Laplacian_Smoothing_v4 = SolverShader.FindKernel("Laplacian_Smoothing_v4");     //v4 - perform the blending
        Jump_up = SolverShader.FindKernel("Jump_Up");   //if key down, X - values up
        Force_Init = SolverShader.FindKernel("Force_Init");
        Force_Comp = SolverShader.FindKernel("Force_Compute");
        Force_Final = SolverShader.FindKernel("Force_Final"); //final addition of force
        VX_Update = SolverShader.FindKernel("Update_Simulation");   //include collision detection already
        init_invDm = SolverShader.FindKernel("Init_InvDm");

        //Pass all the Physical parameters into Compute Shader
        SolverShader.SetFloat("dt", dt);
        SolverShader.SetFloat("mass", mass);
        SolverShader.SetFloat("stiffness_0", stiffness_0);
        SolverShader.SetFloat("stiffness_1", stiffness_1);
        SolverShader.SetFloat("damp", damp);

        SolverShader.SetFloats("gravity", new float[] { gravity.x, gravity.y, gravity.z });
        //SolverShader.SetFloats("gravity", new float[] { 0f, -9.8f, 0f });
        SolverShader.SetFloats("N", new float[] { N.x, N.y, N.z });
        SolverShader.SetFloats("P", new float[] { P.x, P.y, P.z });

        SolverShader.SetFloat("u_T", u_T);
        SolverShader.SetFloat("u_N", u_N);
        SolverShader.SetFloat("blend_a", blend_a);

        SolverShader.SetInt("tet_number", tet_number);
        SolverShader.SetInt("total_verts", total_verts);

        //utilized for allocate the intiial values of compute buffers
        Vector3[] V = new Vector3[total_verts];
        Force = new Vector3[total_verts];
        Vector3[] V_sum = new Vector3[total_verts];
        int3[] F_delta = new int3[total_verts];
        int3[] V_delta = new int3[total_verts];
        int [] V_num = new int[total_verts];   //for average values
        InitGPUBuffers();

        for(int i=0; i<total_verts; i++)
        {
            V[i] = Vector3.zero;
            Force[i] = Vector3.zero;
            V_sum[i] = Vector3.zero;
            V_num[i] = 0;
            F_delta[i] = new int3(0, 0, 0);
            V_delta[i] = new int3(0, 0, 0);
        }
        //since inv_Dm is computed in CPU part, and we need to pass it into the compute shader

      
        Force_com.SetData(Force);
        Force_delta.SetData(F_delta);
        V_sum_com.SetData(V_sum);
        V_sum_delta.SetData(V_delta);
        V_num_com.SetData(V_num);
        Tet_com.SetData(Tet);
        X_com.SetData(X);
        V_com.SetData(V);
        inv_Dm_com.SetData(inv_Dm);

        //lastly setup the thread group size
        uint threadnum = 0;
        SolverShader.GetKernelThreadGroupSizes(Force_Init, out threadnum, out _, out _);
        threadGroups_X = (int)((total_verts + (threadnum - 1)) / threadnum);
        SolverShader.GetKernelThreadGroupSizes(Force_Comp, out threadnum, out _, out _);
        threadGroups_Tet = (int)((tet_number + (threadnum - 1)) / threadnum);

        print("Tet_Number: " + tet_number);
    }

    void InitGPUBuffers()
    {
        Force_com = new ComputeBuffer(total_verts, sizeof(float) *3);
        Force_delta = new ComputeBuffer(total_verts, sizeof(int) * 3);
        inv_Dm_com = new ComputeBuffer(tet_number, sizeof(float) * 16);
        V_sum_com = new ComputeBuffer(total_verts, sizeof(float) * 3);
        V_sum_delta= new ComputeBuffer(total_verts, sizeof(int) * 3);
        V_num_com = new ComputeBuffer(total_verts, sizeof(int));
        Tet_com = new ComputeBuffer(tet_number * 4, sizeof(int));
        X_com = new ComputeBuffer(total_verts, sizeof(float) * 3);
        V_com = new ComputeBuffer(total_verts, sizeof(float) * 3);
        
    }

    void SetGPUBufferToKernels()
    {
        //1.build_invDm in CPU, which is already obtained
        
        //2. Laplacian_v1, v2, v3 ,v4
        SolverShader.SetBuffer(Laplacian_Smoothing_v1, "V_sum", V_sum_com);
        SolverShader.SetBuffer(Laplacian_Smoothing_v1, "V_num", V_num_com);
        SolverShader.SetBuffer(Laplacian_Smoothing_v1, "V_Sumdelta", V_sum_delta);
        SolverShader.SetBuffer(Laplacian_Smoothing_v1, "Force_delta", Force_delta);

        SolverShader.SetBuffer(Laplacian_Smoothing_v2, "V", V_com);
        SolverShader.SetBuffer(Laplacian_Smoothing_v2, "V_Sumdelta", V_sum_delta);
        SolverShader.SetBuffer(Laplacian_Smoothing_v2, "V_num", V_num_com);
        SolverShader.SetBuffer(Laplacian_Smoothing_v2, "Tet", Tet_com);

        SolverShader.SetBuffer(Laplacian_Smoothing_v3, "V_sum", V_sum_com);
        SolverShader.SetBuffer(Laplacian_Smoothing_v3, "V_Sumdelta", V_sum_delta);

        SolverShader.SetBuffer(Laplacian_Smoothing_v4, "V", V_com);
        SolverShader.SetBuffer(Laplacian_Smoothing_v4, "V_sum", V_sum_com);
        SolverShader.SetBuffer(Laplacian_Smoothing_v4, "V_num", V_num_com);

        //3. Force Comp
        SolverShader.SetBuffer(Force_Init, "Force", Force_com);

        SolverShader.SetBuffer(Force_Comp, "Tet", Tet_com);
        SolverShader.SetBuffer(Force_Comp, "inv_Dm", inv_Dm_com);
        SolverShader.SetBuffer(Force_Comp, "Force_delta", Force_delta);
        SolverShader.SetBuffer(Force_Comp, "X", X_com);

        SolverShader.SetBuffer(Force_Final, "Force", Force_com);
        SolverShader.SetBuffer(Force_Final, "Force_delta", Force_delta);

        //4. Update_Simulation
        SolverShader.SetBuffer(VX_Update, "V", V_com);
        SolverShader.SetBuffer(VX_Update, "X", X_com);
        SolverShader.SetBuffer(VX_Update, "Force", Force_com);

        //jump up 
        SolverShader.SetBuffer(Jump_up, "V", V_com);

        //Init Inv_Dm, useless, could be ignored
        SolverShader.SetBuffer(init_invDm, "inv_Dm", inv_Dm_com);
        SolverShader.Dispatch(init_invDm, threadGroups_Tet, 1, 1);
    }
    #endregion



    #region Simulation part in CPU
    void SimulateOneStep()
    {
        //#region Build inv_DM
        //SolverShader.Dispatch(Build_InvDm, threadGroups_Tet, 1, 1);
        //#endregion

        #region Laplacian Smoothing
        SolverShader.Dispatch(Laplacian_Smoothing_v1, threadGroups_X, 1, 1);
        SolverShader.Dispatch(Laplacian_Smoothing_v2, threadGroups_Tet, 1, 1);
        SolverShader.Dispatch(Laplacian_Smoothing_v3, threadGroups_X, 1, 1);
        SolverShader.Dispatch(Laplacian_Smoothing_v4, threadGroups_X, 1, 1);
        #endregion
        
        //jump up, kernel SetUp
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SolverShader.Dispatch(Jump_up, threadGroups_X, 1, 1);
        }

        #region Force_Comp
        SolverShader.Dispatch(Force_Init, threadGroups_X, 1, 1);
        SolverShader.Dispatch(Force_Comp, threadGroups_Tet, 1, 1);
        SolverShader.Dispatch(Force_Final, threadGroups_X, 1, 1);
        Force_com.GetData(Force);
        //print("Force: " + Force[0]);  //debug only
           
        #endregion

        #region UpdateSimulation
        SolverShader.Dispatch(VX_Update, threadGroups_X, 1, 1);
        #endregion
    }
    void UpdateDataToMesh()
    {
        X_com.GetData(X);
        int vertex_number = 0;
        for (int tet = 0; tet < tet_number; tet++)
        {
            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];

            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];

            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];

            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.RecalculateNormals();

    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        InitModelStructure();
        GPUPartInitialization();
        SetGPUBufferToKernels();

        startSimulationOnPlay = true;   //initialization is finished and let's go
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= deltaTimeStep) timePassed = 0.0f;
        if (startSimulationOnPlay && timePassed == 0.0f)
        {
            for (int i = 0; i < iterationSteps; i++)
                SimulateOneStep();
            UpdateDataToMesh();

            /*inv_Dm_com.GetData(inv_Dm);
            float min_v = float.MaxValue;
            for(int i=0; i<tet_number; i++)
            {
                if (inv_Dm[i].determinant < min_v)
                    min_v = inv_Dm[i].determinant;
                if ((inv_Dm[i].determinant-0.0f)<1e-1f)
                {
                    print("Wrong inv_Dm: " + i);
                }
            }

            print("Minimum Determinant Value: " + min_v);
            */
        }
    }

    private void OnDestroy()
    {
        //dispose compute buffers
        if (Force_com != null) Force_com.Dispose();
        if (X_com != null) X_com.Dispose();
        if (V_com != null) V_com.Dispose();
        if (V_sum_com != null) V_sum_com.Dispose();
        if (V_sum_delta != null) V_sum_delta.Dispose();
        if (Force_delta != null) Force_delta.Dispose();
        if (inv_Dm_com != null) inv_Dm_com.Dispose();
        if (V_num_com != null) V_num_com.Dispose();
        if (Tet_com != null) Tet_com.Dispose(); 
    }
}
