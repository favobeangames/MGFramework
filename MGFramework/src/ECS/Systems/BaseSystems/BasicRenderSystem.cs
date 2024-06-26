﻿using FavobeanGames.MGFramework.Cameras;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.MGFramework.ECS;

/// <summary>
/// Graphics manager class contains all graphic rendering logic
/// </summary>
public class BaseRenderSystem : EntityDrawSystem
{
    protected readonly Game game;

    private bool gameWindowIsSet;
    protected readonly GameWindow gameWindow;
    protected Camera camera;

    protected GraphicsDevice graphicsDevice;
    protected GraphicsBatch graphicsBatch;
    protected GraphicsRenderingOptions graphicsRenderingOptions;

    protected ComponentMapper<Sprite> spriteMapper;
    protected ComponentMapper<Polygon> polygonMapper;
    protected ComponentMapper<Circle> circleMapper;

    /// <summary>
    /// Flag, when enabled will show the bounding box, and velocity direction of the graphic
    /// </summary>
    public bool DebugMode { get; set; }

    public BaseRenderSystem(GraphicsDeviceManager graphics, int screenWidth, int screenHeight, GraphicsRenderingOptions graphicsRenderingOptions)
        :base (Aspect.Any(typeof(Sprite), typeof(Shape)))
    {
        gameWindow = new GameWindow(graphics, screenWidth, screenHeight);

        graphicsDevice = graphics.GraphicsDevice;
        graphicsBatch = new GraphicsBatch(graphicsDevice);
        this.graphicsRenderingOptions = graphicsRenderingOptions ?? GraphicsRenderingOptions.DefaultRenderingOptions;
    }

    public BaseRenderSystem(Game game, GameWindow gameWindow, GraphicsRenderingOptions graphicsRenderingOptions)
        :base (Aspect.Any(typeof(Sprite), typeof(Polygon), typeof(Circle)))
    {
        this.game = game;
        this.gameWindow = gameWindow;

        graphicsDevice = game.GraphicsDevice;
        graphicsBatch = new GraphicsBatch(graphicsDevice);
        this.graphicsRenderingOptions = graphicsRenderingOptions ?? GraphicsRenderingOptions.DefaultRenderingOptions;
    }

    public BaseRenderSystem(Game game, GameWindow gameWindow, GraphicsRenderingOptions graphicsRenderingOptions, AspectBuilder builder)
        :base (builder)
    {
        this.game = game;
        this.gameWindow = gameWindow;

        graphicsDevice = game.GraphicsDevice;
        graphicsBatch = new GraphicsBatch(graphicsDevice);
        this.graphicsRenderingOptions = graphicsRenderingOptions ?? GraphicsRenderingOptions.DefaultRenderingOptions;
    }

    protected override void Initialize(IComponentService componentService)
    {
        spriteMapper = componentService.GetMapper<Sprite>();
        polygonMapper = componentService.GetMapper<Polygon>();
        circleMapper = componentService.GetMapper<Circle>();
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

    public override void Draw()
    {
        graphicsDevice.Clear(Color.Black);

        SetWindow();

        graphicsBatch.Begin(camera);

        foreach (var entityId in ActiveEntities)
        {
            var sprite = spriteMapper.Get(entityId);
            var polygon = polygonMapper.Get(entityId);
            var circle = circleMapper.Get(entityId);

            if (sprite != null)
                graphicsBatch.Draw(sprite);
            if (polygon != null)
                graphicsBatch.Draw(polygon);
            if (circle != null)
                graphicsBatch.Draw(circle);
        }

        graphicsBatch.End();

        UnSetWindow();
        PresentWindow();
    }
}