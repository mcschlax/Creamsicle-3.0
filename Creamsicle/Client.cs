using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Creamsicle
{
    class Client
    {
        private static class ClientOptions
        {
            internal static readonly Option Host = new Option("Host", new HashSet<string> { "-h", "--h", "Host" }, false, "Host name by IP address, defaults to 127.0.0.1", "127.0.0.1", typeof(String));
            internal static readonly Option Port = new Option("Port", new HashSet<string> { "-p", "--p", "Port" }, true, "Port number", typeof(Int32));
            internal static readonly Option Message = new Option("Message", new HashSet<string> { "-m", "--m", "Message" }, true, "Message", typeof(String));
            internal static readonly HashSet<Option> Options = new HashSet<Option> { Host, Port, Message };
        }
        internal static void MainClient(string[] args)
        {
            Option.ParseArgs(ClientOptions.Options, args);
            MainClient(ClientOptions.Host.Value, ClientOptions.Port.Value, ClientOptions.Message.Value);
        }

        private static void MainClient(string hostname, int port, string message)
        {
            Console.WriteLine($"Client Connecting to {hostname} on {port}");
            try
            {
                using (UdpClient client = new UdpClient(hostname, port))
                {
                    Console.WriteLine($"Client Sending {message}");
                    byte[] payload = Encoding.Unicode.GetBytes(message);
                    client.Send(payload, payload.Length);
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
