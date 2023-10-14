

Shader "FogMaterialShader"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
        _FadeColor("Fade Color", Color) = (1, 1, 1, 1)
        _FadeLength("Fade Length", Range(0, 1000)) = 0.15
	}
	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On

        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
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
				 // diff between original texture and the current fragment
                float diff = screenDepth - Linear01Depth(v.vertex.z);
                float intersect = 0;

                if(diff > 0)
					// value between 0 and 1
                    intersect = 1 - smoothstep(0, _ProjectionParams.w * _FadeLength, diff);

				float fadeFactor = pow(intersect,4);
                fixed4 fadeColor = fixed4(lerp(_Color.rgb, _FadeColor, fadeFactor), 1); 
                fixed4 col =  fadeColor;
                col.a = _Color.a * (1-fadeFactor); 
				
				return col;
			}
			ENDCG
		}
	}
}