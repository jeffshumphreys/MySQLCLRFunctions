using System;
using System.Collections.Generic;
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
    }
}
