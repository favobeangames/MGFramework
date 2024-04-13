using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework;

public static class Utils
{
    /**
     * Scales vector coordinates to make it a unit vector
     */
    public static void Normalize(ref float x, ref float y)
    {
        float invLen = 1f / MathF.Sqrt(x * x + y * y);

        x *= invLen;
        y *= invLen;
    }
}