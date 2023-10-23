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
				vertOut o;
                
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
				c.rgb *= c.a;

                return c;

			}
			ENDCG
		}
	}
}