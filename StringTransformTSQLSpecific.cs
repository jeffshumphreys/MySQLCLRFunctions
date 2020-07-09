using System;
using System.Collections;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace MySQLCLRFunctions
{
    public static class StringTransformTSQLSpecific
    {
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RemoveSQLServerNameDelimiters(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            if (string.IsNullOrEmpty(input)) return input;

            input = input.Trim().Trim(new char[] { '[', ']' }).Replace("]]", "]");
            return input;
        }

        /// <summary>
        /// 'SELECT * FROM dbo.table where name = $3', 1, 'That''s my boy'
        /// </summary>
        /// <param name="sqlwithparametersembedded"></param>
        /// <param name="paramno"></param>
        /// <param name="newvalue"></param>
        /// <returns></returns>
        public static string ExpandSQLParameterString(string sqlwithparametersembedded, int paramno, string newvalue)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-escapes-in-regular-expressions
            newvalue = "'" + newvalue.Replace("'", "''") + "'";
            sqlwithparametersembedded = Regex.Replace(sqlwithparametersembedded, @"\$\d+(^\d|$)", newvalue);
            return sqlwithparametersembedded;
        }

        public static string ExpandSQLParameter(string sqlwithparametersembedded, int paramno, string newvalue)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-escapes-in-regular-expressions
            sqlwithparametersembedded = Regex.Replace(sqlwithparametersembedded, @"\$\d+(^\d|$)", newvalue);
            return sqlwithparametersembedded;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * StripDownSQLModule SQL Modules are hard to read as a single line with all the header junk.
         * Unsafe because uses pointers to improve performance the old fashioned C way for replacements that are 1 to 1 in length.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string StripDownSQLModule(string input, bool toSingleLine, bool dropFullLineComments)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            input = input.ToLowerInvariant(); // We loose pretty, but matching is better
            input = input.Replace("\t", " ").Replace("[dbo].", "").Replace(" as ", " ").Replace("N'", "'");

            input = StringTransform.ReplaceRecursive(input, "  ", " ");
            var lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var cleanlines = new List<string>(100);

            foreach (var line in lines)
            {
                if (Regex.IsMatch(line, "^--=+$")
                || Regex.IsMatch(line, "^---+$")
                || (dropFullLineComments && Regex.IsMatch(line.Trim(), "^--.*$")) // Dump comments
                || line.Trim().Equals("set nocount on", StringComparison.CurrentCultureIgnoreCase)
                )

                {
                    // Discard
                }
                else
                {
                    cleanlines.Add(line.TrimEnd(null));
                }
            }

            if (toSingleLine)
            {
                var temps = String.Join(" ", cleanlines).ReplaceRecursiveExt("  ", " "); // After tabs newlines converted to spaces
                temps = temps.Replace(", ", ",").Replace(" =", "=").Replace("= ", "=").Replace(" (", "(").Replace("[", "").Replace("]", "").Replace(" begin ", "{")
                    .Replace(" end ", "}").ReplaceMatchExt("\bend\b?$", "}");
                return temps;
            }
            return String.Join(Environment.NewLine, cleanlines);
        }
    }
}
