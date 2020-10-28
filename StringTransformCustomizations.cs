using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace MySQLCLRFunctions
{
    // Horrible collection of things I made to help that I haven't used.

    public static class StringTransformCustomizations
    {
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string CleanUpSQLServerNameDelimiters(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            if (string.IsNullOrEmpty(input)) return input;
            string[] nameparts = new string[4];
            int howmanynameparts = 0;
            string currentnamepart = "";
            int currentnamepartidx = 0;
            int howmanyleftbrackets = 0, howmanyrightbrackets = 0, howmanydots = 0;
            int nestedintobracketslevel = 0;
            int charactersinthispart = 0;
            char? lastchar = null;
            foreach (char c in input)
            {
                lastchar = c;
                if (c == '.')
                {
                    if (nestedintobracketslevel == 0)
                    {
                        nameparts[currentnamepartidx] = currentnamepart;
                        currentnamepartidx++;
                        currentnamepart = "";
                        charactersinthispart = 0;
                        howmanynameparts++;
                    }
                }
                else if (c == '[')
                {
                    if (nestedintobracketslevel == 0)
                    {
                        if (charactersinthispart > 0)
                        {
                            return $"2:error while parsing {input}: on character {c} at namepart {currentnamepart} with number of characters in part = {charactersinthispart} and nested to {nestedintobracketslevel}";
                        }
                        else
                        {
                            nestedintobracketslevel++;
                        }
                    }
                    else
                    {
                        return $"2a:error while parsing {input}: on character {c} at namepart {currentnamepart} with number of characters in part = {charactersinthispart} and nested to {nestedintobracketslevel}";
                    }
                }
                else if (c == ']')
                {
                    if (nestedintobracketslevel <= 0)
                    {
                        return $"3:error while parsing {input}: on character {c} at namepart {currentnamepart} with number of characters in part = {charactersinthispart} and nested to {nestedintobracketslevel}";
                    }
                    if (charactersinthispart == 0)
                    {
                        return $"5:error while parsing {input}: on character {c} at namepart {currentnamepart} with number of characters in part = {charactersinthispart} and nested to {nestedintobracketslevel}";
                    }
                    else
                    {
                        nestedintobracketslevel--;
                    }
                }
                // instance separator?
                else
                {
                    charactersinthispart++;
                    currentnamepart += c;
                    // if > 128, error.
                    // Check for legal characters
                }
            }

            if (lastchar == '.')
            {
                return $"4:error while parsing {input}: on last character {lastchar} at namepart {currentnamepart} with number of characters in part = {charactersinthispart} and nested to {nestedintobracketslevel}";
            }

            if (charactersinthispart > 0) {
                howmanynameparts++;
                nameparts[currentnamepartidx] = currentnamepart;
            }

            string rebuiltname = "";
            int partno = 0;
            foreach (string namepart in nameparts)
            {
                if (partno >= howmanynameparts) break;
                if (string.IsNullOrWhiteSpace(namepart))
                {
                    if (howmanynameparts - partno == 2) rebuiltname += ".";
                    else
                    {
                        return $"6:error while parsing {input}: rebuiltname = {rebuiltname} with partno {partno} on last character {lastchar} at namepart {currentnamepart} with number of characters in part = {charactersinthispart} and nested to {nestedintobracketslevel}";
                    }
                }
                else
                {
                    if (partno > 0)
                    {
                        rebuiltname += '.';
                    }
                    rebuiltname += "[" + namepart + "]";
                }
                partno++;
            }

            return rebuiltname;
        }
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
                var temps = String.Join(" ", cleanlines).REPLACERECURSIVES("  ", " "); // After tabs newlines converted to spaces
                return temps.Replace(", ", ",").Replace(" =", "=").Replace("= ", "=").Replace(" (", "(").Replace("[", "").Replace("]", "").Replace(" begin ", "{")
                    .Replace(" end ", "}").REPLACEMATCHX("\bend\b?$", "}");
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
