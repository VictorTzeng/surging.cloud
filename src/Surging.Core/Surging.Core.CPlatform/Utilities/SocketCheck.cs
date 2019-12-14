using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Utilities
{
    public static class SocketCheck
    {
        public static bool TestConnection(string host, int port, int millisecondsTimeout = 5) 
        {
            var client = new TcpClient();
            try
            {
                var ar = client.BeginConnect(host,port, null, null);
                ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                return client.Connected;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                client.Close();
            }
        }

        public static bool TestConnection(IPAddress iPAddress, int port, int millisecondsTimeout = 5)
        {
            var client = new TcpClient();
            try
            {
                var ar = client.BeginConnect(iPAddress, port, null, null);
                ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                return client.Connected;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                client.Close();
            }
        }

    }
}
