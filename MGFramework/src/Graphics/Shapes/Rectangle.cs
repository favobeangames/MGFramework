using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Rectangle : Primitive
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

    public override void Draw(PrimitiveBatch primitiveBatch)
    {
        primitiveBatch.DrawRectangle(position.X, position.Y, width, height, FillColor);
    }
}