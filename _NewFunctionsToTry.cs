using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MySQLCLRFunctions
{
    public static class _NewFunctionsToTry
    {
        public static int fn_clr_LongRunningAdder(int x, int y)
        {
            var t = Task.Factory.StartNew(() => LongRunning(x, y));
            return t.Result;
        }

        private static int LongRunning(int x, int y)
        {
            var wait = (x + y) * 100;
            Thread.Sleep(wait);
            return x + y;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static SqlString TestSqlChar(SqlChars streamingString)
        {
            //char c = streamingString.Read(1,)
            return streamingString.ToSqlString();
        }

        /*
         * Function to SIMPLIFY adding missing dlls.
         * Warning: The Microsoft .NET Framework assembly 'microsoft.visualstudio.qualitytools.unittestframework, version=10.0.0.0, culture=neutral, publickeytoken=b03f5f7f11d50a3a, processorarchitecture=msil.' you are registering is not fully tested in the SQL Server hosted environment and is not supported. In the future, if you upgrade or service this assembly or the .NET Framework, your CLR integration routine may stop working. Please refer SQL Server Books Online for more details.
         * Error: CANNOT Assembly 'MySQLCLRFunctions' references assembly 'system.web, version=2.0.0.0, culture=neutral, publickeytoken=b03f5f7f11d50a3a.', which is not present in the current database. SQL Server attempted to locate and automatically load the referenced assembly from the same location where referring assembly came from, but that operation has failed (reason: 2(The system cannot find the file specified.)). Please load the referenced assembly into the current database and retry your request.
         * 
         * Found a site with a guy who showed how to do it in 1,392 steps!
         */
    }
}
