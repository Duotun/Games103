using UnityEngine;
using System.Collections;

public class Rigid_Bunny : MonoBehaviour 
{
	bool launched 		= false;
	float dt 			= 0.015f;   // could be different from the fps
	Vector3 v 			= new Vector3(0, 0, 0);	// velocity
	Vector3 w 			= new Vector3(0, 0, 0);	// angular velocity
	
	float mass;									// mass
	Matrix4x4 I_ref;							// reference inertia

	float linear_decay	= 0.95f;				// for velocity decay
	float angular_decay	= 0.98f;				
	public float uN 	= 0.002f;					// for collision impulse, uN
	public float uT = 0.3f;   // u_T, for collision impulse 

	Vector3 gravity = new Vector3(0f, -9.8f, 0f);

	Quaternion Quat_Add(Quaternion a, Quaternion b)
	{
		Quaternion res = Quaternion.identity;
		//component-wise addition
		res.x = a.x + b.x;
		res.y = a.y + b.y;
		res.z = a.z + b.z;
		res.w = a.w + b.w;

		return res;
	}

	// for later to use matrix4X4.multiplyVector
	Matrix4x4 CrossDotMat(Vector3 r)
	{
		Matrix4x4 res = Matrix4x4.zero;
		res[0, 1] = -r.z;
		res[0, 2] = r.y;
		res[1, 0] = r.z;
		res[1, 2] = -r.x;
		res[2, 0] = -r.y;
		res[2, 1] = r.x;
		res[3,3] = 1.0f;
		return res;
	}

	Matrix4x4 Mat_Add(Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 res = Matrix4x4.zero;
		res.m00 = a.m00 + b.m00; res.m10 = a.m10 + b.m10;
		res.m01 = a.m01 + b.m01; res.m11 = a.m11 + b.m11;
		res.m02 = a.m02 + b.m02; res.m12 = a.m12 + b.m12;
		res.m03 = a.m03 + b.m03; res.m13 = a.m13 + b.m13;

		res.m20 = a.m20 + b.m20; res.m30 = a.m30 + b.m30;
		res.m21 = a.m21 + b.m21; res.m31 = a.m31 + b.m31;
		res.m22 = a.m22 + b.m22; res.m32 = a.m32 + b.m32;
		res.m23 = a.m23 + b.m23; res.m33 = a.m33 + b.m33;

		return res;
	}

	Matrix4x4 Mat_Scale(Matrix4x4 a, float s)
	{
		Matrix4x4 res = Matrix4x4.zero;
		for(int i=0; i< 4; i++)
		{
			for(int j=0; j<4; j++)
			     res[i, j] = a[i, j] * s;
		}

		return res;
	}
	// Use this for initialization
	void Start () 
	{		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;

		float m=1;
		mass=0;
		for (int i=0; i<vertices.Length; i++) 
		{
			mass += m;
			float diag=m*vertices[i].sqrMagnitude; // just for
			I_ref[0, 0]+=diag;
			I_ref[1, 1]+=diag;
			I_ref[2, 2]+=diag;
			I_ref[0, 0]-=m*vertices[i][0]*vertices[i][0];    //for the x, y, z
			I_ref[0, 1]-=m*vertices[i][0]*vertices[i][1];
			I_ref[0, 2]-=m*vertices[i][0]*vertices[i][2];
			I_ref[1, 0]-=m*vertices[i][1]*vertices[i][0];
			I_ref[1, 1]-=m*vertices[i][1]*vertices[i][1];
			I_ref[1, 2]-=m*vertices[i][1]*vertices[i][2];
			I_ref[2, 0]-=m*vertices[i][2]*vertices[i][0];
			I_ref[2, 1]-=m*vertices[i][2]*vertices[i][1];
			I_ref[2, 2]-=m*vertices[i][2]*vertices[i][2];
		}
		I_ref [3, 3] = 1;
	}
	
	Matrix4x4 Get_Cross_Matrix(Vector3 a)
	{
		//Get the cross product matrix of vector a
		Matrix4x4 A = Matrix4x4.zero;
		A [0, 0] = 0; 
		A [0, 1] = -a [2]; 
		A [0, 2] = a [1]; 
		A [1, 0] = a [2]; 
		A [1, 1] = 0; 
		A [1, 2] = -a [0]; 
		A [2, 0] = -a [1]; 
		A [2, 1] = a [0]; 
		A [2, 2] = 0; 
		A [3, 3] = 1;
		return A;
	}

