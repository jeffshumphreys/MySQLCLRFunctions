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
    public static class NetworkCollect
    {
        [SqlFunction()]
        public static SqlString PingGetAddress(SqlString Machine)
        {
            string host = Machine.ToString();

            if (Machine.ToString().Contains("\\"))
            {
                host = Machine.ToString().Substring(0, Machine.ToString().IndexOf("\\"));
                // Named instance
            }

            PingReply pr = PingHostGetReply(host);

            if (pr.Status == IPStatus.Success)
                return new SqlString(pr.Address.ToString());
            else
                return SqlString.Null;
        }

        [SqlFunction()]
        public static SqlBytes PingGetReturnBuffer(SqlString Machine)
        {
            string host = Machine.ToString();

            if (Machine.ToString().Contains("\\"))
            {
                host = Machine.ToString().Substring(0, Machine.ToString().IndexOf("\\"));
                // Named instance
            }

            PingReply pr = PingHostGetReply(host);

            if (pr.Status == IPStatus.Success)
                return new SqlBytes(pr.Buffer);
            else
                return SqlBytes.Null;
        }

        [SqlFunction()]
        public static SqlString GetHostNames(SqlString MachineOrAlias)
        {
            string host = MachineOrAlias.ToString();

            if (MachineOrAlias.ToString().Contains("\\"))
            {
                host = MachineOrAlias.ToString().Substring(0, MachineOrAlias.ToString().IndexOf("\\"));
                // Named instance
            }

            IPHostEntry truehost = Dns.GetHostEntry(host);

            string hosts = string.Join<IPAddress>(";", truehost.AddressList);
            return new SqlString(hosts);
        }

        [SqlFunction()]
        public static SqlString GetHostAliases(SqlString MachineOrAlias)
        {
            string host = MachineOrAlias.ToString();

            if (MachineOrAlias.ToString().Contains("\\"))
            {
                host = MachineOrAlias.ToString().Substring(0, MachineOrAlias.ToString().IndexOf("\\"));
                // Named instance
            }

#pragma warning disable CS0618 // Type or member is obsolete
            IPHostEntry truehost = Dns.Resolve(host);
#pragma warning restore CS0618 // Type or member is obsolete

            string hosts = string.Join<string>(";", truehost.Aliases);
            return new SqlString(hosts);
        }

        [SqlFunction()]
        public static SqlString GetHostRealName(SqlString MachineOrAlias)
        {
            string host = MachineOrAlias.ToString();

            if (MachineOrAlias.ToString().Contains("\\"))
            {
                host = MachineOrAlias.ToString().Substring(0, MachineOrAlias.ToString().IndexOf("\\"));
                //return new SqlString(host);
                // Named instance
            }
            try
            {
#pragma warning disable CS0618 // Type or member is obsolete
                IPHostEntry truehost = Dns.Resolve(host);
#pragma warning restore CS0618 // Type or member is obsolete

                host = truehost.HostName;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
                return SqlString.Null;
            }
            return new SqlString(host);
        }
        private static PingReply PingHostGetReply(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;
            PingReply reply = null;

            try
            {
                pinger = new Ping();
                reply = pinger.Send(nameOrAddress);
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

            return reply;
        }
    }
}
