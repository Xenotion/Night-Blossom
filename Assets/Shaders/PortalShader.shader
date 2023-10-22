// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "PortalShader"{
Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		_RefractionX("X Refraction", Range(-1,1)) = 0.01
		_RefractionY("Y Refraction", Range(-1,1)) = 0.01

		_DistortionScrollX("X Scroll Speed", Range(-100,100)) = -0.1
	    _DistortionScrollY("Y Scroll Speed", Range(-100,100)) = 0.1
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
	
			float _DistortionScrollX;
			float _DistortionScrollY;
            float _DistortionScaleX;
			float _DistortionScaleY;
			float _RefractionX;
			float _RefractionY;

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
                o.grabCoord.x -= _RefractionX / 2;
				o.grabCoord.y -= _RefractionY / 2;
                o.grabCoord +=  float4(sin(_Time.y *_DistortionScrollX) *_DistortionScaleX, sin(_Time.y *_DistortionScrollY) *_DistortionScaleY, 0, 0);
                o.color = v.color * _Color;

				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
               	float2 distortionScale = float2(_DistortionScaleX, _DistortionScaleY);
				float2 refraction = float2(_RefractionX, _RefractionY);
                // fixed4 c = tex2D(_GrabTexture, v.grabcoord + (refraction * tex2D(_GrabTexture, distortionScale * v.grabcoord))) * v.color;
                 fixed4 c = tex2Dproj(_GrabTexture, v.grabCoord) * v.color;
				c.rgb *= c.a;

                return c;

			}
			ENDCG
		}
	}
}