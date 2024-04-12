using FavobeanGames.MGFramework.Cameras;
using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameWindow = FavobeanGames.MGFramework.Screen.GameWindow;

namespace FavobeanGames.MGFramework.ECS;

/// <summary>
/// Graphics manager class contains all graphic rendering logic
/// </summary>
public class BaseRenderSystem : EntityDrawSystem
{
    private readonly Game game;

    private bool gameWindowIsSet;
    private readonly GameWindow gameWindow;
    private Camera camera;

    private GraphicsDevice graphicsDevice;
    private GraphicsBatch graphicsBatch;
    private GraphicsRenderingOptions graphicsRenderingOptions;

    /// <summary>
    /// Flag, when enabled will show the bounding box, and velocity direction of the graphic
    /// </summary>
    public bool DebugMode { get; set; }

    public BaseRenderSystem(Game game, int screenWidth, int screenHeight, GraphicsRenderingOptions graphicsRenderingOptions)
        :base (Aspect.SetAspect(typeof(Graphic)))
    {
        this.game = game;
        gameWindow = new GameWindow(game, screenWidth, screenHeight);

        graphicsDevice = game.GraphicsDevice;
        graphicsBatch = new GraphicsBatch(graphicsDevice);
        this.graphicsRenderingOptions = graphicsRenderingOptions ?? GraphicsRenderingOptions.DefaultRenderingOptions;
    }

    public BaseRenderSystem(Game game, GameWindow gameWindow, GraphicsRenderingOptions graphicsRenderingOptions)
        :base (Aspect.SetAspect(typeof(Graphic)))
    {
        this.game = game;
        this.gameWindow = gameWindow;

        graphicsDevice = game.GraphicsDevice;
        graphicsBatch = new GraphicsBatch(graphicsDevice);
        this.graphicsRenderingOptions = graphicsRenderingOptions ?? GraphicsRenderingOptions.DefaultRenderingOptions;
    }

    /// <summary>
    /// Sets the graphics device render target to screens target
    /// </summary>
    public void SetWindow()
    {
        if (gameWindowIsSet)
        {
            System.Diagnostics.Debug.WriteLine("GameWindow.Set() warning: RenderTarget is already set");
            return;
        }

        graphicsDevice.SetRenderTarget(gameWindow.Target);
        gameWindowIsSet = true;
    }

    /// <summary>
    /// Sets the graphics device render target to the back buffer
    /// </summary>
    public void UnSetWindow()
    {
        if (!gameWindowIsSet)
        {
            System.Diagnostics.Debug.WriteLine("GameWindow.UnSet() warning: RenderTarget is not set");
            return;
        }

        game.GraphicsDevice.SetRenderTarget(null);
        gameWindowIsSet = false;
    }

    /// <summary>
    /// Renders scene graphics to screens render target
    /// </summary>
    public void PresentWindow()
    {
        game.GraphicsDevice.Clear(Color.Black);
        gameWindow.Draw(graphicsBatch);
    }

    /// <summary>
    /// Sets the camera for the graphics batch to inform how to render graphics
    /// </summary>
    /// <param name="newCamera"></param>
    public void SetCamera(Camera newCamera)
    {
        this.camera = newCamera;
    }

    /// <summary>
    /// Unsets the camera for the graphics batch to inform how to render graphics
    /// </summary>
    /// <param name="camera"></param>
    public void UnsetCamera()
    {
        camera = null;
    }

    public void Draw(Entity entity)
    {
        // graphicsBatch.Draw(entity.Graphic);
    }

    public void Draw(params Graphic[] graphics)
    {
        graphicsDevice.Clear(Color.Black);

        SetWindow();

        graphicsBatch.Begin(camera);

        foreach (var graphic in graphics)
        {
            graphicsBatch.Draw(graphic);
        }

        graphicsBatch.End();

        UnSetWindow();
        PresentWindow();
    }
    public override void Draw(params Entity[] entities)
    {
        graphicsDevice.Clear(Color.Black);

        SetWindow();

        graphicsBatch.Begin(camera);

        foreach (Entity entity in entities)
        {
            entity.Draw(graphicsBatch);
        }

        graphicsBatch.End();

        UnSetWindow();
        PresentWindow();
    }
}