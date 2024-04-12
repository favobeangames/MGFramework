using System.Collections.Generic;
using FavobeanGames.MGFramework.DataStructures.Collections;
using FavobeanGames.MGFramework.Screen;
using Microsoft.Xna.Framework;

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
    /// <summary>
    /// Reference to the current screen to be rendered on the window
    /// </summary>
    private GameScreen currentGameScreen;

    public ScreenSystem() :base(Aspect.SetAspect(typeof(GameScreen)))
    {
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
            currentGameScreen.UnloadScreen();

            foreach (Entity entity in currentGameScreen.Entities)
            {
                gameWorld.MoveEntityToInactive(entity);
            }

            currentGameScreen = null;
        }
    }

    public void LoadNewScreen(GameScreen gameScreen)
    {
        UnloadCurrentScreen();
        gameScreen.LoadScreen();
        currentGameScreen = gameScreen;

        foreach (Entity entity in currentGameScreen.Entities)
        {
            gameWorld.MoveEntityToActive(entity);
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (currentGameScreen != null)
        {
            currentGameScreen.Update(gameTime);
            if (currentGameScreen.TransitionedToNewScreen)
            {
            }
        }
    }
}