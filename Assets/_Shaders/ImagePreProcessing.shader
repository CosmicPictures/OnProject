Shader "Custom/ImagePreProcessing"
{
	Properties
	{
		_MainTex("", 2D) = "white" {}
	_Threshold("threshold", float) = 1.0
	}

		SubShader
	{
		ZTest Always Cull Off ZWrite Off Fog{ Mode Off } //Rendering settings

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc" 
		//we include "UnityCG.cginc" to use the appdata_img struct

		struct v2f
	{
		float4 pos : POSITION;
		half2 uv : TEXCOORD0;
	};

	//Our Vertex Shader 
	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
		return o;
	}

	sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders
	uniform float _Threshold;

	//Our Fragment Shader
	fixed4 frag(v2f i) : COLOR
	{
		fixed4 orgCol = tex2D(_MainTex, i.uv); //Get the orginal rendered color 

	float lumen = orgCol.r + orgCol.g + orgCol.b;

	if (i.uv.x < 0.10 || i.uv.x > 0.9 || i.uv.y < 0.10 || i.uv.y > 0.9)
	{
		return fixed4(0.0,0.0,0.0,1.0);
	}

	if ((lumen / 3) > _Threshold * 1.40)
	{
		return fixed4(1.0,0.0,0.0,1.0);
	}

	return fixed4(0.0,0.0,0.0,1.0);
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}