# Games103 - Physics-based Animation
Games 103 - HW and Notes (HW File are archived and the following are notes)



#### Notes

##### Animation Paradigm

Update state + time step (not the frame rate)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzFkZTA0YTRhMjBjNWY3OTVmNmZlOGU0ZGJkYjEyNDZfQWlDWHZWZVhCeGxkbFFlSUhkcnhDSVlobkxSY1FwcVJfVG9rZW46Ym94azQ4eGJTNnJHQXRCaTBBajJIUEtmMFVmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OGE2NDFkNjQ3MTg0NTQ2MTZhZjFkNDAwNWM4OTVkOGNfWEtqdm9TZkV5RnpXS1h4NXlHQTA4RzBnTm1pdWQ0UjFfVG9rZW46Ym94azRrMUlNYlFURU9IM25xNGtaWDgwWW56XzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Vector Definition (According to the Screen Space)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NDAwZDM2OTc5MzdkOWY5ODI2OTAwYmM4ZWVhZjEzYjNfd1lMNkdPV2NaTWNPcjNZOThvY0l1M1hhWElQcU5FU3lfVG9rZW46Ym94azRBUXBJQk9IeUNpem5IcEZ3MW9FQjljXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZjFhNjMzZmQ0NjEwNDBhZmZjMDEzNzkwOWQzN2QzY2Vfc2IxZlRuTlhDY2VDMzNTTXd2RVVWSGlUQU1QRlFZQlFfVG9rZW46Ym94azQxeDFBckI0cVhJTWF6YXlNYk1HTVU0XzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

##### Test a Point Inside a Triangle 

测试与三边连起来的normal 是否同向 (**the same directions**)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NDAyNjc0OTMzYzQxMzVlMTA5ODRlZmUzOGE0YzAxMzFfNVFuaTFnT3RETENJbnUzNnQ4ZXRpbll6eUdMRFJ1ODNfVG9rZW46Ym94azRyQ3R4dDdpcFN3ZGsxS0Y4ak43eXRjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ODFkY2M0ODMxZmZjZjFlNDFmYmFmNjJhYjAxZTlmYmZfTVdaeWU0SkpwRXMxdWIxOUhobTJyR0diZ0YyZlRnRGxfVG9rZW46Ym94azRLZ0Q1R1NZekN2bHVPSnFuSGxRZnhkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Barycentric Coordinates (If p is inside the triangle, otherwise weights could be negative)

-> barycentric interpolation

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZDAyOTNhYTUwZTA5ODc2YTdkNzRlYmJlYzBjNzVlMWZfWmZpYkQ2V1MzdVZEUE5ucEF4WGt2NDk2dlZHZVF0cDNfVG9rZW46Ym94azRZNmRqY2hsTVhETkJUVkxXVnU4MzlnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

###### Tetrahedral Volume (Could be negative here, inverted)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MzkxZjhhNTUzZjI0MTM4YWU1MWEwNDFjODEyYThkYThfOVRhNEFJVEVFWWdlaWtvU0t2bXBPQ2t3NDY5N0RvbHBfVG9rZW46Ym94azRBSUllYW8xNnhWWmlHVUxTMWxVMnVmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

##### 

##### Singular Value Decomposition

Linear Deformation -> Rotation + Scaline + Rotation

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZGYyODI2NTA4MzYyYWY5NTFiN2RkZWFjNmMzYWY2NjZfYzIycmFIZktrTmRYUElucHowVFdTeGxqMUY5N0FLZXhfVG9rZW46Ym94azRlSXNVYnJCUVRLR2t1N3U5eGhFZ2lkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



If A is symmetric -> **Eigenvalue Decomposition**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzYwOTQwYzc2YThkNTI0MjJiM2I4Y2YxZmVlMDIwZTZfeUw5MWZXYUk1Vk15ZGZYWkxNRExjT0NMYmIxeElzNElfVG9rZW46Ym94azR5TkpHSWUxWXBTSVpiZ3hxMHd4endkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

##### Symmetric Positive Definiteness 最强解释

与Eigenvalue decomposition 相结合, 从 v^T d v >0 开始, s.p.d <-> eigenvlaues are positive

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=Y2YxODdhYWE3OGEyYmZjNzFkYWMzYmIxNTM3ZGM4NDhfZFFaRkx6VnpXQ3NlR3g2NG9PeUk0OEZuMVRsd3FDNndfVG9rZW46Ym94azRRRUtNNlhSZlVWT01wZEdBMWNoS1djXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzRiNWQ4YTg3NTNiMDA4MDZkYWY2MDJjZTJhZjUxMGVfWk1POUZzTEo4VDhGMlkzMENMWW5BMURpazRLYWJzYkxfVG9rZW46Ym94azR6b3FpTGxQeGpiOGRhd1ZXRGxLWUNnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

In practice, 按列/按行进行求和比较对角线元素

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZWQ0OTIwZGFjMzM0MDUyYTNjYmYxOWJlYTViODlmMzJfaHZud0RmWXYwUllEMkpkS2x0RHlkdFl2UDIwSHQ0MVhfVG9rZW46Ym94azRhWXFwb0JWekpld2E4bHE3WEJCdm10XzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### To solve AX = B, Iterative Method

spectral radius (largest absolute eigenvalue)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZDlhMDFjYzA1YzljYTQ1NzM2MTY1ZTI2NzU5NmJiYTRfM3ExSHRNem1HRVU2dm9YRW8xb21YeW5BZEk4VEo4bjZfVG9rZW46Ym94azRYNjNFZEM0YVR6TTJseDQ1QmZVYnhiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

##### Derivative & Gradient

Gradient -> Column vectors , from the **definition of derivatives** df/dx -> row vectors (based on that the **vector x is a column vector**)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzdlMmY0NzNiODQ0YTI5ZWQ0OTNmMzNjOWFmNmIwOTRfRVVtbHV3Q0xIbE1pcWxHY0NsYURzMlYyUFl2SlhVUnpfVG9rZW46Ym94azRia1JQazRVVzdtbnpVdGJkVXI5WjZkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

