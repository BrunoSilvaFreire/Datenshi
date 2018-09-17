Shader "Datenshi/Glitch" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchNoise("Glitch Noise", 2D) = "black" {}
        _GridColor("Grid Color", Color) = (1,0,1,1)
        _GridHeight("Grid Height", Float) = 1
        _GridMoveSpeed("Grid Move Speed", Float) = 50
    }
    SubShader {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }
            
            sampler2D _MainTex;
            sampler2D _GlitchNoise;
            fixed _GridHeight;
            fixed4 _GridColor;
            float2 _MainTex_TexelSize;
            fixed _GridMoveSpeed;
            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.uv;
                float texelSize = _MainTex_TexelSize.y;
                float heightPos = uv.y;
                float pixel = (heightPos + _Time.x * _GridMoveSpeed) / texelSize;
                if (pixel % (_GridHeight * 2) > _GridHeight){
                    return _GridColor;
                }
                fixed4 base = tex2D(_MainTex, uv) * i.color;
                //fixed4 noise = tex2D(_GlitchNoise, i.uv);
                return base;
            }
            ENDCG
        }
    }
}