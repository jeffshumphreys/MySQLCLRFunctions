using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
/*
* Formatting may lengthen, reduce, or do nothing to the size of a string.  It's more about presentation.  Which might put it under Humanization?
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
        public static string RPad(string input, int length)
        {
            if (StringTest.IsNull(input)) return input;

            return input.PadRight(length);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Prepend spaces on the left so that the text is right-justified.  Could be zeroes, or X's.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LPadC(string input, int length, char character)
        {
            if (StringTest.IsNull(input)) return input;

            return input.PadLeft(length, character);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Append a specific character on the right out to a fixed point.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RPadC(string input, int length, char character)
        {
            if (StringTest.IsNull(input)) return input;

            return input.PadRight(length, character);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Prepend spaces on the left so that the text is right-justified.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LPad(string input, int length)
        {
            if (StringTest.IsNull(input)) return input;

            return input.PadLeft(length);
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

            return Regex.Replace(input.ToLower(), @"\b[a-z]\w+", (Match match) =>
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }
    }
}
