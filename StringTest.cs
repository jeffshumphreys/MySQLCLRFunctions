using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using static MySQLCLRFunctions.CharacterTransform;
namespace MySQLCLRFunctions
{
    // Functions that take input, are non-mutating, do not involve floating point or date time data, and return either true or false.
    public static class StringTest
    {
        /***************************************************************************************************************************************************************************************************
         * 
         *  Simple expanding name to avoid constant confusion I have with what this function does.
         *  Because most people don't equivocate white space with an empty space, which is the opposite of space.  It is non-space.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsNullOrWhiteSpaceOrEmpty(string input) { return string.IsNullOrWhiteSpace(input); }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsNullOrEmpty(string input) { return string.IsNullOrEmpty(input); }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsNull(string input) { return input == null; }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsEmpty(string input) { return input?.Length == 0; }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsEmptyOrWhiteSpace(string input) { return input?.Length == 0 || IsWhiteSpace(input); }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsNullOrWhiteSpace(string input) { return input == null || IsWhiteSpace(input); }

        /***************************************************************************************************************************************************************************************************
         * 
         *  White space is not empty!  Ask any human person besides a programmer!  An empty string has no space in it.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsWhiteSpace(string input)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsEmpty(input)) return false; // White space IS NOT empty string!!!! It exists!!!! Think about it!  Stop being a lazy programmer!

            foreach (var c in input)
            {
                if (!Char.IsWhiteSpace(c))
                    return false;
            }

            return true;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         *  Computer (Host) names vs. IP4 in the same columns.
         * 
         ***************************************************************************************************************************************************************************************************/

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsIP4(string input)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsEmpty(input)) return false; // Empty string doesn't start with anything

            if (IPAddress.TryParse(input, out IPAddress address))
            {
                if (address.AddressFamily is System.Net.Sockets.AddressFamily.InterNetwork)
                    return true;
            }

            return false;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         *  Computer (Host) names vs. IP6 in the same columns.  I'm not using this yet.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsIP6(string input)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsEmpty(input)) return false; // Empty string doesn't start with anything

            if (IPAddress.TryParse(input, out IPAddress address))
            {
                if (address.AddressFamily is System.Net.Sockets.AddressFamily.InterNetworkV6)
                    return true;
            }

