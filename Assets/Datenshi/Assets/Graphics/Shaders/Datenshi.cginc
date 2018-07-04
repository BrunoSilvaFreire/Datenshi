struct SurfaceInput {
    float4 color : COLOR;
    float4 uv : TEXCOORD;
};

struct DatenshiInput {
    sampler2D mainTex;
    sampler2D emissiveTex;
    fixed4 mainColor;
    fixed4 overrideColor;
    float overrideAmount;
};

void HandleSurface(SurfaceInput input, DatenshiInput datenshi, inout SurfaceOutputStandard o) {
    fixed4 textureColor = tex2D(datenshi.mainTex, input.uv_MainTex) * mainColor;
    o.Alpha = textureColor.a;
    o.Albedo = lerp(textureColor.rgb, datenshi.overrideColor.rgb, datenshi.overrideAmount);
    fixed4 e = tex2D(_Emission, IN.uv_MainTex);
    o.Emission = e * e.a;
    o.Normal = fixed3(0,0,1);
    if (length(c.rgb) < 0.001) {
	    o.Albedo = fixed3(0,0,0);
    }
}