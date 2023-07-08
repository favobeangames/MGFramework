using System.Collections.Generic;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.MGFramework.Graphics;

/// <summary>
/// A GraphicsLayer represents the graphics that should be rendered
/// on the same layer. This will give us control on rendering graphics
/// behind/in front of other graphics.
/// </summary>
public class GraphicsLayer
{
    private bool isDisposed;
    
    public LayerKey Key;

    public RenderTarget2D RenderTarget;
    public List<Graphic> Graphics { get; }

    public GraphicsLayer()
    {
            
    }
    public GraphicsLayer(LayerKey key)
    {
        Key = key;
        Graphics = new List<Graphic>();
    }
    public GraphicsLayer(LayerKey key, GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
    {
        Key = key;
        RenderTarget = new RenderTarget2D(graphicsDevice, screenWidth, screenHeight);
        Graphics = new List<Graphic>();
    }

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }
        
        RenderTarget?.Dispose();
        isDisposed = true;
    }
}