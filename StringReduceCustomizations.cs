using Microsoft.SqlServer.Server;
using static MySQLCLRFunctions.StringTest;

namespace MySQLCLRFunctions
{
    /*
     * Customizations means functions that are not as general as the ones in StringReduce.
     * RemoveFunctionCall may be a tad general, but still only applies to code.
     * 
     */
    public static class StringReduceCustomizations
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * StripDownCherwellDescription    Descriptions in Cherwell tickets are often full of garbage.  Strip out garbage.
         * Needs work.
         * 
         * Product customized for: 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string TrimCherwellTicketField(string input)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return input.Trim();
        }

        /**
         * Converts "TRIM(NULLIF(mgrdtl1.info, ''))" to "TRIM(mgrdtl1.info)"
         */
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RemoveFunctionCall(string input, string functionName)
        {
            // scan line for functionname + "(", then find matching ")" and work backwards to previous ","
            return input;
        }

        /**************************************************************************************************************************************************************************************
         * 
         * Replaces my overused function spread:  NULLIF(LTRIM(RTRIM(CAST(c.Title AS VARCHAR(100)))), '')
         * Except for the downcast from NVARCHAR to VARCHAR, which is technically risky, but if all input is ascii...
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string TrimNormalizeStringInput(string input)
        {
            if (IsNull(input)) return input; // Null is a fine value.
            if (IsEmptyOrWhiteSpace(input)) return null; // Normalize to null for joins, coalesces, ISNULL tests

            input = input.Trim().REPLACEMATCHX(@"\s+", " ");
            return input;
        }
    }
}
