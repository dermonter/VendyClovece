using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Backend
{
    class Board
    {
        public Color[] Players { get; private set; }
        public Tile[] Tiles { get; private set; }
        public Tile[] EndTiles { get; private set; }
        public Tile[] StartTiles { get; private set; }

        public Board()
        {
            Players = new Color[] { Color.Aquamarine, Color.DarkRed, Color.Yellow,Color.Lime };
            Tiles = new Tile[40];
            StartTiles = new Tile[16];
            EndTiles = new Tile[16];

            for (int i = 0; i < StartTiles.Length; i++)
            {
                StartTiles[i] = new Tile(Players[i / 4]);
                EndTiles[i] = new Tile(Players[i / 4]);
            }

            int playerIndex = 0;
            for (int i = 0; i < Tiles.Length; i++)
            {
                if (i % 10 == 0)
                {
                    Tiles[i] = new Tile(Players[playerIndex++]);
                    continue;
                }
                Tiles[i] = new Tile(Color.White);
            }

            SetTilePositions();
        }

        public void SetTilePositions()
        {
            Vector2 origin = new Vector2(0, 0);
            int currTile = 0;
            int endTile = 0;
            int startTile = 0;

            for (int i = 0; i < 4; i++)
            {
                // Draw Game Tiles
                float angle = (float)Math.PI / 2 * i;
                float originX = 1;
                float originY = -5;

                for (int j = 0; j < 5; j++)
                {
                    Vector2 pos = new Vector2(originX, originY + j);
                    pos = Utils.RotateAroundOrigin(pos, origin, angle);
                    Tiles[currTile++].SetPosition(pos);
                }

                for (int j = 0; j < 4; j++)
                {
                    Vector2 pos = new Vector2(originX + j + 1, originY + 4);
                    pos = Utils.RotateAroundOrigin(pos, origin, angle);
                    Tiles[currTile++].SetPosition(pos);
                }

                Vector2 lastPos = Utils.RotateAroundOrigin(new Vector2(originX + 4, originY + 5), origin, angle);
                Tiles[currTile++].SetPosition(lastPos);

                // Draw End Tiles
                for (int j = 0; j < 4; j++)
                {
                    Vector2 endPos = new Vector2(originX - 1, originY + j + 1);
                    endPos = Utils.RotateAroundOrigin(endPos, origin, angle);
                    EndTiles[endTile++].SetPosition(endPos);
                }

                // Draw Start Tiles
                for (int j = 0; j < 4; j++)
                {
                    Vector2 startPos = new Vector2(originX + 3 + j % 2, originY + j / 2);
                    startPos = Utils.RotateAroundOrigin(startPos, origin, angle);
                    StartTiles[startTile++].SetPosition(startPos);
                }
            }
        }
    }
}
