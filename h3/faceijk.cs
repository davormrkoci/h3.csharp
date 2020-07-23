using System;
using h3;

namespace h3
{
    /** @struct FaceIJK
     * @brief Face number and ijk coordinates on that face-centered coordinate
     * system
     */
    public struct FaceIJK {
        public int face;        ///< face number
        public CoordIJK coord;  ///< ijk coordinates on that face
        
        // indexes for faceNeighbors table
        /** IJ quadrant faceNeighbors table direction */
        public const int IJ = 1;
        /** KI quadrant faceNeighbors table direction */
        public const int KI = 2;
        /** JK quadrant faceNeighbors table direction */
        public const int JK = 3;
        /** Invalid face index */
        public const int INVALID_FACE = -1;

        /** square root of 7 */
        private const double M_SQRT7 = 2.6457513110645905905016157536392604257102;

        /** @brief icosahedron face centers in lat/lon radians */
        static readonly GeoCoord[] faceCenterGeo = {
            new GeoCoord { lat = 0.803582649718989942, lon = 1.248397419617396099 },    // face  0
            new GeoCoord { lat = 1.307747883455638156, lon = 2.536945009877921159 },    // face  1
            new GeoCoord { lat = 1.054751253523952054, lon = -1.347517358900396623 },   // face  2
            new GeoCoord { lat = 0.600191595538186799, lon = -0.450603909469755746 },   // face  3
            new GeoCoord { lat = 0.491715428198773866, lon = 0.401988202911306943 },    // face  4
            new GeoCoord { lat = 0.172745327415618701, lon = 1.678146885280433686 },    // face  5
            new GeoCoord { lat = 0.605929321571350690, lon = 2.953923329812411617 },    // face  6
            new GeoCoord { lat = 0.427370518328979641, lon = -1.888876200336285401 },   // face  7
            new GeoCoord { lat = -0.079066118549212831, lon = -0.733429513380867741 },  // face  8
            new GeoCoord { lat = -0.230961644455383637, lon = 0.506495587332349035 },   // face  9
            new GeoCoord { lat = 0.079066118549212831, lon = 2.408163140208925497 },    // face 10
            new GeoCoord { lat = 0.230961644455383637, lon = -2.635097066257444203 },   // face 11
            new GeoCoord { lat = -0.172745327415618701, lon = -1.463445768309359553 },  // face 12
            new GeoCoord { lat = -0.605929321571350690, lon = -0.187669323777381622 },  // face 13
            new GeoCoord { lat = -0.427370518328979641, lon = 1.252716453253507838 },   // face 14
            new GeoCoord { lat = -0.600191595538186799, lon = 2.690988744120037492 },   // face 15
            new GeoCoord { lat = -0.491715428198773866, lon = -2.739604450678486295 },  // face 16
            new GeoCoord { lat = -0.803582649718989942, lon = -1.893195233972397139 },  // face 17
            new GeoCoord { lat = -1.307747883455638156, lon = -0.604647643711872080 },  // face 18
            new GeoCoord { lat = -1.054751253523952054, lon = 1.794075294689396615 },   // face 19
        };

        /** @brief icosahedron face centers in x/y/z on the unit sphere */
        static readonly Vec3d[] faceCenterPoint = {
            new Vec3d { x = 0.2199307791404606, y = 0.6583691780274996, z = 0.7198475378926182 },     // face  0
            new Vec3d { x = -0.2139234834501421, y = 0.1478171829550703, z = 0.9656017935214205 },    // face  1
            new Vec3d { x = 0.1092625278784797, y = -0.4811951572873210, z = 0.8697775121287253 },    // face  2
            new Vec3d { x = 0.7428567301586791, y = -0.3593941678278028, z = 0.5648005936517033 },    // face  3
            new Vec3d { x = 0.8112534709140969, y = 0.3448953237639384, z = 0.4721387736413930 },     // face  4
            new Vec3d { x = -0.1055498149613921, y = 0.9794457296411413, z = 0.1718874610009365 },    // face  5
            new Vec3d { x = -0.8075407579970092, y = 0.1533552485898818, z = 0.5695261994882688 },    // face  6
            new Vec3d { x = -0.2846148069787907, y = -0.8644080972654206, z = 0.4144792552473539 },   // face  7
            new Vec3d { x = 0.7405621473854482, y = -0.6673299564565524, z = -0.0789837646326737 },   // face  8
            new Vec3d { x = 0.8512303986474293, y = 0.4722343788582681, z = -0.2289137388687808 },    // face  9
            new Vec3d { x = -0.7405621473854481, y = 0.6673299564565524, z = 0.0789837646326737 },    // face 10
            new Vec3d { x = -0.8512303986474292, y = -0.4722343788582682, z = 0.2289137388687808 },   // face 11
            new Vec3d { x = 0.1055498149613919, y = -0.9794457296411413, z = -0.1718874610009365 },   // face 12
            new Vec3d { x = 0.8075407579970092, y = -0.1533552485898819, z = -0.5695261994882688 },   // face 13
            new Vec3d { x = 0.2846148069787908, y = 0.8644080972654204, z = -0.4144792552473539 },    // face 14
            new Vec3d { x = -0.7428567301586791, y = 0.3593941678278027, z = -0.5648005936517033 },   // face 15
            new Vec3d { x = -0.8112534709140971, y = -0.3448953237639382, z = -0.4721387736413930 },  // face 16
            new Vec3d { x = -0.2199307791404607, y = -0.6583691780274996, z = -0.7198475378926182 },  // face 17
            new Vec3d { x = 0.2139234834501420, y = -0.1478171829550704, z = -0.9656017935214205 },   // face 18
            new Vec3d { x = -0.1092625278784796, y = 0.4811951572873210, z = -0.8697775121287253 },   // face 19
        };

