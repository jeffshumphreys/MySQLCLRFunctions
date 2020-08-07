using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions
{
    public static class AssemblyTools
    {
        [SqlProcedure]
        // Stolen from the desk of https://github.com/enriquecatala/SQLCLRUtils/blob/master/SQLCLRUtils/clr/AssemblyExporter.cs
        // Curious: How/why would this be used? For deployments?
        // How to use SqlDataReader; parameterized execution, Pipes, SqlDbType? And FileStream. Very cool.

        public static void SaveAssembly(string assemblyName, string destinationPath)
        {
            const string sql = "SELECT af.name, af.content FROM sys.assemblies a INNER JOIN sys.assembly_files af ON a.assembly_id = af.assembly_id WHERE a.name = @assemblyname";
            using SqlConnection conn = new SqlConnection("context connection=true");
            using SqlCommand cmd = new SqlCommand(sql, conn);
#pragma warning disable IDE0017 // Simplify object initialization (No idea what it's referring to)
            var param = new SqlParameter("@assemblyname", SqlDbType.VarChar);
#pragma warning restore IDE0017 // Simplify object initialization
            param.Value = assemblyName;
            cmd.Parameters.Add(param);
            cmd.Connection.Open();  //Open the context connetion
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) //Iterate through assembly files
            {
                string assemblyFileName = reader.GetString(0);  //get assembly file name from the name (first) column
                SqlBytes bytes = reader.GetSqlBytes(1);         //get assembly binary data from the content (second) column
                string outputFile = Path.Combine(destinationPath, assemblyFileName);
                SqlContext.Pipe.Send(string.Format("Exporting assembly file [{0}] to [{1}]", assemblyFileName, outputFile)); //Send information about exported file back to the calling session
                using FileStream byteStream = new FileStream(outputFile, FileMode.CreateNew);
                byteStream.Write(bytes.Value, 0, (int)bytes.Length);
                byteStream.Close();
            }
        }
    }
}
