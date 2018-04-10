Shader "Datenshi/EntityShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_OverrideColor ("OverrideColor", Color) = (1,1,1,1)
		_OccludeColor ("OccludeColor", Color) = (1,1,1,1)
		_OccludeAmount ("OccludeAmount", Range(0, 1)) = 0
		[Toggle]
		_DrawOcclusion("Draw Occlusion", Float) = 1
		[PerRendererData] 
		_OverrideAmount ("OverrideAmount", Range(0, 1)) = 0
		[PerRendererData] _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "white" {}
	}
	SubShader {
	    Tags { 
	        "Queue"="Transparent" 
	        "RenderType"="Transparent"
	     }
		//LOD 200
        Cull Off
        Pass {
		    ZWrite Off        
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Greater 
            CGPROGRAM
		        #pragma vertex vert
			    #pragma fragment frag
    			
    			#include "UnityCG.cginc"

	    		struct appdata
		    	{
			    	float4 vertex : POSITION;
				    float2 uv : TEXCOORD;
			    };

			    struct v2f {
				    float2 uv : TEXCOORD;
				    float4 vertex : SV_POSITION;
			    };

			    v2f vert (appdata v) {
				    v2f o;
				    o.vertex = UnityObjectToClipPos(v.vertex);
				    o.uv = v.uv;
				    return o;
			    }
			
			sampler2D _MainTex;
			fixed4 _OccludeColor;
			fixed _OccludeAmount;
			fixed _DrawOcclusion;
			fixed4 frag (v2f i) : SV_Target {
			    if (_DrawOcclusion == 0) {
			        return fixed4(0,0,0,0);
			    }
			    fixed4 rawCol = tex2D(_MainTex, i.uv);
				fixed4 col = lerp(rawCol, _OccludeColor, _OccludeAmount);
				col.a = rawCol.a;
				return col;
			}
        ENDCG
        
        }
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
        

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Alpha = c.a;
			o.Albedo = lerp(c.rgb, _OverrideColor.rgb, _OverrideAmount); 
			//o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
				o.Normal = fixed3(0,0,1);

			// Hack for outline: Make black pixels always black
			if (length(c.rgb)<0.001) {
				o.Albedo = fixed3(0,0,0);
			}
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}