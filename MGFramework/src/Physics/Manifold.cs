using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace FavobeanGames.MGFramework.Physics;

public class Manifold
{
    public readonly Body BodyA;
    public readonly Body BodyB;
    public readonly Vector2 Normal;
    public readonly float Depth;
    public readonly Vector2 Contact1;
    public readonly Vector2 Contact2;
    public readonly int ContactCount;

    public Manifold(
        Body bodyA, Body bodyB,
        Vector2 normal, float depth,
        Vector2 contact1, Vector2 contact2, int contactCount)
    {
        BodyA = bodyA;
        BodyB = bodyB;
        Normal = normal;
        Depth = depth;
        Contact1 = contact1;
        Contact2 = contact2;
        ContactCount = contactCount;
    }
}