using System;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Geometry2D.Shapes;

public class Vector
{
    private const float FLOAT_TOLERANCE = 0.0005f;
    public float X { get; set; }
    public float Y { get; set; }

    public Vector(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Returns a new Vector with X and Y values of NaN
    /// </summary>
    public static readonly Vector Empty = new Vector(float.NaN, float.NaN);

    /// <summary>
    /// Returns a new Vector with X and Y values of 0
    /// </summary>
    public static readonly Vector Zero = new Vector(0, 0);

    /// <summary>
    /// Casts the vector object to a MonoGame Vector2 object
    /// </summary>
    /// <returns>MonoGame Vector2 object</returns>
    public Vector2 ToMgVector2()
    {
        return new Vector2(X, Y);
    }

    public static Vector operator- (Vector v1, Vector v2)
    {
        return new Vector(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static Vector operator+ (Vector v1, Vector v2)
    {
        return new Vector(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static bool operator== (Vector v1, Vector v2) => System.Math.Abs(v1!.X - v2!.X) < FLOAT_TOLERANCE && System.Math.Abs(v1.Y - v2.Y) < FLOAT_TOLERANCE;

    public static bool operator!= (Vector v1, Vector v2) => System.Math.Abs(v1!.X - v2!.X) > FLOAT_TOLERANCE || System.Math.Abs(v1.Y - v2.Y) > FLOAT_TOLERANCE;

    public override bool Equals(object obj)
    {
        if (obj is Vector vector)
        {
            return this == vector;
        }

        return false;
    }

    protected bool Equals(Vector other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}