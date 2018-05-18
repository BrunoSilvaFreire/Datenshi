Shader "Unlit/AbsoluteColorShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {
            "Queue" = "Transparent"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }
            
            sampler2D _MainTex;
            
            fixed4 frag (v2f i) : SV_Target {
                fixed4 c = i.color;
                fixed texAlpha =tex2D(_MainTex, i.uv).a;
                if (texAlpha < c.a) {
                    c.a = texAlpha;
                }
                return c;
            }
            ENDCG
        }
    }
}