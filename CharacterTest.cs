using System;
using System.Text.RegularExpressions;
using static MySQLCLRFunctions._SharedConstants;
using static MySQLCLRFunctions.StringTest;

namespace MySQLCLRFunctions
{
    public static class CharacterTest
    {
        public static bool? NotInX(char? input, string pattern)
        {
            if (input == null) return null;
            if (IsNull(pattern)) throw new ArgumentNullException("pattern cannot be null");
            if (IsEmpty(pattern)) throw new ArgumentNullException("pattern cannot be empty. characters cannot contain and empty string. Get over it.");

            return Regex.IsMatch(input.ToString(), pattern);
        }

        internal static bool? NOTINX(this char? input, string pattern)
        {
            return NotInX(input, pattern);
        }
    }
}