using Microsoft.SqlServer.Server;
using System;
using System.Text;
using System.Text.RegularExpressions;

/*
 * Standard single-letter abbreviations:
 * - C - Single Character, or a string of characters
 * - S - A variable line single string treated as a single string
 * - X - Regex expression
 * - Ls - Series of lines in a single string with line separator
 * 
 * - TitleCase - Externally facing methods (if attributed with SqlFunction)
 * - UPPERCASE - Internal fluent extension methods wrapping the external ones.
 */
namespace MySQLCLRFunctions
{

    /*
     * 
     * Naming conventions for regex parameters
     * - find is for flat string
     * - pattern when a matching process
     * - regexcapturepattern when it has to capture something
     * - match is a noun, what was matched to the pattern
     */
    public static class StringTransform
    {
        // Convenience and clarity function

        public static string[] SingleStringAsArray(string element1)
        {
            var arr = new string[1];
            arr[0] = element1;
            return arr;
        }

        // Extensions for internal use and simpler Fluent design, but not for SQL to call
        internal static string REPLACEMATCHX(this string input, string pattern, string replacement)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(pattern)) return input;
            if (replacement == null) return input;  // Did they mean empty string?

            return ReplaceMatchX(input, pattern, replacement);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Similar to REPLACE in SQL Server, except it has .NET regex for matching
         * 
         * Note parameter naming convention: find is for flat string, pattern when a matching process, regexcapturepattern when it has to capture something
         * TODO: Many examples of how it can be used effectively.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string ReplaceMatchX(string input, string pattern, string replacement)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (replacement == null) return input;  // Did they mean empty string?

            return Regex.Replace(input, pattern, replacement);
        }

        // TODO: Create a case insensitive version
        //public static string ReplaceMatchXi(string input, string pattern, string replacement)

        // Extensions for internal use and simpler Fluent design, but not for SQL to call
        internal unsafe static string REPLACERECURSIVES(this string input, string marker, string replacement)
        {
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(input)) return input;
            if (StringTest.IsNullOrWhiteSpaceOrEmpty(marker)) return input;
            if (replacement == null) return input;  // Did they mean empty string?

            return ReplaceRecursiveS(input, marker, replacement);
        }

        /***************************************************************************************************************************************************************************************************
         * 
         * Replace again and again.  This is for when I want to remove all but one of the spaces, or all the "             " wide open spaces in a SQL proc header except for one space.
         * 
         * This is good as part of a series of "fluent" methods to reduce a proc to an easily read header string.  For quick review of a million modules.
         * This is also my one attempt to try and avoid blowing up memory with immutable strings.
         * 
         **************************************************************************************************************************************************************************************/
        unsafe public static string ReplaceRecursiveS(string input, string marker, string replacement)
        {
            if (StringTest.IsNullOrEmpty(input)) return input;
            // Unreal Request: Cannot replace empty strings. Infinite.
            if (StringTest.IsNullOrEmpty(marker)) return input;
            if (replacement == null) return null;  // Did they mean empty string?

            if (marker == replacement) return input;
            if (replacement.Contains(marker))
            {
                throw new ArgumentOutOfRangeException(nameof(replacement), "the replacement string contains the sought string, so recursion would cause a blowup.");
            }

            if (marker.Length != replacement.Length)
            {
                StringBuilder sb = new StringBuilder(input);
                while (true)
                {
                    int formersize = sb.Length;
                    sb.Replace(marker, replacement);
                    int newsize = sb.Length;
                    if (formersize == newsize)
                    {
                        return sb.ToString();
                    }
                }
            }
            else if (marker.Length == replacement.Length)
            {
                int findlen = marker.Length;
                var findchars = marker.ToCharArray();
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
        /*
            ST('GroupUniqueId               '  AS NVARCHAR(10
            ST('GroupHandle                 '  AS NVARCHAR(10
            ST('GroupName                   '  AS NVARCHAR(10
            ST('GroupDisplayName            '  AS NVARCHAR(10
            ST('GroupDisplayNameSpaceless   '  AS NVARCHAR(10
            ST('GroupOwnerId                '  AS NVARCHAR(10
            ST('GroupOwnerHandle            '  AS NVARCHAR(10
            ST('GroupOwner                  '  AS NVARCHAR(10
            ST('GroupMemberCount            '  AS NVARCHAR(10
            ST('GroupMembers                '  AS NVARCHAR(10
            ST('GroupEmail                  '  AS NVARCHAR(10
            ST('OrganizationalUnitChain     '  AS NVARCHAR(10
            ST('GroupNotes                  '  AS NVARCHAR(10
            ST('GroupHistory                '  AS NVARCHAR(10
            ST('GroupCreatedOn              '  AS NVARCHAR(10
            ST('Security1Distribution0      '  AS NVARCHAR(10
            ST('Universal2Global1Local0     '  AS NVARCHAR(10
            ST('ExportedToFileOn'            AS NVARCHAR(1024
   
            In the above slice, take all spaces before the string "'  AS" and put them after the "'"
            This is using the Alt+Down+Drag operation in SSMS to capture a slice.
            Lines are separated by NL or CR or LF.
         */
        public static string SwitchAllCInLsAroundSToAfterC(string lines, string switchC, string aroundS, string toAfterC)
        {
            return lines;
        }

        /*
            UniqueId'  AS NVARCHAR(1024))
            Handle'  AS NVARCHAR(1024))
            Name'  AS NVARCHAR(1024))
            DisplayName'  AS NVARCHAR(1024))
            DisplayNameSpaceless'  AS NVARCHAR(1024))
            OwnerId'  AS NVARCHAR(1024))
            OwnerHandle'  AS NVARCHAR(1024))
            Owner'  AS NVARCHAR(1024))
            MemberCount'  AS NVARCHAR(1024))
            Members'  AS NVARCHAR(1024))
            Email'  AS NVARCHAR(1024))
            izationalUnitChain'  AS NVARCHAR(1024))
            Notes'  AS NVARCHAR(1024))
            History'  AS NVARCHAR(1024))
            CreatedOn'  AS NVARCHAR(1024))
            ity1Distribution0'  AS NVARCHAR(1024))
            rsal2Global1Local0'  AS NVARCHAR(1024))
            tedToFileOn'            AS NVARCHAR(1024))

        Alignment to the "AS"
        becomes:

            UniqueId'              AS NVARCHAR(1024))
            Handle'                AS NVARCHAR(1024))
            Name'                  AS NVARCHAR(1024))
            DisplayName'           AS NVARCHAR(1024))
            DisplayNameSpaceless'  AS NVARCHAR(1024))
            OwnerId'               AS NVARCHAR(1024))
            OwnerHandle'           AS NVARCHAR(1024))
            Owner'                 AS NVARCHAR(1024))
            MemberCount'           AS NVARCHAR(1024))
            Members'               AS NVARCHAR(1024))
            Email'                 AS NVARCHAR(1024))
            izationalUnitChain'    AS NVARCHAR(1024))
            Notes'                 AS NVARCHAR(1024))
            History'               AS NVARCHAR(1024))
            CreatedOn'             AS NVARCHAR(1024))
            ity1Distribution0'     AS NVARCHAR(1024))
            rsal2Global1Local0'    AS NVARCHAR(1024))
            tedToFileOn'           AS NVARCHAR(1024))

         */
        public static string AlignLOnS(string lines, string alignToLongestS)
        {
            return lines;
        }
    }
}
