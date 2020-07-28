using Microsoft.SqlServer.Server;
using System.Text;
/*
* Take measurements from a string.  Not actual values from a string as Extract does.
*/
namespace MySQLCLRFunctions
{
    public static class StringDecode
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * Garbage characters in massive text fields are hard to deal with and see if they're invisible to the naked eye, but they waste space and confuse comparisons.
         * 
         * This is very helpful. 
         * 
         *    The string snippet 
         *              ".com                  There are 3"  becomes ".com131016013101601310There are 3"  
         *              
         *    Notice the "160" in there.  what is that?  Doesn't matter, I can just strip it. This is a real example, and that "160" has made many an analysts' life difficult.
         *    
         * Here's an example of this used in conjunction with multispace stripping:
         * Comment = REPLACE(dbo.ReplaceRecursive(REPLACE(SUBSTRING(Input, LEN('2020-01-22T07:41:40 by ')+2, 32000), CHAR(160), ''), CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10), CHAR(13) + CHAR(10)), ' ' + CHAR(13) + CHAR(10), CHAR(13) + CHAR(10))
         * 
         * It's not pretty, and that's another level of simplification/generalization, but first things first. Notice that I stripped the "160" out first, then the NLs are easier to manage because they line up.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RevealNonPrintables(string input)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;

            StringBuilder sb = new StringBuilder(input.Length + 10);
            foreach (char c in input.ToCharArray())
            {
                double i = (int)c;
                if (i < 32 || i > 127)
                    sb.Append(i.ToString());
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