For a Vector -> Divergence is the **trace (sum of the diagonal elements)** of Jacobian

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OTA4Y2RlNWM5MDBlYzZjMTRlMTEzZWMzMzJjYjFkOWRfdWVuS3YxazdyckFueDFsYnlXQ3Blams2YXB2bDhtUklfVG9rZW46Ym94azQxQmh0dndFS0V4QjZvV0hQcG9IMDZnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

The same applies to the 2nd-order Derivatives

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MjU1ODc3MzNlNmMxYWM4ZDJhOTM1NzY5ZWFmY2I4ZGFfbDBWeUppZlFMeDgwV21CVXZrVjlCdmtmaXYzYzhac29fVG9rZW46Ym94azRXTjFnUlI1eXdoR3lTcWVSM3hsWU1jXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Let's go for an example of Derivative (|x|)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzQ0Njg5YjE1ZmM2YTczNWVkNzY3YmNkNWI3MWNiMDVfTHp5enFqNHdVSjhCSHltZkdQTWY4MVM4Ukp0UmVCaUFfVG9rZW46Ym94azRCWENZZFNXY1ExOTNVZzVMcVR5RDRkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



So for the spring -> from the Engergy Function to the Hessian

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MzBmMjZmMmQ5OWMxMjMwNDI0MTdmNjIwYTA0OTEzOGVfbnJTYUlUbGdrOFVxWjRoeGxvWnd6ZWVqam94UjhkTmRfVG9rZW46Ym94azROMWpuVjFQaDdVNHBhWDdCbWl2U1lmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)





##### Quaternion Product = Cross Product - Dot Product

cross product of imaginary part (a, b, c for i, j, k) - dot product of real part (w)

https://www.johndcook.com/blog/2012/02/15/dot-cross-and-quaternion-products/

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=N2QzZTI0ODg2YTgwNWQ4YmRmMmUyOWYzZTY1MTViYjJfVnVZMmlDUnVEUGFVM2lLdlhHeHQ4ZE5CYVJTcExCZUlfVG9rZW46Ym94azRiR29hNlBDQmgzNHZ4a25ua0piMkNkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Integration Method

Explicit Method (t_0)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NjBjOWE0MmMyOWE3ZWM0Zjc3YzBmNTM5ZjkzYWJlYjFfSU5PMEV3aHpvWm9td3FnZkFOQmFxRDZZY0tuRmZRdzdfVG9rZW46Ym94azR5UTZmdkNHU1BJUmlmMU85UUl1TFdoXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Implicit Method (t_1)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZDkyOTg4N2QyMzNlMWEyNWFjNzFhN2ZmMjgxMGMxNTJfdkljZnB6a0JFUTlwTzVUbktmaVdNOVduNGdJMVNTTmFfVG9rZW46Ym94azRjU0xtc0g2UVRhbVJZYWR6TWlEeEtiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Midpoint Method (t_0.5)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YjlmYzU5Y2E1ZjlkMGExMmEwZGE4ZjE4NDRhMmExMTRfOFk2aFBiUDdjMUd0Q2NRTlZ3Q2RWeEFpQnlxNHpBdkNfVG9rZW46Ym94azQ5cVU5VkNLeHFTb1RaeVhSWldVd2ZjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Then for the posiiton update in the end, utilize two mid-point integration -> leapfrog

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NDg1MzE3NWRlMDQ0YzBhZTFhM2FjNjhlNDdjY2RmMjBfN3pGaFltR0VrT1NTNTlTN1N3andnQ0JuOEl0V0NUbndfVG9rZW46Ym94azQ2UXpobEVMNXFmemd2R3BaZkkwR0RSXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Why we use quaterniont for the Rotation

real part and complex part for axis + multiplication of rotations

**norm of quaternion = 1** to make sure it is a rotation

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ODcwNTQwNWY5NGIzN2Q1ZGM3YjUzY2FlNTQwMzc5YjdfaVduSDdOdGRkMjR2eHFWSXdvSER6RVR2Rmhjd1JlektfVG9rZW46Ym94azQ4d2t2N28xaElVVXZxNXlpS1lhTldiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YjczYTU0ODIxOTYzZTVmMWI1MDc2ZDExOGQ5YWZhNDhfcHluZTlIS1VIcGhtWkRhNlZqTDI5TkNMODk4S2V5RmZfVG9rZW46Ym94azRueHJqYU84ajA5Umw5MW1od0JNUmNjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MzZhNzZmMzVhNjg1ZDFkNTQzZTc4NjdjN2Q5ZWVhOTJfcTdFQjdmRm5FVVpnYXQyTUg5SE5TNWdYemZUamRxb0JfVG9rZW46Ym94azRBR3NTMk9EWHczTDNSRWdQQWdKcm5kXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

##### Force Accumulation for Rotation and Translation (Torque and Inertia)

http://www.kwon3d.com/theory/moi/iten.html

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YjE2ZDkwNzIxYWQ3ZGZhOWE2M2RmZmY3ZTc5YjBlYjVfbTBZakVWR0JONnB2dmJEU2ZQVndYY0VReEJGbGhuNm1fVG9rZW46Ym94azQwbXNrYVVjcmVaS3QwV2d4bXFLSGJkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Inertia used for describiing the **resistance** to rotational tendency caused by torque

比如下图，右边旋转对物体影响会更猛一点，因为inertia 都集中在了那个轴附近 (thus, not constant)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzAzNzNiYWMyYWM1YmEwNzNjMWQ0Nzk0ZTQ4NjYwMjdfQTdoT2lTckNmYUpxQ1FoSmN1MHJTb1BrVFlSUlU1c0dfVG9rZW46Ym94azRVbnBadW5aVG9JcFB1TDVvcGw0QjVtXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Rigidbody Simulation Implementation

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NmE0YWQyMjcxMWE1NWM3N2YxY2ExYWFmOWNkY2U5MzFfajA0NnlnZHllVWZVb3AwaUUybTk0STg0cTJMQnhWU3FfVG9rZW46Ym94azR3aXh3M3NuWXNiMm9VWklYRlQxUGpnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MTM0OWZmYTVkYjhjYmUwMzI2ZTZmMmZjMWEwMTJlZTVfbEFQOTI2MmhscWg1N1FwNGxtUmI0NlRseFQySWVNdHlfVG9rZW46Ym94azRVanVZcEY0MmdhWFVQN3FtUUszdW1oXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Particl Collision Detection and Response

