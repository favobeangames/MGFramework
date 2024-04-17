using System.Collections.Generic;
using FavobeanGames.MGFramework.Graphics.Primitives;

namespace DemoSandbox.Demos;

public class GeomCollision
{
    public int Geometry1Index;
    public int Geometry2Index;

    public List<Circle> CollisionPoints;

    public GeomCollision(int geometry1Index, int geometry2Index)
    {
        CollisionPoints = new List<Circle>();
        Geometry1Index = geometry1Index;
        Geometry2Index = geometry2Index;
    }
}