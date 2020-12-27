using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VendyClovece.UI;
using System.Linq;
using VendyClovece.Client;

namespace VendyClovece.Backend
{
    public class GameMaster : Singleton<GameMaster>
    {
        public GameState gameState;
        int currentPlayer;
        public int roll;
        readonly private Random generator;

        public Board Board { get; private set; }
        public List<Pawn[]> Players { get; private set; }

        public GameMaster() : base()
        {
            gameState = GameState.YOUR_TURN_NOT_ROLLED;
            currentPlayer = 0;
            generator = new Random();
            Board = new Board(HasPawn);
            Players = new List<Pawn[]> 
            { 
                new Pawn[4], 
                new Pawn[4]
            };
        }

        private int DiceRoll() => generator.Next(1, 7);

        public void Roll()
        {
            if (gameState != GameState.YOUR_TURN_NOT_ROLLED)
                roll = -1;

            ClientSend.Roll();
        }

        public void SelectPawn(Pawn pawn)
        {
            if (gameState != GameState.YOUR_TURN_ROLLED)
                return;

            if (currentPlayer != pawn.PlayerId)
                return;

            // Move the pawn HEHE
            Board.Move(pawn, roll);

            roll = 0;
            currentPlayer++;
            gameState = GameState.OPONENT_TURN;
        }

        public IEnumerable<Clickable> InitPlayers(Texture2D pawnHitbox)
        {
            return Board.InitPlayers(Players, pawnHitbox);
        }

        public void EmulateEnemy()
        {
            if (gameState != GameState.OPONENT_TURN)
                return;

            int rolled = DiceRoll();
            if (rolled == 6)
                rolled += DiceRoll();

            Pawn randPawn = Players[currentPlayer][generator.Next(0, 4)];
            Board.Move(randPawn, rolled);

            currentPlayer = ++currentPlayer % Players.Count;

            if (currentPlayer == 0)
                gameState = GameState.YOUR_TURN_NOT_ROLLED;
        }

        private Pawn HasPawn(Tile tile)
        {
            IEnumerable<Pawn> pawns = Players.SelectMany(p => p);
            return pawns.FirstOrDefault(p => p.CurrentTile.Id == tile.Id);
        }
    }

    public enum GameState
    {
        YOUR_TURN_NOT_ROLLED,
        YOUR_TURN_ROLLED,
        OPONENT_TURN
    }
}
