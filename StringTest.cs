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
using System.CodeDom;


namespace MySQLCLRFunctions
{
    /// <summary>
    /// Functions that take input, are non-mutating, do not involve floating point or date time data, and return either true or false.
    /// </summary>
    public static class StringTest
    {
        /// <summary>
        /// 
        /// Returns true if the input string starts with the sought string.
        /// 
        /// </summary>
        /// <param name="input">String to search in</param>
        /// <param name="searchFor">String to search for</param>
        /// <returns></returns>
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool StartsWith(string input, string searchFor)
        {
            return input.StartsWith(searchFor);
        }

        /// <summary>
        /// 
        /// Returns true if the input string ends with the sought string.
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="searchFor"></param>
        /// <returns></returns>
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool EndsWith(string input, string searchFor)
        {
            return input.EndsWith(searchFor);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool LegalName(string input, string characterrule)
        {
            if (characterrule == "SQL Server Server Name")
            {
                // Instance portion Cannot be Default or MSSQLServer
                // Instance be up to 16 characters, Unicode Standard 2.0, decimal numbers basic latin or other national scripts, $, #, _
                // First character must be letter, &, _, #
                // No embedded spaces, special characters, backslash, comma, colon, or at sign.

                // Windows Server 2008 R2 NetBIOS limited to 15 char
            }

            return true;
        }
        /// <summary>
        /// AnyOfTheseAreAnyOfThose
        /// 
        /// Any of these delimited substrings are in the delimited list of sought substrings.
        /// 
        /// </summary>
        /// <param name="inputstrings"></param>
        /// <param name="stringsitmightcontain"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool AnyOfTheseAreAnyOfThose(string inputstrings, string stringsitmightcontain, string sep)
        {
            var inputsasarray = inputstrings.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string i in stringsitmightcontain.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (inputsasarray.Contains(i))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
