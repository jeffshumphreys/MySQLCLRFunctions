using Microsoft.SqlServer.Server;
using static MySQLCLRFunctions.StringTest;
using System;
using System.Text.RegularExpressions;
/*
* Take measurements from a string.  Not actual values from a string as Extract does, and not a "changed" (immutable) string like Transform or Pivot, or Format, Reduce.
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
        public static int? HowManyX(string input, string pattern)
        {
            // General Rule: Follow SQL rules around null
            if (IsNull(input) || IsNull(pattern)) return null;
            if (IsEmpty(pattern)) throw new ArgumentOutOfRangeException("Empty strings as a search pattern would result in infinite loop.");
            if (IsEmpty(input)) return 0; // Something can't be in nothing
            if (IsWhiteSpace(pattern)) return null;

            MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.None, matchTimeout: TimeSpan.FromSeconds(2));
            
            return matches.Count;
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
