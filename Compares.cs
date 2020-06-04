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
    public static class Compares
    {
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = false, IsPrecise = true)]
        public static DateTime GreatestOf2DateTimes(DateTime d1, DateTime d2)
        {
            if (d1 == null) return d2;
            if (d2 == null) return d1;
            if (d1.CompareTo(d2) >= 0) return d1;
            return d2;
        }
    }
}
