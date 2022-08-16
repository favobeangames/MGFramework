using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.DataStructures.Geometry
{
    /// <summary>
    ///     It's a line... Inherits from MathNet.Spatial.Euclidean.Line2D
    /// </summary>
    public class Line : Geometry
    {
        private Rectangle lineWidth;

        public Line()
        {
        }

        public Line(params Vector2[] points)
            : base(points)
        {
            CalculateMidpoint();
        }

        public Line(params Point[] points)
            : base(points)
        {
            CalculateMidpoint();
        }

        public void UpdatePoint(int pointIndex, Vector2 point)
        {
            Points[pointIndex].CenterPoint = point;
            InitializeSprite();
        }

        #region Helpers

        /// <summary>
        ///     Calculates the midpoint of the line
        ///     M = ((x1 + x2)/2, (y1 + y2)/2)
        /// </summary>
        private void CalculateMidpoint()
        {
            if (Points.Count == 2)
                CenterPoint =
                    new Vector2(
                        (Points[0].CenterPoint.X + Points[1].CenterPoint.X) / 2,
                        (Points[0].CenterPoint.Y + Points[1].CenterPoint.Y) / 2);
        }

        #endregion

        #region Sprite Functions

        public override void InitializeSprite()
        {
            base.InitializeSprite();
            lineWidth = new Rectangle(
                (int) Points[0].CenterPoint.X,
                (int) Points[0].CenterPoint.Y,
                (int) Vector2.Distance(Points[0].CenterPoint, Points[1].CenterPoint),
                1);
            Sprite.Rotation = (float) Algorithms.GetVectorDirection(Points[0].CenterPoint, Points[1].CenterPoint);
        }

        public override void DrawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                TextureCreator.PixelTexture,
                lineWidth,
                TextureCreator.PixelTexture.Bounds,
                Sprite.Color,
                Sprite.Rotation,
                Sprite.Origin,
                Sprite.SpriteEffect,
                Sprite.Depth);

            if (ArePointsVisible) DrawPoints(spriteBatch);
        }

        #endregion

        #region Overrides

        public override bool CollidesWith(Vector2 point)
        {
            return base.CollidesWith(point);
        }

        public override bool CollidesWith(Circle circle)
        {
            return base.CollidesWith(circle);
        }

        public override bool CollidesWith(Geometry geometry)
        {
            return base.CollidesWith(geometry);
        }

        #endregion
    }
}