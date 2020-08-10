using Microsoft.SqlServer.Server;
using System.IO;

namespace MySQLCLRFunctions
{
    public static class Files
    {
        // Need a way to copy off locked files into temp space so I can open them in SSMS scripts, alter them swapping out macros, and execute them.
        // But if you have a file open in SSMS and it is part of a Project, it is locked for reading.  Files opened outside of a solution/project don't get locked this way.
        // 
        // This also works great for Excel old style (xls) when someone leaves it open on their desktop but you need to load something.
        // 
        // <returns>temporary file is created within the user's temporary folder.</returns>
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string TempFilePath()
        {
            return Path.GetTempFileName();
        }

        // TODO: FileWatcher
        // TODO: FileEventWatcher
    }
}