	// In this function, update v and w by the impulse due to the collision with
	//a plane <P, N>
	void Collision_Impulse(Vector3 P, Vector3 N)
	{
			//only consider the simple two planes here?
			//and utilize the signed distance function
			
			//1. for every vertex, update the position according to the quaternion
			//test with each vertex by x_i = x_0 + R * vertice_pos

			Matrix4x4 R = Matrix4x4.Rotate(transform.rotation);  
			Vector3 x = transform.position;
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			Vector3 v_avg = Vector3.zero;
			Vector3 Rr_avg = Vector3.zero;
			int cnt = 0;  //for the average

			float phi;  //for the sdf checking - collision impulse
			for(int i=0; i<vertices.Length; i++)
			{
				Vector4 Rr_i = R * new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, 1f);   //rotation multiplication
				Vector3 x_i = x + (Vector3)Rr_i;
				phi = Vector3.Dot((x_i - P), N); 
				if(phi <0)
				{
					 //print(phi);
					 //check v_i, whether the penetration directions
					 //Vector3 dv_i = CrossDotMat(w) * Rr_i;
					 //Vector3 v_i = v + dv_i;
					 Vector3 v_i = v + (Vector3) (CrossDotMat(w) * Rr_i);   //general mutliplication
					 //print(Vector3.Dot(v_i, N));
					 if(Vector3.Dot(v_i, N)< 0)
					 {
						 //accumulate first
						 v_avg += v_i;
						 Rr_avg += (Vector3)Rr_i;
						 cnt++;
					 }
				}
			}

			
			//2. if accmulated successfully, compute the impulse j
			if(cnt > 0)
			{
			  //print(cnt);
			 // obtain the average
			  v_avg /= cnt;
			  Rr_avg /= cnt;
			  Vector3 v_N =  Vector3.Dot(v_avg, N) * N;
			  Vector3 v_T = v_avg - v_N;
			
			  float alpha = Mathf.Max(0f, 1 - uT * (1 + uN)*v_N.magnitude / v_T.magnitude);
			  Vector3 v_N_new =  -uN * v_N;
			  Vector3 v_T_new = alpha * v_T;
			  Vector3 v_new = v_N_new + v_T_new;

			  //use v_new to obtain J
			  Matrix4x4 Rrr_i = CrossDotMat(Rr_avg);
			  Matrix4x4 K = Mat_Add(Mat_Scale(Matrix4x4.identity, 1.0f/mass), Mat_Scale(Rrr_i * I_ref.inverse * Rrr_i, -1f));
			  //Vector3 J =  K.inverse.MultiplyPoint((v_new - v_avg));  //general mutliplication
			  Vector4 v_tp = new Vector4((v_new - v_avg).x, (v_new - v_avg).y, (v_new - v_avg).z, 1.0f);
			  Vector4 J = K.inverse * v_tp;
			  //update v and w using J
			  v = v + (Vector3) J *(1.0f/mass);
			  //w = w + I_ref.inverse.MultiplyPoint((Rrr_i.MultiplyPoint(J)));
			  Vector3 dw = I_ref.inverse * (Rrr_i * J);
			  w = w + dw;
			}

			cnt = 0;
	}

	// Update is called once per frame
	void Update () 
	{
		//Game Control
		if(Input.GetKey("r"))
		{
			transform.position = new Vector3 (0, 0.6f, 0);
			uN = 0.2f;
			uT = 0.2f;
			launched=false;

			v = Vector3.zero;
			w = Vector3.zero;
			transform.rotation = Quaternion.identity;    //restore to the default states
		}

		if(Input.GetKey("l"))
		{
			v = new Vector3 (5, 2, 0);
			w = new Vector3(5, 5, 0);
			launched=true;
		}

		if(!launched) return;
		// Part I: Update velocities to v_0.5 and w_0.5
		// gravity could be considered here for v, gravity has no effect for the w
		// use leapfrog here,  0.5 * delta_t
		v += dt * 0.5f * gravity;

		v*= linear_decay;
		w*= angular_decay;
		// Part II: Collision Impulse , force and velocity
		Collision_Impulse(new Vector3(0, 0.01f, 0), new Vector3(0, 1, 0));
		Collision_Impulse(new Vector3(2, 0, 0), new Vector3(-1, 0, 0));

		// Part III: Update position & orientation
		//Update linear status
		Vector3 x = transform.position;
		//update position according to the velocity
		x += v * dt;
		//Update angular status
		Quaternion q = transform.rotation;
		q = Quat_Add(q, new Quaternion(dt*w.x * 0.5f, dt * w.y * 0.5f, dt * w.z * 0.5f, 0) * q);   
		//q.Normalize();
		// Part IV: Assign to the object
		transform.position = x;
		transform.rotation = q;
		//Then update the velocity and angular velocity to the v_1 and w_1
		//since we are using the leap frog method
		v += dt * 0.5f * gravity;
		
	}
}
