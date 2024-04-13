using System.Collections.Generic;
using System.Linq;
using FavobeanGames.MGFramework.Geometry2D.Shapes;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Circle = FavobeanGames.MGFramework.Geometry2D.Shapes.Circle;
using Polygon = FavobeanGames.MGFramework.Geometry2D.Shapes.Polygon;

namespace FavobeanGames.MGFramework.Physics;

public class RigidBody : Body
{
    public RigidBody(bool isStatic, float density, float mass, float restitution, float area,
        ShapeType shapeType, Transform2 transform2)
    : base(BodyType.Rigid, isStatic, density, mass, restitution, area)
    {
        ShapeType = shapeType;
        Transform2 = transform2;
        Inertia = CalculateRotationalInertia();
    }

    public RigidBody(int id, bool isStatic, float density, float mass, float restitution, float area,
        ShapeType shapeType, Transform2 transform2)
        : base(id, BodyType.Rigid, isStatic, density, mass, restitution, area)
    {
        ShapeType = shapeType;
        Transform2 = transform2;
        Inertia = CalculateRotationalInertia();
    }

    public RigidBody(int id, bool isStatic, float density, float mass, float restitution, float area,
        ShapeType shapeType, Transform2 transform2, Geometry geometry)
        : base(id, BodyType.Rigid, isStatic, density, mass, restitution, area)
    {
        ShapeType = shapeType;
        Transform2 = transform2;
        Geometry = geometry;
        Inertia = CalculateRotationalInertia();
    }

    public RigidBody(int id, bool isStatic, float density, float mass, float restitution, float area,
        Transform2 transform2, IEnumerable<Vector2> vertices)
        : base(id, BodyType.Rigid, isStatic, density, mass, restitution, area)
    {
        Transform2 = transform2;
        Geometry = new Polygon(transform2, vertices.ToArray());
        Inertia = CalculateRotationalInertia();
    }

    public RigidBody(int id, bool isStatic, float density, float mass, float restitution, float area,
        Vector2 position, IEnumerable<Vector2> vertices)
        : base(id, BodyType.Rigid, isStatic, density, mass, restitution, area)
    {
        Transform2 = new Transform2(position, Vector2.One, 0f);
        Geometry = new Polygon(Transform2, vertices.ToArray());
        Inertia = CalculateRotationalInertia();
    }

    public RigidBody(int id, bool isStatic, float density, float mass, float restitution, float area,
        Vector2 center, float radius)
        : base(id, BodyType.Rigid, isStatic, density, mass, restitution, area)
    {
        Transform2 = new Transform2(center, Vector2.One, 0f);
        Geometry = new Circle(Transform2, radius);
        Inertia = CalculateRotationalInertia();
    }

    internal override void Step(float time, Vector2 gravity, int iterations)
    {
        if (IsStatic)
        {
            return;
        }

        time /= iterations;

        LinearVelocity += gravity * time;
        LinearVelocity += (Force/Mass) * time;
        Move(LinearVelocity * time);
        Transform2.Rotation -= RotationalVelocity * time;

        Force = Vector2.Zero;
    }
}