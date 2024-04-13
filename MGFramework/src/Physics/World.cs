using System;
using System.Collections.Generic;
using FavobeanGames.MGFramework.Physics.Math;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Physics;

/// <summary>
/// Determines what type of resolution the world should apply to bodies when they collide
/// </summary>
public enum CollisionResolutionType
{
    // Does not resolve collision, still fires event to be subscribed to
    None,
    // Resolves collision between bodies, does not apply rotation or friction
    Basic,
    // Resolves collision between bodies with rotation, but no friction
    BaseWithRotational,
    // Resolves collision between bodies with rotation and friction
    BaseWithRotationalAndFriction,
}

public class WorldOptions
{
    /// <summary>
    /// Collision system determines how the world should resolve collision between bodies
    /// </summary>
    public readonly CollisionResolutionType CollisionResolutionType;

    public WorldOptions(CollisionResolutionType collisionResolutionType)
    {
        CollisionResolutionType = collisionResolutionType;
    }
}
public class World
{
    /// <summary>
    /// Min and Max area of an object in the world
    /// </summary>
    public static readonly float MinBodySize = 0.01f * 0.01f;
    public static readonly float MaxBodySize = 64f * 64f;

    /// <summary>
    /// Min and Max density of an object in the world
    /// g/cm^3
    /// </summary>
    public static readonly float MinDensity = 0.5f;
    public static readonly float MaxDensity = 21.4f;

    public static readonly int MinIterations = 1;
    public static readonly int MaxIterations = 128;

    public Vector2 Gravity { get; }

    /// <summary>
    /// Collection of bodies in the world
    /// </summary>
    private readonly List<Body> bodies;

    /// <summary>
    /// Count of bodies in the world
    /// </summary>
    public int BodyCount => bodies.Count;

    private Vector2[] contactList;
    private Vector2[] impulseList;
    private Vector2[] frictionImpulseList;
    private Vector2[] raList;
    private Vector2[] rbList;
    private float[] jList;

    /// <summary>
    /// List of bodies that have collided with one another
    /// </summary>
    private readonly List<(int,int)> contactPairs;

    /// <summary>
    /// Optional parameters to configure how the world behaves
    /// </summary>
    private WorldOptions worldOptions;
    public World()
    {
        Gravity = Vector2.Zero;
        bodies = new List<Body>();
        contactPairs = new List<(int,int)>();

        contactList = new Vector2[2];
        impulseList = new Vector2[2];
        frictionImpulseList = new Vector2[2];
        raList = new Vector2[2];
        rbList = new Vector2[2];
        jList = new float[2];

        worldOptions = new WorldOptions(CollisionResolutionType.None);
    }

    public World(Vector2 gravity, WorldOptions worldOptions)
    {
        Gravity = gravity;
        bodies = new List<Body>();
        contactPairs = new List<(int,int)>();

        contactList = new Vector2[2];
        impulseList = new Vector2[2];
        frictionImpulseList = new Vector2[2];
        raList = new Vector2[2];
        rbList = new Vector2[2];
        jList = new float[2];

        this.worldOptions = worldOptions;
    }

    /// <summary>
    /// Adds the body to the world
    /// </summary>
    /// <param name="body">Body object to add to the world</param>
    public void AddBody(Body body)
    {
        bodies.Add(body);
    }

    /// <summary>
    /// Removes the body from the world
    /// </summary>
    /// <param name="body">Body to remove</param>
    public void RemoveBody(Body body)
    {
        bodies.Remove(body);
    }

    /// <summary>
    /// Returns the body from its index
    /// </summary>
    /// <param name="index">Index of the body from the collection</param>
    /// <param name="body">(Out param) Body returned from collection if exists</param>
    /// <returns>Bool flag if a body exists at the index passed</returns>
    public bool GetBody(int index, out Body body)
    {
        body = null;

        if (index < 0 || index > BodyCount)
        {
            return false;
        }

        body = bodies[index];
        return true;
    }

    public void Step(float time, int totalIterations)
    {
        totalIterations = System.Math.Clamp(totalIterations, MinIterations, MaxIterations);

        for (int currentIteration = 0; currentIteration < totalIterations; currentIteration++)
        {
            contactPairs.Clear();
            StepBodies(time, Gravity, totalIterations);
            BroadPhase();
            NarrowPhase();
        }
    }

