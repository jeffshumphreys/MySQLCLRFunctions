using Microsoft.SqlServer.Server;
using System;

namespace MySQLCLRFunctions
{
    /*************************************************************************
     * 
     * These don't work, but they can be changed to call actual sound functions.
     * 
     *************************************************************************/
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

        // TODO: UpSince a date, humanize
        // TODO: Crashed?
        // TODO: Did SQL Server properly restart?
        // TODO: What account is SQL Server running as?
        // TODO: Mount points and their size

    }
}
