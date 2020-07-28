using Microsoft.SqlServer.Server;
using NUnit.Framework;
/*
* Take measurements from a string.  Not actual values from a string as Extract does.
*/
namespace MySQLCLRFunctions
{
    public static class StringMeasure
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * How many of a string occurs?  What about overlap?
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static int? HowMany(string input, string marker)
        {
            // General Rule: Follow SQL rules around null
            if (input == null || marker == null) return null;
            // throw exception if marker = ""
            // return 1 if input = "" and marker = ""??????

            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return null;

            // Warning: May not count "%%%%" where search string is "%%".  Is it 2 or 1.
            int howMany = (input.Length - input.Replace(marker, "").Length) / marker.Length;
   
            return howMany;
        }
    }
}
