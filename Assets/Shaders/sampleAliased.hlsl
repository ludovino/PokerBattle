#if defined(SAMPLE_TEXTURE2D_GRAD)
float4 SampleAliasedSprite(float2 uvIn,float4 texelSize, Texture2D tex, SamplerState sampler_tex)
{
	float2 boxSize = clamp(fwidth(uvIn) * texelSize.zw, 1e-5, 1);
    float2 tx = uvIn * texelSize.zw - 0.5 * boxSize;
    float2 txOffset = saturate((frac(tx) - (1 - boxSize)) / boxSize);
    float2 uv = (floor(tx) + 0.5 + txOffset) * texelSize.xy;
    return SAMPLE_TEXTURE2D_GRAD(tex, sampler_tex,  uv, ddx(uvIn), ddy(uvIn));
}
#else
float4 SampleAliasedSprite( float4 texelSize, sampler2D tex, float2 uvIn)
{
	float2 boxSize = clamp(fwidth(uvIn) * texelSize.zw, 1e-5, 1);
    float2 tx = uvIn * texelSize.zw - 0.5 * boxSize;
    float2 txOffset = saturate((frac(tx) - (1 - boxSize)) / boxSize);
    float2 uv = (floor(tx) + 0.5 + txOffset) * texelSize.xy;
    return tex2Dgrad(tex, uv, ddx(uvIn), ddy(uvIn));
}
#endif