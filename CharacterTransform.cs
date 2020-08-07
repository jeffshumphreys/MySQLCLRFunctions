using System;
using System.Collections.Generic;
using System.Linq;
using static MySQLCLRFunctions.StringTest;
using static MySQLCLRFunctions._SharedConstants;
using static MySQLCLRFunctions.StringReduce;
namespace MySQLCLRFunctions
{
    public static class CharacterTransform
    {
        public static string ReplaceFirstC(this string input, char replacement)
        {
            if (IsNullOrEmpty(input)) return input;

            //if (Char.GetUnicodeCategory(replacement) != System.Globalization.UnicodeCategory.) Warning!

            char firstC = input.First();
            if (firstC == replacement) return input;
            return replacement + LTrimOne(input);
        }

        public static string ReplaceLastC(this string input, char replacement)
        {
            if (IsNullOrEmpty(input)) return input;

            //if (Char.GetUnicodeCategory(replacement) != System.Globalization.UnicodeCategory.) Warning!

            char lastC = input.Last();
            if (lastC == replacement) return input;
            return RTrimOne(input) + replacement;
        }
    }
}
