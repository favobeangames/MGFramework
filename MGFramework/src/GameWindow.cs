using System;
using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.MGFramework;

public class GameWindow: IDisposable
{
    private static readonly int MinDim = 64;
    private static readonly int MaxDim = 4096;
    
    private bool isDisposed;
    private GraphicsDeviceManager graphics;

    private RenderTarget2D target;
    public RenderTarget2D Target => target;

    public int ScreenWidth;

    public int ScreenHeight;
    public int GameWidth => target.Width;
    public int GameHeight => target.Height;
    public Rectangle ScreenDestRect => CalculateDestinationRectangle();
    public GameWindow()
    {
        
    }

    public GameWindow(GraphicsDeviceManager graphics, int width, int height)
    {
        width = System.Math.Clamp(width, MinDim, MaxDim);
        height = System.Math.Clamp(height, MinDim, MaxDim);

        ScreenWidth = width;
        ScreenHeight = height;

        this.graphics = graphics;

        target = new RenderTarget2D(this.graphics.GraphicsDevice, width, height);

        this.graphics.PreferredBackBufferWidth = width;
        this.graphics.PreferredBackBufferHeight = height;
        this.graphics.ApplyChanges();
    }

    public GameWindow(GraphicsDeviceManager graphics, int screenWidth, int screenHeight, int gameWidth, int gameHeight)
    {
        screenWidth = System.Math.Clamp(screenWidth, MinDim, MaxDim);
        screenHeight = System.Math.Clamp(screenHeight, MinDim, MaxDim);

        ScreenWidth = screenWidth;
        ScreenHeight = screenHeight;

        this.graphics = graphics;

        target = new RenderTarget2D(this.graphics.GraphicsDevice, gameWidth, gameHeight);

        this.graphics.PreferredBackBufferWidth = screenWidth;
        this.graphics.PreferredBackBufferHeight = screenHeight;
        this.graphics.ApplyChanges();
    }

    public void Draw(GraphicsBatch graphicsBatch)
    {
        graphicsBatch.DrawTarget(target, ScreenDestRect, Color.White);
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
    /// Calculates the screen positions and dimensions based on the graphic devices
    /// bounds compared to the screens aspect ratio.
    /// </summary>
    /// <returns>Rectangle with screen dimensions that fit within the window</returns>
    public Rectangle CalculateDestinationRectangle()
    {
        Rectangle backBufferBounds = graphics.GraphicsDevice.PresentationParameters.Bounds;
        float backBufferAspectRatio = (float)backBufferBounds.Width / backBufferBounds.Height;
        float screenAspectRatio = (float)GameWidth / GameHeight;

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