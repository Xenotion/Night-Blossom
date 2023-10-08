//https://docs.unity3d.com/Manual/SL-SurfaceShaders.html
Shader "DistanceFog"
{
	Properties {
         _Color ("Material Color", Color) = (0, 0, 0, 1.0)
        _FogColor ("Fog Color", Color) = (0.0, 0.0, 0.0, 1.0)
        _FogStartDistance ("Fog Start Distance", Range(0, 100)) = 20.0
        _FogEndDistance ("Fog End Distance", Range(0, 100)) = 100.0
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert finalcolor:mycolor vertex:myvert
        struct Input {
            float fog;
        };
        float4 _Color ;
        fixed4 _FogColor;
        float _FogStartDistance;
        float _FogEndDistance;
        void myvert (inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            float4 hpos = UnityObjectToClipPos(v.vertex);
            hpos.xy /= hpos.w;
            
            
            float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
  
            float3 toCamera = _WorldSpaceCameraPos.xyz - worldPos.xyz;
      
            float distanceToCamera = length(toCamera);
        
            data.fog = saturate((distanceToCamera - _FogStartDistance) / (_FogEndDistance - _FogStartDistance));
        }
        
        void mycolor (Input IN, SurfaceOutput o, inout fixed4 color)
        {
            fixed3 albedoColor = _Color.rgb;
            fixed3 fogColor = _FogColor.rgb;
            
        
            color.rgb = lerp(albedoColor, fogColor, IN.fog);
        }
        void surf (Input IN, inout SurfaceOutput o) {
          
            o.Albedo = _Color.rgb;
        }
        ENDCG
    } 
    Fallback "Diffuse"

}