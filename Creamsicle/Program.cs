using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Creamsicle
{
    class Program
    {
        class Option
        {
            internal string Name { get; }
            internal HashSet<string> Aliases { get; }
            internal bool IsRequired { get; }
            internal string HelpMsg { get; }

            internal dynamic Value { get; private set; }
            internal Type Type { get; }


            internal Option(string Name, HashSet<string> Aliases, bool IsRequired, string HelpMsg, dynamic Value, Type Type) : this(Name, Aliases, IsRequired, HelpMsg, Type)
            {
                this.Value = Value;
            }

            internal Option(string Name, HashSet<string> Aliases, bool IsRequired, string HelpMsg, Type Type)
            {
                this.Name = Name;
                this.Aliases = Aliases;
                this.IsRequired = IsRequired;
                this.HelpMsg = HelpMsg;
                this.Type = Type;
            }

            internal void ValidateTypeSetValue(string arg)
            {
                if (Type == typeof(Boolean))
                {
                    Value = true;
                }
                else if (!String.IsNullOrWhiteSpace(arg))
                {
                    if (Type == typeof(Int32))
                    {
                        Value = Int32.Parse(arg);
                    }
                    else if (Type == typeof(String))
                    {
                        Value = arg;
                    }
                    else
                    {
                        throw new ArgumentException($"Not valid type", arg);
                    }
                }
                else
                {
                    throw new ArgumentException($"Not valid arg (Null/WhiteSpace)", arg);
                }
            }

            internal static void ParseArgs(HashSet<Option> options, string[] args)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    bool found = false;
                    foreach (Option option in options)
                    {
                        foreach (string alias in option.Aliases)
                        {
                            if (args[i].Equals(alias, StringComparison.OrdinalIgnoreCase))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            string value = (++i < args.Length) ? args[i] : null;
                            option.ValidateTypeSetValue(value);
                            break;
                        }
                    }
                }

                foreach (Option option in options)
                {
                    if (option.IsRequired && option.Value == null)
                    {
                        throw new ArgumentException("Required arg is null", option.Name);
                    }
                }
            }
        }

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

        private static class MainOptions
        {
            internal static readonly Option AsClient = new Option("AsClient", new HashSet<string> { "-c", "--c", "Client" }, true, "Run as Client", false, typeof(Boolean));
            internal static readonly Option AsServer = new Option("AsServer", new HashSet<string> { "-s", "--s", "Server" }, true, "Run as Server", false, typeof(Boolean));
            internal static readonly HashSet<Option> Options = new HashSet<Option> { AsClient, AsServer };
        }
        static void Main(string[] args)
        {
            Option.ParseArgs(MainOptions.Options, args);
            if (MainOptions.AsClient.Value)
            {
                Client.MainClient(args);
            }
            else if (MainOptions.AsServer.Value)
            {
                Server.MainServer(args);
            }
            else
            {
                Console.WriteLine("Please select -c or -s");
            }
        }
    }
}
