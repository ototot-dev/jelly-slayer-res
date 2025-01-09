Shader "PROTOFACTOR/Hair/Lambert_Anistropic" 
{
    Properties 
	{
        _Color ("Color", Color) = (.5,.5,.5,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_BumpMap("Normal",2D)="bump"{}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_Emm("Emissive",Range(0,1) ) = 0.5
		_AnisoTex ("Anisotropic Direction (RGB)", 2D) = "bump" {}
		_AnisoOffset ("Anisotropic Highlight Offset", Range(-1,1)) = -0.2
		_SpecularTex ("Specular (R) Gloss (G) Anisotropic Mask (B)", 2D) = "gray" {}
    }
	
    SubShader 
	{
        Tags { "Queue"="Transparent+100" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }    

        CGPROGRAM
		#pragma target 3.0
        #pragma surface surf Aniso  noambient nolightmap nodirlightmap novertexlights noforwardadd 
		
		struct SurfaceOutputAniso
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed4 AnisoDir;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
		};
		
		sampler2D  _AnisoTex;
		float _AnisoOffset;
		sampler2D _SpecularTex;
        sampler2D _MainTex;
		sampler2D _BumpMap;

        half _Cutoff;
		float _Emm;
		fixed4 _Color;
		
		inline fixed4 LightingAniso(SurfaceOutputAniso s, fixed3 lightDir, fixed3 viewDir, fixed atten)
		{
			fixed3 h = normalize(normalize(lightDir) + normalize(viewDir));
			float NdotL = saturate(dot(s.Normal, lightDir));
 
			fixed HdotA = dot(normalize(s.Normal + s.AnisoDir.rgb), h);
			float aniso = max(0, sin(radians((HdotA + _AnisoOffset) * 180)));
 
			float spec = saturate(dot(s.Normal, h));
			spec = saturate(pow(lerp(spec, aniso, s.AnisoDir.a), s.Gloss * 128) * s.Specular);
 
			fixed4 c;
			c.rgb = ((s.Albedo * _LightColor0.rgb * NdotL) + (_LightColor0.rgb * spec)) * (atten );
			c.a = s.Alpha;
			//clip(s.Alpha - _Cutoff);
			return c;
		}
		
		

		
        struct Input 
		{
            float2 uv_MainTex;
			float IsFacing:VFACE;  
			float2 uv_AnisoTex;			
        };     
		

		

        void surf (Input IN, inout SurfaceOutputAniso o) 
		{
            fixed4 colorMap = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Normal =  UnpackNormal(tex2D (_BumpMap, IN.uv_MainTex));

			fixed3 spec = tex2D(_SpecularTex, IN.uv_MainTex).rgb;
			o.Specular = spec.r;
			o.Gloss = spec.g;

			o.AnisoDir = fixed4(tex2D(_AnisoTex, IN.uv_AnisoTex).rgb, spec.b);
			
			clip(colorMap.a - _Cutoff);			
			
			
            o.Albedo = colorMap.rgb;
			
            o.Emission = _Emm * colorMap.rgb;			


			
            o.Alpha = colorMap.a;
			
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
        #pragma surface surf Aniso keepalpha  noambient nolightmap nodirlightmap novertexlights noforwardadd 
       #pragma target 3.0
	   
		struct SurfaceOutputAniso
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed4 AnisoDir;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
		};
		
		sampler2D  _AnisoTex;
		float _AnisoOffset;
		sampler2D _SpecularTex;
        sampler2D _MainTex;
		sampler2D _BumpMap;

        half _Cutoff;
		float _Emm;
		fixed4 _Color;
		
        struct Input 
		{
            float2 uv_MainTex;
			float IsFacing:VFACE;
			float2 uv_AnisoTex;
        };  

		
		inline fixed4 LightingAniso(SurfaceOutputAniso s, fixed3 lightDir, fixed3 viewDir, fixed atten)
		{
			fixed3 h = normalize(normalize(lightDir) + normalize(viewDir));
			float NdotL = saturate(dot(s.Normal, lightDir));
 
			fixed HdotA = dot(normalize(s.Normal + s.AnisoDir.rgb), h);
			float aniso = max(0, sin(radians((HdotA + _AnisoOffset) * 180)));
 
			float spec = saturate(dot(s.Normal, h));
			spec = saturate(pow(lerp(spec, aniso, s.AnisoDir.a), s.Gloss * 128) * s.Specular);
 
			fixed4 c;
			c.rgb = ((s.Albedo * _LightColor0.rgb * NdotL) + (_LightColor0.rgb * spec)) * (atten);
			c.a = s.Alpha;
			//clip(s.Alpha - _Cutoff);
			return c;
		}
		
        void surf (Input IN, inout SurfaceOutputAniso o) 
		{

            fixed4 colorMap = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Normal =  UnpackNormal(tex2D (_BumpMap, IN.uv_MainTex));
			
			fixed3 spec = tex2D(_SpecularTex, IN.uv_MainTex).rgb;
			o.Specular = spec.r;
			o.Gloss = spec.g;
			
			o.AnisoDir = fixed4(tex2D(_AnisoTex, IN.uv_AnisoTex).rgb, spec.b);
			
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