        /** @brief icosahedron face ijk axes as azimuth in radians from face center to
         * vertex 0/1/2 respectively
         */
        static readonly double[][] faceAxesAzRadsCII = {
            new [] {5.619958268523939882, 3.525563166130744542,
             1.431168063737548730},  // face  0
            new [] {5.760339081714187279, 3.665943979320991689,
             1.571548876927796127},  // face  1
            new [] {0.780213654393430055, 4.969003859179821079,
             2.874608756786625655},  // face  2
            new [] {0.430469363979999913, 4.619259568766391033,
             2.524864466373195467},  // face  3
            new [] {6.130269123335111400, 4.035874020941915804,
             1.941478918548720291},  // face  4
            new [] {2.692877706530642877, 0.598482604137447119,
             4.787272808923838195},  // face  5
            new [] {2.982963003477243874, 0.888567901084048369,
             5.077358105870439581},  // face  6
            new [] {3.532912002790141181, 1.438516900396945656,
             5.627307105183336758},  // face  7
            new [] {3.494305004259568154, 1.399909901866372864,
             5.588700106652763840},  // face  8
            new [] {3.003214169499538391, 0.908819067106342928,
             5.097609271892733906},  // face  9
            new [] {5.930472956509811562, 3.836077854116615875,
             1.741682751723420374},  // face 10
            new [] {0.138378484090254847, 4.327168688876645809,
             2.232773586483450311},  // face 11
            new [] {0.448714947059150361, 4.637505151845541521,
             2.543110049452346120},  // face 12
            new [] {0.158629650112549365, 4.347419854898940135,
             2.253024752505744869},  // face 13
            new [] {5.891865957979238535, 3.797470855586042958,
             1.703075753192847583},  // face 14
            new [] {2.711123289609793325, 0.616728187216597771,
             4.805518392002988683},  // face 15
            new [] {3.294508837434268316, 1.200113735041072948,
             5.388903939827463911},  // face 16
            new [] {3.804819692245439833, 1.710424589852244509,
             5.899214794638635174},  // face 17
            new [] {3.664438879055192436, 1.570043776661997111,
             5.758833981448388027},  // face 18
            new [] {2.361378999196363184, 0.266983896803167583,
             4.455774101589558636},  // face 19
        };

