using FavobeanGames.MGFramework.Geometry2D.Math;
using FavobeanGames.MGFramework.Geometry2D.Shapes;

namespace MGFrameworkTests.Geometry2D.Math;

public class VectorMathTests
{
    [Fact]
    public void TestDotProduct()
    {
        Vector v1 = new Vector(1, 2);
        Vector v2 = new Vector(4, 5);
        float expected = 1 * 4 + 2 * 5;
        Assert.Equal(expected, VectorMath.Dot(v1, v2));
    }
}