Signed Distance Function Example

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZTdiMTdlMDczNjllODIxZTcyNTUzMDQ0YzQ1MGY5YjdfZ2NBVkhEclZacWNlQkhIYWxBalJaM21NWDNEckFmTW1fVG9rZW46Ym94azRhMWhmV1ZuQmIyaW5iOE9vQmR5WGpmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Utilize SDF < 0

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=M2U2ZTMxMGIwYzA2ZWZhYTA0N2NjZDMxOWZhNjA0YmNfNWJUenB2SVZTdnpXSGg2eE5lOENUQWNMb1BpSHR2dTlfVG9rZW46Ym94azRvWE44dkRrV0sxV0xPbUprTFZINWU0XzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MzMxNGE4ZjJmYzdjOWIwNmZhYjRmMzI3MTc3MDcxYmRfNnp5WTJFMWVXdVFWMDNuZmw3emxVYzlrZGhDQVBZV1VfVG9rZW46Ym94azRIRFNoaW1xbnhUQkFFZ1g0WDZPYnVnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

###### 1) Quadratic Penalty Method -> next update

if sdf < 0 -> apply force to compell it + **buffer to avoid penetration**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=M2I4ZjhiZGQ5MzgyMzg5ZGU2ZjQ3ZTcwMjJjMDA2NmRfRFRaeGFjQ1R6c1d5aHN6bUlrOEs1OThPcWFRclhGYktfVG9rZW46Ym94azQ4Skk3VHJkelA0WXRKU0xLckFIQkxkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YTYwMTRhNzQ5MDhmODE1NTUzOTFkM2U2ZWI4MzQ0OTZfN0g0MElwQUJqT1p3ajZOVlc2eFlzMW1tU3M1T1FNc2xfVG9rZW46Ym94azRlb010Y2hhMnRIOXA5aDFLa3N6RWZnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

if k is not a constant factor

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OGExOGU3OTJlZDcwYjM3NGM0YmQ2Nzc2ZWUyMDk1YmVfMzB2Vk9ZczZqS3k1WWF6WnNWVWFHdVhtVUV1UFpGeHRfVG9rZW46Ym94azQ3cGY4NlMyaEV3M2Mybm84MHF4MUljXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



###### 2) Impulse Method -> Change Happens all of a Sudden

change the position and velocity all of a sudden + apply Coulomb's law

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MjdiOWNjOWYyNzdhMWI5ZWNkNjk0NzU3MjI5ZTMwOGNfdEx6MjFiRTlDR3U5RHJBbnlLZ085RFN2enVIenI5b2pfVG9rZW46Ym94azRHenlyRnNHS0M1SHRoWmNUY3dOcnJjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Shape Matching for Rigidbod Dynamics

\1) Move vertices independently 2) Add **rigidity constraints** to keep rigid body

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NGM1Zjc5MjRlZjJjZWI2NTU0NDk4MDJhOWZhNTIyMWNfaG5ieXJQMHlkcDhDaUlnMHFCSm83ak5YbzVrdTZ4RmhfVG9rZW46Ym94azQ4bHRCd3FlckpqVlRvQ210R2E4UDhiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

 Keep the centroid the same as before

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzM0NWJkMTI2OTZhMmFjNTM1MjdkYjMxNjE3MjlkOThfWkdIeUgzOTRuTXN1ZUlQQkpoc3Z5czNkaGtZTFdKdzRfVG9rZW46Ym94azRPQk5tOVpEZ2REQzc2dmVvakdPQXZmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Polar Decomposition -> Rotation and Deformation (Scaling)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YmM1MDk2ZWIwMDlhYjA4NjFlMGQyMjcyYmNhMjllY2JfWE5oSnFkRVVTSW1ETWJGb0xXcEplZm1FTjNONFhnWGJfVG9rZW46Ym94azRVbTUyVkhOYUQ5RHJpSlhzekNlOG5jXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTgxYjMzMWE0ODQzMDYzZTVkZDEwOTNiYmNlOGFjMjBfdkZCSGZiQ1kzdjlnS0xTSG5qVDhXMEc1a20ycnRENFlfVG9rZW46Ym94azQyVzBITFlNVW9mRW5UZTBnMjNHdnJiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

##### Mass-Spring System

###### 1) Spring and Structure

spring force - restoring the spring (negative derivative of energy)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTJjNzIxYzExNTExMDAwNTI2YjVlYzg5NGU1ZTk2YWRfZEpwZHRQeWR5cThhVFhHQ1RtSG9UY2VzYVVVenlXODFfVG9rZW46Ym94azRGeHlVN0xUNjVwT2k0UlhQVG5EdmFoXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

For multiple springs, energe and forces are simply summed up

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NGI4OWUyMjJkN2Q4ZjcxNjQyZGE1ZjkyYzM1ZjAzOWNfSWpMQnVHd09LSEdUWVhZU0dsMmVMakM3WmlWZUhORE5fVG9rZW46Ym94azRJZkhCYkxIVHE0ZnJLVEVYZWFCbHlnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Bending spring is used to avoid bending along some directions / diagonal springs for diagonal stretching and compress

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OTI3OWVlOTlkYWUzZWZjNTFmYTc4MjAxMmJlNmVhZTVfTUpqc2c4TG5JZm96TnB4Y2hpR0l4TnV4elhKVkZiWnhfVG9rZW46Ym94azREWWJ1VVVreXk0Wm1wc2ZMQzJhdjBjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

###### 2) Build mass-spring system from triangular mesh

should consider the the diagonal spring across two triangles (**utilizing neighboring information**)

###### 3) Explicit Integration of Mass-Spring-System (numerical instability, e.g., overshooting)

