Shader "Datenshi/EntityShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_OverrideColor ("OverrideColor", Color) = (1,1,1,1)
		_OverrideAmount ("OverrideAmount", float) = 0
		[PerRendererData] _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "white" {}
	}
	SubShader {
	    Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		LOD 200
		Cull Off

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalMap;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		fixed4 _OverrideColor;
        fixed _OverrideAmount;
        
		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Alpha = c.a;
			o.Albedo = lerp(c.rgb, _OverrideColor.rgb, _OverrideAmount); 
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));

			// Hack for outline: Make black pixels always black
			if (length(c.rgb)<0.001) {
				o.Normal = fixed3(0,0,-1);
				o.Albedo = fixed3(0,0,0);
			}
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}