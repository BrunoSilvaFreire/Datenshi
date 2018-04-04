Shader "Datenshi/BlackAndWhiteShader"
{
    Properties
    {
        _Amount("Amount", Range(0,1)) = 0
        _DarkenAmount("Darken Amount", Range(0,1)) =0    
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
	{
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			fixed _Amount;
			fixed _DarkenAmount;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 bw = (col.r + col.g + col.b) / 3;
				col = lerp(col, bw, _Amount);
				col.rgb -= _DarkenAmount * _Amount;
				return col;
			}
			ENDCG
		}
	}
}