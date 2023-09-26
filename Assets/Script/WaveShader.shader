//UNITY_SHADER_NO_UPGRADE
// This shader file is adpated from the code provided in the Week 7 & Week 8 workshops
Shader "Unlit/WaveShader"
{
	Properties
	{
    	_Color ("Material Color", Color) = (0, 0, 0, 1.0)
	}
	SubShader
	{
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			uniform float4 _Color;
	
		

			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				float4 displacement = float4(sin( _Time.y *4.0f + v.vertex.y*1.5f) *0.04f -0.025f, sin( _Time.y *4.0f + v.vertex.x*1.5f) *0.01f -0.01f, sin( _Time.y *4.0f + v.vertex.y*1.5f)*0.01f, 0.0f);
				v.vertex += displacement;
				vertOut o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // Default

				return o;
			}
			
			// Implementation of the fragment (pixel) shader
			fixed4 frag(vertOut v) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}