overshooting happens generally when stiffness is large or the delta time is large.

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ODEwNGY4MjJiZDA3MDlmMWM2MDlmMGQzMDhjZjEzYmRfeEk5OXRiWWFGdXBBTmFRQjZ5cG00UWtMdVlqMFo0czJfVG9rZW46Ym94azQxOGs2aW1uUWU1NzUzdHc4djVBSUpjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

###### 4) Implicit Integration (x[1] from v[1], v[1] from f[1])

Solve the equation by assuming forces are **only influced** by the position

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NDFmNjk1YzVlMTljYTY4ZjQwZmZiMTYwMmY5MjU0NDlfZjMxV2duUEN6eGlOQnBZVXhadUVSRHpkWFVsdXN3Sm1fVG9rZW46Ym94azQ1bFhzZ2tWZnVkUlE2VjZtQXFPYm5iXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

-> Optimize Porblem (argmin)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MDY3MDUzZDgwMDQxNGQ4ODU0OGI5MWQzYWE1MWM2YjJfZzF5RGZocFBqbDZrNU9Gc2FvcGZJUXJ2UVprVGo5N09fVG9rZW46Ym94azR3c3duNTQ3MWFITDlWZEtSdjJiOWlmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Then utilize approximation method to solve: e.g., newton-raphson method + taylor expansion

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=Y2QzZThjNjI2MzMwNTAyNjA3YTdhYmI3YzY3MTQ0ODNfMDBGOVRxSmdldmRTYVNLek8wa0QwUjUzTk8yWDBsUDRfVG9rZW46Ym94azRLSzBhNmJKaVh3Vm5UYmM1VVVlRTVlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

either **local minimum or maximum** method depending on the second derivatives

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MmZmZDJlMGNjZWIwMWU3YjdiMjhlZDRkOTQ5NzVkMjNfZjI2OFNxbUxsM0tPQzIyeUM4ZU9JY2VxQnJIQ2VycjJfVG9rZW46Ym94azQwQ25RYkVIa1owSG5nR2RVOWk3MldlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Compute the Gradient (delta_x) then

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=Y2NmNzdjMDNmOWY4YzcyZjQ2YjExODc2YjU0ZjI0YTdfend3S0JPeWJVRG1FWEJFb2V2ZE5RYlV0c1RoeE5uNm9fVG9rZW46Ym94azRRaWw0amVjWjlXSkxEdW13NlpkUmxkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

###### 5) Spring Hessian

When spring is streatched, H is spd (1 - L/||x|| >0). When spring is compressed, H may be not and then producing **multiple outcomes**.

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NWVmYjRjZGIzMWE1Yzg1NjE2YWNiMTAyOWQ5ODIyM2JfWk5ZUUFZRjJ2clVxdzk1M250UjBRYW1icjZpdTBYZHZfVG9rZW46Ym94azRvRmFpMUE4RjJRejZ1SFZ6RzFNaUJoXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ODQ4YzFiMWQ5ODgzNGIxZmNlMTcwMTUzMTcxYTE1MjNfODV0U0k0d01GZUpFNkFPbnM5TjdpMFdabUNjSm1QMWVfVG9rZW46Ym94azRaaTJPZWFtdkxMQ3hQa0djRlN0dEZjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



delta_t less, H becomes more spd

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OWU3OWIyNzEwYzUwYTcwNDU0MzNmY2QzZjJjNTcwZTNfNEljR3dHVElScGdwaVZ1aE1DbExWTmVPMnI2YXc3OWtfVG9rZW46Ym94azQ5NURPT0UxOEJXT0x6OEpvY0RQbk1kXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Positive Definiteness & Maximum & Minimum

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZmZjZTE3NTliZWJlYWZlZmI2ZGNjMjJjZGY5ODk3YzNfcmUxRFMwZG5vV1NsVDNCVEMxSkt2QmY1UG96TFJndFdfVG9rZW46Ym94azRsT1prZWFCdXlOaDFRUnZibnEzcmhlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

If H is not Spd the **AX = B is hard to solve**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZDFmN2IxYjEyYTgzZmU4OGY2NDMwZmYxNjNiZmU5MzdfMXhoU3NHenRkMkNpQ3ZYU1J1VjJ5WkdkandwNWpDemZfVG9rZW46Ym94azQ4bUZXYzZ4QUFXQ0tKYzE4RWxCcFNaXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

###### 6) Method to solve Linear System (Just a few here)

Jacobi utilize the residual to iterate and diagonal of the A matrix

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YTg3ZGYyMjQ5NmQwOGMyNDllNzYyY2Y0Y2JiMjYxOTZfcDl1TXFMSkFWemthNWtEVFMwaHkxcjQzbjhxT1RtSkZfVG9rZW46Ym94azROMm5rWFZyZ2dHb2E4enRHYWdLWXRkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MjM3NWU5OTMwOTQxNDdiZDFmMDUxZWNmYWUxOGNhODRfQVBHeXJsV1NrWVRSTkZaVXIxRzRaWmhCY1pQaXg1bEZfVG9rZW46Ym94azRGOENzY3Nsc0hDb2dQUjl2SktwZXFnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OWE5MjM2NDUwY2U1NzAzNzM2OTNkOTMzYTY3ODJhYTBfZWRSYzlXZGF3bEVVbnN2RmN4VTdwUVBqVzVSVU1OalVfVG9rZW46Ym94azRwcEdpakpYbFdhTFNZckl2Tk50YXdnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



###### 7) Bending and Locking Issue

Length changes a little for the bending -> use dihedral angle model to solve

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTBjNzkwYTI0N2YxYzJiNzA0ZTQzNWVlOTBmMDZkNWJfb21BVzdnTUJxeTFkZ0FiTnN0dUxId1lrU0wxOEpzeTFfVG9rZW46Ym94azQybk1FSTZUNVV0Wmg0UHFwNFNESlBoXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

