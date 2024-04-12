using System.Collections.Generic;
using FavobeanGames.MGFramework.DataStructures.Collections;
using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Screen;

public class GameScreen
{
    /// <summary>
    /// Bounds of the game screen
    /// </summary>
    public RectangleF Bounds { get; }

    /// <summary>
    /// List of entities that reside on the screen
    /// </summary>
    public readonly EntityList Entities;

    public bool TransitionedToNewScreen;

    /// <summary>
    /// Stores the game screens that this one can transition to
    /// </summary>
    private readonly List<ScreenOverlay> screenOverlays;

    public GameScreen(RectangleF bounds)
    {
        Entities = new EntityList();
        Bounds = bounds;

        screenOverlays = new List<ScreenOverlay>();
        screenOverlays.Add(new TransitionOverlay(this));
    }

    public virtual void LoadContent(ContentManager content) { }

    public virtual void Update(GameTime gameTime)
    {
        foreach (Entity entity in Entities)
        {
            entity.Update(gameTime);
        }

        foreach (ScreenOverlay screenOverlay in screenOverlays)
        {
            screenOverlay.Update(gameTime);
        }
    }

    public virtual void Draw(GraphicsBatch graphicsBatch)
    {
        foreach (Entity entity in Entities)
        {
            entity.Draw(graphicsBatch);
        }

        foreach (ScreenOverlay screenOverlay in screenOverlays)
        {
            screenOverlay.Draw(graphicsBatch);
        }
    }

    public void StartScreenTransition()
    {
        ScreenOverlay overlay = screenOverlays.Find(o => o.OverlayType == OverlayType.Transition);
        overlay?.Start();
    }

    /// <summary>
    /// Loads anything needed for the screen
    /// Called before the screen is rendered to the window
    /// </summary>
    public virtual void LoadScreen()
    {
        TransitionedToNewScreen = false;
    }

    /// <summary>
    /// Unloads anything for the screen
    /// Called when the screen is stopped rendering to the window
    /// </summary>
    public virtual void UnloadScreen() { }
}