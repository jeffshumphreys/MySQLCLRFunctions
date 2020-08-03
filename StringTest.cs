using Microsoft.SqlServer.Server;
using System;
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
        public static bool IsNullOrWhiteSpaceOrEmpty(string input)
        {
            return (string.IsNullOrWhiteSpace(input));
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsNullOrEmpty(string input)
        {
            return (input == null || input == "");
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsNull(string input)
        {
            return (input == null);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsEmpty(string input)
        {
            return (input == "");
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsEmptyOrWhiteSpace(string input)
        {
            return (input == "" || IsWhiteSpace(input));
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsNullOrWhiteSpace(string input)
        {
            return (input == null || IsWhiteSpace(input));
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  White space is not empty!  Ask any human person besides a programmer!  An empty string has no space in it.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsWhiteSpace(string input)
        {
            if (input == "") return false;
            foreach (var c in input.ToCharArray())
            {
                if (!Char.IsWhiteSpace(c)) return false;

            }

            return true;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         *  Computer (Host) names vs. IP4 in the same columns.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsIP4(string input)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return false;

            IPAddress address;
            if (IPAddress.TryParse(input, out address))
            {
                if (address.AddressFamily is System.Net.Sockets.AddressFamily.InterNetwork) return true;
            }

            return false;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Computer (Host) names vs. IP6 in the same columns.  I'm not using this yet.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsIP6(string input)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return false;

            IPAddress address;
            if (IPAddress.TryParse(input, out address))
            {
                if (address.AddressFamily is System.Net.Sockets.AddressFamily.InterNetworkV6) return true;
            }

            return false;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * Returns true if the input string starts with the sought string.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool? StartsWith(string input, string marker)
        {
            if (StringTest.IsNull(input)) return false;
            if (StringTest.IsNullOrEmpty(marker)) return null;

            return input.StartsWith(marker);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Returns true if the input string ends with the sought string.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool? EndsWith(string input, string marker)
        {
            if (StringTest.IsNull(input)) return false;
            if (StringTest.IsNullOrEmpty(input)) return null;

            return input.EndsWith(marker);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Stub.  There are significant rules around server names.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool LegalName(string input, string characterrule)
        {
            throw new NotImplementedException("Copied from old code. Great idea, but needs to be written.");

#pragma warning disable CS0162 // Unreachable code detected
            if (characterrule == "SQL Server Server Name")
#pragma warning restore CS0162 // Unreachable code detected
            {
                // Instance portion Cannot be Default or MSSQLServer
                // Instance be up to 16 characters, Unicode Standard 2.0, decimal numbers basic latin or other national scripts, $, #, _
                // First character must be letter, &, _, #
                // No embedded spaces, special characters, backslash, comma, colon, or at sign.

                // Windows Server 2008 R2 NetBIOS limited to 15 char
            }

            return true;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Any of these substrings are in the delimited list of sought substrings.  I would default the delimiter, but SQL Server doesn't care for optional parameters.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool AnyOfTheseSAreAnyOfThoseS(string inputs, string markers, string sep)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(inputs)) return false;

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
        public static bool LikeAny(string input, string patterns, string patternsep)
        {
            throw new NotImplementedException("Copied from old code. Great idea, but needs to be written.");

#pragma warning disable CS0162 // Unreachable code detected
            foreach (string pattern in patterns.Split(new string[] { patternsep }, StringSplitOptions.RemoveEmptyEntries))
#pragma warning restore CS0162 // Unreachable code detected
            {
                if (Regex.IsMatch(input, pattern)) return true;
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
