using System;
using System.Collections;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System.Xml.Schema;

namespace MySQLCLRFunctions
{
    /*
     * 
     * Naming conventions for regex parameters
     * - find is for flat string
     * - regexmatchpattern when a matching process
     * - regexcapturepattern when it has to capture something
     * - match is a noun, what was matched to the pattern
     */
    public static class StringTransform
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * Pull Out Matches by Regex
         * 
         * - "Matches" plural is what I use to indicate a table valued function. I'll make Match for inline SQL pulls.
         * - No need to add "Exp" since always pull out by expression.  Can't think of how else.
         * 
         **************************************************************************************************************************************************************************************/

        // This attribute is used only by Microsoft Visual Studio to automatically register the specified method as a TVF. It is not used by SQL Server.
        // For table-valued functions, the columns of the return table type cannot include timestamp columns or non-Unicode string data type columns (such as char, varchar, and text). The NOT NULL constraint is not supported.
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithCapStrs")]
        public static IEnumerable Matches(String stringtoextractmatchesintorows, String regexcapturepattern)
        {
            Regex stringsplitCompiledRegex = new Regex(regexcapturepattern);
            MatchCollection matches = stringsplitCompiledRegex.Matches(stringtoextractmatchesintorows);
            return matches;
        }

        private static void FillRowWithCapStrs(Object matchObject, out SqlString match, out SqlInt32 matchat)
        {
            match = new SqlString(((Match)matchObject).Captures[0].Value);
            match = new SqlString(((Match)matchObject).Groups[1].Value);
            matchat = ((Match)matchObject).Captures[0].Index;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Split into Pieces by Regex
         * 
         * - Very useful function, and simplified down from anything PAID SQL Sharp has.
         * - Trying to simplify and shorten names without over complicating.  "Exp" short for expression.  
         * - Not much else I can think of that I would split other than strings, so drop the "String".  DateTime_Split?  Numeric_Split?  Bit_Split?
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPieces")]
        public static IEnumerable Pieces(String stringtosplitintopieces, String regexmatchpattern)
        {
            returnpieceorderno = 1;
            string[] stringpieces = Regex.Split(stringtosplitintopieces, regexmatchpattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            return stringpieces;
        }

        private static int returnpieceorderno = 0;
        private static void FillRowWithStrPieces(Object obj, out SqlString piece, out SqlInt32 pieceorderNo)
        {
            piece = obj.ToString();
            pieceorderNo = returnpieceorderno++;
        }


        // Extensions for internal use and simpler Fluent design, but not for SQL to call
        internal static string ReplaceMatchExt(this string input, string regexmatchpattern, string replacement)
        {
            return ReplaceMatch(input, regexmatchpattern, replacement);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * ReplaceMatch    Similar to REPLACE in SQL Server, except it has .NET regex
         * 
         * Note parameter naming convention: find is for flat string, regexmatchpattern when a matching process, regexcapturepattern when it has to capture something
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string ReplaceMatch(string input, string regexmatchpattern, string replacement)
        {
            return Regex.Replace(input, regexmatchpattern, replacement);
        }

        // Extensions for internal use and simpler Fluent design, but not for SQL to call
        internal unsafe static string ReplaceRecursiveExt(this string input, string find, string replacement)
        {
            return ReplaceRecursive(input, find, replacement);
        }

        unsafe public static string ReplaceRecursive(string input, string find, string replacement)
        {
            if (find == replacement) return input;
            if (replacement.Contains(find))
            {
                throw new ArgumentOutOfRangeException(nameof(replacement), "the replacement string contains the sought string, so recursion would cause a blowup.");
            }

            if (find.Length != replacement.Length)
            {
                StringBuilder sb = new StringBuilder(input);
                while (true)
                {
                    int formersize = sb.Length;
                    sb.Replace(find, replacement);
                    int newsize = sb.Length;
                    if (formersize == newsize)
                    {
                        return sb.ToString();
                    }
                }
            }
            else if (find.Length == replacement.Length)
            {
                int findlen = find.Length;
                var findchars = find.ToCharArray();
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

        /***************************************************************************************************************************************************************************************************
         * 
         * Trim repeating bunches of character off right of string
         * 
         * - Created for trimming "1.3930000000" off floating point number in sql which REFUSES to round.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RTrimChar(string input, string trimAllOccFromRight)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            return input.TrimEnd(trimAllOccFromRight.ToCharArray());
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RTrimN(string input, int howmanycharactersoffend)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input) || howmanycharactersoffend == 0) return input;
            if (input.Length <= howmanycharactersoffend) return string.Empty;
            if (howmanycharactersoffend < 0) throw new ArgumentOutOfRangeException(nameof(howmanycharactersoffend), howmanycharactersoffend, "Cannot have negate remove characters.");
            return input.Substring(0, input.Length - howmanycharactersoffend);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LPad(string input, int padToLen)
        {
            return input.PadLeft(padToLen);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RPad(string input, int padToLen)
        {
            return input.PadRight(padToLen);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RPadChar(string input, int padToLen, char padCh)
        {
            return input.PadRight(padToLen, padCh);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LPadChar(string input, int padToLen, char padCh)
        {
            return input.PadLeft(padToLen, padCh);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string BlankOut(string input, string blankanyofthese, string sep)
        {
            foreach (string i in blankanyofthese.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries))
            {
                input.Replace(i, String.Empty);
            }
            return input;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * Title    Like "UPPER" in SQL Server, as opposed to ToUpper in C#/.NET
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string Title(string input)
        {
            return Regex.Replace(input, @"\b[a-z]\w+", delegate (Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }
    }
}
