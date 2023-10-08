

Shader "FogMaterialShader"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
        _FadeColor("Fade Color", Color) = (1, 1, 1, 1)
        _FadeLength("Fade Length", Range(0, 100)) = 0.15
	}
	SubShader
	{
        CULL OFF
		Pass
		{
			CGPROGRAM
	
			#pragma vertex vert
            #pragma fragment frag

			#include "UnityCG.cginc"

			uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;

            sampler2D _CameraDepthTexture;
            fixed4 _Color;
            fixed3 _FadeColor;
            float _FadeLength;

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				//float4 color : COLOR;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
                float2 screenuv = v.vertex.xy / _ScreenParams.xy;
                float screenDepth = Linear01Depth(tex2D(_CameraDepthTexture, screenuv));
                float diff = screenDepth - Linear01Depth(v.vertex.z);
                 float intersect = 0;

                if(diff > 0)
                    intersect = 1 - smoothstep(0, _ProjectionParams.w * _FadeLength, diff);

                fixed4 glowColor = fixed4(lerp(_Color.rgb, _FadeColor, pow(intersect, 4)), 1); 
                fixed4 col = _Color * _Color.a + glowColor;
                col.a = _Color.a;  
				return col;
			}
			ENDCG
		}
	}
}