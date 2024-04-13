using System;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Line : Shape
{
    private float thickness;

    public Line()
    {
        ShapeType = ShapeType.Line;
    }

    public Line(float thickness, Vector2 startPosition, Vector2 endPosition, Color fillColor)
    {
        this.thickness = thickness;
        Geometry = new Geometry2D.Shapes.Line(startPosition, endPosition);
        FillColor = fillColor;
        ShapeType = ShapeType.Line;
    }

    public Line(Transform2 transform2, Vector2 startPosition, Vector2 endPosition, float thickness, Color fillColor)
        : base(transform2, fillColor)
    {
        Geometry = new Geometry2D.Shapes.Line(startPosition, endPosition);
        this.thickness = thickness;
        ShapeType = ShapeType.Line;
    }

    public override void Draw(ShapeBatch shapeBatch)
    {
        if (Geometry.Vertices is null)
        {
#if DEBUG
            throw new NullReferenceException("Line must contain Vertices. Vertices were null");
#endif
            return;
        }

        for (int i = 0; i <= Geometry.Vertices.Length - 2; i++)
        {
            shapeBatch.DrawLine(Geometry.Vertices[i], Geometry.Vertices[i+1], thickness, FillColor);
        }
    }
}