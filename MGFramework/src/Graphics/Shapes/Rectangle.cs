using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Rectangle : Shape
{
    private int width;
    private int height;
    private Vector2 position;

    public Rectangle()
    {
        
    }

    public Rectangle(Vector2 position, int width, int height, Color fillColor)
    {
        this.position = position;
        this.width = width;
        this.height = height;
        FillColor = fillColor;
    }

    public override void Draw(ShapeBatch shapeBatch)
    {
        shapeBatch.DrawRectangle(position.X, position.Y, width, height, FillColor);
    }
}