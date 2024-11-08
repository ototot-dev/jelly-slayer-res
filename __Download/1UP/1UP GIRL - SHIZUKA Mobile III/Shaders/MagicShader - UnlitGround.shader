Shader "1UP/Magic Shader/Unlit Ground"
{
	Properties
	{
		[HideInInspector] _DIFFUSE("FolderTag", float) = 1.0
		[Toggle(_DiffuseEnable)] _DiffuseEnable("Enable DIFFUSE Function", Float) = 1.0
		_DiffuseColor("Diffuse Color", Color) = (1,1,1,1)
		_DiffuseIntensity("Diffuse Instensity", Range(0.0, 1.0)) = 0.0
		_DiffuseColorLevel("Diffuse Color Level", Range(0.0, 2.0)) = 0.0
		_DiffuseUvTex("Diffuse Texture", 2D) = "white" {}

		[HideInInspector] _UNLITSHADOW("FolderTag", float) = 1.0
		[Toggle(_ShadowEnable)] _ShadowEnable("Enable UNLIT SHADOW Function", Float) = 1.0
		_ShadowColor("Shadow Color", Color) = (0, 0, 0, 1)
		_ShadowIntensity("Shadow Intensity", Range(0.0, 1.0)) = 1.0
		_ShadowBlur("Shadow Blur", Range(0.0, 10.0)) = 1.0
		_ShadowNoise("Shadow Noise", Range(0.0, 30.0)) = 1.0
		[Toggle(_ShadowCutout)] _ShadowCutout("Enable Cutout Mode", Float) = 1.0

		 [HideInInspector]_ShadowOffset0("Hide", Range(0, 1.0)) = 0.0
		 [HideInInspector]_ShadowOffset1("Hide", Range(0, 1.0)) = 0.0
		 [HideInInspector]_ShadowOffset2("Hide", Range(0, 1.0)) = 0.0
		// [Toggle(_ShadowOnly)] _ShadowOnly("Debug Indirect Shadow", Float) = 0

		[HideInInspector] _ShadowTex0("Hide", 2D) = "white" {}
		[HideInInspector] _ShadowTex1("Hide", 2D) = "white" {}
		[HideInInspector] _ShadowTex2("Hide", 2D) = "white" {}
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass 
		{
            // Cull Back

			CGPROGRAM
			#pragma shader_feature _DiffuseEnable
			#pragma shader_feature _ShadowEnable
			#pragma shader_feature _ShadowCutout

			#include "UnityCG.cginc"
			#pragma vertex vert  
			#pragma fragment frag 

			// Materials Params
			float4 _DiffuseColor;
			float _DiffuseIntensity;
			float _DiffuseColorLevel;
			sampler2D _DiffuseUvTex;
			float _DiffuseAlphaCutoff;


			// Shadow Params
			sampler2D _ShadowTex0;
			float4x4 _ShadowProj0;
			float _ShadowOffset0;
			sampler2D _ShadowTex1;
			float4x4 _ShadowProj1;
			float _ShadowOffset1;
			sampler2D _ShadowTex2;
			float4x4 _ShadowProj2;
			float _ShadowOffset2;

			float4 _ShadowColor;
			float _ShadowIntensity;
			float _ShadowBlur;
			float _ShadowNoise;

			#define PI          3.14159265359

			#define DL 2.399963229728653  // PI * ( 3.0 - sqrt( 5.0 ) )
			#define EULER 2.718281828459045

			// user variables
			#define noiseAmount 0.4

			struct vertexInput {
				float4 vertex : POSITION;
                float4 uv: TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct vertexOutput {
				float4 pos			: SV_POSITION;
				float4 worldPos		: TEXCOORD0;
                float4 uv			: TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
			};

            float2 rand(float2 coord) {
				float2 noise;
				float nx = dot(coord, float2(12.9898, 78.233));
				float ny = dot(coord, float2(12.0909, 78.233));
				noise = clamp(frac(43758.5453 * sin(float2(nx, ny))), 0.0, 1.0);

				return (noise * 2.0 - 1.0) * noiseAmount;
			}

			float getDepth(float depth, float posZ) {
				if ((posZ - depth) < -0.0002) return 1.0;
				return 0.0;
			}

			float4 getSelfShadow(float4 worldPos, float2 noise,
				float4x4 shadowProj, sampler2D shadowMap, float shadowOffset) {
				float4 lndcpos = mul(shadowProj, worldPos);
				lndcpos.xyz = lndcpos.xyz / lndcpos.w;

				// calc depth here
				float4 posOffset = float4(_ShadowNoise *
					float3(noise.x, noise.x, noise.y) * 
					float3(0.001, 0.002, 0.003), 0.0);
				lndcpos = mul(shadowProj, worldPos + posOffset);
				lndcpos.xyz = lndcpos.xyz / lndcpos.w;
				float3 luvpos = lndcpos * 0.5 + 0.5;
				float2 uvOffset = float2(_ShadowBlur, _ShadowBlur) / 512.0;
				float texDepth = tex2D(shadowMap, luvpos.xy).r;
                float2 xoff = float2(1.0, 0.0) * uvOffset;
                float2 yoff = float2(0.0, 1.0) * uvOffset;
				float texDepth1 = tex2D(shadowMap, luvpos.xy + xoff);
				float texDepth2 = tex2D(shadowMap, luvpos.xy - xoff);
				float texDepth3 = tex2D(shadowMap, luvpos.xy + yoff);
				float texDepth4 = tex2D(shadowMap, luvpos.xy - yoff);

				float texColor = getDepth(texDepth, lndcpos.z);
				float texColor1 = getDepth(texDepth1, lndcpos.z);
				float texColor2 = getDepth(texDepth2, lndcpos.z);
				float texColor3 = getDepth(texDepth3, lndcpos.z);
				float texColor4 = getDepth(texDepth4, lndcpos.z);
				texColor = 1.0 - (texColor + texColor1 + texColor2 + texColor3 + texColor4) / 5.0;
				return lerp(1.0 - shadowOffset, float4(1.0, 1.0, 1.0, 1.0), texColor);
			}

			vertexOutput vert(vertexInput i) {
				vertexOutput o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
				o.worldPos = mul(unity_ObjectToWorld, i.vertex);
				return o;
			}

			float4 frag(vertexOutput i) : COLOR {
                float2 screenCoord = i.pos.xy / _ScreenParams.xy;
				float2 noise = rand(screenCoord);
				noise.x = saturate(noise.x);

				float4 shadowColor = float4(1.0, 1.0, 1.0, 1.0);
				float4 shadowColor0 = float4(1.0, 1.0, 1.0, 1.0);
				float4 shadowColor1 = float4(1.0, 1.0, 1.0, 1.0);
				float4 shadowColor2 = float4(1.0, 1.0, 1.0, 1.0);
                // noise = float2(0, 0);
				#ifdef _ShadowEnable
					if (_ShadowOffset0 > 0.001) {
						shadowColor0 = getSelfShadow(i.worldPos, noise, 
						_ShadowProj0, _ShadowTex0, _ShadowOffset0);
					}
					if (_ShadowOffset1 > 0.001) {
						shadowColor1 = getSelfShadow(i.worldPos, noise, 
						_ShadowProj1, _ShadowTex1, _ShadowOffset1);
					}
					if (_ShadowOffset2 > 0.001) {
						shadowColor2 = getSelfShadow(i.worldPos, noise, 
						_ShadowProj2, _ShadowTex2, _ShadowOffset2);
					}
					shadowColor = 
						min(min(shadowColor0, shadowColor1), shadowColor2);
					float shadowValue = 1.0 - shadowColor.r;
					shadowColor.rgb = 
						_ShadowColor.rgb * (shadowValue) + shadowColor.rgb;
                    // shadowColor.rgb += _ShadowIntensity;
				#endif
                
				float4 diffuseColor = float4(1.0, 1.0, 1.0, 1.0);
				#ifdef _DiffuseEnable
					float4 diffuseTexCol = tex2D(_DiffuseUvTex, i.uv.xy);
					diffuseTexCol = float4(lerp(diffuseTexCol.rgb,
						_DiffuseColor.rgb * diffuseTexCol.rgb, 
						_DiffuseColor.a), 1.0);
                    diffuseTexCol *= (1.0 + _DiffuseColorLevel);
                    diffuseColor = lerp(diffuseColor, diffuseTexCol, _DiffuseIntensity);
				#endif

				float4 color = diffuseColor;
                color.rgb = lerp(color.rgb, shadowColor.rgb, _ShadowIntensity);
                #ifdef _ShadowCutout
				color = _ShadowColor;
                color.a = (1.0 - shadowColor.a) * _ShadowIntensity;
                #endif
                return color;
			}
			ENDCG
		}
	}

    // FallBack Off
    // FallBack "Transparent"
	CustomEditor "GUIAsset1UP.MagicShaderGUI"
}
