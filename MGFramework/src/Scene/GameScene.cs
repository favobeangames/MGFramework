using System.Collections.Generic;
using FavobeanGames.MGFramework.Cameras;
using Microsoft.Xna.Framework;

namespace FBFramework.Scenes;

/// <summary>
/// A GameScene is essentially a cutscene.
/// </summary>
public class GameScene
{
    /// <summary>
    /// Flag to determine if all steps on the scene have finished
    /// </summary>
    public bool Finished;

    private List<SceneStep> sceneSteps;
    private SceneStep currentStep;

    public GameScene(Camera camera)
    {
        sceneSteps = new List<SceneStep>();
    }

    public void LoadSteps(params SceneStep[] steps)
    {
        if (steps != null && steps.Length > 0)
        {
            sceneSteps.AddRange(steps);
            currentStep = steps[0];
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        if (!Finished)
        {
            UpdateStep(gameTime);
        }
    }

    private void UpdateStep(GameTime gameTime)
    {
        if (sceneSteps.Count == 0)
        {
            Finished = true;
            return;
        }
        currentStep ??= sceneSteps[0];

        currentStep.Update(gameTime);
    }
}