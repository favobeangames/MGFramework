using System;
using System.Numerics;
using Vector = FavobeanGames.MGFramework.Geometry2D.Shapes.Vector;

namespace FavobeanGames.MGFramework.Geometry2D.Math;

public static class VectorMath
{
    /// <summary>
    /// Returns the cosine of the angle between two vectors.
    /// Determines the direction of forces (Ex. Are two objects moving towards
    /// one another?)
    /// </summary>
    /// <param name="v1">first Vector object</param>
    /// <param name="v2">second Vector object</param>
    /// <returns>Angle between two vectors</returns>
    public static float Dot(Vector v1, Vector v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y;
    }

    /// <summary>
    /// Returns the cosine of the angle between two vectors.
    /// Determines the direction of forces (Ex. Are two objects moving towards
    /// one another?)
    /// </summary>
    /// <param name="x1">X-coordinate of the first vector</param>
    /// <param name="y1">Y-coordinate of the first vector</param>
    /// <param name="x2">X-coordinate of the second vector</param>
    /// <param name="y2">Y-coordinate of the second vector</param>
    /// <returns></returns>
    public static float Dot(float x1, float y1, float x2, float y2)
    {
        return x1 * x2 + y1 * y2;
    }

    /// <summary>
    /// Returns a vector that is perpendicular to both input vectors.
    /// This will help determine the orientation of surfaces and finding
    /// collision normals
    /// </summary>
    /// <param name="v1">first Vector object</param>
    /// <param name="v2">second Vector object</param>
    /// <returns>z component of the cross product</returns>
    public static float Cross(Vector v1, Vector v2)
    {
        return v1.X * v2.Y - v1.Y * v2.X;
    }

    /// <summary>
    /// Returns a vector that is perpendicular to both input vectors.
    /// This will help determine the orientation of surfaces and finding
    /// collision normals
    /// </summary>
    /// <param name="x1">X-coordinate of the first vector</param>
    /// <param name="y1">Y-coordinate of the first vector</param>
    /// <param name="x2">X-coordinate of the second vector</param>
    /// <param name="y2">Y-coordinate of the second vector</param>
    /// <returns>z component of the cross product</returns>
    public static float Cross(float x1, float y1, float x2, float y2)
    {
        return x1 * y2 - y1 * x2;
    }

    /// <summary>
    /// Calculates the length of a vector.
    /// Helpful with normalization and collision response calculations
    /// </summary>
    /// <param name="v1">Vector to determine the length of</param>
    /// <returns>the length of the vector</returns>
    public static float Magnitude(Vector v1)
    {
        return MathF.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
    }

    /// <summary>
    /// Calculates the length of a vector.
    /// Helpful with normalization and collision response calculations
    /// </summary>
    /// <param name="x1">X-coordinate of the vector</param>
    /// <param name="y1">Y-coordinate of the vector</param>
    /// <returns>the length of the vector</returns>
    public static float Magnitude(float x, float y)
    {
        return MathF.Sqrt(x * x + y * y);
    }

    /// <summary>
    /// Creates a unit vector pointing in the same direction of the input vector.
    /// Helpful when we need to move objects in the same direction.
    /// </summary>
    /// <param name="v1">Vector to get the direction of</param>
    /// <returns>vector that represents the direction of the input vector</returns>
    public static Vector Normalize(Vector v)
    {
        var magnitude = Magnitude(v);
        return new Vector(
            v.X / magnitude,
            v.Y / magnitude
        );
    }

    /// <summary>
    /// Creates a unit vector pointing in the same direction of the input vector.
    /// Helpful when we need to move objects in the same direction.
    /// </summary>
    /// <param name="x1">X-coordinate of the vector</param>
    /// <param name="y1">Y-coordinate of the vector</param>
    /// <returns>vector that represents the direction of the input vector</returns>
    public static Vector Normalize(float x, float y)
    {
        var magnitude = Magnitude(x, y);
        return new Vector(
            x / magnitude,
            y / magnitude
        );
    }

    /// <summary>
    /// Returns a vector that lies in the direction of the v1 input vector of the v2 input vector.
    /// Helpful determining velocities that contribute to a collision.
    /// </summary>
    /// <param name="v1">Vector object</param>
    /// <param name="v2">Vector object</param>
    /// <returns>Vector of the projection</returns>
    public static Vector Project(Vector v1, Vector v2)
    {
        float dot = Dot(v1, v2);
        float bMagnitudeSquared = Dot(v2, v2);
        float scalar = dot / bMagnitudeSquared;
        return new Vector(scalar * v2.X, scalar * v2.Y);
    }

    /// <summary>
    /// Returns a vector that lies in the direction of the v1 input vector of the v2 input vector.
    /// Helpful determining velocities that contribute to a collision.
    /// </summary>
    /// <param name="x1">X-coordinate of the first vector</param>
    /// <param name="y1">Y-coordinate of the first vector</param>
    /// <param name="x2">X-coordinate of the second vector</param>
    /// <param name="y2">Y-coordinate of the second vector</param>
    /// <returns>Vector of the projection</returns>
    public static Vector Project(float x1, float y1, float x2, float y2)
    {
        float dot = Dot(x1, y1, x2, y2);
        float bMagnitudeSquared = Dot(x2, y2, x2, y2);
        float scalar = dot / bMagnitudeSquared;
        return new Vector(scalar * x2, scalar * y2);
    }

    /// <summary>
    /// Returns a reflection vector from the incident vector v1 and a surface
    /// normal vector.
    /// This will determine the bounce direction of collisions.
    /// </summary>
    /// <param name="v">Vector of the direction of the collision</param>
    /// <param name="normal">Surface normal vector to collide off</param>
    /// <returns>Vector </returns>
    public static Vector Reflection(Vector v, Vector normal)
    {
        float dot = Dot(v, normal);
        return new Vector(
            v.X - 2 * dot * normal.X,
            v.Y - 2 * dot * normal.Y
        );
    }

    /// <summary>
    /// Calculates the distance between two points
    /// </summary>
    /// <param name="v1">first point</param>
    /// <param name="v2">second point</param>
    /// <returns>Distance between the vectors</returns>
    public static float DistanceBetween(Vector v1, Vector v2)
    {
        return MathF.Sqrt(MathF.Pow(v2.X - v1.X, 2) + MathF.Pow(v2.Y - v1.Y, 2));
    }
}