﻿using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Geometry2D.Shapes;

public class Line : Geometry
{
    /// <summary>
    /// Starting point of the line
    /// </summary>
    public Vector2 Start { get; set; }

    /// <summary>
    /// Ending point of the line
    /// </summary>
    public Vector2 End { get; set; }

    public Line(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
        Vertices = new[]
        {
            Start,
            End
        };
        GeometryType = GeometryType.Line;
    }
    public Line(Transform2 transform2, Vector2 start, Vector2 end)
        : base(transform2)
    {
        Start = start;
        End = end;
        Vertices = new[]
        {
            Start,
            End
        };
        GeometryType = GeometryType.Line;

        UpdateTransformedVertices();
        UpdateAabb();
    }

    public override void UpdateVertices(Vector2[] vertices)
    {
        base.UpdateVertices(vertices);
        Start = vertices[0];
        End = vertices[1];
    }
}