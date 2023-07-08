using System;
using FavobeanGames.Components;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics;

/// <summary>
/// Graphics manager class contains all graphic rendering logic
/// </summary>
public class GraphicsManager
{
    private readonly Game game;
    private GraphicsBatch graphicsBatch;
    private PrimitiveBatch primitiveBatch;

    private GraphicsRenderingOptions graphicsRenderingOptions;

    private Screen currentScreen;

    public GraphicsManager(Game game, GraphicsRenderingOptions graphicsRenderingOptions)
    {
        this.game = game ?? throw new ArgumentNullException("game");

        graphicsBatch = new GraphicsBatch(game);
        primitiveBatch = new PrimitiveBatch(game);

        this.graphicsRenderingOptions = graphicsRenderingOptions ?? GraphicsRenderingOptions.DefaultRenderingOptions;
    }

    /// <summary>
    /// Updates all graphics
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime)
    {
        foreach (var graphicsLayer in currentScreen.GraphicsLayers.GetLayers())
        {
            foreach (var graphic in graphicsLayer.Graphics)
            {
                graphic.Update(gameTime);
            }
        }
    }

    /// <summary>
    /// Draws all graphics to the screen
    /// Primitives will always be drawn over sprites due to the batch being
    /// flushed after the graphics batch.
    /// </summary>
    public void DrawGraphics(Camera camera)
    {
        graphicsBatch.Begin(camera);

        // TODO: Reorder graphics based on rendering settings
        foreach (var graphicsLayer in currentScreen.GraphicsLayers.GetLayers())
        {
            foreach (var graphic in graphicsLayer.Graphics)
            {
                graphicsBatch.Draw(graphic);
            }
        }
        
        graphicsBatch.End();
    }

    /// <summary>
    /// Draws current screen to 
    /// </summary>
    public void DrawScreen()
    {
        if (currentScreen != null)
        {
            //graphicsBatch.Begin(null);
            graphicsBatch.DrawTarget(currentScreen.Target, currentScreen.ScreenDestRect, Color.White);
            //graphicsBatch.End();
        }
    }

    /// <summary>
    /// Loads screens graphics layers to render to the screen
    /// </summary>
    /// <param name="screen">Game screen to render</param>
    public void LoadScreen(Screen screen)
    {
        currentScreen = screen;
    }
}