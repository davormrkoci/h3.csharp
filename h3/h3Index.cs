using System.Globalization;

namespace h3
{
    public struct H3Index
    {
        private ulong value;

        public H3Index(ulong value) { this.value = value; }

        public static implicit operator H3Index(ulong h3value) => new H3Index(h3value);

        public static implicit operator ulong(H3Index h3index) => h3index.value;

        /** The number of bits in an H3 index. */
        public const int H3_NUM_BITS = 64;

        /** The bit offset of the max resolution digit in an H3 index. */
        public const int H3_MAX_OFFSET = 63;

        /** The bit offset of the mode in an H3 index. */
        public const int H3_MODE_OFFSET = 59;

        /** The bit offset of the base cell in an H3 index. */
        public const int H3_BC_OFFSET = 45;

        /** The bit offset of the resolution in an H3 index. */
        public const int H3_RES_OFFSET = 52;

        /** The bit offset of the reserved bits in an H3 index. */
        public const int H3_RESERVED_OFFSET = 56;

        /** The number of bits in a single H3 resolution digit. */
        public const int H3_PER_DIGIT_OFFSET = 3;

        /** 1 in the highest bit, 0's everywhere else. */
        public const ulong H3_HIGH_BIT_MASK = ((ulong) (1) << H3_MAX_OFFSET);

        /** 0 in the highest bit, 1's everywhere else. */
        public const ulong H3_HIGH_BIT_MASK_NEGATIVE = (~H3_HIGH_BIT_MASK);

        /** 1's in the 4 mode bits, 0's everywhere else. */
        public const ulong H3_MODE_MASK = ((ulong) (15) << H3_MODE_OFFSET);

        /** 0's in the 4 mode bits, 1's everywhere else. */
        public const ulong H3_MODE_MASK_NEGATIVE = (~H3_MODE_MASK);

        /** 1's in the 7 base cell bits, 0's everywhere else. */
        public const ulong H3_BC_MASK = ((ulong) (127) << H3_BC_OFFSET);

        /** 0's in the 7 base cell bits, 1's everywhere else. */
        public const ulong H3_BC_MASK_NEGATIVE = (~H3_BC_MASK);

        /** 1's in the 4 resolution bits, 0's everywhere else. */
        public const ulong H3_RES_MASK = ((ulong) (15) << H3_RES_OFFSET);

        /** 0's in the 4 resolution bits, 1's everywhere else. */
        public const ulong H3_RES_MASK_NEGATIVE = (~H3_RES_MASK);

        /** 1's in the 3 reserved bits, 0's everywhere else. */
        public const ulong H3_RESERVED_MASK = ((ulong) (7) << H3_RESERVED_OFFSET);

        /** 0's in the 3 reserved bits, 1's everywhere else. */
        public const ulong H3_RESERVED_MASK_NEGATIVE = (~H3_RESERVED_MASK);

        /** 1's in the 3 bits of res 15 digit bits, 0's everywhere else. */
        public const ulong H3_DIGIT_MASK = ((ulong) (7));

        /** 0's in the 7 base cell bits, 1's everywhere else. */
        public const ulong H3_DIGIT_MASK_NEGATIVE = (~H3_DIGIT_MASK);

        /** H3 index with mode 0, res 0, base cell 0, and 7 for all index digits. */
        public const ulong H3_INIT = 35184372088831ul;

        /**
         * Gets the highest bit of the H3 index.
         */
        public static int H3_GET_HIGH_BIT(H3Index h3)
        {
            return ((int) ((((h3) & H3_HIGH_BIT_MASK) >> H3_MAX_OFFSET)));
        }

        /**
         * Sets the highest bit of the h3 to v.
         */
        public static void H3_SET_HIGH_BIT(ref H3Index h3, int v)
        {
            h3 = (((h3) & H3_HIGH_BIT_MASK_NEGATIVE) | (((ulong) (v)) << H3_MAX_OFFSET));
        }

        /**
         * Gets the integer mode of h3.
         */
        public static int H3_GET_MODE(H3Index h3)
        {
            return ((int) ((((h3) & H3_MODE_MASK) >> H3_MODE_OFFSET)));
        }

        /**
         * Sets the integer mode of h3 to v.
         */
        public static void H3_SET_MODE(ref H3Index h3, int v)
        {
            h3 = (((h3) & H3_MODE_MASK_NEGATIVE) | (((ulong) (v)) << H3_MODE_OFFSET));
        }

        /**
         * Gets the integer base cell of h3.
         */
        public static int H3_GET_BASE_CELL(H3Index h3)
        {
            return ((int) ((((h3) & H3_BC_MASK) >> H3_BC_OFFSET)));
        }

        /**
         * Sets the integer base cell of h3 to bc.
         */
        public static void H3_SET_BASE_CELL(ref H3Index h3, int bc)
        {
            h3 = (((h3) & H3_BC_MASK_NEGATIVE) | (((ulong) (bc)) << H3_BC_OFFSET));
        }

        /**
         * Gets the integer resolution of h3.
         */
        public static int H3_GET_RESOLUTION(H3Index h3)
        {
            return ((int) ((((h3) & H3_RES_MASK) >> H3_RES_OFFSET)));
        }

        /**
         * Sets the integer resolution of h3.
         */
        public static H3Index H3_SET_RESOLUTION(ref H3Index h3, int res)
        {
            h3 = (((h3) & H3_RES_MASK_NEGATIVE) | (((ulong) (res)) << H3_RES_OFFSET));
            return h3;
        }

