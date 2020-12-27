using System;
using System.Collections.Generic;
using System.Text;

namespace CloveceServer.Client
{
    public class Player
    {
        public int Id { get; private set; }

        public Player(int id)
        {
            Id = id;
        }
    }
}
