Shader "PROTOFACTOR/Hair/Standard" 
{
    Properties 
	{
        _Color ("Color", Color) = (.5,.5,.5,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_BumpMap("Normal",2D)="bump"{}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_Emm("Emissive",Range(0,1) ) = 0.5
        _OcclusionMap("Occlusion", 2D) = "white" {}
    }
    SubShader {
        Tags { "Queue"="Transparent+100" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }    
               
        CGPROGRAM
		#pragma target 3.0
        #pragma surface surf Standard  noambient nolightmap nodirlightmap novertexlights noforwardadd 
       
        struct Input 
		{
            float2 uv_MainTex;
			float IsFacing:VFACE;   
        };     
		
		sampler2D   _MetallicGlossMap;
        sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _OcclusionMap;
        half _Cutoff;
		float _Emm;
        half _Glossiness;
        half _Metallic;
		fixed4 _Color;
		

        void surf (Input IN, inout SurfaceOutputStandard o) 
		{
            fixed4 colorMap = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Normal =  UnpackNormal(tex2D (_BumpMap, IN.uv_MainTex));
			
			//o.Occlusion = tex2D (_OcclusionMap, IN.uv_MainTex).g;
			
			half4 mg = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			
			clip(colorMap.a - _Cutoff);			
			
			
            o.Albedo = colorMap.rgb;
			
            o.Emission = _Emm * colorMap.rgb;			
			
            o.Metallic = _Metallic * mg.r;
			
			o.Smoothness =  _Glossiness * (1-  mg.a);
			
            o.Alpha = 0.0;
			
			if (IN.IsFacing < 0.5)
			{
				o.Normal *= -1.0;
			}
        }
        ENDCG
       
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
		Tags { "Queue"="Transparent+100" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }    

        CGPROGRAM
        #pragma surface surf Standard keepalpha  noambient nolightmap nodirlightmap novertexlights noforwardadd 
       #pragma target 3.0
        struct Input 
		{
            float2 uv_MainTex;
			float IsFacing:VFACE;
        };  
		
		sampler2D   _MetallicGlossMap;
        sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _OcclusionMap;
        half _Cutoff;
		float _Emm;
        half _Glossiness;
        half _Metallic;
		fixed4 _Color;
		
		
        void surf (Input IN, inout SurfaceOutputStandard o) 
		{

            fixed4 colorMap = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Normal =  UnpackNormal(tex2D (_BumpMap, IN.uv_MainTex));
			
			//o.Occlusion = tex2D (_OcclusionMap, IN.uv_MainTex).g;
			
			half4 mg = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			
			clip(-(colorMap.a - _Cutoff));
			
            o.Albedo = colorMap.rgb;
			
            o.Emission = _Emm *  colorMap.rgb;
			
            o.Metallic = _Metallic * mg.r;
			
            o.Smoothness =  _Glossiness * (1-  mg.a);
			
            o.Alpha = colorMap.a;
			
			if (IN.IsFacing < 0.5)
			{
				o.Normal *= -1.0;
			}				
		}
        ENDCG
    }
    Fallback "Transparent/Cutout/Diffuse"
}