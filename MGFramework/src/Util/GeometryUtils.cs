using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Util;

public static class GeometryUtils
{

    private const float LeftAngle = (float) MathHelper.Pi;
    private const float TopAngle = (float) (1.5 * MathHelper.Pi);
    private const float RightStartAngle = 0;
    private const float RightEndAngle = (float) (2 * MathHelper.Pi);
    private const float BotAngle = (float) (.5 * MathHelper.Pi);

    public static Vector2[] CreateRoundedRectangleVertices(RectangleF rect, float radius, int points)
    {
        List<Vector2> verts = new List<Vector2>();

        // Clockwise from the top-left, generate the vertex segments
        var tlCenter = new Vector2(rect.Left + radius, rect.Top + radius);
        var trCenter = new Vector2(rect.Right - radius, rect.Top + radius);
        var brCenter = new Vector2(rect.Right - radius, rect.Bottom - radius);
        var blCenter = new Vector2(rect.Left + radius, rect.Bottom - radius);

        var tlSegments = CreateCircleSegment(tlCenter, radius, points, LeftAngle, TopAngle);
        var trSegments = CreateCircleSegment(trCenter, radius, points, TopAngle, RightEndAngle);
        var brSegments = CreateCircleSegment(brCenter, radius, points, RightStartAngle, BotAngle);
        var blSegments = CreateCircleSegment(blCenter, radius, points, BotAngle, LeftAngle);

        verts.AddRange(tlSegments);
        verts.AddRange(trSegments);
        verts.AddRange(brSegments);
        verts.AddRange(blSegments);

        return verts.ToArray();
    }

    public static Vector2[] CreateRoundedRectangleVertices(RectangleF rect, int points)
    {
        List<Vector2> verts = new List<Vector2>();
        var radius = rect.Height / 2;

        // Clockwise from the top-left, generate the vertex segments
        var lCenter = new Vector2(rect.Left + radius, rect.Top + radius);
        var rCenter = new Vector2(rect.Right - radius, rect.Top + radius);

        var lSegments = CreateCircleSegment(lCenter, radius, points, BotAngle, TopAngle);
        var rSegments = CreateCircleSegment(rCenter, radius, points, TopAngle, BotAngle);

        verts.AddRange(lSegments);
        verts.AddRange(rSegments);

        return verts.ToArray();
    }

    private static Vector2[] CreateCircleSegment(Vector2 center, float radius, int points, float start, float end)
    {
        List<Vector2> segments = new List<Vector2>();
        var step = (end - start) / points;
        var theta = start;

        for (var i = 0; i < points; i++)
        {
            segments.Add(center + new Vector2((float) (radius * MathF.Cos(theta)), (float) (radius * MathF.Sin(theta))));
            theta += step;
        }
        segments.Add(center + new Vector2((float) (radius * MathF.Cos(end)), (float) (radius * MathF.Sin(end))));

        return segments.ToArray();
    }

    public static Vector2 FindNearestPointInsideRect(RectangleF rect, Vector2 point)
    {
        var newPoint = point;
        if (rect.Contains(point))
            return newPoint;

        if (point.X < rect.Left)
            newPoint = newPoint.SetX(rect.Left);

        if (point.X > rect.Right)
            newPoint = newPoint.SetX(rect.Right);

        if (point.Y < rect.Top)
            newPoint = newPoint.SetY(rect.Top);

        if (point.Y > rect.Bottom)
            newPoint = newPoint.SetY(rect.Bottom);

        return newPoint;
    }
}