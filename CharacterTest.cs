using System;
using System.Text.RegularExpressions;
using static MySQLCLRFunctions._SharedConstants;
using static MySQLCLRFunctions.StringTest;
using static MySQLCLRFunctions.CharacterTransform;
using static MySQLCLRFunctions.CharacterTest;
using System.Linq;

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

        public static bool ISNULLORWHITESPACE(this char? input)
        {
            if (input == null) return true;
            if (input.In(' ', '\n', '\r', '\t')) return true;    // Wait: Are these empty or whitespace?
            if (input == 0) return true; // Tricky.  It's not really SQL NULL.
            return false;
        }

        public static bool ISNULLORWHITESPACE(this char input)
        {
            if (input == ' ') return true;
            if (input == 0) return true; // Tricky.  It's not really SQL NULL.
            return false;
        }
        public static bool In(this char input, params char[] markers)
        {
            return markers.Contains(input);
        }
        public static bool In(this char? input, params char[] markers)
        {
            return markers.Contains((char)input);
        }
        public static bool IsWhiteSpace(char c) { return c == ' '; } // Uhh, will this work from Sql Server??
    }
}