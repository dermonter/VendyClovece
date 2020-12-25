using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Backend
{
    class Board
    {
        public Color[] Tiles { get; private set; }

        public Board()
        {
            Tiles = new Color[40];
            for (int i = 0; i < Tiles.Length; i++)
            {
                if (i % 10 == 0)
                {
                    Tiles[i] = Color.Aquamarine;
                    continue;
                }
                Tiles[i] = Color.White;
            }
        }
    }
}
