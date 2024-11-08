Shader "1UP/Magic Shader/Unlit Base"
{
	Properties
	{
		[HideInInspector] _OUTLINE("FolderTag", float) = 1.0
		[Toggle(_OutlineEnable)] _OutlineEnable("Enable Outline Function", Float) = 0
		_OutlineColor("Outline Color", Color) = (1, 0, 0, 1)
		_OutlineWidth("Outline Width", Range(0, 1)) = 0.0

		[HideInInspector] _METALLIC("FolderTag", float) = 1.0
		_LightPos("Light Position", Vector) = (100, 100, 100, 0)
		_Roughness("Roughness", Range(0.0, 1.0)) = 0.0

		[HideInInspector] _AO("FolderTag", float) = 1.0
		[Toggle(_AoEnable)] _AoEnable("Enable AO Function", Float) = 1.0
		_AoColor("AO Color", Color) = (0, 0, 0, 1)
		_AoSpread("AO Spread", Range(0.0, 200.0)) = 10.0
		_AoXOffset("AO X Offset", Range(-1.0, 1.0)) = 0.0
		_AoYOffset("AO Y Offset", Range(-1.0, 1.0)) = 0.0
		_AoLimitValue("Ao Limit Value", Range(0.0, 1.0)) = 1.0
		[Toggle(_AOOnly)] _AOOnly("Debug AO", Float) = 0

		[HideInInspector] _DIFFUSE("FolderTag", float) = 1.0
		[Toggle(_DiffuseEnable)] _DiffuseEnable("Enable DIFFUSE Function", Float) = 1.0
		_DiffuseColor("Diffuse Color", Color) = (1,1,1,1)
		_DiffuseIntensity("Diffuse Instensity", Range(0.0, 1.0)) = 0.0
		_DiffuseColorLevel("Diffuse Color Level", Range(0.0, 2.0)) = 0.0
		_DiffuseUvTex("Diffuse Texture", 2D) = "white" {}
		[KeywordEnum(UV0, UV1)] _UV_CHANNEL("UV channel", Float) = 0

		[HideInInspector] _MATCAP("FolderTag", float) = 1.0
		[Toggle(_MatCapEnable)] _MatCapEnable("Enable MATCAP Function", Float) = 1.0
		_MaterialTex("MatCap Texture", 2D) = "white" {}
		_MaterialColor("MatCap Color", Color) = (1,1,1,1)
		_MaterialColorLevel("MatCap Color Level", Range(0.0, 3.0)) = 0.0
		[Toggle(_RevertMatCap)] _RevertMatCap("Revert MatCap", Float) = 0
		[KeywordEnum(None, OpenCV, Linear)] _GRAY("MatCap GrayStyle", Float) = 0

		[HideInInspector] _REFLECT("FolderTag", float) = 1.0
		[Toggle(_ReflectEnable)] _ReflectEnable("Enable REFLECT Function", Float) = 1.0
		_ReflectTex("Texture", 2D) = "white" {}
		_ReflectIntensity("Instensity", Range(0.0, 1.0)) = 0.0
		_ReflectRoughness("Roughness", Range(0, 1.0)) = 0.0
		_CubeMap("Reflect CubeMap", Cube) = "" {}
		[Toggle(_UseCubeMap)] _UseCubeMap("Use CubeMap", Float) = 0

		[HideInInspector] _UNLITSHADOW("FolderTag", float) = 1.0
		[Toggle(_ShadowEnable)] _ShadowEnable("Enable UNLIT SHADOW Function", Float) = 1.0
		_ShadowColor("Shadow Color", Color) = (0, 0, 0, 1)
		_ShadowFixOffset("Shadow Fix Offset", Range(0, 1.0)) = 0.0
		_ShadowBlur("Shadow Blur", Range(0.0, 10.0)) = 1.0
		_ShadowNoise("Shadow Noise", Range(0.0, 30.0)) = 1.0

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
		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "Transparent"
		}

		Pass 
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Back

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

			#pragma shader_feature _ShadowEnable
			// #pragma shader_feature _ShadowOnly

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
			float _DiffuseIntensity;
			float _DiffuseColorLevel;
			sampler2D _DiffuseUvTex;
			float _DiffuseAlphaCutoff;

			sampler2D _MaterialTex;
			float4 _MaterialColor;
			float _MaterialColorLevel;

			sampler2D _ReflectTex;
			float _ReflectIntensity;
			float _ReflectRoughness;
			samplerCUBE _CubeMap;

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
			float _ShadowFixOffset;
			float _ShadowBlur;
			float _ShadowNoise;

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
				float4 shadowWPos	: TEXCOORD5;
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

			float4 getAoColor(float2 depthCoord) {
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
					float3(0.001, 0.003, 0.005), 0.0);
				lndcpos = mul(shadowProj, worldPos + posOffset);
				lndcpos.xyz = lndcpos.xyz / lndcpos.w;
				float3 luvpos = lndcpos * 0.5 + 0.5;
				float2 uvOffset = float2(_ShadowBlur, _ShadowBlur) / 512.0;
				float texDepth = tex2D(shadowMap, luvpos.xy).r;
				float texDepth1 = tex2D(shadowMap,
					luvpos.xy + uvOffset * float2(1.0, 0.0));
				float texDepth2 = tex2D(shadowMap,
					luvpos.xy - uvOffset * float2(1.0, 0.0));
				float texDepth3 = tex2D(shadowMap,
					luvpos.xy + uvOffset * float2(0.0, 1.0));
				float texDepth4 = tex2D(shadowMap,
					luvpos.xy - uvOffset * float2(0.0, 1.0));

				float texColor = getDepth(texDepth, lndcpos.z);
				float texColor1 = getDepth(texDepth1, lndcpos.z);
				float texColor2 = getDepth(texDepth2, lndcpos.z);
				float texColor3 = getDepth(texDepth3, lndcpos.z);
				texColor = 1.0 - (texColor + texColor1 + texColor2 + texColor3) / 4.0;
				return lerp(1.0 - shadowOffset, float4(1.0, 1.0, 1.0, 1.0), texColor);
			}

			vertexOutput vert(vertexInput i) {
				vertexOutput o;

				// float4x4 modelMatrix = unity_ObjectToWorld;
				// float4x4 modelMatrixInverse = unity_WorldToObject; 

				float t = _ShadowFixOffset;
				float4 swpos1 = mul(unity_ObjectToWorld, 
				i.vertex + float4(t, 0.0, 0.0, 0.0));
				float4 swpos2 = mul(unity_ObjectToWorld, 
				i.vertex + float4(0.0, t, 0.0, 0.0));
				float4 swpos3 = mul(unity_ObjectToWorld, 
				i.vertex + float4(0.0, 0.0, t, 0.0));
				o.shadowWPos = (swpos1 + swpos2 + swpos3) / 3.0;
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

			float4 frag(vertexOutput i) : COLOR {
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

				float4 shadowColor = float4(1.0, 1.0, 1.0, 1.0);
				float4 shadowColor0 = float4(1.0, 1.0, 1.0, 1.0);
				float4 shadowColor1 = float4(1.0, 1.0, 1.0, 1.0);
				float4 shadowColor2 = float4(1.0, 1.0, 1.0, 1.0);
				#ifdef _ShadowEnable
					if (_ShadowOffset0 > 0.001) {
						shadowColor0 = getSelfShadow(i.shadowWPos, noise, 
						_ShadowProj0, _ShadowTex0, _ShadowOffset0);
					}
					if (_ShadowOffset1 > 0.001) {
						shadowColor1 = getSelfShadow(i.shadowWPos, noise, 
						_ShadowProj1, _ShadowTex1, _ShadowOffset1);
					}
					if (_ShadowOffset2 > 0.001) {
						shadowColor2 = getSelfShadow(i.shadowWPos, noise, 
						_ShadowProj2, _ShadowTex2, _ShadowOffset2);
					}
					shadowColor = 
						min(min(shadowColor0, shadowColor1), shadowColor2);
					float shadowValue = 1.0 - shadowColor.r;
					shadowColor.rgb = 
						_ShadowColor.rgb * (shadowValue) + shadowColor.rgb;
					shadowColor.a = _ShadowColor.a;
				#endif

				// #if _ShadowOnly
				// return float4(shadowValue, 0.0, 0.0, 1.0);
				// return float4(shadowColor0.aaa, 1.0);
				// return shadowColor;
				// #endif

				float4 diffuseColor = float4(1.0, 1.0, 1.0, 1.0);
				#ifdef _DiffuseEnable
					float4 diffuseTexCol = tex2D(_DiffuseUvTex, i.uv.xy);
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
				#ifdef _ShadowEnable
					finalColor = lerp(finalColor, 
						shadowColor * finalColor, 
						shadowColor.a);
				#endif

				return finalColor;
			}
			ENDCG
		}

		Pass
		{
			Name "Outline Pass"
			Tags {}
			Cull Front
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _OutlineEnable
			
			fixed4 _OutlineColor;
			fixed _OutlineWidth;
			
			struct customData
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			struct v2f
			{
				float4 pos : POSITION;
				float4 color : COLOR;
			};
			
			v2f vert(customData v)
			{           
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				#ifdef _OutlineEnable
					float3 vertexOutline = fixed3(0, 0, 0);
					vertexOutline = v.vertex.xyz + normalize(v.normal) * _OutlineWidth / 20.0;
					o.pos = UnityObjectToClipPos(float4(vertexOutline, 1.0));
					o.color = _OutlineColor;
				#endif
				return o;
			}
			
			half4 frag(v2f i) :COLOR
			{          
				float4 col = float4(0, 0, 0, 0);
				#ifdef _OutlineEnable
					col = i.color;
				#endif
				return col;
			}
			
			ENDCG
		}
	}

	FallBack "Diffuse"
	CustomEditor "GUIAsset1UP.MagicShaderGUI"
}
