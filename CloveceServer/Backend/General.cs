using CloveceServer.Server;

namespace CloveceServer.Backend
{
    public class General
    {
        private static void InitServerData()
        {
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                Globals.clients.Add(new Client.Client());
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
