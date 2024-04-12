using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Geometry2D.Shapes;

public class Shape : Geometry
{
    private List<Vector2> vertices;
    /// <summary>
    /// Exterior points of the polygon
    /// </summary>
    public new List<Vector2> Vertices
    {
        get => vertices;
        set
        {
            vertices = value;
            UpdateLines();
        }
    }

    /// <summary>
    /// Lines of the polygon
    /// </summary>
    public readonly List<Line> ExteriorVertices;

    public Shape(params Vector2[] points)
    {
        ExteriorVertices = new List<Line>();
        Vertices = points.ToList();

        GeometryType = GeometryType.Polygon;
    }

    public Shape(List<Vector2> points)
    {
        ExteriorVertices = new List<Line>();
        Vertices = points;

        GeometryType = GeometryType.Polygon;
    }

    public Shape(Transform2 transform2, params Vector2[] points)
        : base(transform2)
    {
        ExteriorVertices = new List<Line>();
        Vertices = points.ToList();

        GeometryType = GeometryType.Polygon;
    }

    public Shape(Transform2 transform2, List<Vector2> points)
        : base(transform2)
    {
        ExteriorVertices = new List<Line>();
        Vertices = points;

        GeometryType = GeometryType.Polygon;
    }

    /// <summary>
    /// Creates lines for the points of the polygon
    /// </summary>
    private void UpdateLines()
    {
        ExteriorVertices.Clear();
        for (int i = 0; i < Vertices.Count; i++)
        {
            ExteriorVertices.Add(new Line(Vertices[i], Vertices[(i + 1) % Vertices.Count]));
        }
    }
}