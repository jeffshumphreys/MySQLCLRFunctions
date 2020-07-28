using Microsoft.SqlServer.Server;
using System;
using System.Text.RegularExpressions;

namespace MySQLCLRFunctions
{
    public static class StringExtract
    {
        private const int NOT_FOUND = -1;
        private const int CHARACTER_AFTER_MARKER = 1;
        private const int BACKSET_FOR_ZEROBASED = -1;

        /***************************************************************************************************************************************************************************************************
         * 
         *  Find the marker, and pull the entire string before that marker first appears, not including that marker.
         *  null or empty string as a marker returns that input.  Think of use case: Running column values through a "LeftOf(',')"  Null input should not magically
         *  change into a value, i.e., a set of blanks or empty string!  By returning input, we say, "Nevermind, just pass thru
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LeftOf(string input, string marker)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(marker)) return null;  // The test is invalid!  null is NOT a valid search value, so the result must represent INVALID parameter!

            var i = input.IndexOf(marker);
            if (i == NOT_FOUND) return string.Empty;  // Why?  So, "LeftOf('x', 'y') is '', because empty string represents NOTHING, where as null represents INVALID INPUTS. This may help chaining functions.  A null from a valid test will force any expression to null.  Is that what is desired?

            return input.Substring(0, i);
        }

        private static int IndexOfLastChar(this String input)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return NOT_FOUND;
            return input.Length - 1;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Find anything left of the nth found marker - TO THE PREVIOUS MARKER AND NOT INCLUDING THE PREVIOUS MARKER
         *  May need a better name. LeftOfNthFromPrevious?
         *  LeftOfNth("EDWPROD.UserData.x.y", ".", 2);=> UserData
         *  LeftOfNth("EDWPROD.UserData.x.y", ".", 3);=> x
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LeftOfNth(string input, string marker, int n)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));
            var i = input.IndexOf(marker);
            if (i == NOT_FOUND) return null;
            if (n == 1) return input.Substring(0, i);
            int previous_i = NOT_FOUND;
            for (int j = 1; j < n; j++)
            {
                if (i == input.IndexOfLastChar()) return string.Empty;
                previous_i = i;
                i = input.IndexOf(marker, i + marker.Length);
                if (i == NOT_FOUND) return string.Empty;
            }
            if (i >= input.IndexOfLastChar()) return string.Empty;
            if (i == NOT_FOUND) return null;
            int seglen = i - (previous_i + marker.Length);
            return input.Substring(previous_i + marker.Length, seglen);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Pull m pieces back from the nth finding of a marker.
         *  LeftMOfNth("EDWPROD.UserData.x.y", ".", 1, 1);=> EDWPROD
         *  LeftMOfNth("EDWPROD.UserData.x.y", ".", 2, 2);=> EDWPROD.UserData
         *  LeftMOfNth("EDWPROD.UserData.x.y", ".", 1, 2);=> 
         *  LeftMOfNth("EDWPROD.UserData.x.y", ".", 2, 4);=> .   (bug)
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LeftMOfNth(string input, string marker, int n, int howmany)
        {
            int[] pointsfound = new int[n];
            if (string.IsNullOrEmpty(input)) return null;
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            if (n == 0) return null;
            if (howmany > n) return null;

            int i = 0;
            for (int j = 1; j <= n; j++)
            {
                if (i >= input.IndexOfLastChar()) return string.Empty;
                pointsfound[j+BACKSET_FOR_ZEROBASED] = i;
                i = input.IndexOf(marker, i + marker.Length);
                if (i == NOT_FOUND) return string.Empty;
                if (j == howmany)
                {
                    int startofseg = pointsfound[j - howmany];
                    int endofseg = i;
                    int seglen = endofseg - startofseg;
                    return input.Substring(startofseg, seglen);
                }
            }
            return string.Empty;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Any of the characters in marker, any string before any of those.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LeftOfAny(string input, string markerchars)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(markerchars)) return input;

            var i = input.IndexOfAny(markerchars.ToCharArray()); // What about dups?
            if (i == NOT_FOUND) return string.Empty;
            return input.Substring(0, i);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Any string after this string is found.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RightOf(string input, string marker)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(marker)) return input;

            var i = input.IndexOf(marker);
            if (i == NOT_FOUND) return string.Empty;
            return input.Substring(i + marker.Length);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Any of the characters in marker, any string after any of those.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RightOfAny(string input, string markercharacters)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(markercharacters)) return input;

            var i = input.IndexOfAny(markercharacters.ToCharArray());
            if (i == NOT_FOUND) return string.Empty;
            return input.Substring(i + 1);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Extract a snippet of a string given a starting and ending position, rather than substring with starting and length to return.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string Cut(string input, int from, int to)
        {
            if (to - from <= 0)  return String.Empty; 
            if (to > input.Length) return input;

            return input.Substring(from, to - from);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Client-specific.  But a good sampling of how Active Directory data can be pulledExtract a snippet of a string given a starting and ending position, rather than substring with starting 
         * and length to return.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string GetFirstName(string FullName)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(FullName)) return FullName;

            string workingFullName = FullName.Trim();
            string firstName;

            if (workingFullName.EndsWith("(CWF)")) workingFullName = workingFullName.Substring(0, workingFullName.Length - 6).Trim();
            if (workingFullName.EndsWith("(SSI)")) workingFullName = workingFullName.Substring(0, workingFullName.Length - 6).Trim();
            if (workingFullName.EndsWith("(RDI Contractor)")) workingFullName = workingFullName.Substring(0, workingFullName.Length - "(RDI Contractor)".Length).Trim();
            if (workingFullName.Contains(","))
            {
                firstName = workingFullName.Substring(workingFullName.IndexOf(",") + 1);
            }
            else if (workingFullName.Contains(" "))
            {
                firstName = workingFullName.Substring(0, workingFullName.IndexOf(" "));
            }
            else
            {
                firstName = workingFullName;
            }

            if (firstName.EndsWith(" Jr")) { firstName = firstName.Substring(0, firstName.Length - 3); }

            if (firstName.Length > 2 && Char.IsLetter(firstName[firstName.Length - 1]) && (firstName[firstName.Length - 2] == ' '))
            {
                firstName = firstName.Substring(0, firstName.Length - 2);
            }

            return firstName.Trim();
        }

        /***************************************************************************************************************************************************************************************************
           * 
           * Microsoft, and language/compiler designers in general, are very reductive.  "Just use substring!"  
           * But that's not the point of these tiny functions.  
           * Methods are language.  Don't use a screwdriver as a hammer just to save walking back to your truck.  Do it right!  And in a way that is readable to other humans!
           * 
           **************************************************************************************************************************************************************************************/
        public static string Left(this string input, int howmany)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return input.Substring(0, howmany);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Extract the first word using regex word semantics.
        * 
        ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FirstWord(string input)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            return input.Split(@"\W")[0];
         }