    /// <summary>
    /// Broad phase collision detection loops through bodies and determines
    /// and records which ones collided with each other.
    /// </summary>
    private void BroadPhase()
    {
        // Check for body collisions
        for (int i = 0; i < BodyCount; i++)
        {
            Body bodyA = bodies[i];

            for (int j = i + 1; j < BodyCount; j++)
            {
                Body bodyB = bodies[j];

                if (bodyA.IsStatic && bodyB.IsStatic)
                {
                    continue;
                }

                // Check Axis Aligned Bounding Box collisions of bodies before
                // running the more complex collision algorithms
                var bodyAbb = bodyA.Geometry.Aabb;
                var bodyBbb = bodyB.Geometry.Aabb;
                if (!bodyAbb.Intersects(bodyBbb)) continue;

                contactPairs.Add((i, j));
            }
        }
    }

    /// <summary>
    /// Narrow Phase resolves the collision between bodies
    /// </summary>
    private void NarrowPhase()
    {
        for (int i = 0; i < contactPairs.Count; i++)
        {
            (int, int) pair = contactPairs[i];
            Body bodyA = bodies[pair.Item1];
            Body bodyB = bodies[pair.Item2];

            if (Collisions.Collide(bodyA, bodyB, out Vector2 normal, out float depth))
            {
                Collisions.FindContactPoints(bodyA, bodyB, out Vector2 contact1, out Vector2 contact2, out int contactCount);
                Manifold contact = new Manifold(bodyA, bodyB, normal, depth, contact1, contact2, contactCount);

                if (worldOptions.CollisionResolutionType != CollisionResolutionType.None)
                    SeparateBodies(bodyA, bodyB, normal * depth);

                // TODO: Make this more performant somehow...
                bodyA.OnCollidedWith(contact);
                bodyB.OnCollidedWith(contact);

                switch (worldOptions.CollisionResolutionType)
                {
                    case CollisionResolutionType.Basic:
                        ResolveCollisionBasic(in contact);
                        break;
                    case CollisionResolutionType.BaseWithRotational:
                        ResolveCollisionWithRotation(in contact);
                        break;
                    case CollisionResolutionType.BaseWithRotationalAndFriction:
                        ResolveCollisionWithRotationAndFriction(in contact);
                        break;
                }
            }
        }
    }
    public void StepBodies(float time, Vector2 gravity, int totalIterations)
    {
        // Move the bodies through the world
        for (int i = 0; i < BodyCount; i++)
        {
            bodies[i].Step(time, gravity, totalIterations);
        }
    }
    private void SeparateBodies(Body bodyA, Body bodyB, Vector2 mtv)
    {
        if (bodyA.IsStatic)
        {
            bodyB.Move(mtv);
        }
        else if (bodyB.IsStatic)
        {
            bodyA.Move(-mtv);
        }
        else
        {
            bodyA.Move(-mtv / 2f);
            bodyB.Move(mtv / 2f);
        }
    }

    /// <summary>
    /// Resolves collision between bodies
    /// </summary>
    /// <param name="contact">Contact manifold containing collision details between two bodies</param>
    public void ResolveCollisionBasic(in Manifold contact)
    {
        Vector2 relativeVelocity = contact.BodyB.LinearVelocity - contact.BodyA.LinearVelocity;

        if (Vector2.Dot(relativeVelocity, contact.Normal) > 0f)
        {
            // Objects are already moving apart
            return;
        }

        float e = MathF.Min(contact.BodyA.Restitution, contact.BodyB.Restitution);
        float j = -(1f + e) * Vector2.Dot(relativeVelocity, contact.Normal);
        j /= contact.BodyA.InvMass + contact.BodyB.InvMass;

        Vector2 impulse = j * contact.Normal;

        contact.BodyA.LinearVelocity -= impulse * contact.BodyA.InvMass;
        contact.BodyB.LinearVelocity += impulse * contact.BodyB.InvMass;
    }

