using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

/*
* All splits, pieces, matches, keypairs
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

        //-----------------------------------------------------------------------------------------------------------
        // Helper class for Matches method.
        //-----------------------------------------------------------------------------------------------------------
        public class CapturedMatches
        {
            public int matchOrderNo; public string capturedMatch; public int capturedMatchStartsAt;
            public CapturedMatches(int lmatchOrderNo, string lcapturedMatch, int lcapturedmatchstartsat)
            { matchOrderNo = lmatchOrderNo; capturedMatch = lcapturedMatch; capturedMatchStartsAt = lcapturedmatchstartsat; }
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
            returnpieceorderno = 1;
            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            return stringpieces;
        }

        private static int returnpieceorderno = 0;

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
                string piece = StringExtract.Mid(input, nextpiecestartsat, matchAtEndOfPieceAt);
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

        private static void FillRowWithCapStrs(Object ob, out SqlInt32 matchorderNo, out SqlString match, out SqlInt32 capturedmatchstartsat)
        {
            var capturedMatch = ob as CapturedMatches;
            matchorderNo = capturedMatch.matchOrderNo;
            match = new SqlString(capturedMatch.capturedMatch);
            capturedmatchstartsat = capturedMatch.capturedMatchStartsAt;
        }

        // .Net SqlClient Data Provider: Msg 51000, Level 16, State 1, Line 95 Error: CANNOT ALTER ASSEMBLY failed because the required method "FillRowWithStrPiecesWithContextAndMatch" in type "MySQLCLRFunctions.StringPivot" was not found with the same signature in the updated assembly.
        //------------------------------------------------------------------------------------------------------
        private/* Cannot change to public without breaking assembly */ static void FillRowWithStrPiecesWithContext(Object ob, out SqlInt32 pieceorderNo, out SqlString previousPiece, out SqlString piece, out SqlString nextPiece)
        {
            var pieceContext = ob as PieceContext;
            pieceorderNo = pieceContext.pieceOrderNo;
            previousPiece = pieceContext.previousPiece;
            piece = pieceContext.piece;
            nextPiece = pieceContext.nextPiece;
        }

        //------------------------------------------------------------------------------------------------------
        private static void FillRowWithStrPieces(Object obj, out SqlString piece, out SqlInt32 pieceorderNo)
        {
            piece = obj.ToString();
            pieceorderNo = returnpieceorderno++;
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

        //-----------------------------------------------------------------------------------------------------------
        public class KeyPairHelper
        {
            public int pieceOrderNo; public string key; public string value;
            public KeyPairHelper(int lpieceOrderNo, string lkey, string lvalue)
            { pieceOrderNo = lpieceOrderNo; key = lkey; value = lvalue;  }
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Split into key pairs, and if the value is multiple values, split those out into multiple rows.
         * Created due to some websites having some great name to diminutive maps, and in a readable format.
         * 
         * Input: Edward => Ned, Ed, Eddy, Eddie\nHenry => Harry, Hal\nJacob => Jake
         * Output:
         *      Edward      Ned
         *      Edward      Ed
         *      Edward      Eddy
         *      Edward      Eddie
         *      Henry       Harry
         *      Henry       Hal
         *      Jacob       Jake
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithKeyPairs")]
        public static IEnumerable KeyValuePairsWithMultiValues(string input, string betweeneachkeyvaluepair, string betweenkeyandvalue, string betweensubvalues)
        {
            if (betweeneachkeyvaluepair == null) betweeneachkeyvaluepair = new string('\n', 1);
            if (betweenkeyandvalue == null) betweenkeyandvalue = " => ";
            if (betweensubvalues == null) betweensubvalues = ", ";
            string[] stringpieces = input.Split(betweeneachkeyvaluepair);
            int nofpieces = stringpieces.Length;
            var pieces = new List<KeyPairHelper>(nofpieces);
            for (int i = 0; i < nofpieces; i++)
            {
                string keypair = stringpieces[i];
                string[] splitkeypair = keypair.Split(betweenkeyandvalue);
                if (splitkeypair.Length != 2) continue;
                string lkey = splitkeypair[0]; string lval = splitkeypair[1];
                string[] splitmultivalues = lval.Split(betweensubvalues);
                foreach (string subval in splitmultivalues)
                { 
                    pieces.Add(new KeyPairHelper(i+1, lkey, subval));
                }
            }

            return pieces.ToArray();
        }

        //------------------------------------------------------------------------------------------------------
        private static void FillRowWithKeyPairs(Object obj, out SqlInt32 pieceorderNo, out SqlString keystring, out SqlString valuestring)
        {
            var pieceContext = obj as KeyPairHelper;
            pieceorderNo = pieceContext.pieceOrderNo;
            keystring = pieceContext.key;
            valuestring = pieceContext.value;
        }

        private static void FillRowWithGroupCapStrs(Object ob, out SqlInt32 matchorderNo, out SqlString match, out SqlInt32 capturedmatchstartsat)
        {
            var capturedMatch = ob as MatchedCaptures;
            matchorderNo = capturedMatch.matchOrderNo;
            match = new SqlString(capturedMatch.capturedMatch);
            capturedmatchstartsat = capturedMatch.capturedMatchStartsAt;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillRowWithGroupCapStrs")]
        public static IEnumerable Captures(string input, string pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            MatchCollection regexmatches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));

            int nofmatches = regexmatches.Count;
            var matches = new List<MatchedCaptures>(nofmatches);
            for (int i = 0; i < nofmatches; i++)
            {
                string match = ""; int startsat = -1;
                //TOCONSIDERDOING: string nextMatch = null;
                //TOCONSIDERDOING: string previousMatch = null;
                if (regexmatches[i].Groups.Count >= 1)
                {
                    match = regexmatches[i].Groups[1].Value;
                    startsat = regexmatches[i].Groups[1].Index;
                } else
                {
                    match = "";
                    startsat = -1;
                }

                //if (i < nofmatches - 1) nextMatch = regexmatches[i + 1].Groups[1].Value;
                //if (i > 0) previousMatch = regexmatches[i - 1].Groups[1].Value;

                matches.Add(new MatchedCaptures(lmatchOrderNo: i + 1, lcapturedMatch: match, lcapturedmatchstartsat: startsat));
            }
            return matches.ToArray();
        }

        //-----------------------------------------------------------------------------------------------------------
        // Helper class for Matches method.
        //-----------------------------------------------------------------------------------------------------------
        public class MatchedCaptures
        {
            public int matchOrderNo; public string capturedMatch; public int capturedMatchStartsAt;
            public MatchedCaptures(int lmatchOrderNo, string lcapturedMatch, int lcapturedmatchstartsat)
            { matchOrderNo = lmatchOrderNo; capturedMatch = lcapturedMatch; capturedMatchStartsAt = lcapturedmatchstartsat; }
        }
      }
}
