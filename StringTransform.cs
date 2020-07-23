using System;
using System.Collections;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System.Xml.Schema;
using System.Collections.Generic;

namespace MySQLCLRFunctions
{
    /*
     * 
     * Naming conventions for regex parameters
     * - find is for flat string
     * - regexmatchpattern when a matching process
     * - regexcapturepattern when it has to capture something
     * - match is a noun, what was matched to the pattern
     */
    public static class StringTransform
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * Pull Out Matches by Regex
         * 
         * - "Matches" plural is what I use to indicate a table valued function. I'll make Match for inline SQL pulls.
         * - No need to add "Exp" since always pull out by expression.  Can't think of how else.
         * 
         **************************************************************************************************************************************************************************************/
        // This attribute is used only by Microsoft Visual Studio to automatically register the specified method as a TVF. It is not used by SQL Server.
        // For table-valued functions, the columns of the return table type cannot include timestamp columns or non-Unicode string data type columns (such as char, varchar, and text). The NOT NULL constraint is not supported.
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithCapStrs")]
        public static IEnumerable Matches(String stringtoextractmatchesintorows, String regexcapturepattern)
        {
            Regex stringsplitCompiledRegex = new Regex(regexcapturepattern);
            string[] piece = stringsplitCompiledRegex.Split(stringtoextractmatchesintorows);
            return piece;
        }

        private static void FillRowWithCapStrs(Object matchObject, out SqlString piece, out SqlInt32 matchat)
        {
            piece = new SqlString(((Match)matchObject).Captures[0].Value);
            piece = new SqlString(((Match)matchObject).Groups[1].Value);
            matchat = ((Match)matchObject).Captures[0].Index;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Pull Out Matches by Regex and include the pieces before and after, making it easier to detect patterns.
         * 
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPiecesWithContext")]
        public static IEnumerable PiecesWithContext(String stringtosplitintopieces, String regexmatchpattern)
        {
            string[] stringpieces = Regex.Split(stringtosplitintopieces, regexmatchpattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));

            int nofpieces = stringpieces.Length;
            var pieces = new List<PieceContext>(nofpieces);
            for (int i=0;i < nofpieces; i++)
            {
                string piece = stringpieces[i];
                string nextPiece = null;
                if (i < nofpieces - 1) nextPiece = stringpieces[i + 1];
                string previousPiece = null;
                if (i > 0) previousPiece = stringpieces[i - 1];
                pieces.Add(new PieceContext(lpieceOrderNo: i + 1, lpreviousPiece: previousPiece, lpiece: piece, lnextPiece: nextPiece));
            }
            return pieces.ToArray();
        }

        //------------------------------------------------------------------------------------------------------
        private static void FillRowWithStrPiecesWithContext(Object obj, out SqlInt32 pieceorderNo, out SqlString previousPiece, out SqlString piece, out SqlString nextPiece)
        {
            var pieceContext = obj as PieceContext;
            pieceorderNo = pieceContext.pieceOrderNo;
            previousPiece = pieceContext.previousPiece;
            piece = pieceContext.piece;
            nextPiece = pieceContext.nextPiece;
        }

