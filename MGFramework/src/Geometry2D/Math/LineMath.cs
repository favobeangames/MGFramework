using System;
using FavobeanGames.MGFramework.Geometry2D.Shapes;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Geometry2D.Math;

public static class LineMath
{
    /// <summary>
    /// Calculates the slope of the line.
    /// Helpful to represent object trajectories and defining boundaries
    /// </summary>
    /// <param name="line">Line to find the slope of</param>
    /// <returns>Slope of the line</returns>
    public static float Slope(Line line)
    {
        return (line.End.Y - line.Start.Y) / (line.End.X - line.Start.X);
    }

    /// <summary>
    /// Calculates the y-coordinate of a point on a line using the point-slope formula.
    /// </summary>
    /// <param name="x">The x-coordinate where the y-coordinate is to be calculated.</param>
    /// <param name="slope">The slope of the line.</param>
    /// <param name="point">A point that exists on the line.</param>
    /// <returns>The y-coordinate of the point on the line.</returns>
    public static float PointSlope(float x, float slope, Vector2 point)
    {
        return slope * (x - point.X) + point.Y;
    }

    /// <summary>
    /// Calculates the equation of a line passing through two given points using the two-point formula.
    /// </summary>
    /// <param name="line">Line .</param>
    /// <param name="slope">The slope of the line.</param>
    /// <param name="yIntercept">The y-intercept of the line.</param>
    public static void TwoPoint(Line line, out float slope, out float yIntercept)
    {
        slope = Slope(line);

        yIntercept = line.Start.Y - slope * line.Start.X;
    }

