using Microsoft.SqlServer.Server;
using System;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using static MySQLCLRFunctions.StringTest;
using static MySQLCLRFunctions._SharedConstants;
using System.Linq;

/*
* Take measurements from a string.  Not actual values from a string as Extract does, and not a "changed" (immutable) string like Transform or Pivot, or Format, Reduce.
* 
* WARNING SqlDateTime is not the same as datetime.  SqlDateTime maps to DATETIME, DateTime maps to DATETIME2
* 
* S = string
* C = char
* X = Regex pattern
* M
* N
* L = Array, list.
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
        public static SqlInt32 HowManyS(string input, string marker)
        {
            if (IsNull(input) || IsNull(marker)) return SqlInt32.Null;
            if (IsEmpty(marker)) throw new ArgumentOutOfRangeException("Empty strings would result in infinite loop.");
            if (IsEmpty(input)) return 0;

            // Warning: May not count "%%%%" where search string is "%%".  Is it 2 or 1.
            int howMany = (input.Length - input.Replace(marker, "").Length) / marker.Length;

            return howMany;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlInt32 HowManyC(string input, char c)
        {
            if (IsNull(input))  return SqlInt32.Null;
            if (IsEmpty(input)) return 0;

            int howMany = input.Count(i => i == c);

            return howMany;
        }

        internal static int? HOWMANYS(this string input, string marker)
        {
            var rtnval = HowManyS(input, marker);
            if (rtnval.IsNull) return null;
            return rtnval.Value;
        }
        internal static int? HOWMANYC(this SqlString input, char c)
        {
            var rtnval = HowManyC(input.ToString(), c);   // Not very clever.
            if (rtnval.IsNull) return null;
            return rtnval.Value;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * How many of a string occurs?  What about overlap?
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlInt32 HowManyX(string input, string pattern)
        {
            // General Rule: Follow SQL rules around null
            if (IsNull(input) || IsNull(pattern)) return SqlInt32.Null;
            if (IsEmpty(pattern)) throw new ArgumentOutOfRangeException("Empty strings as a search pattern would result in infinite loop.");
            if (IsEmpty(input)) return 0; // Something can't be in nothing
            if (IsWhiteSpace(pattern)) return SqlInt32.Null;

            MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.None, matchTimeout: TimeSpan.FromSeconds(2));

            return matches.Count;
        }
        internal static int? HOWMANYX(this string input, string pattern)
        {
            var rtnval = HowManyX(input, pattern);
            if (rtnval.IsNull) return null;
            return rtnval.Value;
        }
        internal static int? HOWMANYX(this SqlString input, string pattern)
        {
            var rtnval = HowManyX(input.ToString(), pattern);
            if (rtnval.IsNull) return null;
            return rtnval.Value;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Same as SQL Server IN clause.
         * 
         ***************************************************************************************************************************************************************************************************/
        public static bool? In(this string input, params string[] markers)
        {
            return input.InL(markers);
        }

        public static bool? InL(this string input, string[] markers)
        {
            if (IsNull(input)) return null;
            if (IsEmpty(input)) return false;    // Something can never be in nothing. Can empty = empty? No.

            foreach (string marker in markers)
            {
                if (marker != null && input == marker) return true;
            }

            return false;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * Can't do in SQL Server, unfortunately.
         * 
         ***************************************************************************************************************************************************************************************************/
        public static T Min<T>(params T[] args) where T: struct, IComparable
        {
            bool notset = true;
            T minarg = default;

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
            T minarg = default;

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
            T maxarg = default;

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
