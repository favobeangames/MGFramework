using System;
using Microsoft.Xna.Framework;

namespace FavobeanGames
{
    public class Algorithms
    {
        /// <summary>
        ///     Returns the distance between two points in 2D space
        /// </summary>
        /// <param name="p1">Starting point</param>
        /// <param name="p2">Ending point</param>
        /// <returns>Return the magnitude as a double</returns>
        public static double GetVectorMagnitude(Vector2 p1, Vector2 p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        /// <summary>
        ///     Returns the distance between two points in 3D space
        /// </summary>
        /// <param name="p1">Starting point</param>
        /// <param name="p2">Ending point</param>
        /// <returns>Return the magnitude as a double</returns>
        public static double GetVectorMagnitude(Vector3 p1, Vector3 p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }

        /// <summary>
        ///     Returns the direction in degrees of a vector from p1 to p2
        /// </summary>
        /// <param name="p1">Starting point</param>
        /// <param name="p2">Destination</param>
        /// <returns>Angle in radians</returns>
        public static double GetVectorDirection(Vector2 p1, Vector2 p2)
        {
            return Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }

        /// <summary>
        ///     Returns point along the slope between two points by the percentage of the weight
        ///     x = start + (destination - start) * weight
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="destination">Desination point</param>
        /// <param name="weight">Percentage we want to interpolate (0 - 1)</param>
        /// <returns></returns>
        public static Vector2 LinearInterpolation(Vector2 start, Vector2 destination, float weight)
        {
            return start + (destination - start) * weight;
        }

        public static double LinearInterpolation(double start, double destination, float weight)
        {
            return start + (destination - start) * weight;
        }

        /// <summary>
        ///     Returns the slope of two points
        ///     Y2 - Y1 / X2 - X1
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double GetSlopeOfPoints(Vector2 start, Vector2 end)
        {
            float y, x;
            if (start.Y <= end.Y)
                y = end.Y - start.Y;
            else
                y = start.Y - end.Y;

            if (start.X <= end.X)
                x = end.X - start.X;
            else
                x = start.X - end.X;
            return y / x;
        }

        /// <summary>
        ///     Gets the angle between two vectors. This will assume the origin of both vectors is (0,0)
        ///     Converts vectors into unit vectors and uses the dot product
        ///     DP = v1x * v2x + v1y * v2y
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>Returns range between -1 and 1. -1 being 180 degree angle and 1 being 0</returns>
        public static double GetAngleBetweenTwoVectors(Vector2 v1, Vector2 v2)
        {
            var v1Unit = CalculateUnitVector(v1);
            var v2Unit = CalculateUnitVector(v2);

            return v1Unit.X * v2Unit.X + v1Unit.Y * v2Unit.Y;
        }

        /// <summary>
        ///     Returns the dot product of AB * BC
        ///     AB * BC = |AB| * |BC| * Cos(theta)
        /// </summary>
        /// <param name="a">Point a</param>
        /// <param name="b">Point b</param>
        /// <param name="c">Point c</param>
        /// <returns>Angle between vectors</returns>
        public static float DotProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            var BAx = a.X - b.X;
            var BAy = a.Y - b.Y;
            var BCx = c.X - b.X;
            var BCy = c.Y - b.Y;

            // Calculate the dot product
            return BAx * BCx + BAy * BCy;
        }

        /// <summary>
        ///     Return the cross product AB * BC.
        ///     The cross product is a vector perpendicular to AB
        ///     and BC having length |AB| * |BC| * Sin(theta) and
        ///     with direction given by the right-hand rule.
        ///     For two vectors in the X-Y plane, the result is a
        ///     vector with X and Y components 0 so the Z component
        ///     gives the vector's length and direction.
        /// </summary>
        /// <param name="a">Point a</param>
        /// <param name="b">Point b</param>
        /// <param name="c">Point c</param>
        /// <returns>float of the </returns>
        public static float CrossProductLength(Vector2 a, Vector2 b, Vector2 c)
        {
            // Get the vectors' coordinates.
            var BAx = a.X - b.X;
            var BAy = a.Y - b.Y;
            var BCx = c.X - b.X;
            var BCy = c.Y - b.Y;

            // Calculate the Z coordinate of the cross product.
            return BAx * BCy - BAy * BCx;
        }

        #region Helpers

        /// <summary>
        ///     Converts radians to degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians / Math.PI * 180;
        }

        /// <summary>
        ///     Converts degrees to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees / 180 * Math.PI;
        }

        /// <summary>
        ///     Calculates the unit vector of the given vector by dividing the given vector by its magnitude
        /// </summary>
        /// <param name="v1">Vector to convert</param>
        /// <returns>Unit vector as Vector2</returns>
        public static Vector2 CalculateUnitVector(Vector2 v1)
        {
            var magnitude = GetVectorMagnitude(new Vector2(0, 0), v1);
            return v1 / (float) magnitude;
        }

        /// <summary>
        ///     Calculates the unit vector of the given vector by dividing the given vector by its magnitude
        /// </summary>
        /// <param name="v1">Vector start point</param>
        /// <param name="v2">Vector end point</param>
        /// <returns>Unit vector as Vector2</returns>
        public static Vector2 CalculateUnitVector(Vector2 v1, Vector2 v2)
        {
            var magnitude = GetVectorMagnitude(v1, v2);
            return v2 / (float) magnitude;
        }

        /// <summary>
        ///     Calculates weight for linear interpolation by
        ///     dividing elapsed time by the duration
        /// </summary>
        /// <param name="elapsed">Time passed since start of linear interpolation</param>
        /// <param name="duration">Total duration of linear interpolation</param>
        /// <returns></returns>
        public static float CalculateWeight(float elapsed, float duration)
        {
            return elapsed / duration;
        }

        #endregion
    }
}