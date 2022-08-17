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
            InitializeSprite();
        }

        #region Sprite Functions

        public override void InitializeSprite()
        {
            Sprite = new Sprite();
            Sprite.Texture = TextureCreator.CreateCircle((int) Radius, Color.White);
            Sprite.SourceRect = Sprite.Texture.Bounds;
            Sprite.Position = CenterPoint;
            Sprite.Color = Color.White;
            Sprite.Origin = new Vector2(
                Sprite.Texture.Bounds.Width / 2,
                Sprite.Texture.Bounds.Height / 2);
        }

        #endregion

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