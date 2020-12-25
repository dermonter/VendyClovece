using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Backend
{
    class Board
    {
        public Color[] Tiles { get; private set; }
        public Color[] Players { get; private set; }
        public Color[] EndTiles { get; private set; }
        public Color[] StartTiles { get; private set; }

        public Board()
        {
            Players = new Color[] { Color.Aquamarine, Color.DarkRed, Color.Yellow, Color.Lime };
            Tiles = new Color[40];
            StartTiles = new Color[16];
            EndTiles = new Color[16];

            for (int i = 0; i < StartTiles.Length; i++)
            {
                StartTiles[i] = Players[i / 4];
                EndTiles[i] = Players[i / 4];
            }

            int playerIndex = 0;
            for (int i = 0; i < Tiles.Length; i++)
            {
                if (i % 10 == 0)
                {
                    Tiles[i] = Players[playerIndex++];
                    continue;
                }
                Tiles[i] = Color.White;
            }
        }
    }
}
