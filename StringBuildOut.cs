using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions
{
    public static class StringBuildOut
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * For building CSV lines in a loop.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string AppendWithSeparator(string input, string newfield, string sep)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(newfield)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(sep)) sep = ", ";

            // No sense adding the same value
            if (input == string.Empty) input = newfield;
            else input = input + sep + newfield;

            return input;
        }
    }
}
