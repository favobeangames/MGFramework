using System;
using FavobeanGames.Components;
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
    private Game game;
    private SpriteBatch spriteBatch;
    private PrimitiveBatch primitiveBatch;
    private BasicEffect effect;

    /// <summary>
    /// GraphicsBatch handles batch drawing of graphics to the screen
    /// </summary>
    /// <param name="game">MonoGame Game instance</param>
    /// <exception cref="ArgumentNullException">Throws exception if Game parameter is null</exception>
    public GraphicsBatch(Game game)
    {
        this.game = game ?? throw new ArgumentNullException("game");

        spriteBatch = new SpriteBatch(this.game.GraphicsDevice);
        primitiveBatch = new PrimitiveBatch(this.game);
        
        effect = new BasicEffect(this.game.GraphicsDevice);
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
            Viewport vp = game.GraphicsDevice.Viewport;
            effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, 0, vp.Height, 0f, 1f);
            effect.View = Matrix.Identity;
        }
        else
        {
            camera.UpdateMatrices();
            effect.View = camera.View;
            effect.Projection = camera.Projection;
        }

        spriteBatch.Begin(
            blendState: BlendState.AlphaBlend, 
            samplerState: SamplerState.PointClamp, 
            rasterizerState: RasterizerState.CullNone, 
            effect: effect);
        
        primitiveBatch.Begin(camera);
    }

    public void End()
    {
        spriteBatch.End();
        primitiveBatch.End();
    }

    public void Draw(Sprite sprite)
    {
        sprite.Draw(spriteBatch);
    }

    public void Draw(Graphic graphic)
    {
        switch (graphic.GraphicType)
        {
            case GraphicType.Primitive:
                graphic.Draw(primitiveBatch);
                break;
            case GraphicType.Sprite:
                graphic.Draw(spriteBatch);
                break;
        }
    }

    public void DrawTarget(RenderTarget2D target, Rectangle destRect, Color color)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(target, destRect, color);
        spriteBatch.End();
    }
    
    public void Dispose()
    {
        if (isDisposed) return;
        
        spriteBatch?.Dispose();
        primitiveBatch?.Dispose();
        isDisposed = true;
    }
}