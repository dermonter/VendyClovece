using System;
using System.Collections.Generic;
using System.Text;

namespace CloveceServer.Backend
{
    public class GameMaster
    {
        private static GameMaster instance = null;
        public static GameMaster Instance => instance;

        public Tile[] Tiles { get; private set; }
        public Tile[] StartTiles { get; private set; }
        public Tile[] EndTiles { get; private set; }

        public class Tile
        {
            public int? PlayerId { get; set; }
            public int PawnId { get; set; }

            public Tile()
            {
                PlayerId = null;
                PawnId = -1;
            }

            public Tile(int playerId, int pawnId)
            {
                PlayerId = playerId;
                PawnId = pawnId;
            }
        }

        public GameMaster()
        {
            if (Instance != null)
                throw new Exception("More than one instance of singleton");
            instance = this;

            Tiles = new Tile[40];
            StartTiles = new Tile[16];
            EndTiles = new Tile[16];

            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = new Tile();
            }

            for (int i = 0; i < StartTiles.Length; i++)
            {
                StartTiles[i] = new Tile();
            }

            for (int i = 0; i < EndTiles.Length; i++)
            {
                EndTiles[i] = new Tile();
            }
        }

        public void InitGame(int playerCount)
        {
            for (int i = 0; i < playerCount; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    EndTiles[j + i * 4].PlayerId = i;
                    EndTiles[j + i * 4].PawnId = j;
                }
            }
        }
    }
}
