using System.Collections.Generic;
using DemoSandbox.Demos;
using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;
using GameWindow = FavobeanGames.MGFramework.GameWindow;

namespace DemoSandbox;

public class Game1 : Game
{
    private const int screenWidth = 1280;
    private const int screenHeight = 720;
    private const int gameWidth = 1280;
    private const int gameHeight = 720;

    private GraphicsDeviceManager graphics;
    private GraphicsBatch graphicsBatch;
    private GameWindow gameWindow;

    private List<Demo> availableDemos;

    // State variables
    private bool onSelectScreen = true;
    private int currentRunningDemo = 0;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        gameWindow = new GameWindow(graphics, screenWidth, screenHeight, gameWidth, gameHeight);

        availableDemos = new List<Demo>
        {
            new GeometryDemo(),
        };

        base.Initialize();
    }

    protected override void LoadContent()
    {
        graphicsBatch = new GraphicsBatch(GraphicsDevice);

        availableDemos.ForEach(d =>
        {
            d.LoadContent(Content).Initialize(gameWindow);
        });
    }

    protected override void Update(GameTime gameTime)
    {
        if (onSelectScreen)
        {
            // Selecting a demo
            if (availableDemos.Count == 1)
            {
                currentRunningDemo = 0;
                onSelectScreen = false;
            }
        }
        else
        {
            // Update our current demo
            availableDemos[currentRunningDemo].Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        if (onSelectScreen)
        {
            // Selecting a demo
        }
        else
        {
            // Render our current demo objects
            availableDemos[currentRunningDemo].Draw(graphicsBatch);
        }

        base.Draw(gameTime);
    }
}