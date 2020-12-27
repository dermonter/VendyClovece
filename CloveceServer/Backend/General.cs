using CloveceServer.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloveceServer.Backend
{
    public class General
    {
        private static void InitServerData()
        {
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                Globals.clients.Add(i, new Client.Client());
            }
        }

        public static void StartServer()
        {
            InitServerData();
            ServerTCP.InitNetwork();
            Logger.Log(LogType.info2, "Server started");
        }
    }
}