        //-----------------------------------------------------------------------------------------------------------
        // Helper class for the PiecesWithContext only. The FillRowMethod only takes an object, so you can't send multiple
        // values to it.  This has to be public for tester to use to parse out
        //-----------------------------------------------------------------------------------------------------------
        public class PieceContext
        {
            public int pieceOrderNo; public string previousPiece; public string piece; public string nextPiece;
            public PieceContext(int lpieceOrderNo, string lpreviousPiece, string lpiece, string lnextPiece)
            { pieceOrderNo = lpieceOrderNo; previousPiece = lpreviousPiece; piece = lpiece; nextPiece = lnextPiece; }
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Split into Pieces by Regex match function
         * 
         * - Very useful function, and simplified down from anything PAID SQL Sharp has.
         * - Trying to simplify and shorten names without over complicating, so I call it Pieces rather than RegExMatch.
         * - Not much else I can think of that I would split other than strings, so drop the "String" suffix.  DateTime_Split?  Numeric_Split?  Bit_Split?  Maybe.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPieces")]
        public static IEnumerable Pieces(String stringtosplitintopieces, String regexmatchpattern)
        {
            string[] stringpieces = Regex.Split(stringtosplitintopieces, regexmatchpattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            return stringpieces;
        }

        private static int returnpieceorderno = 0;
        //------------------------------------------------------------------------------------------------------
        private static void FillRowWithStrPieces(Object obj, out SqlString piece, out SqlInt32 pieceorderNo)
        {
            piece = obj.ToString();
            pieceorderNo = returnpieceorderno++;
        }

        //-----------------------------------------------------------------------------------------------------------
        // Helper class for the PiecesWithContext only. The FillRowMethod only takes an object, so you can't send multiple
        // values to it.  So we compile a class instance.
        //-----------------------------------------------------------------------------------------------------------
        public class PieceMatchContext
        {
            public int pieceOrderNo; public string previousPiece; public string matchAtStartOfPiece; public string piece; public string matchAtEndOfPiece; public string nextPiece;
            public PieceMatchContext(int lpieceOrderNo, string lpreviousPiece, string lmatchAtStartOfPiece, string lpiece, string lmatchAtEndOfPiece, string lnextPiece)
            { pieceOrderNo = lpieceOrderNo; previousPiece = lpreviousPiece; matchAtStartOfPiece = lmatchAtStartOfPiece;  piece = lpiece; matchAtEndOfPiece = lmatchAtEndOfPiece; nextPiece = lnextPiece; }
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Split into Pieces by Regex match function, and capture what was matched, and even the match function in each returned row!
         * 
         * - Created to support splitting out a message string for formatting into a RAISERROR format, but letting the caller use SQL_VARIANT.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPiecesWithContextAndMatch")]
        public static IEnumerable PiecesWithMatches(String stringtosplitintopieces, String regexmatchpattern)
        {
            MatchCollection stringpieces = Regex.Matches(stringtosplitintopieces, regexmatchpattern, RegexOptions.None, TimeSpan.FromSeconds(3)); // Must be case sensitive due to variations in type specification

            int nofpieces = stringpieces.Count;
            var pieces = new List<PieceMatchContext>(nofpieces);
            int nextpiecestartsat = 0;
            string matchAtStartOfPiece = null;

            for (int i = 0; i < nofpieces; i++)
            {
                string matchAtEndOfPiece = stringpieces[i].Value;
                int matchAtEndOfPieceAt = stringpieces[i].Index;
                string piece = StringExtract.Cut(stringtosplitintopieces, nextpiecestartsat, matchAtEndOfPieceAt);
                nextpiecestartsat = matchAtEndOfPieceAt + matchAtEndOfPiece.Length;
                pieces.Add(new PieceMatchContext(lpieceOrderNo: i + 1, lpreviousPiece: null, lmatchAtStartOfPiece: matchAtStartOfPiece, lpiece: piece, lmatchAtEndOfPiece: matchAtEndOfPiece, lnextPiece: null)); ;
            }

            // Fill in the context information from previous and next pieces and matches.  This makes function use in SQL easier.

            for (int i = 0; i < pieces.Count; i++)
            {
                if (i < pieces.Count-1)
                {
                    pieces[i].nextPiece = pieces[i+1].piece;
                }

                if (i > 0)
                {
                    pieces[i].previousPiece = pieces[i - 1].piece;
                    pieces[i].matchAtStartOfPiece = pieces[i-1].matchAtEndOfPiece;
                }
            }    

            return pieces.ToArray();
        }

        //------------------------------------------------------------------------------------------------------
        private static void FillRowWithStrPiecesWithContextAndMatch(Object obj, out SqlInt32 pieceorderNo, out SqlString previousPiece, out SqlString matchAtStartOfPiece, out SqlString piece, out SqlString matchAtEndOfPiece, out SqlString nextPiece)
        {
            var pieceContext = obj as PieceMatchContext;
            pieceorderNo = pieceContext.pieceOrderNo;
            previousPiece = pieceContext.previousPiece;
            matchAtStartOfPiece = pieceContext.matchAtStartOfPiece;
            piece = pieceContext.piece;
            matchAtEndOfPiece = pieceContext.matchAtEndOfPiece;
            nextPiece = pieceContext.nextPiece;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Return a specific piece by its index.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPieces")]
        public static string PieceNumber(String stringtosplitintopieces, String regexmatchpattern, int piecenumbertoreturn)
        {
            returnpieceorderno = 1;
            string[] stringpieces = Regex.Split(stringtosplitintopieces, regexmatchpattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
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
        public static string LastPiece(String stringtosplitintopieces, String regexmatchpattern)
        {
            returnpieceorderno = 1;
            string[] stringpieces = Regex.Split(stringtosplitintopieces, regexmatchpattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
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

        // Extensions for internal use and simpler Fluent design, but not for SQL to call
        internal static string ReplaceMatchExt(this string input, string regexmatchpattern, string replacement)
        {
            return ReplaceMatch(input, regexmatchpattern, replacement);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Similar to REPLACE in SQL Server, except it has .NET regex for matching
         * 
         * Note parameter naming convention: find is for flat string, regexmatchpattern when a matching process, regexcapturepattern when it has to capture something
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string ReplaceMatch(string input, string regexmatchpattern, string replacement)
        {
            return Regex.Replace(input, regexmatchpattern, replacement);
        }

        // Extensions for internal use and simpler Fluent design, but not for SQL to call
        internal unsafe static string ReplaceRecursiveExt(this string input, string find, string replacement)
        {
            return ReplaceRecursive(input, find, replacement);
        }

       /***************************************************************************************************************************************************************************************************
        * 
        * Replace again and again.  This is for when I want to remove all but one of the spaces, or all the "             " wide open spaces in a SQL proc header except for one space.
        * 
        * This is good as part of a series of "fluent" methods to reduce a proc to an easily read header string.  For quick review of a million modules.
        * 
        **************************************************************************************************************************************************************************************/
        unsafe public static string ReplaceRecursive(string input, string find, string replacement)
        {
            if (find == replacement) return input;
            if (replacement.Contains(find))
            {
                throw new ArgumentOutOfRangeException(nameof(replacement), "the replacement string contains the sought string, so recursion would cause a blowup.");
            }

            if (find.Length != replacement.Length)
            {
                StringBuilder sb = new StringBuilder(input);
                while (true)
                {
                    int formersize = sb.Length;
                    sb.Replace(find, replacement);
                    int newsize = sb.Length;
                    if (formersize == newsize)
                    {
                        return sb.ToString();
                    }
                }
            }
            else if (find.Length == replacement.Length)
            {
                int findlen = find.Length;
                var findchars = find.ToCharArray();
                int len = input.Length;
                fixed (char* pointer = input)
                {
                    // Add one to each of the characters.
                    for (int i = 0; i <= len - findlen; i++)
                    {
                        int j2 = 0;
                        for (int j = i; j < len; j++)
                        {
                            if (pointer[j + j2] != findchars[j2]) goto outerloop;
                            j2++;
                            if (j2 == findlen)
                            {
                                // Replace!!!!!!!!

                                pointer[j] = findchars[0];
                            }
                        }
                    outerloop:
                        ;
                    }
                    // Return the mutated string.
                    return new string(pointer);
                }
            }

            return input;
        }
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
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            StringBuilder sb = new StringBuilder(input.Length+10);
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

        /***************************************************************************************************************************************************************************************************
         * 
         * Removing those annoying brackets of any kind, BUT only when they're matching. This came up when an input domain column contained '(domain.com)'.  It's easy enough to write something inline,
         * but what if you mess up?  Do it safely, where the function name says what you're intent is, not some SUBSTRING(s, 2, len(s)-2).  That's not easy to read, and what about '(domain.xyz' where they are not paired?
         * My function just skips if they do not match.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string StripBracketing(string input)
        {
            // https://en.wikipedia.org/wiki/Bracket
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            if (input.Last() == ']' && input.First() == '[') return input.Mid(1, -1);
            if (input.Last() == '⦌' && input.First() == '⦋') return input.Mid(1, -1);
            if (input.Last() == '}' && input.First() == '{') return input.Mid(1, -1);
            if (input.Last() == '⟭' && input.First() == '⟬') return input.Mid(1, -1);//white tortoise shell brackets
            if (input.Last() == ')' && input.First() == '(') return input.Mid(1, -1);
            if (input.Last() == ':' && input.First() == ':') return input.Mid(1, -1);
            if (input.Last() == '⟧' && input.First() == '⟦') return input.Mid(1, -1);
            if (input.Last() == '>' && input.First() == '<') return input.Mid(1, -1);
            if (input.Last() == '⟩' && input.First() == '⟨') return input.Mid(1, -1);
            if (input.Last() == '⟫' && input.First() == '⟪') return input.Mid(1, -1);
            // Also, <<, >>
            // Les Guillemets
            if (input.Last() == '»' && input.First() == '«') return input.Mid(1, -1);

            if (input.Last() == '"' && input.First() == '"') return input.Mid(1, -1);
            if (input.Last() == '\'' && input.First() == '\'') return input.Mid(1, -1);
            if (input.Last() == '’' && input.First() == '‘') return input.Mid(1, -1); // opening/closing quote
            if (input.Last() == '”' && input.First() == '“') return input.Mid(1, -1); // opening/closing quote
            // HTML: &lsquo; &rsquo; &ldquo; &rdquo;
            // — (tiret)
            return input;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Microsoft, and language/compiler designers in general, are very reductive.  "Just use substring!"  That's not the point.  Methods are language.  Don't use a screwdriver as a hammer just to save walking back
         * to your truck.  Do it right!  And in a way that is readable to other humans!
         * 
         **************************************************************************************************************************************************************************************/
        public static string Left(this string input, int howmany)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            return input.Substring(0, howmany);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Overloading where others fear to tread.  ints, character arrays.  This isn't C though, so we won't get them confused.  int doesn't implicitly become a 4-character array!
         * 
         **************************************************************************************************************************************************************************************/
        public static string TrimEnd(this string input, int howmanycharactersofftheend)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            return input.Left(input.Length - howmanycharactersofftheend);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Not at all the BASIC function Mid, but what the hey.  I need a piece from a string, and I want the cleverness of supporting negatives as it's intuitive.
         * Matter of fact, I need substring to be a little smarter too.
         * 
         **************************************************************************************************************************************************************************************/
        public static string Mid(this string input, int from, int to)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
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
        /***************************************************************************************************************************************************************************************************
         * 
         * Trim repeating bunches of character off right of string
         * 
         * - Created for trimming "1.3930000000" off floating point number in sql which REFUSES to round.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RTrimChar(string input, string trimAllOccFromRight)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            return input.TrimEnd(trimAllOccFromRight.ToCharArray());
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Trim a specified number of characters off the right.  Much simpler than IIF(LEN(s) > 0 AND s IS NOT NULL), SUBSTRING(s, LEN(s) - 1), s)
         * 
         * - Created for trimming "1.3930000000" off floating point number in sql which REFUSES to round.
         * 
         * - Example of function that always returns a value that would fit inplace, and so does NOT need to copy string!
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RTrimN(string input, int howmanycharactersoffend)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input) || howmanycharactersoffend == 0) return input;
            if (input.Length <= howmanycharactersoffend) return string.Empty;
            if (howmanycharactersoffend < 0) throw new ArgumentOutOfRangeException(nameof(howmanycharactersoffend), howmanycharactersoffend, "Cannot have negate remove characters.");
            return input.Substring(0, input.Length - howmanycharactersoffend);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Prepend spaces on the left so that the text is right-justified.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LPad(string input, int padToLen)
        {
            return input.PadLeft(padToLen);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Append spaces on the right out to a fixed point.  This is for displaying to a text output.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RPad(string input, int padToLen)
        {
            return input.PadRight(padToLen);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Prepend spaces on the left so that the text is right-justified.  Could be zeroes, or X's.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LPadChar(string input, int padToLen, char padCh)
        {
            return input.PadLeft(padToLen, padCh);
        }

        /***************************************************************************************************************************************************************************************************
        * 
        * Append a specific character on the right out to a fixed point.
        * 
        **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RPadChar(string input, int padToLen, char padCh)
        {
            return input.PadRight(padToLen, padCh);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Remove any of instances of any of the list of strings given.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string BlankOut(string input, string blankanyofthese, string sep)
        {
            foreach (string i in blankanyofthese.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries))
            {
                input.Replace(i, String.Empty);
            }
            return input;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * Like "UPPER" in SQL Server, as opposed to ToUpper in C#/.NET.  I'm trying to avoid "To-" naming since I already use "Is-" naming and these are for SQL use.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string Title(string input)
        {
            return Regex.Replace(input, @"\b[a-z]\w+", delegate (Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }
    }
}
