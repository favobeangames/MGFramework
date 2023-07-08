using System;
using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.Components;

public class Screen: IDisposable
{
    private static readonly int MinDim = 64;
    private static readonly int MaxDim = 4096;
    
    private bool isDisposed;
    private RenderTarget2D target;
    private bool isSet;
    private Game game;
    
    public int Width => target.Width;
    public int Height => target.Height;
    
    public RenderTarget2D Target => target;
    public Rectangle ScreenDestRect => CalculateDestinationRectangle();
    
    public GraphicsLayers GraphicsLayers { get; }

    public Screen()
    {
        
    }

    public Screen(Game game, int width, int height)
    {
        width = Math.Clamp(width, MinDim, MaxDim);
        height = Math.Clamp(height, MinDim, MaxDim);

        this.game = game;
        target = new RenderTarget2D(this.game.GraphicsDevice, width, height);

        GraphicsLayers = new GraphicsLayers();
    }

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }
        
        target?.Dispose();
        isDisposed = true;
    }

    /// <summary>
    /// Sets the graphics device render target to screens target
    /// </summary>
    /// <exception cref="Exception">Throws exception if the screens render target is already set</exception>
    public void Set()
    {
        if (isSet)
        {
            throw new Exception("Render target is already set.");
        }
        
        game.GraphicsDevice.SetRenderTarget(target);
        isSet = true;
    }

    /// <summary>
    /// Sets the graphics device render target to the back buffer
    /// </summary>
    /// <exception cref="Exception">Throws exception if the screens render target is not set</exception>
    public void UnSet()
    {
        if (!isSet)
        {
            throw new Exception("Render target is not set.");
        }
        game.GraphicsDevice.SetRenderTarget(null);
        isSet = false;
    }

    /// <summary>
    /// Renders scene graphics to screens render target
    /// </summary>
    /// <param name="graphicsManager"></param>
    /// <exception cref="Exception">Throws exception if graphics manager is null</exception>
    public void Present(GraphicsManager graphicsManager)
    {
        if (graphicsManager is null)
        {
            throw new Exception("Graphics Manager is null.");
        }

#if DEBUG 
        game.GraphicsDevice.Clear(Color.OrangeRed);
#else
        game.GraphicsDevice.Clear(Color.Black);
#endif
        
        graphicsManager.DrawScreen();
    }

    /// <summary>
    /// Calculates the screen positions and dimensions based on the graphic devices
    /// bounds compared to the screens aspect ratio.
    /// </summary>
    /// <returns>Rectangle with screen dimensions that fit within the window</returns>
    private Rectangle CalculateDestinationRectangle()
    {
        Rectangle backBufferBounds = game.GraphicsDevice.PresentationParameters.Bounds;
        float backBufferAspectRatio = (float)backBufferBounds.Width / backBufferBounds.Height;
        float screenAspectRatio = (float)Width / Height;

        float rx = 0f;
        float ry = 0f;
        float rw = backBufferBounds.Width;
        float rh = backBufferBounds.Height;
        
        if (backBufferAspectRatio > screenAspectRatio)
        {
            rw = rh * screenAspectRatio;
            rx = (backBufferBounds.Width - rw) / 2f;
        } 
        else if (backBufferAspectRatio < screenAspectRatio)
        {
            rh = rw / screenAspectRatio;
            ry = (backBufferBounds.Height - rh) / 2f;
        }

        return new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
    }
}