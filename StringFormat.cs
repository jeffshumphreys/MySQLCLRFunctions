using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
/*
* Take measurements from a string.  Not actual values from a string as Extract does.
*/
namespace MySQLCLRFunctions
{
    public static class StringFormat
    {
        /***************************************************************************************************************************************************************************************************
        * 
        * Append spaces on the right out to a fixed point.  This is for displaying to a text output.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RPad(string input, int padToLen)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return input.PadRight(padToLen);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Prepend spaces on the left so that the text is right-justified.  Could be zeroes, or X's.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LPadChar(string input, int padToLen, char padCh)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return input.PadLeft(padToLen, padCh);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Append a specific character on the right out to a fixed point.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RPadChar(string input, int padToLen, char padCh)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return input.PadRight(padToLen, padCh);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Prepend spaces on the left so that the text is right-justified.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LPad(string input, int padToLen)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return input.PadLeft(padToLen);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * For name-style, this is like "UPPER" in SQL Server, as opposed to ToUpper in C#/.NET.  I'm trying to avoid "To-" naming since I already use "Is-" naming and these are for SQL use.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string Title(string input)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return Regex.Replace(input.ToLower(), @"\b[a-z]\w+", delegate (Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }
    }
}
