using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Polygon : Shape
{
    private int[] triangleIndices;
    private Vector2[] vertices;

    public Polygon(Transform2 transform2, Vector2[] vertices, Color fillColor)
        : base(fillColor)
    {
        this.vertices = vertices;
        Geometry = new Geometry2D.Shapes.Polygon(transform2, vertices);
        CalculateTriangleIndices();
        FillColor = fillColor;
        ShapeType = ShapeType.Polygon;
    }
    public Polygon(Transform2 transform2, Vector2[] vertices,  Color fillColor, int thickness, Color outlineColor)
        : base(outlineColor, thickness, fillColor)
    {
        Geometry = new Geometry2D.Shapes.Polygon(transform2, vertices);
        CalculateTriangleIndices();

        ShapeType = ShapeType.Polygon;
    }

    public static Polygon PolygonFromRectangle(RectangleF rect)
    {
        Transform2 transform2 = Transform2.Empty;
        var vertices = new[]
        {
            new Vector2(rect.Left, rect.Top),
            new Vector2(rect.Right, rect.Top),
            new Vector2(rect.Right, rect.Bottom),
            new Vector2(rect.Left, rect.Bottom),
            new Vector2(rect.Left, rect.Top),
        };
        return new Polygon(transform2, vertices, Color.Transparent, 2, Color.Black);
    }

    public override void Draw(ShapeBatch shapeBatch)
    {
        if (Geometry.Vertices is null)
        {
#if DEBUG
            throw new NullReferenceException("Polygon must contain Vertices. Vertices were null");
#endif
            return;
        }

        if (FillColor != Color.Transparent)
        {
            shapeBatch.DrawPolygonFill(Geometry.Vertices, triangleIndices, FillColor, Transform2.TransformMatrix);
        }

        if (HasOutline)
        {
            shapeBatch.DrawPolygon(Geometry.Vertices, OutlineThickness, OutlineColor, Transform2.TransformMatrix);
        }
    }

    /// <summary>
    /// Returns a list of vertices transformed into world space
    /// </summary>
    /// <returns>Array of Vector2</returns>
    public override Vector2[] GetTransformedVertices()
    {
        return Geometry.TransformedVertices;
    }

    /// <summary>
    /// Calculates the indices of the triangles needed to render to create
    /// a filled polygon
    /// </summary>
    private void CalculateTriangleIndices()
    {
        int indicesCount = (Geometry.Vertices.Length - 2) * 3;
        triangleIndices = new int[indicesCount];

        int indexCount = 1;
        for (int i = 0; i < indicesCount; i+=3)
        {
            triangleIndices[i] = 0;
            triangleIndices[i + 1] = indexCount;
            triangleIndices[i + 2] = indexCount + 1;
            indexCount++;
        }
    }
}