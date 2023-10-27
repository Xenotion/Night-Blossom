// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "PortalShader"{
Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		
		_DistortionSpeedX("X Distortion Speed", Range(-10,10)) = -0.1
	    _DistortionSpeedY("Y Distortion Speed", Range(-10,10)) = 0.1
		

	    _DistortionFreqX("X Distortion Frequency", Range(-10,10)) = 0.1
	    _DistortionFreqY("Y Distortion Frequency", Range(-10,10)) = 0.1

		_DistortionScaleX("X Scale", float) = 1.0
		_DistortionScaleY("Y Scale", float) = 1.0

		_Lx("X width", float) = 4.0
		_Ly("Y width", float) = 4.0
		_nx("X nodes", float) = 4.0
		_ny("Y nodes", float) = 4.0
	}
	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        CULL OFF
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        /// https://docs.unity3d.com/Manual/SL-GrabPass.html
        GrabPass { 

        }

		Pass
		{
			CGPROGRAM
	
			#pragma vertex vert
            #pragma fragment frag

			#include "UnityCG.cginc"

			fixed4 _Color;
	
			float _DistortionSpeedX;
			float _DistortionSpeedY;

			float _DistortionFreqX;
			float _DistortionFreqY;
		
            float _DistortionScaleX;
			float _DistortionScaleY;


			float _Lx;
			float _Ly;
			float _nx;
			float _ny;
			

            sampler2D _GrabTexture;

			struct vertIn
			{
				float4 vertex : POSITION;
				//float4 normal : NORMAL;
				float4 color : COLOR;
                
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
                float4 grabCoord : TEXCOORD1;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				float PI = 3.14f;
	
				vertOut o;
				// wave function from :https://chem.libretexts.org/Bookshelves/Physical_and_Theoretical_Chemistry_Textbook_Maps/Physical_Chemistry_(LibreTexts)/03%3A_The_Schrodinger_Equation_and_a_Particle_in_a_Box/3.09%3A_A_Particle_in_a_Three-Dimensional_Box
				// z is x, x is y, y is z
				//float waveFunction = sqrt(2/_Lx)*sin((_nx * PI * v.vertex.z)/_Lx) * sqrt(2/_Ly)*sin((_ny * PI * v.vertex.x)/_Ly);
				//float waveFunction = sin(v.vertex.x*PI/4);


				// drop wave from https://al-roomi.org/benchmarks/unconstrained/2-dimensions/54-drop-wave-function
				float adjustedX = v.vertex.z * 5.12 / _Lx;
				float adjustedY = v.vertex.x * 5.12 / _Ly;
				float waveFunction = -0.001 * (1 + cos(12 * sqrt(adjustedX * adjustedX + adjustedY * adjustedY))/ 0.5f * (adjustedX * adjustedX + adjustedY * adjustedY) + 2);

                float4 displacement = float4(0.0f, 0.0f, 0.0f, 0.0f);
				displacement.y =  sin(_Time.y) * waveFunction;
                v.vertex.xyz += displacement.xyz;
				o.vertex = UnityObjectToClipPos(v.vertex);
			
                o.grabCoord =  ComputeGrabScreenPos(o.vertex);
                // o.grabcoord = (float2(o.vertex.x, o.vertex.y * -1 ) +o.vertex.w) * 0.5;
              
                o.grabCoord +=  float4(sin(_Time.y *_DistortionSpeedX + o.grabCoord.y*_DistortionFreqX) *_DistortionScaleX,
					 					sin(_Time.y *_DistortionSpeedY + o.grabCoord.x*_DistortionFreqY) *_DistortionScaleY,
					  					0, 0);

				
                o.color = v.color * _Color;

				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
               	float2 distortionScale = float2(_DistortionScaleX, _DistortionScaleY);
			
                // fixed4 c = tex2D(_GrabTexture, v.grabcoord + (refraction * tex2D(_GrabTexture, distortionScale * v.grabcoord))) * v.color;
                fixed4 c = tex2Dproj(_GrabTexture, v.grabCoord) * v.color;
				//fixed4 c = v.color;
				c.rgb *= c.a;

                return c;

			}
			ENDCG
		}
	}
}