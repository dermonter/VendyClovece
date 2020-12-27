using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VendyClovece.UI;
using System.Linq;

namespace VendyClovece.Backend
{
    public class GameMaster
    {
        GameState gameState;
        int currentPlayer;
        int roll;
        readonly private Random generator;

        public Board Board { get; private set; }
        public List<Pawn[]> Players { get; private set; }

        public GameMaster()
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

        public int Roll()
        {
            if (gameState != GameState.YOUR_TURN_NOT_ROLLED)
                return -1;

            roll += DiceRoll();
            if (roll == 6)
                return roll;

            gameState = GameState.YOUR_TURN_ROLLED;

            return roll;
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
