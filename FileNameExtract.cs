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
using System.Runtime.Remoting.Messaging;

namespace MySQLCLRFunctions
{
    public static class FileNameExtract
    {
        /*******************************************************************************************************
         * 
         *        File Name Functions
         * 
         *******************************************************************************************************/

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FileNameExtension(string fullfilepath)
        {
            return new FileInfo(fullfilepath).Extension;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FileNameWithoutExtension(string fullfilepath)
        {
            var s = new FileInfo(fullfilepath).Name;
            var ie = new FileInfo(fullfilepath).Extension;
            var l = s.Length;
            var el = ie.Length;
            return s.Substring(0, l - (el));
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static bool IsLegalFileName(string filenamewithoutext)
        {
            // http://stackoverflow.com/questions/1976007/ddg#31976060
            var invalidchars = new char[] {'/', '\\', '"', ':', '>', '<', '|', '?', '*'};

            if (filenamewithoutext.Any(c => char.IsControl(c) || invalidchars.Contains(c)))
            {
                return false;
            }

            var invalidnames = new string[] {"CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };

            if (invalidnames.Contains(filenamewithoutext)) return false;

            if (new char[] { '.', ' ' }.Contains(filenamewithoutext.Last())) return false;

            return true;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FileNameWithExtension(string fullfilepath)
        {
            return new FileInfo(fullfilepath).Name;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FileInDirectory(string fullfilepath)
        {
            return new FileInfo(fullfilepath).DirectoryName;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string FileInFolder(string fullfilepath)
        {
            return new FileInfo(fullfilepath).Directory.Parent.Name;
        }
    }
}