    /// <summary>
    /// Resolves collision between bodies with rotation based on bodies inertia
    /// </summary>
    /// <param name="contact">Contact manifold containing collision details between two bodies</param>
    public void ResolveCollisionWithRotation(in Manifold contact)
    {
        var bodyA = contact.BodyA;
        var bodyB = contact.BodyB;
        var normal = contact.Normal;
        var contact1 = contact.Contact1;
        var contact2 = contact.Contact2;
        int contactCount = contact.ContactCount;

        float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

        contactList[0] = contact1;
        contactList[1] = contact2;

        for (int i = 0; i < contactCount; i++)
        {
            impulseList[i] = Vector2.Zero;
            raList[i] = Vector2.Zero;
            rbList[i] = Vector2.Zero;
        }

        for (int i = 0; i < contactCount; i++)
        {
            // Vector pointing from center of object to point of collision
            Vector2 ra = contactList[i] - bodyA.Transform2.Position;
            Vector2 rb = contactList[i] - bodyB.Transform2.Position;

            raList[i] = ra;
            rbList[i] = rb;

            Vector2 raPerp = new Vector2(-ra.Y, ra.X);
            Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

            Vector2 angularLinearVelocityA = raPerp * bodyA.RotationalVelocity;
            Vector2 angularLinearVeolocityB = rbPerp * bodyB.RotationalVelocity;

            Vector2 relativeVelocity =
                (bodyB.LinearVelocity + angularLinearVeolocityB) -
                (bodyA.LinearVelocity + angularLinearVelocityA);

            float contactVelocityMagnitude = Vector2.Dot(relativeVelocity, normal);
            if (contactVelocityMagnitude > 0f)
            {
                // Objects are already moving apart
                return;
            }

            float raPerpDotN = Vector2.Dot(raPerp, normal);
            float rbPerpDotN = Vector2.Dot(rbPerp, normal);

            float denom = bodyA.InvMass + bodyB.InvMass +
                          (raPerpDotN * raPerpDotN) * bodyA.InvInertia +
                          (rbPerpDotN * rbPerpDotN) * bodyB.InvInertia;

            float j = -(1f + e) * contactVelocityMagnitude;
            j /= denom;
            j /= contactCount;

            Vector2 impulse = j * normal;
            impulseList[i] = impulse;
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 impulse = impulseList[i];
            Vector2 ra = raList[i];
            Vector2 rb = rbList[i];

            bodyA.LinearVelocity += -impulse * bodyA.InvMass;
            bodyA.RotationalVelocity += -VectorHelper.CrossProduct2D(ra, impulse) * bodyA.InvInertia;
            bodyB.LinearVelocity += impulse * bodyB.InvMass;
            bodyB.RotationalVelocity += VectorHelper.CrossProduct2D(rb, impulse) * bodyB.InvInertia;
        }
    }

