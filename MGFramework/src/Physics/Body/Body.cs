using System;
using FavobeanGames.MGFramework.Geometry2D.Shapes;
using Microsoft.Xna.Framework;
using FavobeanGames.MGFramework.Graphics.Primitives;
using FavobeanGames.MGFramework.Util;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Physics;

/// <summary>
/// Delegate tp notify consumers that a collision has taken place. Returns the Manifold of
/// the two bodies that collided
/// </summary>
public delegate void BodyCollidedDelegate(Manifold collision);

/// <summary>
/// Enum storing the type of physics body in the world
/// </summary>
public enum BodyType
{
    Rigid,
    Soft,
}
public class Body
{
    /// <summary>
    /// Id of the body
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Type of shape of the body (Polygon, circle, etc)
    /// </summary>
    public ShapeType ShapeType;

    /// <summary>
    /// Contains all positional and vertex information
    /// </summary>
    public Transform2 Transform2 { get; protected set; }

    /// <summary>
    /// Geometry object of the body
    /// </summary>
    public Geometry Geometry { get; protected set; }

    /// <summary>
    /// Velocity of the body in the world
    /// </summary>
    public Vector2 LinearVelocity { get; set; }

    /// <summary>
    /// Speed of rotation of the body
    /// </summary>
    public float RotationalVelocity { get; set; }

    /// <summary>
    /// Force applied to the body in the world
    /// </summary>
    public Vector2 Force { get; protected set; }

    /// <summary>
    /// Flag to determine if the primitive does not move
    /// </summary>
    public bool IsStatic { get; }

    /// <summary>
    /// Density of the primitive
    /// </summary>
    public float Density { get; }

    /// <summary>
    /// Mass of the primitive
    /// </summary>
    public float Mass { get; }

    /// <summary>
    /// Inverse mass value
    /// </summary>
    public float InvMass
    {
        get
        {
            if (!IsStatic) return 1f / Mass;

            return 0f;
        }
    }

    /// <summary>
    /// Moment of Inertia for the body
    /// </summary>
    public float Inertia;

    /// <summary>
    /// Inverse of the Intertia of the body
    /// </summary>
    public float InvInertia
    {
        get
        {
            if (!IsStatic) return 1f / Inertia;

            return 0f;
        }
    }

    /// <summary>
    /// Ratio of the velocity components along the normal plane of
    /// contact after and before the collision.
    /// Simply, how bouncy an object is when it comes into contact
    /// with another object.
    /// </summary>
    public float Restitution { get; }

    /// <summary>
    /// Area of the primitive shape
    /// </summary>
    public float Area { get; }

    /// <summary>
    /// Body type (Rigid, Soft)
    /// </summary>
    public BodyType BodyType { get; protected set; }

    /// <summary>
    /// Friction value that represents the friction that is keeping the bodies in place
    /// </summary>
    public readonly float StaticFriction;

    /// <summary>
    /// Friction value while body is moving along another body
    /// </summary>
    public readonly float DynamicFriction;

    /// <summary>
    /// Delegate tp notify consumers that a collision has taken place. Returns the Manifold of
    /// the two bodies that collided
    /// </summary>
    public event Action<Manifold> BodyCollided;

    /// <summary>
    /// Event that can be subscribed to, to give an extra check for game systems
    /// to allow for separate collision checks
    /// </summary>
    public event Func<Manifold, bool> BodyResolveCheck;

    /// <summary>
    /// Event that can be subscribed to, to give the new position that the body moved to
    /// in the physics world
    /// </summary>
    public event Action<Vector2> BodyMovedTo;

    public Body()
    {
    }
    public Body(BodyType bodyType, ShapeType shapeType)
    {
        BodyType = bodyType;
    }

    public Body(BodyType bodyType, bool isStatic, float density, float mass, float restitution, float area)
    {
        BodyType = bodyType;
        IsStatic = isStatic;
        Density = density;
        Mass = mass;
        Restitution = restitution;
        Area = area;
        Force = Vector2.Zero;

        StaticFriction = 0.6f;
        DynamicFriction = 0.4f;
    }

    public Body(int id, BodyType bodyType, bool isStatic, float density, float mass, float restitution, float area)
    {
        Id = id;
        BodyType = bodyType;
        IsStatic = isStatic;
        Density = density;
        Mass = mass;
        Restitution = restitution;
        Area = area;
        Force = Vector2.Zero;

        StaticFriction = 0.6f;
        DynamicFriction = 0.4f;
    }

    internal virtual void Step(float time, Vector2 gravity, int iterations)
    {
    }

    public virtual void Move(Vector2 amount)
    {
        Transform2.Position += amount;
        BodyMovedTo?.Invoke(Transform2.Position);
    }

    public virtual void AddForce(Vector2 force)
    {
        Force = force;
    }

    public virtual void OnCollidedWith(Manifold collision)
    {
        BodyCollided?.Invoke(collision);
    }

    public virtual bool OnBodyResolveCheck(Manifold collision)
    {
        return BodyResolveCheck?.Invoke(collision) ?? false;
    }

    protected virtual float CalculateRotationalInertia()
    {
        switch (ShapeType)
        {
            case ShapeType.Circle:
                return 1f / 2f * Mass * Geometry.Radius * Geometry.Radius;
            case ShapeType.Polygon:
                var aabb = Geometry.Aabb;
                return 1f / 12f * Mass * (aabb.Width * aabb.Width + aabb.Height * aabb.Height);
        }

        return 0;
    }
}