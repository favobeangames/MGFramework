using System.Collections.Generic;
using FavobeanGames.MGFramework;
using FavobeanGames.MGFramework.Cameras;
using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using FavobeanGames.MGFramework.Input;
using FavobeanGames.MGFramework.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using RacketRivalsCC.Match;
using RacketRivalsCC.Screens;
using RacketRivalsCC.Systems;
using GameWindow = FavobeanGames.MGFramework.GameWindow;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Transform2 = FavobeanGames.MGFramework.Transform2;

namespace RacketRivalsCC;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private GameWorld gameWorld;
    private BaseRenderSystem renderSystem;
    private ScreenSystem screenSystem;

    private Dictionary<string, BitmapFont> fonts;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        fonts = new Dictionary<string, BitmapFont>();
    }

    protected override void Initialize()
    {
        graphics.PreferredBackBufferWidth = Constants.BASE_SCREEN_WIDTH;
        graphics.PreferredBackBufferHeight = Constants.BASE_SCREEN_HEIGHT;
        graphics.ApplyChanges();

        // Load in all textures for use
        Assets.Initialize(Content);

        var testPage = new TestPage();

        screenSystem = new ScreenSystem(testPage);

        var gameWindow = new GameWindow(this, Constants.BASE_GAME_WIDTH, Constants.BASE_GAME_HEIGHT);
        renderSystem = new RenderSystem(
            this,
            gameWindow,
            GraphicsRenderingOptions.DefaultRenderingOptions,
            screenSystem);

        var camera = new Camera(gameWindow, CameraOptions.OrthographicCameraOptions);
        camera.MoveTo(new Vector2(150, 65));
        camera.SetZoom(3);
        renderSystem.SetCamera(camera);

        var cameraSystem = new CameraSystem(camera);

        gameWorld = new GameWorldBuilder()
            .AddSystem(screenSystem)
            .AddSystem(new CollisionSystem(Vector2.Zero, 4, new WorldOptions(CollisionResolutionType.Basic)))
            .AddSystem(new PlayerSystem())
            .AddSystem(new MatchSystem())
            .AddSystem(new ServingSystem())
            .AddSystem(new ShadowSystem())
            .AddSystem(cameraSystem)
            .AddSystem(renderSystem)
            .Build();

        var player = gameWorld.CreateEntity();
        var playerTexture = Assets.Get("testMatchAvatarSheet");
        var position = new Vector2(150, 50);
        var playerOrigin = new Vector2(16, 16);

        // var playerTransform = new Transform2
        // {
        //     Position = position,
        // };
        //var playerObj = new GameObject(12, 20, playerTransform, playerOrigin);

        // var playerBodyTransform = new Transform2
        // {
        //     Position = position + new Vector2(0, playerObj.Height / 2),
        //     Vertices = new[]
        //     {
        //         new Vector2(-5, 3f),
        //         new Vector2(5, 3f),
        //         new Vector2(5, -3f),
        //         new Vector2(-5, -3f)
        //     }
        // };
        //playerObj.CreateGameObjectBody(player.Id, playerBodyTransform, player);

        //cameraSystem.SetFollowTransform(playerTransform);
        cameraSystem.SetViewingBounds(new RectangleF(35f, 20f, 230f, 120f));

        var playerSpriteSheet = new SpriteSheet();
        playerSpriteSheet.AddAnimation("idle",
            true,
            new AnimationFrame(new Rectangle(32, 0, 32, 32)),
            new AnimationFrame(new Rectangle(0,0,32,32)),
            new AnimationFrame(new Rectangle(32, 0, 32, 32)),
            new AnimationFrame(new Rectangle(64, 0, 32, 32)));
        playerSpriteSheet.AddAnimation("serveMovement",
            new AnimationFrame(new Rectangle(0, 64, 32, 32)));
        playerSpriteSheet.AddAnimation("serveThrow",
            new AnimationFrame(new Rectangle(0, 32, 32, 32)));
        playerSpriteSheet.AddAnimation("serveHit",
            new AnimationFrame(new Rectangle(32, 32, 32, 32), .1f),
            new AnimationFrame(new Rectangle(64, 32, 32, 32), .1f));
        playerSpriteSheet.SetAnimation("idle");

        // var playerSprite = new Sprite(playerTexture, playerSpriteSheet, playerTransform, playerOrigin)
        // {
        //     LayerDepth = 1,
        // };

        var playerInput = new InputManager(camera, gameWindow);

        playerInput.ControlMapping.AddControl("moveLeft", new InputControl(Buttons.LeftThumbstickLeft, Keys.A));
        playerInput.ControlMapping.AddControl("moveRight", new InputControl(Buttons.LeftThumbstickRight, Keys.D));
        playerInput.ControlMapping.AddControl("moveUp", new InputControl(Buttons.LeftThumbstickUp, Keys.W));
        playerInput.ControlMapping.AddControl("moveDown", new InputControl(Buttons.LeftThumbstickDown, Keys.S));
        playerInput.ControlMapping.AddControl("action", new InputControl(Buttons.A, Keys.Space));
        var playerComp = new Player(camera, playerInput);
        var playerStats = new Stats();
        playerStats.SetPower(1);
        playerStats.SetSpeed(4.5f);
        playerStats.SetSpin(1);
        playerComp.InAMatch = true;
        player.AttachComponent(playerComp);
        // player.AttachComponent(playerObj);
        // player.AttachComponent(playerTransform);
        // player.AttachComponent(playerSprite);
        player.AttachComponent(playerStats);
        // player.AttachComponent(new Polygon(playerBodyTransform, Color.Red * .5f));
        // playerObj.CreateGameObjectShadow(playerTransform, player);
        player.AttachComponent(new MatchPlayer());

        // Load the new current screen
        screenSystem.LoadCurrentScreen(testPage);

        // Initialize a test match
        var match = gameWorld.CreateEntity();
        //match.AttachComponent(new MatchObject(Courts.BasicCourt(), 2));

        base.Initialize();
    }

    protected override void LoadContent()
    {

    }

    protected override void Update(GameTime gameTime)
    {
        gameWorld.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        gameWorld.Draw();

        base.Draw(gameTime);
    }
}