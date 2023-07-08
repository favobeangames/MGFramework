using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Polygon : Primitive
{
    private Vector2[] vertices;
    private int[] triangleIndices;

    public Polygon(Vector2[] vertices, Color fillColor)
        : base(fillColor)
    {
        this.vertices = vertices;
        CalculateTriangleIndices();
    }
    public Polygon(Vector2[] vertices, Color fillColor, int thickness, Color outlineColor)
        :base(outlineColor, thickness, fillColor)
    {
        this.vertices = vertices;
        CalculateTriangleIndices();
    }

    public override void Draw(PrimitiveBatch primitiveBatch)
    {
        if (FillColor != Color.Transparent)
        {
            primitiveBatch.DrawPolygonFill(vertices, triangleIndices, FillColor, TransformMatrix);
        }

        if (HasOutline)
        {
            primitiveBatch.DrawPolygon(vertices, OutlineThickness, OutlineColor, TransformMatrix);
        }
    }

    /// <summary>
    /// Calculates the indices of the triangles needed to render to create
    /// a filled polygon
    /// </summary>
    private void CalculateTriangleIndices()
    {
        int indicesCount = (vertices.Length - 2) * 3;
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