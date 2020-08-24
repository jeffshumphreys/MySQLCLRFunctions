using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace MySQLCLRFunctions
{
    public static class StackExtract
    {
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string Caller()
        {
            return string.Empty;
        }
    }
}
