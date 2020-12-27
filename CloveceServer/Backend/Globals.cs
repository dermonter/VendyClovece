using System.Collections.Generic;

namespace CloveceServer.Backend
{
    public class Globals
    {
        public static bool serverIsRunning = false;
        public static Dictionary<int, Client.Client> clients = new Dictionary<int, Client.Client>();
    }
}
