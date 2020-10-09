void tanh3_float(float3 A, float3 B, out float3 Out)
{
	// radius, theta, phi
	// omega between xy plane and shows height
	// phi is on xy plane

	//float d = length(B) - A.x;

	Out = A.x * float3(sin(A.y) * cos(A.z), cos(A.y), sin(A.y) * sin(A.z)); // correct for just transforming spherical -> cartesian
	//Out = A.x * float3(sin(A.y + B.y) * cos(A.z + B.z), cos(A.y + B.y), sin(A.y +B.y) * sin(A.z +B.z));
	//Out = A.x * float3(sin(A.z) * cos(A.y), sin(A.z) * sin(A.y), cos(A.z));
}