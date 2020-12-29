using CloveceServer.Backend;
using System;

namespace CloveceServer.Client
{
    public class Player
    {
        private Random generator;

        public int Id { get; private set; }
        public int Rolled { get; private set; }
        public GameState GameState { get; private set; }
        public (TileType tile, int index)[] Pawns { get; private set; }

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

        public void Moved()
        {
            Rolled = 0;
            GameState = GameState.OPONENT_TURN;
            GameStateChanged = true;
        }

        public void YourTurn()
        {
            GameState = GameState.YOUR_TURN_NOT_ROLLED;
            GameStateChanged = true;
        }

        public Player(int id, GameState gameState, (TileType, int)[] pawns)
        {
            Id = id;
            generator = new Random();
            Rolled = 0;
            GameState = gameState;
            GameStateChanged = true;
            Pawns = pawns;
        }
    }
}
