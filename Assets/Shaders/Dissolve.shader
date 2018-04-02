// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Dissolve" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Emissive ("Emissive", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_DissolveRatio("Dissolve", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull off // back front or off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _Emissive;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			// code above Usefull for optimisation

		sampler2D _Noise;
		float _DissolveRatio;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			// Second texture for dissolve.
			fixed4 noise = tex2D (_Noise, IN.uv_MainTex);
			/*
			float dissolveLol = sin(_Time.y);
			c.rgb = lerp(float3(1, 0.5, 0), float3(0, 0, 1), dissolveLol);
			clip(noise - dissolveLol); // Delete frag from buffer renderer if result is negative.
			*/
			float burn = noise.r;
			clip(burn - _DissolveRatio);

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = _Emissive;

			float2 uv = IN.uv_MainTex;

			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
