using System.Collections.Generic;
using FavobeanGames.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.DataStructures.Geometry
{
    public enum geometryType
    {
        Line,
        Circle,
        ConcavePolygon,
        ConvexPolygon
    }

    /// <summary>
    ///     geometry shape.
    /// </summary>
    public class Geometry
    {
        public Vector2 CenterPoint;
        public List<Point> Points;

        protected Sprite Sprite;

        public Geometry()
        {
        }

        public Geometry(params Vector2[] points)
        {
            InitializePoints(points);
        }

        public Geometry(params Point[] points)
        {
            foreach (var point in points) Points.Add(point);
        }

        public bool ArePointsVisible { get; set; }

        public virtual void Initialize()
        {
        }

        /// <summary>
        ///     Checks collision with point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool CollidesWith(Vector2 point)
        {
            return false;
        }

        /// <summary>
        ///     Although a circle is a geometry, it handles collision slightly differently
        /// </summary>
        /// <param name="circle">Circle object</param>
        /// <returns></returns>
        public virtual bool CollidesWith(Circle circle)
        {
            return false;
        }

        /// <summary>
        ///     Checks collision with other primitvie (polygon)
        /// </summary>
        /// <param name="Geometry">Polygon geometry</param>
        /// <returns></returns>
        public virtual bool CollidesWith(Geometry geometry)
        {
            return false;
        }

        #region Validation Functions

        /// <summary>
        ///     Validates geometry at initialization. This will ensure proper parameters were
        ///     passed when creating geometry objects
        /// </summary>
        /// <returns></returns>
        private bool ValidateGeometry()
        {
            return true;
        }

        #endregion

        #region Sprite Functions

        /// <summary>
        ///     If we want to draw the geometry shape we need to initialize
        ///     the sprite object
        /// </summary>
        public virtual void InitializeSprite()
        {
            Sprite = new Sprite();
        }

        public virtual void UpdateSprite(GameTime gameTime)
        {
            if (Sprite != null) Sprite.Update(gameTime);
        }

        public virtual void DrawSprite(SpriteBatch spriteBatch)
        {
            if (Sprite != null) Sprite.Draw(spriteBatch);
        }

        public virtual void DrawPoints(SpriteBatch spriteBatch)
        {
            if (Points == null) return;

            foreach (var point in Points) point.DrawSprite(spriteBatch);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Initializes physics geometry to be used for collisions.
        /// </summary>
        public virtual void InitializeGeometry()
        {
        }
        
        public virtual void InitializePoints(Vector2[] points)
        {
            Points = new List<Point>();
            foreach (var point in points) Points.Add(new Point(point));
        }

        /// <summary>
        /// Looks at all points and updates any positional changes
        /// </summary>
        public virtual void UpdatePoints()
        {
            if (Points == null) return;

            foreach (var point in Points) point.Sprite.Position = point.CenterPoint;
        }

        #endregion
    }
}