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
        public static IEnumerable Pieces(String stringtosplitintopieces, String regexsplitterpattern)
        {
            returnpieceorderno = 1;
            string[] stringpieces = Regex.Split(stringtosplitintopieces, regexsplitterpattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            return stringpieces;
        }

        private static int returnpieceorderno = 0;
        private static void FillRowWithStrPieces(Object obj, out SqlString piece, out SqlInt32 pieceorderNo)
        {
            piece = obj.ToString();
            pieceorderNo = returnpieceorderno++;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        /***************************************************************************************************************************************************************************************************
         * 
         * Trim repeating bunches of character off right of string
         * 
         * - Created for trimming "1.3930000000" off floating point number in sql which REFUSES to round.
         * 
         **************************************************************************************************************************************************************************************/
        public static string RTrimChar(string input, string trimAllOccFromRight)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            return input.TrimEnd(trimAllOccFromRight.ToCharArray());
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
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RemoveSQLServerNameDelimiters(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            if (string.IsNullOrEmpty(input)) return input;

            input = input.Trim().Trim(new char[] { '[', ']' }).Replace("]]", "]");
            return input;
        }

    }
}
