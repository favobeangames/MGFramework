using FavobeanGames.MGFramework.Geometry2D.Math;
using FavobeanGames.MGFramework.Geometry2D.Shapes;

namespace MGFrameworkTests.Geometry2D.Math;

public class VectorMathTests
{
    [Fact]
    public void TestDotProduct_Success()
    {
        Vector v1 = new Vector(1, 2);
        Vector v2 = new Vector(4, 5);
        float expected = 1 * 4 + 2 * 5;
        Assert.Equal(expected, VectorMath.Dot(v1, v2));
    }

    [Fact]
    public void TestCrossProduct()
    {
        Vector v1 = new Vector(1, 2);
        Vector v2 = new Vector(4, 5);
        float expected = 1 * 5 - 2 * 4;
        Assert.Equal(expected, VectorMath.Cross(v1, v2));
    }

    [Fact]
    public void TestMagnitude()
    {
        Vector v1 = new Vector(3, 4);
        float expected = (float)5; // By Pythagorean theorem
        Assert.Equal(expected, VectorMath.Magnitude(v1));
    }

    [Fact]
    public void TestNormalization()
    {
        Vector v1 = new Vector(3, 4);
        Vector expected = new Vector(3 / 5f, 4 / 5f); // By Pythagorean theorem
        Assert.Equal(expected, VectorMath.Normalize(v1));
    }

    [Fact]
    public void TestProjection()
    {
        Vector v1 = new Vector(1, 2);
        Vector v2 = new Vector(4, 5);
        // Projection of vector1 onto vector2: (1*4 + 2*5) / (4*4 + 5*5) * (4, 5) = (14/41)* (4, 5)
        Vector expected = new Vector((14f / 41f) * 4, (14f / 41f) * 5);
        Assert.Equal(expected, VectorMath.Project(v1, v2));
    }

    [Fact]
    public void TestReflection()
    {
        Vector v1 = new Vector(1, 1);
        Vector normal = new Vector(0, 1);
        // Reflection of vector off the normal {0, 1}: (1, 1) - 2 * (1 * 1) * (0, 1) = (1, 1) - 2 * (1) * (0, 1) = (1, 1) - (0, 2) = (1, -1)
        Vector expected = new Vector(1, -1);
        Assert.Equal(expected, VectorMath.Reflection(v1, normal));
    }

    [Fact]
    public void TestDistanceBetween()
    {
        Vector v1 = new Vector(1, 2);
        Vector v2 = new Vector(4, 6);
        // Distance between (1, 2) and (4, 6) by Pythagorean theorem
        float expected = MathF.Sqrt(3 * 3 + 4 * 4);
        Assert.Equal(expected, VectorMath.DistanceBetween(v1, v2));
    }
}