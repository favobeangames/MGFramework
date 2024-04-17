using System;
using FavobeanGames.MGFramework.Geometry2D.Shapes;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Line : Shape
{
    private float thickness;

    public Line()
    {
        ShapeType = ShapeType.Line;
    }

    public Line(Geometry geometry, float thickness, Color fillColor)
    {
        Geometry = geometry;
        this.thickness = thickness;
        FillColor = fillColor;
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