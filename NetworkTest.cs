using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Net.NetworkInformation;

namespace MySQLCLRFunctions
{
    public static class NetworkTest
    {
        [SqlFunction()]
        public static SqlBoolean Ping(SqlString Machine)
        {
            string host = Machine.ToString();

            if (Machine.ToString().Contains("\\"))
            {
                host = Machine.ToString().Substring(0, Machine.ToString().IndexOf("\\") - 1);
                // Named instance
            }

            if (PingHost(host))
                return SqlBoolean.True;
            else
                return SqlBoolean.False;
        }

        private static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }
    }
}
