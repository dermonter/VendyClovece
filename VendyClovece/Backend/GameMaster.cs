using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Backend
{
    public class GameMaster
    {
        GameState gameState;
        int currentPlayer;

        public GameMaster()
        {
            gameState = GameState.YOUR_TURN_NOT_ROLLED;
            currentPlayer = 0;
        }
        public void Roll(int playerId)
        {

        }

        public void SelectPawn(Pawn pawn)
        {

        }
    }
}
