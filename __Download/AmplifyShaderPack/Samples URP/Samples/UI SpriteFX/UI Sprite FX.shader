// Made with Amplify Shader Editor v1.9.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaderPack/UI SpriteFX"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_RingColor("Ring Color", Color) = (0,0.3793104,1,1)
		[NoScaleOffset]_Ring("Ring", 2D) = "white" {}
		[HideInInspector]_CustomUVS("CustomUVS", Vector) = (0,0,0,0)
		_RingPos("Ring Pos", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			#define ASE_VERSION 19800

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float4 _MainTex_ST;
			uniform float4 _RingColor;
			uniform sampler2D _Ring;
			uniform float4 _CustomUVS;
			uniform float4 _RingPos;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode30 = tex2D( _MainTex, uv_MainTex );
				float2 appendResult54 = (float2((0.0 + (uv_MainTex.x - _CustomUVS.x) * (1.0 - 0.0) / (_CustomUVS.z - _CustomUVS.x)) , (0.0 + (uv_MainTex.y - _CustomUVS.y) * (1.0 - 0.0) / (_CustomUVS.w - _CustomUVS.y))));
				float4 appendResult65 = (float4(_RingPos.x , ( _RingPos.y * _SinTime.w ) , _RingPos.z , _RingPos.w));
				float4 temp_output_57_0_g9 = appendResult65;
				float2 temp_output_2_0_g9 = (temp_output_57_0_g9).zw;
				float2 temp_cast_0 = (1.0).xx;
				float2 temp_output_13_0_g9 = ( ( ( appendResult54 + (temp_output_57_0_g9).xy ) * temp_output_2_0_g9 ) + -( ( temp_output_2_0_g9 - temp_cast_0 ) * 0.5 ) );
				float TimeVar197_g9 = _Time.y;
				float cos17_g9 = cos( TimeVar197_g9 );
				float sin17_g9 = sin( TimeVar197_g9 );
				float2 rotator17_g9 = mul( temp_output_13_0_g9 - float2( 0.5,0.5 ) , float2x2( cos17_g9 , -sin17_g9 , sin17_g9 , cos17_g9 )) + float2( 0.5,0.5 );
				float4 tex2DNode97_g9 = tex2D( _Ring, rotator17_g9 );
				float temp_output_115_0_g9 = step( ( (temp_output_13_0_g9).y + -0.5 ) , 0.0 );
				float lerpResult125_g9 = lerp( 1.0 , ( 1.0 - tex2DNode30.a ) , ( 1.0 - temp_output_115_0_g9 ));
				float4 lerpResult59 = lerp( tex2DNode30 , _RingColor , (( tex2DNode97_g9 * lerpResult125_g9 * tex2DNode97_g9.a )).a);
				
				fixed4 c = lerpResult59;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19800
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;29;-2528,-208;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;66;-1168,608;Inherit;False;745;437;Comment;4;32;63;64;65;Wavy Ring;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;61;-2138.606,153.7167;Inherit;False;797.0422;558.1058;Comment;7;57;55;50;51;52;53;54;Remap Sprite UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinTimeNode;63;-1056,864;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;-2026.838,506.8231;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;32;-1120,656;Float;False;Property;_RingPos;Ring Pos;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,-0.06,0.98,2.54;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-2088.606,203.7167;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;55;-2058.288,332.4499;Float;False;Property;_CustomUVS;CustomUVS;9;1;[HideInInspector];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0.5,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;-2026.44,596.8225;Float;False;Constant;_Float2;Float 2;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;53;-1741.042,270.4898;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;52;-1746.327,470.5828;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-736,816;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-1360,-224;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.CommentaryNode;67;-1042,-98;Inherit;False;1151;592;Comment;5;31;36;58;60;71;Rotation FX;0,1,0.1310346,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;65;-592,688;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;36;-960,384;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;31;-992,80;Float;True;Property;_Ring;Ring;8;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;2d68d658f4789a241a173ed536f6340e;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DynamicAppendNode;54;-1520,400;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;71;-400,144;Inherit;False;UI-Sprite Effect Layer;0;;9;789bf62641c5cfe4ab7126850acc22b8;18,74,2,204,2,191,0,225,1,242,0,237,1,249,0,186,0,177,0,182,0,229,0,92,0,98,1,234,0,126,1,129,1,130,1,31,0;18;192;COLOR;1,1,1,1;False;39;COLOR;1,1,1,1;False;37;SAMPLER2D;;False;218;FLOAT2;0,0;False;239;FLOAT2;0,0;False;181;FLOAT2;0,0;False;75;SAMPLER2D;;False;80;FLOAT;1;False;183;FLOAT2;0,0;False;188;SAMPLER2D;;False;33;SAMPLER2D;;False;248;FLOAT2;0,0;False;233;SAMPLER2D;;False;101;SAMPLER2D;;False;57;FLOAT4;0,0,0,0;False;40;FLOAT;0;False;231;FLOAT;1;False;30;FLOAT;1;False;2;COLOR;0;FLOAT2;172
Node;AmplifyShaderEditor.ColorNode;58;-368,-48;Float;False;Property;_RingColor;Ring Color;7;0;Create;True;0;0;0;False;0;False;0,0.3793104,1,1;0,1,0.3793102,1;False;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ComponentMaskNode;60;-128,128;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;59;176,-208;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;27;336,-208;Float;False;True;-1;2;ASEMaterialInspector;0;10;AmplifyShaderPack/UI SpriteFX;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;57;2;29;0
WireConnection;53;0;57;2
WireConnection;53;1;55;2
WireConnection;53;2;55;4
WireConnection;53;3;50;0
WireConnection;53;4;51;0
WireConnection;52;0;57;1
WireConnection;52;1;55;1
WireConnection;52;2;55;3
WireConnection;52;3;50;0
WireConnection;52;4;51;0
WireConnection;64;0;32;2
WireConnection;64;1;63;4
WireConnection;30;0;29;0
WireConnection;65;0;32;1
WireConnection;65;1;64;0
WireConnection;65;2;32;3
WireConnection;65;3;32;4
WireConnection;36;0;30;4
WireConnection;54;0;52;0
WireConnection;54;1;53;0
WireConnection;71;37;31;0
WireConnection;71;239;54;0
WireConnection;71;57;65;0
WireConnection;71;30;36;0
WireConnection;60;0;71;0
WireConnection;59;0;30;0
WireConnection;59;1;58;0
WireConnection;59;2;60;0
WireConnection;27;0;59;0
ASEEND*/
//CHKSM=06B5B044FA5DBA56E36579A6374C6AD9D686CCC5