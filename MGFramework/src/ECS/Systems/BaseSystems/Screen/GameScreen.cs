using System.Collections.Generic;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.ECS;

public abstract class GameScreen
{
    /// <summary>
    /// Bounds of the game screen
    /// </summary>
    public RectangleF Bounds { get; }

    public GameScreen(RectangleF bounds)
    {
        Bounds = bounds;
    }

    public virtual void Initialize(GameWorld gameWorld) { }

    /// <summary>
    /// Loads anything needed for the screen
    /// Called before the screen is rendered to the window
    /// </summary>
    /// <param name="gameWorld">GameWorld instance to allow the creation of entities</param>
    /// <returns>Collection of entity ids that were created</returns>
    public abstract IEnumerable<int> LoadScreen(GameWorld gameWorld);

    /// <summary>
    /// Unloads anything for the screen
    /// Called when the screen is stopped rendering to the window
    /// </summary>
    public abstract void UnloadScreen(GameWorld gameWorld);
}