using Microsoft.SqlServer.Server;

namespace MySQLCLRFunctions
{
    public static class StringReduceCustomizations
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * StripDownCherwellDescription    Descriptions in Cherwell tickets are often full of garbage.  Strip out garbage.
         * Needs work.
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
