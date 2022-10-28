using System.Collections.Generic;
using System.Drawing;
using FavobeanGames.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.Framework.DataStructures.Geometry
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

        #region Helpers

        public virtual void InitializePoints(Vector2[] points)
        {
            Points = new List<Point>();
            foreach (var point in points) Points.Add(new Point(point));
        }

        /// <summary>
        /// Returns Axis Aligned Bounding Box for the geometry
        /// </summary>
        /// <returns>Rectangle</returns>
        public RectangleF AABB()
        {
            return null;
        }

        #endregion
    }
}