using Microsoft.SqlServer.Server;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MySQLCLRFunctions
{
    /*
     * 
     * Naming conventions for regex parameters
     * - find is for flat string
     * - pattern when a matching process
     * - regexcapturepattern when it has to capture something
     * - match is a noun, what was matched to the pattern
     */
    public static class StringTransform
    {
        // Extensions for internal use and simpler Fluent design, but not for SQL to call
        internal static string ReplaceMatchExt(this string input, string pattern, string replacement)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;
            if (replacement == null) return input;  // Did they mean empty string?

            return ReplaceMatch(input, pattern, replacement);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Similar to REPLACE in SQL Server, except it has .NET regex for matching
         * 
         * Note parameter naming convention: find is for flat string, pattern when a matching process, regexcapturepattern when it has to capture something
         * TODO: Many examples of how it can be used effectively.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string ReplaceMatch(string input, string pattern, string replacement)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (replacement == null) return input;  // Did they mean empty string?

            return Regex.Replace(input, pattern, replacement);
        }

        // Extensions for internal use and simpler Fluent design, but not for SQL to call
        internal unsafe static string ReplaceRecursiveExt(this string input, string marker, string replacement)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(marker)) return input;
            if (replacement == null) return input;  // Did they mean empty string?

            return ReplaceRecursive(input, marker, replacement);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Replace again and again.  This is for when I want to remove all but one of the spaces, or all the "             " wide open spaces in a SQL proc header except for one space.
         * 
         * This is good as part of a series of "fluent" methods to reduce a proc to an easily read header string.  For quick review of a million modules.
         * 
         **************************************************************************************************************************************************************************************/
        unsafe public static string ReplaceRecursive(string input, string marker, string replacement)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(marker)) return input;
            if (replacement == null) return input;  // Did they mean empty string?

            if (marker == replacement) return input;
            if (replacement.Contains(marker))
            {
                throw new ArgumentOutOfRangeException(nameof(replacement), "the replacement string contains the sought string, so recursion would cause a blowup.");
            }

            if (marker.Length != replacement.Length)
            {
                StringBuilder sb = new StringBuilder(input);
                while (true)
                {
                    int formersize = sb.Length;
                    sb.Replace(marker, replacement);
                    int newsize = sb.Length;
                    if (formersize == newsize)
                    {
                        return sb.ToString();
                    }
                }
            }
            else if (marker.Length == replacement.Length)
            {
                int findlen = marker.Length;
                var findchars = marker.ToCharArray();
                int len = input.Length;
                fixed (char* pointer = input)
                {
                    // Add one to each of the characters.
                    for (int i = 0; i <= len - findlen; i++)
                    {
                        int j2 = 0;
                        for (int j = i; j < len; j++)
                        {
                            if (pointer[j + j2] != findchars[j2]) goto outerloop;
                            j2++;
                            if (j2 == findlen)
                            {
                                // Replace!!!!!!!!

                                pointer[j] = findchars[0];
                            }
                        }
                    outerloop:
                        ;
                    }
                    // Return the mutated string.
                    return new string(pointer);
                }
            }

            return input;
        }

    }
}
