using System.Collections.Generic;
using FavobeanGames.Components;
using FavobeanGames.MGFramework.Camera;
using FavobeanGames.MGFramework.Components;
using Microsoft.Xna.Framework;

namespace FBFramework.Scenes;

/// <summary>
/// A GameScene is essentially a cutscene.
/// </summary>
public class GameScene
{
    public readonly GameScreen GameScreen;

    /// <summary>
    /// Flag to determine if all steps on the scene have finished
    /// </summary>
    public bool Finished;

    private List<SceneStep> sceneSteps;
    private SceneStep currentStep;

    public GameScene(GameScreen screen, Camera camera)
    {
        GameScreen = screen;
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