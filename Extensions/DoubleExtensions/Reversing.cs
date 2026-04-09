namespace net.erlenddahl.Extensions.DoubleExtensions
{
    public static class Reversing { 

        /// <summary>
        /// Will reverse the given number (return -N if it is positive, or +N if it is negative).
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static double Reverse(this double num)
        {
            return num * -1;
        }

        /// <summary>
        /// Will reverse the given number (return -N if it is positive, or +N if it is negative) IF the given expression is true.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static double ReverseIf(this double num, bool expression)
        {
            if (expression)
                return num * -1;
            return num;
        }
    }
}