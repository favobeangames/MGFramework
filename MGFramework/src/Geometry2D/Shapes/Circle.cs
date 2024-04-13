using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Geometry2D.Shapes;

public class Circle : Geometry
{
    /// <summary>
    /// Center point of the circle
    /// If a transform object is set for the geometry then the center
    /// will represent the transforms position
    /// </summary>
    public Vector2 Center => Transform2?.Position ?? new Vector2(X, Y);

    public Circle(float x, float y, float radius)
    {
        X = x;
        Y = y;
        Radius = radius;
        Width = radius * 2f;
        Height = radius * 2f;

        GeometryType = GeometryType.Circle;
    }

    public Circle(Vector2 center, float radius)
    {
        X = center.X;
        Y = center.Y;
        Radius = radius;
        Width = radius * 2f;
        Height = radius * 2f;

        GeometryType = GeometryType.Circle;
    }

    public Circle(Transform2 transform2, float radius)
        : base(transform2)
    {
        Radius = radius;
        Width = radius * 2f;
        Height = radius * 2f;

        GeometryType = GeometryType.Circle;

        UpdateTransformedVertices();
        UpdateAabb();
    }
}