// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New AmplifyShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Color0("Color 0", Color) = (0,0,0,0)
		_Float0("Float 0", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			fixed filler;
		};

		uniform float4 _Color0;
		uniform float _Float0;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Emission = _Color0.rgb;
			o.Alpha = 1;
			float4 lerpResult9 = lerp( float4(0,0,0,0) , float4(1,1,1,0) , _Float0);
			clip( lerpResult9.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14501
1927;29;1906;1004;1206.24;214.5975;1;True;True
Node;AmplifyShaderEditor.ColorNode;7;-795.4524,-119.8816;Float;False;Constant;_Color1;Color 1;3;0;Create;True;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-825.2396,146.4025;Float;False;Constant;_Color2;Color 2;3;0;Create;True;0;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-989.4934,625.576;Float;False;Property;_Float0;Float 0;2;0;Create;True;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-345.4934,-110.424;Float;False;Property;_Color0;Color 0;1;0;Create;True;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;9;-515.3807,143.1434;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1148.704,397.9411;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;6cf12fa7855b5e94ebaaef870dd4be15;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;44,1;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;New AmplifyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;0;False;0;0;False;0;Custom;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;7;0
WireConnection;9;1;10;0
WireConnection;9;2;5;0
WireConnection;0;2;3;0
WireConnection;0;10;9;0
ASEEND*/
//CHKSM=F7722F052FE95DF4C770BED3A5C27D6027765F51