    /// <summary>
    /// Resolves collision between bodies with rotation based on bodies inertia and friction
    /// </summary>
    /// <param name="contact">Contact manifold containing collision details between two bodies</param>
    public void ResolveCollisionWithRotationAndFriction(in Manifold contact)
    {
        var bodyA = contact.BodyA;
        var bodyB = contact.BodyB;
        var normal = contact.Normal;
        var contact1 = contact.Contact1;
        var contact2 = contact.Contact2;
        int contactCount = contact.ContactCount;

        float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

        float sf = bodyA.StaticFriction + bodyB.StaticFriction * 0.5f;
        float df = bodyA.DynamicFriction + bodyB.DynamicFriction * 0.5f;

        contactList[0] = contact1;
        contactList[1] = contact2;

        for (int i = 0; i < contactCount; i++)
        {
            impulseList[i] = Vector2.Zero;
            frictionImpulseList[i] = Vector2.Zero;
            raList[i] = Vector2.Zero;
            rbList[i] = Vector2.Zero;
            jList[i] = 0f;
        }

        for (int i = 0; i < contactCount; i++)
        {
            // Vector pointing from center of object to point of collision
            Vector2 ra = contactList[i] - bodyA.Transform2.Position;
            Vector2 rb = contactList[i] - bodyB.Transform2.Position;

            raList[i] = ra;
            rbList[i] = rb;

            Vector2 raPerp = new Vector2(-ra.Y, ra.X);
            Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

            Vector2 angularLinearVelocityA = raPerp * bodyA.RotationalVelocity;
            Vector2 angularLinearVeolocityB = rbPerp * bodyB.RotationalVelocity;

            Vector2 relativeVelocity =
                (bodyB.LinearVelocity + angularLinearVeolocityB) -
                (bodyA.LinearVelocity + angularLinearVelocityA);

            float contactVelocityMagnitude = Vector2.Dot(relativeVelocity, normal);
            if (contactVelocityMagnitude > 0f)
            {
                // Objects are already moving apart
                return;
            }

            float raPerpDotN = Vector2.Dot(raPerp, normal);
            float rbPerpDotN = Vector2.Dot(rbPerp, normal);

            float denom = bodyA.InvMass + bodyB.InvMass +
                          (raPerpDotN * raPerpDotN) * bodyA.InvInertia +
                          (rbPerpDotN * rbPerpDotN) * bodyB.InvInertia;

            float j = -(1f + e) * contactVelocityMagnitude;
            j /= denom;
            j /= contactCount;

            jList[i] = j;

            Vector2 impulse = j * normal;
            impulseList[i] = impulse;
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 impulse = impulseList[i];
            Vector2 ra = raList[i];
            Vector2 rb = rbList[i];

            bodyA.LinearVelocity += -impulse * bodyA.InvMass;
            bodyA.RotationalVelocity += -VectorHelper.CrossProduct2D(ra, impulse) * bodyA.InvInertia;
            bodyB.LinearVelocity += impulse * bodyB.InvMass;
            bodyB.RotationalVelocity += VectorHelper.CrossProduct2D(rb, impulse) * bodyB.InvInertia;
        }

        for (int i = 0; i < contactCount; i++)
        {
            // Vector pointing from center of object to point of collision
            Vector2 ra = contactList[i] - bodyA.Transform2.Position;
            Vector2 rb = contactList[i] - bodyB.Transform2.Position;

            raList[i] = ra;
            rbList[i] = rb;

            Vector2 raPerp = new Vector2(-ra.Y, ra.X);
            Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

            Vector2 angularLinearVelocityA = raPerp * bodyA.RotationalVelocity;
            Vector2 angularLinearVeolocityB = rbPerp * bodyB.RotationalVelocity;

            Vector2 relativeVelocity =
                (bodyB.LinearVelocity + angularLinearVeolocityB) -
                (bodyA.LinearVelocity + angularLinearVelocityA);

            Vector2 tangent = relativeVelocity - Vector2.Dot(relativeVelocity, normal) * normal;

            if (tangent.EqualsWithTolerence(Vector2.Zero, Collisions.FLOAT_EQUALITY_TOLERANCE))
            {
                continue;
            }

            tangent = Vector2.Normalize(tangent);

            float raPerpDotT = Vector2.Dot(raPerp, tangent);
            float rbPerpDotT = Vector2.Dot(rbPerp, tangent);

            float denom = bodyA.InvMass + bodyB.InvMass +
                          (raPerpDotT * raPerpDotT) * bodyA.InvInertia +
                          (rbPerpDotT * rbPerpDotT) * bodyB.InvInertia;

            float jt = -Vector2.Dot(relativeVelocity, tangent);
            jt /= denom;
            jt /= contactCount;

            Vector2 frictionImpluse;

            if (MathF.Abs(jt) <= jList[i] * sf)
            {
                frictionImpluse = jt * tangent;
            }
            else
            {
                frictionImpluse = -jList[i] * tangent * df;
            }

            frictionImpulseList[i] = frictionImpluse;
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 frictionImpulse = frictionImpulseList[i];
            Vector2 ra = raList[i];
            Vector2 rb = rbList[i];

            bodyA.LinearVelocity += -frictionImpulse * bodyA.InvMass;
            bodyA.RotationalVelocity += -VectorHelper.CrossProduct2D(ra, frictionImpulse) * bodyA.InvInertia;
            bodyB.LinearVelocity += frictionImpulse * bodyB.InvMass;
            bodyB.RotationalVelocity += VectorHelper.CrossProduct2D(rb, frictionImpulse) * bodyB.InvInertia;
        }
    }
}