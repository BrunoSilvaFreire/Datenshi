Shader "Unlit/UIOcclusionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,0,1,1)
        _OccludeColor("Occlude Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags {
            "Queue"="Transparent" 
	        "RenderType"="Transparent"
	    }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
		        #pragma vertex vert
			    #pragma fragment frag
    			
    			#include "UnityCG.cginc"

	    		struct appdata
		    	{
			    	float4 vertex : POSITION;
				    float2 uv : TEXCOORD;
				    float4 color : COLOR;
			    };

			    struct v2f {
				    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD;
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
			    fixed4 _Color;
		    	fixed4 frag (v2f i) : SV_Target {
		    	    fixed4 color = i.color;
		    	    color.a = tex2D(_MainTex, i.uv).a * _Color.a;
				    return color;
			    }
            ENDCG
        }
        Tags {
            "Queue"="Transparent" 
	        "RenderType"="Transparent"
	    }
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
		    	fixed4 _Color;
		    	fixed4 frag (v2f i) : SV_Target {
				    fixed4 col = _OccludeColor;
				    col.a = tex2D(_MainTex, i.uv).a * _Color.a;
				    return col;
			    }
            ENDCG
        }
      
    }
}