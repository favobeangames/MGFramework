using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Physics.Math;

public static class PolygonHelper
{

    public static float FindPolygonArea(Vector2[] vertices)
    {
        float totalArea = 0f;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 a = vertices[i];
            // Mod will ensure we don't go out of bounds of the array, as well as
            // ensures we loop back to the starting point of the polygon
            Vector2 b = vertices[(i + 1) % vertices.Length];

            float dy = (a.Y + b.Y) / 2f;
            float dx = (b.X - a.X);

            float area = dy * dx;
            totalArea += area;
        }

        return totalArea;
    }
}