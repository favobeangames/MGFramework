using FavobeanGames.MGFramework.Cameras;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameWindow = FavobeanGames.MGFramework.GameWindow;

namespace DemoSandbox.Demos;

/// <summary>
/// Demo class contains the base world functionality of what the demo will contain
/// These objects control how the demo will function and render objects
/// Will also enable us to switch between them so we only need to store one project
/// </summary>
public abstract class Demo
{
    /// <summary>
    /// Title of the demo
    /// </summary>
    public string Title;
    public abstract Demo Initialize(GameWindow gameWindow);

    public abstract Demo LoadContent(ContentManager content);
    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GraphicsBatch graphicsBatch);
}