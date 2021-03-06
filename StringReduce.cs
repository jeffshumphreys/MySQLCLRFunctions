﻿using Microsoft.SqlServer.Server;
using System;
using System.Linq;
using static MySQLCLRFunctions.StringTest;
using static MySQLCLRFunctions.StringExtract;
using static MySQLCLRFunctions._SharedConstants;

namespace MySQLCLRFunctions
{
    public static class StringReduce
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * Removing those annoying brackets of any kind, BUT only when they're matching. This came up when an input domain column contained '(domain.com)'.  It's easy enough to write something inline,
         * but what if you mess up?  Do it safely, where the function name says what you're intent is.
         * This is instead of SUBSTRING(s, 2, len(s)-2) (Note that this will blow up on 0 or 1 character strings).  
         * It's not as easy to skim-read, and corner-cases always eventually work their way back in to the expression. What about '(domain.xyz' where they are not paired?
         * My function just skips if they do not match, but it could be enhanced to throw an error, perhaps on a context setting "throw error if function not effected"
         * 
         * Usefulness Analysis: I need to clean some long strings with parenthised words, and it worked!
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string TrimBrackets(string input)
        {
            // https://en.wikipedia.org/wiki/Bracket
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;

            if (input.First() == '[' && input.Last() == ']' ) return input.MID(1, -1);
            if (input.First() == '⦋' && input.Last() == '⦌'  ) return input.MID(1, -1);
            if (input.First() == '{' && input.Last() == '}' ) return input.MID(1, -1);
            if (input.First() == '⟬' && input.Last() == '⟭' ) return input.MID(1, -1);//white tortoise shell brackets
            if (input.First() == '(' && input.Last() == ')' ) return input.MID(1, -1);
            if (input.First() == ':' && input.Last() == ':' ) return input.MID(1, -1);
            if (input.First() == '⟦' && input.Last() == '⟧' ) return input.MID(1, -1);
            if (input.First() == '<' && input.Last() == '>' ) return input.MID(1, -1);
            if (input.First() == '⟨' && input.Last() == '⟩'  ) return input.MID(1, -1);
            if (input.First() == '⟪' && input.Last() == '⟫' ) return input.MID(1, -1);
            if (input.First() == '«' && input.Last() == '»') return input.MID(1, -1);  // Les Guillemets

            if (input.First() == '"' && input.Last() == '"') return input.MID(1, -1);
            if ( input.First() == '\\' && input.Last() == '\\') return input.MID(1, -1);
            if (input.First() == '‘' && input.Last() == '’') return input.MID(1, -1); // opening/closing quote
            if (input.First() == '“' && input.Last() == '”') return input.MID(1, -1); // opening/closing quote
            // HTML: &lsquo; &rsquo; &ldquo; &rdquo;
            // — (tiret)

            return input;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * This removes any of substrings in the input that in the list of strings given.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string BlankOut(string input, string blankanyofthese, string sep)
        {
            if (IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(blankanyofthese)) return input;
            if (IsNullOrWhiteSpaceOrEmpty(sep)) return input;

            foreach (string i in blankanyofthese.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries))
            {
                input.Replace(i, String.Empty);
            }

            return input;
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Overloading where others fear to tread.  ints, character arrays.  This isn't C though, so we won't get them confused.  In C#, int doesn't implicitly become a 4-character array!
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string TrimEnd(string input, int howmany)
        {
            if (IsNullOrEmpty(input)) return input;
            if (howmany >= input.Length) return string.Empty;

            return Left(input, input.Length - howmany);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Very minor but clarifying. So common, and one-off errors are so frustrating.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RTrimOne(string input)
        {
            if (IsNullOrEmpty(input)) return input;

            return Left(input, input.Length - 1);
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
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;

            if (input.Length <= howmanycharactersoffend) return string.Empty;
            if (howmanycharactersoffend < 0) throw new ArgumentOutOfRangeException(nameof(howmanycharactersoffend), howmanycharactersoffend, "Cannot have negate remove characters.");

            return input.Substring(0, input.Length - howmanycharactersoffend);
        }
        /***************************************************************************************************************************************************************************************************
          * 
          * Trim repeating bunches of character off right of string
          * Created for trimming "1.3930000000" off floating point number in sql which REFUSES to round.
          * 
          **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RTrimAnyC(string input, string markerchars)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(markerchars)) return input;

            return input.TrimEnd(markerchars.ToCharArray());
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LTrimOne(string input)
        {
            if (IsNullOrEmpty(input)) return input;

            return input.Substring(1);
        }

        /***************************************************************************************************************************************************************************************************
       * 
       * Trims off the string if it matches the beginning.  Like a scoped replace.
       * 
       **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LTrimIfStartsWithS(string input, string marker)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(marker)) return input;

            if (input.StartsWith(marker))
            {
                return input.Substring(input.IndexOf(marker) + marker.Length);
            }
            return input;
        }

        /***************************************************************************************************************************************************************************************************
          * 
          * Trim (n) letters off the beginning of a string.
          * NAMING ERROR: NameLeft or LTrim???  LTrimN would align with SQL Server LTRIM.
          * 
          **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LTrimN(string input, int nofChar2Remove)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (input.Length <= nofChar2Remove) return string.Empty;

            return input.Substring(nofChar2Remove);
        }
    }
}