the sum of internal forces is zero; don't stretch the bending edge; span of normals

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTBjNzQyY2I2ZTdhM2U0MmQwMjM2ZDVhZGE2NjU2NzFfelN4c1hsZzI5WmxkbDBMZ0dIdHNCbjQzeWVOeFBQbTJfVG9rZW46Ym94azRJMUFJd0FGSlBSZDgycnptRkhia3dxXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=M2NjZjFkNTk4ZjViYzI3MWUzNTUzNTZjZGNjNjA2ZmRfUTh4VDRZZmdxYkEyNDQyOXRWdVJzWk9panZEU2FqODhfVG9rZW46Ym94azRjTjdUUWUxUnVhTjJYWmZScFFYbmxoXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Then obtain the forces as a function with u and f considering the bending angle (theta)

This method is very hard to do implicit integration since the derivative is too hard (**no definition for the energy**)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MTQ4Y2NjNTY0NjVkMzBjYmE4ODlmY2M3Zjc2NWVlMTlfcGM0TElHRXg1SmpPMmFXRGVtdmFzeWtVUWxlanM2bGdfVG9rZW46Ym94azRaZ2xJb1hhdXMwMUxvS1dCQWt4SjBlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Then, there is another bending model which is easy to implement - **quadratic bending model**

quadratic model based on two assumptions: little straching and **starting from** the planer case

Highly related to the **laplacian operator /curvature**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTQ0MjBkNTQ3NzViZDJkNWRmNjMxYTdiNGM5OTVjNDlfV1VTNlBvdUYzTDYxa29KblFObWJlbEVTTjBFU2o5ZTVfVG9rZW46Ym94azQ0OERJRnh1c0Q2TGdwalVTUU1FdDVmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZmFiMmYwYjg5MDVlNjVhNWQyMTEwZjk3ZDM0MGYwMWJfUWVmZjh4Qnh0MDJSejhlUXU2bk53blRTMGhNQkFyaERfVG9rZW46Ym94azRSVHZ6bUN6VGlsYUcxTEtiS1BSczlmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



Locking Issue: 

consider a high stiffness spring, then the bottom-right case is nearly impossible to bend

The fundamental reason is due to a short of **degree of freedom**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NjRkYmZiOGI3NDI2NTQ4OTAwM2I4NjA0NWEyOTY5YWNfdHpBQ2Ywc0pVR1dQSDdySVRmbTQ5RFUzWDdPTnBtVVBfVG9rZW46Ym94azR5clpmZGppdDAyS3NFazVzMTc4UWJnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZWRiNTU2NTQ2YTc3NzM3ZDQwNGYxNTc4MzEwNmI3MzBfWlpBdG5yZnlZaVZ1RjdUY0ZGWmFlVlRhWUhFOE90TnFfVG9rZW46Ym94azRjcnZVNHYybTJKWmhpMWtFakdiM2JlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Constrained Approach

###### Stiffness Issue - incersing stiffness (k) can be problematic sometimes

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MWU3MTcyYjA2MDdiZGFiNzBkMDI5YjkxOWIwMGM3MzlfbG1ZNlVPeFI5ZG5tRzRLdnNWSXBDa3h3Wm9KbldHU0tfVG9rZW46Ym94azRBYUJablFMem9SNjdMT05TRVpyNnVkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

For example, length as a constraint -> projection function (keep the mass center constant)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=Y2E5YTdmMDhlNTM2MWZhZDZkMGY3OGViMWQ1NjQ2MTlfdmIzdVdiS3ZadkxkNnhST01XbUNqUzdnd3VEYVNTVGhfVG9rZW46Ym94azRiMTJKckdSY3FHQzU4UkZNU3lXZEZiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

multiple springs approaching the **constrained conditions (rest length)**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTlmMjUxNGY4ZDAwNzM0YmFmYTI0YWJkNDdjZmQ0YTlfdFdwZERMa0ljS0gwSnY4aWlKUjNzNzhOMTYzaXlNenNfVG9rZW46Ym94azRYMzZDYjNhWFRoTGwzblFBRE1sRThiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YmFlYmIyYmE3MmU4NjBlZjIzYWIxNzM1YTY2NzkyNDNfcHF1NjV3Nld4azl5OVVRQVhEelpjNDJQOTBFbWlWc1JfVG9rZW46Ym94azRBVWxqODd5TmtsRFZWSEM3V3ZQNnBjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Then to avoid the above bias problem, jacobi method is presented. Process **all** the edges related to the one vertex first and update **the average case** then. (n is for the number of updating)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MTQ1MjUxZDkzYzA4ODdhNDZjMzk1ZDVjZGY1NzA4NzRfWUxNMWpBRk1HSWZvWHRlSlpZSTZBOU9hRXo5OEl5bU9fVG9rZW46Ym94azRTYldEM0MwUkxWa2lZNmxZSXN6TDZrXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Position Based Dynamics (PBD)

Highly influced by the **number of iteration and the mesh resolution**

More iterations -> close to the constaints and hence more stiff

**low** mesh resolution -> **close to the constaints become more easily** and hence more stiff

PBD is based on the **projection function** and that's why we mentioned the above approaches for the constrained. PBD is a process to conduct **constraints** on the simulation Very similar to the shape mathing method but here PBD's containts mainly on the **edges of flex objects** while shape matching cares about the **rigidbody** mesh itself.

Generally simulating in real-time when the vertices are less than **1000**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MDYwNDk1YjVkM2UwMjZlYWIxNTJiMWM2M2IwZWFiMDlfQ3JnQ1RDbzI5UVNtclFRVVVqVDJCQ3h6WFhQSUFLN3BfVG9rZW46Ym94azQ5RWhtTmZ0OVdsQ3hqQ0UzTExWc3BkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MzEzMzcwNWVmN2YwYTk0Zjc4ODk2YjVlYmY2ZThjYWZfMUZqaG0wNFhzekV4cjhscDBXSzRJR2ZsWkZ6YjluWlhfVG9rZW46Ym94azQyUmlGeXVwa0lEdkpiN0J5UzlTYlVoXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

To improve the cons of PBD, let's do some improvements

###### 1) Strain Limiting

