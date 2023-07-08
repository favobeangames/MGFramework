using System.Diagnostics;
using System.Linq;
using FavobeanGames.Components;
using FavobeanGames.Components.Input;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using FBFramework.Components.GameScreen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace fb_framework_test_sandbox;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private GraphicsManager graphicsManager;
    
    private Screen screen;
    private GameScreen gameScreen1;
    private Camera camera;
    private InputManager input;

    private Player player;
    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.ApplyChanges();

        int screenWidth = 1280;
        int screenHeight = 720;

        screen = new Screen(this, screenWidth, screenHeight);
        gameScreen1 = new GameScreen(new RectangleF(0, 0, 2560, 1440));
        camera = new Camera(screen, CameraOptions.PerspectiveCameraOptions);
        input = new InputManager();

        player = new Player(input);
        player.Position = new Vector2(0, 0);

        camera.SetEntityToFollow(player);

        base.Initialize();
    }
    
    protected override void LoadContent()
    {
        graphicsManager = new GraphicsManager(this, GraphicsRenderingOptions.DefaultRenderingOptions);

        screen.GraphicsLayers.AddLayer(LayerKey.BaseMapKey, gameScreen1.MapGraphics.First(), player.Graphic);

        graphicsManager.LoadScreen(screen);
    }

    /// <summary>
    /// Update game components
    /// </summary>
    /// <param name="gameTime"></param>
    protected override void Update(GameTime gameTime)
    {
        input.Update(gameTime);

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

        if (input.IsKeyPressed(Keys.NumPad6))
        {
            camera.Move(new Vector2(10, 0));
        }
        if (input.IsKeyPressed(Keys.NumPad4))
        {
            camera.Move(new Vector2(-10, 0));
        }
        if (input.IsKeyPressed(Keys.NumPad8))
        {
            camera.Move(new Vector2(0, 10));
        }
        if (input.IsKeyPressed(Keys.NumPad2))
        {
            camera.Move(new Vector2(0, -10));
        }

        if (input.IsKeyPressed(Keys.C))
        {
            if (camera.CurrentGameScreen == null)
            {
                camera.SetCurrentGameScreen(gameScreen1, true);
            }
            else
            {
                camera.ResetCurrentGameScreen();
            }
        }

        player.Update(gameTime);
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