        /**
         * Gets the resolution res integer digit (0-7) of h3.
         */
        public static Direction H3_GET_INDEX_DIGIT(H3Index h3, int res)
        {
            return ((Direction) ((((h3) >> ((Constants.MAX_H3_RES - (res)) * H3_PER_DIGIT_OFFSET)) & H3_DIGIT_MASK)));
        }

        /**
         * Sets a value in the reserved space. Setting to non-zero may produce invalid
         * indexes.
         */
        public static void H3_SET_RESERVED_BITS(ref H3Index h3, int v)
        {
            h3 = (((h3) & H3_RESERVED_MASK_NEGATIVE) | (((ulong) (v)) << H3_RESERVED_OFFSET));
        }

        /**
         * Gets a value in the reserved space. Should always be zero for valid indexes.
         */
        public static int H3_GET_RESERVED_BITS(H3Index h3)
        {
            return ((int) ((((h3) & H3_RESERVED_MASK) >> H3_RESERVED_OFFSET)));
        }

        /**
         * Sets the resolution res digit of h3 to the integer digit (0-7)
         */
        public static void H3_SET_INDEX_DIGIT(ref H3Index h3, int res, Direction digit)
        {
            h3 = (((h3) & ~((H3_DIGIT_MASK << ((Constants.MAX_H3_RES - (res)) * H3_PER_DIGIT_OFFSET)))) | 
                    (((ulong) (digit)) << ((Constants.MAX_H3_RES - (res)) * H3_PER_DIGIT_OFFSET)));
        }

        /**
         * Invalid index used to indicate an error from geoToH3 and related functions.
         */
        public const ulong H3_INVALID_INDEX = 0;

        /*
         * Return codes for compact
         */
        public const int COMPACT_SUCCESS = 0;
        public const int COMPACT_LOOP_EXCEEDED = -1;
        public const int COMPACT_DUPLICATE = -2;
        public const int COMPACT_ALLOC_FAILED = -3;
        
        /**
         * Returns the H3 resolution of an H3 index.
         * @param h The H3 index.
         * @return The resolution of the H3 index argument.
         */
        public static int h3GetResolution(H3Index h) { return H3_GET_RESOLUTION(h); }

        /**
         * Returns the H3 base cell "number" of an H3 cell (hexagon or pentagon).
         *
         * Note: Technically works on H3 edges, but will return base cell of the
         * origin cell.
         *
         * @param h The H3 cell.
         * @return The base cell "number" of the H3 cell argument.
         */
        public static int h3GetBaseCell(H3Index h) { return H3_GET_BASE_CELL(h); }

        /**
         * Converts a string representation of an H3 index into an H3 index.
         * @param str The string representation of an H3 index.
         * @return The H3 index corresponding to the string argument, or 0 if invalid.
         */
        public static H3Index stringToH3(string str) {
            ulong h3;
            if (ulong.TryParse(str, NumberStyles.HexNumber, null, out h3))
                return h3;
            
            return H3_INVALID_INDEX;
        }

        /**
         * Converts an H3 index into a string representation.
         * @param h The H3 index to convert.
         * @param str The string representation of the H3 index.
         * @param sz Size of the buffer `str`
         */
        public static string h3ToString(H3Index h)
        {
            return h.value.ToString("x");
        }

        /**
         * Returns whether or not an H3 index is a valid cell (hexagon or pentagon).
         * @param h The H3 index to validate.
         * @return 1 if the H3 index if valid, and 0 if it is not.
         */
        public static bool h3IsValid(H3Index h) {
            if (H3_GET_HIGH_BIT(h) != 0) return false;

            if (H3_GET_MODE(h) != Constants.H3_HEXAGON_MODE) return false;

            if (H3_GET_RESERVED_BITS(h) != 0) return false;

            int baseCell = H3_GET_BASE_CELL(h);
            if (baseCell < 0 || baseCell >= Constants.NUM_BASE_CELLS) return false;

            int res = H3_GET_RESOLUTION(h);
            if (res < 0 || res > Constants.MAX_H3_RES) return false;

            bool foundFirstNonZeroDigit = false;
            for (int r = 1; r <= res; r++) {
                Direction digit = H3_GET_INDEX_DIGIT(h, r);

                if (!foundFirstNonZeroDigit && digit != Direction.CENTER_DIGIT) {
                    foundFirstNonZeroDigit = true;
                    if (BaseCellData._isBaseCellPentagon(baseCell) && digit == Direction.K_AXES_DIGIT) {
                        return false;
                    }
                }

                if (digit < Direction.CENTER_DIGIT || digit >= Direction.NUM_DIGITS) return false;
            }

            for (int r = res + 1; r <= Constants.MAX_H3_RES; r++) {
                Direction digit = H3_GET_INDEX_DIGIT(h, r);
                if (digit != Direction.INVALID_DIGIT) return false;
            }

            return true;
        }