Use **projection function for correction only** rather than simulation in PBD

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=M2I5YjIwMGUzMDMwNmI0MjNiZmJlNDhjOWQ3ZjY5MmZfbllZbVZtSmE4dHhIcTM1b3RxcU50ZnJDd0hvNDh3M0xfVG9rZW46Ym94azRTaGk4dDlEZmsybnpMNWdDY0RmdkFoXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Spring Strain limit **becomes a range** (relax the constraints)

**Strain is the physics parameter** to describe the stratching change property

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OTk2ZGEyM2VkNTQ3MGIwOThhMTEzMWIxN2MyZTVmNDRfUjN3MUpZZThLVXhWa3lKZGFHODN1VGdRSjVIdWxWcFFfVG9rZW46Ym94azRaYUpFQjBqbW9MRENNckNyS3k5N2pSXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZTk1ODhjOTJhYWFkZmE1YTMxNzQ0ODY3NmI3ZGFmMzhfdGdkUkx4ZWozN0FiVWJaWnkyOW03UGI2VVRzV010eGpfVG9rZW46Ym94azR1SHE2M21pN0dNM1FPQXhZV3V5TVFlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

This relaxation could be 2D, 3D. For example, the following triangle area case

Update the vertice case according to the mass point and newly scaling factor

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YWQ1MGVkZWE0MzQwYTQ2YWViMzMxYWJjZGNjMmNkNzZfbUFmSDR0QVNQSFNvdVlaRWhCTE5pREJvazladm9VdEtfVG9rZW46Ym94azRIMFZNazdENFhFaTRhSjZVd3htY01mXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Why we use strain limiting - a good complement for the large deformation and locking issue

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ODg3ZGQxMzJhY2VjYzExOGIyMDYzNDlmMDcwMDFiYzRfZkhIV1ltVGlpbUs2Y3huUmVnbHp0VEh1YUFWbm5TS0JfVG9rZW46Ym94azRidmJNbjE2UjNTZlhPN2tFd29UcDZiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

##### Projective Dynamics

projective dynamics uses **projection** to define a **quadratic** energy.

For example, for the following triangle mesh, we define the energe by summing the edges

To keep it original the distance between x_inew and x_jnew should be the **rest length**

Utilize the projection of x_inew and x_jnew **in the middle**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTk3OGExN2U2ZDA2MzQwY2I5NDVkZjBlZDRhMmU4NDVfdUtrNU0xT3VXbUdDaHhIWFdvaHFYVEc0aWxnZjlRRVBfVG9rZW46Ym94azQ5aW0zT3JZUEVBRzFObzBsRXFBd3BkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

The main difference with the previous spring model is the **Hessian, making a** **constant Hessian (related to the identity matrix only)**

Diagonal parts store the **information of vertices** and non-diagonal parts store the **edge informaiton. Be carefule about the positive and negative things**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTUyMDJmYjBhNjM3MDhhNWQ3NTMzNTFkY2M4MTllYzZfdHhIM1UyMkdwS3REV2tmcmFUQkNxYWszcUpqTHk5d0FfVG9rZW46Ym94azRrYnI0QTdaVVNib1FOb2dCTGM4MUNVXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Simulation by Projective Dynamics

Solve a linear system with a **constant matrix which could be** **precomuputed** **for all**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YTkwMWU4MGM0OWZkODJmMjJhODZkOGI3ZWQ2MGYzMTBfWDl3dENhNjh5eG5iaWxlNTdtUXlsNEpTMVppbm1SQ25fVG9rZW46Ym94azQyVWVaQzJrOWdPd0lrZkJOM3drWnlkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

###### Why does it work for all (Porjective Dynamics)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZGFjMTI2Y2JlNDcyNzUzZmJiMjk1NjdhZDYwZGM1NTlfT2pIc09ReEVYR0hwNnpVOEN2alFoeHFER1J2ZzBDdEhfVG9rZW46Ym94azRleHZrNm9LazdRN3V1TndmVE1KaVdiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



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

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ODJlZDFhZDAyNzJkMzViNTJiZmY4MDkxOWViNzI2NjFfYmVBZU5UQTZvdUJ3aHJGeXdBZkZSRHFBMUxtMURudHhfVG9rZW46Ym94azR4dWJnWlNRQkZJWE44VDFwcXhQZXVjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Build the linear system with two equations (momentum and updating with lambda)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YWFhZjllZjFhMjBhMjI4MWQ1NzNhZWQ2MWM1OThmZTBfQWRPd2l5elRpS1hOQndPWU9BR2l3MlR3UjRoTzVKZHhfVG9rZW46Ym94azQxY3BJSnpmN2Y3NjRHc2pueTJIcTFlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



Then solve this linear system

Solve the two unknown variables together (**primal-dual, often requiring Positive definite)** or schur complement

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTI0NjIyYjJkMTE2ZDczZjE3NDE2ZWE3YjlkZjMwYTVfTjBnalkxMVE5aTI3T0F4dEQ4bWFRRmV0eGZBeENqVFVfVG9rZW46Ym94azRyYmFNM0JPTEFlOTRRT1RFVlZrUTdkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MGM4YmM5ODM4MWU1OWUzM2E0YTM1ZjEyZTdhNjYyZWNfeW5EUldEeW8yejA0VWVxdnhMa2pEV3BWblA2VW9HQ2FfVG9rZW46Ym94azRjYzNaS0FWTDRVcHAxaTdBcGg5VnVlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

Summary of the Above

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NjNiM2Y0MjFlY2I0Y2U3ODJjNDRlZWVkZWI0MmNkN2JfcDFDV3pLdEJ5NGdVRnR0S2Z6T2xpZmVmMmsyZVVvWTFfVG9rZW46Ym94azRyNllGN1dhUU9rVnhFam1YbEljWFhtXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### The Linear Finite Element Method (FEM)

