using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FBFramework.Scenese;

public class SceneStep
{
    /// <summary>
    /// Flag to determine if the step has completed
    /// </summary>
    public bool Finished;

    /// <summary>
    /// Function that stores the logic of the step
    /// </summary>
    /// <example>
    /// <code>
    /// var perform = (GameTime gameTime) => {
    ///     var seconds = gameTime.getElapsedSeconds();
    ///     var stepFinished = camera.LerpTo(newPosition);
    ///     return stepFinished;
    /// }
    /// </code>
    /// </example>
    private Func<GameTime, bool> performStep;

    public SceneStep(Func<GameTime, bool> performStep)
    {
        this.performStep = performStep;
    }

    public virtual void Update(GameTime gameTime)
    {
        if (!Finished)
        {
            Finished = performStep(gameTime);
        }
    }
}