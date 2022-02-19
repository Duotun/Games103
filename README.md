# Games103 - Physics-based Animation
Games 103 - HW and Notes (HW File are archived and the following are notes)



#### Notes

##### Animation Paradigm

Update state + time step (not the frame rate)

![img](/images/1)

![img](/images/2)



##### Vector Definition (According to the Screen Space)

![img](/images/3)

![img](/images/4)

##### Test a Point Inside a Triangle 

测试与三边连起来的normal 是否同向 (**the same directions**)

![img](/images/5)

![img](/images/6)



##### Barycentric Coordinates (If p is inside the triangle, otherwise weights could be negative)

-> barycentric interpolation

![img](/images/7)

###### Tetrahedral Volume (Could be negative here, inverted)

![img](/images/8)

##### 

##### Singular Value Decomposition

Linear Deformation -> Rotation + Scaline + Rotation

![img](/images/9)



If A is symmetric -> **Eigenvalue Decomposition**

![img](/images/10)

##### Symmetric Positive Definiteness 最强解释

与Eigenvalue decomposition 相结合, 从 v^T d v >0 开始, s.p.d <-> eigenvlaues are positive

![img](/images/11)

![img](/images/12)

In practice, 按列/按行进行求和比较对角线元素

![img](images/13)



##### To solve AX = B, Iterative Method

spectral radius (largest absolute eigenvalue)

![img](images/14)

##### Derivative & Gradient

Gradient -> Column vectors , from the **definition of derivatives** df/dx -> row vectors (based on that the **vector x is a column vector**)

![img](images/15)

For a Vector -> Divergence is the **trace (sum of the diagonal elements)** of Jacobian

![img](images/16)

The same applies to the 2nd-order Derivatives

![img](images/17)

Let's go for an example of Derivative (|x|)

![img](images/18)



So for the spring -> from the Engergy Function to the Hessian

![img](images/19)





##### Quaternion Product = Cross Product - Dot Product

cross product of imaginary part (a, b, c for i, j, k) - dot product of real part (w)

https://www.johndcook.com/blog/2012/02/15/dot-cross-and-quaternion-products/

![img](images/20)



##### Integration Method

Explicit Method (t_0)

![img](images/21)

Implicit Method (t_1)

![img](images/22)

Midpoint Method (t_0.5)

![img](images/23)

Then for the posiiton update in the end, utilize two mid-point integration -> leapfrog

![img](images/24)



##### Why we use quaterniont for the Rotation

real part and complex part for axis + multiplication of rotations

**norm of quaternion = 1** to make sure it is a rotation

![img](images/25)

![img](images/26)

![img](images/27)

##### Force Accumulation for Rotation and Translation (Torque and Inertia)

http://www.kwon3d.com/theory/moi/iten.html

![img](images/28)

Inertia used for describiing the **resistance** to rotational tendency caused by torque

比如下图，右边旋转对物体影响会更猛一点，因为inertia 都集中在了那个轴附近 (thus, not constant)

![img](images/29)



##### Rigidbody Simulation Implementation

![img](images/30)

![img](images/31)



##### Particl Collision Detection and Response

Signed Distance Function Example

![img](images/32)

Utilize SDF < 0

![img](images/33)

![img](images/34)

###### 1) Quadratic Penalty Method -> next update

if sdf < 0 -> apply force to compell it + **buffer to avoid penetration**

![img](images/35)

![img](images/36)

if k is not a constant factor

![img](images/37)



###### 2) Impulse Method -> Change Happens all of a Sudden

change the position and velocity all of a sudden + apply Coulomb's law

![img](images/38)



##### Shape Matching for Rigidbod Dynamics

\1) Move vertices independently 2) Add **rigidity constraints** to keep rigid body

![img](images/39)

 Keep the centroid the same as before

![img](images/40)

Polar Decomposition -> Rotation and Deformation (Scaling)

![img](images/41)

![img](images/42)

##### Mass-Spring System

###### 1) Spring and Structure

spring force - restoring the spring (negative derivative of energy)

![img](images/43)

For multiple springs, energe and forces are simply summed up

![img](images/44)

Bending spring is used to avoid bending along some directions / diagonal springs for diagonal stretching and compress

![img](images/45)

###### 2) Build mass-spring system from triangular mesh

should consider the the diagonal spring across two triangles (**utilizing neighboring information**)

###### 3) Explicit Integration of Mass-Spring-System (numerical instability, e.g., overshooting)

overshooting happens generally when stiffness is large or the delta time is large.

![img](images/46)

###### 4) Implicit Integration (x[1] from v[1], v[1] from f[1])

Solve the equation by assuming forces are **only influced** by the position

![img](images/47)

-> Optimize Porblem (argmin)

