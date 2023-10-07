//https://docs.unity3d.com/Manual/SL-SurfaceShaders.html
Shader "FogShader"
{
	Properties 
    {
       
        _Color ("Material Color", Color) = (0, 0, 0, 1.0)
  
    }
  
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off
        ZWrite On
  
        CGPROGRAM
  
        #pragma surface surf Lambert 
  

        float4 _Color;
  
        struct Input 
        {
            float4 pos;
        };

    
  
        void surf (Input IN, inout SurfaceOutput o) 
        {
            o.Albedo = _Color.rgb;
       
        }
  
       
  
        ENDCG
    }
}