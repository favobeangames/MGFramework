using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Geometry2D.Shapes;

public class Polygon : Geometry
{
    /// <summary>
    /// Lines of the polygon
    /// </summary>
    public readonly List<Line> ExteriorVertices;

    public Polygon(params Vector2[] points)
    {
        ExteriorVertices = new List<Line>();
        Vertices = points;

        GeometryType = GeometryType.Polygon;
    }

    public Polygon(List<Vector2> points)
    {
        ExteriorVertices = new List<Line>();
        Vertices = points.ToArray();
        UpdateLines();

        GeometryType = GeometryType.Polygon;
    }

    public Polygon(Transform2 transform2, params Vector2[] points)
        : base(transform2)
    {
        ExteriorVertices = new List<Line>();
        Vertices = points;
        UpdateLines();

        GeometryType = GeometryType.Polygon;

        UpdateTransformedVertices();
        UpdateAabb();
    }

    public Polygon(Transform2 transform2, List<Vector2> points)
        : base(transform2)
    {
        ExteriorVertices = new List<Line>();
        Vertices = points.ToArray();
        UpdateLines();

        GeometryType = GeometryType.Polygon;

        UpdateTransformedVertices();
        UpdateAabb();
    }

    /// <summary>
    /// Creates lines for the points of the polygon
    /// </summary>
    private void UpdateLines()
    {
        ExteriorVertices.Clear();
        for (int i = 0; i < Vertices.Length; i++)
        {
            ExteriorVertices.Add(new Line(Vertices[i], Vertices[(i + 1) % Vertices.Length]));
        }
    }
}