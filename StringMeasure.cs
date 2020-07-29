using Microsoft.SqlServer.Server;
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

        /***************************************************************************************************************************************************************************************************
         * 
         * Can't do in SQL Server, unfortunately.
         * 
         ***************************************************************************************************************************************************************************************************/
        internal static T Min<T>(params T[] args) where T: struct, IComparable
        {
            bool notset = true;
            T minarg = default(T);

            foreach(T arg in args)
            {
                if (notset)
                {
                    minarg = arg;
                }
                if (arg.CompareTo(minarg) < 0) minarg = arg;
            }

            return minarg;
        }

        internal static T MinOver<T>(T floor, params T[] args) where T : struct, IComparable
        {
            bool notset = true;
            T minarg = default(T);

            foreach (T arg in args)
            {
                if (notset)
                {
                    minarg = arg;
                }
                if (arg.CompareTo(minarg) < 0 ) minarg = arg;
            }

            if (minarg.CompareTo(floor) < 0) minarg = floor;
            return minarg;
        }

        internal static T Max<T>(params T[] args) where T : struct, IComparable
        {
            bool notset = true;
            T maxarg = default(T);

            foreach (T arg in args)
            {
                if (notset)
                {
                    maxarg = arg;
                }
                if (arg.CompareTo(maxarg) > 0) maxarg = arg;
            }

            return maxarg;
        }
    }
}
