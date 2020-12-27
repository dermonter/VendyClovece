using CloveceServer.Backend;
using CloveceServer.Server;
using System;
using System.Threading;

namespace CloveceServer
{
    class Program
    {
        private static GameMaster gameMaster;

        static void Main(string[] args)
        {
            Globals.serverIsRunning = true;
            Logger.Initialize(ConsoleColor.Cyan);

            Thread _gameThread = new Thread(new ThreadStart(GameLogicThread));
            _gameThread.Start();
            General.StartServer();
        }

        private static void GameLogicThread()
        {
            Logger.Log(LogType.info1, "Game thread started. Running at " + Constants.TICKS_PER_SEC + " ticks per second");
            // Init Game
            Initialize();

            DateTime _lastLoop = DateTime.Now;
            DateTime _nextLoop = _lastLoop.AddMilliseconds(Constants.MS_PER_TICK);

            while(Globals.serverIsRunning)
            {
                while(_nextLoop < DateTime.Now)
                {
                    Logger.WriteLogs();
                    // Update Loop
                    Update();

                    _lastLoop = _nextLoop;
                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                }

                if (_nextLoop > DateTime.Now)
                {
                    Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }

        private static void Initialize()
        {
            gameMaster = new GameMaster();
        }

        private static void Update()
        {
            for (int i = 0; i < Globals.clients.Count; i++)
            {
                if (!Globals.clients[i].isPlaying)
                    continue;

                if (Globals.clients[i].Player.GameStateChanged)
                {
                    Globals.clients[i].Player.GameStateChanged = false;
                    ServerSend.BoardState(i);
                }
            }
        }
    }
}
