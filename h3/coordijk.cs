using System;

namespace h3
{
    /** @struct CoordIJK
     * @brief IJK hexagon coordinates
     *
     * Each axis is spaced 120 degrees apart.
     */
    public struct CoordIJK {
        public int i;  ///< i component
        public int j;  ///< j component
        public int k;  ///< k component

        /** @brief CoordIJK unit vectors corresponding to the 7 H3 digits.
        */
        static readonly CoordIJK[] UNIT_VECS = {
            new CoordIJK { i = 0, j = 0, k = 0 },  // direction 0
            new CoordIJK { i = 0, j = 0, k = 1 },  // direction 1
            new CoordIJK { i = 0, j = 1, k = 0 },  // direction 2
            new CoordIJK { i = 0, j = 1, k = 1 },  // direction 3
            new CoordIJK { i = 1, j = 0, k = 0 },  // direction 4
            new CoordIJK { i = 1, j = 0, k = 1 },  // direction 5
            new CoordIJK { i = 1, j = 1, k = 0 }   // direction 6
        };
        
        /**
         * Sets an IJK coordinate to the specified component values.
         *
         * @param ijk The IJK coordinate to set.
         * @param i The desired i component value.
         * @param j The desired j component value.
         * @param k The desired k component value.
         */
        public static void _setIJK(ref CoordIJK ijk, int i, int j, int k)
        {
            ijk.i = i;
            ijk.j = j;
            ijk.k = k;
        }
        
        /**
         * Determine the containing hex in ijk+ coordinates for a 2D cartesian
         * coordinate vector (from DGGRID).
         *
         * @param v The 2D cartesian coordinate vector.
         * @param h The ijk+ coordinates of the containing hex.
         */
        public static void _hex2dToCoordIJK(in Vec2d v, ref CoordIJK h)
        {
            double a1, a2;
            double x1, x2;
            int m1, m2;
            double r1, r2;

            // quantize into the ij system and then normalize
            h.k = 0;

            a1 = Math.Abs(v.x);
            a2 = Math.Abs(v.y);

            // first do a reverse conversion
            x2 = a2 / Constants.M_SIN60;
            x1 = a1 + x2 / 2.0;

            // check if we have the center of a hex
            m1 = (int) x1;
            m2 = (int) x2;

            // otherwise round correctly
            r1 = x1 - m1;
            r2 = x2 - m2;

            if (r1 < 0.5) {
                if (r1 < 1.0 / 3.0) {
                    if (r2 < (1.0 + r1) / 2.0) {
                        h.i = m1;
                        h.j = m2;
                    } else {
                        h.i = m1;
                        h.j = m2 + 1;
                    }
                } else {
                    if (r2 < (1.0 - r1)) {
                        h.j = m2;
                    } else {
                        h.j = m2 + 1;
                    }

                    if ((1.0 - r1) <= r2 && r2 < (2.0 * r1)) {
                        h.i = m1 + 1;
                    } else {
                        h.i = m1;
                    }
                }
            } else {
                if (r1 < 2.0 / 3.0) {
                    if (r2 < (1.0 - r1)) {
                        h.j = m2;
                    } else {
                        h.j = m2 + 1;
                    }

                    if ((2.0 * r1 - 1.0) < r2 && r2 < (1.0 - r1)) {
                        h.i = m1;
                    } else {
                        h.i = m1 + 1;
                    }
                } else {
                    if (r2 < (r1 / 2.0)) {
                        h.i = m1 + 1;
                        h.j = m2;
                    } else {
                        h.i = m1 + 1;
                        h.j = m2 + 1;
                    }
                }
            }

            // now fold across the axes if necessary

            if (v.x < 0.0) {
                if ((h.j % 2) == 0)  // even
                {
                    long axisi = h.j / 2;
                    long diff = h.i - axisi;
                    h.i = (int) (h.i - 2.0 * diff);
                } else {
                    long axisi = (h.j + 1) / 2;
                    long diff = h.i - axisi;
                    h.i = (int) (h.i - (2.0 * diff + 1));
                }
            }

            if (v.y < 0.0) {
                h.i = h.i - (2 * h.j + 1) / 2;
                h.j = -1 * h.j;
            }

            _ijkNormalize(ref h);
        }
        
        /**
         * Find the center point in 2D cartesian coordinates of a hex.
         *
         * @param h The ijk coordinates of the hex.
         * @param v The 2D cartesian coordinates of the hex center point.
         */
        public static void _ijkToHex2d(in CoordIJK h, ref Vec2d v)
        {
            int i = h.i - h.k;
            int j = h.j - h.k;

            v.x = i - 0.5 * j;
            v.y = j * Constants.M_SQRT3_2;
        }
        
