using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VendyClovece.UI;

namespace VendyClovece.Backend
{
    public class GameMaster
    {
        GameState gameState;
        int currentPlayer;
        int roll;

        public Board Board { get; private set; }
        public List<Pawn[]> Players { get; private set; }

        Random generator;

        public GameMaster()
        {
            gameState = GameState.YOUR_TURN_NOT_ROLLED;
            currentPlayer = 0;
            generator = new Random();
            Board = new Board();
            Players = new List<Pawn[]> 
            { 
                new Pawn[4], 
                new Pawn[4],
                new Pawn[4]
            };
        }

        private int DiceRoll() => generator.Next(1, 7);

        public int Roll()
        {
            if (gameState != GameState.YOUR_TURN_NOT_ROLLED)
                return -1;

            roll = DiceRoll();

            gameState = GameState.YOUR_TURN_ROLLED;

            return roll;
        }

        public void SelectPawn(Pawn pawn)
        {
            if (gameState != GameState.YOUR_TURN_ROLLED)
                return;

            // Move the pawn HEHE
            Board.Move(pawn, roll);

            gameState = GameState.OPONENT_TURN;
            currentPlayer++;
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
            Pawn randPawn = Players[currentPlayer][generator.Next(0, 4)];
            Board.Move(randPawn, rolled);

            currentPlayer = ++currentPlayer % Players.Count;

            if (currentPlayer == 0)
                gameState = GameState.YOUR_TURN_NOT_ROLLED;
        }
    }
}
