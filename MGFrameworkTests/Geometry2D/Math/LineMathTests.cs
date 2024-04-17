using FavobeanGames.MGFramework.Geometry2D.Math;
using FavobeanGames.MGFramework.Geometry2D.Shapes;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace MGFrameworkTests.Geometry2D.Math;

public class LineMathTests
{
    [Fact]
    public void TwoPointTest_CorrectCalculation()
    {
        var line = new Line(new Vector2(1, 2), new Vector2(3, 4));

        const float expectedSlope = 1;
        const float expectedYIntercept = 1;

        // Act
        LineMath.TwoPoint(line, out var slope, out var yIntercept);

        // Assert
        Assert.Equal(expectedSlope, slope);
        Assert.Equal(expectedYIntercept, yIntercept);
    }

    [Fact]
    public void PointSlope_CorrectCalculation()
    {
        float x = 5;
        float slope = 2;
        Vector2 point = new Vector2(1, 3);

        float expectedY = 11; // Expected y-coordinate at x = 5;

        float actual = LineMath.PointSlope(x, slope, point);

        Assert.Equal(expectedY, actual);
    }

    [Fact]
    public void LinesIntersect_ParallelLines_ShouldReturnFalse()
    {
        float m1 = 2;
        float b1 = 1;
        float m2 = 2;
        float b2 = 5;

        bool result = LineMath.LinesIntersect(m1, b1, m2, b2, out var intersectionPoint);

        Assert.False(result);
    }

    [Fact]
    public void LinesIntersect_OverlappingParallelLines_ShouldReturnTrue()
    {
        float m1 = 2;
        float b1 = 1;
        float m2 = 2;
        float b2 = 1;

        // Overlapping parallel lines intersect at all points so it will just return a zeroed vector object
        Vector2 expected = Vector2.Zero;
        bool result = LineMath.LinesIntersect(m1, b1, m2, b2, out var intersectionPoint);

        Assert.True(result);
        Assert.Equal(expected, intersectionPoint);
    }

    [Fact]
    public void LinesIntersect_IntersectingLines_ShouldReturnTrue()
    {
        float m1 = 2;
        float b1 = 1;
        float m2 = -0.5f;
        float b2 = 3;

        // Should intersect at a specific point
        Vector2 expected = new Vector2(0.8f, 2.6f);
        bool result = LineMath.LinesIntersect(m1, b1, m2, b2, out var intersectionPoint);

        Assert.True(result);
        Assert.Equal(expected, intersectionPoint);
    }

    [Fact]
    public void LineSegmentIntersection_ShouldReturnTrue()
    {
        Line line1 = new Line(new Vector2(2, 5), new Vector2(7, 8));
        Line line2 = new Line(new Vector2(3, 3), new Vector2(5, 12));

        Vector2 expected = new Vector2(3.6666667f, 6);

        bool result = LineMath.LineSegmentIntersection(line1, line2, out var intersectionPoint);

        Assert.True(result);
        Assert.Equal(expected, intersectionPoint);
    }

    [Fact]
    public void LineSegmentIntersection_ShouldReturnFalse()
    {
        Line line1 = new Line(new Vector2(2, 5), new Vector2(7, 8));
        Line line2 = new Line(new Vector2(2, 2), new Vector2(4, 5));

        bool result = LineMath.LineSegmentIntersection(line1, line2, out var intersectionPoint);

        Assert.False(result);
        Assert.True(intersectionPoint.IsNaN());
    }
}