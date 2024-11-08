Shader "PolySoft/StandardMetallicSpecialMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		[HideInInspector] _Mask1("Mask 1", Color) = (0,1,1,0)   
		[HideInInspector] _Mask2("Mask 2", Color) = (1,0,1,0) 
		[HideInInspector] _Mask3("Mask 3", Color) = (1,1,0,0) 
		_Color1 ("Color 1", Color) = (1,1,1,1)
		_Color2 ("Color 2", Color) = (1,1,1,1)
		_Color3 ("Color 3", Color) = (1,1,1,1)
		_EmissionColor1("Emission 1 alpha intensity", Color) = (1,1,1,0)
		_EmissionColor2("Emission 2 alpha intensity", Color) = (1,1,1,0)
		_EmissionColor3("Emission 3 alpha intensity", Color) = (1,1,1,0)
		_Metallic1 ("Metallic 1", Range(0,1)) = 0.0 
		_Metallic2 ("Metallic 2", Range(0,1)) = 0.0 
		_Metallic3 ("Metallic 3", Range(0,1)) = 0.0 
		_Gloss1 ("Gloss 1", Range(0,1)) = 0.0 
		_Gloss2 ("Gloss 2", Range(0,1)) = 0.0 
		_Gloss3 ("Gloss 3", Range(0,1)) = 0.0 
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 color:COLOR;
        };
        fixed4 _Color;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;
		fixed4 _EmissionColor1;
		fixed4 _EmissionColor2;
		fixed4 _EmissionColor3;
		fixed4 _Mask1;
		fixed4 _Mask2;
		fixed4 _Mask3;
		fixed _Metallic1;
		fixed _Metallic2;
		fixed _Metallic3;
		fixed _Gloss1;
		fixed _Gloss2;
		fixed _Gloss3;
        UNITY_INSTANCING_BUFFER_START(Props)

        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			half3 c1 = _Mask1.rgb-c.rgb;
			half3 c2 = _Mask2.rgb-c.rgb;
			half3 c3 = _Mask3.rgb-c.rgb;

			half m1 = length(c1) == 0 ? 1 : 0;
			half m2 = length(c2) == 0 ? 1 : 0;
			half m3 = length(c3) == 0 ? 1 : 0;

			c.rgb = lerp(c.rgb, _Color1, m1);
			c.rgb = lerp(c.rgb, _Color2, m2);
			c.rgb = lerp(c.rgb, _Color3, m3);
			c.rgb = c.rgb * (1 - IN.color.g) + IN.color.g * (_EmissionColor1 * m2 + _EmissionColor2 * m1 + _EmissionColor3 * m3);
            o.Albedo = c.rgb  * _Color;

            o.Metallic = (m1 * _Metallic1 + m2 * _Metallic2 + m3 * _Metallic3) * (1 - IN.color.g);
            o.Smoothness = (m1 * _Gloss1 + m2 * _Gloss2 + m3 * _Gloss3) * (1 - IN.color.g);
			o.Emission = IN.color.g * (_EmissionColor1 * 10 * m2 * _EmissionColor1.a + _EmissionColor2 * 10 * m1 * _EmissionColor2.a + _EmissionColor3 * 10 * m3 * _EmissionColor3.a);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
