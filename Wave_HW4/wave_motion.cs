using UnityEngine;
using System.Collections;

public class wave_motion : MonoBehaviour 
{
	[SerializeField]
	int size 		= 100;   // larger than 65535, bad for int
	float rate 		= 0.005f;
	float gamma		= 0.002f;
	float damping 	= 0.99f;
	float[,] 	old_h;
	float[,]	low_h;
	float[,]	vh;  //virtual height for coupling
	float[,]	b;

	bool [,]	cg_mask;
	float[,]	cg_p;
	float[,]	cg_r;
	float[,]	cg_Ap;
	//bool 	tag=true;

	Vector3 	cube_v = Vector3.zero;  //?
	Vector3 	cube_w = Vector3.zero;


	public Transform block_1;
	Bounds block_1_bounds;
	public Transform block_2;
	Bounds block_2_bounds;

	// Use this for initialization
	void Start () 
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear ();
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;   // we need 32 for further!!!

		Vector3[] X=new Vector3[size * size];

		for (int i=0; i<size; i++)
		for (int j=0; j<size; j++) 
		{
			X[i*size+j].x=i*0.1f-size*0.05f;   
			X[i*size+j].y=0;
			X[i*size+j].z=j*0.1f-size*0.05f;
		}

		int[] T = new int[(size - 1) * (size - 1) * 6];
		int index = 0;
		for (int i=0; i<size-1; i++)
		for (int j=0; j<size-1; j++)   // clock-wise order
		{
			T[index*6+0]=(i+0)*size+(j+0);
			T[index*6+1]=(i+0)*size+(j+1);
			T[index*6+2]=(i+1)*size+(j+1);
			T[index*6+3]=(i+0)*size+(j+0);
			T[index*6+4]=(i+1)*size+(j+1);
			T[index*6+5]=(i+1)*size+(j+0);
			index++;
		}
		print("Pos: Start" + X[0]);
		print("Pos: End" + X[(size -1) * size + size - 1]);
		mesh.vertices  = X;
		mesh.triangles = T;
		mesh.RecalculateNormals ();

		low_h 	= new float[size,size];
		old_h 	= new float[size,size];
		vh 	  	= new float[size,size];
		b 	  	= new float[size,size];

		cg_mask	= new bool [size,size];
		cg_p 	= new float[size,size];
		cg_r 	= new float[size,size];
		cg_Ap 	= new float[size,size];

