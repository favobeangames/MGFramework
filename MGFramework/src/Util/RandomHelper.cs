using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Util;

public static class RandomHelper
{
    private static readonly Random Rand = new Random();

    public static int RandomInt(int min, int max)
    {
        return Rand.Next(min, max);
    }

    public static float RandomFloat(float min, float max)
    {
        return Rand.NextSingle(min, max);
    }
    public static Color RandomColor()
    {
        Color result = new Color((float)Rand.NextDouble(), (float)Rand.NextDouble(), (float)Rand.NextDouble());
        return result;
    }

    public static Color RandomColor(Random rand)
    {
        Color result = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
        return result;
    }

    public static bool RandomBoolean()
    {
        int value = Rand.Next(0, 2);

        if (value == 0)
        {
            return false;
        }

        return true;
    }
}