        /**
         * Returns whether or not two ijk coordinates contain exactly the same
         * component values.
         *
         * @param c1 The first set of ijk coordinates.
         * @param c2 The second set of ijk coordinates.
         * @return 1 if the two addresses match, 0 if they do not.
         */
        public static bool _ijkMatches(in CoordIJK c1, in CoordIJK c2)
        {
            return (c1.i == c2.i && c1.j == c2.j && c1.k == c2.k);
        }
        
        /**
         * Add two ijk coordinates.
         *
         * @param h1 The first set of ijk coordinates.
         * @param h2 The second set of ijk coordinates.
         * @param sum The sum of the two sets of ijk coordinates.
         */
        public static void _ijkAdd(in CoordIJK h1, in CoordIJK h2, ref CoordIJK sum)
        {
            sum.i = h1.i + h2.i;
            sum.j = h1.j + h2.j;
            sum.k = h1.k + h2.k;
        }

        /**
         * Subtract two ijk coordinates.
         *
         * @param h1 The first set of ijk coordinates.
         * @param h2 The second set of ijk coordinates.
         * @param diff The difference of the two sets of ijk coordinates (h1 - h2).
         */
        public static void _ijkSub(in CoordIJK h1, in CoordIJK h2, ref CoordIJK diff)
        {
            diff.i = h1.i - h2.i;
            diff.j = h1.j - h2.j;
            diff.k = h1.k - h2.k;
        }

        /**
         * Uniformly scale ijk coordinates by a scalar. Works in place.
         *
         * @param c The ijk coordinates to scale.
         * @param factor The scaling factor.
         */
        public static void _ijkScale(ref CoordIJK c, int factor)
        {
            c.i *= factor;
            c.j *= factor;
            c.k *= factor;
        }

        /**
         * Normalizes ijk coordinates by setting the components to the smallest possible
         * values. Works in place.
         *
         * @param c The ijk coordinates to normalize.
         */
        public static void _ijkNormalize(ref CoordIJK c)
        {
            // remove any negative values
            if (c.i < 0) {
                c.j -= c.i;
                c.k -= c.i;
                c.i = 0;
            }

            if (c.j < 0) {
                c.i -= c.j;
                c.k -= c.j;
                c.j = 0;
            }

            if (c.k < 0) {
                c.i -= c.k;
                c.j -= c.k;
                c.k = 0;
            }

            // remove the min value if needed
            int min = c.i;
            if (c.j < min) min = c.j;
            if (c.k < min) min = c.k;
            if (min > 0) {
                c.i -= min;
                c.j -= min;
                c.k -= min;
            }
        }
        
        /**
         * Determines the H3 digit corresponding to a unit vector in ijk coordinates.
         *
         * @param ijk The ijk coordinates; must be a unit vector.
         * @return The H3 digit (0-6) corresponding to the ijk unit vector, or
         * INVALID_DIGIT on failure.
         */
        public static Direction _unitIjkToDigit(in CoordIJK ijk)
        {
            CoordIJK c = ijk;
            _ijkNormalize(ref c);

            Direction digit = Direction.INVALID_DIGIT;
            for (Direction i = Direction.CENTER_DIGIT; i < Direction.NUM_DIGITS; i++) {
                if (_ijkMatches(c, UNIT_VECS[(int) i])) {
                    digit = i;
                    break;
                }
            }

            return digit;
        }

        /**
         * Find the normalized ijk coordinates of the indexing parent of a cell in a
         * counter-clockwise aperture 7 grid. Works in place.
         *
         * @param ijk The ijk coordinates.
         */
        public static void _upAp7(ref CoordIJK ijk)
        {
            // convert to CoordIJ
            int i = ijk.i - ijk.k;
            int j = ijk.j - ijk.k;

            //TODO: is round the correct call here? lroundl
            ijk.i = (int) Math.Round((3 * i - j) / 7.0);
            ijk.j = (int) Math.Round((i + 2 * j) / 7.0);
            ijk.k = 0;
            _ijkNormalize(ref ijk);
        }

        /**
         * Find the normalized ijk coordinates of the indexing parent of a cell in a
         * clockwise aperture 7 grid. Works in place.
         *
         * @param ijk The ijk coordinates.
         */
        public static void _upAp7r(ref CoordIJK ijk)
        {
            // convert to CoordIJ
            int i = ijk.i - ijk.k;
            int j = ijk.j - ijk.k;

            //TODO: is round the correct call here? lroundl
            ijk.i = (int) Math.Round((2 * i + j) / 7.0);
            ijk.j = (int) Math.Round((3 * j - i) / 7.0);
            ijk.k = 0;
            _ijkNormalize(ref ijk);
        }