    /// <summary>
    /// Determines if two lines intersect given their slope-intercept
    /// </summary>
    /// <param name="m1">The slope of the first line.</param>
    /// <param name="b1">The y-intercept of the first line.</param>
    /// <param name="m2">The slope of the second line.</param>
    /// <param name="b2">The y-intercept of the second line.</param>
    /// <param name="intersectionPoint">The intersection point</param>
    /// <returns>True if the lines intersect, false otherwise</returns>
    public static bool LinesIntersect(float m1, float b1, float m2, float b2, out Vector2 intersectionPoint)
    {
        intersectionPoint = new Vector2(float.NaN, float.NaN);

        // Check if the lines are parallel
        if (MathF.Abs(m1 - m2) < float.Epsilon)
        {
            // Lines are parallel, check if they are overlapping
            // If true, lines are overlapping so there is no true intersection point
            // If false, lines are parallel and not overlapping so there is not intersection
            return MathF.Abs(b1 - b2) < float.Epsilon;
        }

        // Lines are not parallel, find intersection point
        intersectionPoint.X = (b2 - b1) / (m1 - m2);
        intersectionPoint.Y = m1 * intersectionPoint.X + b1;

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="line1"></param>
    /// <param name="line2"></param>
    /// <param name="intersectionPoint"></param>
    /// <returns></returns>
    public static bool LineSegmentIntersection(Line line1, Line line2, out Vector2 intersectionPoint)
    {
        intersectionPoint = new Vector2(float.NaN, float.NaN);

        var tolerance = 0.0005f;

        double x1 = line1.Start.X, y1 = line1.Start.Y;
        double x2 = line1.End.X, y2 = line1.End.Y;
        double x3 = line2.Start.X, y3 = line2.Start.Y;
        double x4 = line2.End.X, y4 = line2.End.Y;

        // equations of the form x=c (two vertical lines) with overlapping
        if (System.Math.Abs(x1 - x2) < tolerance && System.Math.Abs(x3 - x4) < tolerance && System.Math.Abs(x1 - x3) < tolerance)
        {
            return false;
        }

        //equations of the form y=c (two horizontal lines) with overlapping
        if (System.Math.Abs(y1 - y2) < tolerance && System.Math.Abs(y3 - y4) < tolerance && System.Math.Abs(y1 - y3) < tolerance)
        {
            return false;
        }

        //equations of the form x=c (two vertical parallel lines)
        if (System.Math.Abs(x1 - x2) < tolerance && System.Math.Abs(x3 - x4) < tolerance)
        {
            //return default (no intersection)
            return false;
        }

        //equations of the form y=c (two horizontal parallel lines)
        if (System.Math.Abs(y1 - y2) < tolerance && System.Math.Abs(y3 - y4) < tolerance)
        {
            //return default (no intersection)
            return false;
        }

        //general equation of line is y = mx + c where m is the slope
        //assume equation of line 1 as y1 = m1x1 + c1
        //=> -m1x1 + y1 = c1 ----(1)
        //assume equation of line 2 as y2 = m2x2 + c2
        //=> -m2x2 + y2 = c2 -----(2)
        //if line 1 and 2 intersect then x1=x2=x & y1=y2=y where (x,y) is the intersection point
        //so we will get below two equations
        //-m1x + y = c1 --------(3)
        //-m2x + y = c2 --------(4)

        double x, y;

        //lineA is vertical x1 = x2
        //slope will be infinity
        //so lets derive another solution
        if (System.Math.Abs(x1 - x2) < tolerance)
        {
            //compute slope of line 2 (m2) and c2
            double m2 = (y4 - y3) / (x4 - x3);
            double c2 = -m2 * x3 + y3;

            //equation of vertical line is x = c
            //if line 1 and 2 intersect then x1=c1=x
            //subsitute x=x1 in (4) => -m2x1 + y = c2
            // => y = c2 + m2x1
            x = x1;
            y = c2 + m2 * x1;
        }
        //lineB is vertical x3 = x4
        //slope will be infinity
        //so lets derive another solution
        else if (System.Math.Abs(x3 - x4) < tolerance)
        {
            //compute slope of line 1 (m1) and c2
            double m1 = (y2 - y1) / (x2 - x1);
            double c1 = -m1 * x1 + y1;

            //equation of vertical line is x = c
            //if line 1 and 2 intersect then x3=c3=x
            //subsitute x=x3 in (3) => -m1x3 + y = c1
            // => y = c1 + m1x3
            x = x3;
            y = c1 + m1 * x3;
        }
        //lineA & lineB are not vertical
        //(could be horizontal we can handle it with slope = 0)
        else
        {
            //compute slope of line 1 (m1) and c2
            double m1 = (y2 - y1) / (x2 - x1);
            double c1 = -m1 * x1 + y1;

            //compute slope of line 2 (m2) and c2
            double m2 = (y4 - y3) / (x4 - x3);
            double c2 = -m2 * x3 + y3;

            //solving equations (3) & (4) => x = (c1-c2)/(m2-m1)
            //plugging x value in equation (4) => y = c2 + m2 * x
            x = (c1 - c2) / (m2 - m1);
            y = c2 + m2 * x;

            //verify by plugging intersection point (x, y)
            //in orginal equations (1) & (2) to see if they intersect
            //otherwise x,y values will not be finite and will fail this check
            if (!(System.Math.Abs(-m1 * x + y - c1) < tolerance
                  && System.Math.Abs(-m2 * x + y - c2) < tolerance))
            {
                //return default (no intersection)
                return false;
            }
        }

        //x,y can intersect outside the line segment since line is infinitely long
        //so finally check if x, y is within both the line segments
        if (IsInsideLine(line1, x, y) &&
            IsInsideLine(line2, x, y))
        {
            intersectionPoint = new Vector2((float)x, (float)y);
            return true;
        }

        //return default (no intersection)
        return false;
    }

    // Returns true if given point(x,y) is inside the given line segment
    private static bool IsInsideLine(Line line, double x, double y)
    {
        return (x >= line.Start.X && x <= line.End.X
                || x >= line.End.X && x <= line.Start.X)
               && (y >= line.Start.Y && y <= line.End.Y
                   || y >= line.End.Y && y <= line.Start.Y);
    }
}