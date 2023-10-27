

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


		_Ks ("Specular Reflection Constant", float) = 1
        _Kd("Diffuse Reflection Constant", float) =1
        _Ka("Ambient Reflection Constant", float) =1
        _Alpha("Shininess Constant", float) =1 
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
			#include "Lighting.cginc"

			uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;

            sampler2D _CameraDepthTexture;
            fixed4 _Color;
            fixed3 _FadeColor;
            float _FadeLength;
			float _TransparentLength;
			float _K;
			float _C;

			uniform float _Ks;
            uniform float _Kd;
            uniform float _Ka;
            uniform float _Alpha;


			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;
                // apply the material color
                v.color.rgb = _Color;

                // Gouraud shading, adapted from workshop 9 code

                // Convert Vertex position and corresponding normal into world coords.
				// Note that we have to multiply the normal by the transposed inverse of the world 
				// transformation matrix (for cases where we have non-uniform scaling; we also don't
				// care about the "fourth" dimension, because translations don't affect the normal) 
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

				// Calculate ambient RGB intensities
			
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * _Ka;

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float fAtt = 1; // not sure what this is 
	
				float3 L = normalize(_WorldSpaceLightPos0.xyz - worldVertex.xyz); 
                float LdotN = dot(L, worldNormal.xyz);
                float3 dif = fAtt * _LightColor0.rgb * _Kd * v.color.rgb * saturate(LdotN); 
               
				// Calculate specular reflections
				
				float3 V = normalize(_WorldSpaceCameraPos - worldVertex.xyz );

                // direction of the reflection
                // calculated using the formula from: https://math.stackexchange.com/questions/13261/how-to-get-a-reflection-vector
				// note: this is reversed to make the light direction look correct inside the fog
				float3 R = -1 * normalize(L - 2 * dot(L, worldNormal.xyz) * worldNormal.xyz);

                // kinda cool effect: use vector to camera instead of to light
                //float3 R = normalize(V - 2 * dot(V, worldNormal.xyz) * worldNormal.xyz);
				float3 spe = fAtt * _LightColor0.rgb * _Ks * pow(saturate(dot(V, R)), _Alpha); 

				// Combine Phong illumination model components
				o.color.rgb = amb.rgb + dif.rgb + spe.rgb;
				o.color.a = v.color.a;


                o.vertex = UnityObjectToClipPos(v.vertex);


                // Pass texture coordinates
               //   o.uv = v.uv;

               

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
                fixed4 fadeColor = fixed4(lerp(v.color.rgb, _FadeColor, fadeFactor), 1); 
                fixed4 col =  v.color * v.color.a + fadeColor;
				// change transparency 
                col.a = (1-transparentIntersect); 
				
				return col;
			}
			ENDCG
		}
	}
}