        /**
         * Find the normalized ijk coordinates of the hex centered on the indicated
         * hex at the next finer aperture 7 counter-clockwise resolution. Works in
         * place.
         *
         * @param ijk The ijk coordinates.
         */
        public static void _downAp7(ref CoordIJK ijk)
        {
            // res r unit vectors in res r+1
            CoordIJK iVec = new CoordIJK { i=3, j=0, k=1 };
            CoordIJK jVec = new CoordIJK { i=1, j=3, k=0 };
            CoordIJK kVec = new CoordIJK { i=0, j=1, k=3 };

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /**
         * Find the normalized ijk coordinates of the hex centered on the indicated
         * hex at the next finer aperture 7 clockwise resolution. Works in place.
         *
         * @param ijk The ijk coordinates.
         */
        public static void _downAp7r(ref CoordIJK ijk)
        {
            // res r unit vectors in res r+1
            CoordIJK iVec = new CoordIJK { i=3, j=1, k=0 };
            CoordIJK jVec = new CoordIJK { i=0, j=3, k=1 };
            CoordIJK kVec = new CoordIJK { i=1, j=0, k=3 };

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /**
         * Find the normalized ijk coordinates of the hex in the specified digit
         * direction from the specified ijk coordinates. Works in place.
         *
         * @param ijk The ijk coordinates.
         * @param digit The digit direction from the original ijk coordinates.
         */
        public static void _neighbor(ref CoordIJK ijk, Direction digit)
        {
            if (digit > Direction.CENTER_DIGIT && digit < Direction.NUM_DIGITS) {
                _ijkAdd(ijk, UNIT_VECS[(int) digit], ref ijk);
                _ijkNormalize(ref ijk);
            }
        }
        
        /**
         * Rotates ijk coordinates 60 degrees counter-clockwise. Works in place.
         *
         * @param ijk The ijk coordinates.
         */
        public static void _ijkRotate60ccw(ref CoordIJK ijk)
        {
            // unit vector rotations
            CoordIJK iVec = new CoordIJK { i=1, j=1, k=0 };
            CoordIJK jVec = new CoordIJK { i=0, j=1, k=1 };
            CoordIJK kVec = new CoordIJK { i=1, j=0, k=1 };

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }
        
        /**
         * Rotates ijk coordinates 60 degrees clockwise. Works in place.
         *
         * @param ijk The ijk coordinates.
         */
        public static void _ijkRotate60cw(ref CoordIJK ijk)
        {
            // unit vector rotations
            CoordIJK iVec = new CoordIJK { i=1, j=0, k=1 };
            CoordIJK jVec = new CoordIJK { i=1, j=1, k=0 };
            CoordIJK kVec = new CoordIJK { i=0, j=1, k=1 };

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }
        
        /**
         * Rotates indexing digit 60 degrees counter-clockwise. Returns result.
         *
         * @param digit Indexing digit (between 1 and 6 inclusive)
         */
        public static Direction _rotate60ccw(Direction digit)
        {
            switch (digit) {
                case Direction.K_AXES_DIGIT:
                    return Direction.IK_AXES_DIGIT;
                case Direction.IK_AXES_DIGIT:
                    return Direction.I_AXES_DIGIT;
                case Direction.I_AXES_DIGIT:
                    return Direction.IJ_AXES_DIGIT;
                case Direction.IJ_AXES_DIGIT:
                    return Direction.J_AXES_DIGIT;
                case Direction.J_AXES_DIGIT:
                    return Direction.JK_AXES_DIGIT;
                case Direction.JK_AXES_DIGIT:
                    return Direction.K_AXES_DIGIT;
                default:
                    return digit;
            }
        }

        /**
         * Rotates indexing digit 60 degrees clockwise. Returns result.
         *
         * @param digit Indexing digit (between 1 and 6 inclusive)
         */
        public static Direction _rotate60cw(Direction digit)
        {
            switch (digit) {
                case Direction.K_AXES_DIGIT:
                    return Direction.JK_AXES_DIGIT;
                case Direction.JK_AXES_DIGIT:
                    return Direction.J_AXES_DIGIT;
                case Direction.J_AXES_DIGIT:
                    return Direction.IJ_AXES_DIGIT;
                case Direction.IJ_AXES_DIGIT:
                    return Direction.I_AXES_DIGIT;
                case Direction.I_AXES_DIGIT:
                    return Direction.IK_AXES_DIGIT;
                case Direction.IK_AXES_DIGIT:
                    return Direction.K_AXES_DIGIT;
                default:
                    return digit;
            }
        }
        
        /**
         * Find the normalized ijk coordinates of the hex centered on the indicated
         * hex at the next finer aperture 3 counter-clockwise resolution. Works in
         * place.
         *
         * @param ijk The ijk coordinates.
         */
        public static void _downAp3(ref CoordIJK ijk)
        {
            // res r unit vectors in res r+1
            CoordIJK iVec = new CoordIJK { i=2, j=0, k=1 };
            CoordIJK jVec = new CoordIJK { i=1, j=2, k=0 };
            CoordIJK kVec = new CoordIJK { i=0, j=1, k=2 };

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /**
         * Find the normalized ijk coordinates of the hex centered on the indicated
         * hex at the next finer aperture 3 clockwise resolution. Works in place.
         *
         * @param ijk The ijk coordinates.
         */
        public static void _downAp3r(ref CoordIJK ijk)
        {
            // res r unit vectors in res r+1
            CoordIJK iVec = new CoordIJK { i=2, j=1, k=0 };
            CoordIJK jVec = new CoordIJK { i=0, j=2, k=1 };
            CoordIJK kVec = new CoordIJK { i=1, j=0, k=2 };

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }
        
        /**
         * Finds the distance between the two coordinates. Returns result.
         *
         * @param c1 The first set of ijk coordinates.
         * @param c2 The second set of ijk coordinates.
         */
        public static int ijkDistance(in CoordIJK c1, in CoordIJK c2)
        {
            CoordIJK diff = new CoordIJK();
            _ijkSub(c1, c2, ref diff);
            _ijkNormalize(ref diff);
            CoordIJK absDiff = new CoordIJK { i=Math.Abs(diff.i), j=Math.Abs(diff.j), k=Math.Abs(diff.k) };
            return Math.Max(absDiff.i, Math.Max(absDiff.j, absDiff.k));
        }
        
        /**
         * Transforms coordinates from the IJK+ coordinate system to the IJ coordinate
         * system.
         *
         * @param ijk The input IJK+ coordinates
         * @param ij The output IJ coordinates
         */
        public static void ijkToIj(in CoordIJK ijk, ref CoordIJ ij)
        {
            ij.i = ijk.i - ijk.k;
            ij.j = ijk.j - ijk.k;
        }

        /**
         * Transforms coordinates from the IJ coordinate system to the IJK+ coordinate
         * system.
         *
         * @param ij The input IJ coordinates
         * @param ijk The output IJK+ coordinates
         */
        public static void ijToIjk(in CoordIJ ij, ref CoordIJK ijk)
        {
            ijk.i = ij.i;
            ijk.j = ij.j;
            ijk.k = 0;

            _ijkNormalize(ref ijk);
        }
        
        /**
         * Convert IJK coordinates to cube coordinates, in place
         * @param ijk Coordinate to convert
         */
        public static void ijkToCube(ref CoordIJK ijk)
        {
            ijk.i = -ijk.i + ijk.k;
            ijk.j = ijk.j - ijk.k;
            ijk.k = -ijk.i - ijk.j;
        }

        /**
         * Convert cube coordinates to IJK coordinates, in place
         * @param ijk Coordinate to convert
         */
        public static void cubeToIjk(ref CoordIJK ijk)
        {
            ijk.i = -ijk.i;
            ijk.k = 0;
            _ijkNormalize(ref ijk);
        }
    }
    
    /** @brief H3 digit representing ijk+ axes direction.
     * Values will be within the lowest 3 bits of an integer.
     */
    public enum Direction {
        /** H3 digit in center */
        CENTER_DIGIT = 0,
        /** H3 digit in k-axes direction */
        K_AXES_DIGIT = 1,
        /** H3 digit in j-axes direction */
        J_AXES_DIGIT = 2,
        /** H3 digit in j == k direction */
        JK_AXES_DIGIT = J_AXES_DIGIT | K_AXES_DIGIT, /* 3 */
        /** H3 digit in i-axes direction */
        I_AXES_DIGIT = 4,
        /** H3 digit in i == k direction */
        IK_AXES_DIGIT = I_AXES_DIGIT | K_AXES_DIGIT, /* 5 */
        /** H3 digit in i == j direction */
        IJ_AXES_DIGIT = I_AXES_DIGIT | J_AXES_DIGIT, /* 6 */
        /** H3 digit in the invalid direction */
        INVALID_DIGIT = 7,
        /** Valid digits will be less than this value. Same value as INVALID_DIGIT.
         */
        NUM_DIGITS = INVALID_DIGIT
    };
}