        /**
         * Initializes an H3 index.
         * @param hp The H3 index to initialize.
         * @param res The H3 resolution to initialize the index to.
         * @param baseCell The H3 base cell to initialize the index to.
         * @param initDigit The H3 digit (0-7) to initialize all of the index digits to.
         */
        public static void setH3Index(ref H3Index hp, int res, int baseCell, Direction initDigit) {
            H3Index h = H3_INIT;
            H3_SET_MODE(ref h, Constants.H3_HEXAGON_MODE);
            H3_SET_RESOLUTION(ref h, res);
            H3_SET_BASE_CELL(ref h, baseCell);
            for (int r = 1; r <= res; r++) H3_SET_INDEX_DIGIT(ref h, r, initDigit);
            hp = h;
        }

        /**
         * h3ToParent produces the parent index for a given H3 index
         *
         * @param h H3Index to find parent of
         * @param parentRes The resolution to switch to (parent, grandparent, etc)
         *
         * @return H3Index of the parent, or 0 if you actually asked for a child
         */
        public static H3Index h3ToParent(H3Index h, int parentRes) {
            int childRes = H3_GET_RESOLUTION(h);
            if (parentRes > childRes) {
                return H3_INVALID_INDEX;
            } else if (parentRes == childRes) {
                return h;
            } else if (parentRes < 0 || parentRes > Constants.MAX_H3_RES) {
                return H3_INVALID_INDEX;
            }
            H3Index parentH = H3_SET_RESOLUTION(ref h, parentRes);
            for (int i = parentRes + 1; i <= childRes; i++) {
                H3_SET_INDEX_DIGIT(ref parentH, i, (Direction) H3_DIGIT_MASK);
            }
            return parentH;
        }

        /**
         * Determines whether one resolution is a valid child resolution of another.
         * Each resolution is considered a valid child resolution of itself.
         *
         * @param parentRes int resolution of the parent
         * @param childRes int resolution of the child
         *
         * @return The validity of the child resolution
         */
        public static bool _isValidChildRes(int parentRes, int childRes) {
            if (childRes < parentRes || childRes > Constants.MAX_H3_RES) {
                return false;
            }
            return true;
        }

        /**
         * maxH3ToChildrenSize returns the maximum number of children possible for a
         * given child level.
         *
         * @param h H3Index to find the number of children of
         * @param childRes The resolution of the child level you're interested in
         *
         * @return int count of maximum number of children (equal for hexagons, less for
         * pentagons
         */
        public static int maxH3ToChildrenSize(H3Index h, int childRes) {
            int parentRes = H3_GET_RESOLUTION(h);
            if (!_isValidChildRes(parentRes, childRes)) {
                return 0;
            }
            return mathExtensions._ipow(7, (childRes - parentRes));
        }

        /**
         * makeDirectChild takes an index and immediately returns the immediate child
         * index based on the specified cell number. Bit operations only, could generate
         * invalid indexes if not careful (deleted cell under a pentagon).
         *
         * @param h H3Index to find the direct child of
         * @param cellNumber int id of the direct child (0-6)
         *
         * @return The new H3Index for the child
         */
        public static H3Index makeDirectChild(H3Index h, Direction cellNumber) {
            int childRes = H3_GET_RESOLUTION(h) + 1;
            H3Index childH = H3_SET_RESOLUTION(ref h, childRes);
            H3_SET_INDEX_DIGIT(ref childH, childRes, cellNumber);
            return childH;
        }

        /**
         * h3ToChildren takes the given hexagon id and generates all of the children
         * at the specified resolution storing them into the provided memory pointer.
         * It's assumed that maxH3ToChildrenSize was used to determine the allocation.
         *
         * @param h H3Index to find the children of
         * @param childRes int the child level to produce
         * @param children H3Index* the memory to store the resulting addresses in
         */
        public static void h3ToChildren(H3Index h, int childRes, H3Index[] children, int idx = 0)
        {
            int parentRes = H3_GET_RESOLUTION(h);
            if (!_isValidChildRes(parentRes, childRes)) {
                return;
            } else if (parentRes == childRes) {
                children[idx++] = h;
                return;
            }
            int bufferSize = maxH3ToChildrenSize(h, childRes);
            int bufferChildStep = (bufferSize / 7);
            bool isAPentagon = h3IsPentagon(h);
            for (Direction i = 0; i < Direction.NUM_DIGITS; i++) {
                if (isAPentagon && i == Direction.K_AXES_DIGIT) {
                    int nextChildIdx = idx + bufferChildStep;
                    while (idx < nextChildIdx) {
                        children[idx++] = H3_INVALID_INDEX;
                    }
                } else {
                    h3ToChildren(makeDirectChild(h, i), childRes, children, idx);
                    idx += bufferChildStep;
                }
            }
        }

        /**
         * h3ToCenterChild produces the center child index for a given H3 index at
         * the specified resolution
         *
         * @param h H3Index to find center child of
         * @param childRes The resolution to switch to
         *
         * @return H3Index of the center child, or 0 if you actually asked for a parent
         */
        public static H3Index h3ToCenterChild(H3Index h, int childRes) {
            int parentRes = H3_GET_RESOLUTION(h);
            if (!_isValidChildRes(parentRes, childRes)) {
                return H3_INVALID_INDEX;
            } else if (childRes == parentRes) {
                return h;
            }
            H3Index child = H3_SET_RESOLUTION(ref h, childRes);
            for (int i = parentRes + 1; i <= childRes; i++) {
                H3_SET_INDEX_DIGIT(ref child, i, 0);
            }
            return child;
        }