		for (int i=0; i<size; i++)
		for (int j=0; j<size; j++) 
		{
			low_h[i,j]=99999;
			old_h[i,j]=0;
			vh[i,j]=0;
		}
	}

	void A_Times(bool[,] mask, float[,] x, float[,] Ax, int li, int ui, int lj, int uj)
	{
		for(int i=li; i<=ui; i++)
		for(int j=lj; j<=uj; j++)
		if(i>=0 && j>=0 && i<size && j<size && mask[i,j])
		{
			Ax[i,j]=0;
			if(i!=0)		Ax[i,j]-=x[i-1,j]-x[i,j];
			if(i!=size-1)	Ax[i,j]-=x[i+1,j]-x[i,j];
			if(j!=0)		Ax[i,j]-=x[i,j-1]-x[i,j];
			if(j!=size-1)	Ax[i,j]-=x[i,j+1]-x[i,j];
		}
	}

	float Dot(bool[,] mask, float[,] x, float[,] y, int li, int ui, int lj, int uj)
	{
		float ret=0;
		for(int i=li; i<=ui; i++)
		for(int j=lj; j<=uj; j++)
		if(i>=0 && j>=0 && i<size && j<size && mask[i,j])
		{
			ret+=x[i,j]*y[i,j];
		}
		return ret;
	}

	//well still ok to GPU but really complicated
	void Conjugate_Gradient(bool[,] mask, float[,] b, float[,] x, int li, int ui, int lj, int uj)
	{
		//Solve the Laplacian problem by CG.
		A_Times(mask, x, cg_r, li, ui, lj, uj);

		for(int i=li; i<=ui; i++)
		for(int j=lj; j<=uj; j++)
		if(i>=0 && j>=0 && i<size && j<size && mask[i,j])
		{
			cg_p[i,j]=cg_r[i,j]=b[i,j]-cg_r[i,j];
		}

		float rk_norm=Dot(mask, cg_r, cg_r, li, ui, lj, uj);

		for(int k=0; k<128; k++)
		{
			if(rk_norm<1e-10f)	break;
			A_Times(mask, cg_p, cg_Ap, li, ui, lj, uj);
			float alpha=rk_norm/Dot(mask, cg_p, cg_Ap, li, ui, lj, uj);

			for(int i=li; i<=ui; i++)
			for(int j=lj; j<=uj; j++)
			if(i>=0 && j>=0 && i<size && j<size && mask[i,j])
			{
				x[i,j]   +=alpha*cg_p[i,j];
				cg_r[i,j]-=alpha*cg_Ap[i,j];
			}

			float _rk_norm=Dot(mask, cg_r, cg_r, li, ui, lj, uj);
			float beta=_rk_norm/rk_norm;
			rk_norm=_rk_norm;

			for(int i=li; i<=ui; i++)
			for(int j=lj; j<=uj; j++)
			if(i>=0 && j>=0 && i<size && j<size && mask[i,j])
			{
				cg_p[i,j]=cg_r[i,j]+beta*cg_p[i,j];
			}
		}

	}

	void Shallow_Wave(float[,] old_h, float[,] h, float [,] new_h)
	{		
		//Step 1:
		//TODO: Compute new_h based on the shallow wave model.
		for(int i=0; i<size; i++)
        {
			for(int j=0; j<size; j++)
            {
				new_h[i, j] = h[i, j] + damping * (h[i, j] - old_h[i, j]);  //viscosity
				//volume preservation
				if(i-1>=0)
					new_h[i, j] += rate * (h[i - 1, j] - h[i, j]);
				if(i+1< size)
					new_h[i, j] += rate * (h[i + 1, j] - h[i, j]);
				if (j - 1 >= 0)
					new_h[i, j] += rate * (h[i, j-1] - h[i, j]);
				if (j + 1 < size)
					new_h[i, j] += rate * (h[i, j+1] - h[i, j]);
				//reinitialize cg_mask, b, vh each time
				cg_mask[i, j] = false;
				b[i, j] = 0.0f;
				vh[i, j] = 0.0f;
			}
        }

		//Step 2: Block->Water coupling    
		//TODO: for block 1, calculate low_h.
		Vector3 block_pos = block_1.position;
		block_1_bounds = block_1.GetComponent<BoxCollider>().bounds;
		//check reduced grid for potential collision
		//make it back to index of size
		//int li = (int)((block_pos.x + (size-1f)*0.05f) * size * 0.1f), ri = (int)((block_pos.x + (size - 1f) * 0.05f) * size * 0.1f);
		//int lj = (int)((block_pos.z + (size+1f)* 0.05f) * size * 0.1f), rj = (int)((block_pos.z + (size + 1f) * 0.05f) * size * 0.1f);
		int li = (int)((block_pos.x + size * 0.05f-1.0f) * 10f), ri = (int)((block_pos.x + size * 0.05f+1.0f) * 10f);
		int lj = (int)((block_pos.z + size * 0.05f-1.0f) * 10f), rj = (int)((block_pos.z + size * 0.05f+1.0f) * 10f);    //from ray intersection back to the index
		li = Mathf.Clamp(li, 0, size - 1);
		ri = Mathf.Clamp(ri, 0, size - 1);
		lj = Mathf.Clamp(lj, 0, size - 1);
		rj = Mathf.Clamp(rj, 0, size - 1);

		
		float ei = 10f;
		// use intersect ray for determing the low_h
		//https://docs.unity3d.com/ScriptReference/Bounds.IntersectRay.html
		Vector3[] wave_x = this.GetComponent<MeshFilter>().mesh.vertices;
		Ray ray;
		for (int i=li; i<=ri; i++)
        {
			
			for (int j=lj; j<=rj; j++)
            {
				float dis = 1000f;
				Vector3 wave_discrete = wave_x[size * i + j];
				wave_discrete.y = -10f;   //test from the bottom
				ray = new Ray(wave_discrete, Vector3.up);
				block_1_bounds.IntersectRay(ray, out dis);
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
		Conjugate_Gradient(cg_mask, b, vh, li, ri, lj, rj);  //region restricted
		//TODO: for block 2, calculate low_h.
		block_pos = block_2.position;
		block_2_bounds = block_2.GetComponent<BoxCollider>().bounds;
		//check reduced grid for potential collision
		//make it back to index of size
		li = (int)((block_pos.x + size* 0.05f-1.0f) * 10f); ri = (int)((block_pos.x + size* 0.05f+1.0f) * 10f);
		lj = (int)((block_pos.z + size* 0.05f-1.0f) * 10f); rj = (int)((block_pos.z + size * 0.05f+1.0f) * 10f);
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
															 //TODO: Diminish vh.
		
		//TODO: Update new_h by vh.
		//I combined this together gamma * rate
		for (int i=0; i<size; i++)
        {
			for(int j=0; j<size; j++)
            {
				if (i - 1 >= 0)
					new_h[i, j] += rate * gamma * (vh[i - 1, j] - vh[i, j]);
				if (i + 1 < size)
					new_h[i, j] += rate * gamma * (vh[i + 1, j] - vh[i, j]);
				if (j - 1 >= 0)
					new_h[i, j] += rate * gamma * (vh[i, j - 1] - vh[i, j]);
				if (j + 1 < size)
					new_h[i, j] += rate * gamma * (vh[i, j + 1] - vh[i, j]);

			}
        }
		//Step 3
		//TODO: old_h <- h; h <- new_h;
		//need to assign the value one by one
		for (int i=0; i<size; i++)
        {
			for(int j=0; j<size; j++)
            {
				old_h[i, j] = h[i, j];
				h[i, j] = new_h[i, j];
            }
        }
		//Step 4: Water->Block coupling. the second coupling (bonus)
		//More TODO here.
	}
	

	// Update is called once per frame
	void Update () 
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] X    = mesh.vertices;
		float[,] new_h = new float[size, size];
		float[,] h     = new float[size, size];

		//TODO: Load X.y into h.
		for(int i=0; i<size; i++)
        {
			for(int j=0; j<size; j++)
            {
				h[i, j] = X[i*size+j].y;
            }
        }
		if (Input.GetKeyDown ("r")) // not supported in gpu
		{
			//TODO: Add random water.
			Random.InitState(Time.frameCount);  //use frame count as the seed of random generator
			int i= Random.Range(0, size), j= Random.Range(0, size);
			float r = Random.Range(0, 0.3f);
			h[i, j] += r;
			float cnt = 0f;
			if (i + 1 < size) cnt++;
			if (i - 1 >= 0) cnt++;
			if (j + 1 < size) cnt++;
			if (j - 1 >= 0) cnt++;

			if (i + 1 < size)
				h[i + 1, j] -= r/cnt;
			if (i - 1 >= 0)
				h[i - 1, j] -= r/cnt;
			if (j+1 <size)
				h[i, j + 1] -= r/cnt;
			if (j - 1 >= 0)
				h[i, j-1] -= r/cnt;

		}
	
		for(int l=0; l<8; l++)
		{
			Shallow_Wave(old_h, h, new_h);
		}

		//TODO: Store h back into X.y and recalculate normal.
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				X[i * size + j].y = h[i, j];
			}
		}
		mesh.vertices = X;
		mesh.RecalculateNormals();
		
	}
}
