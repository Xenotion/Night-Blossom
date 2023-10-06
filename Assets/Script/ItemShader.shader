Shader "ItemShader"
{
    Properties
    {
        //_PointLightColor ("Point Light Color", Color) = (0, 0, 0)
        //_PointLightPosition ("Point Light Position", Vector) = (0.0, 0.0, 0.0)
        _Color ("Material Color", Color) = (0, 0, 0, 1.0)
        _Ks ("Specular Reflection Constant", float) = 1
        _Kd("Diffuse Reflection Constant", float) =1
        _Ka("Ambient Reflection Constant", float) =1
        _Alpha("Shininess Constant", float) =1 
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
            #include "Lighting.cginc"
            
            // uniform float3 _PointLightColor;
            // uniform float3 _PointLightPosition;
            uniform float4 _Color;
            uniform float _Ks;
            uniform float _Kd;
            uniform float _Ka;
            uniform float _Alpha;


            struct vertIn
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float4 color : COLOR;
                // texture not needed?
                //float2 uv : TEXCOORD0; 
            };

            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                //float2 uv : TEXCOORD0; 
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
				float3 V = normalize(_WorldSpaceCameraPos - worldVertex.xyz);

                // direction of the reflection
                // calculated using the formula from: https://math.stackexchange.com/questions/13261/how-to-get-a-reflection-vector
				//float3 R = normalize(L - 2 * dot(L, worldNormal.xyz) * worldNormal.xyz);

                // glowing effect: use vector to camera instead of to light
                float3 R = normalize(V - 2 * dot(V, worldNormal.xyz) * worldNormal.xyz);
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
                return v.color;
            }
            ENDCG
        }
    }
}