![img](images/48)

Then utilize approximation method to solve: e.g., newton-raphson method + taylor expansion

![img](images/49)

either **local minimum or maximum** method depending on the second derivatives

![img](images/50)

Compute the Gradient (delta_x) then

![img](images/51)

###### 5) Spring Hessian

When spring is streatched, H is spd (1 - L/||x|| >0). When spring is compressed, H may be not and then producing **multiple outcomes**.

![img](images/52)



![img](images/53)



delta_t less, H becomes more spd

![img](images/54)

Positive Definiteness & Maximum & Minimum

![img](images/55)

If H is not Spd the **AX = B is hard to solve**

![img](images/56)

###### 6) Method to solve Linear System (Just a few here)

Jacobi utilize the residual to iterate and diagonal of the A matrix

![img](images/57)



![img](images/58)

![img](images/59)



###### 7) Bending and Locking Issue

Length changes a little for the bending -> use dihedral angle model to solve

![img]images/60)

the sum of internal forces is zero; don't stretch the bending edge; span of normals

![img](images/61)

![img](images/62)

Then obtain the forces as a function with u and f considering the bending angle (theta)

This method is very hard to do implicit integration since the derivative is too hard (**no definition for the energy**)

![img](images/63)

Then, there is another bending model which is easy to implement - **quadratic bending model**

quadratic model based on two assumptions: little straching and **starting from** the planer case

Highly related to the **laplacian operator /curvature**

![img](images/64)

![img](images/65)



Locking Issue: 

consider a high stiffness spring, then the bottom-right case is nearly impossible to bend

The fundamental reason is due to a short of **degree of freedom**

![img](images/66)

![img](images/67)



##### Constrained Approach

###### Stiffness Issue - incersing stiffness (k) can be problematic sometimes

![img](images/68)

For example, length as a constraint -> projection function (keep the mass center constant)

![img](images/69)

multiple springs approaching the **constrained conditions (rest length)**

![img](images/70)

![img](images/71)

Then to avoid the above bias problem, jacobi method is presented. Process **all** the edges related to the one vertex first and update **the average case** then. (n is for the number of updating)

![img](images/72)



##### Position Based Dynamics (PBD)

Highly influced by the **number of iteration and the mesh resolution**

More iterations -> close to the constaints and hence more stiff

**low** mesh resolution -> **close to the constaints become more easily** and hence more stiff

PBD is based on the **projection function** and that's why we mentioned the above approaches for the constrained. PBD is a process to conduct **constraints** on the simulation Very similar to the shape mathing method but here PBD's containts mainly on the **edges of flex objects** while shape matching cares about the **rigidbody** mesh itself.

Generally simulating in real-time when the vertices are less than **1000**

![img](images/73)

![img](images/74)

To improve the cons of PBD, let's do some improvements

###### 1) Strain Limiting

Use **projection function for correction only** rather than simulation in PBD

![img](images/75)

Spring Strain limit **becomes a range** (relax the constraints)

**Strain is the physics parameter** to describe the stratching change property

![img](images/76)

![img](images/77)

This relaxation could be 2D, 3D. For example, the following triangle area case

Update the vertice case according to the mass point and newly scaling factor

![img](images/78)

Why we use strain limiting - a good complement for the large deformation and locking issue

![img](images/79)

##### Projective Dynamics

projective dynamics uses **projection** to define a **quadratic** energy.

For example, for the following triangle mesh, we define the energe by summing the edges

To keep it original the distance between x_inew and x_jnew should be the **rest length**

Utilize the projection of x_inew and x_jnew **in the middle**

![img](images/80)

The main difference with the previous spring model is the **Hessian, making a** **constant Hessian (related to the identity matrix only)**

Diagonal parts store the **information of vertices** and non-diagonal parts store the **edge informaiton. Be carefule about the positive and negative things**

![img](images/81)

Simulation by Projective Dynamics

Solve a linear system with a **constant matrix which could be** **precomuputed** **for all**

![img](images/82)

###### Why does it work for all (Porjective Dynamics)

![img](images/83)



The advantage of PBD is that the **memory access number** is very small reducing the overall time comsuming! Not physical parameters and only caring about the positions.

###### Shape Matching - Also Projective Dynamics (If assuming rotaiton matrix is constant)



###### Pros and Cons of Projective Dynamics

Pros: 

\1) Fast simualtion with physical meaning

\2) Fast on CPU with one factorization with direct solver

\3) Fast convergence in the first iterations



Cons:

\1) Slow on GPUs (GPU doean't support direct solver well)

\2) Slow convergence over time and it fails to consider the **Hessian** 

**(also suffer from the high stiffness)**

\3) Can't easily handle **constraint changes** (e.g., **remesh** like fracture or contacts)



