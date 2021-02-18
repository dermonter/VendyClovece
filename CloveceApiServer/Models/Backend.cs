using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloveceApiServer.Models
{
    public class Backend
    {
        private const int TIMEOUT_PERIOD = 200;
        private static readonly object currentMaxGameIdLock = new object();
        private static long currentMaxGameId = 0;

        private static Queue<(Action<object> callback, object data)> callStack = new ();
        private static Dictionary<long, GameModel> games = new ();

        public static void Run()
        {
            while (true)
            {
                while (callStack.Count != 0)
                {
                    var call = callStack.Dequeue();
                    call.callback(call.data);
                }

                Thread.Sleep(TIMEOUT_PERIOD);
            }
        }

        public static long CreateGame()
        {
            long gameId;
            lock (currentMaxGameIdLock) {
                GameModel game = new GameModel(currentMaxGameId, "Cool game", true);
                games.Add(currentMaxGameId, game);
                gameId = currentMaxGameId++;
            }
            return gameId;
        }

        public static GameModel GetGame(long id)
        {
            GameModel reference = null;
            if (!games.ContainsKey(id))
                return null;

            lock (games[id])
            {
                reference = new GameModel(games[id]);
            }
            return reference;
        }
    }
}
