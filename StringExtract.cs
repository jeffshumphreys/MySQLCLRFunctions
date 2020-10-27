using Microsoft.SqlServer.Server;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using static MySQLCLRFunctions._SharedConstants;
using static MySQLCLRFunctions.StringTest;
using static MySQLCLRFunctions.StringTransform;
using static System.Math;

namespace MySQLCLRFunctions
{
    public static class StringExtract
    {
        /***************************************************************************************************************************************************************************************************
         * 
         *  Find the marker, and pull the entire string before that marker first appears, not including that marker.
         *  null or empty string as a marker returns that input.  Think of use case: Running column values through a "LeftOf(',')"  Null input should not magically
         *  change into a value, i.e., a set of blanks or empty string!  By returning input, we say, "Nevermind, just pass thru
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LeftOfS(string input, string marker)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(marker)) return null;  // The test is invalid!  null is NOT a valid search value, so the result must represent INVALID parameter!

            var i = input.IndexOf(marker);
            if (i == NOT_FOUND) return string.Empty;  // Why?  So, "LeftOf('x', 'y') is '', because empty string represents NOTHING, where as null represents INVALID INPUTS. This may help chaining functions.  A null from a valid test will force any expression to null.  Is that what is desired?

            return input.Substring(0, i);
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
        public static string LeftOfNthS(string input, string marker, int n)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));

            var i = input.IndexOf(marker);
            if (i == NOT_FOUND) return null;
            if (n == 1) return input.Substring(0, i);
            int previous_i = NOT_FOUND;
            for (int j = 1; j < n; j++)
            {
                if (i == input.IndexOfLastC()) return string.Empty;
                previous_i = i;
                i = input.IndexOf(marker, i + marker.Length);
                if (i == NOT_FOUND) return string.Empty;
            }
            if (i >= input.IndexOfLastC()) return string.Empty;
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
        public static string LeftMOfNthS(string input, string marker, int n, int howmany)
        {
            int[] pointsfound = new int[n];
            if (string.IsNullOrEmpty(input)) return null;
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            if (n == 0) return null;
            if (howmany > n) return null;

            int i = 0;
            for (int j = 1; j <= n; j++)
            {
                if (i >= input.IndexOfLastC()) return string.Empty;
                pointsfound[j + BACKSET_FOR_ZEROBASED] = i;
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
        public static string LeftOfAnyC(string input, string markerchars)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(markerchars)) return input;

            var i = input.IndexOfAny(markerchars.ToCharArray()); // What about dups?
            if (i == NOT_FOUND) return string.Empty;

            return input.Substring(0, i);
        }

        /***************************************************************************************************************************************************************************************************
           * 
           * Microsoft, and language/compiler designers in general, are very reductive.  "Just use substring!"  
           * But that's not the point of these tiny functions.  
           * Methods are language.  Don't use a screwdriver as a hammer just to save walking back to your truck.  Do it right!  And in a way that is readable to other humans!
           * 
           **************************************************************************************************************************************************************************************/
        public static string Left(string input, int howmany)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (howmany == 0) return string.Empty;
            if (howmany < 0) return null;

            return input.Substring(0, howmany);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Extract the first word before a specific string
        * 
        ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FirstWordBeforeS(string input, string marker)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return input.Split(SingleStringAsArray(marker), StringSplitOptions.None)[0];
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Extract the first word before any of the characters in the passed string.  Smallest word returned.
        * 
        ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FirstWordBeforeAnyC(string input, string markerchars)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;

            int firstindex = input.IndexOfAny(markerchars.ToCharArray());
            if (firstindex < 1) return null;

            return Left(input, firstindex);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Extract the first word using regex word semantics.
        * 
        ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FirstWordW(string input)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;

            return input.SplitX(@"\W")[0];
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
            if (IsNullOrWhiteSpaceOrEmpty(FullName)) return FullName;

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

            return StringExtract.FirstWordW(firstName.Trim());
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Any string after this string is found.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RightOfS(string input, string marker)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(marker)) return input;

            var i = input.IndexOf(marker);
            if (i == NOT_FOUND) return string.Empty;

            return input.Substring(i + marker.Length);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Any string after this point is extracted. Either from the left or from the right.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RightOfN(string input, int n)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (n == 0) return input;
            if (n < 0 && Abs(n) >= input.Length) return input;
            if (n > input.Length) return input;  // No need to throw an exception.
            if (n < 0) return input.Right(Abs(n));  // See?  Simple.  If the negative was an error, then we'll have to deal with that.

