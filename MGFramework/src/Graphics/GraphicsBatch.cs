using System;
using FavobeanGames.MGFramework.Cameras;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace FavobeanGames.MGFramework.Graphics;

/// <summary>
/// Wrapper class of the MonoGame SpriteBatch class
/// 
/// Modifies the coordinate space that Monogame bases graphics off of,
/// so rather then (0,0) being Top-Left corner, we apply a BasicEffect
/// to change (0,0) to be Bottom-Left so Positive X moves right on the screen
/// and Positive Y moves up on the screen.
/// </summary>
public class GraphicsBatch : IDisposable
{
    private bool isDisposed;
    private GraphicsDevice graphicsDevice;
    private BasicEffect effect;

    public SpriteBatch SpriteBatch;
    public ShapeBatch ShapeBatch;

    /// <summary>
    /// GraphicsBatch handles batch drawing of graphics to the screen
    /// </summary>
    /// <param name="game">MonoGame Game instance</param>
    /// <exception cref="ArgumentNullException">Throws exception if graphicsDevice parameter is null</exception>
    public GraphicsBatch(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice ?? throw new ArgumentNullException("graphicsDevice");

        SpriteBatch = new SpriteBatch(graphicsDevice);
        ShapeBatch = new ShapeBatch(graphicsDevice);
        
        effect = new BasicEffect(graphicsDevice);
        effect.FogEnabled = false;
        effect.Texture = null;
        effect.TextureEnabled = true;
        effect.LightingEnabled = false;
        effect.VertexColorEnabled = true;
        effect.World = Matrix.Identity;
        effect.Projection = Matrix.Identity;
        effect.View = Matrix.Identity;
    }

    public void Begin(Camera camera)
    {
        if (camera is null)
        {
            Viewport vp = graphicsDevice.Viewport;
            effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, 0, vp.Height, 0f, -1f);
            effect.View = Matrix.Identity;
        }
        else
        {
            camera.UpdateMatrices();
            effect.View = camera.View;
            effect.Projection = camera.Projection;
        }

        SpriteBatch.Begin(
            blendState: BlendState.AlphaBlend, 
            samplerState: SamplerState.PointClamp, 
            rasterizerState: RasterizerState.CullNone,
            effect: effect);

        ShapeBatch.Begin(effect);
    }

    public void SpriteBatchBegin(Camera camera)
    {
        if (camera is null)
        {
            Viewport vp = graphicsDevice.Viewport;
            effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, 0, vp.Height, 0f, -1f);
            effect.View = Matrix.Identity;
        }
        else
        {
            camera.UpdateMatrices();
            effect.View = camera.View;
            effect.Projection = camera.Projection;
        }

        SpriteBatch.Begin(
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            rasterizerState: RasterizerState.CullNone,
            effect: effect);
    }

    public void SpriteBatchBegin(Effect eff)
    {
        SpriteBatch.Begin(
            sortMode: SpriteSortMode.Immediate,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            rasterizerState: RasterizerState.CullNone,
            effect: eff);
    }

    public void PrimitiveBatchBegin(Camera camera)
    {
        if (camera is null)
        {
            Viewport vp = graphicsDevice.Viewport;
            effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, 0, vp.Height, 0f, -1f);
            effect.View = Matrix.Identity;
        }
        else
        {
            camera.UpdateMatrices();
            effect.View = camera.View;
            effect.Projection = camera.Projection;
        }

        ShapeBatch.Begin(effect);
    }

    public void End()
    {
        SpriteBatch.End();
        ShapeBatch.End();
    }

    public void SpriteBatchEnd()
    {
        SpriteBatch.End();
    }

    public void PrimitiveBatchEnd()
    {
        ShapeBatch.End();
    }
    public void Draw(Sprite sprite)
    {
        sprite.Draw(SpriteBatch);
    }

    public void Draw(Graphic graphic)
    {
        switch (graphic.GraphicType)
        {
            case GraphicType.Primitive:
                graphic.Draw(ShapeBatch);
                break;
            case GraphicType.Sprite:
            case GraphicType.Text:
                graphic.Draw(SpriteBatch);
                break;
        }
    }

    public void Draw(Graphic graphic, Transform2 transform2)
    {
        switch (graphic.GraphicType)
        {
            case GraphicType.Primitive:
                graphic.Draw(ShapeBatch, transform2);
                break;
            case GraphicType.Sprite:
            case GraphicType.Text:
                graphic.Draw(SpriteBatch, transform2);
                break;
        }
    }

    public void DrawTarget(RenderTarget2D target, Rectangle destRect, Color color)
    {
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(target, destRect, color);
        SpriteBatch.End();
    }
    
    public void Dispose()
    {
        if (isDisposed) return;
        
        SpriteBatch?.Dispose();
        ShapeBatch?.Dispose();
        isDisposed = true;
    }
}