        /**
         * compact takes a set of hexagons all at the same resolution and compresses
         * them by pruning full child branches to the parent level. This is also done
         * for all parents recursively to get the minimum number of hex addresses that
         * perfectly cover the defined space.
         * @param h3Set Set of hexagons
         * @param compactedSet The output array of compressed hexagons (preallocated)
         * @param numHexes The size of the input and output arrays (possible that no
         * contiguous regions exist in the set at all and no compression possible)
         * @return an error code on bad input data
         */
        public static int compact(in H3Index[] h3Set, H3Index[] compactedSet, int numHexes) {
            if (numHexes == 0) {
                return COMPACT_SUCCESS;
            }
            int res = H3_GET_RESOLUTION(h3Set[0]);
            if (res == 0) {
                // No compaction possible, just copy the set to output
                for (int i = 0; i < numHexes; i++) {
                    compactedSet[i] = h3Set[i];
                }
                return COMPACT_SUCCESS;
            }
            H3Index[] remainingHexes = new H3Index[numHexes];
            for (int i = 0; i < numHexes; ++i)
            {
                remainingHexes[i] = h3Set[i];
            }
            H3Index[] hashSetArray = new H3Index[numHexes];
            int compactedSetOffset = 0;
            int numRemainingHexes = numHexes;
            while (numRemainingHexes > 0) {
                res = H3_GET_RESOLUTION(remainingHexes[0]);
                int parentRes = res - 1;
                // Put the parents of the hexagons into the temp array
                // via a hashing mechanism, and use the reserved bits
                // to track how many times a parent is duplicated
                for (int i = 0; i < numRemainingHexes; i++) {
                    H3Index currIndex = remainingHexes[i];
                    if (currIndex != 0) {
                        H3Index parent = h3ToParent(currIndex, parentRes);
                        // Modulus hash the parent into the temp array
                        int loc = (int)(parent % (ulong) numRemainingHexes);
                        int loopCount = 0;
                        while (hashSetArray[loc] != 0) {
                            if (loopCount > numRemainingHexes) {  
                                // This case should not be possible because at most one
                                // index is placed into hashSetArray per
                                // numRemainingHexes.
                                return COMPACT_LOOP_EXCEEDED;
                            }
                            H3Index tempIndex =
                                hashSetArray[loc] & H3_RESERVED_MASK_NEGATIVE;
                            if (tempIndex == parent) {
                                int count = H3_GET_RESERVED_BITS(hashSetArray[loc]) + 1;
                                int limitCount = 7;
                                if (h3IsPentagon(
                                        tempIndex & H3_RESERVED_MASK_NEGATIVE)) {
                                    limitCount--;
                                }
                                // One is added to count for this check to match one
                                // being added to count later in this function when
                                // checking for all children being present.
                                if (count + 1 > limitCount) {
                                    // Only possible on duplicate input
                                    return COMPACT_DUPLICATE;
                                }
                                H3_SET_RESERVED_BITS(ref parent, count);
                                hashSetArray[loc] = H3_INVALID_INDEX;
                            } else {
                                loc = (loc + 1) % numRemainingHexes;
                            }
                            loopCount++;
                        }
                        hashSetArray[loc] = parent;
                    }
                }
                // Determine which parent hexagons have a complete set
                // of children and put them in the compactableHexes array
                int compactableCount = 0;
                int maxCompactableCount =
                    numRemainingHexes / 6;  // Somehow all pentagons; conservative
                if (maxCompactableCount == 0)
                {
                    for (int i = 0; i < numRemainingHexes; ++i)
                    {
                        compactedSet[compactedSetOffset+i] = remainingHexes[i];
                    }
                    break;
                }
                H3Index[] compactableHexes = new H3Index[maxCompactableCount];
                for (int i = 0; i < numRemainingHexes; i++) {
                    if (hashSetArray[i] == 0) continue;
                    int count = H3_GET_RESERVED_BITS(hashSetArray[i]) + 1;
                    // Include the deleted direction for pentagons as implicitly "there"
                    if (h3IsPentagon(hashSetArray[i] &
                                                H3_RESERVED_MASK_NEGATIVE)) {
                        // We need this later on, no need to recalculate
                        H3_SET_RESERVED_BITS(ref hashSetArray[i], count);
                        // Increment count after setting the reserved bits,
                        // since count is already incremented above, so it
                        // will be the expected value for a complete hexagon.
                        count++;
                    }
                    if (count == 7) {
                        // Bingo! Full set!
                        compactableHexes[compactableCount] =
                            hashSetArray[i] & H3_RESERVED_MASK_NEGATIVE;
                        compactableCount++;
                    }
                }
                // Uncompactable hexes are immediately copied into the
                // output compactedSetOffset
                int uncompactableCount = 0;
                for (int i = 0; i < numRemainingHexes; i++) {
                    H3Index currIndex = remainingHexes[i];
                    if (currIndex != H3_INVALID_INDEX) {
                        H3Index parent = h3ToParent(currIndex, parentRes);
                        // Modulus hash the parent into the temp array
                        // to determine if this index was included in
                        // the compactableHexes array
                        int loc = (int)(parent % (ulong) numRemainingHexes);
                        int loopCount = 0;
                        bool isUncompactable = true;
                        do {
                            if (loopCount > numRemainingHexes) {  // LCOV_EXCL_BR_LINE
                                // This case should not be possible because at most one
                                // index is placed into hashSetArray per input hexagon.
                                return COMPACT_LOOP_EXCEEDED;
                            }
                            H3Index tempIndex =
                                hashSetArray[loc] & H3_RESERVED_MASK_NEGATIVE;
                            if (tempIndex == parent) {
                                int count = H3_GET_RESERVED_BITS(hashSetArray[loc]) + 1;
                                if (count == 7) {
                                    isUncompactable = false;
                                }
                                break;
                            } else {
                                loc = (loc + 1) % numRemainingHexes;
                            }
                            loopCount++;
                        } while (hashSetArray[loc] != parent);
                        if (isUncompactable)
                        {
                            compactedSet[compactedSetOffset + uncompactableCount] = remainingHexes[i];
                            uncompactableCount++;
                        }
                    }
                }
                // Set up for the next loop
                for (int i = 0; i < numHexes; ++i)
                {
                    hashSetArray[i] = 0;
                }
                compactedSetOffset += uncompactableCount;
                for (int i = 0; i < compactableCount; ++i)
                {
                    remainingHexes[i] = compactableHexes[i];
                }
                numRemainingHexes = compactableCount;
            }
            return COMPACT_SUCCESS;
        }