            return input.Substring(n);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Any of the characters in marker, any string after any of those.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RightOfAnyC(string input, string markercharacters)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(markercharacters)) return input;

            var i = input.IndexOfAny(markercharacters.ToCharArray());
            if (i == NOT_FOUND) return string.Empty;
            return input.Substring(i + 1);
        }

        public static string Right(this string input, int howmany)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (howmany == 0) return string.Empty;
            if (howmany < 0) return null;

            var tupin = input.Reverse();

            return Left(tupin, howmany).Reverse();
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Convenience function to return the last piece
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPieces")]
        public static string LastPieceX(string input, string pattern)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            if (stringpieces?.Length > 0)
            {
                string laststring = stringpieces[stringpieces.Length - 1];
                if (string.IsNullOrWhiteSpace(laststring))
                {
                    if (stringpieces?.Length > 1)
                    {
                        laststring = stringpieces[stringpieces.Length - 2];
                    }
                }
                return laststring;
            }
            else
            {
                return null;
            }
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Extract a regex match
        * 
        ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string ExtractXi(string input, string pattern)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            if (stringpieces?.Length > 0)
            {
                string matchedstring = stringpieces[0];
                if (string.IsNullOrWhiteSpace(matchedstring))
                {
                    return null;
                }
                return matchedstring;
            }
            else
            {
                return null;
            }
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string ExtractX(string input, string pattern)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(2));
            int mini = 0;
            string matchedstring = null;
            while (true) {
                if (stringpieces?.Length > mini)
                {
                    matchedstring = stringpieces[mini];
                    if (!string.IsNullOrWhiteSpace(matchedstring))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                mini++;
            }

            return matchedstring;
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Extract everything after a specific string
        * 
        ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string EverythingAfterS(string input, string marker)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(marker)) return input;

            int i = input.FindIndexOf(marker);
            if (i == -1) return string.Empty;
            if (i + marker.Length > input.Length) return string.Empty;

            return input.Substring(i + marker.Length);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string EverythingAfterX(string input, string pattern)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            Match match = Regex.Match(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(2));

            if (match == null) return string.Empty;
            if (!match.Success) return string.Empty;
            return input.Substring(match.Index+match.Length);
        }


        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string ExtractPersonsName(string input)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            // Following tested on:
            // - Requested by Jeff Humphreys(3/1/2017 DL 128641) => Jeff Humphreys
            //string pattern = @"[Rr]eq.+ [Bb]y +([A-Z][a-z]+ +[A-Z][a-z]+)[(]";
            string pattern = @"[Rr]eq.*? [Bb]y +([A-Z][a-z]+ +[A-Z][a-z]+) *?[(\d\-]";

            Match match = Regex.Match(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(4));

            if (match == null) return string.Empty;
            if (!match.Success) return string.Empty;
            return match.Groups[1].Value;
        }

        public static string Reverse(this string input)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;

            char[] inputAsCharArray = input.ToCharArray();
            Array.Reverse(inputAsCharArray);
            return new string(inputAsCharArray);
        }

        private static string SafeSubstring(string input, int from, int howmanychar)
        {
            if (from < 0 || from >= input.Length || howmanychar + from > input.Length || howmanychar < 0)
            {
                ;
            }

            return input.Substring(from, howmanychar);
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * This is not at all the BASIC function Mid, but what the hey.  I need a snippet from a string, and I want the cleverness of supporting negatives as it's intuitive.
         * Matter of fact, I need substring to be a little smarter too.
         * 
         **************************************************************************************************************************************************************************************/
        public static string Mid(string input, int from, int to)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;

            if (from > 0 && to > 0 && from > to) return string.Empty;
            if (from < 0 && to < 0 && to < from) return string.Empty;
            if (from > 0 && to < 0)
            {
                if (input.Length - from + to <= 0) return string.Empty; // They ran past each other.
#pragma warning disable RCS1032 // Remove redundant parentheses.
                string x = SafeSubstring(input, from, input.Length - (from + Abs(to))); // Yikes!
#pragma warning restore RCS1032 // Remove redundant parentheses.
                return x;
            }

            if (from < 0 && to < 0)
            {
                int i = -from;
                string x = SafeSubstring(input.Right(i), 0, -(from - to)); // This use of "to" is very confusing!
                return x;
            }

            if (to >= from)
            {
                int adj_from = from + BACKSET_FOR_ZEROBASED;
                int converted_to_length = to - from + 1; // Offset to grab the character "to" is on.
                return SafeSubstring(input, adj_from, converted_to_length);
            }

            throw new ArgumentOutOfRangeException("Unable to determine what Mid from and to parameters are requesting.");
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * "Trick" pattern I made up.  An internal wrapper for fluent design.
         * 
         **************************************************************************************************************************************************************************************/

#pragma warning disable RCS1163 // Unused parameter.
#pragma warning disable IDE0060 // Remove unused parameter
        public static string MID(this string input, int from, int to, int extmethod = 0)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore RCS1163 // Unused parameter.
        {
            return Mid(input, from, to);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Return a specific piece by its index.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPieces")]
        public static string PieceNumberX(String input, String pattern, int piecenumbertoreturn)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;

            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            if (stringpieces != null && stringpieces.Length >= piecenumbertoreturn)
                return stringpieces[piecenumbertoreturn - 1];
            else
                return null;
        }

        public static int FindIndexOf(this string input, string marker)
        {
            return input.IndexOf(marker);
        }

        private static int IndexOfLastC(this String input)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return NOT_FOUND;

            return input.Length - 1;
        }
    }
}