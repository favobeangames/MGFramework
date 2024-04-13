using System;
using FavobeanGames.MGFramework.Geometry2D.Shapes;
using FavobeanGames.MGFramework.Graphics.Primitives;
using FavobeanGames.MGFramework.Physics.Math;
using MonoGame.Extended;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace FavobeanGames.MGFramework.Physics;

public static class Collisions
{
    public static readonly float FLOAT_EQUALITY_TOLERANCE = 0.0005f;

    /// <summary>
    ///
    /// </summary>
    /// <param name="p">The point for which distance is to be calculated</param>
    /// <param name="a">One endpoint of the line segment</param>
    /// <param name="b">The other endpoint of the line segment</param>
    /// <param name="distanceSquared">The distance between the point and the line segment</param>
    /// <param name="contact">The point on the line segment closest to the given point</param>
    private static void PointSegmentDistance(Vector2 p, Vector2 a, Vector2 b, out float distanceSquared, out Vector2 contact)
    {
        Vector2 ab = b - a;
        Vector2 ap = p - a;

        float proj = Vector2.Dot(ap, ab);
        float abLenSq = VectorHelper.DistanceSquared(ab);
        float d = proj / abLenSq;

        if (d <= 0f)
        {
            contact = a;
        }
        else if (d >= 1f)
        {
            contact = b;
        }
        else
        {
            contact = a + ab * d;
        }

        distanceSquared = VectorHelper.DistanceSquared(p, contact);
    }
    public static void FindContactPoints(
        Body bodyA, Body bodyB,
        out Vector2 contact1, out Vector2 contact2,
        out int contactCount)
    {
        contact1 = Vector2.Zero;
        contact2 = Vector2.Zero;
        contactCount = 0;

        ShapeType bodyAShapeType = bodyA.ShapeType;
        ShapeType bodyBShapeType = bodyB.ShapeType;

        switch (bodyAShapeType)
        {
            case ShapeType.Polygon:
                if (bodyBShapeType is ShapeType.Polygon)
                {
                    FindPolygonContactPoints(
                        bodyA.Geometry.TransformedVertices,
                        bodyB.Geometry.TransformedVertices,
                        out contact1, out contact2, out contactCount);
                    break;
                }

                if (bodyBShapeType is ShapeType.Circle)
                {
                    FindCirclePolygonContactPoint(
                        bodyB.Transform2.Position, bodyA.Geometry.TransformedVertices,
                        out contact1);
                    contactCount = 1;
                }

                break;
            case ShapeType.Circle:
                if (bodyBShapeType is ShapeType.Polygon)
                {
                    FindCirclePolygonContactPoint(
                        bodyA.Transform2.Position, bodyB.Geometry.TransformedVertices,
                        out contact1);
                    contactCount = 1;
                    break;
                }

                if (bodyBShapeType is ShapeType.Circle)
                {
                    FindCirclesContactPoint(bodyA.Transform2.Position, bodyA.Geometry.Radius, bodyB.Transform2.Position, out contact1);
                    contactCount = 1;
                }

                break;
        }
    }

    private static void FindCirclesContactPoint(Vector2 centerA, float radiusA, Vector2 centerB, out Vector2 contactPoint)
    {
        Vector2 ab = centerB - centerA;
        contactPoint = centerA + Vector2.Normalize(ab) * radiusA;
    }

    private static void FindCirclePolygonContactPoint(Vector2 circleCenter, Vector2[] polygonVertices, out Vector2 contactPoint)
    {
        float minDistanceSquared = float.MaxValue;
        contactPoint = Vector2.Zero;

        for (int i = 0; i < polygonVertices.Length; i++)
        {
            Vector2 va = polygonVertices[i];
            Vector2 vb = polygonVertices[(i + 1) % polygonVertices.Length];
            PointSegmentDistance(circleCenter, va, vb, out float distanceSquared, out Vector2 contact);

            if (distanceSquared < minDistanceSquared)
            {
                minDistanceSquared = distanceSquared;
                contactPoint = contact;
            }
        }

    }

