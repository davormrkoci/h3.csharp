namespace h3
{
    public class mathExtensions
    {
        /**
         * _ipow does integer exponentiation efficiently. Taken from StackOverflow.
         *
         * @param base the integer base
         * @param exp the integer exponent
         *
         * @return the exponentiated value
         */
        public static int _ipow(int basen, int exp) {
            int result = 1;
            while (exp != 0) {
                if ((exp & 1) != 0) result *= basen;
                exp >>= 1;
                basen *= basen;
            }

            return result;
        }
    }
}