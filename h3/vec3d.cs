using System;

namespace h3
{
    public struct Vec3d {
        public double x;  ///< x component
        public double y;  ///< y component
        public double z;  ///< z component
        
        /**
         * Square of a number
         *
         * @param x The input number.
         * @return The square of the input number.
         */
        public static double _square(double x) { return x * x; }

        /**
         * Calculate the square of the distance between two 3D coordinates.
         *
         * @param v1 The first 3D coordinate.
         * @param v2 The second 3D coordinate.
         * @return The square of the distance between the given points.
         */
        public static double _pointSquareDist(in Vec3d v1, in Vec3d v2) {
            return _square(v1.x - v2.x) + _square(v1.y - v2.y) +
                   _square(v1.z - v2.z);
        }

        /**
         * Calculate the 3D coordinate on unit sphere from the latitude and longitude.
         *
         * @param geo The latitude and longitude of the point.
         * @param v The 3D coordinate of the point.
         */
        public static void _geoToVec3d(in GeoCoord geo, ref Vec3d v) {
            double r = Math.Cos(geo.lat);

            v.z = Math.Sin(geo.lat);
            v.x = Math.Cos(geo.lon) * r;
            v.y = Math.Sin(geo.lon) * r;
        }
    }
}