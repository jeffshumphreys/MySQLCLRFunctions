using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace MySQLCLRFunctions
{
    // Functions that take input, are non-mutating, do not involve floating point or date time data, and return either true or false.
    public static class StringTest
    {
        /***************************************************************************************************************************************************************************************************
         * 
         *  Simple expanding name to avoid constant confusion I have with what this function does.
         *  Because most people don't equivocate white space with an empty space, which is the opposite of space.  It is non-space.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsNullOrWhiteSpaceOrEmpty(string input) { return string.IsNullOrWhiteSpace(input); }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsNullOrEmpty(string input) { return string.IsNullOrEmpty(input); }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsNull(string input) { return input == null; }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsEmpty(string input) { return input?.Length == 0; }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsEmptyOrWhiteSpace(string input) { return input?.Length == 0 || IsWhiteSpace(input); }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsNullOrWhiteSpace(string input) { return input == null || IsWhiteSpace(input); }

        /***************************************************************************************************************************************************************************************************
         * 
         *  White space is not empty!  Ask any human person besides a programmer!  An empty string has no space in it.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsWhiteSpace(string input)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsEmpty(input)) return false; // White space IS NOT empty string!!!! It exists!!!! Think about it!  Stop being a lazy programmer!

            foreach (var c in input)
            {
                if (!Char.IsWhiteSpace(c))
                    return false;
            }

            return true;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         *  Computer (Host) names vs. IP4 in the same columns.
         * 
         ***************************************************************************************************************************************************************************************************/

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsIP4(string input)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsEmpty(input)) return false; // Empty string doesn't start with anything

            if (IPAddress.TryParse(input, out IPAddress address))
            {
                if (address.AddressFamily is System.Net.Sockets.AddressFamily.InterNetwork)
                    return true;
            }

            return false;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Computer (Host) names vs. IP6 in the same columns.  I'm not using this yet.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsIP6(string input)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsEmpty(input)) return false; // Empty string doesn't start with anything

            if (IPAddress.TryParse(input, out IPAddress address))
            {
                if (address.AddressFamily is System.Net.Sockets.AddressFamily.InterNetworkV6)
                    return true;
            }

            return false;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * Returns true if the input string starts with the sought string.
         * 
         ***************************************************************************************************************************************************************************************************/

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean StartsWithS(string input, string marker)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsNull(marker)) throw new ArgumentNullException("marker cannot be null");
            if (IsEmpty(marker)) throw new ArgumentNullException("marker cannot be empty. Would be an infinite loop.");
            if (IsEmpty(input)) return false; // Empty string doesn't start with anything

            return input.StartsWith(marker);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Returns true if the input string ends with the sought string.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean EndsWithS(string input, string marker)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsNull(marker)) throw new ArgumentNullException("marker cannot be null");
            if (IsEmpty(marker)) throw new ArgumentNullException("marker cannot be empty. Would be an infinite loop.");
            if (IsEmpty(input)) return false; // Empty string doesn't end with anything

            return input.EndsWith(marker);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Stub.  There are significant rules around server names.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean LegalName(string input, string rule)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsNull(rule)) throw new ArgumentNullException("character rule cannot be null");
            if (IsEmpty(rule)) throw new ArgumentNullException("rule cannot be empty. That would be a non-rule.");
            if (IsEmpty(input)) return false; // Empty string is never a legal name

            if (rule == "SQL Server Server Name")
            {
                if ((bool)input.FirstC().NotInX("[a-zA-Z\\&\\_\\#]")) return false;
                // Instance portion Cannot be Default or MSSQLServer
                // Instance be up to 16 characters, Unicode Standard 2.0, decimal numbers basic latin or other national scripts, $, #, _
                // First character must be letter, &, _, #
                // No embedded spaces, special characters, backslash, comma, colon, or at sign.

                // Windows Server 2008 R2 NetBIOS limited to 15 char
                return true;
            }
            else
            {
                throw new ArgumentException($"Unrecognize rule: {rule}");
            }
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Any of these substrings are in the delimited list of sought substrings.  I would default the delimiter, but SQL Server doesn't care for optional parameters.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean AnyOfTheseSAreAnyOfThoseS(string inputs, string markers, string sep)
        {
            if (IsNull(inputs) || IsNull(markers) || IsNull(sep)) return SqlBoolean.Null;
            if (inputs.Length == 0) return false; // Nothing can be in an empty string
            if (markers.Length == 0 || sep.Length == 0) throw new ArgumentOutOfRangeException("Empty strings in a search make no sense.");

            var inputsasarray = inputs.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string i in markers.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (inputsasarray.Contains(i))
                {
                    return true;
                }
            }

            return false;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * SQL style like for multiple strings.  So "Car%;%X[0-9]%;"  Probably just regex strings, but I don't know.  But caller will have to put "^" and "$" around string if they want exact matches.
         * Need working examples.  Haven't tested.
         * 
         * Idea is: SELECT * FROM x where dbo.LikeAny(FullName, '%Humphreys;Humphrey%;JSH;%Jeff%Hum%;Jeff%H;(Jeff|Jeffrey|Jeffry);') = 1
         * 
         ***************************************************************************************************************************************************************************************************/

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean LikeAny(string inputs, string patterns, string inputsep, string patternsep)
        {
            if (IsNull(inputs) || IsNull(patterns) || IsNull(inputsep) || IsNull(patternsep)) return SqlBoolean.Null; // SQL: Nulls make null
            if (inputs.Length == 0) return false; // Nothing can be in an empty string
            if (patterns.Length == 0 || inputsep.Length == 0 || patternsep.Length == 0) throw new ArgumentOutOfRangeException("Empty strings in a search make no sense.");

            var inputsasarray = inputs.Split(new string[] { inputsep }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pattern in patterns.Split(new string[] { inputsep }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (string input in inputsasarray)
                {
                    if (Regex.IsMatch(input, pattern)) return true;
                }
            }

            return false;
        }

        /*
         *
         *          var str = "White Red Blue Green Yellow Black Gray";
         *          var achromaticColors = new[] {"White", "Black", "Gray"};
         *          var exquisiteColors = new[] {"FloralWhite", "Bistre", "DavyGrey"};
         *          str = str.ReplaceAll(achromaticColors, exquisiteColors);
         *          // str == "FloralWhite Red Blue Green Yellow Bistre DavyGrey"
         *
         *      public static string ReplaceAll(this string value, IEnumerable<string> oldValues, IEnumerable<string> newValues)
         *      {
         *       var sbStr = new StringBuilder(value);
         *       var newValueEnum = newValues.GetEnumerator();
         *       foreach (var old in oldValues)
         *       {
         *           if (!newValueEnum.MoveNext())
         *               throw new ArgumentOutOfRangeException("newValues", "newValues sequence is shorter than oldValues sequence");
         *           sbStr.Replace(old, newValueEnum.Current);
         *       }
         *       if (newValueEnum.MoveNext())
         *           throw new ArgumentOutOfRangeException("newValues", "newValues sequence is longer than oldValues sequence");
         *      
         *       return sbStr.ToString();
         *       }
         */
    }
}
