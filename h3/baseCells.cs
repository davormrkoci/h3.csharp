namespace h3
{
    /** @struct BaseCellOrient
     *  @brief base cell at a given ijk and required rotations into its system
     */
    public struct BaseCellOrient {
        public int baseCell;  ///< base cell number
        public int ccwRot60;  ///< number of ccw 60 degree rotations relative to current
                              /// face
    };
        
    /** @struct BaseCellData
     * @brief information on a single base cell
     */
    public class BaseCellData {
        public FaceIJK homeFijk;        ///< "home" face and normalized ijk coordinates on that face
        public bool isPentagon;         ///< is this base cell a pentagon?
        public int[] cwOffsetPent;      ///< if a pentagon, what are its two clockwise offset
                                        /// faces?
        
        public const int INVALID_BASE_CELL = 127;

        /** Maximum input for any component to face-to-base-cell lookup functions */
        public const int MAX_FACE_COORD = 2;

        public BaseCellData()
        {
            homeFijk = new FaceIJK();
            isPentagon = false;
            cwOffsetPent = new int[2];
        }
        
        /** @brief Neighboring base cell ID in each IJK direction.
         *
         * For each base cell, for each direction, the neighboring base
         * cell ID is given. 127 indicates there is no neighbor in that direction.
         */
        public static readonly int[][] baseCellNeighbors = {
            new [] {0, 1, 5, 2, 4, 3, 8},                          // base cell 0
            new [] {1, 7, 6, 9, 0, 3, 2},                          // base cell 1
            new [] {2, 6, 10, 11, 0, 1, 5},                        // base cell 2
            new [] {3, 13, 1, 7, 4, 12, 0},                        // base cell 3
            new [] {4, INVALID_BASE_CELL, 15, 8, 3, 0, 12},        // base cell 4 (pentagon)
            new [] {5, 2, 18, 10, 8, 0, 16},                       // base cell 5
            new [] {6, 14, 11, 17, 1, 9, 2},                       // base cell 6
            new [] {7, 21, 9, 19, 3, 13, 1},                       // base cell 7
            new [] {8, 5, 22, 16, 4, 0, 15},                       // base cell 8
            new [] {9, 19, 14, 20, 1, 7, 6},                       // base cell 9
            new [] {10, 11, 24, 23, 5, 2, 18},                     // base cell 10
            new [] {11, 17, 23, 25, 2, 6, 10},                     // base cell 11
            new [] {12, 28, 13, 26, 4, 15, 3},                     // base cell 12
            new [] {13, 26, 21, 29, 3, 12, 7},                     // base cell 13
            new [] {14, INVALID_BASE_CELL, 17, 27, 9, 20, 6},      // base cell 14 (pentagon)
            new [] {15, 22, 28, 31, 4, 8, 12},                     // base cell 15
            new [] {16, 18, 33, 30, 8, 5, 22},                     // base cell 16
            new [] {17, 11, 14, 6, 35, 25, 27},                    // base cell 17
            new [] {18, 24, 30, 32, 5, 10, 16},                    // base cell 18
            new [] {19, 34, 20, 36, 7, 21, 9},                     // base cell 19
            new [] {20, 14, 19, 9, 40, 27, 36},                    // base cell 20
            new [] {21, 38, 19, 34, 13, 29, 7},                    // base cell 21
            new [] {22, 16, 41, 33, 15, 8, 31},                    // base cell 22
            new [] {23, 24, 11, 10, 39, 37, 25},                   // base cell 23
            new [] {24, INVALID_BASE_CELL, 32, 37, 10, 23, 18},    // base cell 24 (pentagon)
            new [] {25, 23, 17, 11, 45, 39, 35},                   // base cell 25
            new [] {26, 42, 29, 43, 12, 28, 13},                   // base cell 26
            new [] {27, 40, 35, 46, 14, 20, 17},                   // base cell 27
            new [] {28, 31, 42, 44, 12, 15, 26},                   // base cell 28
            new [] {29, 43, 38, 47, 13, 26, 21},                   // base cell 29
            new [] {30, 32, 48, 50, 16, 18, 33},                   // base cell 30
            new [] {31, 41, 44, 53, 15, 22, 28},                   // base cell 31
            new [] {32, 30, 24, 18, 52, 50, 37},                   // base cell 32
            new [] {33, 30, 49, 48, 22, 16, 41},                   // base cell 33
            new [] {34, 19, 38, 21, 54, 36, 51},                   // base cell 34
            new [] {35, 46, 45, 56, 17, 27, 25},                   // base cell 35
            new [] {36, 20, 34, 19, 55, 40, 54},                   // base cell 36
            new [] {37, 39, 52, 57, 24, 23, 32},                   // base cell 37
            new [] {38, INVALID_BASE_CELL, 34, 51, 29, 47, 21},    // base cell 38 (pentagon)
            new [] {39, 37, 25, 23, 59, 57, 45},                   // base cell 39
            new [] {40, 27, 36, 20, 60, 46, 55},                   // base cell 40
            new [] {41, 49, 53, 61, 22, 33, 31},                   // base cell 41
            new [] {42, 58, 43, 62, 28, 44, 26},                   // base cell 42
            new [] {43, 62, 47, 64, 26, 42, 29},                   // base cell 43
            new [] {44, 53, 58, 65, 28, 31, 42},                   // base cell 44
            new [] {45, 39, 35, 25, 63, 59, 56},                   // base cell 45
            new [] {46, 60, 56, 68, 27, 40, 35},                   // base cell 46
            new [] {47, 38, 43, 29, 69, 51, 64},                   // base cell 47
            new [] {48, 49, 30, 33, 67, 66, 50},                   // base cell 48
            new [] {49, INVALID_BASE_CELL, 61, 66, 33, 48, 41},    // base cell 49 (pentagon)
            new [] {50, 48, 32, 30, 70, 67, 52},                   // base cell 50
            new [] {51, 69, 54, 71, 38, 47, 34},                   // base cell 51
            new [] {52, 57, 70, 74, 32, 37, 50},                   // base cell 52
            new [] {53, 61, 65, 75, 31, 41, 44},                   // base cell 53
            new [] {54, 71, 55, 73, 34, 51, 36},                   // base cell 54
            new [] {55, 40, 54, 36, 72, 60, 73},                   // base cell 55
            new [] {56, 68, 63, 77, 35, 46, 45},                   // base cell 56
            new [] {57, 59, 74, 78, 37, 39, 52},                   // base cell 57
            new [] {58, INVALID_BASE_CELL, 62, 76, 44, 65, 42},    // base cell 58 (pentagon)
            new [] {59, 63, 78, 79, 39, 45, 57},                   // base cell 59
            new [] {60, 72, 68, 80, 40, 55, 46},                   // base cell 60
            new [] {61, 53, 49, 41, 81, 75, 66},                   // base cell 61
            new [] {62, 43, 58, 42, 82, 64, 76},                   // base cell 62
            new [] {63, INVALID_BASE_CELL, 56, 45, 79, 59, 77},    // base cell 63 (pentagon)
            new [] {64, 47, 62, 43, 84, 69, 82},                   // base cell 64
            new [] {65, 58, 53, 44, 86, 76, 75},                   // base cell 65
            new [] {66, 67, 81, 85, 49, 48, 61},                   // base cell 66
            new [] {67, 66, 50, 48, 87, 85, 70},                   // base cell 67
            new [] {68, 56, 60, 46, 90, 77, 80},                   // base cell 68
            new [] {69, 51, 64, 47, 89, 71, 84},                   // base cell 69
            new [] {70, 67, 52, 50, 83, 87, 74},                   // base cell 70
            new [] {71, 89, 73, 91, 51, 69, 54},                   // base cell 71
            new [] {72, INVALID_BASE_CELL, 73, 55, 80, 60, 88},    // base cell 72 (pentagon)
            new [] {73, 91, 72, 88, 54, 71, 55},                   // base cell 73
            new [] {74, 78, 83, 92, 52, 57, 70},                   // base cell 74
            new [] {75, 65, 61, 53, 94, 86, 81},                   // base cell 75
            new [] {76, 86, 82, 96, 58, 65, 62},                   // base cell 76
            new [] {77, 63, 68, 56, 93, 79, 90},                   // base cell 77
            new [] {78, 74, 59, 57, 95, 92, 79},                   // base cell 78
            new [] {79, 78, 63, 59, 93, 95, 77},                   // base cell 79
            new [] {80, 68, 72, 60, 99, 90, 88},                   // base cell 80
            new [] {81, 85, 94, 101, 61, 66, 75},                  // base cell 81
            new [] {82, 96, 84, 98, 62, 76, 64},                   // base cell 82
            new [] {83, INVALID_BASE_CELL, 74, 70, 100, 87, 92},   // base cell 83 (pentagon)
            new [] {84, 69, 82, 64, 97, 89, 98},                   // base cell 84
            new [] {85, 87, 101, 102, 66, 67, 81},                 // base cell 85
            new [] {86, 76, 75, 65, 104, 96, 94},                  // base cell 86
            new [] {87, 83, 102, 100, 67, 70, 85},                 // base cell 87
            new [] {88, 72, 91, 73, 99, 80, 105},                  // base cell 88
            new [] {89, 97, 91, 103, 69, 84, 71},                  // base cell 89
            new [] {90, 77, 80, 68, 106, 93, 99},                  // base cell 90
            new [] {91, 73, 89, 71, 105, 88, 103},                 // base cell 91
            new [] {92, 83, 78, 74, 108, 100, 95},                 // base cell 92
            new [] {93, 79, 90, 77, 109, 95, 106},                 // base cell 93
            new [] {94, 86, 81, 75, 107, 104, 101},                // base cell 94
            new [] {95, 92, 79, 78, 109, 108, 93},                 // base cell 95
            new [] {96, 104, 98, 110, 76, 86, 82},                 // base cell 96
            new [] {97, INVALID_BASE_CELL, 98, 84, 103, 89, 111},  // base cell 97 (pentagon)
            new [] {98, 110, 97, 111, 82, 96, 84},                 // base cell 98
            new [] {99, 80, 105, 88, 106, 90, 113},                // base cell 99
            new [] {100, 102, 83, 87, 108, 114, 92},               // base cell 100
            new [] {101, 102, 107, 112, 81, 85, 94},               // base cell 101
            new [] {102, 101, 87, 85, 114, 112, 100},              // base cell 102
            new [] {103, 91, 97, 89, 116, 105, 111},               // base cell 103
            new [] {104, 107, 110, 115, 86, 94, 96},               // base cell 104
            new [] {105, 88, 103, 91, 113, 99, 116},               // base cell 105
            new [] {106, 93, 99, 90, 117, 109, 113},               // base cell 106
            new [] {107, INVALID_BASE_CELL, 101, 94, 115, 104,
             112},                                // base cell 107 (pentagon)
            new [] {108, 100, 95, 92, 118, 114, 109},    // base cell 108
            new [] {109, 108, 93, 95, 117, 118, 106},    // base cell 109
            new [] {110, 98, 104, 96, 119, 111, 115},    // base cell 110
            new [] {111, 97, 110, 98, 116, 103, 119},    // base cell 111
            new [] {112, 107, 102, 101, 120, 115, 114},  // base cell 112
            new [] {113, 99, 116, 105, 117, 106, 121},   // base cell 113
            new [] {114, 112, 100, 102, 118, 120, 108},  // base cell 114
            new [] {115, 110, 107, 104, 120, 119, 112},  // base cell 115
            new [] {116, 103, 119, 111, 113, 105, 121},  // base cell 116
            new [] {117, INVALID_BASE_CELL, 109, 118, 113, 121,
             106},                                // base cell 117 (pentagon)
            new [] {118, 120, 108, 114, 117, 121, 109},  // base cell 118
            new [] {119, 111, 115, 110, 121, 116, 120},  // base cell 119
            new [] {120, 115, 114, 112, 121, 119, 118},  // base cell 120
            new [] {121, 116, 120, 119, 117, 113, 118},  // base cell 121
        };

        /** @brief Neighboring base cell rotations in each IJK direction.
         *
         * For each base cell, for each direction, the number of 60 degree
         * CCW rotations to the coordinate system of the neighbor is given.
         * -1 indicates there is no neighbor in that direction.
         */
        public static readonly int[][] baseCellNeighbor60CCWRots = {
            new [] {0, 5, 0, 0, 1, 5, 1},   // base cell 0
            new [] {0, 0, 1, 0, 1, 0, 1},   // base cell 1
            new [] {0, 0, 0, 0, 0, 5, 0},   // base cell 2
            new [] {0, 5, 0, 0, 2, 5, 1},   // base cell 3
            new [] {0, -1, 1, 0, 3, 4, 2},  // base cell 4 (pentagon)
            new [] {0, 0, 1, 0, 1, 0, 1},   // base cell 5
            new [] {0, 0, 0, 3, 5, 5, 0},   // base cell 6
            new [] {0, 0, 0, 0, 0, 5, 0},   // base cell 7
            new [] {0, 5, 0, 0, 0, 5, 1},   // base cell 8
            new [] {0, 0, 1, 3, 0, 0, 1},   // base cell 9
            new [] {0, 0, 1, 3, 0, 0, 1},   // base cell 10
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 11
            new [] {0, 5, 0, 0, 3, 5, 1},   // base cell 12
            new [] {0, 0, 1, 0, 1, 0, 1},   // base cell 13
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 14 (pentagon)
            new [] {0, 5, 0, 0, 4, 5, 1},   // base cell 15
            new [] {0, 0, 0, 0, 0, 5, 0},   // base cell 16
            new [] {0, 3, 3, 3, 3, 0, 3},   // base cell 17
            new [] {0, 0, 0, 3, 5, 5, 0},   // base cell 18
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 19
            new [] {0, 3, 3, 3, 0, 3, 0},   // base cell 20
            new [] {0, 0, 0, 3, 5, 5, 0},   // base cell 21
            new [] {0, 0, 1, 0, 1, 0, 1},   // base cell 22
            new [] {0, 3, 3, 3, 0, 3, 0},   // base cell 23
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 24 (pentagon)
            new [] {0, 0, 0, 3, 0, 0, 3},   // base cell 25
            new [] {0, 0, 0, 0, 0, 5, 0},   // base cell 26
            new [] {0, 3, 0, 0, 0, 3, 3},   // base cell 27
            new [] {0, 0, 1, 0, 1, 0, 1},   // base cell 28
            new [] {0, 0, 1, 3, 0, 0, 1},   // base cell 29
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 30
            new [] {0, 0, 0, 0, 0, 5, 0},   // base cell 31
            new [] {0, 3, 3, 3, 3, 0, 3},   // base cell 32
            new [] {0, 0, 1, 3, 0, 0, 1},   // base cell 33
            new [] {0, 3, 3, 3, 3, 0, 3},   // base cell 34
            new [] {0, 0, 3, 0, 3, 0, 3},   // base cell 35
            new [] {0, 0, 0, 3, 0, 0, 3},   // base cell 36
            new [] {0, 3, 0, 0, 0, 3, 3},   // base cell 37
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 38 (pentagon)
            new [] {0, 3, 0, 0, 3, 3, 0},   // base cell 39
            new [] {0, 3, 0, 0, 3, 3, 0},   // base cell 40
            new [] {0, 0, 0, 3, 5, 5, 0},   // base cell 41
            new [] {0, 0, 0, 3, 5, 5, 0},   // base cell 42
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 43
            new [] {0, 0, 1, 3, 0, 0, 1},   // base cell 44
            new [] {0, 0, 3, 0, 0, 3, 3},   // base cell 45
            new [] {0, 0, 0, 3, 0, 3, 0},   // base cell 46
            new [] {0, 3, 3, 3, 0, 3, 0},   // base cell 47
            new [] {0, 3, 3, 3, 0, 3, 0},   // base cell 48
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 49 (pentagon)
            new [] {0, 0, 0, 3, 0, 0, 3},   // base cell 50
            new [] {0, 3, 0, 0, 0, 3, 3},   // base cell 51
            new [] {0, 0, 3, 0, 3, 0, 3},   // base cell 52
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 53
            new [] {0, 0, 3, 0, 3, 0, 3},   // base cell 54
            new [] {0, 0, 3, 0, 0, 3, 3},   // base cell 55
            new [] {0, 3, 3, 3, 0, 0, 3},   // base cell 56
            new [] {0, 0, 0, 3, 0, 3, 0},   // base cell 57
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 58 (pentagon)
            new [] {0, 3, 3, 3, 3, 3, 0},   // base cell 59
            new [] {0, 3, 3, 3, 3, 3, 0},   // base cell 60
            new [] {0, 3, 3, 3, 3, 0, 3},   // base cell 61
            new [] {0, 3, 3, 3, 3, 0, 3},   // base cell 62
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 63 (pentagon)
            new [] {0, 0, 0, 3, 0, 0, 3},   // base cell 64
            new [] {0, 3, 3, 3, 0, 3, 0},   // base cell 65
            new [] {0, 3, 0, 0, 0, 3, 3},   // base cell 66
            new [] {0, 3, 0, 0, 3, 3, 0},   // base cell 67
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 68
            new [] {0, 3, 0, 0, 3, 3, 0},   // base cell 69
            new [] {0, 0, 3, 0, 0, 3, 3},   // base cell 70
            new [] {0, 0, 0, 3, 0, 3, 0},   // base cell 71
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 72 (pentagon)
            new [] {0, 3, 3, 3, 0, 0, 3},   // base cell 73
            new [] {0, 3, 3, 3, 0, 0, 3},   // base cell 74
            new [] {0, 0, 0, 3, 0, 0, 3},   // base cell 75
            new [] {0, 3, 0, 0, 0, 3, 3},   // base cell 76
            new [] {0, 0, 0, 3, 0, 5, 0},   // base cell 77
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 78
            new [] {0, 0, 1, 3, 1, 0, 1},   // base cell 79
            new [] {0, 0, 1, 3, 1, 0, 1},   // base cell 80
            new [] {0, 0, 3, 0, 3, 0, 3},   // base cell 81
            new [] {0, 0, 3, 0, 3, 0, 3},   // base cell 82
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 83 (pentagon)
            new [] {0, 0, 3, 0, 0, 3, 3},   // base cell 84
            new [] {0, 0, 0, 3, 0, 3, 0},   // base cell 85
            new [] {0, 3, 0, 0, 3, 3, 0},   // base cell 86
            new [] {0, 3, 3, 3, 3, 3, 0},   // base cell 87
            new [] {0, 0, 0, 3, 0, 5, 0},   // base cell 88
            new [] {0, 3, 3, 3, 3, 3, 0},   // base cell 89
            new [] {0, 0, 0, 0, 0, 0, 1},   // base cell 90
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 91
            new [] {0, 0, 0, 3, 0, 5, 0},   // base cell 92
            new [] {0, 5, 0, 0, 5, 5, 0},   // base cell 93
            new [] {0, 0, 3, 0, 0, 3, 3},   // base cell 94
            new [] {0, 0, 0, 0, 0, 0, 1},   // base cell 95
            new [] {0, 0, 0, 3, 0, 3, 0},   // base cell 96
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 97 (pentagon)
            new [] {0, 3, 3, 3, 0, 0, 3},   // base cell 98
            new [] {0, 5, 0, 0, 5, 5, 0},   // base cell 99
            new [] {0, 0, 1, 3, 1, 0, 1},   // base cell 100
            new [] {0, 3, 3, 3, 0, 0, 3},   // base cell 101
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 102
            new [] {0, 0, 1, 3, 1, 0, 1},   // base cell 103
            new [] {0, 3, 3, 3, 3, 3, 0},   // base cell 104
            new [] {0, 0, 0, 0, 0, 0, 1},   // base cell 105
            new [] {0, 0, 1, 0, 3, 5, 1},   // base cell 106
            new [] {0, -1, 3, 0, 5, 2, 0},  // base cell 107 (pentagon)
            new [] {0, 5, 0, 0, 5, 5, 0},   // base cell 108
            new [] {0, 0, 1, 0, 4, 5, 1},   // base cell 109
            new [] {0, 3, 3, 3, 0, 0, 0},   // base cell 110
            new [] {0, 0, 0, 3, 0, 5, 0},   // base cell 111
            new [] {0, 0, 0, 3, 0, 5, 0},   // base cell 112
            new [] {0, 0, 1, 0, 2, 5, 1},   // base cell 113
            new [] {0, 0, 0, 0, 0, 0, 1},   // base cell 114
            new [] {0, 0, 1, 3, 1, 0, 1},   // base cell 115
            new [] {0, 5, 0, 0, 5, 5, 0},   // base cell 116
            new [] {0, -1, 1, 0, 3, 4, 2},  // base cell 117 (pentagon)
            new [] {0, 0, 1, 0, 0, 5, 1},   // base cell 118
            new [] {0, 0, 0, 0, 0, 0, 1},   // base cell 119
            new [] {0, 5, 0, 0, 5, 5, 0},   // base cell 120
            new [] {0, 0, 1, 0, 1, 5, 1},   // base cell 121
        };

        /** @brief Resolution 0 base cell lookup table for each face.
         *
         * Given the face number and a resolution 0 ijk+ coordinate in that face's
         * face-centered ijk coordinate system, gives the base cell located at that
         * coordinate and the number of 60 ccw rotations to rotate into that base
         * cell's orientation.
         *
         * Valid lookup coordinates are from (0, 0, 0) to (2, 2, 2).
         *
         * This table can be accessed using the functions `_faceIjkToBaseCell` and
         * `_faceIjkToBaseCellCCWrot60`
         */
        public static readonly BaseCellOrient[][][][] faceIjkBaseCells = {
            new [] {// face 0
                new [] {
                    // i 0
                    new [] {new BaseCellOrient { baseCell=16, ccwRot60=0 }, new BaseCellOrient { baseCell=18, ccwRot60=0 }, new BaseCellOrient { baseCell=24, ccwRot60=0 }},  // j 0
                    new [] {new BaseCellOrient { baseCell=33, ccwRot60=0 }, new BaseCellOrient { baseCell=30, ccwRot60=0 }, new BaseCellOrient { baseCell=32, ccwRot60=3 }},  // j 1
                    new [] {new BaseCellOrient { baseCell=49, ccwRot60=1 }, new BaseCellOrient { baseCell=48, ccwRot60=3 }, new BaseCellOrient { baseCell=50, ccwRot60=3 }}   // j 2
                },
                new [] {
                    // i 1
                    new [] {new BaseCellOrient { baseCell=8, ccwRot60=0 }, new BaseCellOrient { baseCell=5, ccwRot60=5 }, new BaseCellOrient { baseCell=10, ccwRot60=5 }},    // j 0
                    new [] {new BaseCellOrient { baseCell=22, ccwRot60=0 }, new BaseCellOrient { baseCell=16, ccwRot60=0 }, new BaseCellOrient { baseCell=18, ccwRot60=0 }},  // j 1
                    new [] {new BaseCellOrient { baseCell=41, ccwRot60=1 }, new BaseCellOrient { baseCell=33, ccwRot60=0 }, new BaseCellOrient { baseCell=30, ccwRot60=0 }}   // j 2
                },
                new [] {
                    // i 2
                    new [] {new BaseCellOrient { baseCell=4, ccwRot60=0 }, new BaseCellOrient { baseCell=0, ccwRot60=5 }, new BaseCellOrient { baseCell=2, ccwRot60=5 }},    // j 0
                    new [] {new BaseCellOrient { baseCell=15, ccwRot60=1 }, new BaseCellOrient { baseCell=8, ccwRot60=0 }, new BaseCellOrient { baseCell=5, ccwRot60=5 }},   // j 1
                    new [] {new BaseCellOrient { baseCell=31, ccwRot60=1 }, new BaseCellOrient { baseCell=22, ccwRot60=0 }, new BaseCellOrient { baseCell=16, ccwRot60=0 }}  // j 2
                }},
            new [] {// face 1
                new[] {
                    // i 0
                    new [] {new BaseCellOrient { baseCell=2, ccwRot60=0 }, new BaseCellOrient { baseCell=6, ccwRot60=0 }, new BaseCellOrient { baseCell=14, ccwRot60=0 }},    // j 0
                    new [] {new BaseCellOrient { baseCell=10, ccwRot60=0 }, new BaseCellOrient { baseCell=11, ccwRot60=0 }, new BaseCellOrient { baseCell=17, ccwRot60=3 }},  // j 1
                    new [] {new BaseCellOrient { baseCell=24, ccwRot60=1 }, new BaseCellOrient { baseCell=23, ccwRot60=3 }, new BaseCellOrient { baseCell=25, ccwRot60=3 }}   // j 2
                },
                new [] {
                    // i 1
                    new [] {new BaseCellOrient { baseCell=0, ccwRot60=0 }, new BaseCellOrient { baseCell=1, ccwRot60=5 }, new BaseCellOrient { baseCell=9, ccwRot60=5 }},    // j 0
                    new [] {new BaseCellOrient { baseCell=5, ccwRot60=0 }, new BaseCellOrient { baseCell=2, ccwRot60=0 }, new BaseCellOrient { baseCell=6, ccwRot60=0 }},    // j 1
                    new [] {new BaseCellOrient { baseCell=18, ccwRot60=1 }, new BaseCellOrient { baseCell=10, ccwRot60=0 }, new BaseCellOrient { baseCell=11, ccwRot60=0 }}  // j 2
                },
                new [] {
                    // i 2
                    new [] {new BaseCellOrient { baseCell=4, ccwRot60=1 }, new BaseCellOrient { baseCell=3, ccwRot60=5 }, new BaseCellOrient { baseCell=7, ccwRot60=5 }},  // j 0
                    new [] {new BaseCellOrient { baseCell=8, ccwRot60=1 }, new BaseCellOrient { baseCell=0, ccwRot60=0 }, new BaseCellOrient { baseCell=1, ccwRot60=5 }},  // j 1
                    new [] {new BaseCellOrient { baseCell=16, ccwRot60=1 }, new BaseCellOrient { baseCell=5, ccwRot60=0 }, new BaseCellOrient { baseCell=2, ccwRot60=0 }}  // j 2
                }},
            new [] {// face 2
                new [] {
                    // i 0
                    new [] {new BaseCellOrient { baseCell=7, ccwRot60=0 }, new BaseCellOrient { baseCell=21, ccwRot60=0 }, new BaseCellOrient { baseCell=38, ccwRot60=0 }},  // j 0
                    new [] {new BaseCellOrient { baseCell=9, ccwRot60=0 }, new BaseCellOrient { baseCell=19, ccwRot60=0 }, new BaseCellOrient { baseCell=34, ccwRot60=3 }},  // j 1
                    new [] {new BaseCellOrient { baseCell=14, ccwRot60=1 }, new BaseCellOrient { baseCell=20, ccwRot60=3 }, new BaseCellOrient { baseCell=36, ccwRot60=3 }}  // j 2
                },
                new [] {
                    // i 1
                    new [] {new BaseCellOrient { baseCell=3, ccwRot60=0 }, new BaseCellOrient { baseCell=13, ccwRot60=5 }, new BaseCellOrient { baseCell=29, ccwRot60=5 }},  // j 0
                    new [] {new BaseCellOrient { baseCell=1, ccwRot60=0 }, new BaseCellOrient { baseCell=7, ccwRot60=0 }, new BaseCellOrient { baseCell=21, ccwRot60=0 }},   // j 1
                    new [] {new BaseCellOrient { baseCell=6, ccwRot60=1 }, new BaseCellOrient { baseCell=9, ccwRot60=0 }, new BaseCellOrient { baseCell=19, ccwRot60=0 }}    // j 2
                },
                new [] {
                    // i 2
                    new [] {new BaseCellOrient { baseCell=4, ccwRot60=2 }, new BaseCellOrient { baseCell=12, ccwRot60=5 }, new BaseCellOrient { baseCell=26, ccwRot60=5 }},  // j 0
                    new [] {new BaseCellOrient { baseCell=0, ccwRot60=1 }, new BaseCellOrient { baseCell=3, ccwRot60=0 }, new BaseCellOrient { baseCell=13, ccwRot60=5 }},   // j 1
                    new [] {new BaseCellOrient { baseCell=2, ccwRot60=1 }, new BaseCellOrient { baseCell=1, ccwRot60=0 }, new BaseCellOrient { baseCell=7, ccwRot60=0 }}     // j 2
                }},
            new [] {// face 3
                new [] {
                // i 0
                new [] {new BaseCellOrient { baseCell=26, ccwRot60=0 }, new BaseCellOrient { baseCell=42, ccwRot60=0 }, new BaseCellOrient { baseCell=58, ccwRot60=0 }},  // j 0
                new [] {new BaseCellOrient { baseCell=29, ccwRot60=0 }, new BaseCellOrient { baseCell=43, ccwRot60=0 }, new BaseCellOrient { baseCell=62, ccwRot60=3 }},  // j 1
                new [] {new BaseCellOrient { baseCell=38, ccwRot60=1 }, new BaseCellOrient { baseCell=47, ccwRot60=3 }, new BaseCellOrient { baseCell=64, ccwRot60=3 }}   // j 2
                },
                new [] {
                // i 1
                new [] {new BaseCellOrient { baseCell=12, ccwRot60=0 }, new BaseCellOrient { baseCell=28, ccwRot60=5 }, new BaseCellOrient { baseCell=44, ccwRot60=5 }},  // j 0
                new [] {new BaseCellOrient { baseCell=13, ccwRot60=0 }, new BaseCellOrient { baseCell=26, ccwRot60=0 }, new BaseCellOrient { baseCell=42, ccwRot60=0 }},  // j 1
                new [] {new BaseCellOrient { baseCell=21, ccwRot60=1 }, new BaseCellOrient { baseCell=29, ccwRot60=0 }, new BaseCellOrient { baseCell=43, ccwRot60=0 }}   // j 2
                },
                new [] {
                // i 2
                new [] {new BaseCellOrient { baseCell=4, ccwRot60=3 }, new BaseCellOrient { baseCell=15, ccwRot60=5 }, new BaseCellOrient { baseCell=31, ccwRot60=5 }},  // j 0
                new [] {new BaseCellOrient { baseCell=3, ccwRot60=1 }, new BaseCellOrient { baseCell=12, ccwRot60=0 }, new BaseCellOrient { baseCell=28, ccwRot60=5 }},  // j 1
                new [] {new BaseCellOrient { baseCell=7, ccwRot60=1 }, new BaseCellOrient { baseCell=13, ccwRot60=0 }, new BaseCellOrient { baseCell=26, ccwRot60=0 }}   // j 2
                }},
            new [] {// face 4
                new [] {
                // i 0
                new [] {new BaseCellOrient { baseCell=31, ccwRot60=0 }, new BaseCellOrient { baseCell=41, ccwRot60=0 }, new BaseCellOrient { baseCell=49, ccwRot60=0 }},  // j 0
                new [] {new BaseCellOrient { baseCell=44, ccwRot60=0 }, new BaseCellOrient { baseCell=53, ccwRot60=0 }, new BaseCellOrient { baseCell=61, ccwRot60=3 }},  // j 1
                new [] {new BaseCellOrient { baseCell=58, ccwRot60=1 }, new BaseCellOrient { baseCell=65, ccwRot60=3 }, new BaseCellOrient { baseCell=75, ccwRot60=3 }}   // j 2
                },
                new [] {
                // i 1
                new [] {new BaseCellOrient { baseCell=15, ccwRot60=0 }, new BaseCellOrient { baseCell=22, ccwRot60=5 }, new BaseCellOrient { baseCell=33, ccwRot60=5 }},  // j 0
                new [] {new BaseCellOrient { baseCell=28, ccwRot60=0 }, new BaseCellOrient { baseCell=31, ccwRot60=0 }, new BaseCellOrient { baseCell=41, ccwRot60=0 }},  // j 1
                new [] {new BaseCellOrient { baseCell=42, ccwRot60=1 }, new BaseCellOrient { baseCell=44, ccwRot60=0 }, new BaseCellOrient { baseCell=53, ccwRot60=0 }}   // j 2
                },
                new [] {
                // i 2
                new [] {new BaseCellOrient { baseCell=4, ccwRot60=4 }, new BaseCellOrient { baseCell=8, ccwRot60=5 }, new BaseCellOrient { baseCell=16, ccwRot60=5 }},    // j 0
                new [] {new BaseCellOrient { baseCell=12, ccwRot60=1 }, new BaseCellOrient { baseCell=15, ccwRot60=0 }, new BaseCellOrient { baseCell=22, ccwRot60=5 }},  // j 1
                new [] {new BaseCellOrient { baseCell=26, ccwRot60=1 }, new BaseCellOrient { baseCell=28, ccwRot60=0 }, new BaseCellOrient { baseCell=31, ccwRot60=0 }}   // j 2
                }},
            new [] {// face 5
                new [] {
                 // i 0
                 new [] {new BaseCellOrient { baseCell=50, ccwRot60=0 }, new BaseCellOrient { baseCell=48, ccwRot60=0 }, new BaseCellOrient { baseCell=49, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=32, ccwRot60=0 }, new BaseCellOrient { baseCell=30, ccwRot60=3 }, new BaseCellOrient { baseCell=33, ccwRot60=3 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=24, ccwRot60=3 }, new BaseCellOrient { baseCell=18, ccwRot60=3 }, new BaseCellOrient { baseCell=16, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] {new BaseCellOrient { baseCell=70, ccwRot60=0 }, new BaseCellOrient { baseCell=67, ccwRot60=0 }, new BaseCellOrient { baseCell=66, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=52, ccwRot60=3 }, new BaseCellOrient { baseCell=50, ccwRot60=0 }, new BaseCellOrient { baseCell=48, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=37, ccwRot60=3 }, new BaseCellOrient { baseCell=32, ccwRot60=0 }, new BaseCellOrient { baseCell=30, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] {new BaseCellOrient { baseCell=83, ccwRot60=0 }, new BaseCellOrient { baseCell=87, ccwRot60=3 }, new BaseCellOrient { baseCell=85, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=74, ccwRot60=3 }, new BaseCellOrient { baseCell=70, ccwRot60=0 }, new BaseCellOrient { baseCell=67, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=57, ccwRot60=1 }, new BaseCellOrient { baseCell=52, ccwRot60=3 }, new BaseCellOrient { baseCell=50, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 6
                new [] {
                 // i 0
                 new [] {new BaseCellOrient { baseCell=25, ccwRot60=0 }, new BaseCellOrient { baseCell=23, ccwRot60=0 }, new BaseCellOrient { baseCell=24, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=17, ccwRot60=0 }, new BaseCellOrient { baseCell=11, ccwRot60=3 }, new BaseCellOrient { baseCell=10, ccwRot60=3 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=14, ccwRot60=3 }, new BaseCellOrient { baseCell=6, ccwRot60=3 }, new BaseCellOrient { baseCell=2, ccwRot60=3 }}     // j 2
             },
                new [] {
                 // i 1
                 new [] {new BaseCellOrient { baseCell=45, ccwRot60=0 }, new BaseCellOrient { baseCell=39, ccwRot60=0 }, new BaseCellOrient { baseCell=37, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=35, ccwRot60=3 }, new BaseCellOrient { baseCell=25, ccwRot60=0 }, new BaseCellOrient { baseCell=23, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=27, ccwRot60=3 }, new BaseCellOrient { baseCell=17, ccwRot60=0 }, new BaseCellOrient { baseCell=11, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] {new BaseCellOrient { baseCell=63, ccwRot60=0 }, new BaseCellOrient { baseCell=59, ccwRot60=3 }, new BaseCellOrient { baseCell=57, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=56, ccwRot60=3 }, new BaseCellOrient { baseCell=45, ccwRot60=0 }, new BaseCellOrient { baseCell=39, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=46, ccwRot60=3 }, new BaseCellOrient { baseCell=35, ccwRot60=3 }, new BaseCellOrient { baseCell=25, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 7
                new [] {
                 // i 0
                 new [] {new BaseCellOrient { baseCell=36, ccwRot60=0 }, new BaseCellOrient { baseCell=20, ccwRot60=0 }, new BaseCellOrient { baseCell=14, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=34, ccwRot60=0 }, new BaseCellOrient { baseCell=19, ccwRot60=3 }, new BaseCellOrient { baseCell=9, ccwRot60=3 }},   // j 1
                 new [] {new BaseCellOrient { baseCell=38, ccwRot60=3 }, new BaseCellOrient { baseCell=21, ccwRot60=3 }, new BaseCellOrient { baseCell=7, ccwRot60=3 }}    // j 2
             },
                new [] {
                 // i 1
                 new [] {new BaseCellOrient { baseCell=55, ccwRot60=0 }, new BaseCellOrient { baseCell=40, ccwRot60=0 }, new BaseCellOrient { baseCell=27, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=54, ccwRot60=3 }, new BaseCellOrient { baseCell=36, ccwRot60=0 }, new BaseCellOrient { baseCell=20, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=51, ccwRot60=3 }, new BaseCellOrient { baseCell=34, ccwRot60=0 }, new BaseCellOrient { baseCell=19, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] {new BaseCellOrient { baseCell=72, ccwRot60=0 }, new BaseCellOrient { baseCell=60, ccwRot60=3 }, new BaseCellOrient { baseCell=46, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=73, ccwRot60=3 }, new BaseCellOrient { baseCell=55, ccwRot60=0 }, new BaseCellOrient { baseCell=40, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=71, ccwRot60=3 }, new BaseCellOrient { baseCell=54, ccwRot60=3 }, new BaseCellOrient { baseCell=36, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 8
                new [] {
                 // i 0
                 new [] {new BaseCellOrient { baseCell=64, ccwRot60=0 }, new BaseCellOrient { baseCell=47, ccwRot60=0 }, new BaseCellOrient { baseCell=38, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=62, ccwRot60=0 }, new BaseCellOrient { baseCell=43, ccwRot60=3 }, new BaseCellOrient { baseCell=29, ccwRot60=3 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=58, ccwRot60=3 }, new BaseCellOrient { baseCell=42, ccwRot60=3 }, new BaseCellOrient { baseCell=26, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] {new BaseCellOrient { baseCell=84, ccwRot60=0 }, new BaseCellOrient { baseCell=69, ccwRot60=0 }, new BaseCellOrient { baseCell=51, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=82, ccwRot60=3 }, new BaseCellOrient { baseCell=64, ccwRot60=0 }, new BaseCellOrient { baseCell=47, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=76, ccwRot60=3 }, new BaseCellOrient { baseCell=62, ccwRot60=0 }, new BaseCellOrient { baseCell=43, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] {new BaseCellOrient { baseCell=97, ccwRot60=0 }, new BaseCellOrient { baseCell=89, ccwRot60=3 }, new BaseCellOrient { baseCell=71, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=98, ccwRot60=3 }, new BaseCellOrient { baseCell=84, ccwRot60=0 }, new BaseCellOrient { baseCell=69, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=96, ccwRot60=3 }, new BaseCellOrient { baseCell=82, ccwRot60=3 }, new BaseCellOrient { baseCell=64, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 9
                new [] {
                 // i 0
                 new [] {new BaseCellOrient { baseCell=75, ccwRot60=0 }, new BaseCellOrient { baseCell=65, ccwRot60=0 }, new BaseCellOrient { baseCell=58, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=61, ccwRot60=0 }, new BaseCellOrient { baseCell=53, ccwRot60=3 }, new BaseCellOrient { baseCell=44, ccwRot60=3 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=49, ccwRot60=3 }, new BaseCellOrient { baseCell=41, ccwRot60=3 }, new BaseCellOrient { baseCell=31, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] {new BaseCellOrient { baseCell=94, ccwRot60=0 }, new BaseCellOrient { baseCell=86, ccwRot60=0 }, new BaseCellOrient { baseCell=76, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=81, ccwRot60=3 }, new BaseCellOrient { baseCell=75, ccwRot60=0 }, new BaseCellOrient { baseCell=65, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=66, ccwRot60=3 }, new BaseCellOrient { baseCell=61, ccwRot60=0 }, new BaseCellOrient { baseCell=53, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] {new BaseCellOrient { baseCell=107, ccwRot60=0 }, new BaseCellOrient { baseCell=104, ccwRot60=3 }, new BaseCellOrient { baseCell=96, ccwRot60=3 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=101, ccwRot60=3 }, new BaseCellOrient { baseCell=94, ccwRot60=0 }, new BaseCellOrient { baseCell=86, ccwRot60=0 }},   // j 1
                 new [] {new BaseCellOrient { baseCell=85, ccwRot60=3 }, new BaseCellOrient { baseCell=81, ccwRot60=3 }, new BaseCellOrient { baseCell=75, ccwRot60=0 }}     // j 2
             }},
            new [] {// face 10
                new [] {
                 // i 0
                 new [] { new BaseCellOrient { baseCell=57, ccwRot60=0 }, new BaseCellOrient { baseCell=59, ccwRot60=0 }, new BaseCellOrient { baseCell=63, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=74, ccwRot60=0 }, new BaseCellOrient { baseCell=78, ccwRot60=3 }, new BaseCellOrient { baseCell=79, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=83, ccwRot60=3 }, new BaseCellOrient { baseCell=92, ccwRot60=3 }, new BaseCellOrient { baseCell=95, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] { new BaseCellOrient { baseCell=37, ccwRot60=0 }, new BaseCellOrient { baseCell=39, ccwRot60=3 }, new BaseCellOrient { baseCell=45, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=52, ccwRot60=0 }, new BaseCellOrient { baseCell=57, ccwRot60=0 }, new BaseCellOrient { baseCell=59, ccwRot60=0 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=70, ccwRot60=3 }, new BaseCellOrient { baseCell=74, ccwRot60=0 }, new BaseCellOrient { baseCell=78, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] { new BaseCellOrient { baseCell=24, ccwRot60=0 }, new BaseCellOrient { baseCell=23, ccwRot60=3 }, new BaseCellOrient { baseCell=25, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=32, ccwRot60=3 }, new BaseCellOrient { baseCell=37, ccwRot60=0 }, new BaseCellOrient { baseCell=39, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=50, ccwRot60=3 }, new BaseCellOrient { baseCell=52, ccwRot60=0 }, new BaseCellOrient { baseCell=57, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 11
                new [] {
                 // i 0
                 new [] { new BaseCellOrient { baseCell=46, ccwRot60=0 }, new BaseCellOrient { baseCell=60, ccwRot60=0 }, new BaseCellOrient { baseCell=72, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=56, ccwRot60=0 }, new BaseCellOrient { baseCell=68, ccwRot60=3 }, new BaseCellOrient { baseCell=80, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=63, ccwRot60=3 }, new BaseCellOrient { baseCell=77, ccwRot60=3 }, new BaseCellOrient { baseCell=90, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] { new BaseCellOrient { baseCell=27, ccwRot60=0 }, new BaseCellOrient { baseCell=40, ccwRot60=3 }, new BaseCellOrient { baseCell=55, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=35, ccwRot60=0 }, new BaseCellOrient { baseCell=46, ccwRot60=0 }, new BaseCellOrient { baseCell=60, ccwRot60=0 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=45, ccwRot60=3 }, new BaseCellOrient { baseCell=56, ccwRot60=0 }, new BaseCellOrient { baseCell=68, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] { new BaseCellOrient { baseCell=14, ccwRot60=0 }, new BaseCellOrient { baseCell=20, ccwRot60=3 }, new BaseCellOrient { baseCell=36, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=17, ccwRot60=3 }, new BaseCellOrient { baseCell=27, ccwRot60=0 }, new BaseCellOrient { baseCell=40, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=25, ccwRot60=3 }, new BaseCellOrient { baseCell=35, ccwRot60=0 }, new BaseCellOrient { baseCell=46, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 12
                new [] {
                 // i 0
                 new [] { new BaseCellOrient { baseCell=71, ccwRot60=0 }, new BaseCellOrient { baseCell=89, ccwRot60=0 }, new BaseCellOrient { baseCell=97, ccwRot60=3 }},   // j 0
                 new [] { new BaseCellOrient { baseCell=73, ccwRot60=0 }, new BaseCellOrient { baseCell=91, ccwRot60=3 }, new BaseCellOrient { baseCell=103, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=72, ccwRot60=3 }, new BaseCellOrient { baseCell=88, ccwRot60=3 }, new BaseCellOrient { baseCell=105, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] { new BaseCellOrient { baseCell=51, ccwRot60=0 }, new BaseCellOrient { baseCell=69, ccwRot60=3 }, new BaseCellOrient { baseCell=84, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=54, ccwRot60=0 }, new BaseCellOrient { baseCell=71, ccwRot60=0 }, new BaseCellOrient { baseCell=89, ccwRot60=0 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=55, ccwRot60=3 }, new BaseCellOrient { baseCell=73, ccwRot60=0 }, new BaseCellOrient { baseCell=91, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] { new BaseCellOrient { baseCell=38, ccwRot60=0 }, new BaseCellOrient { baseCell=47, ccwRot60=3 }, new BaseCellOrient { baseCell=64, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=34, ccwRot60=3 }, new BaseCellOrient { baseCell=51, ccwRot60=0 }, new BaseCellOrient { baseCell=69, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=36, ccwRot60=3 }, new BaseCellOrient { baseCell=54, ccwRot60=0 }, new BaseCellOrient { baseCell=71, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 13
                new [] {
                 // i 0
                 new [] { new BaseCellOrient { baseCell=96, ccwRot60=0 }, new BaseCellOrient { baseCell=104, ccwRot60=0 }, new BaseCellOrient { baseCell=107, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=98, ccwRot60=0 }, new BaseCellOrient { baseCell=110, ccwRot60=3 }, new BaseCellOrient { baseCell=115, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=97, ccwRot60=3 }, new BaseCellOrient { baseCell=111, ccwRot60=3 }, new BaseCellOrient { baseCell=119, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] { new BaseCellOrient { baseCell=76, ccwRot60=0 }, new BaseCellOrient { baseCell=86, ccwRot60=3 }, new BaseCellOrient { baseCell=94, ccwRot60=3 }},   // j 0
                 new [] { new BaseCellOrient { baseCell=82, ccwRot60=0 }, new BaseCellOrient { baseCell=96, ccwRot60=0 }, new BaseCellOrient { baseCell=104, ccwRot60=0 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=84, ccwRot60=3 }, new BaseCellOrient { baseCell=98, ccwRot60=0 }, new BaseCellOrient { baseCell=110, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] { new BaseCellOrient { baseCell=58, ccwRot60=0 }, new BaseCellOrient { baseCell=65, ccwRot60=3 }, new BaseCellOrient { baseCell=75, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=62, ccwRot60=3 }, new BaseCellOrient { baseCell=76, ccwRot60=0 }, new BaseCellOrient { baseCell=86, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=64, ccwRot60=3 }, new BaseCellOrient { baseCell=82, ccwRot60=0 }, new BaseCellOrient { baseCell=96, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 14
                new [] {
                 // i 0
                 new [] { new BaseCellOrient { baseCell=85, ccwRot60=0 }, new BaseCellOrient { baseCell=87, ccwRot60=0 }, new BaseCellOrient { baseCell=83, ccwRot60=3 }},     // j 0
                 new [] { new BaseCellOrient { baseCell=101, ccwRot60=0 }, new BaseCellOrient { baseCell=102, ccwRot60=3 }, new BaseCellOrient { baseCell=100, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=107, ccwRot60=3 }, new BaseCellOrient { baseCell=112, ccwRot60=3 }, new BaseCellOrient { baseCell=114, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] { new BaseCellOrient { baseCell=66, ccwRot60=0 }, new BaseCellOrient { baseCell=67, ccwRot60=3 }, new BaseCellOrient { baseCell=70, ccwRot60=3 }},   // j 0
                 new [] { new BaseCellOrient { baseCell=81, ccwRot60=0 }, new BaseCellOrient { baseCell=85, ccwRot60=0 }, new BaseCellOrient { baseCell=87, ccwRot60=0 }},   // j 1
                 new [] { new BaseCellOrient { baseCell=94, ccwRot60=3 }, new BaseCellOrient { baseCell=101, ccwRot60=0 }, new BaseCellOrient { baseCell=102, ccwRot60=3 }}  // j 2
             },
                new [] {
                 // i 2
                 new [] { new BaseCellOrient { baseCell=49, ccwRot60=0 }, new BaseCellOrient { baseCell=48, ccwRot60=3 }, new BaseCellOrient { baseCell=50, ccwRot60=3 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=61, ccwRot60=3 }, new BaseCellOrient { baseCell=66, ccwRot60=0 }, new BaseCellOrient { baseCell=67, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=75, ccwRot60=3 }, new BaseCellOrient { baseCell=81, ccwRot60=0 }, new BaseCellOrient { baseCell=85, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 15
                new [] {
                 // i 0
                 new [] { new BaseCellOrient { baseCell=95, ccwRot60=0 }, new BaseCellOrient { baseCell=92, ccwRot60=0 }, new BaseCellOrient { baseCell=83, ccwRot60=0 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=79, ccwRot60=0 }, new BaseCellOrient { baseCell=78, ccwRot60=0 }, new BaseCellOrient { baseCell=74, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=63, ccwRot60=1 }, new BaseCellOrient { baseCell=59, ccwRot60=3 }, new BaseCellOrient { baseCell=57, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] { new BaseCellOrient { baseCell=109, ccwRot60=0 }, new BaseCellOrient { baseCell=108, ccwRot60=0 }, new BaseCellOrient { baseCell=100, ccwRot60=5 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=93, ccwRot60=1 }, new BaseCellOrient { baseCell=95, ccwRot60=0 }, new BaseCellOrient { baseCell=92, ccwRot60=0 }},     // j 1
                 new [] { new BaseCellOrient { baseCell=77, ccwRot60=1 }, new BaseCellOrient { baseCell=79, ccwRot60=0 }, new BaseCellOrient { baseCell=78, ccwRot60=0 }}      // j 2
             },
                new [] {
                 // i 2
                 new [] { new BaseCellOrient { baseCell=117, ccwRot60=4 }, new BaseCellOrient { baseCell=118, ccwRot60=5 }, new BaseCellOrient { baseCell=114, ccwRot60=5 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=106, ccwRot60=1 }, new BaseCellOrient { baseCell=109, ccwRot60=0 }, new BaseCellOrient { baseCell=108, ccwRot60=0 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=90, ccwRot60=1 }, new BaseCellOrient { baseCell=93, ccwRot60=1 }, new BaseCellOrient { baseCell=95, ccwRot60=0 }}      // j 2
             }},
            new [] {// face 16
                new [] {
                 // i 0
                 new [] { new BaseCellOrient { baseCell=90, ccwRot60=0 }, new BaseCellOrient { baseCell=77, ccwRot60=0 }, new BaseCellOrient { baseCell=63, ccwRot60=0 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=80, ccwRot60=0 }, new BaseCellOrient { baseCell=68, ccwRot60=0 }, new BaseCellOrient { baseCell=56, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=72, ccwRot60=1 }, new BaseCellOrient { baseCell=60, ccwRot60=3 }, new BaseCellOrient { baseCell=46, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] { new BaseCellOrient { baseCell=106, ccwRot60=0 }, new BaseCellOrient { baseCell=93, ccwRot60=0 }, new BaseCellOrient { baseCell=79, ccwRot60=5 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=99, ccwRot60=1 }, new BaseCellOrient { baseCell=90, ccwRot60=0 }, new BaseCellOrient { baseCell=77, ccwRot60=0 }},   // j 1
                 new [] { new BaseCellOrient { baseCell=88, ccwRot60=1 }, new BaseCellOrient { baseCell=80, ccwRot60=0 }, new BaseCellOrient { baseCell=68, ccwRot60=0 }}    // j 2
             },
                new [] {
                 // i 2
                 new [] { new BaseCellOrient { baseCell=117, ccwRot60=3 }, new BaseCellOrient { baseCell=109, ccwRot60=5 }, new BaseCellOrient { baseCell=95, ccwRot60=5 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=113, ccwRot60=1 }, new BaseCellOrient { baseCell=106, ccwRot60=0 }, new BaseCellOrient { baseCell=93, ccwRot60=0 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=105, ccwRot60=1 }, new BaseCellOrient { baseCell=99, ccwRot60=1 }, new BaseCellOrient { baseCell=90, ccwRot60=0 }}    // j 2
             }},
            new [] {// face 17
                new [] {
                 // i 0
                 new [] { new BaseCellOrient { baseCell=105, ccwRot60=0 }, new BaseCellOrient { baseCell=88, ccwRot60=0 }, new BaseCellOrient { baseCell=72, ccwRot60=0 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=103, ccwRot60=0 }, new BaseCellOrient { baseCell=91, ccwRot60=0 }, new BaseCellOrient { baseCell=73, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=97, ccwRot60=1 }, new BaseCellOrient { baseCell=89, ccwRot60=3 }, new BaseCellOrient { baseCell=71, ccwRot60=3 }}    // j 2
             },
                new [] {
                 // i 1
                 new [] { new BaseCellOrient { baseCell=113, ccwRot60=0 }, new BaseCellOrient { baseCell=99, ccwRot60=0 }, new BaseCellOrient { baseCell=80, ccwRot60=5 }},   // j 0
                 new [] { new BaseCellOrient { baseCell=116, ccwRot60=1 }, new BaseCellOrient { baseCell=105, ccwRot60=0 }, new BaseCellOrient { baseCell=88, ccwRot60=0 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=111, ccwRot60=1 }, new BaseCellOrient { baseCell=103, ccwRot60=0 }, new BaseCellOrient { baseCell=91, ccwRot60=0 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] { new BaseCellOrient { baseCell=117, ccwRot60=2 }, new BaseCellOrient { baseCell=106, ccwRot60=5 }, new BaseCellOrient { baseCell=90, ccwRot60=5 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=121, ccwRot60=1 }, new BaseCellOrient { baseCell=113, ccwRot60=0 }, new BaseCellOrient { baseCell=99, ccwRot60=0 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=119, ccwRot60=1 }, new BaseCellOrient { baseCell=116, ccwRot60=1 }, new BaseCellOrient { baseCell=105, ccwRot60=0 }}  // j 2
             }},
            new [] {// face 18
                new [] {
                 // i 0
                 new [] { new BaseCellOrient { baseCell=119, ccwRot60=0 }, new BaseCellOrient { baseCell=111, ccwRot60=0 }, new BaseCellOrient { baseCell=97, ccwRot60=0 }},  // j 0
                 new [] { new BaseCellOrient { baseCell=115, ccwRot60=0 }, new BaseCellOrient { baseCell=110, ccwRot60=0 }, new BaseCellOrient { baseCell=98, ccwRot60=3 }},  // j 1
                 new [] { new BaseCellOrient { baseCell=107, ccwRot60=1 }, new BaseCellOrient { baseCell=104, ccwRot60=3 }, new BaseCellOrient { baseCell=96, ccwRot60=3 }}   // j 2
             },
                new [] {
                 // i 1
                 new [] {new BaseCellOrient { baseCell=121, ccwRot60=0 }, new BaseCellOrient { baseCell=116, ccwRot60=0 }, new BaseCellOrient { baseCell=103, ccwRot60=5 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=120, ccwRot60=1 }, new BaseCellOrient { baseCell=119, ccwRot60=0 }, new BaseCellOrient { baseCell=111, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=112, ccwRot60=1 }, new BaseCellOrient { baseCell=115, ccwRot60=0 }, new BaseCellOrient { baseCell=110, ccwRot60=0 }}   // j 2
             },
                new [] {
                 // i 2
                 new [] {new BaseCellOrient { baseCell=117, ccwRot60=1 }, new BaseCellOrient { baseCell=113, ccwRot60=5 }, new BaseCellOrient { baseCell=105, ccwRot60=5 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=118, ccwRot60=1 }, new BaseCellOrient { baseCell=121, ccwRot60=0 }, new BaseCellOrient { baseCell=116, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=114, ccwRot60=1 }, new BaseCellOrient { baseCell=120, ccwRot60=1 }, new BaseCellOrient { baseCell=119, ccwRot60=0 }}   // j 2
             }},
            new [] {// face 19
                new [] {
                 // i 0
                 new [] {new BaseCellOrient { baseCell=114, ccwRot60=0 }, new BaseCellOrient { baseCell=112, ccwRot60=0 }, new BaseCellOrient { baseCell=107, ccwRot60=0 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=100, ccwRot60=0 }, new BaseCellOrient { baseCell=102, ccwRot60=0 }, new BaseCellOrient { baseCell=101, ccwRot60=3 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=83, ccwRot60=1 }, new BaseCellOrient { baseCell=87, ccwRot60=3 }, new BaseCellOrient { baseCell=85, ccwRot60=3 }}      // j 2
             },
                new [] {
                 // i 1
                 new [] {new BaseCellOrient { baseCell=118, ccwRot60=0 }, new BaseCellOrient { baseCell=120, ccwRot60=0 }, new BaseCellOrient { baseCell=115, ccwRot60=5 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=108, ccwRot60=1 }, new BaseCellOrient { baseCell=114, ccwRot60=0 }, new BaseCellOrient { baseCell=112, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=92, ccwRot60=1 }, new BaseCellOrient { baseCell=100, ccwRot60=0 }, new BaseCellOrient { baseCell=102, ccwRot60=0 }}    // j 2
             },
                new [] {
                 // i 2
                 new [] {new BaseCellOrient { baseCell=117, ccwRot60=0 }, new BaseCellOrient { baseCell=121, ccwRot60=5 }, new BaseCellOrient { baseCell=119, ccwRot60=5 }},  // j 0
                 new [] {new BaseCellOrient { baseCell=109, ccwRot60=1 }, new BaseCellOrient { baseCell=118, ccwRot60=0 }, new BaseCellOrient { baseCell=120, ccwRot60=0 }},  // j 1
                 new [] {new BaseCellOrient { baseCell=95, ccwRot60=1 }, new BaseCellOrient { baseCell=108, ccwRot60=1 }, new BaseCellOrient { baseCell=114, ccwRot60=0 }}    // j 2
             }}};

        /** @brief Resolution 0 base cell data table.
         *
         * For each base cell, gives the "home" face and ijk+ coordinates on that face,
         * whether or not the base cell is a pentagon. Additionally, if the base cell
         * is a pentagon, the two cw offset rotation adjacent faces are given (-1
         * indicates that no cw offset rotation faces exist for this base cell).
         */
        public static readonly BaseCellData[] baseCellData = {

            new BaseCellData {homeFijk={face=1,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 0
            new BaseCellData {homeFijk={face=2,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 1
            new BaseCellData {homeFijk={face=1,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 2
            new BaseCellData {homeFijk={face=2,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 3
            new BaseCellData {homeFijk={face=0,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{-1, -1}},   // base cell 4
            new BaseCellData {homeFijk={face=1,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 5
            new BaseCellData {homeFijk={face=1,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 6
            new BaseCellData {homeFijk={face=2,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 7
            new BaseCellData {homeFijk={face=0,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 8
            new BaseCellData {homeFijk={face=2,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 9
            new BaseCellData {homeFijk={face=1,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 10
            new BaseCellData {homeFijk={face=1,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 11
            new BaseCellData {homeFijk={face=3,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 12
            new BaseCellData {homeFijk={face=3,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 13
            new BaseCellData {homeFijk={face=11,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{2, 6}},    // base cell 14
            new BaseCellData {homeFijk={face=4,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 15
            new BaseCellData {homeFijk={face=0,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 16
            new BaseCellData {homeFijk={face=6,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 17
            new BaseCellData {homeFijk={face=0,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 18
            new BaseCellData {homeFijk={face=2,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 19
            new BaseCellData {homeFijk={face=7,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 20
            new BaseCellData {homeFijk={face=2,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 21
            new BaseCellData {homeFijk={face=0,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 22
            new BaseCellData {homeFijk={face=6,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 23
            new BaseCellData {homeFijk={face=10,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{1, 5}},    // base cell 24
            new BaseCellData {homeFijk={face=6,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 25
            new BaseCellData {homeFijk={face=3,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 26
            new BaseCellData {homeFijk={face=11,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 27
            new BaseCellData {homeFijk={face=4,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 28
            new BaseCellData {homeFijk={face=3,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 29
            new BaseCellData {homeFijk={face=0,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 30
            new BaseCellData {homeFijk={face=4,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 31
            new BaseCellData {homeFijk={face=5,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 32
            new BaseCellData {homeFijk={face=0,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 33
            new BaseCellData {homeFijk={face=7,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 34
            new BaseCellData {homeFijk={face=11,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 35
            new BaseCellData {homeFijk={face=7,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 36
            new BaseCellData {homeFijk={face=10,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 37
            new BaseCellData {homeFijk={face=12,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{3, 7}},    // base cell 38
            new BaseCellData {homeFijk={face=6,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 39
            new BaseCellData {homeFijk={face=7,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 40
            new BaseCellData {homeFijk={face=4,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 41
            new BaseCellData {homeFijk={face=3,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 42
            new BaseCellData {homeFijk={face=3,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 43
            new BaseCellData {homeFijk={face=4,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 44
            new BaseCellData {homeFijk={face=6,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 45
            new BaseCellData {homeFijk={face=11,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 46
            new BaseCellData {homeFijk={face=8,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 47
            new BaseCellData {homeFijk={face=5,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 48
            new BaseCellData {homeFijk={face=14,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{0, 9}},    // base cell 49
            new BaseCellData {homeFijk={face=5,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 50
            new BaseCellData {homeFijk={face=12,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 51
            new BaseCellData {homeFijk={face=10,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 52
            new BaseCellData {homeFijk={face=4,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 53
            new BaseCellData {homeFijk={face=12,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 54
            new BaseCellData {homeFijk={face=7,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 55
            new BaseCellData {homeFijk={face=11,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 56
            new BaseCellData {homeFijk={face=10,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 57
            new BaseCellData {homeFijk={face=13,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{4, 8}},    // base cell 58
            new BaseCellData {homeFijk={face=10,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 59
            new BaseCellData {homeFijk={face=11,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 60
            new BaseCellData {homeFijk={face=9,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 61
            new BaseCellData {homeFijk={face=8,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 62
            new BaseCellData {homeFijk={face=6,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{11, 15}},   // base cell 63
            new BaseCellData {homeFijk={face=8,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 64
            new BaseCellData {homeFijk={face=9,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 65
            new BaseCellData {homeFijk={face=14,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 66
            new BaseCellData {homeFijk={face=5,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 67
            new BaseCellData {homeFijk={face=16,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 68
            new BaseCellData {homeFijk={face=8,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 69
            new BaseCellData {homeFijk={face=5,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 70
            new BaseCellData {homeFijk={face=12,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 71
            new BaseCellData {homeFijk={face=7,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{12, 16}},   // base cell 72
            new BaseCellData {homeFijk={face=12,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 73
            new BaseCellData {homeFijk={face=10,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 74
            new BaseCellData {homeFijk={face=9,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 75
            new BaseCellData {homeFijk={face=13,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 76
            new BaseCellData {homeFijk={face=16,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 77
            new BaseCellData {homeFijk={face=15,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 78
            new BaseCellData {homeFijk={face=15,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 79
            new BaseCellData {homeFijk={face=16,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 80
            new BaseCellData {homeFijk={face=14,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 81
            new BaseCellData {homeFijk={face=13,coord=new CoordIJK{i=1,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 82
            new BaseCellData {homeFijk={face=5,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{10, 19}},   // base cell 83
            new BaseCellData {homeFijk={face=8,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 84
            new BaseCellData {homeFijk={face=14,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 85
            new BaseCellData {homeFijk={face=9,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 86
            new BaseCellData {homeFijk={face=14,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 87
            new BaseCellData {homeFijk={face=17,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 88
            new BaseCellData {homeFijk={face=12,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 89
            new BaseCellData {homeFijk={face=16,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 90
            new BaseCellData {homeFijk={face=17,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 91
            new BaseCellData {homeFijk={face=15,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 92
            new BaseCellData {homeFijk={face=16,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 93
            new BaseCellData {homeFijk={face=9,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},     // base cell 94
            new BaseCellData {homeFijk={face=15,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 95
            new BaseCellData {homeFijk={face=13,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 96
            new BaseCellData {homeFijk={face=8,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{13, 17}},   // base cell 97
            new BaseCellData {homeFijk={face=13,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 98
            new BaseCellData {homeFijk={face=17,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 99
            new BaseCellData {homeFijk={face=19,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 100
            new BaseCellData {homeFijk={face=14,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 101
            new BaseCellData {homeFijk={face=19,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 102
            new BaseCellData {homeFijk={face=17,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 103
            new BaseCellData {homeFijk={face=13,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 104
            new BaseCellData {homeFijk={face=17,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 105
            new BaseCellData {homeFijk={face=16,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 106
            new BaseCellData {homeFijk={face=9,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{14, 18}},   // base cell 107
            new BaseCellData {homeFijk={face=15,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 108
            new BaseCellData {homeFijk={face=15,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 109
            new BaseCellData {homeFijk={face=18,coord=new CoordIJK{i=0,j=1,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 110
            new BaseCellData {homeFijk={face=18,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 111
            new BaseCellData {homeFijk={face=19,coord=new CoordIJK{i=0,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 112
            new BaseCellData {homeFijk={face=17,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 113
            new BaseCellData {homeFijk={face=19,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 114
            new BaseCellData {homeFijk={face=18,coord=new CoordIJK{i=0,j=1,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 115
            new BaseCellData {homeFijk={face=18,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 116
            new BaseCellData {homeFijk={face=19,coord=new CoordIJK{i=2,j=0,k=0}},isPentagon=true,cwOffsetPent=new []{-1, -1}},  // base cell 117
            new BaseCellData {homeFijk={face=19,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 118
            new BaseCellData {homeFijk={face=18,coord=new CoordIJK{i=0,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 119
            new BaseCellData {homeFijk={face=19,coord=new CoordIJK{i=1,j=0,k=1}},isPentagon=false,cwOffsetPent=new []{0, 0}},    // base cell 120
            new BaseCellData {homeFijk={face=18,coord=new CoordIJK{i=1,j=0,k=0}},isPentagon=false,cwOffsetPent=new []{0, 0}}     // base cell 121
        };

        /** @brief Return whether or not the indicated base cell is a pentagon. */
        public static bool _isBaseCellPentagon(int baseCell) {
            return baseCellData[baseCell].isPentagon;
        }

        /** @brief Return whether the indicated base cell is a pentagon where all
         * neighbors are oriented towards it. */
        public static bool _isBaseCellPolarPentagon(int baseCell) {
            return baseCell == 4 || baseCell == 117;
        }

        /** @brief Find base cell given FaceIJK.
         *
         * Given the face number and a resolution 0 ijk+ coordinate in that face's
         * face-centered ijk coordinate system, return the base cell located at that
         * coordinate.
         *
         * Valid ijk+ lookup coordinates are from (0, 0, 0) to (2, 2, 2).
         */
        public static int _faceIjkToBaseCell(in FaceIJK h) {
            return faceIjkBaseCells[h.face][h.coord.i][h.coord.j][h.coord.k]
                .baseCell;
        }

        /** @brief Find base cell given FaceIJK.
         *
         * Given the face number and a resolution 0 ijk+ coordinate in that face's
         * face-centered ijk coordinate system, return the number of 60' ccw rotations
         * to rotate into the coordinate system of the base cell at that coordinates.
         *
         * Valid ijk+ lookup coordinates are from (0, 0, 0) to (2, 2, 2).
         */
        public static int _faceIjkToBaseCellCCWrot60(in FaceIJK h) {
            return faceIjkBaseCells[h.face][h.coord.i][h.coord.j][h.coord.k]
                .ccwRot60;
        }

        /** @brief Find the FaceIJK given a base cell.
         */
        public static void _baseCellToFaceIjk(int baseCell, ref FaceIJK h) {
            h = baseCellData[baseCell].homeFijk;
        }

        /** @brief Return whether or not the tested face is a cw offset face.
         */
        public static bool _baseCellIsCwOffset(int baseCell, int testFace) {
            return baseCellData[baseCell].cwOffsetPent[0] == testFace ||
                   baseCellData[baseCell].cwOffsetPent[1] == testFace;
        }

        /** @brief Return the neighboring base cell in the given direction.
         */
        public static int _getBaseCellNeighbor(int baseCell, Direction dir) {
            return baseCellNeighbors[baseCell][(int) dir];
        }

        /** @brief Return the direction from the origin base cell to the neighbor.
         * Returns INVALID_DIGIT if the base cells are not neighbors.
         */
        public static Direction _getBaseCellDirection(int originBaseCell, int neighboringBaseCell) {
            for (Direction dir = Direction.CENTER_DIGIT; dir < Direction.NUM_DIGITS; dir++) {
                int testBaseCell = _getBaseCellNeighbor(originBaseCell, dir);
                if (testBaseCell == neighboringBaseCell) {
                    return dir;
                }
            }
            return Direction.INVALID_DIGIT;
        }

        /**
         * res0IndexCount returns the number of resolution 0 indexes
         *
         * @return int count of resolution 0 indexes
         */
        public static int res0IndexCount() { return Constants.NUM_BASE_CELLS; }

        /**
         * getRes0Indexes generates all base cells storing them into the provided
         * memory pointer. Buffer must be of size NUM_BASE_CELLS * sizeof(H3Index).
         *
         * @param out H3Index* the memory to store the resulting base cells in
         */
        public static void getRes0Indexes(H3Index[] indexes) {
            for (int bc = 0; bc < Constants.NUM_BASE_CELLS; bc++) {
                H3Index baseCell = H3Index.H3_INIT;
                H3Index.H3_SET_MODE(ref baseCell, Constants.H3_HEXAGON_MODE);
                H3Index.H3_SET_BASE_CELL(ref baseCell, bc);
                indexes[bc] = baseCell;
            }
        }
    }
}
