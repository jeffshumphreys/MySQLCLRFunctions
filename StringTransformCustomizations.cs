﻿using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MySQLCLRFunctions
{
    // Horrible collection of things I made to help that I haven't used.

    public static class StringTransformCustomizations
    {
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RemoveSQLServerNameDelimiters(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            if (string.IsNullOrEmpty(input)) return input;

            return input.Trim().Trim(new char[] { '[', ']' }).Replace("]]", "]");
        }

        public static string ExpandSQLParameterString(string sqlwithparametersembedded, int parameterno, string newvalue)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-escapes-in-regular-expressions
            newvalue = "'" + newvalue.Replace("'", "''") + "'";
            return Regex.Replace(sqlwithparametersembedded, $@"\$\{parameterno}(^\d|$)", newvalue);
        }

        public static string ExpandSQLParameter(string sqlwithparametersembedded, int parameterno, string newvalue)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-escapes-in-regular-expressions
            return Regex.Replace(sqlwithparametersembedded, $@"\$\{parameterno}(^\d|$)", newvalue);
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * SQL Modules are hard to read as a single line with all the header junk.
         * Unsafe because uses pointers to improve performance the old fashioned C way for replacements that are 1 to 1 in length.
         * 
         * Possibly move to StringReduce.
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string TrimSQL(string input, bool toSingleLine, bool dropFullLineComments)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            input = input.ToLowerInvariant(); // We loose pretty, but matching is better
            input = input.Replace("\t", " ").Replace("[dbo].", "").Replace(" as ", " ").Replace("N'", "'");

            input = StringTransform.ReplaceRecursiveS(input, "  ", " ");
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
                return temps.Replace(", ", ",").Replace(" =", "=").Replace("= ", "=").Replace(" (", "(").Replace("[", "").Replace("]", "").Replace(" begin ", "{")
                    .Replace(" end ", "}").ReplaceMatchExt("\bend\b?$", "}");
            }
            return String.Join(Environment.NewLine, cleanlines);
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * Not using
         * 
         **************************************************************************************************************************************************************************************/
#if false
        [Obsolete]
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string BuildRaiserrorMessage(string message, string Param1 = null, string Param2 = null, string Param3 = null, string Param4 = null, string Param5 = null
    , string Param6 = null, string Param7 = null, string Param8 = null, string Param9 = null, string Param10 = null)
        {
            StringBuilder cmd = new StringBuilder(2048);
            cmd.Append("RAISERROR('").Append(message).Append("',");

            if (Param1 != null)
            {
                string[] param1parts = Param1.Split(':');
                string param1val = param1parts[0];
                string param1type = param1parts[1];
                //if (param1type == "string")
            }
            return null;
        }
#endif
    }
}
