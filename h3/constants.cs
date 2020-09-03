using System;

namespace h3
{
    public class Constants
    {
            /** pi */
            public const double M_PI = Math.PI;
            
            /** pi / 2.0 */
            public const double M_PI_2 = M_PI / 2;

            /** 2.0 * PI */
            public const double M_2PI = M_PI * 2;

            /** pi / 180 */
            public const double M_PI_180 = M_PI / 180;

            /** pi * 180 */
            public const double M_180_PI = M_PI * 180;

            /** threshold epsilon */
            public const double EPSILON = 0.0000000000000001;
            
            /** sqrt(3) / 2.0 */
            public const double M_SQRT3_2 = 0.8660254037844386467637231707529361834714;

            /** sin(60') */
            public const double M_SIN60 = M_SQRT3_2;

            /** rotation angle between Class II and Class III resolution axes
             * (asin(sqrt(3.0 / 28.0))) */
            public const double M_AP7_ROT_RADS = 0.333473172251832115336090755351601070065900389;

            /** sin(M_AP7_ROT_RADS) */
            public const double M_SIN_AP7_ROT = 0.3273268353539885718950318;

            /** cos(M_AP7_ROT_RADS) */
            public const double M_COS_AP7_ROT = 0.9449111825230680680167902;

            /** earth radius in kilometers using WGS84 authalic radius */
            public const double EARTH_RADIUS_KM = 6371.007180918475;

            /** scaling factor from hex2d resolution 0 unit length
             * (or distance between adjacent cell center points
             * on the plane) to gnomonic unit length. */
            public const double RES0_U_GNOMONIC = 0.38196601125010500003;

            /** max H3 resolution; H3 version 1 has 16 resolutions, numbered 0 through 15 */
            public const int MAX_H3_RES = 15;

            /** The number of faces on an icosahedron */
            public const int NUM_ICOSA_FACES = 20;

            /** The number of H3 base cells */
            public const int NUM_BASE_CELLS = 122;

            /** The number of vertices in a hexagon */
            public const int NUM_HEX_VERTS = 6;
            
            /** The number of vertices in a pentagon */
            public const int NUM_PENT_VERTS = 5;
            
            /** The number of pentagons per resolution **/
            public const int NUM_PENTAGONS = 12;

            /** H3 index modes */
            public const int H3_HEXAGON_MODE = 1;
            public const int H3_UNIEDGE_MODE = 2;
    }
}