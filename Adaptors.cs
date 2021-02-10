using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
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
                return DateTime.ParseExact(InputAsStringDateTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        private static string[][] formats = new string[][]
        {
                new string [] {"yyyyMMddHHmmssfffffff", "21" }
              , new string [] { "yyyyMMddHHmmssfff", "17" }
              , new string [] {"yyyyMMddHHmmss", "14" }
              , new string [] {"yyyyMMdd", "8" }
              , new string [] { "yyMMdd", "6" }
              , new string [] { "MMddyyyy", "8" }
              , new string [] { "MMddyy", "6" }
              , new string [] {"MM/dd/yy", "8" }
              , new string [] {"MM/dd/yyyy", "10" }
              , new string [] {"ddd dd MMM yyyy h:mm tt zzz", "0" }
              , new string [] {"MMddyyyyHHmmss", "14" }
              , new string [] {"dd/MM/yyyy HH:mm:ss.ffffff", "0"}
              , new string [] {"d", "0" }
        };

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static DateTime? ToDate(string InputAsStringDateTime)
        {

            if (InputAsStringDateTime == null) return null;
            if (string.IsNullOrWhiteSpace(InputAsStringDateTime)) return null;
            // 20021111182004.0Z
            // 20021031003422
            foreach (string[] _format in formats)
            {

                try
                {
                    if (_format[1] == "0")
                        return DateTime.ParseExact(InputAsStringDateTime, _format[0], CultureInfo.InvariantCulture);
                    else
                    {
                        return DateTime.ParseExact(InputAsStringDateTime, _format[0], CultureInfo.InvariantCulture);
                    }
                }
                catch (FormatException)
                {
                }
            }

            return null;
        }
    }
}
