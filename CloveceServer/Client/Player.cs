using System;
using System.Collections.Generic;
using System.Text;
using CloveceServer.Backend;

namespace CloveceServer.Client
{
    public class Player
    {
        private Random generator;

        public int Id { get; private set; }
        public int Rolled { get; private set; }
        public GameState GameState { get; private set; }
        private (int x, int y)[] pawns;

        public bool GameStateChanged { get; set; }

        public int Roll()
        {
            Rolled += generator.Next(1, 7);

            if (Rolled != 6)
            {
                GameState = GameState.YOUR_TURN_ROLLED;
            }

            return Rolled;
        }

        public Player(int id, GameState gameState)
        {
            Id = id;
            generator = new Random();
            Rolled = 0;
            GameState = gameState;
            GameStateChanged = true;
            pawns = new (int, int)[4];
        }
    }
}
