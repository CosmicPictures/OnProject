// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.texture[0]' with 'UNITY_MATRIX_TEXTURE0'
// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECT' with 'tex2D'
// Upgrade NOTE: replaced 'texRECTproj' with 'tex2Dproj'

Shader "Hidden/Old Camera" {
Properties {
	_MainTex ("Base (RGB)", RECT) = "white" {}
	_GrainTex ("Base (RGB)", 2D) = "gray" {}
	_Vignette ("Base (RGB)", 2D) = "gray" {}
	_Scale ("Scale", Float) =1.0
	
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off Fog { Mode off }

CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
#pragma exclude_renderers gles
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

struct v2f { 
	float4 pos	: POSITION;
	float2 uv	: TEXCOORD0;
	float2 uvg	: TEXCOORD1; // grain
	float3 screen : TEXCOORD2;


}; 

uniform sampler2D _MainTex;
uniform sampler2D _GrainTex;
uniform sampler2D _Vignette;
uniform float4 _GrainOffsetScale;
uniform float4 _ScratchOffsetScale;
uniform float _Intensity;
uniform float _Scale;
uniform float4 _GlobalDepthTextureSize;


v2f vert (appdata_img v)
{
	v2f o;
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord);
	o.uvg = v.texcoord.xy * _GrainOffsetScale.zw + _GrainOffsetScale.xy;
		float3x4 mat= float3x4 (
		0.5, 0, 0, 0.5,
		0, 0.5, 0, 0.5,
		0, 0, 0, 1
	);
	float3 coord = mul(mat, o.pos);
	#ifdef SHADER_API_OPENGL
	coord.xy *= _GlobalDepthTextureSize.xy;
	#endif
	o.screen = coord;

	return o;
}

sampler2D _GlobalDepthTexture;

half4 frag (v2f i) : COLOR
{
	half4 col;
float z = tex2Dproj( _GlobalDepthTexture, i.screen ).r;
	float sceneDepth = DECODE_EYEDEPTH( z );

	
	half2 uvr=(i.uv+0.0007*_Scale);
half2 uvb=(i.uv-0.0007*_Scale);
col.ga = tex2D(_MainTex, i.uv).ga;
	col.r = (tex2D(_MainTex, uvr).r*1.05);
		col.b = (tex2D(_MainTex, uvb).b*0.99);



	// sample noise texture and do a signed add
	half3 grain = tex2D(_GrainTex, i.uvg).rgb * 2 - 1;
	half vign = tex2D(_Vignette, i.uv.xy).r;
		grain.rgb *=saturate(sceneDepth*0.1);
	col.rgb += grain*0.2* _Intensity.x ;
	col.rgb *= vign ;



	return col;
}
ENDCG
	}
}

Fallback off

}