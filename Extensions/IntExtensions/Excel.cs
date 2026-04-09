using System;
using System.IO;

namespace net.erlenddahl.Extensions.IntExtensions
{
    public static class Excel
    {
        /// <summary>
        /// Returns the "Excel column" equivalent of the given number. 0 is A, 1 is B, etc. After Z, it will start with double characters, AA, AB, ..., BA, BB, etc.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToExcelColumn(this int num)
        {
            if (num < 0) throw new InvalidDataException("Cannot generate Excel column for numbers less than zero (given: " + num + ").");

            //Source: https://stackoverflow.com/questions/181596/how-to-convert-a-column-number-eg-127-into-an-excel-column-eg-aa

            //Increment with one to make A = 0 instead of A = 1.
            num++;
            var columnName = string.Empty;

            while (num > 0)
            {
                var modulo = (num - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                num = (num - modulo) / 26;
            }

            return columnName;
        }
    }
}