##### Constrained Dynamics (For Very Stiff Case)

Reduce **iteration numbers** while performing a good convergence with the high stiffness

![img](images/84)

Build the linear system with two equations (momentum and updating with lambda)

![img](images/85)



Then solve this linear system

Solve the two unknown variables together (**primal-dual, often requiring Positive definite)** or schur complement

![img](images/86)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MGM4YmM5ODM4MWU1OWUzM2E0YTM1ZjEyZTdhNjYyZWNfeW5EUldEeW8yejA0VWVxdnhMa2pEV3BWblA2VW9HQ2FfVG9rZW46Ym94azRjYzNaS0FWTDRVcHAxaTdBcGg5VnVlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Summary of the Above

![img](images/87)



##### The Linear Finite Element Method (FEM)

Deformation could be described in a linear equation. F could also describe the **vector change but F also contains rotation.**

![img](images/88)

![img](images/89)

**Green Strain** (G) -> a symmtry matrix utilizing svd to cancel extra elements

Strain (a type of tensor) is defined to describe the deformation

G is rotation invariant

![img](images/90)

###### Strain Energy Density Function

e.g., stvk model (Saint Venant-Kirchhoff Model) + lame parameters in the **reference states**

![img](images/91)

The we could obtian the forces from the density funciton

The sum of internal forces are zero

![img](images/92)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MmJjMGNmYzVkNjNlYjk4NWMyNDdmYTBjZDVkMzJiMzRfV1Z0N2pXMVpJcHBXMTExYWJ0OFBKWUdvVW5XNjBwYTVfVG9rZW46Ym94azRsSWZoU1R3cVVnRWI3akNDd21KN0dmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Finite Volume Method

Let's start with the source of internal deformable forces

Tranction t: internal force per unit length (area)

![img](images/93)

 Normal is constant over edges (3D over areas)

![img](images/94)

![img](images/95)



###### Laplacian Smoothing 

Obtain averaged positoin values by local neighbors (often used to ease numerical instability) 

![img](images/96)



###### Difference of Stress (mapping of internel normal) between FEM and FVM

Stress = Traction but the configuration is defined differently in FEM and FVM

Traction could be interpreted as a sort of **force density**

Stress is computed in the reference state in FEM for the energy density

Stress is computed in the deformed state in FVM for the calculation of force.

![img](images/97)



![img](images/98)

![img](images/99)

![img](images/100)

Then using the above translation to obtain the cauchy stress to calculate the **forces**

To calculate the force, we could use the three above stresses

![img](images/101)

![img](images/102)



###### FEM/FVM Framework

![img](images/103)

In the implementaiton stage, we can apply no force on elements to identify our implementation, which means F becomes identity, G becomes zero and all the internal forces become zero too.

###### Hyperelastic Model (A more general way than stvk)

From energy density -> stress, treat First PK stress P is a function of Deformation gradient (F)

![img](images/104)



**Isotropical Material**

Streatch in different positions and direcitons causing the same deformation (如橡胶， rubber)

Then we only wanna how to stretch (scaling) looks like, which is called the principal stretch

![img](images/105)

![img](images/106)

Then, the simulation of hyperelastic model could become the following loop

![img](images/107)

###### Limitation of STVK

If deformation is somehow inversed, the stress will become zero and the deformation **cannot be restored to 1**.

![img](images/108)



**Poisson Effect**

used to help keep the volume **constant** when the stress is applied (例如拉上去，那么其他部分会瘦身从而保持体积恒定）

![img](images/109)



###### Summary of FEM and FVM (Derivative and Integral)

![img](images/110)



##### Non-Linear Optimization

**Descent Gradient Method -> Step Size**

![img](/images/111)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzJmZDQ4ZjBiZWZlY2JiMzc3NGYxMzkxMGE1ZDUwYzdfT1lvVGIyTHJ4cmFDRkY4WVNTc09PSU44M1VLbmZzYWNfVG9rZW46Ym94azRNZk9hWmFCbDJSc2pCNDdnbmkxdFRlXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)



**Descend Direction**

Utilize the dot product, if in the same side of the gradient, then the direction is OK

![img](/images/112)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NDQzYmYyMzRkNDE4ZmZiYTQwYmM4OTUwYzhiYzFmY2VfRjJKV2hwYmtDR0xEb2FRa1hRQ3FkbXdFTk9nOHBBQ2VfVG9rZW46Ym94azRpNmpuTU1scjZ3ZlNET09VRk9sU3lkXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

 So, in the origin, the descent describe how the **integration** could work

![img](/images/113)

For example, why we don't newton's method in the often cases is because that the per-iteracion cost is too much since we have to compute the **hessian matrix** each iteration.

![img](/images/114)

