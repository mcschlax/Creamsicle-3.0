using System;
using System.Collections.Generic;

namespace Creamsicle
{
    class Program
    {
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
