using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FVM : MonoBehaviour
{
	float dt 			= 0.003f;
    float mass 			= 1;
	float stiffness_0	= 20000.0f;
    float stiffness_1 	= 5000.0f;
    float damp			= 0.999f;

	int[] 		Tet;
	int tet_number;			//The number of tetrahedra

	Vector3[] 	Force;
	Vector3[] 	V;
	Vector3[] 	X;
	int number;				//The number of vertices

	Matrix4x4[] inv_Dm;

	//For Laplacian smoothing.
	Vector3[]   V_sum;
	int[]		V_num;

	SVD svd = new SVD();
    Vector3 gravity;
    Vector3 N;
    Vector3 P;

    public float u_T = 0.5f;   //for the collision impulse
    public float u_N = 0.1f;
    public float blend_a = 0.3f;
    //define the plane for simple collision detection
    // Start is called before the first frame update
    void Start()
    {
        //setup gravity parameter
        gravity = new Vector3(0f, -9.8f, 0f);
        N = new Vector3(0, 1f, 0);   //N for the Plane, collision detection
        P = new Vector3(0, -2.999f, 0);
        // FILO IO: Read the house model from files.
    	// The model is from Jonathan Schewchuk's Stellar lib.
    	{
    		string fileContent = File.ReadAllText("Assets/hw3/house2.ele");
    		string[] Strings = fileContent.Split(new char[]{' ', '\t', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
    		
    		tet_number=int.Parse(Strings[0]);
        	Tet = new int[tet_number*4];

    		for(int tet=0; tet<tet_number; tet++)
    		{
				Tet[tet*4+0]=int.Parse(Strings[tet*5+4])-1;
				Tet[tet*4+1]=int.Parse(Strings[tet*5+5])-1;
				Tet[tet*4+2]=int.Parse(Strings[tet*5+6])-1;
				Tet[tet*4+3]=int.Parse(Strings[tet*5+7])-1;
			}
    	}
    	{
			string fileContent = File.ReadAllText("Assets/hw3/house2.node");
    		string[] Strings = fileContent.Split(new char[]{' ', '\t', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
    		number = int.Parse(Strings[0]);
    		X = new Vector3[number];
       		for(int i=0; i<number; i++)
       		{
       			X[i].x=float.Parse(Strings[i*5+5])*0.4f;
       			X[i].y=float.Parse(Strings[i*5+6])*0.4f;
       			X[i].z=float.Parse(Strings[i*5+7])*0.4f;
       		}
    		//Centralize the model.
	    	Vector3 center=Vector3.zero;
	    	for(int i=0; i<number; i++)		center+=X[i];
	    	center=center/number;
	    	for(int i=0; i<number; i++)
	    	{
	    		X[i]-=center;
	    		float temp=X[i].y;
	    		X[i].y=X[i].z;
	    		X[i].z=temp;   // xzy to xyz coord
	    	}
		}

        //code for the single test showcase
        /*tet_number=1;
        Tet = new int[tet_number*4];
        Tet[0]=0;
        Tet[1]=1;
        Tet[2]=2;
        Tet[3]=3;

        number=4;
        X = new Vector3[number];
        V = new Vector3[number];
        Force = new Vector3[number];
        X[0]= new Vector3(0, 0, 0);
        X[1]= new Vector3(1, 0, 0);
        X[2]= new Vector3(0, 1, 0);
        X[3]= new Vector3(0, 0, 1);*/


        //Create triangle mesh.
       	Vector3[] vertices = new Vector3[tet_number*12];
        int vertex_number=0;
        for(int tet=0; tet<tet_number; tet++)   // 3 * 4
        {
        	vertices[vertex_number++]=X[Tet[tet*4+0]]; // follow counter clock-wise order
            vertices[vertex_number++]=X[Tet[tet*4+2]];
        	vertices[vertex_number++]=X[Tet[tet*4+1]];

        	vertices[vertex_number++]=X[Tet[tet*4+0]];
        	vertices[vertex_number++]=X[Tet[tet*4+3]];
        	vertices[vertex_number++]=X[Tet[tet*4+2]];

        	vertices[vertex_number++]=X[Tet[tet*4+0]];
        	vertices[vertex_number++]=X[Tet[tet*4+1]];
        	vertices[vertex_number++]=X[Tet[tet*4+3]];

        	vertices[vertex_number++]=X[Tet[tet*4+1]];
        	vertices[vertex_number++]=X[Tet[tet*4+2]];
        	vertices[vertex_number++]=X[Tet[tet*4+3]];
        }

        int[] triangles = new int[tet_number*12];
        for(int t=0; t<tet_number*4; t++)
        {
        	triangles[t*3+0]=t*3+0;    
        	triangles[t*3+1]=t*3+1;
        	triangles[t*3+2]=t*3+2;
        }
        Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.vertices  = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();

		V 	  = new Vector3[number];
        Force = new Vector3[number];
        V_sum = new Vector3[number];
        V_num = new int[number];   //for average values?

        // TODO: Need to allocate and assign inv_Dm
        // build edge matrix for each tetrahedron
        inv_Dm = new Matrix4x4[tet_number];
        for(int i=0; i<tet_number;i++)
        {
            inv_Dm[i] = Build_Edge_Matrix(i).inverse;
        }
        
    }

    Matrix4x4 Build_Edge_Matrix(int tet)
    {
    	Matrix4x4 ret=Matrix4x4.zero;
        //TODO: Need to build edge matrix here.
        // [x10, x20, x30]
        Vector3 X0 = X[Tet[4*tet]];
        Vector3 X1 = X[Tet[4*tet+1]];
        Vector3 X2 = X[Tet[4*tet+2]];
        Vector3 X3 = X[Tet[4*tet+3]];
        ret.SetColumn(0, X1 - X0);
        ret.SetColumn(1, X2 - X0);
        ret.SetColumn(2, X3 - X0);

        ret[3, 3] = 1.0f;  //homogeneous matrix
		return ret;
    }


    void _Update()
    {
        Matrix4x4 U_m = Matrix4x4.zero, A_m= Matrix4x4.zero, V_m = Matrix4x4.zero;  //for SVD
        // begin laplacian smoothing before simulation
        for(int i=0; i<number;i++)
        {
            V_sum[i] = Vector3.zero; V_num[i] = 0;
        }
        for(int i=0; i<tet_number; i++)  //only adding neighboring velocities
        {
            int p0 = Tet[4 * i + 0], p1 = Tet[4 * i + 1];
            int p2 = Tet[4 * i + 2], p3 = Tet[4 * i + 3];
            V_sum[p3] += V[p0] + V[p1] + V[p2];
            V_num[p3] += 3;
            V_sum[p2] += V[p0] + V[p1] + V[p3];
            V_num[p2] += 3;
            V_sum[p1] += V[p0] + V[p2] + V[p3];
            V_num[p1] += 3;
            V_sum[p0] += V[p3] + V[p1] + V[p2];
            V_num[p0] += 3;
        }
        //blending into V[i]
        for(int i=0; i< number; i++)
        {
            V[i] = blend_a * V[i] + (1 - blend_a) * V_sum[i]/V_num[i];
        }
        
    	// Jump up.
		if(Input.GetKeyDown(KeyCode.Space))
    	{
    		for(int i=0; i<number; i++)
    			V[i].y+=0.2f;
    	}

    	for(int i=0 ;i<number; i++)
    	{
            //TODO: Add gravity to Force.
            Force[i] = gravity;
    	}

    	for(int tet=0; tet<tet_number; tet++)
    	{
            int p0 = Tet[tet * 4];
            int p1 = Tet[tet * 4+1];
            int p2 = Tet[tet * 4+2];
            int p3 = Tet[tet * 4+3];
            //TODO: Deformation Gradient
            Matrix4x4 F = Build_Edge_Matrix(tet);
            F = F * inv_Dm[tet];
            F[3, 3] = 1f;

            /*
            //TODO: Green Strain
            Matrix4x4 G;   //although using 4x4, computation actually uses 3x3
            G = Matrix_Add(F.transpose * F, Matrix_Mul_Float(Matrix4x4.identity, -1f));
            G = Matrix_Mul_Float(G, 0.5f);
            G[3, 3] = 1.0f;
            //TODO: Second PK Stress
            Matrix4x4 S;
            S = Matrix_Mul_Float(G, 2.0f * stiffness_1);
            S = Matrix_Add(S, Matrix_Mul_Float(Matrix4x4.identity, stiffness_0 * Matrix_Trace(G)));
            S[3, 3] = 1f;
            //TODO: Elastic Force
            //Obtain First PK Stress from S
            
            Matrix4x4 P_m = F * S;
            */

            // SVD method to obtain First PK Stress
            svd.svd(F, ref U_m, ref A_m, ref V_m);
            Matrix4x4 D_m = Matrix4x4.zero;
            float I_c = A_m[0, 0] * A_m[0, 0] +
                A_m[1, 1] * A_m[1, 1] +
                A_m[2, 2] * A_m[2, 2];
            float dEdI = stiffness_0 * (I_c - 3f) * 0.25f - stiffness_1 * 0.5f;
            float dEdII = stiffness_1 * 0.25f;
            D_m[0, 0] = 2f * dEdI * A_m[0, 0] + 4f * dEdII * A_m[0, 0] * A_m[0, 0] * A_m[0, 0];
            D_m[1, 1] = 2f * dEdI * A_m[1, 1] + 4f * dEdII * A_m[1, 1] * A_m[1, 1] * A_m[1, 1];
            D_m[2, 2] = 2f * dEdI * A_m[2, 2] + 4f * dEdII * A_m[2, 2] * A_m[2, 2] * A_m[2, 2];
            D_m[3, 3] = 1f;
            Matrix4x4 P_m = U_m * D_m * V_m.transpose;
            P_m[3, 3] = 1.0f;
            Matrix4x4 F_3 = Matrix_Mul_Float(P_m * inv_Dm[tet].transpose, -1.0f * 1f / 6f / inv_Dm[tet].determinant);
            
            //print("A_m: ?" + A_m);
            Force[p1] += (Vector3)(F_3.GetColumn(0));
            Force[p2] += (Vector3)(F_3.GetColumn(1));
            Force[p3] += (Vector3)(F_3.GetColumn(2));
            Force[p0] -= (Vector3)(F_3.GetColumn(0));
            Force[p0] -= (Vector3)(F_3.GetColumn(1));
            Force[p0] -= (Vector3)(F_3.GetColumn(2));
        }

    	for(int i=0; i<number; i++)
    	{
            //TODO: Update X and V here.
            //explicit integration
            V[i] += (Force[i]/mass) * dt;  //simulation of acceleration
            V[i] *= damp;
            X[i] += V[i] * dt;

            //TODO: (Particle) collision with floor.
            //impulse-based method, define the N of plane (0, 1, 0)

            var phi = Vector3.Dot(X[i] - P, N);
            if (phi >= 0 || Vector3.Dot(V[i],N)>=0) continue;
            Vector3 V_Ni = Vector3.Dot(V[i], N) * N;
            Vector3 V_Ti = V[i] - V_Ni;
            float alpha = Mathf.Max(0, 1f - u_T * (1 + u_N) * V_Ni.magnitude / V_Ti.magnitude);

            //update to new and I don't want to declare new stuff
            V_Ni = -u_N * V_Ni;
            V_Ti = alpha * V_Ti;
            V[i] = V_Ni + V_Ti;

            //also Update the Position Here
            X[i] = X[i] + N * Mathf.Abs(phi);
        }
    }

    // Update is called once per frame
    void Update()
    {
    	for(int l=0; l<15; l++)  //10 iterations 
    		 _Update();

    	// Dump the vertex array for rendering.
    	Vector3[] vertices = new Vector3[tet_number*12];
        int vertex_number=0;   //build triangle mesh fro tetrahedron
        for(int tet=0; tet<tet_number; tet++)
        {
        	vertices[vertex_number++]=X[Tet[tet*4+0]];
        	vertices[vertex_number++]=X[Tet[tet*4+2]];
        	vertices[vertex_number++]=X[Tet[tet*4+1]];

        	vertices[vertex_number++]=X[Tet[tet*4+0]];
        	vertices[vertex_number++]=X[Tet[tet*4+3]];
        	vertices[vertex_number++]=X[Tet[tet*4+2]];

        	vertices[vertex_number++]=X[Tet[tet*4+0]];
        	vertices[vertex_number++]=X[Tet[tet*4+1]];
        	vertices[vertex_number++]=X[Tet[tet*4+3]];

        	vertices[vertex_number++]=X[Tet[tet*4+1]];
        	vertices[vertex_number++]=X[Tet[tet*4+2]];
        	vertices[vertex_number++]=X[Tet[tet*4+3]];
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.vertices  = vertices;
		mesh.RecalculateNormals ();
    }

    Matrix4x4 Matrix_Mul_Float(Matrix4x4 a, float c)
    {
        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                a[i, j] *= c;
            }
        }
        return a;
    }

    Matrix4x4 Matrix_Add(Matrix4x4 a, Matrix4x4 b)
    {
        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                a[i, j] += b[i, j];
            }
        }
        return a;
    }

    float Matrix_Trace(Matrix4x4 a)
    {
        float res = 0f;
        for(int i=0; i<3; i++)   // no need to consider the last one, since 3x3
        {
            res += a[i, i];
        }
        return res;
    }
}