        /** @brief Definition of which faces neighbor each other. */
        static readonly FaceOrientIJK[][] faceNeighbors = {
            new [] {
                // face 0
                new FaceOrientIJK { face = 0, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 4, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 1, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 5, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 1
                new FaceOrientIJK { face = 1, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 0, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 2, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 6, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 2
                new FaceOrientIJK { face = 2, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 1, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 3, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 7, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 3
                new FaceOrientIJK { face = 3, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 2, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 4, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 8, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 4
                new FaceOrientIJK { face = 4, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 3, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 0, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 9, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 5
                new FaceOrientIJK { face = 5, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},   // central face
                new FaceOrientIJK { face = 10, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},  // ij quadrant
                new FaceOrientIJK { face = 14, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},  // ki quadrant
                new FaceOrientIJK { face = 0, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}    // jk quadrant
            },
            new [] {
                // face 6
                new FaceOrientIJK { face = 6, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},   // central face
                new FaceOrientIJK { face = 11, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},  // ij quadrant
                new FaceOrientIJK { face = 10, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},  // ki quadrant
                new FaceOrientIJK { face = 1, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}    // jk quadrant
            },
            new [] {
                // face 7
                new FaceOrientIJK { face = 7, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},   // central face
                new FaceOrientIJK { face = 12, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},  // ij quadrant
                new FaceOrientIJK { face = 11, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},  // ki quadrant
                new FaceOrientIJK { face = 2, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}    // jk quadrant
            },
            new [] {
                // face 8
                new FaceOrientIJK { face = 8, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},   // central face
                new FaceOrientIJK { face = 13, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},  // ij quadrant
                new FaceOrientIJK { face = 12, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},  // ki quadrant
                new FaceOrientIJK { face = 3, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}    // jk quadrant
            },
            new [] {
                // face 9
                new FaceOrientIJK { face = 9, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},   // central face
                new FaceOrientIJK { face = 14, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},  // ij quadrant
                new FaceOrientIJK { face = 13, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},  // ki quadrant
                new FaceOrientIJK { face = 4, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}    // jk quadrant
            },
            new [] {
                // face 10
                new FaceOrientIJK { face = 10, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 5, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},   // ij quadrant
                new FaceOrientIJK { face = 6, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},   // ki quadrant
                new FaceOrientIJK { face = 15, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 11
                new FaceOrientIJK { face = 11, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 6, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},   // ij quadrant
                new FaceOrientIJK { face = 7, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},   // ki quadrant
                new FaceOrientIJK { face = 16, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 12
                new FaceOrientIJK { face = 12, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 7, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},   // ij quadrant
                new FaceOrientIJK { face = 8, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},   // ki quadrant
                new FaceOrientIJK { face = 17, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 13
                new FaceOrientIJK { face = 13, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 8, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},   // ij quadrant
                new FaceOrientIJK { face = 9, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},   // ki quadrant
                new FaceOrientIJK { face = 18, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 14
                new FaceOrientIJK { face = 14, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 9, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 3},   // ij quadrant
                new FaceOrientIJK { face = 5, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 3},   // ki quadrant
                new FaceOrientIJK { face = 19, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 15
                new FaceOrientIJK { face = 15, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 16, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 19, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 10, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 16
                new FaceOrientIJK { face = 16, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 17, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 15, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 11, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 17
                new FaceOrientIJK { face = 17, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 18, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 16, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 12, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 18
                new FaceOrientIJK { face = 18, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 19, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 17, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 13, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            },
            new [] {
                // face 19
                new FaceOrientIJK { face = 19, translate = new CoordIJK { i=0, j=0, k=0 }, ccwRot60 = 0},  // central face
                new FaceOrientIJK { face = 15, translate = new CoordIJK { i=2, j=0, k=2 }, ccwRot60 = 1},  // ij quadrant
                new FaceOrientIJK { face = 18, translate = new CoordIJK { i=2, j=2, k=0 }, ccwRot60 = 5},  // ki quadrant
                new FaceOrientIJK { face = 14, translate = new CoordIJK { i=0, j=2, k=2 }, ccwRot60 = 3}   // jk quadrant
            }};

        /** @brief direction from the origin face to the destination face, relative to
         * the origin face's coordinate system, or -1 if not adjacent.
         */
        static readonly int[][] adjacentFaceDir = {
            new [] {0,  KI, -1, -1, IJ, JK, -1, -1, -1, -1,
             -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},  // face 0
            new [] {IJ, 0,  KI, -1, -1, -1, JK, -1, -1, -1,
             -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},  // face 1
            new [] {-1, IJ, 0,  KI, -1, -1, -1, JK, -1, -1,
             -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},  // face 2
            new [] {-1, -1, IJ, 0,  KI, -1, -1, -1, JK, -1,
             -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},  // face 3
            new [] {KI, -1, -1, IJ, 0,  -1, -1, -1, -1, JK,
             -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},  // face 4
            new [] {JK, -1, -1, -1, -1, 0,  -1, -1, -1, -1,
             IJ, -1, -1, -1, KI, -1, -1, -1, -1, -1},  // face 5
            new [] {-1, JK, -1, -1, -1, -1, 0,  -1, -1, -1,
             KI, IJ, -1, -1, -1, -1, -1, -1, -1, -1},  // face 6
            new [] {-1, -1, JK, -1, -1, -1, -1, 0,  -1, -1,
             -1, KI, IJ, -1, -1, -1, -1, -1, -1, -1},  // face 7
            new [] {-1, -1, -1, JK, -1, -1, -1, -1, 0,  -1,
             -1, -1, KI, IJ, -1, -1, -1, -1, -1, -1},  // face 8
            new [] {-1, -1, -1, -1, JK, -1, -1, -1, -1, 0,
             -1, -1, -1, KI, IJ, -1, -1, -1, -1, -1},  // face 9
            new [] {-1, -1, -1, -1, -1, IJ, KI, -1, -1, -1,
             0,  -1, -1, -1, -1, JK, -1, -1, -1, -1},  // face 10
            new [] {-1, -1, -1, -1, -1, -1, IJ, KI, -1, -1,
             -1, 0,  -1, -1, -1, -1, JK, -1, -1, -1},  // face 11
            new [] {-1, -1, -1, -1, -1, -1, -1, IJ, KI, -1,
             -1, -1, 0,  -1, -1, -1, -1, JK, -1, -1},  // face 12
            new [] {-1, -1, -1, -1, -1, -1, -1, -1, IJ, KI,
             -1, -1, -1, 0,  -1, -1, -1, -1, JK, -1},  // face 13
            new [] {-1, -1, -1, -1, -1, KI, -1, -1, -1, IJ,
             -1, -1, -1, -1, 0,  -1, -1, -1, -1, JK},  // face 14
            new [] {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
             JK, -1, -1, -1, -1, 0,  IJ, -1, -1, KI},  // face 15
            new [] {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
             -1, JK, -1, -1, -1, KI, 0,  IJ, -1, -1},  // face 16
            new [] {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
             -1, -1, JK, -1, -1, -1, KI, 0,  IJ, -1},  // face 17
            new [] {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
             -1, -1, -1, JK, -1, -1, -1, KI, 0,  IJ},  // face 18
            new [] {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
             -1, -1, -1, -1, JK, IJ, -1, -1, KI, 0}  // face 19
        };

        /** @brief overage distance table */
        static readonly int[] maxDimByCIIres = {
            2,        // res  0
            -1,       // res  1
            14,       // res  2
            -1,       // res  3
            98,       // res  4
            -1,       // res  5
            686,      // res  6
            -1,       // res  7
            4802,     // res  8
            -1,       // res  9
            33614,    // res 10
            -1,       // res 11
            235298,   // res 12
            -1,       // res 13
            1647086,  // res 14
            -1,       // res 15
            11529602  // res 16
        };

        /** @brief unit scale distance table */
        static readonly int[] unitScaleByCIIres = {
            1,       // res  0
            -1,      // res  1
            7,       // res  2
            -1,      // res  3
            49,      // res  4
            -1,      // res  5
            343,     // res  6
            -1,      // res  7
            2401,    // res  8
            -1,      // res  9
            16807,   // res 10
            -1,      // res 11
            117649,  // res 12
            -1,      // res 13
            823543,  // res 14
            -1,      // res 15
            5764801  // res 16
        };
        
        /**
         * Encodes a coordinate on the sphere to the FaceIJK address of the containing
         * cell at the specified resolution.
         *
         * @param g The spherical coordinates to encode.
         * @param res The desired H3 resolution for the encoding.
         * @param h The FaceIJK address of the containing cell at resolution res.
         */
        public static void _geoToFaceIjk(in GeoCoord g, int res, ref FaceIJK h) {
            // first convert to hex2d
            Vec2d v = new Vec2d();
            _geoToHex2d(g, res, ref h.face, ref v);

            // then convert to ijk+
            CoordIJK._hex2dToCoordIJK(v, ref h.coord);
        }

        /**
         * Encodes a coordinate on the sphere to the corresponding icosahedral face and
         * containing 2D hex coordinates relative to that face center.
         *
         * @param g The spherical coordinates to encode.
         * @param res The desired H3 resolution for the encoding.
         * @param face The icosahedral face containing the spherical coordinates.
         * @param v The 2D hex coordinates of the cell containing the point.
         */
        public static void _geoToHex2d(in GeoCoord g, int res, ref int face, ref Vec2d v) {
            Vec3d v3d = new Vec3d();
            Vec3d._geoToVec3d(g, ref v3d);

            // determine the icosahedron face
            face = 0;
            double sqd = Vec3d._pointSquareDist(faceCenterPoint[0], v3d);
            for (int f = 1; f < Constants.NUM_ICOSA_FACES; f++) {
                double sqdT = Vec3d._pointSquareDist(faceCenterPoint[f], v3d);
                if (sqdT < sqd) {
                    face = f;
                    sqd = sqdT;
                }
            }

            // cos(r) = 1 - 2 * sin^2(r/2) = 1 - 2 * (sqd / 4) = 1 - sqd/2
            double r = Math.Acos(1 - sqd / 2);

            if (r < Constants.EPSILON) {
                v.x = v.y = 0.0;
                return;
            }

            // now have face and r, now find CCW theta from CII i-axis
            double theta =
                GeoCoord._posAngleRads(faceAxesAzRadsCII[face][0] -
                              GeoCoord._posAngleRads(GeoCoord._geoAzimuthRads(faceCenterGeo[face], g)));

            // adjust theta for Class III (odd resolutions)
            if (H3Index.isResClassIII(res)) theta = GeoCoord._posAngleRads(theta - Constants.M_AP7_ROT_RADS);

            // perform gnomonic scaling of r
            r = Math.Tan(r);

            // scale for current resolution length u
            r /= Constants.RES0_U_GNOMONIC;
            for (int i = 0; i < res; i++) r *= M_SQRT7;

            // we now have (r, theta) in hex2d with theta ccw from x-axes

            // convert to local x,y
            v.x = r * Math.Cos(theta);
            v.y = r * Math.Sin(theta);
        }

        /**
         * Determines the center point in spherical coordinates of a cell given by 2D
         * hex coordinates on a particular icosahedral face.
         *
         * @param v The 2D hex coordinates of the cell.
         * @param face The icosahedral face upon which the 2D hex coordinate system is
         *             centered.
         * @param res The H3 resolution of the cell.
         * @param substrate Indicates whether or not this grid is actually a substrate
         *        grid relative to the specified resolution.
         * @param g The spherical coordinates of the cell center point.
         */
        public static void _hex2dToGeo(in Vec2d v, int face, int res, bool substrate,
                         ref GeoCoord g) {
            // calculate (r, theta) in hex2d
            double r = Vec2d._v2dMag(v);

            if (r < Constants.EPSILON) {
                g = faceCenterGeo[face];
                return;
            }

            double theta = Math.Atan2(v.y, v.x);

            // scale for current resolution length u
            for (int i = 0; i < res; i++) r /= M_SQRT7;

            // scale accordingly if this is a substrate grid
            if (substrate) {
                r /= 3.0;
                if (H3Index.isResClassIII(res)) r /= M_SQRT7;
            }

            r *= Constants.RES0_U_GNOMONIC;

            // perform inverse gnomonic scaling of r
            r = Math.Atan(r);

            // adjust theta for Class III
            // if a substrate grid, then it's already been adjusted for Class III
            if (!substrate && H3Index.isResClassIII(res))
                theta = GeoCoord._posAngleRads(theta + Constants.M_AP7_ROT_RADS);

            // find theta as an azimuth
            theta = GeoCoord._posAngleRads(faceAxesAzRadsCII[face][0] - theta);

            // now find the point at (r,theta) from the face center
            GeoCoord._geoAzDistanceRads(faceCenterGeo[face], theta, r, ref g);
        }

        /**
         * Determines the center point in spherical coordinates of a cell given by
         * a FaceIJK address at a specified resolution.
         *
         * @param h The FaceIJK address of the cell.
         * @param res The H3 resolution of the cell.
         * @param g The spherical coordinates of the cell center point.
         */
        public static void _faceIjkToGeo(in FaceIJK h, int res, ref GeoCoord g) {
            Vec2d v = new Vec2d();
            CoordIJK._ijkToHex2d(h.coord, ref v);
            _hex2dToGeo(v, h.face, res, false, ref g);
        }

        /**
         * Generates the cell boundary in spherical coordinates for a pentagonal cell
         * given by a FaceIJK address at a specified resolution.
         *
         * @param h The FaceIJK address of the pentagonal cell.
         * @param res The H3 resolution of the cell.
         * @param g The spherical coordinates of the cell boundary.
         */
        public static void _faceIjkPentToGeoBoundary(in FaceIJK h, int res, ref GeoBoundary g, bool addEdgeVerts) {
            int adjRes = res;
            FaceIJK centerIJK = h;
            FaceIJK[] fijkVerts = new FaceIJK[Constants.NUM_PENT_VERTS];
            _faceIjkPentToVerts(ref centerIJK, ref adjRes, ref fijkVerts);

            // convert each vertex to lat/lon
            // adjust the face of each vertex as appropriate and introduce
            // edge-crossing vertices as needed
            g.numVerts = 0;
            FaceIJK lastFijk = new FaceIJK();
            for (int vert = 0; vert < Constants.NUM_PENT_VERTS + 1; vert++) {
                int v = vert % Constants.NUM_PENT_VERTS;

                FaceIJK fijk = fijkVerts[v];

                _adjustPentVertOverage(ref fijk, adjRes);

                // all Class III pentagon edges cross icosa edges
                // note that Class II pentagons have vertices on the edge,
                // not edge intersections
                if (H3Index.isResClassIII(res) && vert > 0 && addEdgeVerts) {
                    // find hex2d of the two vertexes on the last face

                    FaceIJK tmpFijk = fijk;

                    Vec2d orig2d0 = new Vec2d();
                    CoordIJK._ijkToHex2d(lastFijk.coord, ref orig2d0);

                    int currentToLastDir = adjacentFaceDir[tmpFijk.face][lastFijk.face];

                    ref FaceOrientIJK fijkOrient =
                        ref faceNeighbors[tmpFijk.face][currentToLastDir];

                    tmpFijk.face = fijkOrient.face;
                    ref CoordIJK ijk = ref tmpFijk.coord;

                    // rotate and translate for adjacent face
                    for (int i = 0; i < fijkOrient.ccwRot60; i++) CoordIJK._ijkRotate60ccw(ref ijk);

                    CoordIJK transVec = fijkOrient.translate;
                    CoordIJK._ijkScale(ref transVec, unitScaleByCIIres[adjRes] * 3);
                    CoordIJK._ijkAdd(ijk, transVec, ref ijk);
                    CoordIJK._ijkNormalize(ref ijk);

                    Vec2d orig2d1 = new Vec2d();
                    CoordIJK._ijkToHex2d(ijk, ref orig2d1);

                    // find the appropriate icosa face edge vertexes
                    int maxDim = maxDimByCIIres[adjRes];
                    Vec2d v0 = new Vec2d {x=3.0 * maxDim, y=0.0};
                    Vec2d v1 = new Vec2d {x=-1.5 * maxDim, y=3.0 * Constants.M_SQRT3_2 * maxDim};
                    Vec2d v2 = new Vec2d {x=-1.5 * maxDim, y=-3.0 * Constants.M_SQRT3_2 * maxDim};

                    Vec2d edge0;
                    Vec2d edge1;
                    switch (adjacentFaceDir[tmpFijk.face][fijk.face]) {
                        case IJ:
                            edge0 = v0;
                            edge1 = v1;
                            break;
                        case JK:
                            edge0 = v1;
                            edge1 = v2;
                            break;
                        case KI:
                        default:
                            //assert(adjacentFaceDir[tmpFijk.face][fijk.face] == KI);
                            edge0 = v2;
                            edge1 = v0;
                            break;
                    }

                    // find the intersection and add the lat/lon point to the result
                    Vec2d inter = new Vec2d();
                    Vec2d._v2dIntersect(orig2d0, orig2d1, edge0, edge1, ref inter);
                    _hex2dToGeo(inter, tmpFijk.face, adjRes, true,
                                ref g.verts[g.numVerts]);
                    g.numVerts++;
                }

                // convert vertex to lat/lon and add to the result
                // vert == NUM_PENT_VERTS is only used to test for possible intersection
                // on last edge
                if (vert < Constants.NUM_PENT_VERTS) {
                    Vec2d vec = new Vec2d();
                    CoordIJK._ijkToHex2d(fijk.coord, ref vec);
                    _hex2dToGeo(vec, fijk.face, adjRes, true, ref g.verts[g.numVerts]);
                    g.numVerts++;
                }

                lastFijk = fijk;
            }
        }

        /**
         * Get the vertices of a pentagon cell as substrate FaceIJK addresses
         *
         * @param fijk The FaceIJK address of the cell.
         * @param res The H3 resolution of the cell. This may be adjusted if
         *            necessary for the substrate grid resolution.
         * @param fijkVerts Output array for the vertices
         */
        public static void _faceIjkPentToVerts(ref FaceIJK fijk, ref int res, ref FaceIJK[] fijkVerts)
        {
            CoordIJK[] verts;

            if (!H3Index.isResClassIII(res))
            {
                // the vertexes of an origin-centered pentagon in a Class II resolution on a
                // substrate grid with aperture sequence 33r. The aperture 3 gets us the
                // vertices, and the 3r gets us back to Class II.
                // vertices listed ccw from the i-axes
                verts = new []
                {
                    new CoordIJK {i = 2, j = 1, k = 0}, // 0
                    new CoordIJK {i = 1, j = 2, k = 0}, // 1
                    new CoordIJK {i = 0, j = 2, k = 1}, // 2
                    new CoordIJK {i = 0, j = 1, k = 2}, // 3
                    new CoordIJK {i = 1, j = 0, k = 2}, // 4
                };
            }
            else
            {
                // the vertexes of an origin-centered pentagon in a Class III resolution on
                // a substrate grid with aperture sequence 33r7r. The aperture 3 gets us the
                // vertices, and the 3r7r gets us to Class II. vertices listed ccw from the
                // i-axes
                verts = new []
                {
                    new CoordIJK {i = 5, j = 4, k = 0}, // 0
                    new CoordIJK {i = 1, j = 5, k = 0}, // 1
                    new CoordIJK {i = 0, j = 5, k = 4}, // 2
                    new CoordIJK {i = 0, j = 1, k = 5}, // 3
                    new CoordIJK {i = 4, j = 0, k = 5}, // 4
                };
            }
            
            // adjust the center point to be in an aperture 33r substrate grid
            // these should be composed for speed
            CoordIJK._downAp3(ref fijk.coord);
            CoordIJK._downAp3r(ref fijk.coord);

            // if res is Class III we need to add a cw aperture 7 to get to
            // icosahedral Class II
            if (H3Index.isResClassIII(res)) {
                CoordIJK._downAp7r(ref fijk.coord);
                res += 1;
            }

            // The center point is now in the same substrate grid as the origin
            // cell vertices. Add the center point substate coordinates
            // to each vertex to translate the vertices to that cell.
            for (int v = 0; v < Constants.NUM_PENT_VERTS; v++) {
                fijkVerts[v].face = fijk.face;
                CoordIJK._ijkAdd(fijk.coord, verts[v], ref fijkVerts[v].coord);
                CoordIJK._ijkNormalize(ref fijkVerts[v].coord);
            }
        }

        /**
         * Generates the cell boundary in spherical coordinates for a cell given by a
         * FaceIJK address at a specified resolution.
         *
         * @param h The FaceIJK address of the cell.
         * @param res The H3 resolution of the cell.
         * @param isPentagon Whether or not the cell is a pentagon.
         * @param g The spherical coordinates of the cell boundary.
         */
        public static void _faceIjkToGeoBoundary(in FaceIJK h, int res, bool isPentagon,
                                   ref GeoBoundary g, bool addEdgeVerts) {
            if (isPentagon) {
                _faceIjkPentToGeoBoundary(h, res, ref g, addEdgeVerts);
                return;
            }

            int adjRes = res;
            FaceIJK centerIJK = h;
            FaceIJK[] fijkVerts = new FaceIJK[Constants.NUM_HEX_VERTS];
            _faceIjkToVerts(ref centerIJK, ref adjRes, ref fijkVerts);

            // convert each vertex to lat/lon
            // adjust the face of each vertex as appropriate and introduce
            // edge-crossing vertices as needed
            g.numVerts = 0;
            int lastFace = -1;
            Overage lastOverage = Overage.NO_OVERAGE;
            for (int vert = 0; vert < Constants.NUM_HEX_VERTS + 1; vert++) {
                int v = vert % Constants.NUM_HEX_VERTS;

                FaceIJK fijk = fijkVerts[v];

                bool pentLeading4 = false;
                Overage overage = _adjustOverageClassII(ref fijk, adjRes, pentLeading4, true);

                /*
                Check for edge-crossing. Each face of the underlying icosahedron is a
                different projection plane. So if an edge of the hexagon crosses an
                icosahedron edge, an additional vertex must be introduced at that
                intersection point. Then each half of the cell edge can be projected
                to geographic coordinates using the appropriate icosahedron face
                projection. Note that Class II cell edges have vertices on the face
                edge, with no edge line intersections.
                */
                if (H3Index.isResClassIII(res) && vert > 0 && fijk.face != lastFace &&
                    lastOverage != Overage.FACE_EDGE && addEdgeVerts) {
                    // find hex2d of the two vertexes on original face
                    int lastV = (v + 5) % Constants.NUM_HEX_VERTS;
                    Vec2d orig2d0 = new Vec2d();
                    CoordIJK._ijkToHex2d(fijkVerts[lastV].coord, ref orig2d0);

                    Vec2d orig2d1 = new Vec2d();
                    CoordIJK._ijkToHex2d(fijkVerts[v].coord, ref orig2d1);

                    // find the appropriate icosa face edge vertexes
                    int maxDim = maxDimByCIIres[adjRes];
                    Vec2d v0 = new Vec2d { x = 3.0 * maxDim, y = 0.0 };
                    Vec2d v1 = new Vec2d { x = -1.5 * maxDim, y = 3.0 * Constants.M_SQRT3_2 * maxDim };
                    Vec2d v2 = new Vec2d { x = -1.5 * maxDim, y = -3.0 * Constants.M_SQRT3_2 * maxDim };

                    int face2 = ((lastFace == centerIJK.face) ? fijk.face : lastFace);
                    Vec2d edge0;
                    Vec2d edge1;
                    switch (adjacentFaceDir[centerIJK.face][face2]) {
                        case IJ:
                            edge0 = v0;
                            edge1 = v1;
                            break;
                        case JK:
                            edge0 = v1;
                            edge1 = v2;
                            break;
                        case KI:
                        default:
                            //assert(adjacentFaceDir[centerIJK.face][face2] == KI);
                            edge0 = v2;
                            edge1 = v0;
                            break;
                    }

                    // find the intersection and add the lat/lon point to the result
                    Vec2d inter = new Vec2d();
                    Vec2d._v2dIntersect(orig2d0, orig2d1, edge0, edge1, ref inter);
                    /*
                    If a point of intersection occurs at a hexagon vertex, then each
                    adjacent hexagon edge will lie completely on a single icosahedron
                    face, and no additional vertex is required.
                    */
                    bool isIntersectionAtVertex =
                        Vec2d._v2dEquals(orig2d0, inter) || Vec2d._v2dEquals(orig2d1, inter);
                    if (!isIntersectionAtVertex) {
                        _hex2dToGeo(inter, centerIJK.face, adjRes, true,
                                    ref g.verts[g.numVerts]);
                        g.numVerts++;
                    }
                }

                // convert vertex to lat/lon and add to the result
                // vert == NUM_HEX_VERTS is only used to test for possible intersection
                // on last edge
                if (vert < Constants.NUM_HEX_VERTS) {
                    Vec2d vec = new Vec2d();
                    CoordIJK._ijkToHex2d(fijk.coord, ref vec);
                    _hex2dToGeo(vec, fijk.face, adjRes, true, ref g.verts[g.numVerts]);
                    g.numVerts++;
                }

                lastFace = fijk.face;
                lastOverage = overage;
            }
        }

        /**
         * Get the vertices of a cell as substrate FaceIJK addresses
         *
         * @param fijk The FaceIJK address of the cell.
         * @param res The H3 resolution of the cell. This may be adjusted if
         *            necessary for the substrate grid resolution.
         * @param fijkVerts Output array for the vertices
         */
        public static void _faceIjkToVerts(ref FaceIJK fijk, ref int res, ref FaceIJK[] fijkVerts)
        {
            CoordIJK[] verts;
            
            if (!H3Index.isResClassIII(res))
            {
                // the vertexes of an origin-centered cell in a Class II resolution on a
                // substrate grid with aperture sequence 33r. The aperture 3 gets us the
                // vertices, and the 3r gets us back to Class II.
                // vertices listed ccw from the i-axes
                verts = new[]
                {
                    new CoordIJK {i = 2, j = 1, k = 0}, // 0
                    new CoordIJK {i = 1, j = 2, k = 0}, // 1
                    new CoordIJK {i = 0, j = 2, k = 1}, // 2
                    new CoordIJK {i = 0, j = 1, k = 2}, // 3
                    new CoordIJK {i = 1, j = 0, k = 2}, // 4
                    new CoordIJK {i = 2, j = 0, k = 1} // 5
                };
            } else {
                // the vertexes of an origin-centered cell in a Class III resolution on a
                // substrate grid with aperture sequence 33r7r. The aperture 3 gets us the
                // vertices, and the 3r7r gets us to Class II.
                // vertices listed ccw from the i-axes
                verts = new[]
                {
                    new CoordIJK {i = 5, j = 4, k = 0}, // 0
                    new CoordIJK {i = 1, j = 5, k = 0}, // 1
                    new CoordIJK {i = 0, j = 5, k = 4}, // 2
                    new CoordIJK {i = 0, j = 1, k = 5}, // 3
                    new CoordIJK {i = 4, j = 0, k = 5}, // 4
                    new CoordIJK {i = 5, j = 0, k = 1} // 5
                };
            }
            
            // adjust the center point to be in an aperture 33r substrate grid
            // these should be composed for speed
            CoordIJK._downAp3(ref fijk.coord);
            CoordIJK._downAp3r(ref fijk.coord);

            // if res is Class III we need to add a cw aperture 7 to get to
            // icosahedral Class II
            if (H3Index.isResClassIII(res)) {
                CoordIJK._downAp7r(ref fijk.coord);
                res += 1;
            }

            // The center point is now in the same substrate grid as the origin
            // cell vertices. Add the center point substate coordinates
            // to each vertex to translate the vertices to that cell.
            for (int v = 0; v < Constants.NUM_HEX_VERTS; v++) {
                fijkVerts[v].face = fijk.face;
                CoordIJK._ijkAdd(fijk.coord, verts[v], ref fijkVerts[v].coord);
                CoordIJK._ijkNormalize(ref fijkVerts[v].coord);
            }
        }

        /**
         * Adjusts a FaceIJK address in place so that the resulting cell address is
         * relative to the correct icosahedral face.
         *
         * @param fijk The FaceIJK address of the cell.
         * @param res The H3 resolution of the cell.
         * @param pentLeading4 Whether or not the cell is a pentagon with a leading
         *        digit 4.
         * @param substrate Whether or not the cell is in a substrate grid.
         * @return 0 if on original face (no overage); 1 if on face edge (only occurs
         *         on substrate grids); 2 if overage on new face interior
         */
        public static Overage _adjustOverageClassII(ref FaceIJK fijk, int res, bool pentLeading4,
                                      bool substrate) {
            Overage overage = Overage.NO_OVERAGE;

            ref CoordIJK ijk = ref fijk.coord;

            // get the maximum dimension value; scale if a substrate grid
            int maxDim = maxDimByCIIres[res];
            if (substrate) maxDim *= 3;

            // check for overage
            if (substrate && ijk.i + ijk.j + ijk.k == maxDim)  // on edge
                overage = Overage.FACE_EDGE;
            else if (ijk.i + ijk.j + ijk.k > maxDim)  // overage
            {
                overage = Overage.NEW_FACE;

                FaceOrientIJK fijkOrient;
                if (ijk.k > 0) {
                    if (ijk.j > 0)  // jk "quadrant"
                        fijkOrient = faceNeighbors[fijk.face][JK];
                    else  // ik "quadrant"
                    {
                        fijkOrient = faceNeighbors[fijk.face][KI];

                        // adjust for the pentagonal missing sequence
                        if (pentLeading4) {
                            // translate origin to center of pentagon
                            CoordIJK origin = new CoordIJK();
                            CoordIJK._setIJK(ref origin, maxDim, 0, 0);
                            CoordIJK tmp = new CoordIJK();
                            CoordIJK._ijkSub(ijk, origin, ref tmp);
                            // rotate to adjust for the missing sequence
                            CoordIJK._ijkRotate60cw(ref tmp);
                            // translate the origin back to the center of the triangle
                            CoordIJK._ijkAdd(tmp, origin, ref ijk);
                        }
                    }
                } else  // ij "quadrant"
                    fijkOrient = faceNeighbors[fijk.face][IJ];

                fijk.face = fijkOrient.face;

                // rotate and translate for adjacent face
                for (int i = 0; i < fijkOrient.ccwRot60; i++) CoordIJK._ijkRotate60ccw(ref ijk);

                CoordIJK transVec = fijkOrient.translate;
                int unitScale = unitScaleByCIIres[res];
                if (substrate) unitScale *= 3;
                CoordIJK._ijkScale(ref transVec, unitScale);
                CoordIJK._ijkAdd(ijk, transVec, ref ijk);
                CoordIJK._ijkNormalize(ref ijk);

                // overage points on pentagon boundaries can end up on edges
                if (substrate && (ijk.i + ijk.j + ijk.k) == maxDim)  // on edge
                    overage = Overage.FACE_EDGE;
            }

            return overage;
        }

        /**
         * Adjusts a FaceIJK address for a pentagon vertex in a substrate grid in
         * place so that the resulting cell address is relative to the correct
         * icosahedral face.
         *
         * @param fijk The FaceIJK address of the cell.
         * @param res The H3 resolution of the cell.
         */
        public static Overage _adjustPentVertOverage(ref FaceIJK fijk, int res) {
            bool pentLeading4 = false;
            Overage overage;
            do {
                overage = _adjustOverageClassII(ref fijk, res, pentLeading4, true);
            } while (overage == Overage.NEW_FACE);
            return overage;
        }
        
    }

    /** @struct FaceOrientIJK
     * @brief Information to transform into an adjacent face IJK system
     */
    public struct FaceOrientIJK {
        public int face;            ///< face number
        public CoordIJK translate;  ///< res 0 translation relative to primary face
        public int ccwRot60;        ///< number of 60 degree ccw rotations relative to primary
                                    /// face
    }
    
    /** Digit representing overage type */
    public enum Overage {
        /** No overage (on original face) */
        NO_OVERAGE = 0,
        /** On face edge (only occurs on substrate grids) */
        FACE_EDGE = 1,
        /** Overage on new face interior */
        NEW_FACE = 2
    };
}