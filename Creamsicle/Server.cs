using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Creamsicle
{
    class Server
    {
        private static class ServerOptions
        {
            internal static readonly Option Port = new Option("Port", new HashSet<string> { "-p", "--p", "Port" }, true, "Port number", typeof(Int32));
            internal static readonly HashSet<Option> Options = new HashSet<Option> { Port };
        }
        internal static void MainServer(string[] args)
        {
            Option.ParseArgs(ServerOptions.Options, args);
            MainServer(ServerOptions.Port.Value);
        }

        private static void MainServer(int port)
        {
            Console.WriteLine($"Server Binding to {port}");
            try
            {
                using (UdpClient server = new UdpClient(port))
                {
                    IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, port);
                    string msg;
                    do
                    {
                        Console.WriteLine($"Server Listening on {port}");
                        msg = Encoding.Unicode.GetString(server.Receive(ref serverEP));
                        Console.WriteLine($"Server Received {msg}");
                    } while (!msg.Equals("stop", StringComparison.OrdinalIgnoreCase));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
}
