using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloveceApiServer.Models
{
    public class GameModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public int LastRolled { get; set; }
        public int CurrentPlayer { get; set; }

        private Random generator;

        public GameModel(long id, string name, bool isRunning)
        {
            Id = id;
            Name = name;
            IsRunning = isRunning;
            generator = new Random();
            CurrentPlayer = 0;
        }

        public GameModel(GameModel reference)
        {
            Id = reference.Id;
            Name = reference.Name;
            IsRunning = reference.IsRunning;
            LastRolled = reference.LastRolled;
            CurrentPlayer = reference.CurrentPlayer;
        }

        public int Roll(int playerId)
        {
            if (playerId != CurrentPlayer)
                return -1;

            LastRolled = generator.Next(1, 7);
            return LastRolled;
        }
    }
}
