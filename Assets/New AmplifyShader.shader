// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dissolve"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Texture1("Texture 1", 2D) = "white" {}
		_Dissolve("Dissolve", Range( 0 , 1)) = 1
		_color("color", Color) = (0,0,0,0)
		_emissiveintenisity("emissive intenisity", Range( 1 , 10)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _color;
		uniform sampler2D _Texture1;
		uniform float _Dissolve;
		uniform float _emissiveintenisity;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord62 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 panner64 = ( uv_TexCoord62 + 1 * _Time.y * float2( 0.05,0.02 ));
			float2 panner65 = ( uv_TexCoord62 + 1 * _Time.y * float2( -0.02,-0.05 ));
			float clampResult70 = clamp( ( tex2D( _Texture1, panner64 ).r * tex2D( _Texture1, panner65 ).g ) , 0 , 1 );
			float lerpResult3 = lerp( 0 , 1 , ( clampResult70 - (-1 + (( 1.0 - _Dissolve ) - 0) * (1 - -1) / (1 - 0)) ));
			o.Emission = ( ( _color * ( 1.0 - (-10 + (lerpResult3 - 0) * (10 - -10) / (1 - 0)) ) ) * _emissiveintenisity ).rgb;
			o.Alpha = 1;
			float clampResult31 = clamp( lerpResult3 , 0 , 1 );
			clip( clampResult31 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14501
1927;29;1906;1004;3339.577;936.6803;2.056041;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;62;-2858.219,-104.6851;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;64;-2541.97,-105.8558;Float;True;3;0;FLOAT2;1,1;False;2;FLOAT2;0.05,0.02;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;63;-2533.619,-346.6456;Float;True;Property;_Texture1;Texture 1;1;0;Create;True;0;None;None;False;white;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;65;-2571.893,162.7262;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.02,-0.05;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1912.491,448.6989;Float;False;Property;_Dissolve;Dissolve;2;0;Create;True;0;1;0.395;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;66;-2216.157,148.9313;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;67;-2220.374,-125.3082;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-1733.462,21.82236;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;41;-1547.976,440.866;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;70;-1329.018,91.5584;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;6;-1298.798,429.7146;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;72;-1028.258,508.9619;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;3;-649.2661,201.6469;Float;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;71;-336.4743,-91.08421;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-10;False;4;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;57;-9.355987,-206.698;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;54;-92.05824,-465.7588;Float;False;Property;_color;color;5;0;Create;True;0;0,0,0,0;0.3990964,0,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;61;435.6617,-89.76581;Float;False;Property;_emissiveintenisity;emissive intenisity;6;0;Create;True;0;1;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;273.1768,-307.432;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;38;-457.3582,-770.9897;Float;False;Property;_emissiveON;emissiveON;4;0;Create;True;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;12;-938.3828,-1041.656;Float;True;Property;_emissive;emissive;3;0;Create;True;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;625.4031,-213.8823;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;31;-35.36769,239.2744;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-960.7643,-794.4178;Float;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1460.55,-364.7045;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Dissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;0;False;0;0;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;64;0;62;0
WireConnection;65;0;62;0
WireConnection;66;0;63;0
WireConnection;66;1;65;0
WireConnection;67;0;63;0
WireConnection;67;1;64;0
WireConnection;68;0;67;1
WireConnection;68;1;66;2
WireConnection;41;0;5;0
WireConnection;70;0;68;0
WireConnection;6;0;41;0
WireConnection;72;0;70;0
WireConnection;72;1;6;0
WireConnection;3;2;72;0
WireConnection;71;0;3;0
WireConnection;57;0;71;0
WireConnection;58;0;54;0
WireConnection;58;1;57;0
WireConnection;38;0;12;0
WireConnection;38;1;39;0
WireConnection;60;0;58;0
WireConnection;60;1;61;0
WireConnection;31;0;3;0
WireConnection;0;2;60;0
WireConnection;0;10;31;0
ASEEND*/
//CHKSM=4FAC951CC12A22D517F639F9DA6935BF96C27D25