using System;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Physics.Math;

public static class VectorHelper
{
    #region Vector Math

    /// <summary>
    /// Returns the distance or magnitude of the vector
    /// </summary>
    /// <param name="v">Vector2</param>
    /// <returns>Distance of the vector</returns>
    public static float Distance(Vector2 v)
    {
        return MathF.Sqrt(v.X * v.X + v.Y * v.Y);
    }

    /// <summary>
    /// Returns the distance or magnitude between the vectors
    /// </summary>
    /// <param name="v1">First Vector</param>
    /// <param name="v2">Second Vector</param>
    /// <returns>Distance value between the vectors</returns>
    public static float Distance(Vector2 v1, Vector2 v2)
    {
        float dx = v1.X - v2.X;
        float dy = v1.Y - v2.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Returns the cross product for two vectors in a 2D space
    /// </summary>
    /// <param name="v1">First vector</param>
    /// <param name="v2">Second vector</param>
    /// <returns></returns>
    public static float CrossProduct2D(Vector2 v1, Vector2 v2)
    {
        return v1.X * v2.Y - v1.Y * v2.X;
    }

    #endregion

}