using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;

namespace FavobeanGames.MGFramework.ECS;

public class TransitionOverlay : ScreenOverlay
{
    public bool Visible;

    private Tweener tweener;

    private float transitionRate;

    private Polygon transitionCover;

    public TransitionOverlay(GameScreen gameScreen, float transitionRate = 1f)
        :base(gameScreen, OverlayType.Transition)
    {
        tweener = new Tweener();
        this.transitionRate = transitionRate;

        // Create polygon to cover the screen
        RectangleF b = gameScreen.Bounds;
    }

    public override void Update(GameTime gameTime)
    {
        tweener.Update(gameTime.GetElapsedSeconds());
    }

    public override void Draw(GraphicsBatch graphicsBatch)
    {

    }

    /// <summary>
    /// Sets the visibility of the transition overlay for the screen
    /// </summary>
    public override void Start()
    {
        Visible = true;
        IsActive = true;

        tweener.TweenTo(
                target: transitionCover,
                expression: cover => cover.Alpha,
                toValue: Visible ? 1f : 0,
                duration: transitionRate,
                delay: 0)
            .Easing(EasingFunctions.Linear)
            .OnEnd(_ => IsActive = false);
    }

    public override void Stop()
    {
        // Stops any in progress tweens in progress
        tweener.CancelAll();

        IsActive = false;
    }

    public override void Reset()
    {
        // Complete any in progress tweens in progress before
        // starting a new one.
        tweener.CancelAndCompleteAll();

        Visible = false;
        IsActive = false;
    }
}