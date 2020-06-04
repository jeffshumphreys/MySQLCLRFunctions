using System;
using System.Collections;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System.Xml.Schema;

namespace MySQLCLRFunctions
{
    public static class StringExtract
    {
        private const int NOT_FOUND = -1;
        private const int CHARACTER_AFTER_MARKER = 1;
        private const int BACKSET_FOR_ZEROBASED = -1;

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LeftOf(string input, string marker)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var i = input.IndexOf(marker);
            if (i == NOT_FOUND) return null;
            return input.Substring(0, i);
        }
        private static int IndexOfLastChar(this String str)
        {
            if (string.IsNullOrWhiteSpace(str)) return NOT_FOUND;
            return str.Length - 1;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LeftOfNth(string input, string marker, int n)
        {
            if (string.IsNullOrEmpty(input)) return null;
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            if (n == 0) return null;
            var i = input.IndexOf(marker);
            if (i == NOT_FOUND) return null;
            if (n == 1) return input.Substring(0, i);
            int previous_i = NOT_FOUND;
            for (int j = 1; j < n; j++)
            {
                if (i == input.IndexOfLastChar()) return string.Empty;
                previous_i = i;
                i = input.IndexOf(marker, i + marker.Length);
                if (i == NOT_FOUND) return null;
            }
            if (i >= input.IndexOfLastChar()) return string.Empty;
            if (i == NOT_FOUND) return null;
            int seglen = i - (previous_i + marker.Length);
            return input.Substring(previous_i + marker.Length, seglen);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LeftMOfNth(string input, string marker, int n, int howmany)
        {
            int[] pointsfound = new int[n];
            if (string.IsNullOrEmpty(input)) return null;
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            if (n == 0) return null;
            if (howmany > n) return null;
            int i = 0;
            for (int j = 1; j <= n; j++)
            {
                if (i >= input.IndexOfLastChar()) return string.Empty;
                pointsfound[j+BACKSET_FOR_ZEROBASED] = i;
                i = input.IndexOf(marker, i + marker.Length);
                if (i == NOT_FOUND) return null;
                if (j == howmany)
                {
                    int startofseg = pointsfound[j - howmany];
                    int endofseg = i;
                    int seglen = endofseg - startofseg;
                    return input.Substring(startofseg, seglen);
                }
            }
            return string.Empty;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string LeftOfAny(string input, string markers)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var i = input.IndexOfAny(markers.ToCharArray());
            if (i == NOT_FOUND) return null;
            return input.Substring(0, i);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RightOf(string input, string marker)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var i = input.IndexOf(marker);
            if (i == NOT_FOUND) return null;
            return input.Substring(i + marker.Length);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string RightOfAny(string input, string markers)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var i = input.IndexOfAny(markers.ToCharArray());
            if (i == NOT_FOUND) return null;
            return input.Substring(i + 1);
        }

        /// <summary>
        /// 
        /// Client-specific.  But a good sampling of how Active Directory data can be pulled.
        /// 
        /// </summary>
        /// <param name="FullName"></param>
        /// <returns></returns>
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlString GetFirstName(SqlString FullName)
        {
            string workingFullName = FullName.ToString().Trim();
            string firstName;

            if (workingFullName.EndsWith("(CWF)")) workingFullName = workingFullName.Substring(0, workingFullName.Length - 6).Trim();
            if (workingFullName.EndsWith("(SSI)")) workingFullName = workingFullName.Substring(0, workingFullName.Length - 6).Trim();
            if (workingFullName.EndsWith("(RDI Contractor)")) workingFullName = workingFullName.Substring(0, workingFullName.Length - "(RDI Contractor)".Length).Trim();
            if (workingFullName.Contains(","))
            {
                firstName = workingFullName.Substring(workingFullName.IndexOf(",") + 1);
            }
            else if (workingFullName.Contains(" "))
            {
                firstName = workingFullName.Substring(0, workingFullName.IndexOf(" "));
            }
            else
            {
                firstName = workingFullName;
            }

            if (firstName.EndsWith(" Jr")) { firstName = firstName.Substring(0, firstName.Length - 3); }

            if (firstName.Length > 2 && Char.IsLetter(firstName[firstName.Length - 1]) && (firstName[firstName.Length - 2] == ' '))
            {
                firstName = firstName.Substring(0, firstName.Length - 2);
            }
            return new SqlString(firstName.Trim());
        }
    }
}
