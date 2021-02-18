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

        public GameModel(long id, string name, bool isRunning)
        {
            Id = id;
            Name = name;
            IsRunning = isRunning;
        }

        public GameModel(GameModel reference)
        {
            Id = reference.Id;
            Name = reference.Name;
            IsRunning = reference.IsRunning;
        }
    }
}
