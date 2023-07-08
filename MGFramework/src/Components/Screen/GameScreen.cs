using System.Collections.Generic;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FBFramework.Components.GameScreen;

public class GameScreen
{
    public RectangleF Bounds;

    public List<Graphic> MapGraphics;
    private Polygon background;

    public GameScreen(RectangleF bounds)
    {
        MapGraphics = new List<Graphic>();

        Bounds = bounds;
        background = Primitive.Rectangle;
        background.Scale = new Vector2(bounds.Width, bounds.Height);
        background.Position = new Vector2(bounds.X, bounds.Y);

        MapGraphics.Add(background);
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(GraphicsBatch graphicsBatch)
    {
        graphicsBatch.Draw(background);
    }
}