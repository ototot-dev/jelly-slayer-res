Shader "PROTOFACTOR/Hair/Lambert" 
{
    Properties 
	{
        _Color ("Color", Color) = (.5,.5,.5,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_BumpMap("Normal",2D)="bump"{}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_Emm("Emissive",Range(0,1) ) = 0.5
    }
    SubShader {
        Tags { "Queue"="Transparent+100" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }    
               
        CGPROGRAM
		#pragma target 3.0
        #pragma surface surf Lambert  noambient nolightmap nodirlightmap novertexlights noforwardadd 
       
        struct Input 
		{
            float2 uv_MainTex;
			float IsFacing:VFACE;   
        };     
		

        sampler2D _MainTex;
		sampler2D _BumpMap;

        half _Cutoff;
		float _Emm;
		fixed4 _Color;
		

        void surf (Input IN, inout SurfaceOutput o) 
		{
            fixed4 colorMap = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Normal =  UnpackNormal(tex2D (_BumpMap, IN.uv_MainTex));

			
			clip(colorMap.a - _Cutoff);			
			
			
            o.Albedo = colorMap.rgb;
			
            o.Emission = _Emm * colorMap.rgb;			

			
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
        #pragma surface surf Lambert keepalpha  noambient nolightmap nodirlightmap novertexlights noforwardadd 
       #pragma target 3.0
        struct Input 
		{
            float2 uv_MainTex;
			float IsFacing:VFACE;
        };  
		

        sampler2D _MainTex;
		sampler2D _BumpMap;

        half _Cutoff;
		float _Emm;
		fixed4 _Color;
		
		
        void surf (Input IN, inout SurfaceOutput o) 
		{

            fixed4 colorMap = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Normal =  UnpackNormal(tex2D (_BumpMap, IN.uv_MainTex));
			

			
			clip(-(colorMap.a - _Cutoff));
			
            o.Albedo = colorMap.rgb;
			
            o.Emission = _Emm *  colorMap.rgb;
			
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