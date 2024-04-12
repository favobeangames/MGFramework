using System;
using System.Collections.Generic;
using System.Diagnostics;
using FavobeanGames.Components;
using FavobeanGames.Components.Input;
using FavobeanGames.MGFramework.Components;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using FavobeanGames.MGFramework.Physics;
using FavobeanGames.MGFramework.Util;
using FBFramework.Components.GameScreen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace fb_framework_test_sandbox;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private GraphicsManager graphicsManager;
    
    private Screen screen;
    private GameScreen gameScreen1;
    private Camera camera;
    private InputManager input;

    private List<Entity> entities;
    private World world;

    private Stopwatch watch;
    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.SynchronizeWithVerticalRetrace = true;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        DisplayMode dm = this.GraphicsDevice.DisplayMode;
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.ApplyChanges();

        screen = new Screen(this, 1280, 720);
        gameScreen1 = new GameScreen(new RectangleF(0, 0, 1920, 1080));

        entities = new List<Entity>();
        camera = new Camera(screen, CameraOptions.PerspectiveCameraOptions);
        camera.SetCurrentGameScreen(gameScreen1, true);
        camera.SetZoom(2);
        camera.GetExtents(out RectangleF cameraExtents);
        input = new InputManager(camera, screen);

        world = new World();
        graphicsManager = new GraphicsManager(this, GraphicsRenderingOptions.DefaultRenderingOptions);
        screen.GraphicsLayers.AddLayer(new GraphicsLayer(LayerKey.BaseMapKey, GraphicsDevice, 1280, 720));

        // Static base
        float width = 480;
        float height = 48;

        Polygon bottomPad = new Polygon(new[]
        {
            new Vector2(-width/2f, -height/2f),
            new Vector2(width/2f, -height/2f),
            new Vector2(width/2f, height/2f),
            new Vector2(-width/2f, height/2f),
        }, RandomHelper.RandomColor(), 1, Color.White);
        bottomPad.Position = new Vector2(960, 350);
        world.AddBody(new RigidBody(true, 1f, 1f, 1f, 1f, bottomPad));
        screen.GraphicsLayers.AddGraphicsToLayer(LayerKey.BaseMapKey, bottomPad);

        float ledgeWidth = 240f;
        float ledgeHeight = 32f;

        Polygon ledge1 = new Polygon(new[]
        {
            new Vector2(-ledgeWidth/2f, -ledgeHeight/2f),
            new Vector2(ledgeWidth/2f, -ledgeHeight/2f),
            new Vector2(ledgeWidth/2f, ledgeHeight/2f),
            new Vector2(-ledgeWidth/2f, ledgeHeight/2f),
        }, RandomHelper.RandomColor(), 1, Color.White);
        ledge1.Position = new Vector2(740, 560);
        ledge1.Rotation = MathHelper.TwoPi / 20f;

        world.AddBody(new RigidBody(true, 1f, 1f, 1f, 1f, ledge1));
        screen.GraphicsLayers.AddGraphicsToLayer(LayerKey.BaseMapKey, ledge1);

        Polygon ledge2 = new Polygon(new[]
        {
            new Vector2(-ledgeWidth/2f, -ledgeHeight/2f),
            new Vector2(ledgeWidth/2f, -ledgeHeight/2f),
            new Vector2(ledgeWidth/2f, ledgeHeight/2f),
            new Vector2(-ledgeWidth/2f, ledgeHeight/2f),
        }, RandomHelper.RandomColor(), 1, Color.White);
        ledge2.Position = new Vector2(1150, 640);
        ledge2.Rotation = -MathHelper.TwoPi / 20f;

        world.AddBody(new RigidBody(true, 1f, 1f, 0f, 1f, ledge2));
        screen.GraphicsLayers.AddGraphicsToLayer(LayerKey.BaseMapKey, ledge2);

        watch = new Stopwatch();

        base.Initialize();
    }
    
    protected override void LoadContent()
    {
        graphicsManager.LoadScreen(screen);
    }

    /// <summary>
    /// Update game components
    /// </summary>
    /// <param name="gameTime"></param>
    protected override void Update(GameTime gameTime)
    {
        input.Update(gameTime);

        if (input.MouseLeftButtonPressed())
        {
            float width = RandomHelper.RandomInt(8, 32);
            float height = RandomHelper.RandomInt(8, 32);

            Polygon newSquare = new Polygon(new[]
            {
                new Vector2(-width/2f, -height/2f),
                new Vector2(width/2f, -height/2f),
                new Vector2(width/2f, height/2f),
                new Vector2(-width/2f, height/2f),
            }, RandomHelper.RandomColor(), 1, Color.White);
            newSquare.Position = input.GetMouseWorldPosition();

            RigidBody body = new RigidBody(false, 1f, RandomHelper.RandomFloat(.25f, 5f),
                RandomHelper.RandomFloat(0.5f, 0.85f), 1f, newSquare);

            world.AddBody(body);
            screen.GraphicsLayers.AddGraphicsToLayer(LayerKey.BaseMapKey, newSquare);

            Entity entity = new Entity(newSquare, body);
            entities.Add(entity);
        }

        if (input.MouseRightButtonPressed())
        {
            float radius = RandomHelper.RandomInt(8, 24);

            Circle newCircle = new Circle(
                input.GetMouseWorldPosition(),
                radius,
                24,
                RandomHelper.RandomColor(),
                1,
                Color.White);

            RigidBody body = new RigidBody(false, 1f, RandomHelper.RandomFloat(.25f, 5f),
                RandomHelper.RandomFloat(0.5f, 0.85f), 1f, newCircle);

            world.AddBody(body);
            screen.GraphicsLayers.AddGraphicsToLayer(LayerKey.BaseMapKey, newCircle);

            Entity entity = new Entity(newCircle, body);
            entities.Add(entity);
        }

        if (input.IsKeyPressed(Keys.Escape))
            Exit();

        if (input.IsKeyPressed(Keys.OemMinus))
        {
            this.camera.DecrementZoom();
        }

        if (input.IsKeyPressed(Keys.OemPlus))
        {
            this.camera.IncrementZoom();
        }

        if (input.IsKeyPressed(Keys.Q))
        {
            this.camera.GetExtents(out Vector2 min, out Vector2 max);
            Debug.WriteLine("CamMin: " + min);
            Debug.WriteLine("CamMax: " + max);

            camera.GetExtents(out Vector2 tl, out Vector2 tr, out Vector2 bl, out Vector2 br);
            Debug.WriteLine("CamTopLeft: " + tl);
            Debug.WriteLine("CamTopRight: " + tr);
            Debug.WriteLine("CamBottomLeft: " + bl);
            Debug.WriteLine("CamBottomRight: " + br);
        }

        if (input.IsKeyPressed(Keys.E))
        {
            screen.GraphicsLayers.GetLayers().ForEach((g) =>
            {
                g.Graphics.ForEach((e) =>
                {
                    Debug.WriteLine($"Layer Data: ID: {e.LayerData.LayersId} Key: {e.LayerData.LayerKey.Key}");
                });
            });
        }

        if (input.IsKeyPressed(Keys.C))
        {
            graphicsManager.DebugMode = !graphicsManager.DebugMode;
        }

        if (input.IsKeyPressed(Keys.OemTilde))
        {
            Debug.WriteLine($"BodyCount: {world.BodyCount}");
            Debug.WriteLine($"StepTime: {Math.Round(watch.Elapsed.TotalMilliseconds, 4)}");
            Debug.WriteLine("");
        }

        watch.Restart();
        world.Step(gameTime.GetElapsedSeconds(), new Vector2(0, -50f), 12);
        watch.Stop();

        camera.Update(gameTime);
        graphicsManager.Update(gameTime);

        base.Update(gameTime);
    }

    /// <summary>
    /// Draw game components to screen
    /// </summary>
    /// <param name="gameTime"></param>
    protected override void Draw(GameTime gameTime)
    {
        // Draw Base Map 
        screen.Set();
        GraphicsDevice.Clear(Color.CornflowerBlue);

        graphicsManager.DrawGraphics(camera);

        screen.UnSet();
        screen.Present(graphicsManager);

        base.Draw(gameTime);
    }
}