using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class implicit_model : MonoBehaviour
{
	float 		t 		= 0.0333f;
	float 		mass	= 1;
	float		damping	= 0.99f;
	float 		rho		= 0.995f;
	float 		spring_k = 8000;
	int[] 		E;
	float[] 	L;
	Vector3[] 	V;
	Vector3 gravity = new Vector3(0f, -9.8f, 0f);
	float r = 2.7f;   //hard-code the radius
	float omega = 0f;
	// Start is called before the first frame update
	void Start()
    {
		Mesh mesh = GetComponent<MeshFilter>().mesh;

		//Resize the mesh.
		int n=21;
		Vector3[] X  	= new Vector3[n*n];
		Vector2[] UV 	= new Vector2[n*n];
		int[] triangles	= new int[(n-1)*(n-1)*6];
		for(int j=0; j<n; j++)
		for(int i=0; i<n; i++)
		{
			X[j*n+i] =new Vector3(5-10.0f*i/(n-1), 0, 5-10.0f*j/(n-1));
			UV[j*n+i]=new Vector3(i/(n-1.0f), j/(n-1.0f));    // assign [0 - 1]
		}
		int t=0;
		for(int j=0; j<n-1; j++)
		for(int i=0; i<n-1; i++)	
		{
			triangles[t*6+0]=j*n+i;
			triangles[t*6+1]=j*n+i+1;
			triangles[t*6+2]=(j+1)*n+i+1;
			triangles[t*6+3]=j*n+i;
			triangles[t*6+4]=(j+1)*n+i+1;
			triangles[t*6+5]=(j+1)*n+i;
			t++;
		}
		mesh.vertices=X;
		mesh.triangles=triangles;
		mesh.uv = UV;
		mesh.RecalculateNormals ();


		//Construct the original E, edge index pair
		int[] _E = new int[triangles.Length*2];
		for (int i=0; i<triangles.Length; i+=3) 
		{
			_E[i*2+0]=triangles[i+0];    // two es for one edges
			_E[i*2+1]=triangles[i+1];
			_E[i*2+2]=triangles[i+1];
			_E[i*2+3]=triangles[i+2];
			_E[i*2+4]=triangles[i+2];
			_E[i*2+5]=triangles[i+0];
		}
		//Reorder the original edge list
		for (int i=0; i<_E.Length; i+=2)
			if(_E[i] > _E[i + 1]) 
				Swap(ref _E[i], ref _E[i+1]);
		//Sort the original edge list using quicksort
		Quick_Sort (ref _E, 0, _E.Length/2-1);

		int e_number = 0;
		for (int i=0; i<_E.Length; i+=2)
			if (i == 0 || _E [i + 0] != _E [i - 2] || _E [i + 1] != _E [i - 1]) 
					e_number++;

		E = new int[e_number * 2];
		for (int i=0, e=0; i<_E.Length; i+=2)
			if (i == 0 || _E [i + 0] != _E [i - 2] || _E [i + 1] != _E [i - 1]) 
			{
				E[e*2+0]=_E [i + 0];
				E[e*2+1]=_E [i + 1];
				e++;
			}

		L = new float[E.Length/2];
		for (int e=0; e<E.Length/2; e++) 
		{
			int v0 = E[e*2+0];
			int v1 = E[e*2+1];
			L[e]=(X[v0]-X[v1]).magnitude;
		}

		V = new Vector3[X.Length];
		for (int i=0; i<V.Length; i++)
			V[i] = new Vector3 (0, 0, 0);
    }

    void Quick_Sort(ref int[] a, int l, int r)
	{
		int j;
		if(l<r)
		{
			j=Quick_Sort_Partition(ref a, l, r);
			Quick_Sort (ref a, l, j-1);
			Quick_Sort (ref a, j+1, r);
		}
	}

	int  Quick_Sort_Partition(ref int[] a, int l, int r)
	{
		int pivot_0, pivot_1, i, j;
		pivot_0 = a [l * 2 + 0];
		pivot_1 = a [l * 2 + 1];
		i = l;
		j = r + 1;
		while (true) 
		{
			do ++i; while( i<=r && (a[i*2]<pivot_0 || a[i*2]==pivot_0 && a[i*2+1]<=pivot_1));
			do --j; while(  a[j*2]>pivot_0 || a[j*2]==pivot_0 && a[j*2+1]> pivot_1);
			if(i>=j)	break;
			Swap(ref a[i*2], ref a[j*2]);
			Swap(ref a[i*2+1], ref a[j*2+1]);
		}
		Swap (ref a [l * 2 + 0], ref a [j * 2 + 0]);
		Swap (ref a [l * 2 + 1], ref a [j * 2 + 1]);
		return j;
	}

	void Swap(ref int a, ref int b)
	{
		int temp = a;
		a = b;
		b = temp;
	}

	void Collision_Handling()
	{
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] X = mesh.vertices;

		//Handle colllision.
		var sphere_center = GameObject.Find("Sphere").transform.position;
		float dis = 0f;
		if(sphere_center == null)
        {
			Debug.LogError("No Collision Object");
			return;
        }
		for(int i=0; i<X.Length; i++)
        {
			dis = (X[i] - sphere_center).magnitude;
			if(dis < r)  //then, apply impulse-based collision
            {
				V[i] += 1.0f / t * (sphere_center + r * (X[i] - sphere_center) / dis - X[i]);
				X[i] = sphere_center + r * (X[i] - sphere_center) / dis;
			}
        }
		mesh.vertices = X;
	}

	// back to a very simplified method
	void Get_Gradient(Vector3[] X, Vector3[] X_hat, float t, Vector3[] G)
	{
		// displacement only first for every vertex
		//Momentum and Gravity.
		//add per vertex gravity force
		//combined into one loop
		for (int i=0; i<X.Length; i++)
        {
			G[i] = 1 / t / t * mass * (X[i] - X_hat[i]);
			G[i] -= mass * gravity;     //negated gravity force
        }
		//Spring Force.
		int vi = 0, vj = 0;
		float len = 0f;
		for(int e=0; e< E.Length/2; e++)
        {
			vi = E[2 * e + 0];
			vj = E[2 * e + 1];
			len = (X[vi] - X[vj]).magnitude;
			G[vi] += spring_k * (1.0f - L[e] / len) * (X[vi] - X[vj]);
			G[vj] -= spring_k * (1.0f - L[e] / len) * (X[vi] - X[vj]);
		}
		
	}

	void Jacobi_With_Acceleration(ref Vector3 last_dx, ref Vector3 dx, in Vector3 b)
    {
		Vector3 residual = Vector3.zero;
		float A = 1 / t / t * mass + 4 * spring_k;  //only consider the diagonal
		for (int k =0; k<32; k++)
        {
			residual = -1.0f * b - A * dx;
			if (residual.magnitude < 0.01f) return;
			if (k == 0) omega = 1;
			else if (k == 1) omega = 2 / (2 - rho * rho);
			else omega = 4 / (4 - rho * rho * omega);

			var old_dx = dx;
			dx = dx + residual/A;
			dx = omega * dx + (1 - omega) * last_dx;
			last_dx = old_dx;
		}
    }

    // Update is called once per frame
	void Update () 
	{
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] X 		= mesh.vertices;
		//Vector3[] last__delta_x 	= new Vector3[X.Length];
		Vector3[] X_hat 	= new Vector3[X.Length];
		Vector3[] G 		= new Vector3[X.Length];
		
		//Initial Setup.
		//1. apply damping of v first
		for (int i=0; i< X.Length; i++)
        {
			V[i] *= damping;
			X_hat[i] = X[i] + V[i] * Time.deltaTime;
			X[i] = X_hat[i];   //adding the v_0 first (implicit method)
			//last_X[i] = X[i];

		}

		//we could utilize the Chebyshev Acceleration
		// but no ending condition here since we are using 32 iterations
		for (int k=0; k<32; k++)
		{
			Get_Gradient(X, X_hat, t, G);   
             //let's add the Chebyshev Acceleration here for the delta_x
			for(int i=1; i<X.Length; i++)
            {
				if (i == 20) continue;
				Vector3 delta_x = Vector3.zero;
				Vector3 last_x = Vector3.zero;
				Jacobi_With_Acceleration(ref last_x, ref delta_x, in G[i]);
				X[i] += delta_x;

			}

			/*
			//Update X by gradient. don't update the 0 and 21th vertex, simple method
			for (int i =1; i< X.Length; i++)
            {
				if (i == 20) continue;			
			    X[i] += -1.0f / (1 / t / t * mass + 4 * spring_k) * G[i];		
            }
			*/
			
		}

		//Finishing.
		for (int i = 1; i < X.Length; i++)
		{
			if (i == 20) continue;
			{
				V[i] += 1.0f / t * (X[i] - X_hat[i]);
			}
		}
		mesh.vertices = X;

		Collision_Handling ();
		mesh.RecalculateNormals ();
	}


}
