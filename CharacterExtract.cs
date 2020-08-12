using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions
{
    public static class CharacterExtract
    {
        /***************************************************************************************************************************************************************************************************
        * 
        * Extract everything after a specific string
        * 
        * WARNING: Empty string is not a character!
        * 
        ***************************************************************************************************************************************************************************************************/
        internal static char? FIRSTC(this string input)
        {
            if (StringTest.IsNullOrEmpty(input)) return null; // Empty string is not a character!

            return input[0];
        }

        public static char? FirstC(string input)
        {
            if (StringTest.IsNullOrEmpty(input)) return null; // Empty string is not a character!

            return input[0];
        }
    }
}