    private static void FindPolygonContactPoints(
        Vector2[] verticesA, Vector2[] verticesB,
        out Vector2 contact1, out Vector2 contact2, out int contactCount)
    {
        contact1 = Vector2.Zero;
        contact2 = Vector2.Zero;
        contactCount = 0;

        float minDistanceSquared = float.MaxValue;

        for (int i = 0; i < verticesA.Length; i++)
        {
            Vector2 p = verticesA[i];

            for (int j = 0; j < verticesB.Length; j++)
            {
                Vector2 va = verticesB[j];
                Vector2 vb = verticesB[(j + 1) % verticesB.Length];

                PointSegmentDistance(p, va, vb, out float distanceSquared, out Vector2 cp);

                // Checks equality of float with a tolerance
                if (MathF.Abs(distanceSquared - minDistanceSquared) < FLOAT_EQUALITY_TOLERANCE)
                {
                    if (!cp.EqualsWithTolerence(contact1, FLOAT_EQUALITY_TOLERANCE))
                    {
                        contact2 = cp;
                        contactCount = 2;
                    }
                }
                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    contactCount = 1;
                    contact1 = cp;
                }
            }
        }

        for (int i = 0; i < verticesB.Length; i++)
        {
            Vector2 p = verticesB[i];

            for (int j = 0; j < verticesA.Length; j++)
            {
                Vector2 va = verticesA[j];
                Vector2 vb = verticesA[(j + 1) % verticesA.Length];

                PointSegmentDistance(p, va, vb, out float distanceSquared, out Vector2 cp);

                // Checks equality of float with a tolerance
                if (MathF.Abs(distanceSquared - minDistanceSquared) < FLOAT_EQUALITY_TOLERANCE)
                {
                    if (!cp.EqualsWithTolerence(contact1, FLOAT_EQUALITY_TOLERANCE))
                    {
                        contact2 = cp;
                        contactCount = 2;
                    }
                }
                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    contactCount = 1;
                    contact1 = cp;
                }
            }
        }
    }

    public static bool Collide(Body bodyA, Body bodyB, out Vector2 normal, out float depth)
    {
        normal = Vector2.Zero;
        depth = 0f;

        GeometryType bodyAShapeType = bodyA.Geometry.GeometryType;
        GeometryType bodyBShapeType = bodyB.Geometry.GeometryType;

        switch (bodyAShapeType)
        {
            case GeometryType.Polygon:
                if (bodyBShapeType is GeometryType.Polygon)
                {
                    return IntersectPolygons(
                        bodyA.Transform2.Position,
                        bodyA.Geometry.TransformedVertices,
                        bodyB.Transform2.Position,
                        bodyB.Geometry.TransformedVertices,
                        out normal, out depth);
                }

                if (bodyBShapeType is GeometryType.Circle)
                {
                    bool result = IntersectCirclePolygon(
                        bodyA.Geometry.TransformedVertices,
                        bodyA.Transform2.Position,
                        bodyB.Transform2.Position,
                        bodyB.Geometry.Radius,
                        out normal, out depth);

                    normal = -normal;
                    return result;
                }

                break;
            case GeometryType.Circle:
                if (bodyBShapeType is GeometryType.Polygon)
                {
                    return IntersectCirclePolygon(
                        bodyB.Geometry.TransformedVertices,
                        bodyB.Transform2.Position,
                        bodyA.Transform2.Position,
                        bodyA.Geometry.Radius,
                        out normal, out depth);
                }

                if (bodyBShapeType is GeometryType.Circle)
                {
                    return IntersectCircles(
                        bodyA.Transform2.Position,
                        bodyA.Geometry.Radius,
                        bodyB.Transform2.Position,
                        bodyB.Geometry.Radius,
                        out normal, out depth);
                }

                break;
        }

        return false;
    }

    private static bool IntersectCircles(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB,
        out Vector2 normal, out float depth)
    {
        normal = Vector2.Zero;
        depth = 0f;

        float distance = VectorHelper.Distance(centerA, centerB);
        float radii = radiusA + radiusB;

        if (distance >= radii)
        {
            return false;
        }

        normal = Vector2.Normalize(centerB - centerA);
        depth = radii - distance;
        return true;
    }

    private static bool IntersectCirclePolygon(Vector2[] vertices, Vector2 polygonCenter, Vector2 circleCenter, float radius,
        out Vector2 normal, out float depth)
    {
        normal = Vector2.Zero;
        depth = float.MaxValue;

        Vector2 axis = Vector2.Zero;
        float axisDepth = 0f;
        float minA, maxA, minB, maxB;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 va = vertices[i];
            Vector2 vb = vertices[(i + 1) % vertices.Length];

            Vector2 edge = vb - va;
            axis = new Vector2(-edge.Y, edge.X);
            axis = Vector2.Normalize(axis);

            ProjectVertices(vertices, axis, out minA, out maxA);
            ProjectCircle(circleCenter, radius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            axisDepth = MathF.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        int cpIndex = FindClosestPointOnPolygon(circleCenter, vertices);
        Vector2 cp = vertices[cpIndex];

        axis = cp - circleCenter;
        axis = Vector2.Normalize(axis);

        ProjectVertices(vertices, axis, out minA, out maxA);
        ProjectCircle(circleCenter, radius, axis, out minB, out maxB);

        if (minA >= maxB || minB >= maxA)
        {
            return false;
        }

        axisDepth = MathF.Min(maxB - minA, maxA - minB);

        if (axisDepth < depth)
        {
            depth = axisDepth;
            normal = axis;
        }

        Vector2 direction = polygonCenter - circleCenter;

        if (Vector2.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        return true;
    }

    private static int FindClosestPointOnPolygon(Vector2 circleCenter, Vector2[] vertices)
    {
        int result = -1;
        float minDistance = float.MaxValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 v = vertices[i];
            float distance = VectorHelper.Distance(v, circleCenter);

            if (distance < minDistance)
            {
                minDistance = distance;
                result = i;
            }
        }

        return result;
    }

    private static void ProjectCircle(Vector2 center, float radius, Vector2 axis, out float min, out float max)
    {
        Vector2 direction = Vector2.Normalize(axis);
        Vector2 directionAndRadius = direction * radius;

        Vector2 p1 = center + directionAndRadius;
        Vector2 p2 = center - directionAndRadius;

        min = Vector2.Dot(p1, axis);
        max = Vector2.Dot(p2, axis);

        if (min > max)
        {
            // swap min and max if they are opposite
            (min, max) = (max, min);
        }
    }
    public static bool IntersectPolygons(Vector2 centerA, Vector2[] verticesA, Vector2 centerB, Vector2[] verticesB, out Vector2 normal, out float depth)
    {
        normal = Vector2.Zero;
        depth = float.MaxValue;

        for (int i = 0; i < verticesA.Length; i++)
        {
            Vector2 va = verticesA[i];
            Vector2 vb = verticesA[(i + 1) % verticesA.Length];

            Vector2 edge = vb - va;
            Vector2 axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

            ProjectVertices(verticesA, axis, out float minA, out float maxA);
            ProjectVertices(verticesB, axis, out float minB, out float maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            float axisDepth = MathF.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        for (int i = 0; i < verticesB.Length; i++)
        {
            Vector2 va = verticesB[i];
            Vector2 vb = verticesB[(i + 1) % verticesB.Length];

            Vector2 edge = vb - va;
            Vector2 axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

            ProjectVertices(verticesA, axis, out float minA, out float maxA);
            ProjectVertices(verticesB, axis, out float minB, out float maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            float axisDepth = MathF.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        Vector2 direction = centerB - centerA;

        if (Vector2.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        return true;
    }

    private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
    {
        min = float.MaxValue;
        max = float.MinValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 v = vertices[i];
            float proj = Vector2.Dot(v, axis);

            if (proj < min)
            {
                min = proj;
            }

            if (proj > max)
            {
                max = proj;
            }
        }
    }
}