            return false;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * Returns true if the input string starts with the sought string.
         * 
         ***************************************************************************************************************************************************************************************************/

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean StartsWithS(string input, string marker)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsNull(marker)) throw new ArgumentNullException("marker cannot be null");
            if (IsEmpty(marker)) throw new ArgumentNullException("marker cannot be empty. Would be an infinite loop.");
            if (IsEmpty(input)) return false; // Empty string doesn't start with anything

            return input.StartsWith(marker);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Returns true if the input string ends with the sought string.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean EndsWithS(string input, string marker)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsNull(marker)) throw new ArgumentNullException("marker cannot be null");
            if (IsEmpty(marker)) throw new ArgumentNullException("marker cannot be empty. Would be an infinite loop.");
            if (IsEmpty(input)) return false; // Empty string doesn't end with anything

            return input.EndsWith(marker);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * There are significant rules around server names.  This needs to be moved to StringTestCustomizations and renamed SQLLegalName, since it includes ODBC.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean LegalName(string input, string rule, string subdomain)
        {
            if (IsNull(input)) return SqlBoolean.Null;
            if (IsNull(rule)) throw new ArgumentNullException("character rule cannot be null");
            if (IsEmpty(rule)) throw new ArgumentNullException("rule cannot be empty. That would be a non-rule.");
            if (IsEmpty(input)) return false; // Empty string is never a legal name

            string localizeInput = input.Trim();

            if (rule == "SQL Server Server Name")
            {
                bool atLeastOneMatch = false;

                if ((bool)input.FIRSTC().NOTINX("[a-zA-Z_#@]")) return false;
                if (subdomain == "ODBC") {
                    atLeastOneMatch = true;
                    // List from https://docs.microsoft.com/en-us/sql/t-sql/language-elements/reserved-keywords-transact-sql?view=sql-server-ver15
                    if (input.IN("ABSOLUTE", "ACTION", "ADA", "ADD", "ALL", "ALLOCATE", "ALTER", "AND", "ANY", "ARE", "AS", "ASC", "ASSERTION", "AT", "AUTHORIZATION", "AVG", "BEGIN", "BETWEEN", "BIT", "BIT_LENGTH", "BOTH", "BY", "CASCADE", "CASCADED", "CASE", "CAST", "CATALOG", "CHAR", "CHAR_LENGTH", "CHARACTER", "CHARACTER_LENGTH", "CHECK", "CLOSE", "COALESCE", "COLLATE", "COLLATION", "COLUMN", "COMMIT", "CONNECT", "CONNECTION", "CONSTRAINT", "CONSTRAINTS", "CONTINUE", "CONVERT", "CORRESPONDING", "COUNT", "CREATE", "CROSS", "CURRENT", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATE", "DAY", "DEALLOCATE", "DEC", "DECIMAL", "DECLARE", "DEFAULT", "DEFERRABLE", "DEFERRED", "DELETE", "DESC", "DESCRIBE", "DESCRIPTOR", "DIAGNOSTICS", "DISCONNECT", "DISTINCT", "DOMAIN", "DOUBLE", "DROP", "ELSE", "END", "END-EXEC", "ESCAPE", "EXCEPT", "EXCEPTION", "EXEC", "EXECUTE", "EXISTS", "EXTERNAL", "EXTRACT", "FALSE", "FETCH", "FIRST", "FLOAT", "FOR", "FOREIGN", "FORTRAN", "FOUND", "FROM", "FULL", "GET", "GLOBAL", "GO", "GOTO", "GRANT", "GROUP", "HAVING", "HOUR", "IDENTITY", "IMMEDIATE", "IN", "INCLUDE", "INDEX", "INDICATOR", "INITIALLY", "INNER", "INPUT", "INSENSITIVE", "INSERT", "INT", "INTEGER", "INTERSECT", "INTERVAL", "INTO", "IS", "ISOLATION", "JOIN", "KEY", "LANGUAGE", "LAST", "LEADING", "LEFT", "LEVEL", "LIKE", "LOCAL", "LOWER", "MATCH", "MAX", "MIN", "MINUTE", "MODULE", "MONTH", "NAMES", "NATIONAL", "NATURAL", "NCHAR", "NEXT", "NO", "NONE", "NOT", "NULL", "NULLIF", "NUMERIC", "OCTET_LENGTH", "OF", "ON", "ONLY", "OPEN", "OPTION", "OR", "ORDER", "OUTER", "OUTPUT", "OVERLAPS", "PAD", "PARTIAL", "PASCAL", "POSITION", "PRECISION", "PREPARE", "PRESERVE", "PRIMARY", "PRIOR", "PRIVILEGES", "PROCEDURE", "PUBLIC", "READ", "REAL", "REFERENCES", "RELATIVE", "RESTRICT", "REVOKE", "RIGHT", "ROLLBACK", "ROWS", "SCHEMA", "SCROLL", "SECOND", "SECTION", "SELECT", "SESSION", "SESSION_USER", "SET", "SIZE", "SMALLINT", "SOME", "SPACE", "SQL", "SQLCA", "SQLCODE", "SQLERROR", "SQLSTATE", "SQLWARNING", "SUBSTRING", "SUM", "SYSTEM_USER", "TABLE", "TEMPORARY", "THEN", "TIME", "TIMESTAMP", "TIMEZONE_HOUR", "TIMEZONE_MINUTE", "TO", "TRAILING", "TRANSACTION", "TRANSLATE", "TRANSLATION", "TRIM", "TRUE", "UNION", "UNIQUE", "UNKNOWN", "UPDATE", "UPPER", "USAGE", "USER", "USING", "VALUE", "VALUES", "VARCHAR", "VARYING", "VIEW", "WHEN", "WHENEVER", "WHERE", "WITH", "WORK", "WRITE", "YEAR", "ZONE")
                        == true) return false;
                }
                else if (subdomain.IN("Keyword", "Instance") == true)
                {
                    atLeastOneMatch = true;
                    // List from https://docs.microsoft.com/en-us/sql/t-sql/language-elements/reserved-keywords-transact-sql?view=sql-server-ver15
                    if (input.IN("ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", "BACKUP", "BEGIN", "BETWEEN", "BREAK", "BROWSE", "BULK", "BY", "CASCADE", "CASE", "CHECK", "CHECKPOINT", "CLOSE", "CLUSTERED", "COALESCE", "COLLATE", "COLUMN", "COMMIT", "COMPUTE", "CONSTRAINT", "CONTAINS", "CONTAINSTABLE", "CONTINUE", "CONVERT", "CREATE", "CROSS", "CURRENT", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATABASE", "DBCC", "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DENY", "DESC", "DISK", "DISTINCT", "DISTRIBUTED", "DOUBLE", "DROP", "DUMP", "ELSE", "END", "ERRLVL", "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS", "EXIT", "EXTERNAL", "FETCH", "FILE", "FILLFACTOR", "FOR", "FOREIGN", "FREETEXT", "FREETEXTTABLE", "FROM", "FULL", "FUNCTION", "GOTO", "GRANT", "GROUP", "HAVING", "HOLDLOCK", "IDENTITY", "IDENTITY_INSERT", "IDENTITYCOL", "IF", "IN", "INDEX", "INNER", "INSERT", "INTERSECT", "INTO", "IS", "JOIN", "KEY", "KILL", "LEFT", "LIKE", "LINENO", "LOAD", "MERGE", "NATIONAL", "NOCHECK", "NONCLUSTERED", "NOT", "NULL", "NULLIF", "OF", "OFF", "OFFSETS", "ON", "OPEN", "OPENDATASOURCE", "OPENQUERY", "OPENROWSET", "OPENXML", "OPTION", "OR", "ORDER", "OUTER", "OVER", "PERCENT", "PIVOT", "PLAN", "PRECISION", "PRIMARY", "PRINT", "PROC", "PROCEDURE", "PUBLIC", "RAISERROR", "READ", "READTEXT", "RECONFIGURE", "REFERENCES", "REPLICATION", "RESTORE", "RESTRICT", "RETURN", "REVERT", "REVOKE", "RIGHT", "ROLLBACK", "ROWCOUNT", "ROWGUIDCOL", "RULE", "SAVE", "SCHEMA", "SECURITYAUDIT", "SELECT", "SEMANTICKEYPHRASETABLE", "SEMANTICSIMILARITYDETAILSTABLE", "SEMANTICSIMILARITYTABLE", "SESSION_USER", "SET", "SETUSER", "SHUTDOWN", "SOME", "STATISTICS", "SYSTEM_USER", "TABLE", "TABLESAMPLE", "TEXTSIZE", "THEN", "TO", "TOP", "TRAN", "TRANSACTION", "TRIGGER", "TRUNCATE", "TRY_CONVERT", "TSEQUAL", "UNION", "UNIQUE", "UNPIVOT", "UPDATE", "UPDATETEXT", "USE", "USER", "VALUES", "VARYING", "VIEW", "WAITFOR", "WHEN", "WHERE", "WHILE", "WITH", "WITHIN GROUP", "WRITETEXT")
                        == true) return false;
                }
                else if (subdomain.IN("Future Keyword", "Instance") == true)
                {
                    atLeastOneMatch = true;
                    // List from https://docs.microsoft.com/en-us/sql/t-sql/language-elements/reserved-keywords-transact-sql?view=sql-server-ver15
                    if (input.IN("ABSOLUTE", "ACTION", "ADMIN", "AFTER", "AGGREGATE", "ALIAS", "ALLOCATE", "ARE", "ARRAY", "ASENSITIVE", "ASSERTION", "ASYMMETRIC", "AT", "ATOMIC", "BEFORE", "BINARY", "BIT", "BLOB", "BOOLEAN", "BOTH", "BREADTH", "CALL", "CALLED", "CARDINALITY", "CASCADED", "CAST", "CATALOG", "CHAR", "CHARACTER", "CLASS", "CLOB", "COLLATION", "COLLECT", "COMPLETION", "CONDITION", "CONNECT", "CONNECTION", "CONSTRAINTS", "CONSTRUCTOR", "CORR", "CORRESPONDING", "COVAR_POP", "COVAR_SAMP", "CUBE", "CUME_DIST", "CURRENT_CATALOG", "CURRENT_DEFAULT_TRANSFORM_GROUP", "CURRENT_PATH", "CURRENT_ROLE", "CURRENT_SCHEMA", "CURRENT_TRANSFORM_GROUP_FOR_TYPE", "CYCLE", "DATA", "DATE", "DAY", "DEC", "DECIMAL", "DEFERRABLE", "DEFERRED", "DEPTH", "DEREF", "DESCRIBE", "DESCRIPTOR", "DESTROY", "DESTRUCTOR", "DETERMINISTIC", "DIAGNOSTICS", "DICTIONARY", "DISCONNECT", "DOMAIN", "DYNAMIC", "EACH", "ELEMENT", "END-EXEC", "EQUALS", "EVERY", "EXCEPTION", "FILTER", "FIRST", "FLOAT", "FOUND", "FREE", "FULLTEXTTABLE", "FUSION", "GENERAL", "GET", "GLOBAL", "GO", "GROUPING", "HOLD", "HOST", "HOUR", "IGNORE", "IMMEDIATE", "INDICATOR", "INITIALIZE", "INITIALLY", "INOUT", "INPUT", "INT", "INTEGER", "INTERSECTION", "INTERVAL", "ISOLATION", "ITERATE", "LANGUAGE", "LARGE", "LAST", "LATERAL", "LEADING", "LESS", "LEVEL", "LIKE_REGEX", "LIMIT", "LN", "LOCAL", "LOCALTIME", "LOCALTIMESTAMP", "LOCATOR", "MAP", "MATCH", "MEMBER", "METHOD", "MINUTE", "MOD", "MODIFIES", "MODIFY", "MODULE", "MONTH", "MULTISET", "NAMES", "NATURAL", "NCHAR", "NCLOB", "NEW", "NEXT", "NO", "NONE", "NORMALIZE", "NUMERIC", "OBJECT", "OCCURRENCES_REGEX", "OLD", "ONLY", "OPERATION", "ORDINALITY", "OUT", "OUTPUT", "OVERLAY", "PAD", "PARAMETER", "PARAMETERS", "PARTIAL", "PARTITION", "PATH", "PERCENT_RANK", "PERCENTILE_CONT", "PERCENTILE_DISC", "POSITION_REGEX", "POSTFIX", "PREFIX", "PREORDER", "PREPARE", "PRESERVE", "PRIOR", "PRIVILEGES", "RANGE", "READS", "REAL", "RECURSIVE", "REF", "REFERENCING", "REGR_AVGX", "REGR_AVGY", "REGR_COUNT", "REGR_INTERCEPT", "REGR_R2", "REGR_SLOPE", "REGR_SXX", "REGR_SXY", "REGR_SYY", "RELATIVE", "RELEASE", "RESULT", "RETURNS", "ROLE", "ROLLUP", "ROUTINE", "ROW", "ROWS", "SAVEPOINT", "SCOPE", "SCROLL", "SEARCH", "SECOND", "SECTION", "SENSITIVE", "SEQUENCE", "SESSION", "SETS", "SIMILAR", "SIZE", "SMALLINT", "SPACE", "SPECIFIC", "SPECIFICTYPE", "SQL", "SQLEXCEPTION", "SQLSTATE", "SQLWARNING", "START", "STATE", "STATEMENT", "STATIC", "STDDEV_POP", "STDDEV_SAMP", "STRUCTURE", "SUBMULTISET", "SUBSTRING_REGEX", "SYMMETRIC", "SYSTEM", "TEMPORARY", "TERMINATE", "THAN", "TIME", "TIMESTAMP", "TIMEZONE_HOUR", "TIMEZONE_MINUTE", "TRAILING", "TRANSLATE_REGEX", "TRANSLATION", "TREAT", "UESCAPE", "UNDER", "UNKNOWN", "UNNEST", "USAGE", "USING", "VALUE", "VAR_POP", "VAR_SAMP", "VARCHAR", "VARIABLE", "WHENEVER", "WIDTH_BUCKET", "WINDOW", "WITHIN", "WITHOUT", "WORK", "WRITE", "XMLAGG", "XMLATTRIBUTES", "XMLBINARY", "XMLCAST", "XMLCOMMENT", "XMLCONCAT", "XMLDOCUMENT", "XMLELEMENT", "XMLEXISTS", "XMLFOREST", "XMLITERATE", "XMLNAMESPACES", "XMLPARSE", "XMLPI", "XMLQUERY", "XMLSERIALIZE", "XMLTABLE", "XMLTEXT", "XMLVALIDATE", "YEAR", "ZONE", "FALSE", "TRUE")
                        == true) return false;
                }

                if (subdomain == "Instance")
                {
                    atLeastOneMatch = true;
                    // http://learnsqlwithbru.com/2011/12/01/rules-to-follow-while-naming-a-sql-server-instance/
                    if (input.Length > 16) return false;
                }


                // Instance be up to 16 characters, Unicode Standard 2.0, decimal numbers basic latin or other national scripts, $, #, _
                // First character must be letter, &, _, #
                // No embedded spaces, special characters, backslash, comma, colon, or at sign.

                // Windows Server 2008 R2 NetBIOS limited to 15 char


                // recog can happen under a variety of unpredictable branches, so just track when there was at least one subdomain recognized.
                // If none were recognized, then: a) error? NotImplemented? NULL.
                if (!atLeastOneMatch) return SqlBoolean.Null; // Can't say if it's legal or not!
                return true; // Got through the gauntlet. 
            }
            else
            {
                throw new ArgumentException($"Unrecognize rule: {rule}");
            }
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Any of these substrings are in the delimited list of sought substrings.  I would default the delimiter, but SQL Server doesn't care for optional parameters.
         * 
         ***************************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean AnyOfTheseSAreAnyOfThoseS(string inputs, string markers, string sep)
        {
            if (IsNull(inputs) || IsNull(markers) || IsNull(sep)) return SqlBoolean.Null;
            if (inputs.Length == 0) return false; // Nothing can be in an empty string
            if (markers.Length == 0 || sep.Length == 0) throw new ArgumentOutOfRangeException("Empty strings in a search make no sense.");

            var inputsasarray = inputs.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string i in markers.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (inputsasarray.Contains(i))
                {
                    return true;
                }
            }

            return false;
        }
        /***************************************************************************************************************************************************************************************************
         * 
         * SQL style like for multiple strings.  So "Car%;%X[0-9]%;"  Probably just regex strings, but I don't know.  But caller will have to put "^" and "$" around string if they want exact matches.
         * Need working examples.  Haven't tested.
         * 
         * Idea is: SELECT * FROM x where dbo.LikeAnyX(FullName, '%Humphreys;Humphrey%;JSH;%Jeff%Hum%;Jeff%H;(Jeff|Jeffrey|Jeffry);') = 1
         * 
         * Weirdness: When I name something "Like" in C# for SQLCLR, it means I can pass "%" as control.
         * 
         ***************************************************************************************************************************************************************************************************/

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean LikeAnyX(string inputs, string patterns, string inputsep, string patternsep)
        {
            if (IsNull(inputs) || IsNull(patterns) || IsNull(inputsep) || IsNull(patternsep)) return SqlBoolean.Null; // SQL: Nulls make null
            if (inputs.Length == 0) return false; // Nothing can be in an empty string
            if (patterns.Length == 0 || inputsep.Length == 0 || patternsep.Length == 0) throw new ArgumentOutOfRangeException("Empty strings in a search make no sense.");

            var inputsasarray = inputs.Split(new string[] { inputsep }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pattern in patterns.Split(new string[] { inputsep }, StringSplitOptions.RemoveEmptyEntries))
            {
                string regexpattern = pattern;
                if (regexpattern.StartsWith("%")) regexpattern = ReplaceFirstC(regexpattern, '^');
                if (regexpattern.EndsWith("%")) regexpattern = ReplaceLastC (regexpattern, '$');
                regexpattern = regexpattern.Replace("%", ".+"); // Not a great solution.  Need to test variations.

                foreach (string input in inputsasarray)
                {
                    if (Regex.IsMatch(input, pattern)) return true;
                }
            }

            return false;
        }

        public static bool ValidateRegex(string pattern)
        {
            if (IsNull(pattern)) throw new ArgumentNullException("Null patterns don't work.");
            if (IsEmpty(pattern)) throw new ArgumentException("Empty patterns should be checked. Why are they empty?");
            try
            {
                var regex = new Regex(pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch
            {
                throw;
            }

            /*
            .Net SqlClient Data Provider: Msg 6522, Level 16, State 1, Line 441
            A.NET Framework error occurred during execution of user-defined routine or aggregate "LegalName": 
            System.ArgumentException: parsing "[a-zA-Z\&\_\#]" - Unrecognized escape sequence \_.
            System.ArgumentException: 
               at System.Text.RegularExpressions.RegexParser.ScanCharEscape()
               at System.Text.RegularExpressions.RegexParser.ScanCharClass(Boolean caseInsensitive, Boolean scanOnly)
               at System.Text.RegularExpressions.RegexParser.CountCaptures()
               at System.Text.RegularExpressions.RegexParser.Parse(String re, RegexOptions op)
               at System.Text.RegularExpressions.Regex..ctor(String pattern, RegexOptions options, TimeSpan matchTimeout, Boolean useCache)
               at System.Text.RegularExpressions.Regex.IsMatch(String input, String pattern)
               at MySQLCLRFunctions.CharacterTest.NotInX(Nullable`1 input, String pattern)
               at MySQLCLRFunctions.StringTest.LegalName(String input, String rule)
            .
            */
            return true;
        }
        /*
         *
         *          var str = "White Red Blue Green Yellow Black Gray";
         *          var achromaticColors = new[] {"White", "Black", "Gray"};
         *          var exquisiteColors = new[] {"FloralWhite", "Bistre", "DavyGrey"};
         *          str = str.ReplaceAll(achromaticColors, exquisiteColors);
         *          // str == "FloralWhite Red Blue Green Yellow Bistre DavyGrey"
         *
         *      public static string ReplaceAll(this string value, IEnumerable<string> oldValues, IEnumerable<string> newValues)
         *      {
         *       var sbStr = new StringBuilder(value);
         *       var newValueEnum = newValues.GetEnumerator();
         *       foreach (var old in oldValues)
         *       {
         *           if (!newValueEnum.MoveNext())
         *               throw new ArgumentOutOfRangeException("newValues", "newValues sequence is shorter than oldValues sequence");
         *           sbStr.Replace(old, newValueEnum.Current);
         *       }
         *       if (newValueEnum.MoveNext())
         *           throw new ArgumentOutOfRangeException("newValues", "newValues sequence is longer than oldValues sequence");
         *      
         *       return sbStr.ToString();
         *       }
         */
    }
}
