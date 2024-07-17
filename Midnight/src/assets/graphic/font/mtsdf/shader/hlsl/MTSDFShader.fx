#include "Structures.fxh"

// inspired on: https://www.redblobgames.com/x/2404-distance-field-effects/

Texture2D Texture;
sampler2D TextureSampler = sampler_state {
    Texture = <Texture>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

float4x4 u_WorldViewProj;

// mtsdf
float u_ScreenPixelRange;
float u_DistanceRange;

// font
float4 u_Color;
float u_Rounding; // 0.0: squared; >= 1.0: rounded;

// outline
float4 u_InnerOutlineColor;
float4 u_OuterOutlineColor;
float u_InnerOutlineThickness; // 0.0 ~ 2.5
float u_OuterOutlineThickness; // 0.0 ~ 0.48
float u_OutlineRounding; // 0.0: squared; >= 1.0: rounded;

// glow
float4 u_GlowColor;
float u_GlowLength;
float u_GlowIntensity;
float u_GlowRounding;
float2 u_GlowDisplacement;

// bias away from font center
const float c_OutlineBias = .25;
const float c_GlowBias = .75;

float median(float3 rgb) {
    return max(min(rgb.r, rgb.g), min(max(rgb.r, rgb.g), rgb.b));
}

// Without Depth

VSOut_PosColorTexture VSWithoutDepth(VSIn_PosColorTexture input) {
    VSOut_PosColorTexture output;

    output.Position = mul(float4(input.Position.x, input.Position.y, 0.0, 1.0), u_WorldViewProj);
    output.PositionPS = input.Position;
    output.Diffuse = u_Color * input.Color;
    output.TextureCoord = input.TextureCoord;

    return output;
}

float4 PSWithoutDepth(PSIn_PosColorTexture input) : COLOR0 {
    float4 s = tex2D(TextureSampler, input.TextureCoord);

    // * signed distance *
    float sdf = s.a;
    float msdf = median(s.rgb);

    // lerp between representations => 0.0: squared; 1.0: rounded;
    float inner = lerp(msdf, sdf, u_Rounding) - .5;
    float outline = lerp(msdf, sdf, u_OutlineRounding) - .5;

    // * inner *
    float innerOutline = clamp(u_DistanceRange * inner + .5 + c_OutlineBias - u_InnerOutlineThickness, 0.0, 1.0);

    // combine inner outline color with font color
    float4 px = lerp(u_InnerOutlineColor, input.Diffuse, innerOutline);

    // * outer *
    float outerOutline = clamp(u_DistanceRange * inner + .5 + c_OutlineBias, 0.0, 1.0);

    // combine outer outline color with previous calculated color
    px = lerp(u_OuterOutlineColor, px, outerOutline);

    // * opacity *
    // calculate frag opacity
    float outline_p = outline + u_OuterOutlineThickness;
    float opacity = clamp(u_DistanceRange * outline_p + .5 + c_OutlineBias, 0.0, 1.0);

    // * glow *
    // sample signed distance, but a displacement may be applied
    float4 glow_s = tex2D(TextureSampler, input.TextureCoord - (u_GlowDisplacement * fwidth(input.TextureCoord)));
    float glow_sdf = glow_s.a;
    float glow_msdf = median(glow_s.rgb);

    // lerp between representations => 0.0: squared; 1.0: rounded;
    float glow_d = lerp(glow_msdf, glow_sdf, u_GlowRounding);

    float glow = clamp(u_DistanceRange * outline_p + .5 + c_GlowBias, 0.0, 1.0);

    // glow only occurs when distance is <= .5 - u_OuterOutlineThickness
    // bring distance from [0.0, .5 - u_OuterOutlineThickness] to [0.0, 1.0]
    float glowNormalized = glow_d / (.5 - u_OuterOutlineThickness);

    // increase opacity based on glow length
    float m = clamp(glowNormalized + .05 * u_GlowLength, 0.0, 1.0);

    // calculate final glow opacity
    // it should not be higher than normalized
    // try to modify curve to be gentle (modified by length and intensity)
    float glowOpacity = min(glowNormalized, m - m / (1.0 + .1 * u_GlowLength)) * u_GlowIntensity;

    // combine glow color with previous calculated color
    px = lerp(u_GlowColor, px, glow);

    return px * max(opacity, glowOpacity);
}

// With Depth

VSOut_PosColorTextureDepth VSWithDepth(VSIn_PosColorTexture input) {
    VSOut_PosColorTextureDepth output;

    output.Position = mul(float4(input.Position.x, input.Position.y, 0.0, 1.0), u_WorldViewProj);
    output.PositionPS = input.Position;
    output.Depth = input.Position.z / input.Position.w;
    output.Diffuse = u_Color * input.Color;
    output.TextureCoord = input.TextureCoord;

    return output;
}

PSOut_ColorDepth PSWithDepth(PSIn_PosColorTextureDepth input) {
    PSOut_ColorDepth psOut;
    psOut.Color = tex2D(TextureSampler, input.TextureCoord);
    float screenPxDistance = u_ScreenPixelRange * (median(psOut.Color.rgb) - .5);
    float alpha = clamp(screenPxDistance + .5, 0.0, 1.0);

    psOut.Color = float4(input.Diffuse.rgb, 1.0) * input.Diffuse.a * alpha;

    if (psOut.Color.a < .01f) {
        discard;
    }

    psOut.Depth = input.Depth;
    return psOut;
}

// Techniques

technique WithoutDepth {
	pass {
		VertexShader = compile vs_3_0 VSWithoutDepth();
		PixelShader  = compile ps_3_0 PSWithoutDepth();
	}
};

technique WithDepth {
	pass {
		VertexShader = compile vs_3_0 VSWithDepth();
		PixelShader  = compile ps_3_0 PSWithDepth();
	}
};
