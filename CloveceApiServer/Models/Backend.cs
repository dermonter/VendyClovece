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
                if (!reference.IsRunning)
                    return null;
            }
            return reference;
        }

        public static int Roll(long gameId, int playerId)
        {
            int rolled = -1;
            if (!games.ContainsKey(gameId))
                return rolled;
            lock (games[gameId])
            {
                rolled = games[gameId].Roll(playerId);
            }
            return rolled;
        }

        public static (GameModel gameState, bool result) Move(long gameId, int playerId, int pieceId)
        {
            GameModel reference = null;
            bool result = false;
            if (!games.ContainsKey(gameId))
                return (reference, result);

            lock (games[gameId])
            {
                result = games[gameId].Move(playerId, pieceId);
                reference = new GameModel(games[gameId]);
            }
            return (reference, result);
        }
    }
}
