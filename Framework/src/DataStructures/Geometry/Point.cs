using Microsoft.Xna.Framework;

namespace FavobeanGames.Framework.DataStructures.Geometry
{
    public class Point : Circle
    {

        public Point()
        {
        }

        public Point(Vector2 point, float radius = 5)
        {
            CenterPoint = point;
            Radius = radius;
        }
    }
}