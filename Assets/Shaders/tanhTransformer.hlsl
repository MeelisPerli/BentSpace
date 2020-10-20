void tanh3_float(float3 A, float3 B, float3 C, out float3 Out)
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


	// https://www.shadertoy.com/view/WlGSDV
	float c = cos(B.x);
	float s = sin(B.x);
	float4x4 p_xw = float4x4(
		c, 0, 0, s,
		0, 1, 0, 0,
		0, 0, 1, 0,
		-s, 0, 0, c
		);

	c = cos(B.y);
	s = sin(B.y);
	float4x4 p_yw = float4x4(
		1, 0, 0, 0,
		0, c, 0, s,
		0, 0, 1, 0,
		0, -s, 0, c
		);

	c = cos(B.z);
	s = sin(B.z);
	float4x4 p_zw = float4x4(
		1, 0, 0, 0,
		0, 1, 0, 0,
		0, 0, c, s,
		0, 0, -s, c
		);
	

	//Out = mul(v4, mul(p_xw, mul(p_yw, p_zw)));
	// rotation'
	
	c = cos(C.z);
	s = sin(C.z);
	float4x4 p_xy = float4x4(
		c, s, 0, 0,
		-s, c, 0, 0,
		0, 0, 1, 0,
		0, 0, 0, 1
		);
	
	c = cos(C.y);
	s = sin(C.y);
	float4x4 p_xz = float4x4(
		c, 0, s, 0,
		0, 1, 0, 0,
		-s, 0, c, 0,
		0, 0, 0, 1
		);

	c = cos(C.x);
	s = sin(C.x);
	float4x4 p_yz = float4x4(
		1, 0, 0, 0,
		0, c, s, 0,
		0, -s, c, 0,
		0, 0, 0, 1
		);

	// rotation
	v4 = mul(p_xy, v4);
	v4 = mul(p_xz, v4);

	v4 = mul(p_xw, v4);
	v4 = mul(p_yz, v4);


	// location
	v4 = mul(p_yw, v4);
	v4 = mul(p_zw, v4);
	

	/*

	// rotation
	//v4 = mul(v4, p_xy);
	v4 = mul(v4, p_xz);

	v4 = mul(v4, p_xw);
	v4 = mul(v4, p_yz);

	// location
	v4 = mul(v4, p_yw);
	v4 = mul(v4, p_zw);
	*/
	//float4 rotated = mul(mul(mul(mul(mul(mul(v4, p_xy), p_xz), p_yz), p_xw), p_yw), p_zw);
	//float4 rotated = mul(mul(mul(mul(mul(mul(v4, p_xw), p_yw), p_zw), p_xy), p_xz), p_yz); // bad

	float d = 1 - v4.w;
	
	Out = float3((1 + v4.x/d), (1 + v4.y / d), (1 + v4.z / d));
	
}