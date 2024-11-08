Shader "1UP/Magic Shader/Unlit Cutout Demo"
{
	Properties
	{
		[HideInInspector] _AO("FolderTag", float) = 1.0
		[Toggle(_AoEnable)] _AoEnable("Enable AO Function", Float) = 0
		_AoColor("AO Color", Color) = (0, 0, 0, 1)
		_AoSpread("AO Spread", Range(0.0, 100.0)) = 10.0
		_AoXOffset("AO X Offset", Range(-1.0, 1.0)) = 0.0
		_AoYOffset("AO Y Offset", Range(-1.0, 1.0)) = 0.0
		_AoLimitValue("Ao Limit Value", Range(0.0, 1.0)) = 1.0
		[Toggle(_AOOnly)] _AOOnly("Debug AO", Float) = 0

		[HideInInspector] _METALLIC("FolderTag", float) = 1.0
		_LightPos("Light Position", Vector) = (100, 100, 100, 0)
		_Roughness("Roughness", Range(0.0, 1.0)) = 0.0

		[HideInInspector] _DIFFUSE("FolderTag", float) = 1.0
		[Toggle(_DiffuseEnable)] _DiffuseEnable("Enable DIFFUSE Function", Float) = 0
		_DiffuseColor("Diffuse Color", Color) = (1,1,1,1)
		_DiffuseIntensity("Diffuse Instensity", Range(0.0, 1.0)) = 0.0
		_DiffuseColorLevel("Diffuse Color Level", Range(0.0, 1.0)) = 0.0
		_DiffuseUvTex("Diffuse Texture", 2D) = "white" {}
		_DiffuseAlphaCutout("Diffuse Alpha Cutout", Range(0.0, 1.0)) = 0.5
		[KeywordEnum(UV0, UV1)] _UV_CHANNEL("UV channel", Float) = 0

		[HideInInspector] _MATCAP("FolderTag", float) = 1.0
		[Toggle(_MatCapEnable)] _MatCapEnable("Enable MATCAP Function", Float) = 0
		_MaterialTex("MatCap Texture", 2D) = "white" {}
		_MaterialColor("MatCap Color", Color) = (1,1,1,1)
		_MaterialColorLevel("MatCap Color Level", Range(0.0, 1.0)) = 0.0
		[Toggle(_RevertMatCap)] _RevertMatCap("Revert MatCap", Float) = 0
		[KeywordEnum(None, OpenCV, Linear)] _GRAY("MatCap GrayStyle", Float) = 0

		[HideInInspector] _REFLECT("FolderTag", float) = 1.0
		[Toggle(_ReflectEnable)] _ReflectEnable("Enable REFLECT Function", Float) = 0
		_ReflectTex("Texture", 2D) = "white" {}
		_ReflectIntensity("Instensity", Range(0.0, 1.0)) = 0.0
		_ReflectRoughness("Roughness", Range(0, 1.0)) = 0.0
		_CubeMap("Reflect CubeMap", Cube) = "" {}
		[Toggle(_UseCubeMap)] _UseCubeMap("Use CubeMap", Float) = 0

		_MaskTex("Texture", 2D) = "black" {}
	}

	SubShader
	{
		// Tags
		// {
			// "Queue" = "Geometry"
			// "RenderType" = "Transparent"
		// }

		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass 
		{
			Cull Off

			CGPROGRAM
			#pragma shader_feature _AoEnable
			#pragma shader_feature _AOOnly

			#pragma shader_feature _DiffuseEnable
			#pragma shader_feature _UV_CHANNEL_UV0 _UV_CHANNEL_UV1

			#pragma shader_feature _MatCapEnable
			#pragma shader_feature _GRAY_NONE _GRAY_OPENCV _GRAY_LINEAR
			#pragma shader_feature _RevertMatCap

			#pragma shader_feature _ReflectEnable
			#pragma shader_feature _UseCubeMap

			#include "UnityCG.cginc"
			#pragma vertex vert  
			#pragma fragment frag 

			float3 _LightPos;
			float _Roughness;

			// Ao Params
			float4 _AoColor;
			float _AoSpread;
			float _AoXOffset;
			float _AoYOffset;
			float _AoLimitValue;
			uniform sampler2D _CameraDepthTexture;

			// Materials Params
			float4 _DiffuseColor;
			float _MaterialColorLevel;
			float _DiffuseIntensity;
			float _DiffuseColorLevel;
			sampler2D _DiffuseUvTex;
			float _DiffuseAlphaCutout;

			sampler2D _MaterialTex;
			float4 _MaterialColor;

			sampler2D _ReflectTex;
			float _ReflectIntensity;
			float _ReflectRoughness;
			samplerCUBE _CubeMap;

			sampler2D _MaskTex;

			#define PI          3.14159265359

			#define DL 2.399963229728653  // PI * ( 3.0 - sqrt( 5.0 ) )
			#define EULER 2.718281828459045

			// user variables
			#define noiseAmount 0.4

			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 uv0: TEXCOORD0;
				float4 uv1: TEXCOORD1;
			};
			struct vertexOutput {
				float4 pos			: SV_POSITION;
				float4 worldPos		: TEXCOORD0;
				float3 normalDir	: TEXCOORD1;
				float3 lightPos		: TEXCOORD2;
				float4 uv			: TEXCOORD3;
				float3 viewDir		: TEXCOORD4;
			};

			float D_GGX(float Roughness, float NoH)
			{
				float m = Roughness * Roughness;
				float m2 = m * m;
				float d = (NoH * m2 - NoH) * NoH + 1.0;     // 2 mad
				return m2 / (PI * d * d);                   // 4 mul, 1 rcp
			}

			float Vis_Smith(float Roughness, float NoV, float NoL)
			{
				float a = Roughness * Roughness;
				float a2 = a * a;

				float Vis_SmithV = NoV + sqrt(NoV * (NoV - NoV * a2) + a2);
				float Vis_SmithL = NoL + sqrt(NoL * (NoL - NoL * a2) + a2);
				return 1.0 / (Vis_SmithV * Vis_SmithL);
			}

			float3 F_Schlick(float3 color, float VoH)
			{
				float Fc = pow(1.0 - VoH, 5.0);
				return Fc + (1.0 - Fc) * color;
			}

			float2 rand(float2 coord) {
				float2 noise;
				float nx = dot(coord, float2(12.9898, 78.233));
				float ny = dot(coord, float2(12.0909, 78.233));
				noise = clamp(frac(43758.5453 * sin(float2(nx, ny))), 0.0, 1.0);

				return (noise * 2.0 - 1.0) * noiseAmount;
			}

			float readEyeDepth(float2 coord) {
				float depth = LinearEyeDepth(
				tex2D(_CameraDepthTexture, coord).x);
				return depth;
			}

			float4 getAoColor(float2 depthCoord)
			{
				float depth = readEyeDepth(depthCoord) / 10.0;
				float ao = 1.0;
				int sampLen = 5;
				float step = 0.4 / (depth * _ScreenParams.xy);
				float2 offset = float2(_AoXOffset, _AoYOffset) / 100.0;
				for (int j = 1; j < sampLen; j++) {
					float depth1 = readEyeDepth(depthCoord + offset + step * float2(j, j)) / 10.0;
					float depth2 = readEyeDepth(depthCoord + offset + step * float2(-j, j)) / 10.0;
					float depth3 = readEyeDepth(depthCoord + offset + step * float2(j, -j)) / 10.0;
					float depth4 = readEyeDepth(depthCoord + offset + step * float2(-j, -j)) / 10.0;
					float delta = (depth - depth1);
					delta = max(delta, (depth - depth2));
					delta = max(delta, (depth - depth3));
					delta = max(delta, (depth - depth4));
					delta = min(delta, _AoLimitValue / 100);
					ao -= delta * _AoSpread / j;
				}

				return float4(lerp(_AoColor.rgb, 
				float3(1.0, 1.0, 1.0), ao), 1.0);
			}

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				// float4x4 modelMatrix = unity_ObjectToWorld;
				// float4x4 modelMatrixInverse = unity_WorldToObject; 
				o.worldPos = mul(unity_ObjectToWorld, i.vertex);

				o.normalDir = normalize(mul(float4(i.normal, 0.0), unity_WorldToObject).xyz);
				o.pos = UnityObjectToClipPos(i.vertex);
				//float3 floatLightPos = float3(100.0, 100.0, 100.0);
				o.lightPos = _LightPos;

				// MatCap uv position
				o.uv.z = dot(normalize(UNITY_MATRIX_IT_MV[0].xyz), normalize(i.normal));
				o.uv.w = dot(normalize(UNITY_MATRIX_IT_MV[1].xyz), normalize(i.normal));
				o.uv.zw = o.uv.zw * 0.5 + 0.5;

				#if _RevertMatCap
					o.uv.z = 1.0 - o.uv.z;
				#endif

				#if _UV_CHANNEL_UV0
					o.uv.xy = i.uv0.xy;
				#elif _UV_CHANNEL_UV1
					o.uv.xy = i.uv1.xy;
				#endif
				o.viewDir = mul(unity_ObjectToWorld, i.vertex).xyz - _WorldSpaceCameraPos;

				return o;
			}

			float4 frag(vertexOutput i) : COLOR
			{
				// discard;
				float2 screenCoord = i.pos.xy / _ScreenParams.xy;
				float2 noise = rand(screenCoord);
				noise.x = saturate(noise.x);
				// float depth = readEyeDepth(screenCoord);
				// return float4(depth, depth, depth, 1.0);

				float4 aoColor = float4(1.0, 1.0, 1.0, 1.0);
				#ifdef _AoEnable
					aoColor = getAoColor(screenCoord);
				#endif
				#ifdef _AOOnly
					return aoColor;
				#endif

				float4 diffuseColor = float4(1.0, 1.0, 1.0, 1.0);
				#ifdef _DiffuseEnable
					float4 diffuseTexCol = tex2D(_DiffuseUvTex, i.uv.xy);
					clip(diffuseTexCol.a - _DiffuseAlphaCutout);
					diffuseColor = float4(lerp(diffuseTexCol.rgb,
						_DiffuseColor.rgb * diffuseTexCol.rgb, 
						_DiffuseColor.a), 1.0);
				#endif

				float3 matColor = float3(1.0, 1.0, 1.0);
				#ifdef _MatCapEnable
					float3 matTexCol = tex2D(_MaterialTex, i.uv.zw).rgb;
						matTexCol.rgb += _MaterialColorLevel;

					// GRAY_NONE
					#if _GRAY_OPENCV
						float grayValue = (matTexCol.r * 0.299 + 
							matTexCol.g * 0.587 +
							matTexCol.b * 0.114);
						matTexCol = float3(grayValue, grayValue, grayValue);
					#elif _GRAY_LINEAR
						float grayValue = 
							(matTexCol.r + matTexCol.g + matTexCol.b) / 3.0;
							matTexCol = float3(grayValue, grayValue, grayValue);
					#endif

					matColor = matTexCol.rgb * _MaterialColor.rgb;
				#endif

				float3 N = normalize(i.normalDir);
				float3 V = mul(UNITY_MATRIX_IT_MV, normalize(float3(0.0, 0.0, 1.0)));
				float3 L = normalize(i.lightPos - i.worldPos.xyz);

				float3 H = normalize(V + L);

				float NoL = saturate(dot(N, L));
				float NoV = saturate(dot(N, V));
				float VoH = saturate(dot(V, H));
				float NoH = saturate(dot(N, H));
				float D = D_GGX(_Roughness, NoH);
				float Vis = Vis_Smith(_Roughness, NoV, NoL);
				float3 specF = F_Schlick(diffuseColor.rgb, VoH);

				float3 specular = max(D * Vis * specF * NoL, float3(0.0, 0.0, 0.0));

				float3 color = matColor + specular;

				float4 reflectColor = float4(0.0, 0.0, 0.0, 0.0);
				#ifdef _ReflectEnable
					float matCapStep = noise.x;
					#if _UseCubeMap
						matCapStep *= 3.0;
						float3 reflectedDir = reflect(i.viewDir, normalize(i.normalDir));
						float4 reflectColor1 = texCUBE(_CubeMap, 
							reflectedDir + _ReflectRoughness * 
						float3(-matCapStep, matCapStep, matCapStep));
						float4 reflectColor2 = texCUBE(_CubeMap, 
							reflectedDir + _ReflectRoughness * 
						float3(matCapStep, -matCapStep, matCapStep));
						float4 reflectColor3 = texCUBE(_CubeMap, 
							reflectedDir + _ReflectRoughness * 
						float3(matCapStep, matCapStep, -matCapStep));
						float4 reflectColor4 = texCUBE(_CubeMap, 
							reflectedDir + _ReflectRoughness * 
						float3(-matCapStep, matCapStep, -matCapStep));
					#else
						float4 reflectColor1 = tex2D(_ReflectTex, 
							i.uv.zw + _ReflectRoughness * 
						float2(matCapStep, matCapStep));
						float4 reflectColor2 = tex2D(_ReflectTex, 
							i.uv.zw + _ReflectRoughness * 
						float2(matCapStep, -matCapStep));
						float4 reflectColor3 = tex2D(_ReflectTex, 
							i.uv.zw + _ReflectRoughness * 
						float2(-matCapStep, -matCapStep));
						float4 reflectColor4 = tex2D(_ReflectTex, 
							i.uv.zw + _ReflectRoughness * 
						float2(-matCapStep, matCapStep));
					#endif

					reflectColor = (reflectColor1 + reflectColor2 + 
						reflectColor3 + reflectColor4) / 4.0;
				#endif

				float4 finalColor = float4(color, 1.0);

				#ifdef _ReflectEnable
					finalColor = lerp(finalColor, 
						finalColor * reflectColor,
						_ReflectIntensity);
				#endif

				#ifdef _DiffuseEnable
					finalColor.rgb = lerp(finalColor, 
						finalColor * diffuseColor + diffuseColor * _DiffuseColorLevel, 
						_DiffuseIntensity).rgb;
				#endif

				#ifdef _AoEnable
					finalColor.rgb = min(finalColor.rgb, 
						finalColor.rgb * aoColor.rgb);
				#endif

				return finalColor;
			}
			ENDCG
		}
	}

	FallBack Off
	CustomEditor "GUIAsset1UP.MagicShaderGUI"
}
