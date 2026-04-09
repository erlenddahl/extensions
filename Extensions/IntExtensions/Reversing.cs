namespace net.erlenddahl.Extensions.IntExtensions
{
    public static class Reversing
    {


        /// <summary>
        /// Will reverse the given number (return -N if it is positive, or +N if it is negative).
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int Reverse(this int num)
        {
            return num * -1;
        }

        /// <summary>
        /// Will reverse the given number (return -N if it is positive, or +N if it is negative) IF the given expression is true.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int ReverseIf(this int num, bool expression)
        {
            if (expression)
                return num * -1;
            return num;
        }
    }
}