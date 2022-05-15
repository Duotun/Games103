using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave_motionGPU : MonoBehaviour
{
    //1.0 - GPU Accelerated without CG Part
    #region Simulation Parameters
    [SerializeField]
    private int size = 100;
    private float rate = 0.005f;
    private float gamma = 0.002f;
    private float damping = 0.99f;

    // ADD SIMULTION STEP Control
    [SerializeField]
    private uint iterationSteps = 8;
    [SerializeField]
    private float deltaTimeStep = 0.03f;
    [SerializeField]
    private bool startSimulationOnPlay = false;
    private float timePassed = 0f;
    #endregion


    #region Two Cubes and Compute Shader
    public ComputeShader SolverShader;

    public Transform block_1;
    Bounds block_1_bounds;
    public Transform block_2;
    Bounds block_2_bounds;
    #endregion


    #region DataInCPU
    Mesh flowmesh;
    float[,] new_h;
    float[,] low_h;
    //float[,] old_h;
    float[,] vh;  //virtual height for coupling
    float[,] b;

    bool[,] cg_mask;
    float[,] cg_p;
    float[,] cg_r;
    float[,] cg_Ap;   //we could free those cpu parts after initialization of compute buffer

    float[,] h;   // for Update to Mesh
    //for Update pos of grid water
    Vector3[] pos;
    #endregion

    #region  Kernels
    int kernel_HeightLoad;
    int kernel_ComputeNewH;
    int kernel_UpdateNewH;   //add to the final G
    int kernel_AssignToH;
    int kernel_HeightToMesh;

    //for threading groups
    int threadGroups;
    #endregion

    #region Compute Buffers
    ComputeBuffer old_h_gpu;
    ComputeBuffer new_h_gpu;
    ComputeBuffer h_gpu;
    ComputeBuffer vh_gpu;
    //ComputeBuffer cg_p_gpu;
    //ComputeBuffer cg_r_gpu;
    //ComputeBuffer cg_Ap_gpu;
    ComputeBuffer X_gpu;
    #endregion
    //only h need to be update to the mesh in the end
    //Then for updating the position of meshes in the end, vector3,
    //actually, we are updating positions only for the CPU side in the end


    private void Awake()
    {
        startSimulationOnPlay = false;  // put it true after we prepare everything on GPU side
    }
    // Start is called before the first frame update
    void Start()
    {
        GridInit();
        InitKernels();
        SetGPUParameters();
        InitGPUBufferSize();
        SetGPUBuffer();

        startSimulationOnPlay = true;   //initialization is finished and let's go
    }


    #region Conjugate Gradient
    void A_Times(bool[,] mask, float[,] x, float[,] Ax, int li, int ui, int lj, int uj)
    {
        for (int i = li; i <= ui; i++)
            for (int j = lj; j <= uj; j++)
                if (i >= 0 && j >= 0 && i < size && j < size && mask[i, j])
                {
                    Ax[i, j] = 0;
                    if (i != 0) Ax[i, j] -= x[i - 1, j] - x[i, j];
                    if (i != size - 1) Ax[i, j] -= x[i + 1, j] - x[i, j];
                    if (j != 0) Ax[i, j] -= x[i, j - 1] - x[i, j];
                    if (j != size - 1) Ax[i, j] -= x[i, j + 1] - x[i, j];
                }
    }

    float Dot(bool[,] mask, float[,] x, float[,] y, int li, int ui, int lj, int uj)
    {
        float ret = 0;
        for (int i = li; i <= ui; i++)
            for (int j = lj; j <= uj; j++)
                if (i >= 0 && j >= 0 && i < size && j < size && mask[i, j])
                {
                    ret += x[i, j] * y[i, j];
                }
        return ret;
    }

    //well still ok to GPU but really complicated
    void Conjugate_Gradient(bool[,] mask, float[,] b, float[,] x, int li, int ui, int lj, int uj)
    {
        //Solve the Laplacian problem by CG.
        A_Times(mask, x, cg_r, li, ui, lj, uj);

        for (int i = li; i <= ui; i++)
            for (int j = lj; j <= uj; j++)
                if (i >= 0 && j >= 0 && i < size && j < size && mask[i, j])
                {
                    cg_p[i, j] = cg_r[i, j] = b[i, j] - cg_r[i, j];
                }

        float rk_norm = Dot(mask, cg_r, cg_r, li, ui, lj, uj);

        for (int k = 0; k < 128; k++)
        {
            if (rk_norm < 1e-10f) break;
            A_Times(mask, cg_p, cg_Ap, li, ui, lj, uj);
            float alpha = rk_norm / Dot(mask, cg_p, cg_Ap, li, ui, lj, uj);

            for (int i = li; i <= ui; i++)
                for (int j = lj; j <= uj; j++)
                    if (i >= 0 && j >= 0 && i < size && j < size && mask[i, j])
                    {
                        x[i, j] += alpha * cg_p[i, j];
                        cg_r[i, j] -= alpha * cg_Ap[i, j];
                    }

            float _rk_norm = Dot(mask, cg_r, cg_r, li, ui, lj, uj);
            float beta = _rk_norm / rk_norm;
            rk_norm = _rk_norm;

            for (int i = li; i <= ui; i++)
                for (int j = lj; j <= uj; j++)
                    if (i >= 0 && j >= 0 && i < size && j < size && mask[i, j])
                    {
                        cg_p[i, j] = cg_r[i, j] + beta * cg_p[i, j];
                    }
        }

    }
    #endregion

    #region Grid Init
    void GridInit()
    {
        flowmesh = GetComponent<MeshFilter>().mesh;
        flowmesh.Clear();   // we need to define our own grid
        flowmesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        pos = new Vector3[size * size];
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
            {
                pos[i * size + j].x = i * 0.1f - size * 0.05f;
                pos[i * size + j].y = 0;
                pos[i * size + j].z = j * 0.1f - size * 0.05f;
            }

        int[] T = new int[(size - 1) * (size - 1) * 6];   //just for triangles and don't need to consider afterwards
        int index = 0;
        for (int i = 0; i < size - 1; i++)
            for (int j = 0; j < size - 1; j++)   // clock-wise order
            {
                T[index * 6 + 0] = (i + 0) * size + (j + 0);
                T[index * 6 + 1] = (i + 0) * size + (j + 1);
                T[index * 6 + 2] = (i + 1) * size + (j + 1);
                T[index * 6 + 3] = (i + 0) * size + (j + 0);
                T[index * 6 + 4] = (i + 1) * size + (j + 1);
                T[index * 6 + 5] = (i + 1) * size + (j + 0);
                index++;
            }
        flowmesh.vertices = pos;
        flowmesh.triangles = T;
        flowmesh.RecalculateNormals();

        low_h = new float[size, size];
        new_h = new float[size, size];
        h = new float[size, size];
        vh = new float[size, size];
        b = new float[size, size];

        cg_mask = new bool[size, size];
        cg_p = new float[size, size];
        cg_r = new float[size, size];
        cg_Ap = new float[size, size];

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
            {
                low_h[i, j] = 99999;
                vh[i, j] = 0;
                b[i, j] = 0;
                cg_mask[i, j] = false;
            }
    }
    #endregion

    #region Kernel Init
    void InitKernels()
    {
        if (SolverShader == null) Debug.LogError("Please Assign Compute Shader First!");
        //Find Kernels
        kernel_HeightLoad = SolverShader.FindKernel("HeightLoad");
        kernel_ComputeNewH = SolverShader.FindKernel("ComputeNewH");
        kernel_UpdateNewH = SolverShader.FindKernel("UpdateNewH");
        kernel_AssignToH = SolverShader.FindKernel("AssignToH");
        kernel_HeightToMesh = SolverShader.FindKernel("HeightToMesh");

    }
    #endregion

    #region Compute Buffer Init
    void InitGPUBufferSize()
    {  
        //initial buffer with size and stride
        old_h_gpu = new ComputeBuffer(size * size, sizeof(float));
        new_h_gpu = new ComputeBuffer(size * size, sizeof(float));
        X_gpu = new ComputeBuffer(size * size, sizeof(float)*3);
        h_gpu = new ComputeBuffer(size * size, sizeof(float));
        vh_gpu = new ComputeBuffer(size * size, sizeof(float));
       
    }
    void SetGPUBuffer()
    {
        //bind buffer to the specific kernels
        SolverShader.SetBuffer(kernel_HeightLoad, "X", X_gpu);
        SolverShader.SetBuffer(kernel_HeightLoad, "h", h_gpu);

        SolverShader.SetBuffer(kernel_ComputeNewH, "h", h_gpu);
        SolverShader.SetBuffer(kernel_ComputeNewH, "new_h", new_h_gpu);
        SolverShader.SetBuffer(kernel_ComputeNewH, "old_h", old_h_gpu);

        SolverShader.SetBuffer(kernel_UpdateNewH, "vh", vh_gpu);
        SolverShader.SetBuffer(kernel_UpdateNewH, "new_h", new_h_gpu);

        SolverShader.SetBuffer(kernel_AssignToH, "old_h", old_h_gpu);
        SolverShader.SetBuffer(kernel_AssignToH, "h", h_gpu);
        SolverShader.SetBuffer(kernel_AssignToH, "new_h", new_h_gpu);

        SolverShader.SetBuffer(kernel_HeightToMesh, "X", X_gpu);
        SolverShader.SetBuffer(kernel_HeightToMesh, "h", h_gpu);

        // set up thread groups
        uint threadnum = 0;
        SolverShader.GetKernelThreadGroupSizes(kernel_HeightLoad, out threadnum, out _, out _);
        threadGroups = (int)((size + (threadnum - 1)) / threadnum);
        print("ThreadGroups: " + threadGroups);
        //set up the initial data
        old_h_gpu.SetData(vh);
    }

    //set GPU Constant Variables
    void SetGPUParameters()
    {
        //set up constants
        SolverShader.SetInt("size", size);
        SolverShader.SetFloat("rate", rate);
        SolverShader.SetFloat("gamma", gamma);
        SolverShader.SetFloat("damping", damping);

        print("Size Set Up Successfully: " + size);
    }
    #endregion

    #region Update Simulation
    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= deltaTimeStep) timePassed = 0.0f;
        if (startSimulationOnPlay && timePassed == 0.0f)
        {
            SimulationOneStep();
            UpdateToMesh();
        }
    }

    
    void ShadowWave_GPU()
    {
        //2.1 compute new heights
        SolverShader.Dispatch(kernel_ComputeNewH, threadGroups, threadGroups, 1);
        //2.2 Compute Conjugate Gradient cpu part now
        new_h_gpu.GetData(new_h);
        h_gpu.GetData(h);

        System.Array.Clear(cg_mask, 0, size * size);
        System.Array.Clear(b, 0, size * size);
        System.Array.Clear(vh, 0, size * size);
       
        //print("new_h: " + new_h[0,0]);
        //TODO: for block 1, calculate low_h.
        Vector3 block_pos = block_1.position;
        block_1_bounds = block_1.GetComponent<BoxCollider>().bounds;
        //check reduced grid for potential collision
        //make it back to index of size
        int li = (int)((block_pos.x + size * 0.05f - 1.0f) * 10f), ri = (int)((block_pos.x + size * 0.05f + 1.0f) * 10f);
        int lj = (int)((block_pos.z + size * 0.05f - 1.0f) * 10f), rj = (int)((block_pos.z + size * 0.05f + 1.0f) * 10f);    //from ray intersection back to the index
        li = Mathf.Clamp(li, 0, size - 1);
        ri = Mathf.Clamp(ri, 0, size - 1);
        lj = Mathf.Clamp(lj, 0, size - 1);
        rj = Mathf.Clamp(rj, 0, size - 1);

        
        float ei = 10f;
        // use intersect ray for determing the low_h
        //https://docs.unity3d.com/ScriptReference/Bounds.IntersectRay.html
        Vector3[] wave_x = this.GetComponent<MeshFilter>().mesh.vertices;
        Ray ray;
        for (int i = li; i <= ri; i++)
        {

            for (int j = lj; j <= rj; j++)
            {
                float dis = 1000f;
                Vector3 wave_discrete = wave_x[size * i + j];   //this could be a gpu method actually like ray tracing
                wave_discrete.y = -10f;   //test from the bottom
                ray = new Ray(wave_discrete, Vector3.up);
                block_1_bounds.IntersectRay(ray, out dis);
                low_h[i, j] = dis - ei;
                //TODO: then set up b and cg_mask for conjugate gradient.
                if (low_h[i, j] < h[i, j])   //if in contact, then cg_mask could be gpu accelerated then
                {
                    cg_mask[i, j] = true;
                    b[i, j] = (new_h[i, j] - low_h[i, j]) / rate;
                }
            }
        }

        //TODO: Solve the Poisson equation to obtain vh (virtual height).
        Conjugate_Gradient(cg_mask, b, vh, li, ri, lj, rj);  //region restricted
                                                             //TODO: for block 2, calculate low_h.
        block_pos = block_2.position;
        block_2_bounds = block_2.GetComponent<BoxCollider>().bounds;
        //check reduced grid for potential collision
        //make it back to index of size
        li = (int)((block_pos.x + size * 0.05f - 1.0f) * 10f); ri = (int)((block_pos.x + size * 0.05f + 1.0f) * 10f);
        lj = (int)((block_pos.z + size * 0.05f - 1.0f) * 10f); rj = (int)((block_pos.z + size * 0.05f + 1.0f) * 10f);
        li = Mathf.Clamp(li, 0, size - 1);
        ri = Mathf.Clamp(ri, 0, size - 1);
        lj = Mathf.Clamp(lj, 0, size - 1);
        rj = Mathf.Clamp(rj, 0, size - 1);

        ei = 10f;
        //TODO: then set up b and cg_mask for conjugate gradient.
        for (int i = li; i <= ri; i++)
        {

            for (int j = lj; j <= rj; j++)
            {
                float dis = 1000f;
                Vector3 wave_discrete = wave_x[size * i + j];
                wave_discrete.y = -10f;
                ray = new Ray(wave_discrete, Vector3.up);
                block_2_bounds.IntersectRay(ray, out dis);
                low_h[i, j] = dis - ei;
                //TODO: then set up b and cg_mask for conjugate gradient.
                if (low_h[i, j] < h[i, j])   //if in contact
                {
                    cg_mask[i, j] = true;
                    b[i, j] = (new_h[i, j] - low_h[i, j]) / rate;
                }
            }
        }
        //TODO: Solve the Poisson equation to obtain vh (virtual height).
        //with the two blocks considered
        Conjugate_Gradient(cg_mask, b, vh, li, ri, lj, rj);  //region restricted

        //3.Update new_H
        vh_gpu.SetData(vh);
        SolverShader.Dispatch(kernel_UpdateNewH, threadGroups, threadGroups, 1);
        //4. Update H in the end
        SolverShader.Dispatch(kernel_AssignToH, threadGroups, threadGroups, 1);

    }
    void SimulationOneStep()
    {
        //1. load Height
        pos = flowmesh.vertices;
        X_gpu.SetData(pos);
        SolverShader.Dispatch(kernel_HeightLoad, threadGroups, threadGroups, 1);

        /*if (Input.GetKeyDown("r")) // not supported in gpu
        {
            //TODO: Add random water.
            Random.InitState(Time.frameCount);  //use frame count as the seed of random generator
            int i = Random.Range(0, size), j = Random.Range(0, size);
            float r = Random.Range(0, 0.3f);
            h[i, j] += r;
            float cnt = 0f;
            if (i + 1 < size) cnt++;
            if (i - 1 >= 0) cnt++;
            if (j + 1 < size) cnt++;
            if (j - 1 >= 0) cnt++;

            if (i + 1 < size)
                h[i + 1, j] -= r / cnt;
            if (i - 1 >= 0)
                h[i - 1, j] -= r / cnt;
            if (j + 1 < size)
                h[i, j + 1] -= r / cnt;
            if (j - 1 >= 0)
                h[i, j - 1] -= r / cnt;

        }*/
        //h_gpu.SetData(h);
        //2. Perform Shadow Wave  - several substeps including conjugate _gradient performed on CPU Side
        for (int l = 0; l < iterationSteps; l++)
        {
            ShadowWave_GPU();
           
        }

    }

    void UpdateToMesh()
    {
        //TODO: Store h back into X.y and recalculate normal.
        SolverShader.Dispatch(kernel_HeightToMesh, threadGroups, threadGroups, 1);
        X_gpu.GetData(pos);
        flowmesh.vertices = pos;
        flowmesh.RecalculateNormals();
    }
    #endregion


    private void OnDestroy()
    {
        if (vh_gpu != null) vh_gpu.Dispose();
        if (h_gpu != null) h_gpu.Dispose();
        if (new_h_gpu != null) new_h_gpu.Dispose();
        if (X_gpu != null) X_gpu.Dispose();
        if (old_h_gpu != null) old_h_gpu.Dispose();
        
    }

}
