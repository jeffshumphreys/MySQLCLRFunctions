using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MySQLCLRFunctions
{
    public static class StringSplit
    {
        /******************************************************************************************************************************************************************
         * 
         *     Internal pivoters that just make arrays for internal use and not exposed sets for SQL Server.
         *
         *****************************************************************************************************************************************************************/

        internal static string[] GetWordsW(this string input)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return new string[0];

            return input.SplitX(@"\W");
        }

        internal static string[] SplitX(this string input, string pattern)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return new string[0];
            return Regex.Split(input, pattern, RegexOptions.None);
        }

        public static (string, string) SplitIn2OnC(this string input, char marker, bool trim = true)
        {
            string[] parts = input.Split(marker.ToArray());
            if (parts.Length != 2)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (trim)
                return (parts[0].Trim(), parts[1].Trim());
            else
                return (parts[0], parts[1]);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Split out a line into discrete fields based on Regex capture groups.
         * 
         * 
         * ("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Meta", "Meta", "{5ADEF8D7-EBCC-4958-B0CC-060914E4FEE7}"
         * EndProject
         * 
         * Pattern identifying barriers around fields:
         * \("{(?<GUID>.*?)}"\) = "(?<Name>.*?)", "(?<ProjectFileName>.*?)", "{(?<ParentFolderId>.*?)}"
         *
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, FillRowMethodName = nameof(Pieces4AsSQLRow), TableDefinition = "col1 NVARCHAR(MAX), col2 NVARCHAR(MAX), col3 NVARCHAR(MAX), col4 NVARCHAR(MAX)")]
        public static IEnumerable SplitTo4ColumnsX(string input, string pattern)
        {
            MatchCollection regexmatches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(3));
            int nofmatches = regexmatches.Count;
            string[] distinctfieldvalues = new string[4];

            var rowoffields = new List<Pieces4Record>(1);
            if (nofmatches < 1) return rowoffields.ToArray();
            int j = 0;
            foreach (Group group in regexmatches[0].Groups)
            {
                if (j == 0) { j++; continue; } // Skip the first global capture
                var fieldvalue = group.ToString();
                distinctfieldvalues[j-1] = fieldvalue;  // Store in 0th element
                j++;
            }

            Pieces4Record fieldsasrecord = new Pieces4Record(distinctfieldvalues[0], distinctfieldvalues[1], distinctfieldvalues[2], distinctfieldvalues[3]);
            rowoffields.Add(fieldsasrecord);
            return rowoffields.ToArray();
        }

        // Called from SQL Server only
        private static void Pieces4AsSQLRow(Object obj, out SqlString col1, out SqlString col2, out SqlString col3, out SqlString col4)
        {
            var pieces = obj as Pieces4Record;
            col1 = pieces.col1;
            col2 = pieces.col2;
            col3 = pieces.col3;
            col4 = pieces.col4;
        }

        public class Pieces4Record
        {
            public string col1;
            public string col2;
            public string col3;
            public string col4;

            public Pieces4Record(string lcol1, string lcol2, string lcol3, string lcol4)
            {
                col1 = lcol1;
                col2 = lcol2;
                col3 = lcol3;
                col4 = lcol4;
            }
        }

    }
}
