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
    public static class Humanization
    {
        [SqlFunction(DataAccess = DataAccessKind.None)]
        public static string HumanizeDateTimeDiff(DateTime from)
        {
            if (from == null) return null;
            TimeSpan ts = DateTime.Now.Subtract(from);

            // The trick: make variable contain date and time representing the desired timespan,
            // having +1 in each date component.
            DateTime date = DateTime.MinValue + ts;

            return ProcessPeriod(date.Year - 1, date.Month - 1, "year")
                   ?? ProcessPeriod(date.Month - 1, date.Day - 1, "month")
                   ?? ProcessPeriod(date.Day - 1, date.Hour, "day", "Yesterday")
                   ?? ProcessPeriod(date.Hour, date.Minute, "hour")
                   ?? ProcessPeriod(date.Minute, date.Second, "minute")
                   ?? ProcessPeriod(date.Second, date.Millisecond, "second")
                   ?? ProcessPeriod(date.Millisecond, 0, "millisecond")
                   ?? "Right now";
        }

        private static string ProcessPeriod(int value, int subValue, string name, string singularName = null)
        {
            if (value == 0)
            {
                return null;
            }
            if (value == 1)
            {
                if (!String.IsNullOrEmpty(singularName))
                {
                    return singularName;
                }
                string articleSuffix = name[0] == 'h' ? "n" : String.Empty;
                return subValue == 0
                    ? String.Format("A{0} {1} ago", articleSuffix, name)
                    : String.Format("About a{0} {1} ago", articleSuffix, name);
            }
            return subValue == 0
                ? String.Format("{0} {1}s ago", value, name)
                : String.Format("About {0} {1}s ago", value, name);
        }

    }
}
