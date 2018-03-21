Shader "Unlit/ScanningEffect"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_ScanValue ("ScanValue", Range(0,1)) = 1.
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float2 localuv : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST, _Color;
			float _ScanValue;
			
			v2f vert (appdata v)
			{

				v2f o;
				o.localuv = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.localuv;
				fixed4 col = float4(_Color.rgb, _Color.a * step((uv.y*0.5)+0.5, _ScanValue));
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