        /***************************************************************************************************************************************************************************************************
         * 
         * Return a specific piece by its index.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPieces")]
        public static string PieceNumber(String input, String pattern, int piecenumbertoreturn)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            if (stringpieces != null && stringpieces.Length >= piecenumbertoreturn)
                return stringpieces[piecenumbertoreturn - 1];
            else
                return null;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Convenience function to return the last piece
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPieces")]
        public static string LastPiece(string input, string pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            if (stringpieces != null && stringpieces.Length > 0)
            {
                string laststring = stringpieces[stringpieces.Length - 1];
                if (string.IsNullOrWhiteSpace(laststring))
                {
                    if (stringpieces != null && stringpieces.Length > 1)
                    {
                        laststring = stringpieces[stringpieces.Length - 2];
                    }
                }
                return laststring;
            }
            else
                return null;
        }

        private static string[] SingleStringAsArray(string element1)
        {
            var arr = new string[1];
            arr[0] = element1;
            return arr;
        }
        /***************************************************************************************************************************************************************************************************
        * 
        * Extract the first word before a specific string
        * 
        ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FirstWordBefore(string input, string marker)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            return input.Split(SingleStringAsArray(marker), StringSplitOptions.None)[0];
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Extract the first word before any of the characters in the passed string.  Smallest word returned.
        * 
        ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FirstWordBeforeAnyChar(string input, string markerchars)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            int firstindex = input.IndexOfAny(markerchars.ToCharArray());
            if (firstindex < 1) return null;
            return input.Left(firstindex);
        }

        private static int FindIndexOf(this string input, string marker)
        {
            return input.IndexOf(marker);
        }
        /***************************************************************************************************************************************************************************************************
        * 
        * Extract everything after a specific string
        * 
        ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string EverythingAfter(string input, string marker)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            int i = input.FindIndexOf(marker);
            if (i == -1) return string.Empty;
            if (i + marker.Length > input.Length) return string.Empty;
            return input.Substring(i + marker.Length);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * This is not at all the BASIC function Mid, but what the hey.  I need a snippet from a string, and I want the cleverness of supporting negatives as it's intuitive.
         * Matter of fact, I need substring to be a little smarter too.
         * 
         **************************************************************************************************************************************************************************************/
        public static string Mid(this string input, int from, int to)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (from < 0) return input;
            if (from >= 0 && to >= 0 && from > to) return input;

            if (to < 0)
            {
                string x = input.Substring(from);
                int i = -to;
                x = x.TrimEnd((int)i);
                return x;
            }
            if (to > from) return input.Substring(from, input.Length - (from + to));

            return input;
        }
    }
}
