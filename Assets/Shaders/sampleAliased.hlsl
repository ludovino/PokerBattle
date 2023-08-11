//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


float4 SampleAliasedSprite(float2 uvIn,float4 texelSize, Texture2D tex, SamplerState sampler_tex)
{
	float2 boxSize = clamp(fwidth(uvIn) * texelSize.zw, 1e-5, 1);
    float2 tx = uvIn * texelSize.zw - 0.5 * boxSize;
    float2 txOffset = saturate((frac(tx) - (1 - boxSize)) / boxSize);
    float2 uv = (floor(tx) + 0.5 + txOffset) * texelSize.xy;
    return SAMPLE_TEXTURE2D_GRAD(tex, sampler_tex,  uv, ddx(uvIn), ddy(uvIn));	
}
