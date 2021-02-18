using SimpleServer.Backend;
using SimpleServer.Server;
using System;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameMaster = new GameMaster();
            // should be done by client
            gameMaster.InitGame(1);

            ServerTcp server = new ServerTcp();
            server.Run();
        }
    }
}
