// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Datenshi/EntityShader" {
	Properties {
	    [PerRendererData]
		_Color ("Main Color", Color) = (1,1,1,1)
		_OverrideColor ("Override Color", Color) = (1,1,1,1)
		[PerRendererData] 
		_OverrideAmount ("Override Amount", Range(0, 1)) = 0
		[PerRendererData]
		_AlternativeOverrideColor ("Alternative Override Color", Color) = (1,1,1,1)
		[PerRendererData] 
		_AlternativeOverrideAmount ("Alternative Override Amount", Range(0, 1)) = 0
		_OccludeColor ("OccludeColor", Color) = (1,1,1,1)
		_OccludeAmount ("OccludeAmount", Range(0, 1)) = 0
		[Toggle]
		_DrawOcclusion("Draw Occlusion", Float) = 1
		[PerRendererData]
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "white" {}
		[Toggle]
		[PerRendererData]
    	_Outline("Outline", Int) = 0
		[PerRendererData]
		_OutlineColor("Outline Color", Color) = (0.96, 0, 0.34, 1)
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
        fixed4 _AlternativeOverrideColor;
        fixed _AlternativeOverrideAmount;
        

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 textureColor = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Alpha = textureColor.a;
			fixed3 c = lerp(textureColor.rgb, _OverrideColor.rgb, _OverrideAmount);
			c = lerp(c.rgb, _AlternativeOverrideColor.rgb, _AlternativeOverrideAmount);
			o.Albedo = c;
			//o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
				o.Normal = fixed3(0,0,1);

			// Hack for outline: Make black pixels always black
			if (length(c.rgb)<0.001) {
				o.Albedo = fixed3(0,0,0);
			}
			
		}
		ENDCG
		Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma shader_feature ETC1_EXTERNAL_ALPHA
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            int _Outline;
            fixed4 _OutlineColor;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            const fixed4 Transparent = fixed4(0, 0, 0, 0);

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                if (_Outline == 0) {
                    return Transparent;
                }

                // If outline is enabled and there is a pixel, try to draw an outline.
                if (c.a > 0) {
                    // Get the neighbouring four pixels.
                    fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + fixed2(0, _MainTex_TexelSize.y));
                    fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0, _MainTex_TexelSize.y));
                    fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(_MainTex_TexelSize.x, 0));
                    fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - fixed2(_MainTex_TexelSize.x, 0));

                    // If one of the neighbouring pixels is invisible, we render an outline.
                    if (pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a <= 0) {
                        return _OutlineColor;
                    }
                }

                return Transparent;
            }
            ENDCG
        }
	}
	FallBack "Diffuse"
}