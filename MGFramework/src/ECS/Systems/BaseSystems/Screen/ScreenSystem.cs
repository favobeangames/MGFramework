using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collections;

namespace FavobeanGames.MGFramework.ECS;

/// <summary>
/// Delegate to notify consumers when the screen is loaded to the window
/// </summary>
public delegate void LoadScreenDelegate(GameScreen gameScreen);

/// <summary>
/// Delegate to notify consumers when the screen is unloaded from the window
/// </summary>
public delegate void UnloadScreenDelegate(GameScreen gameScreen);

public class ScreenSystem : EntityUpdateSystem
{
    private List<GameScreen> gameScreens;
    /// <summary>
    /// Reference to the current screen to be rendered on the window
    /// </summary>
    private GameScreen currentGameScreen;

    /// <summary>
    /// Collection of entity ids for the current screens entities
    /// </summary>
    private Bag<int> currentScreenEntities;

    public RectangleF CurrentScreenBounds => currentGameScreen?.Bounds ?? RectangleF.Empty;

    public ScreenSystem(params GameScreen[] screens)
        : base(Aspect.Any( typeof(GameScreen)))
    {
        gameScreens = screens?.ToList();
        currentScreenEntities = new Bag<int>(1024);
    }

    protected override void Initialize(IComponentService componentService)
    {
        gameScreens?.ForEach(s =>
        {
            s.Initialize(gameWorld);
        });
    }

    /// <summary>
    /// Sets the current screen to be viewed
    /// </summary>
    /// <param name="gameScreen"></param>
    public void SetCurrentScreen(GameScreen gameScreen, bool transitionToScreen = false)
    {
        currentGameScreen = gameScreen;
    }

    public void UnloadCurrentScreen()
    {
        if (currentGameScreen != null)
        {
            currentGameScreen.UnloadScreen(gameWorld);
            currentGameScreen = null;
        }
    }

    public void LoadCurrentScreen(GameScreen gameScreen)
    {
        UnloadCurrentScreen();
        currentGameScreen = gameScreen;
        gameScreen.LoadScreen(gameWorld);
    }

    public override void Update(GameTime gameTime)
    {

    }
}