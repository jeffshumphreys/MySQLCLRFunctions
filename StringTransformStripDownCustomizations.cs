using Microsoft.SqlServer.Server;
using System;
using System.Linq;

namespace MySQLCLRFunctions
{
    public static class StringTransformStripDownCustomizations
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * StripDownCherwellDescription    Descriptions in Cherwell tickets are often full of garbage.  Strip out garbage.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string StripDownCherwellDescription(string input)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return input.Trim();
        }
    }
}
