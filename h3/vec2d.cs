using System;

namespace h3
{
    /** @struct Vec2d
     *  @brief 2D floating-point vector
     */
    public struct Vec2d
    {
        public double x;  ///< x component
        public double y;  ///< y component

        /**
         * Calculates the magnitude of a 2D cartesian vector.
         * @param v The 2D cartesian vector.
         * @return The magnitude of the vector.
         */
        public static double _v2dMag(in Vec2d v) { return Math.Sqrt(v.x * v.x + v.y * v.y); }

        /**
         * Finds the intersection between two lines. Assumes that the lines intersect
         * and that the intersection is not at an endpoint of either line.
         * @param p0 The first endpoint of the first line.
         * @param p1 The second endpoint of the first line.
         * @param p2 The first endpoint of the second line.
         * @param p3 The second endpoint of the second line.
         * @param inter The intersection point.
         */
        public static void _v2dIntersect(in Vec2d p0, in Vec2d p1, in Vec2d p2, in Vec2d p3, ref Vec2d inter) {
            Vec2d s1, s2;
            s1.x = p1.x - p0.x;
            s1.y = p1.y - p0.y;
            s2.x = p3.x - p2.x;
            s2.y = p3.y - p2.y;

            double t;
            t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) /
                (-s2.x * s1.y + s1.x * s2.y);

            inter.x = p0.x + (t * s1.x);
            inter.y = p0.y + (t * s1.y);
        }

        /**
         * Whether two 2D vectors are equal. Does not consider possible false
         * negatives due to floating-point errors.
         * @param v1 First vector to compare
         * @param v2 Second vector to compare
         * @return Whether the vectors are equal
         */
        public static bool _v2dEquals(in Vec2d v1, in Vec2d v2) {
            return v1.x == v2.x && v1.y == v2.y;
        }
    }
}