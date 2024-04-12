#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 texelSize;
int shadow;
Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
  float4 Color : COLOR0;
  float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
    if (!color.a)
    {
        float2 offsetX = float2(texelSize.x, 0);
        float2 offsetY = float2(0, texelSize.y);
        float alpha = color.a;

        alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates + offsetX).a);
        alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetX).a);
        alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates + offsetY).a);
        alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY).a);
        alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY - offsetX).a);
        alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates + offsetY + offsetX).a);
        alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY + offsetX).a);
        alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates + offsetY - offsetX).a);

        if (alpha)
        {
            return float4(1, 1, 1, alpha);
        }

        if (shadow == 1)
        {
            alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetX - offsetX).a);
          	alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY - offsetY).a);
          	alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates + offsetY - offsetX - offsetX).a);
          	alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY - offsetX - offsetX).a);
          	alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY - offsetY - offsetY).a);
          	alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY - offsetY + offsetX).a);
          	alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY - offsetY - offsetX - offsetX).a);
          	alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY - offsetY - offsetY - offsetX).a);
          	alpha = max(alpha, tex2D(SpriteTextureSampler, input.TextureCoordinates - offsetY - offsetY - offsetY - offsetX - offsetX).a);

          	if (alpha)
          	{
          	    return float4(0.09f, 0.09f, 0.09f, alpha);
          	}
        }
    }

	return color;
}

technique BasicColorDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};