        /**
         * uncompact takes a compressed set of hexagons and expands back to the
         * original set of hexagons.
         * @param compactedSet Set of hexagons
         * @param numHexes The number of hexes in the input set
         * @param h3Set Output array of decompressed hexagons (preallocated)
         * @param maxHexes The size of the output array to bound check against
         * @param res The hexagon resolution to decompress to
         * @return An error code if output array is too small or any hexagon is
         * smaller than the output resolution.
         */
        public static int uncompact(in H3Index[] compactedSet, int numHexes,
                H3Index[] h3Set, int maxHexes, int res) {
            int outOffset = 0;
            for (int i = 0; i < numHexes; i++) {
                if (compactedSet[i] == 0) continue;
                if (outOffset >= maxHexes) {
                    // We went too far, abort!
                    return -1;
                }
                int currentRes = H3_GET_RESOLUTION(compactedSet[i]);
                if (!_isValidChildRes(currentRes, res)) {
                    // Nonsensical. Abort.
                    return -2;
                }
                if (currentRes == res) {
                    // Just copy and move along
                    h3Set[outOffset] = compactedSet[i];
                    outOffset++;
                } else {
                    // Bigger hexagon to reduce in size
                    int numHexesToGen =
                        maxH3ToChildrenSize(compactedSet[i], res);
                    if (outOffset + numHexesToGen > maxHexes) {
                        // We're about to go too far, abort!
                        return -1;
                    }
                    h3ToChildren(compactedSet[i], res, h3Set, outOffset);
                    outOffset += numHexesToGen;
                }
            }
            return 0;
        }

        /**
         * maxUncompactSize takes a compacted set of hexagons are provides an
         * upper-bound estimate of the size of the uncompacted set of hexagons.
         * @param compactedSet Set of hexagons
         * @param numHexes The number of hexes in the input set
         * @param res The hexagon resolution to decompress to
         * @return The number of hexagons to allocate memory for, or a negative
         * number if an error occurs.
         */
        public static int maxUncompactSize(in H3Index[] compactedSet, int numHexes, int res) {
            int maxNumHexagons = 0;
            for (int i = 0; i < numHexes; i++) {
                if (compactedSet[i] == 0) continue;
                int currentRes = H3_GET_RESOLUTION(compactedSet[i]);
                if (!_isValidChildRes(currentRes, res)) {
                    // Nonsensical. Abort.
                    return -1;
                }
                if (currentRes == res) {
                    maxNumHexagons++;
                } else {
                    // Bigger hexagon to reduce in size
                    int numHexesToGen =
                        maxH3ToChildrenSize(compactedSet[i], res);
                    maxNumHexagons += numHexesToGen;
                }
            }
            return maxNumHexagons;
        }

        /**
         * h3IsResClassIII takes a hexagon ID and determines if it is in a
         * Class III resolution (rotated versus the icosahedron and subject
         * to shape distortion adding extra points on icosahedron edges, making
         * them not true hexagons).
         * @param h The H3Index to check.
         * @return Returns 1 if the hexagon is class III, otherwise 0.
         */
        public static int h3IsResClassIII(H3Index h) { return H3_GET_RESOLUTION(h) % 2; }

        /**
         * h3IsPentagon takes an H3Index and determines if it is actually a
         * pentagon.
         * @param h The H3Index to check.
         * @return Returns 1 if it is a pentagon, otherwise 0.
         */
        public static bool h3IsPentagon(H3Index h) {
            return BaseCellData._isBaseCellPentagon(H3_GET_BASE_CELL(h)) &&
                   _h3LeadingNonZeroDigit(h) == Direction.CENTER_DIGIT;
        }

        /**
         * Returns the highest resolution non-zero digit in an H3Index.
         * @param h The H3Index.
         * @return The highest resolution non-zero digit in the H3Index.
         */
        public static Direction _h3LeadingNonZeroDigit(H3Index h) {
            for (int r = 1; r <= H3_GET_RESOLUTION(h); r++)
                if (H3_GET_INDEX_DIGIT(h, r) != Direction.CENTER_DIGIT) return H3_GET_INDEX_DIGIT(h, r);

            // if we're here it's all 0's
            return Direction.CENTER_DIGIT;
        }

