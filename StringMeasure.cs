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
    public static class StringMeasure
    {
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static int HowMany(string input, string howManyOfThese)
        {
            // Warning: May not count "%%%%" where search string is "%%".  Is it 2 or 1.
            int howMany = (input.Length - input.Replace(howManyOfThese, "").Length) / howManyOfThese.Length;
            return howMany;
        }
    }
}
