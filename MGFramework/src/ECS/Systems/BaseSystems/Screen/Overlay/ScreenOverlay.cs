using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Screen;

public enum OverlayType
{
    Transition
}

/// <summary>
/// Overlay over a game screen.
/// Examples: Black transition overlay, weather overlays, etc.
/// </summary>
public class ScreenOverlay
{
    /// <summary>
    /// Type of overlay (Transition, weather, etc.)
    /// </summary>
    public OverlayType OverlayType;

    /// <summary>
    /// Flag to indicate that the overlay is active
    /// </summary>
    public bool IsActive;

    /// <summary>
    /// Reference to the game screen for the overlay
    /// </summary>
    private GameScreen GameScreen;

    public ScreenOverlay(GameScreen gameScreen, OverlayType overlayType)
    {
        GameScreen = gameScreen;
        OverlayType = overlayType;
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(GraphicsBatch graphicsBatch) { }

    /// <summary>
    /// Resets the overlay back to its original state
    /// </summary>
    public virtual void Reset() { }

    /// <summary>
    /// Starts any overlay animations or events
    /// </summary>
    public virtual void Start() { }

    /// <summary>
    /// Stops any overlay animations or events
    /// </summary>
    public virtual void Stop() { }
}