using SimpleServer.Server;
using System;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerTcp server = new ServerTcp();
            server.Run();
        }
    }
}
