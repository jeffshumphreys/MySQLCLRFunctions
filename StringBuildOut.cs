using Microsoft.SqlServer.Server;

namespace MySQLCLRFunctions
{
    public static class StringBuildOut
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * For building  lines in a loop.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string AppendWithSeparator(string input, string newfield, string sep)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(newfield)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(sep)) sep = ", ";

            // No sense adding the same value
            if (input?.Length == 0) return newfield;
            else return input + sep + newfield;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * For building CSV lines in a loop.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string AppendWithComma(string input, string newfield)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(newfield)) return input;

            // No sense adding the same value
            if (input?.Length == 0) return newfield;
            else return input + ", " + newfield;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * For building TSV lines in a loop.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string AppendWithTab(string input, string newfield)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(newfield)) return input;

            // No sense adding the same value
            if (input?.Length == 0) return newfield;
            else return input + '\t' + newfield;
        }
    }
}
