// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Datenshi/EntityShader" {
	Properties {
	    _Color("Color", Color) = (1,1,1,1)
		_OccludeColor ("OccludeColor", Color) = (1,1,1,1)
		_OccludeAmount ("OccludeAmount", Range(0, 1)) = 0
		[PerRendererData]
		_OverrideColor ("OverrideColor", Color) = (1,1,1,1)
		[PerRendererData]
		_OverrideAmount ("OverrideAmount", Range(0, 1)) = 0
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Emission("Emission", 2D) = "black"
		_EmissionScale("Emission Scale", Float) = 1
		_EmissionSinFrequency("EmissionSinFrequency", Float) = 1
		_EmissionSinScale("EmissionSinScale", Float) = 1
		_EmissionMinimum("EmissionOffset", Float) = 0
		_NormalMap("Normal Map", 2D) = "white" {}
		_OutlineWidth("Outline Width", Float) = 0.1
		[PerRendererData]
    	_Outline("Outline", Int) = 0
		[PerRendererData]
		_OutlineColor("Outline Color", Color) = (0.96, 0, 0.34, 1)
	}
	SubShader {
	Cull Off
	    Tags { 
	        "Queue"="Transparent" 
	        "RenderType"="Transparent"
	        "PreviewType"="Plane"
	     }
		//LOD 200
		
		//OcclusionPass
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
			fixed4 frag (v2f i) : SV_Target {
			    fixed4 rawCol = tex2D(_MainTex, i.uv);
				fixed4 col = _OccludeColor;
				col.a = rawCol.a * _OccludeAmount;
				return col;
			}
        ENDCG
        
        }
        // Surface Pass
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
		    float4 color: COLOR;
			float2 uv_MainTex : TEXCOORD;
		};

		sampler2D _Emission;
		float4 _OverrideColor;
        float _EmissionMinimum;
        float _EmissionSinScale;
        float _EmissionScale;
        float _EmissionSinFrequency;
        float maximum(float a, float b) {
            return a > b ? a : b;
        }
		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 textureColor = tex2D (_MainTex, IN.uv_MainTex) * IN.color;
			
			o.Alpha = textureColor.a * _OverrideColor.a;
			o.Albedo = textureColor;
			fixed4 c = tex2D(_Emission, IN.uv_MainTex);
			fixed4 e = c;
			float intensity = e.a * _EmissionMinimum;
			intensity += (sin(_Time.x *_EmissionSinFrequency) / 2 + 0.5) * _EmissionSinScale * e.a;
            o.Emission = e * intensity * _EmissionScale;
			
			//o.Albedo = c;
			//o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
            o.Normal = fixed3(0,0,1);
            //o.Emission = tex2D(_Emission, IN.uv_MainTex);
			
		}
		ENDCG
		
		//OurlinePass
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
            float _OutlineWidth;
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
                    fixed2 yOffset = fixed2(0, _OutlineWidth * _MainTex_TexelSize.y);
                    fixed2 xOffset = fixed2(_OutlineWidth * _MainTex_TexelSize.x, 0);
                    fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + yOffset);
                    fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - yOffset);
                    fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + xOffset);
                    fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - xOffset);

                    // If one of the neighbouring pixels is invisible, we render an outline.
                    if (pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a <= 0) {
                        return _OutlineColor;
                    }
                }

                return Transparent;
            }
            ENDCG
        }
        Pass {
            ZTest Always
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
			fixed4 _OverrideColor;
			fixed _OverrideAmount;
			fixed4 frag (v2f i) : SV_Target {
			    fixed4 rawCol = tex2D(_MainTex, i.uv);
				fixed4 col = _OverrideColor;
				col.a = rawCol.a * _OverrideAmount;
				return col;
			}
        ENDCG
      }
	}
	FallBack "Diffuse"
}