Deformation could be described in a linear equation. F could also describe the **vector change but F also contains rotation.**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OTI5M2JhZWY3OGZmZmI0OWI0MmQzOTVhZjQ5MjM1MTFfeGpHVWN3cmxreUt0UkFvazdldzZyYzMxMVdOdEFNSVhfVG9rZW46Ym94azQ3anZyVFNrMDhzMmJVVDRPUVBKWndkXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzJkNTdjZWFjNTcwYzEyNjU5Y2RjMmJmZjhhNTZkOGJfZ3o0NWlGTW1WRW9LcWJzVWJ2RUNFUERmdWFpS2wxZ1NfVG9rZW46Ym94azRzeWhLRXJPU2hkaWE5TkZMb2tOeFVmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

**Green Strain** (G) -> a symmtry matrix utilizing svd to cancel extra elements

Strain (a type of tensor) is defined to describe the deformation

G is rotation invariant

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=Nzk1MWExMGFmYWQ5OGRlMDcwNmE0NTJkMjk4YmRhZjhfTXN0OFJiRFFNUnVDVFowTURGWUQ3SklmZEdSdHZ0bG5fVG9rZW46Ym94azRzY0FsTGpoU1JpQTF6N1BDNTF5SUJnXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

###### Strain Energy Density Function

e.g., stvk model (Saint Venant-Kirchhoff Model) + lame parameters in the **reference states**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OTVlNDAzZjRjZmQxMWU2YjM2MDg3OGFlYWJmNzY1NTlfWm1OdlhIVE1HYVRSY2dEWVhDa21CVzhkOGZ3S1VDbHRfVG9rZW46Ym94azRKdUVESTdCSVZ1U1YwQ3M1QUtpMERmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

The we could obtian the forces from the density funciton

The sum of internal forces are zero

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZmQ0Mzk2YWE4YWEyODhlYzgwYjE1MGQzOGM4ZjhhYjVfQ1o1RjRSR1J2YUszUVFQNmpsVTBxWkg2dEdtelRSV2JfVG9rZW46Ym94azRyeFg3UEVhOHdySFBHS2lYeHdsTWRmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MmJjMGNmYzVkNjNlYjk4NWMyNDdmYTBjZDVkMzJiMzRfV1Z0N2pXMVpJcHBXMTExYWJ0OFBKWUdvVW5XNjBwYTVfVG9rZW46Ym94azRsSWZoU1R3cVVnRWI3akNDd21KN0dmXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



##### Finite Volume Method

Let's start with the source of internal deformable forces

Tranction t: internal force per unit length (area)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzY2M2IyMDExMDllMDY4NTc2YTI0ODYzYjE2MzliOWZfZXZXc0ZGQVN5enltV252UktoVnlzY2RuODR4d3VvZ1JfVG9rZW46Ym94azRVOWVxM3Z2WDlQSWVOYU9sdVdCWlZlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

 Normal is constant over edges (3D over areas)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MTQzZTcwZjZiNzVjYzUzOTEzYTYwYThjNjY3MjgxMTVfTzJYSFJ0enBaWDVqeXN6bnJoVGprZDY3TUk2UUxHMllfVG9rZW46Ym94azRNcm1nNWExNFdualhROWVlT29laFZjXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NzdmNDJhMDFkOGY5NDJmNzM3MzBmZGU5NDRmZTgwZTlfQUNxUjRYVXZIWnJob2d4RDJHMzZuTlRsM0pSQjdieVhfVG9rZW46Ym94azRkVGxZdkhTaXhoMlBFaDgxQXNQcXJiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



###### Laplacian Smoothing 

Obtain averaged positoin values by local neighbors (often used to ease numerical instability) 

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=OTkxOWU1Njc2ZmNjMjFjNzAxYzc0MjdhMmU1NWY5YTVfaERkbUl3dExpMFhBQnVkQnZOMkZKVGkzQ1FiV3NBOVlfVG9rZW46Ym94azRlQnN5NnNmYlBiaGcyZlE1Q3VCZU5lXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



###### Difference of Stress (mapping of internel normal) between FEM and FVM

Stress = Traction but the configuration is defined differently in FEM and FVM

Traction could be interpreted as a sort of **force density**

Stress is computed in the reference state in FEM for the energy density

Stress is computed in the deformed state in FVM for the calculation of force.

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MWU4YjQ4NDUyNThjNDU3MTQzYTQxZDQ1OTRjNDgzYzVfcDJxR1ZaOTNDVDBOUXlEalpWVkthVlBENEo3NmN0R1hfVG9rZW46Ym94azRNbHRTUEFSd3N0a0lnVVFqNW5lWmhlXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)



![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=N2ZjNjM0MjU2ZjczN2Y3NmE3MGMxMjUyOTBhZDY0MGZfS2RveDgyaEgzSE9ybDBqS1I4OTl3UVpicHFOenlYejFfVG9rZW46Ym94azQyYWczZkFEQWtyUmhka3lvcERGMnFiXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MWY0NWI2MGVhYzQ2NGE4ZjMwZDM2NzIxZGYzYTA1NWRfRTFaT01adnYzYmZua2JRbUFBbkhUWkVaT1lyS2lWNlNfVG9rZW46Ym94azRIWEljMWdjMnY2QWN3OXllOE4xNUdzXzE2NDUyNzk2MDU6MTY0NTI4MzIwNV9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzA3YWZlYzMwZDg1YTY3NDE5ZDQxMjU3MzgzMTBlMzhfZVQwdzdHT2tsTWVtNUY2ODVqYlc4VjY2YzlxbklCSldfVG9rZW46Ym94azRsVVJVVWtEdHZFbWJRT0tNcmczblJmXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

Then using the above translation to obtain the cauchy stress to calculate the **forces**

To calculate the force, we could use the three above stresses

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YWEzMzVjMWJkN2YwZTYwM2EwMjhjZDYwYTA1MWM1ZTBfbHI2dDBrc0dLTDM3cHFUNEtoV3pzOGY2R1FLWGZtemRfVG9rZW46Ym94azQwYVpkYXdOTmJ2Z2c1VGNyQ2tWS29kXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NGU4YjgwYjg0NjYwNTJmYzViNzAzOTQ4ZjBlNTI1NThfWEVBMVd4WTkwQm1YeHNraVBFTklhWE1vVnZ5aEVLTnZfVG9rZW46Ym94azRkWklRMzlaWlJTSlB3dTFRYWlqczFlXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)



