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
    public static class Environmental
    {
        [SqlFunction(DataAccess = DataAccessKind.None)]
        public static int BeepStandard() // Doesn't work from SQL Server :(
        {
            // 247 is a B
            Console.Beep(247, 500);
            return 0;
        }

        [SqlFunction(DataAccess = DataAccessKind.None)]
        public static int Beep(int frequencyHz, int durationMs)
        {
            // 247 is a B
            Console.Beep(frequencyHz, durationMs);
            return 0;
        }
    }
}
