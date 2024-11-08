Shader "Rob/AnimMultiTextures"
{
	Properties
	{
		_AlbedoAnim("AlbedoAnim", 2D) = "white" {}
		_EmissionAnim("EmissionAnim", 2D) = "black" {}		
		_TileMove("TileMove", Vector) = (0.1,0.1,0,0)
		_TilingX("Tiling X", Float) = 1
		_TilingY("Tiling Y", Float) = 1
		
		_Albedo("Albedo", 2D) = "white" {}
		_Normals("Normals", 2D) = "bump" {}
		_Metallic("Metallic", 2D) = "black" {}
		_Emission("Emission", 2D) = "black" {}
		_Occlusion("Occlusion", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normals;
		uniform float4 _Normals_ST;
		uniform sampler2D _AlbedoAnim;
		uniform float2 _TileMove;
		uniform float _TilingX;
		uniform float _TilingY;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _EmissionAnim;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform float _Smoothness;
		uniform sampler2D _Occlusion;
		uniform float4 _Occlusion_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normals = i.uv_texcoord * _Normals_ST.xy + _Normals_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normals, uv_Normals ) );
			float2 temp_cast_0 = (_TilingX).xx;
			float2 temp_cast_1 = (_TilingY).xx;
			float2 uv_TexCoord22 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			float2 panner19 = ( _Time.x * _TileMove + uv_TexCoord22);
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode13 = tex2D( _Albedo, uv_Albedo );
			float4 lerpResult16 = lerp( tex2D( _AlbedoAnim, panner19 ) , tex2DNode13 , tex2DNode13.a);
			o.Albedo = lerpResult16.rgb;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float4 lerpResult25 = lerp( tex2D( _EmissionAnim, panner19 ) , tex2D( _Emission, uv_Emission ) , tex2DNode13.a);
			o.Emission = lerpResult25.rgb;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			float4 tex2DNode2 = tex2D( _Metallic, uv_Metallic );
			o.Metallic = tex2DNode2.a;
			float lerpResult10 = lerp( tex2DNode2.a , 0.0 , _Smoothness);
			o.Smoothness = lerpResult10;
			float2 uv_Occlusion = i.uv_texcoord * _Occlusion_ST.xy + _Occlusion_ST.zw;
			o.Occlusion = tex2D( _Occlusion, uv_Occlusion ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"

}
