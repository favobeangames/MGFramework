using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FavobeanGames.DataStructures.Geometry
{
    public class 
        Polygon : Geometry
    {
        public List<Line> Lines;

        public Polygon()
        {
        }

        public Polygon(List<Vector2> points)
        {
            if (!IsValidPolygon(points)) throw new Exception("");
        }

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

        #region Helpers

        /// <summary>
        ///     Checks to see if polygon is convex
        /// </summary>
        /// <returns>Boolean if polygon is convex</returns>
        private bool PolygonIsConvex()
        {
            // For each set of three adjacent points A, B, C,
            // find the cross product AB · BC. If the sign of
            // all the cross products is the same, the angles
            // are all positive or negative (depending on the
            // order in which we visit them) so the polygon
            // is convex.
            var gotNegative = false;
            var gotPositive = false;
            var numPoints = Points.Count;
            int B, C;
            for (var A = 0; A < numPoints; A++)
            {
                B = (A + 1) % numPoints;
                C = (B + 1) % numPoints;

                var crossProduct =
                    Algorithms.CrossProductLength(
                        Points[A].CenterPoint,
                        Points[B].CenterPoint,
                        Points[C].CenterPoint);
                if (crossProduct < 0)
                    gotNegative = true;
                else if (crossProduct > 0) gotPositive = true;
                if (gotNegative && gotPositive) return false;
            }

            // Otherwise, the polygon is convex
            return true;
        }

        protected void GenerateLinesFromPoints()
        {
            if (Points != null)
                for (var i = 0; i < Points.Count; i++)
                    if (i + 1 <= Points.Count)
                        Lines.Add(new Line(
                            Points[i],
                            Points[i + 1])
                        );
        }

        /// <summary>
        ///     A Polygon is valid when it contains a list of more then 3 points, and the last point
        ///     is equal to the first. If any of the polygons lines intersect it is also deemed an invalid
        ///     polygon
        /// </summary>
        /// <returns>Boolean if the polygon is valid</returns>
        protected bool IsValidPolygon(List<Vector2> points)
        {
            if (points != null && points.Count >= 3 && points[0].Equals(Points[Points.Count - 1])) return true;

            return false;
        }

        #endregion
    }
}