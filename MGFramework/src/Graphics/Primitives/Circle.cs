using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Circle : Primitive
{
    private float radius;
    private int points;

    public Circle()
    {
        
    }

    public Circle(Vector2 center, float radius, int points, Color fillColor)
        : base(fillColor)
    {
        Position = center;
        this.radius = radius;
        this.points = points;
    }

    public Circle(Vector2 center, float radius, int points, Color fillColor, int outlineThickness, Color outlineColor)
        : base(outlineColor, outlineThickness, fillColor)
    {
        Position = center;
        this.radius = radius;
        this.points = points;
    }

    public override void Draw(PrimitiveBatch primitiveBatch)
    {
        // X and Y are the origin of the circle when rendering. The TransformMatrix
        // supplies the position in which it should be translated to after rendering.
        primitiveBatch.DrawCircleFill(0, 0, radius, points, FillColor, TransformMatrix);
        if (HasOutline)
        {
            primitiveBatch.DrawCircle(0, 0, radius, points, OutlineThickness, OutlineColor, TransformMatrix);
        }
    }
}