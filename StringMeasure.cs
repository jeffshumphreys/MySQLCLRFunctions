using Microsoft.SqlServer.Server;
using static MySQLCLRFunctions.StringTest;
using System;
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
        public static int? HowManyS(string input, string marker)
        {
            // General Rule: Follow SQL rules around null
            if (IsNull(input) || IsNull(marker)) return null;
            if (IsEmpty(marker)) throw new ArgumentOutOfRangeException("Empty strings would result in infinite loop.");
            if (IsEmpty(input)) return 0;

            // Warning: May not count "%%%%" where search string is "%%".  Is it 2 or 1.
            int howMany = (input.Length - input.Replace(marker, "").Length) / marker.Length;

            return howMany;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * How many of a string occurs?  What about overlap?
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static int? HowManyX(string input, string expression)
        {
            // General Rule: Follow SQL rules around null
            if (input == null || expression == null) return null;
            // throw exception if marker = ""
            // return 1 if input = "" and marker = ""??????

            if (IsNullOrWhiteSpaceOrEmpty(input)) return null;

            // Warning: May not count "%%%%" where search string is "%%".  Is it 2 or 1.
            int howMany = (input.Length - input.Replace(expression, "").Length) / expression.Length;

            return howMany;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * Can't do in SQL Server, unfortunately.
         * 
         ***************************************************************************************************************************************************************************************************/
        public static T Min<T>(params T[] args) where T: struct, IComparable
        {
            bool notset = true;
            T minarg = default(T);

            foreach(T arg in args)
            {
                if (notset)
                {
                    minarg = arg;
                    notset = false;
                }
                if (arg.CompareTo(minarg) < 0 /* arg precedes minarg in the sort order */) minarg = arg;
            }

            return minarg;
        }

        public static T MinOver<T>(T floor, params T[] args) where T : struct, IComparable
        {
            bool notset = true;
            T minarg = default(T);

            foreach (T arg in args)
            {
                if (notset)
                {
                    minarg = arg;
                    notset = false;
                }
                if (arg.CompareTo(minarg) < 0 ) minarg = arg;
            }

            if (minarg.CompareTo(floor) < 0) minarg = floor;
            return minarg;
        }

        public static T Max<T>(params T[] args) where T : struct, IComparable
        {
            bool notset = true;
            T maxarg = default(T);

            foreach (T arg in args)
            {
                if (notset)
                {
                    maxarg = arg;
                    notset = false;
                }

                if (arg.CompareTo(maxarg) > 0 /* arg follows maxarg in the sort order */) maxarg = arg;
            }

            return maxarg;
        }
    }
}
