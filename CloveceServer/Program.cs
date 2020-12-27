using CloveceServer.Backend;
using System;
using System.Threading;

namespace CloveceServer
{
    class Program
    {
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

            DateTime _lastLoop = DateTime.Now;
            DateTime _nextLoop = _lastLoop.AddMilliseconds(Constants.MS_PER_TICK);

            while(Globals.serverIsRunning)
            {
                while(_nextLoop < DateTime.Now)
                {
                    Logger.WriteLogs();
                    // Update Loop

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

        }

        private static void Update()
        {

        }
    }
}