        /**
         * Rotate an H3Index 60 degrees counter-clockwise about a pentagonal center.
         * @param h The H3Index.
         */
        public static H3Index _h3RotatePent60ccw(H3Index h) {
            // rotate in place; skips any leading 1 digits (k-axis)

            int foundFirstNonZeroDigit = 0;
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                // rotate this digit
                H3_SET_INDEX_DIGIT(ref h, r, CoordIJK._rotate60ccw(H3_GET_INDEX_DIGIT(h, r)));

                // look for the first non-zero digit so we
                // can adjust for deleted k-axes sequence
                // if necessary
                if (foundFirstNonZeroDigit == 0 && H3_GET_INDEX_DIGIT(h, r) != 0) {
                    foundFirstNonZeroDigit = 1;

                    // adjust for deleted k-axes sequence
                    if (_h3LeadingNonZeroDigit(h) == Direction.K_AXES_DIGIT)
                        h = _h3Rotate60ccw(h);
                }
            }
            return h;
        }

        /**
         * Rotate an H3Index 60 degrees clockwise about a pentagonal center.
         * @param h The H3Index.
         */
        public static H3Index _h3RotatePent60cw(H3Index h) {
            // rotate in place; skips any leading 1 digits (k-axis)

            int foundFirstNonZeroDigit = 0;
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                // rotate this digit
                H3_SET_INDEX_DIGIT(ref h, r, CoordIJK._rotate60cw(H3_GET_INDEX_DIGIT(h, r)));

                // look for the first non-zero digit so we
                // can adjust for deleted k-axes sequence
                // if necessary
                if (foundFirstNonZeroDigit == 0 && H3_GET_INDEX_DIGIT(h, r) != 0) {
                    foundFirstNonZeroDigit = 1;

                    // adjust for deleted k-axes sequence
                    if (_h3LeadingNonZeroDigit(h) == Direction.K_AXES_DIGIT) h = _h3Rotate60cw(h);
                }
            }
            return h;
        }

        /**
         * Rotate an H3Index 60 degrees counter-clockwise.
         * @param h The H3Index.
         */
        public static H3Index _h3Rotate60ccw(H3Index h) {
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                Direction oldDigit = H3_GET_INDEX_DIGIT(h, r);
                H3_SET_INDEX_DIGIT(ref h, r, CoordIJK._rotate60ccw(oldDigit));
            }

            return h;
        }

        /**
         * Rotate an H3Index 60 degrees clockwise.
         * @param h The H3Index.
         */
        public static H3Index _h3Rotate60cw(H3Index h) {
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                H3_SET_INDEX_DIGIT(ref h, r, CoordIJK._rotate60cw(H3_GET_INDEX_DIGIT(h, r)));
            }

            return h;
        }

        /**
         * Convert an FaceIJK address to the corresponding H3Index.
         * @param fijk The FaceIJK address.
         * @param res The cell resolution.
         * @return The encoded H3Index (or 0 on failure).
         */
        public static H3Index _faceIjkToH3(in FaceIJK fijk, int res) {
            // initialize the index
            H3Index h = H3_INIT;
            H3_SET_MODE(ref h, Constants.H3_HEXAGON_MODE);
            H3_SET_RESOLUTION(ref h, res);

            // check for res 0/base cell
            if (res == 0) {
                if (fijk.coord.i > BaseCellData.MAX_FACE_COORD || fijk.coord.j > BaseCellData.MAX_FACE_COORD ||
                    fijk.coord.k > BaseCellData.MAX_FACE_COORD) {
                    // out of range input
                    return H3_INVALID_INDEX;
                }

                H3_SET_BASE_CELL(ref h, BaseCellData._faceIjkToBaseCell(fijk));
                return h;
            }

            // we need to find the correct base cell FaceIJK for this H3 index;
            // start with the passed in face and resolution res ijk coordinates
            // in that face's coordinate system
            FaceIJK fijkBC = fijk;

            // build the H3Index from finest res up
            // adjust r for the fact that the res 0 base cell offsets the indexing
            // digits
            ref CoordIJK ijk = ref fijkBC.coord;
            for (int r = res - 1; r >= 0; r--) {
                CoordIJK lastIJK = ijk;
                CoordIJK lastCenter;
                if (isResClassIII(r + 1)) {
                    // rotate ccw
                    CoordIJK._upAp7(ref ijk);
                    lastCenter = ijk;
                    CoordIJK._downAp7(ref lastCenter);
                } else {
                    // rotate cw
                    CoordIJK._upAp7r(ref ijk);
                    lastCenter = ijk;
                    CoordIJK._downAp7r(ref lastCenter);
                }

                CoordIJK diff = new CoordIJK();
                CoordIJK._ijkSub(lastIJK, lastCenter, ref diff);
                CoordIJK._ijkNormalize(ref diff);

                H3_SET_INDEX_DIGIT(ref h, r + 1, CoordIJK._unitIjkToDigit(diff));
            }

            // fijkBC should now hold the IJK of the base cell in the
            // coordinate system of the current face

            if (fijkBC.coord.i > BaseCellData.MAX_FACE_COORD || fijkBC.coord.j > BaseCellData.MAX_FACE_COORD ||
                fijkBC.coord.k > BaseCellData.MAX_FACE_COORD) {
                // out of range input
                return H3_INVALID_INDEX;
            }

            // lookup the correct base cell
            int baseCell = BaseCellData._faceIjkToBaseCell(fijkBC);
            H3_SET_BASE_CELL(ref h, baseCell);

            // rotate if necessary to get canonical base cell orientation
            // for this base cell
            int numRots = BaseCellData._faceIjkToBaseCellCCWrot60(fijkBC);
            if (BaseCellData._isBaseCellPentagon(baseCell)) {
                // force rotation out of missing k-axes sub-sequence
                if (_h3LeadingNonZeroDigit(h) == Direction.K_AXES_DIGIT) {
                    // check for a cw/ccw offset face; default is ccw
                    if (BaseCellData._baseCellIsCwOffset(baseCell, fijkBC.face)) {
                        h = _h3Rotate60cw(h);
                    } else {
                        h = _h3Rotate60ccw(h);
                    }
                }

                for (int i = 0; i < numRots; i++) h = _h3RotatePent60ccw(h);
            } else {
                for (int i = 0; i < numRots; i++) {
                    h = _h3Rotate60ccw(h);
                }
            }

            return h;
        }

        static bool isfinite(double d)
        {
            return !double.IsInfinity(d) && !double.IsNaN(d);
        }

        /**
         * Encodes a coordinate on the sphere to the H3 index of the containing cell at
         * the specified resolution.
         *
         * Returns 0 on invalid input.
         *
         * @param g The spherical coordinates to encode.
         * @param res The desired H3 resolution for the encoding.
         * @return The encoded H3Index (or 0 on failure).
         */
        public static H3Index geoToH3(in GeoCoord g, int res) {
            if (res < 0 || res > Constants.MAX_H3_RES) {
                return H3_INVALID_INDEX;
            }
            if (!isfinite(g.lat) || !isfinite(g.lon)) {
                return H3_INVALID_INDEX;
            }

            FaceIJK fijk = new FaceIJK();
            FaceIJK._geoToFaceIjk(g, res, ref fijk);
            return _faceIjkToH3(fijk, res);
        }

        /**
         * Convert an H3Index to the FaceIJK address on a specified icosahedral face.
         * @param h The H3Index.
         * @param fijk The FaceIJK address, initialized with the desired face
         *        and normalized base cell coordinates.
         * @return Returns 1 if the possibility of overage exists, otherwise 0.
         */
        public static bool _h3ToFaceIjkWithInitializedFijk(H3Index h, ref FaceIJK fijk) {
            ref CoordIJK ijk = ref fijk.coord;
            int res = H3_GET_RESOLUTION(h);

            // center base cell hierarchy is entirely on this face
            bool possibleOverage = true;
            if (!BaseCellData._isBaseCellPentagon(H3_GET_BASE_CELL(h)) &&
                (res == 0 ||
                 (fijk.coord.i == 0 && fijk.coord.j == 0 && fijk.coord.k == 0)))
                possibleOverage = false;

            for (int r = 1; r <= res; r++) {
                if (isResClassIII(r)) {
                    // Class III == rotate ccw
                    CoordIJK._downAp7(ref ijk);
                } else {
                    // Class II == rotate cw
                    CoordIJK._downAp7r(ref ijk);
                }

                CoordIJK._neighbor(ref ijk, H3_GET_INDEX_DIGIT(h, r));
            }

            return possibleOverage;
        }

        /**
         * Convert an H3Index to a FaceIJK address.
         * @param h The H3Index.
         * @param fijk The corresponding FaceIJK address.
         */
        public static void _h3ToFaceIjk(H3Index h, ref FaceIJK fijk) {
            int baseCell = H3_GET_BASE_CELL(h);
            // adjust for the pentagonal missing sequence; all of sub-sequence 5 needs
            // to be adjusted (and some of sub-sequence 4 below)
            if (BaseCellData._isBaseCellPentagon(baseCell) && _h3LeadingNonZeroDigit(h) == Direction.IK_AXES_DIGIT)
                h = _h3Rotate60cw(h);

            // start with the "home" face and ijk+ coordinates for the base cell of c
            fijk = BaseCellData.baseCellData[baseCell].homeFijk;
            if (!_h3ToFaceIjkWithInitializedFijk(h, ref fijk))
                return;  // no overage is possible; h lies on this face

            // if we're here we have the potential for an "overage"; i.e., it is
            // possible that c lies on an adjacent face

            CoordIJK origIJK = fijk.coord;

            // if we're in Class III, drop into the next finer Class II grid
            int res = H3_GET_RESOLUTION(h);
            if (isResClassIII(res)) {
                // Class III
                CoordIJK._downAp7r(ref fijk.coord);
                res++;
            }

            // adjust for overage if needed
            // a pentagon base cell with a leading 4 digit requires special handling
            bool pentLeading4 =
                (BaseCellData._isBaseCellPentagon(baseCell) && _h3LeadingNonZeroDigit(h) == Direction.I_AXES_DIGIT);
            if (FaceIJK._adjustOverageClassII(ref fijk, res, pentLeading4, false) != Overage.NO_OVERAGE) {
                // if the base cell is a pentagon we have the potential for secondary
                // overages
                if (BaseCellData._isBaseCellPentagon(baseCell)) {
                    while (FaceIJK._adjustOverageClassII(ref fijk, res, false, false) != Overage.NO_OVERAGE)
                        continue;
                }

                if (res != H3_GET_RESOLUTION(h)) CoordIJK._upAp7r(ref fijk.coord);
            } else if (res != H3_GET_RESOLUTION(h)) {
                fijk.coord = origIJK;
            }
        }

        /**
         * Determines the spherical coordinates of the center point of an H3 index.
         *
         * @param h3 The H3 index.
         * @param g The spherical coordinates of the H3 cell center.
         */
        public static void h3ToGeo(H3Index h3, ref GeoCoord g) {
            FaceIJK fijk = new FaceIJK();
            _h3ToFaceIjk(h3, ref fijk);
            FaceIJK._faceIjkToGeo(fijk, H3_GET_RESOLUTION(h3), ref g);
        }

        /**
         * Determines the cell boundary in spherical coordinates for an H3 index.
         *
         * @param h3 The H3 index.
         * @param gb The boundary of the H3 cell in spherical coordinates.
         */
        public static void h3ToGeoBoundary(H3Index h3, ref GeoBoundary gb, bool addEdgeVerts=false) {
            FaceIJK fijk = new FaceIJK();
            _h3ToFaceIjk(h3, ref fijk);
            FaceIJK._faceIjkToGeoBoundary(fijk, H3_GET_RESOLUTION(h3),
                                  h3IsPentagon(h3), ref gb, addEdgeVerts);
        }

        /**
         * Returns the max number of possible icosahedron faces an H3 index
         * may intersect.
         *
         * @return int count of faces
         */
        public static int maxFaceCount(H3Index h3) {
            // a pentagon always intersects 5 faces, a hexagon never intersects more
            // than 2 (but may only intersect 1)
            return h3IsPentagon(h3) ? 5 : 2;
        }

        /**
         * Find all icosahedron faces intersected by a given H3 index, represented
         * as integers from 0-19. The array is sparse; since 0 is a valid value,
         * invalid array values are represented as -1. It is the responsibility of
         * the caller to filter out invalid values.
         *
         * @param h3 The H3 index
         * @param out Output array. Must be of size maxFaceCount(h3).
         */
        public static void h3GetFaces(H3Index h3, int[] faces) {
            int res = H3_GET_RESOLUTION(h3);
            bool isPentagon = h3IsPentagon(h3);

            // We can't use the vertex-based approach here for class II pentagons,
            // because all their vertices are on the icosahedron edges. Their
            // direct child pentagons cross the same faces, so use those instead.
            if (isPentagon && !isResClassIII(res)) {
                // Note that this would not work for res 15, but this is only run on
                // Class II pentagons, it should never be invoked for a res 15 index.
                H3Index childPentagon = makeDirectChild(h3, 0);
                h3GetFaces(childPentagon, faces);
                return;
            }

            // convert to FaceIJK
            FaceIJK fijk = new FaceIJK();
            _h3ToFaceIjk(h3, ref fijk);

            // Get all vertices as FaceIJK addresses. For simplicity, always
            // initialize the array with 6 verts, ignoring the last one for pentagons
            FaceIJK[] fijkVerts = new FaceIJK[Constants.NUM_HEX_VERTS];
            int vertexCount;

            if (isPentagon) {
                vertexCount = Constants.NUM_PENT_VERTS;
                FaceIJK._faceIjkPentToVerts(ref fijk, ref res, fijkVerts);
            } else {
                vertexCount = Constants.NUM_HEX_VERTS;
                FaceIJK._faceIjkToVerts(ref fijk, ref res, fijkVerts);
            }

            // We may not use all of the slots in the output array,
            // so fill with invalid values to indicate unused slots
            int faceCount = maxFaceCount(h3);
            for (int i = 0; i < faceCount; i++) {
                faces[i] = FaceIJK.INVALID_FACE;
            }

            // add each vertex face, using the output array as a hash set
            for (int i = 0; i < vertexCount; i++) {
                ref FaceIJK vert = ref fijkVerts[i];

                // Adjust overage, determining whether this vertex is
                // on another face
                if (isPentagon) {
                    FaceIJK._adjustPentVertOverage(ref vert, res);
                } else {
                    FaceIJK._adjustOverageClassII(ref vert, res, false, true);
                }

                // Save the face to the output array
                int face = vert.face;
                int pos = 0;
                // Find the first empty output position, or the first position
                // matching the current face
                while (faces[pos] != FaceIJK.INVALID_FACE && faces[pos] != face) pos++;
                faces[pos] = face;
            }
        }

        /**
         * pentagonIndexCount returns the number of pentagons (same at any resolution)
         *
         * @return int count of pentagon indexes
         */
        public static int pentagonIndexCount() { return Constants.NUM_PENTAGONS; }

        /**
         * Generates all pentagons at the specified resolution
         *
         * @param res The resolution to produce pentagons at.
         * @param out Output array. Must be of size pentagonIndexCount().
         */
        public static void getPentagonIndexes(int res, H3Index[] indices) {
            int i = 0;
            for (int bc = 0; bc < Constants.NUM_BASE_CELLS; bc++) {
                if (BaseCellData._isBaseCellPentagon(bc)) {
                    H3Index pentagon = new H3Index();
                    setH3Index(ref pentagon, res, bc, 0);
                    indices[i++] = pentagon;
                }
            }
        }

        /**
         * Returns whether or not a resolution is a Class III grid. Note that odd
         * resolutions are Class III and even resolutions are Class II.
         * @param res The H3 resolution.
         * @return 1 if the resolution is a Class III grid, and 0 if the resolution is
         *         a Class II grid.
         */
        public static bool isResClassIII(int res) { return (res % 2) != 0; }
    }
}