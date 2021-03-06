﻿using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using static MySQLCLRFunctions.StringTest;
using static MySQLCLRFunctions._SharedConstants;

/*
* All splits, pieces, matches, keypairs
*/
namespace MySQLCLRFunctions
{
    public static class StringPivot
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * Pull Out Matches using Regex expressions
         * 
         * "Matches" plural is what I use to indicate a table valued function. I'll make Match for inline SQL pulls.
         * No need to add "Exp" since always pull out what the expression matches.  Can't think of how else.
         * See Pieces for the pieces in between the matches.
         * 
         **************************************************************************************************************************************************************************************/
        // The attribute [FillRowMethodName] is used only by Microsoft Visual Studio to automatically register the specified method as a TVF. It is not used by SQL Server.
        // For table-valued functions, the columns of the return table type cannot include timestamp columns or non-Unicode string data type columns (such as char, varchar, and text). 
        // The NOT NULL constraint is not supported.
        // Called from SQL Server only
        private static void MatchesAsSQLRow(Object ob, out SqlInt32 matchOrderNo, out SqlString match, out SqlString matchType, out SqlInt32 capturedMatchStartsAt, out SqlInt32 recNo)
        {
            var capturedMatch = ob as MatchesRecord;
            matchOrderNo = capturedMatch.matchOrderNo;
            match = new SqlString(capturedMatch.capturedMatch);
            matchType = new SqlString(capturedMatch.matchType);
            capturedMatchStartsAt = capturedMatch.capturedMatchStartsAt;
            recNo = capturedMatch.recNo;
        }
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = nameof(MatchesAsSQLRow)
            , TableDefinition = "matchOrderNo int, match NVARCHAR(MAX), matchType NVARCHAR(MAX), capturedMatchStartsAt int, recNo int")]
        public static IEnumerable MatchesX(string input, string pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            MatchCollection regexmatches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));

            int nofmatches = regexmatches.Count;
            var matches = new List<MatchesRecord>(nofmatches);
            int recno = 0;
            for (int i = 0; i < nofmatches; i++)
            {
                if (regexmatches[i].Groups.Count > 1)
                {
                    for (int j = 1; j < regexmatches[i].Groups.Count; j++)
                    {
                        string match = regexmatches[i].Groups[j].ToString();
                        int startsat = regexmatches[i].Groups[j].Index;
                        string matchtype = "content";
                        recno++;
                        matches.Add(new MatchesRecord(lmatchOrderNo: i + 1, lcapturedMatch: match, lmatchType: matchtype, lcapturedMatchStartsAt: startsat, lrecNo: recno));
                    }
                }
                else if (regexmatches[i].Groups.Count == 1)
                {
                    for (int j = 0; j < regexmatches[i].Groups.Count; j++)
                    {
                        string match = regexmatches[i].Groups[j].ToString();
                        int startsat = regexmatches[i].Groups[j].Index;
                        string matchtype = "entirematchincludingmatchtriggers";
                        recno++;
                        matches.Add(new MatchesRecord(lmatchOrderNo: i + 1, lcapturedMatch: match, lmatchType: matchtype, lcapturedMatchStartsAt: startsat, lrecNo: recno));
                    }
                }
                else if (regexmatches[i].Groups.Count == 0)
                {
                    for (int j = 0; j < regexmatches[i].Captures.Count; j++)
                    {
                        string match = regexmatches[i].Captures[j].ToString();
                        int startsat = regexmatches[i].Captures[j].Index;
                        string matchtype = "capture";
                        recno++;
                        matches.Add(new MatchesRecord(lmatchOrderNo: i + 1, lcapturedMatch: match, lmatchType: matchtype, lcapturedMatchStartsAt: startsat, lrecNo: recno));
                    }
                }
            }
            return matches.ToArray();
        }

        public class MatchesRecord
        {
            public int matchOrderNo;
            public string capturedMatch;
            public string matchType;
            public int capturedMatchStartsAt;
            public int recNo;

            public MatchesRecord(int lmatchOrderNo, string lcapturedMatch, string lmatchType, int lcapturedMatchStartsAt, int lrecNo)
            {
                matchOrderNo = lmatchOrderNo;
                capturedMatch = lcapturedMatch;
                matchType = lmatchType;
                capturedMatchStartsAt = lcapturedMatchStartsAt;
                recNo = lrecNo;
            }
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Pull Out Pieces by Regexing the separators and include the pieces before and after the matched separation string, making it easier to detect patterns.
         * 
         **************************************************************************************************************************************************************************************/
        // Cannot change from private to public without breaking assembly.
        // .Net SqlClient Data Provider: Msg 51000, Level 16, State 1, Line 95 Error: CANNOT ALTER ASSEMBLY failed because the required method "PieceWithMatchesFillRow" in type "MySQLCLRFunctions.StringPivot" was not found with the same signature in the updated assembly.
        // Called from SQL Server only
        private static void PiecesWithContextAsSQLRow(Object ob, out SqlInt32 pieceorderNo, out SqlString previousPiece, out SqlString piece, out SqlString nextPiece)
        {
            var pieceContext = ob as PiecesWithContextRecord;
            pieceorderNo = pieceContext.pieceOrderNo;
            previousPiece = pieceContext.previousPiece;
            piece = pieceContext.piece;
            nextPiece = pieceContext.nextPiece;
        }
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = nameof(PiecesWithContextAsSQLRow)
            , TableDefinition = "pieceOrderNo int, previousPiece NVARCHAR(MAX), piece NVARCHAR(MAX), nextPiece NVARCHAR(MAX)")]
        public static IEnumerable PiecesWithContextX(String input, String pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));

            int nofpieces = stringpieces.Length;
            var pieces = new List<PiecesWithContextRecord>(nofpieces);
            for (int i = 0; i < nofpieces; i++)
            {
                string piece = stringpieces[i];
                string nextPiece = null;
                if (i < nofpieces - 1) nextPiece = stringpieces[i + 1];
                string previousPiece = null;
                if (i > 0) previousPiece = stringpieces[i - 1];
                pieces.Add(new PiecesWithContextRecord(lpieceOrderNo: i + 1, lpreviousPiece: previousPiece, lpiece: piece, lnextPiece: nextPiece));
            }

            return pieces.ToArray();
        }

        public class PiecesWithContextRecord
        {
            public int pieceOrderNo;
            public string previousPiece;
            public string piece;
            public string nextPiece;

            public PiecesWithContextRecord(int lpieceOrderNo, string lpreviousPiece, string lpiece, string lnextPiece)
            {
                pieceOrderNo = lpieceOrderNo;
                previousPiece = lpreviousPiece;
                piece = lpiece;
                nextPiece = lnextPiece;
            }
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
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = nameof(PiecesAsSQLRow), TableDefinition = "piece NVARCHAR(MAX), int pieceorderno")]
        public static IEnumerable PiecesX(string input, string pattern)
        {
            returnpieceorderno = 1;
            string[] stringpieces = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
            return stringpieces;
        }

        private static int returnpieceorderno = 0;

        // Called from SQL Server only
        private static void PiecesAsSQLRow(Object obj, out SqlString piece, out SqlInt32 pieceorderNo)
        {
            piece = obj.ToString();
            pieceorderNo = returnpieceorderno++;
        }

        /***************************************************************************************************************************************************************************************************
          * 
          * Split into Pieces by Regex match function, and capture what was matched, and even the match function in each returned row!
          * 
          * Created to support splitting out a message string for formatting into a RAISERROR format, but letting the caller use SQL_VARIANT for genericity.
          * 
          **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = nameof(PieceWithMatchesAsSQLRow)
            , TableDefinition ="pieceOrderNo int, previousPiece NVARCHAR(MAX), matchAtStartOfPiece NVARCHAR(MAX), piece NVARCHAR(MAX), matchAtEndOfPiece NVARCHAR(MAX), nextPiece NVARCHAR(MAX)")]
         public static IEnumerable PiecesWithMatchesX(string input, string pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            MatchCollection stringPieces = Regex.Matches(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(3)); // Must be case sensitive due to variations in type specification

            int nofpieces = stringPieces.Count;
            var pieces = new List<PiecesWithMatchesRecord>(nofpieces);
            int lastMatchEndedAt = 0;
            for (int i = 0; i < nofpieces; i++)
            {
                string piece = stringPieces[i].Value;
                int pieceStartsAt = stringPieces[i].Index;
                string seperatingStringInFrontOfPiece = input.MID(from: lastMatchEndedAt + UPSET_TO_ONEBASED_FROM_ZEROBASED, to: pieceStartsAt + UPSET_TO_ONEBASED_FROM_ZEROBASED);
                lastMatchEndedAt = pieceStartsAt + piece.Length;
                pieces.Add(new PiecesWithMatchesRecord(lpieceOrderNo: i + 1, lpreviousPiece: null, lmatchAtStartOfPiece: seperatingStringInFrontOfPiece, lpiece: piece, lmatchAtEndOfPiece: piece, lnextPiece: null));
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

        // Called from SQL Server only
        private static void PieceWithMatchesAsSQLRow(Object obj, out SqlInt32 pieceorderNo, out SqlString previousPiece, out SqlString matchAtStartOfPiece, out SqlString piece, out SqlString matchAtEndOfPiece, out SqlString nextPiece)
        {
            var pieceContext = obj as PiecesWithMatchesRecord;
            pieceorderNo = pieceContext.pieceOrderNo;
            previousPiece = pieceContext.previousPiece;
            matchAtStartOfPiece = pieceContext.matchAtStartOfPiece;
            piece = pieceContext.piece;
            matchAtEndOfPiece = pieceContext.matchAtEndOfPiece;
            nextPiece = pieceContext.nextPiece;
        }

        public class PiecesWithMatchesRecord
        {
            public int pieceOrderNo;
            public string previousPiece;
            public string matchAtStartOfPiece;
            public string piece;
            public string matchAtEndOfPiece;
            public string nextPiece;

            public PiecesWithMatchesRecord(int lpieceOrderNo, string lpreviousPiece, string lmatchAtStartOfPiece, string lpiece, string lmatchAtEndOfPiece, string lnextPiece)
            {
                pieceOrderNo = lpieceOrderNo;
                previousPiece = lpreviousPiece;
                matchAtStartOfPiece = lmatchAtStartOfPiece;
                piece = lpiece;
                matchAtEndOfPiece = lmatchAtEndOfPiece;
                nextPiece = lnextPiece;
            }
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
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = nameof(KeyValuePairsWithMultiValuesAsSQLRow)
            , TableDefinition = "pieceOrderNo int, key NVARCHAR(MAX), value int")]
        public static IEnumerable KeyValuePairsWithMultiValuesS(string input, string betweeneachkeyvaluepair, string betweenkeyandvalue, string betweensubvalues)
        {
            if (betweeneachkeyvaluepair == null) betweeneachkeyvaluepair = new string('\n', 1);
            if (betweenkeyandvalue == null) betweenkeyandvalue = " => ";
            if (betweensubvalues == null) betweensubvalues = ", ";
            string[] stringpieces = input.SplitX(betweeneachkeyvaluepair);
            int nofpieces = stringpieces.Length;
            var pieces = new List<KeyValuePairsWithMultiValuesRecord>(nofpieces);
            for (int i = 0; i < nofpieces; i++)
            {
                string keypair = stringpieces[i];
                string[] splitkeypair = keypair.SplitX(betweenkeyandvalue);
                if (splitkeypair.Length != 2) continue;
                string lkey = splitkeypair[0]; string lval = splitkeypair[1];
                string[] splitmultivalues = lval.SplitX(betweensubvalues);
                foreach (string subval in splitmultivalues)
                {
                    pieces.Add(new KeyValuePairsWithMultiValuesRecord(i + 1, lkey, subval));
                }
            }

            return pieces.ToArray();
        }

        // Called from SQL Server only
        private static void KeyValuePairsWithMultiValuesAsSQLRow(Object obj, out SqlInt32 pieceorderNo, out SqlString keystring, out SqlString valuestring)
        {
            var pieceContext = obj as KeyValuePairsWithMultiValuesRecord;
            pieceorderNo = pieceContext.pieceOrderNo;
            keystring = pieceContext.key;
            valuestring = pieceContext.value;
        }

        public class KeyValuePairsWithMultiValuesRecord
        {
            public int pieceOrderNo;
            public string key;
            public string value;

            public KeyValuePairsWithMultiValuesRecord(int lpieceOrderNo, string lkey, string lvalue)
            {
                pieceOrderNo = lpieceOrderNo;
                key = lkey;
                value = lvalue;
            }
        }

        /******************************************************************************************************************************************************************
         * 
         *      Capture the actual Group match in the regex match, not the whole match itself.
         *
         *****************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = nameof(CapturesAsSQLRow)
            , TableDefinition = "matchOrderNo int, match NVARCHAR(MAX), capturedMatchStartsAt int")]
        public static IEnumerable CapturesX(string input, string pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            MatchCollection regexmatches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));

            int nofmatches = regexmatches.Count;
            var matches = new List<CapturesRecord>(nofmatches);

            int ij = 0;
            for (int i = 0; i < nofmatches; i++)
            {
                for (int j = 1; j <= regexmatches[i].Groups.Count; j++)
                {


                    string match;
                    int startsat;
                    int endsat;
                    match = regexmatches[i].Groups[j].Value;
                    startsat = regexmatches[i].Groups[j].Index;
                    endsat = regexmatches[i].Groups[j].Index + regexmatches[i].Groups[j].Length;
                    if (match.Length == 0 && startsat == 0)
                    {
                        // I get this weird empties
                    }
                    else
                    {
                        matches.Add(new CapturesRecord(lmatchOrderNo: ij + 1, lcapturedMatch: match, lcapturedmatchstartsat: startsat));
                        ij++;
                    }
                }
            }
            return matches.ToArray();
        }

        // Called from SQL Server only
        private static void CapturesAsSQLRow(Object ob, out SqlInt32 matchorderNo, out SqlString match, out SqlInt32 capturedmatchstartsat)
        {
            var capturedMatch = ob as CapturesRecord;
            matchorderNo = capturedMatch.matchOrderNo;
            match = new SqlString(capturedMatch.capturedMatch);
            capturedmatchstartsat = capturedMatch.capturedMatchStartsAt;
        }

        public class CapturesRecord
        {
            public int matchOrderNo;
            public string capturedMatch;
            public int capturedMatchStartsAt;

            public CapturesRecord(int lmatchOrderNo, string lcapturedMatch, int lcapturedmatchstartsat)
            {
                matchOrderNo = lmatchOrderNo;
                capturedMatch = lcapturedMatch;
                capturedMatchStartsAt = lcapturedmatchstartsat;
            }
        }

        /******************************************************************************************************************************************************************
          * 
          *      Like Match, but instead returns the context of the string matched, like a lucene search.  good for search for emails or mail references in sql text.
          *
          *****************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = nameof(NearAsSQLRow)
            , TableDefinition = "matchOrderNo int, match NVARCHAR(MAX), capturedMatchStartsAt int, capturedMatchContextStartsAt int, capturedMatchContextEndsAt int, capturedMatchContext NVARCHAR(MAX)")]
        public static IEnumerable NearX(string input, string pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;

            MatchCollection regexmatches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));

            int nofmatches = regexmatches.Count;
            var matches = new List<NearRecord>(nofmatches);
            int contextstartsat,  contextendsat, startsat;
            for (int i = 0; i < nofmatches; i++)
            {
                string match = string.Empty;
                string matchcontext = string.Empty;
                contextstartsat = contextendsat = startsat = NOT_FOUND;

                if (regexmatches[i].Groups.Count >= 1)
                {
                    match = regexmatches[i].Captures[0].Value;
                    startsat = regexmatches[i].Captures[0].Index;
                    contextstartsat = StringMeasure.Max(0, startsat - 20);
                    contextendsat = StringMeasure.MinOver(0, startsat + 20, input.Length);
                    matchcontext = input.MID(contextstartsat, contextendsat);
                }

                matches.Add(new NearRecord(lmatchOrderNo: i + 1, lcapturedMatch: match, lcapturedMatchStartsAt: startsat, lcapturedMatchContextStartsAt: contextstartsat
                    , lcapturedMatchContextEndsAt: contextendsat, lcapturedMatchContext: matchcontext));
            }
            return matches.ToArray();
        }

        // Called from SQL Server only
        private static void NearAsSQLRow(
              Object ob
            , out SqlInt32 matchorderNo
            , out SqlString match
            , out SqlInt32 capturedmatchstartsat
            , out SqlInt32 capturedMatchContextStartsAt
            , out SqlInt32 capturedMatchContextEndsAt
            , out SqlString capturedMatchContext
            )
        {
            var capturedMatch = ob as NearRecord;
            matchorderNo = capturedMatch.matchOrderNo;
            match = new SqlString(capturedMatch.capturedMatch);
            capturedmatchstartsat = capturedMatch.capturedMatchContextStartsAt;
            capturedMatchContextStartsAt = capturedMatch.capturedMatchStartsAt;
            capturedMatchContextEndsAt = capturedMatch.capturedMatchContextEndsAt;
            capturedMatchContext = capturedMatch.capturedMatchContext;
        }

        public class NearRecord
        {
            public int matchOrderNo;
            public string capturedMatch;
            public int capturedMatchStartsAt;
            public int capturedMatchContextStartsAt;
            public int capturedMatchContextEndsAt;
            public string capturedMatchContext;

            public NearRecord(int lmatchOrderNo,
                              string lcapturedMatch,
                              int lcapturedMatchStartsAt,
                              int lcapturedMatchContextStartsAt,
                              int lcapturedMatchContextEndsAt,
                              string lcapturedMatchContext)
            {
                matchOrderNo = lmatchOrderNo;
                capturedMatch = lcapturedMatch;
                capturedMatchStartsAt = lcapturedMatchStartsAt;
                capturedMatchContextStartsAt = lcapturedMatchContextStartsAt;
                capturedMatchContextEndsAt = lcapturedMatchContextEndsAt;
                capturedMatchContext = lcapturedMatchContext;
            }
        }

    }
}
