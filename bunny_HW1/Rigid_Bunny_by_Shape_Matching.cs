using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigid_Bunny_by_Shape_Matching : MonoBehaviour
{
	public bool launched = false;
	Vector3[] X;  //vertex of position
	Vector3[] Q;  //r, never change after the initialzation, we are updating the c and R then
	Vector3[] V;  // velocity
	Matrix4x4 QQt = Matrix4x4.zero;

	Vector3 g = new Vector3(0f, -9.8f, 0f);
	// still use collision impulse for simplicity

	Vector3[] P;
	Vector3[] N;     // two walls for penetration tests
	float decay = 0.95f;
	public float u_T = 0.5f;   //for the collision impulse
	public float u_N = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
    	Mesh mesh = GetComponent<MeshFilter>().mesh;
        V = new Vector3[mesh.vertices.Length];
        X = mesh.vertices;
        Q = mesh.vertices;

		P = new Vector3[2];
		P[0] = new Vector3(0, 0.001f, 0f); P[1]= new Vector3(2f, 0f, 0f);
		N = new Vector3[2];
		N[0]= new Vector3(0, 1f, 0); N[1] = new Vector3(-1f, 0f, 0f);

        //Centerizing Q.
        Vector3 c=Vector3.zero;
        for(int i=0; i<Q.Length; i++)
        	c+=Q[i];
        c/=Q.Length;
        for(int i=0; i<Q.Length; i++)
        	Q[i]-=c;

        //Get QQ^t ready.
		for(int i=0; i<Q.Length; i++)
		{
			QQt[0, 0]+=Q[i][0]*Q[i][0];
			QQt[0, 1]+=Q[i][0]*Q[i][1];
			QQt[0, 2]+=Q[i][0]*Q[i][2];
			QQt[1, 0]+=Q[i][1]*Q[i][0];
			QQt[1, 1]+=Q[i][1]*Q[i][1];
			QQt[1, 2]+=Q[i][1]*Q[i][2];
			QQt[2, 0]+=Q[i][2]*Q[i][0];
			QQt[2, 1]+=Q[i][2]*Q[i][1];
			QQt[2, 2]+=Q[i][2]*Q[i][2];
		}

		QQt[3, 3]=1;    //QQt = sum of r_i * r_i T

		for(int i=0; i<X.Length; i++)
			V[i][0]=4.0f;

		Update_Mesh(transform.position, Matrix4x4.Rotate(transform.rotation), 0);
		transform.position = Vector3.zero;
		transform.rotation=Quaternion.identity;   //don't apply further influence then
   	}

   	// Polar Decomposition that returns the rotation from F.
   	Matrix4x4 Get_Rotation(Matrix4x4 F)  //it seems according to the 3*3 to process this although using 4x4
	{
		Matrix4x4 C = Matrix4x4.zero;
	    for(int ii=0; ii<3; ii++)
	    for(int jj=0; jj<3; jj++)
	    for(int kk=0; kk<3; kk++)
	        C[ii,jj]+=F[kk,ii]*F[kk,jj];
	   
	   	Matrix4x4 C2 = Matrix4x4.zero;
		for(int ii=0; ii<3; ii++)
	    for(int jj=0; jj<3; jj++)
	    for(int kk=0; kk<3; kk++)
	        C2[ii,jj]+=C[ii,kk]*C[jj,kk];
	    
	    float det    =  F[0,0]*F[1,1]*F[2,2]+
	                    F[0,1]*F[1,2]*F[2,0]+
	                    F[1,0]*F[2,1]*F[0,2]-
	                    F[0,2]*F[1,1]*F[2,0]-
	                    F[0,1]*F[1,0]*F[2,2]-
	                    F[0,0]*F[1,2]*F[2,1];
	    
	    float I_c    =   C[0,0]+C[1,1]+C[2,2];
	    float I_c2   =   I_c*I_c;
	    float II_c   =   0.5f*(I_c2-C2[0,0]-C2[1,1]-C2[2,2]);
	    float III_c  =   det*det;
	    float k      =   I_c2-3*II_c;
	    
	    Matrix4x4 inv_U = Matrix4x4.zero;
	    if(k<1e-10f)
	    {
	        float inv_lambda=1/Mathf.Sqrt(I_c/3);
	        inv_U[0,0]=inv_lambda;
	        inv_U[1,1]=inv_lambda;
	        inv_U[2,2]=inv_lambda;
	    }
	    else
	    {
	        float l = I_c*(I_c*I_c-4.5f*II_c)+13.5f*III_c;
	        float k_root = Mathf.Sqrt(k);
	        float value=l/(k*k_root);
	        if(value<-1.0f) value=-1.0f;
	        if(value> 1.0f) value= 1.0f;
	        float phi = Mathf.Acos(value);
	        float lambda2=(I_c+2*k_root*Mathf.Cos(phi/3))/3.0f;
	        float lambda=Mathf.Sqrt(lambda2);
	        
	        float III_u = Mathf.Sqrt(III_c);
	        if(det<0)   III_u=-III_u;
	        float I_u = lambda + Mathf.Sqrt(-lambda2 + I_c + 2*III_u/lambda);
	        float II_u=(I_u*I_u-I_c)*0.5f;
	        
	        
	        float inv_rate, factor;
	        inv_rate=1/(I_u*II_u-III_u);
	        factor=I_u*III_u*inv_rate;
	        
	       	Matrix4x4 U = Matrix4x4.zero;
			U[0,0]=factor;
	        U[1,1]=factor;
	        U[2,2]=factor;
	        
	        factor=(I_u*I_u-II_u)*inv_rate;
	        for(int i=0; i<3; i++)
	        for(int j=0; j<3; j++)
	            U[i,j]+=factor*C[i,j]-inv_rate*C2[i,j];
	        
	        inv_rate=1/III_u;
	        factor=II_u*inv_rate;
	        inv_U[0,0]=factor;
	        inv_U[1,1]=factor;
	        inv_U[2,2]=factor;
	        
	        factor=-I_u*inv_rate;
	        for(int i=0; i<3; i++)
	        for(int j=0; j<3; j++)
	            inv_U[i,j]+=factor*U[i,j]+inv_rate*C[i,j];
	    }
	    
	    Matrix4x4 R=Matrix4x4.zero;
	    for(int ii=0; ii<3; ii++)
	    for(int jj=0; jj<3; jj++)
	    for(int kk=0; kk<3; kk++)
	        R[ii,jj]+=F[ii,kk]*inv_U[kk,jj];
	    R[3,3]=1;
	    return R;
	}

	// Update the mesh vertices according to translation c and rotation R.
	// It also updates the velocity.
	void Update_Mesh(Vector3 c, Matrix4x4 R, float inv_dt)
   	{
   		for(int i=0; i<Q.Length; i++)
		{
			Vector3 x=(Vector3)(R*Q[i])+c;

			V[i]+=(x-X[i])*inv_dt;
			X[i]=x;
		}	
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.vertices=X;
   	}

	void Collision()   //no need for dt
	{
		//particle collision
		// no need to consider the w (rotation) -> update v_new directly
		for(int j=0; j<2; j++)   //two walls 
		{
			// check the penetration first
			for(int i=0;i < X.Length; i++)
			{
				// recongnized as particles independently
			    var phi = Vector3.Dot(X[i]-P[j], N[j]);
				if(phi >=0 || Vector3.Dot(V[i], N[j])>=0) continue;

				Vector3 V_Ni = Vector3.Dot(V[i], N[j]) * N[j];
				Vector3 V_Ti = V[i] - V_Ni;
				float alpha = Mathf.Max(0, 1f - u_T * (1 + u_N) * V_Ni.magnitude / V_Ti.magnitude);

				//update to new and I don't want to declare new stuff
				V_Ni = -u_N * V_Ni;
				V_Ti = alpha * V_Ti;
				V[i] = V_Ni + V_Ti;

				//also Update the Position Here
				X[i] = X[i] + N[j] * Mathf.Abs(phi);   
			}

		}

	}

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKey(KeyCode.R))
		{
			launched = false;
			
			Update_Mesh(new Vector3(0f, 0.6f, 0f), Matrix4x4.Rotate(transform.rotation), 0);
			transform.position = Vector3.zero;
		    transform.rotation= Quaternion.identity;
			V = new Vector3[X.Length];
        	X = GetComponent<MeshFilter>().mesh.vertices;  //reset 
		}

		if(Input.GetKey(KeyCode.L))
		{
			launched = true;
			for(int i=0; i<X.Length; i++)
			V[i][0]= 4.0f;    //initial launching velocity reset
		}

		if(!launched) return; 
  		float dt = 0.015f;
  		//Step 1: run a simple particle system.
        for(int i=0; i<V.Length; i++)   //explicit way 
        {
				// update v and x with gravity 
				V[i] += g * dt;
				V[i] *= decay;
				X[i] += V[i] * dt;
        }

        //Step 2: Perform simple particle collision.
		Collision();

		// Step 3: Use shape matching to get new translation c and 
		// new rotation R. Update the mesh by c and R.
        //Shape Matching (translation)
		//calculate  c = 1/N sum y_i
		Vector3 c = Vector3.zero;
		for(var i =0; i<V.Length; i++)
		{
			c += X[i];
		}
		c /= V.Length;
		//Shape Matching (rotation)
		//calculate R
		Matrix4x4 A = Matrix4x4.zero;
		// the only way in Unity may go to the element-wise process 
		for(int i=0; i<Q.Length; i++)    //update every vertex
		{
			for(int j=0; j<3; j++)
			{
				for(int k=0; k<3; k++)
				{
					A[j, k] += (X[i] -c)[j] * Q[i][k]; 
				}
			}
			A[3,3] = 1;
		}
		A*=QQt.inverse;
		//A = Matrix4x4.identity;
		Matrix4x4 R = Get_Rotation(A);   //obtain the rotation from polar decomposition
		Update_Mesh(c, R, 1/dt);
    }
}
