using FavobeanGames.Components;
using Microsoft.Xna.Framework;

namespace FavobeanGames.DataStructures.Geometry
{
    public class Circle : Geometry
    {
        // Radius of circle
        public float Radius;
        
        public Circle()
        {
        }

        public Circle(Vector2 center, float radius)
        {
            CenterPoint = center;
            Radius = radius;
        }
        
        #region Collision

        public override bool CollidesWith(Vector2 point)
        {
            var dist = Vector2.Distance(CenterPoint, point);
            return dist <= Radius;
        }

        public override bool CollidesWith(Geometry geometry)
        {
            return geometry.CollidesWith(this);
        }

        public override bool CollidesWith(Circle circle)
        {
            var dist = Vector2.Distance(CenterPoint, circle.CenterPoint);
            return dist <= Radius;
        }

        #endregion
    }
}