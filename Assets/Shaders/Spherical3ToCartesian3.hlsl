void Spherical3ToCartesian3_float(float3 A, float4x4 M, float3 camPos, out float3 Out)
{

	float f1 = A.x;
	float f2 = A.y;
	float f3 = A.z;

	
	// to normal 4D
	float4 v4 = float4(
		cos(f1),
		sin(f1) * cos(f2),
		sin(f1) * sin(f2) * cos(f3),
		sin(f1) * sin(f2) * sin(f3)
		);

	v4 = mul(v4, M);

	float d = 1 - v4.w;
	
	Out = float3(v4.x / d, v4.y / d, v4.z / d);

}