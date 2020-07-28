using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

/*
* All splits, pieces, matches
*/
namespace MySQLCLRFunctions
{
    public static class StringPivot
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * Pull Out Matches by Regex
         * 
         * "Matches" plural is what I use to indicate a table valued function. I'll make Match for inline SQL pulls.
         * No need to add "Exp" since always pull out by expression.  Can't think of how else.
         * 
         **************************************************************************************************************************************************************************************/
        // The attribute [FillRowMethodName] is used only by Microsoft Visual Studio to automatically register the specified method as a TVF. It is not used by SQL Server.
        // For table-valued functions, the columns of the return table type cannot include timestamp columns or non-Unicode string data type columns (such as char, varchar, and text). 
        // The NOT NULL constraint is not supported.
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithCapStrs")]
        public static IEnumerable Matches(string input, string pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            MatchCollection regexmatches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));

            int nofmatches = regexmatches.Count;
            var matches = new List<CapturedMatches>(nofmatches);
            for (int i = 0; i < nofmatches; i++)
            {
                string match = regexmatches[i].Captures[0].ToString();
                int startsat = regexmatches[i].Captures[0].Index;
                string nextMatch = null;
                if (i < nofmatches - 1) nextMatch = regexmatches[i + 1].Captures[0].ToString();
                string previousMatch = null;
                if (i > 0) previousMatch = regexmatches[i - 1].Captures[0].ToString();

                matches.Add(new CapturedMatches(lmatchOrderNo: i + 1, lcapturedMatch: match, lcapturedmatchstartsat: startsat));
            }
            return matches.ToArray();
        }

        private static void FillRowWithCapStrs(Object ob, out SqlInt32 matchorderNo, out SqlString match, out SqlInt32 capturedmatchstartsat)
        {
            var capturedMatch = ob as CapturedMatches;
            matchorderNo = capturedMatch.matchOrderNo;
            match = new SqlString(capturedMatch.capturedMatch);
            capturedmatchstartsat = capturedMatch.capturedmatchstartsat;
        }

        //-----------------------------------------------------------------------------------------------------------
        // Helper class for Matches method.
        //-----------------------------------------------------------------------------------------------------------
        public class CapturedMatches
        {
            public int matchOrderNo; public string capturedMatch; public int capturedmatchstartsat;
            public CapturedMatches(int lmatchOrderNo, string lcapturedMatch, int lcapturedmatchstartsat)
            { matchOrderNo = lmatchOrderNo; capturedMatch = lcapturedMatch; capturedmatchstartsat = lcapturedmatchstartsat; }
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Pull Out Matches by Regex and include the pieces before and after the matched separation string, making it easier to detect patterns.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPiecesWithContext")]
        public static IEnumerable PiecesWithContext(String input, String pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));

            int nofpieces = stringpieces.Length;
            var pieces = new List<PieceContext>(nofpieces);
            for (int i = 0; i < nofpieces; i++)
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
        private static void FillRowWithStrPiecesWithContext(Object ob, out SqlInt32 pieceorderNo, out SqlString previousPiece, out SqlString piece, out SqlString nextPiece)
        {
            var pieceContext = ob as PieceContext;
            pieceorderNo = pieceContext.pieceOrderNo;
            previousPiece = pieceContext.previousPiece;
            piece = pieceContext.piece;
            nextPiece = pieceContext.nextPiece;
        }

        //-----------------------------------------------------------------------------------------------------------
        // Helper class for the PiecesWithContext only. The FillRowMethod only takes an object, so you can't send multiple
        // values to it.  This has to be public for tester to use to parse out.  SQL Server may need it too?
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
        public static IEnumerable Pieces(string input, string pattern)
        {
            returnpieceorderno = 0;
            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
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
        // values to it.  So we compile a class instance and send it as an object.
        //-----------------------------------------------------------------------------------------------------------
        public class PieceMatchContext
        {
            public int pieceOrderNo; public string previousPiece; public string matchAtStartOfPiece; public string piece; public string matchAtEndOfPiece; public string nextPiece;
            public PieceMatchContext(int lpieceOrderNo, string lpreviousPiece, string lmatchAtStartOfPiece, string lpiece, string lmatchAtEndOfPiece, string lnextPiece)
            { pieceOrderNo = lpieceOrderNo; previousPiece = lpreviousPiece; matchAtStartOfPiece = lmatchAtStartOfPiece; piece = lpiece; matchAtEndOfPiece = lmatchAtEndOfPiece; nextPiece = lnextPiece; }
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Split into Pieces by Regex match function, and capture what was matched, and even the match function in each returned row!
         * 
         * Created to support splitting out a message string for formatting into a RAISERROR format, but letting the caller use SQL_VARIANT for genericity.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithStrPiecesWithContextAndMatch")]
        public static IEnumerable PiecesWithMatches(String input, String pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            MatchCollection stringpieces = Regex.Matches(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(3)); // Must be case sensitive due to variations in type specification

            int nofpieces = stringpieces.Count;
            var pieces = new List<PieceMatchContext>(nofpieces);
            int nextpiecestartsat = 0;
            string matchAtStartOfPiece = null;

            for (int i = 0; i < nofpieces; i++)
            {
                string matchAtEndOfPiece = stringpieces[i].Value;
                int matchAtEndOfPieceAt = stringpieces[i].Index;
                string piece = StringExtract.Cut(input, nextpiecestartsat, matchAtEndOfPieceAt);
                nextpiecestartsat = matchAtEndOfPieceAt + matchAtEndOfPiece.Length;
                pieces.Add(new PieceMatchContext(lpieceOrderNo: i + 1, lpreviousPiece: null, lmatchAtStartOfPiece: matchAtStartOfPiece, lpiece: piece, lmatchAtEndOfPiece: matchAtEndOfPiece, lnextPiece: null)); ;
            }

            // Fill in the context information from previous and next pieces and matches.  This makes function use in SQL easier.

            for (int i = 0; i < pieces.Count; i++)
            {
                if (i < pieces.Count - 1)
                {
                    pieces[i].nextPiece = pieces[i + 1].piece;
                }

                if (i > 0)
                {
                    pieces[i].previousPiece = pieces[i - 1].piece;
                    pieces[i].matchAtStartOfPiece = pieces[i - 1].matchAtEndOfPiece;
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

        public static string[] GetWords(this string input)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return new string[0];

            return input.Split(@"\W");
        }

        public static string[] Split(this string input, string pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return new string[0];
            return Regex.Split(input, pattern, RegexOptions.None);
        }
    }
}
