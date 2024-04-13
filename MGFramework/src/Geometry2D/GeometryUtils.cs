using Microsoft.Xna.Framework;
using Line = FavobeanGames.MGFramework.Geometry2D.Shapes.Line;

namespace FavobeanGames.MGFramework.Geometry2D;

public class GeometryUtils
{
    public static readonly float FLOAT_EQUALITY_TOLERANCE = 0.0005f;

    /// <summary>
    /// Calculates the distance between a point and a line segment defined by two endpoints.
    /// </summary>
    /// <param name="point">The point for which distance is to be calculated.</param>
    /// <param name="segmentStart">One endpoint of the line segment.</param>
    /// <param name="segmentEnd">The other endpoint of the line segment.</param>
    /// <param name="distanceSquared">The distance between the point and the line segment.</param>
    /// <param name="closestPoint">The point on the line segment closest to the given point.</param>
    public static void DistanceToSegment(Vector2 point, Vector2 segmentStart, Vector2 segmentEnd, out float distanceSquared, out Vector2 closestPoint)
    {
        distanceSquared = Vector2.DistanceSquared(segmentStart, segmentEnd); // Length of the segment squared

        // If the segment start and end points are the same, return distance from point to segment start
        if (distanceSquared == 0)
        {
            closestPoint = segmentStart;
            distanceSquared = Vector2.DistanceSquared(point, segmentStart);
            return;
        }

        // Calculate the parameter along the line where the projection of the point onto the line falls
        var t = System.Math.Max(0, System.Math.Min(1, ((point.X - segmentStart.X) * (segmentEnd.X - segmentStart.X) + (point.Y - segmentStart.Y) * (segmentEnd.Y - segmentStart.Y)) / distanceSquared));

        // Calculate the projection of the point onto the line
        closestPoint = new Vector2(segmentStart.X + t * (segmentEnd.X - segmentStart.X),
            segmentStart.Y + t * (segmentEnd.Y - segmentStart.Y));

        // Calculate the distance between the point and the projection
        distanceSquared = Vector2.DistanceSquared(point, closestPoint);
    }

    /// <summary>
    /// Calculates the distance between a point and a line segment defined by two endpoints.
    /// </summary>
    /// <param name="point">The point for which distance is to be calculated.</param>
    /// <param name="line">line segment.</param>
    /// <param name="distanceSquared">The distance between the point and the line segment.</param>
    /// <param name="closestPoint">The point on the line segment closest to the given point.</param>
    public static void DistanceToSegment(Vector2 point, Line line, out float distanceSquared, out Vector2 closestPoint)
    {
        DistanceToSegment(point, line.Start, line.End, out distanceSquared, out closestPoint);
    }


}