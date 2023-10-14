

Shader "FogMaterialShader"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		// transparency of fade color has no effect
        _FadeColor("Fade Color", Color) = (1, 1, 1, 1)
        _FadeLength("Fade Length", Range(0, 200)) = 0.15
		_TransparentLength("Transparent Length",  Range(0, 200) ) = 10

		// affects how the effects changes over distance
		// function = 2^ k(d-c)
		_K("Distance Function - k", Range(0,20)) = 1
		_C("Distance Function - c", Range(-5,5)) = 7
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
			float _TransparentLength;
			float _K;
			float _C;

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
				float transparentIntersect = 0;
                if(diff > 0){
					// values between 0 and 1
                    intersect = 1 - smoothstep(0, _ProjectionParams.w * _FadeLength, diff);
					transparentIntersect = 1 - smoothstep(0, _ProjectionParams.w * _TransparentLength, diff);

				}
					
				// values to scale color / transparency based on some function of distance
				float fadeFactor = pow(2, _K * (intersect- _C));
				float transparentFactor = pow(2, _K * (transparentIntersect- _C));

				// change color
                fixed4 fadeColor = fixed4(lerp(_Color.rgb, _FadeColor, fadeFactor), 1); 
                fixed4 col =  _Color * _Color.a + fadeColor;
				// change transparency 
                col.a = (1-transparentIntersect); 
				
				return col;
			}
			ENDCG
		}
	}
}