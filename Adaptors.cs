﻿using System;
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
    public static class Adaptors
    {
        /// <summary>
        /// 
        /// Converts a hex string to a VARBINARY string, I think.
        /// 
        /// </summary>
        /// <param name="InputAsHex"></param>
        /// <returns></returns>
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string VarBin2Hex(SqlBytes InputAsHex)
        {
            return BitConverter.ToString(InputAsHex.Buffer);
        }
    }
}
