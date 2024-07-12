#include "Structures.fxh"

Texture2D Texture;
sampler2D TextureSampler = sampler_state {
    Texture = <Texture>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

float4x4 WorldViewProj;
float ScreenPixelRange;
float4 Color;

float median(float3 rgb) {
    return max(min(rgb.r, rgb.g), min(max(rgb.r, rgb.g), rgb.b));
}

// Without Depth

VSOut_PosColorTexture VSWithoutDepth(VSIn_PosColorTexture input) {
    VSOut_PosColorTexture output;

    output.Position = mul(float4(input.Position.x, input.Position.y, 0.0f, 1.0f), WorldViewProj);
    output.PositionPS = input.Position;
    output.Diffuse = Color * input.Color;
    output.TextureCoord = input.TextureCoord;

    return output;
}

float4 PSWithoutDepth(PSIn_PosColorTexture input) : COLOR0 {
    float4 px = tex2D(TextureSampler, input.TextureCoord);
    float screenPxDistance = ScreenPixelRange * (median(px.rgb) - .5f);
    float alpha = clamp(screenPxDistance + .5f, 0.0f, 1.0f);

    px = float4(input.Diffuse.rgb, 1.0f) * input.Diffuse.a * alpha;

    if (px.a < .01f) {
        discard;
    }

    return px;
}

// With Depth

VSOut_PosColorTextureDepth VSWithDepth(VSIn_PosColorTexture input) {
    VSOut_PosColorTextureDepth output;

    output.Position = mul(float4(input.Position.x, input.Position.y, 0.0f, 1.0f), WorldViewProj);
    output.PositionPS = input.Position;
    output.Depth = input.Position.z / input.Position.w;
    output.Diffuse = Color * input.Color;
    output.TextureCoord = input.TextureCoord;

    return output;
}

PSOut_ColorDepth PSWithDepth(PSIn_PosColorTextureDepth input) {
    PSOut_ColorDepth psOut;
    psOut.Color = tex2D(TextureSampler, input.TextureCoord);
    float screenPxDistance = ScreenPixelRange * (median(psOut.Color.rgb) - .5f);
    float alpha = clamp(screenPxDistance + .5f, 0.0f, 1.0f);

    psOut.Color = float4(input.Diffuse.rgb, 1.0f) * input.Diffuse.a * alpha;

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
