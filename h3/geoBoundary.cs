namespace h3
{
    /** @struct GeoBoundary
        @brief cell boundary in latitude/longitude
    */
    public class GeoBoundary {
        public int numVerts;                          ///< number of vertices
        public GeoCoord[] verts;

        ///< vertices in ccw order

        public GeoBoundary()
        {
            numVerts = 0;
            verts = new GeoCoord[MAX_CELL_BNDRY_VERTS];
        }
        
        /** Maximum number of cell boundary vertices; worst case is pentagon:
         *  5 original verts + 5 edge crossings
         */
        const int MAX_CELL_BNDRY_VERTS = 10;
    };
}