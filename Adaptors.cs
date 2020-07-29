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
using System.Globalization;

namespace MySQLCLRFunctions
{
    public static class Adaptors
    {
        // Converts a hex string to a VARBINARY string, I think.

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string VarBin2Hex(SqlBytes InputAsHex)
        {
            if (InputAsHex == null) return null;

            return BitConverter.ToString(InputAsHex.Buffer);
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static DateTime? ADDateTimeString2DateTime(string InputAsStringDateTime)
        {
            if (InputAsStringDateTime == null) return null;
            if (string.IsNullOrWhiteSpace(InputAsStringDateTime)) return null;
            // 20021111182004.0Z
            // 20021031003422
            try
            {
                return DateTime.ParseExact(InputAsStringDateTime.Substring(0, 14), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
