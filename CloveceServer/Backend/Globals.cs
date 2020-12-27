using System.Collections.Generic;

namespace CloveceServer.Backend
{
    public class Globals
    {
        public static bool serverIsRunning = false;
        public static List<Client.Client> clients = new List<Client.Client>();
    }

    public enum GameState
    {
        YOUR_TURN_NOT_ROLLED = 0,
        YOUR_TURN_ROLLED = 1,
        OPONENT_TURN = 2
    }
}
