using System;
using System.Collections.Generic;
using System.Diagnostics;
using FavobeanGames.MGFramework;
using FavobeanGames.MGFramework.Cameras;
using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using FavobeanGames.MGFramework.Input;
using FavobeanGames.MGFramework.Physics;
using FavobeanGames.MGFramework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using GameWindow = FavobeanGames.MGFramework.GameWindow;
using Transform2 = FavobeanGames.MGFramework.Transform2;


namespace fb_framework_test_sandbox;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private BaseRenderSystem renderSystem;
    
    private GameWindow gameWindow;
    private GameScreen gameScreen1;
    private Camera camera;
    private InputManager input;

    private List<Graphic> shapes;
    private World world;

    private Stopwatch watch;
    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.SynchronizeWithVerticalRetrace = true;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        shapes = new List<Graphic>();
    }

    protected override void Initialize()
    {
        DisplayMode dm = this.GraphicsDevice.DisplayMode;
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.ApplyChanges();

        gameWindow = new GameWindow(graphics, 1280, 720);

        camera = new Camera(gameWindow, CameraOptions.PerspectiveCameraOptions);
        camera.SetZoom(2);
        camera.GetExtents(out RectangleF cameraExtents);
        input = new InputManager(camera, gameWindow);

        world = new World(new Vector2(0, -50f), new WorldOptions(CollisionResolutionType.BaseWithRotationalAndFriction));
        renderSystem = new BaseRenderSystem(this, gameWindow, GraphicsRenderingOptions.DefaultRenderingOptions);
        renderSystem.SetCamera(camera);

        // Static base
        float width = 480;
        float height = 48;

        Transform2 bPadTransform = new Transform2(
            new Vector2(cameraExtents.X + cameraExtents.Width / 2, cameraExtents.Top),
            Vector2.One,
            0f);

        Polygon bottomPad = new Polygon(bPadTransform, new[]
        {
            new Vector2(-width / 2f, -height / 2f),
            new Vector2(width / 2f, -height / 2f),
            new Vector2(width / 2f, height / 2f),
            new Vector2(-width / 2f, height / 2f),
        }, RandomHelper.RandomColor(), 1, Color.White);

        var bottomPadBody = new RigidBody(0, true, 1f, 1f, 1f, 1f, ShapeType.Polygon, bPadTransform, bottomPad.Geometry);
        world.AddBody(bottomPadBody);
        shapes.Add(bottomPad);

        float ledgeWidth = 240f;
        float ledgeHeight = 32f;

        Transform2 ledge1Transform = new Transform2(
            new Vector2(cameraExtents.X + cameraExtents.Width / 2 - ledgeWidth,
                cameraExtents.Top + (cameraExtents.Height * 0.5f)),
            Vector2.One,
            MathHelper.TwoPi / 20f);

        Polygon ledge1 = new Polygon(ledge1Transform, new[]
        {
            new Vector2(-ledgeWidth/2f, -ledgeHeight/2f),
            new Vector2(ledgeWidth/2f, -ledgeHeight/2f),
            new Vector2(ledgeWidth/2f, ledgeHeight/2f),
            new Vector2(-ledgeWidth/2f, ledgeHeight/2f),
        }, RandomHelper.RandomColor(), 1, Color.White);
        var ledge1Body = new RigidBody(0, true, 1f, 1f, 1f, 1f, ShapeType.Polygon, ledge1Transform, ledge1.Geometry);
        world.AddBody(ledge1Body);
        shapes.Add(ledge1);

        Transform2 ledge2Transform = new Transform2(
            new Vector2(cameraExtents.X + cameraExtents.Width / 2 + ledgeWidth,
                cameraExtents.Top + (cameraExtents.Height * 0.65f)),
            Vector2.One,
            -MathHelper.TwoPi / 20f);

        Polygon ledge2 = new Polygon(ledge2Transform, new[]
        {
            new Vector2(-ledgeWidth/2f, -ledgeHeight/2f),
            new Vector2(ledgeWidth/2f, -ledgeHeight/2f),
            new Vector2(ledgeWidth/2f, ledgeHeight/2f),
            new Vector2(-ledgeWidth/2f, ledgeHeight/2f),
        }, RandomHelper.RandomColor(), 1, Color.White);
        var ledge2Body = new RigidBody(0, true, 1f, 1f, 0f, 1f, ShapeType.Polygon, ledge2Transform, ledge2.Geometry);
        world.AddBody(ledge2Body);
        shapes.Add(ledge2);

        watch = new Stopwatch();

        base.Initialize();
    }
    
    protected override void LoadContent()
    {

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
            float width = RandomHelper.RandomInt(4, 12);
            float height = RandomHelper.RandomInt(4, 12);

            Transform2 newTrans = new Transform2(input.GetMouseWorldPosition(), Vector2.One, 0f);
            Polygon newSquare = new Polygon(newTrans, new[]
            {
                new Vector2(-width/2f, -height/2f),
                new Vector2(width/2f, -height/2f),
                new Vector2(width/2f, height/2f),
                new Vector2(-width/2f, height/2f),
            }, RandomHelper.RandomColor(), 1, Color.White);
            RigidBody body = new RigidBody(0, false, 1f, RandomHelper.RandomFloat(.25f, 5f),
                RandomHelper.RandomFloat(0.5f, 0.85f), 1f, ShapeType.Polygon, newTrans, newSquare.Geometry);
            world.AddBody(body);
            shapes.Add(newSquare);
        }

        if (input.MouseRightButtonPressed())
        {
            float radius = RandomHelper.RandomInt(8, 24);

            Transform2 newTrans = new Transform2(input.GetMouseWorldPosition(), Vector2.One, 0f);

            Circle newCircle = new Circle(
                newTrans,
                radius,
                24,
                RandomHelper.RandomColor(),
                1,
                Color.White);

            RigidBody body = new RigidBody(0, false, 1f, RandomHelper.RandomFloat(.25f, 5f),
                RandomHelper.RandomFloat(0.5f, 0.85f), 1f, ShapeType.Circle, newTrans, newCircle.Geometry);
            world.AddBody(body);
            shapes.Add(newCircle);
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

        if (input.IsKeyPressed(Keys.OemTilde))
        {
            Debug.WriteLine($"BodyCount: {world.BodyCount}");
            Debug.WriteLine($"StepTime: {Math.Round(watch.Elapsed.TotalMilliseconds, 4)}");
            Debug.WriteLine("");
        }

        watch.Restart();
        world.Step(gameTime.GetElapsedSeconds(), 12);
        watch.Stop();

        camera.Update(gameTime);

        base.Update(gameTime);
    }

    /// <summary>
    /// Draw game components to screen
    /// </summary>
    /// <param name="gameTime"></param>
    protected override void Draw(GameTime gameTime)
    {
        // Draw Base Map
        renderSystem.Draw(shapes.ToArray());

        base.Draw(gameTime);
    }
}