###### FEM/FVM Framework

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NWM1NWY4ZjEyMWU4ZWVlMzBhNWExMWJkMTc1YjgwNTVfaThmWU5SZlJSQWdZbHR3elF3VGlOZlpMbUtDUWpGbmFfVG9rZW46Ym94azR5TEd3eUdDR2VYdUZoY2RQWkhLTGJmXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

In the implementaiton stage, we can apply no force on elements to identify our implementation, which means F becomes identity, G becomes zero and all the internal forces become zero too.

###### Hyperelastic Model (A more general way than stvk)

From energy density -> stress, treat First PK stress P is a function of Deformation gradient (F)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NzdhNzY1OWFiODhhNmFjZTEwZmY4YjM4ODkyN2M0M2Rfam5URnFlVE94ZEY3eUhHazgzNGF1RTZoVExkTmNCOU1fVG9rZW46Ym94azRVMENwakJCa0RvajNGQnk4aWcxNmxnXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)



**Isotropical Material**

Streatch in different positions and direcitons causing the same deformation (如橡胶， rubber)

Then we only wanna how to stretch (scaling) looks like, which is called the principal stretch

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MDMwMTBmYmE3YTUzZDA1YjZlZmU1NjJhYWZmZGVmNDlfVFRhUWpzbFE4VmNuMWx6UXRDUWlOaDdJMEZEUU1BN3BfVG9rZW46Ym94azRaYVBqZzdtaUcyTGc4TnViVWtCb2ljXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YWE5YmQyOWIyNGU4MzM1YjA2MGYyMjlkMjllYWEyZDBfZGw2WUJxRUNHUUdZNEUxbHlNbE1lYWVzQ3g4aW5lbERfVG9rZW46Ym94azQ2SHFJTWhiZlJTandVSlZZZ2ZoN0JmXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

Then, the simulation of hyperelastic model could become the following loop

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=ZjI1YmQ3MGRmMGI3YWJmZGVhMDhiNmNhNDRhN2Q4YTJfMzRiZVk1TzhFRUtWR0hwMjRranlydXJoWExtRTIzZHZfVG9rZW46Ym94azRySjdvQ0hENW9XZ1VsTXFRNjNOdldoXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

###### Limitation of STVK

If deformation is somehow inversed, the stress will become zero and the deformation **cannot be restored to 1**.

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YmJkY2VlNzY1MGI4YTI0YzllNmMyZGZlNjBlMWVmNzNfTWpYUWRQV2ZlUkRFbTJ3M2JVYWZCR2ZXU2xrVnJtc3lfVG9rZW46Ym94azQ0b0ZZamNOdkhHR2JmQ2tmQXpHNzFiXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)



**Poisson Effect**

used to help keep the volume **constant** when the stress is applied (例如拉上去，那么其他部分会瘦身从而保持体积恒定）

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=Y2ZmMmViMzNkY2I5MTVjZjc0MmU0NDAzNGM0MjJlNzhfY2R3cUU4MHpoclhQOTFFU1JWTGpuSFMzNEhGajJJN0NfVG9rZW46Ym94azR2MnlsU1hvTjdBR3VMeGlXQ29yT1NkXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)



###### Summary of FEM and FVM (Derivative and Integral)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NjZmZTNmNWRmNDYzMGNlY2ZiYTVhNDE0MmE3MmMxMDBfSWNiSEJub1FSdjgwWnpQZW5QVEZJa2pZdmdpYWxqbW5fVG9rZW46Ym94azRQZkdYYThkTGRUUG51b2JMRmtVVFpXXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)



##### Non-Linear Optimization

**Descent Gradient Method -> Step Size**

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NTAxZmU5ZDIwMDc1ODg0MTQwZGY1M2ViOTM3NDRkNTRfRXNPcTcyaGxGdFpGVUVYT2JYRGFhOHZQWEljaFc5amNfVG9rZW46Ym94azRaOTJRTms3OWFLVlBHRVBMOVVIaldjXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YzJmZDQ4ZjBiZWZlY2JiMzc3NGYxMzkxMGE1ZDUwYzdfT1lvVGIyTHJ4cmFDRkY4WVNTc09PSU44M1VLbmZzYWNfVG9rZW46Ym94azRNZk9hWmFCbDJSc2pCNDdnbmkxdFRlXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)



**Descend Direction**

Utilize the dot product, if in the same side of the gradient, then the direction is OK

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=YThhNmYyNjQ2MGJhNzViMjlmZDdhNjFlOWIxOGEzZDlfeU4wMzROOHM2OWgxb0RtVVpDb3Jxd2l0Y3RicmY3VklfVG9rZW46Ym94azRyMndCeEhqQlZHaWFPNFBndFJIWW9jXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NDQzYmYyMzRkNDE4ZmZiYTQwYmM4OTUwYzhiYzFmY2VfRjJKV2hwYmtDR0xEb2FRa1hRQ3FkbXdFTk9nOHBBQ2VfVG9rZW46Ym94azRpNmpuTU1scjZ3ZlNET09VRk9sU3lkXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

 So, in the origin, the descent describe how the **integration** could work

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=MWU5N2RkNjFiOTdlZjdhMTBiMzE1ZjdlOTMzODdiMjhfN3BySlpyOU12ZHp3enc2anN6eFpJamRDZ3JmeVBSVlVfVG9rZW46Ym94azQ1d0dWTUc2bXRxQ3UwY0daV256dHNTXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

For example, why we don't newton's method in the often cases is because that the per-iteracion cost is too much since we have to compute the **hessian matrix** each iteration.

![img](https://xiaomi.f.mioffice.cn/space/api/box/stream/download/asynccode/?code=NWY1OTMyYjA1MjE5NzJhNWViMjBjMDgzZDdkM2NjNTdfMVdEMnVEQjM3R2VQdEgzcXBrc0kySWNuREFjM1p2bUdfVG9rZW46Ym94azQ1QXpHMFE3WWlIbXR4bVBCMUFLS2tlXzE2NDUyNzk2MDY6MTY0NTI4MzIwNl9WNA)

