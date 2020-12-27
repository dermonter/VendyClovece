using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VendyClovece.UI;
using System.Linq;

namespace VendyClovece.Backend
{
    public class Board
    {
        private Func<Tile, Pawn> HasPawn;

        readonly Color[] playerColors;
        public Tile[] Tiles { get; private set; }
        public Tile[] EndTiles { get; private set; }
        public Tile[] StartTiles { get; private set; }

        public Board(Func<Tile, Pawn> hasPawn)
        {
            HasPawn = hasPawn;

            playerColors = new Color[] { Color.Aquamarine, Color.DarkRed, Color.Yellow,Color.Lime };
            Tiles = new Tile[40];
            StartTiles = new Tile[16];
            EndTiles = new Tile[16];

            for (int i = 0; i < StartTiles.Length; i++)
            {
                StartTiles[i] = new Tile(playerColors[i / 4], TileType.START);
                EndTiles[i] = new Tile(playerColors[i / 4], TileType.END);
            }

            int playerIndex = 0;
            for (int i = 0; i < Tiles.Length; i++)
            {
                if (i % 10 == 0)
                {
                    Tiles[i] = new Tile(playerColors[playerIndex++], TileType.STARTBOARD);
                    continue;
                }
                Tiles[i] = new Tile(Color.White, TileType.NORMAL);
            }

            SetTilePositions();
        }

        public IEnumerable<Clickable> InitPlayers(List<Pawn[]> players, Texture2D pawnHitbox)
        {
            var result = new List<Clickable>();

            int startTile = 0;
            for (int j = 0; j < players.Count; j++)
            {
                for (int i = 0; i < players[j].Length; i++)
                {
                    var pawn = new Pawn(StartTiles[startTile++], playerColors[j], pawnHitbox, j);
                    players[j][i] = pawn;
                    result.Add(pawn);
                }
            }

            return result;
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

        private int TileIndex(Tile[] tiles, Tile target) => Array.FindIndex(tiles, t => t.Id == target.Id);

        private void ReturnPawnToStart(Pawn target)
        {
            for (int i = target.PlayerId * 4; i < target.PlayerId * 4 + 4; i++)
            {
                if (HasPawn(StartTiles[i]) == null)
                {
                    target.CurrentTile = StartTiles[i];
                    return;
                }
            }
        }

        private bool TileFull(Tile tile, Pawn origin)
        {
            Pawn target = null;
            if ((target = HasPawn(tile)) == null)
                return false;

            if (target.PlayerId == origin.PlayerId)
                return true;

            ReturnPawnToStart(target);

            return false;
        }

        public bool Move(Pawn pawn, int moveFor)
        {
            Tile currTile = pawn.CurrentTile;
            Tile targetTile;

            if (TileIndex(StartTiles, currTile) != -1)
            {
                if (moveFor >= 6)
                {
                    int newTileId = pawn.PlayerId * 10 + (moveFor - 6);
                    targetTile = Tiles[newTileId % Tiles.Length];
                    if (TileFull(targetTile, pawn))
                        return false;
                    pawn.CurrentTile = targetTile;
                    return true;
                }
                return false;
            }

            int index;
            if ((index = TileIndex(EndTiles, currTile)) != -1)
            {
                if ((index % 4) + moveFor >= 4)
                    return false;

                targetTile = EndTiles[index + moveFor];
                if (TileFull(targetTile, pawn))
                    return false;
                pawn.CurrentTile = targetTile;
                return true;
            }

            if ((index = TileIndex(Tiles, currTile)) != -1)
            {
                int newTile = (index + moveFor) % Tiles.Length;

                if (index / 10 != newTile / 10)
                {
                    if (newTile / 10 == pawn.PlayerId)
                    {
                        // move to end
                        int endIndex = newTile % 10;
                        if (endIndex >= 4)
                            return false;
                        targetTile = EndTiles[pawn.PlayerId * 4 + endIndex];
                        if (TileFull(targetTile, pawn))
                            return false;
                        pawn.CurrentTile = targetTile;
                        return true;
                    }
                    else
                    {
                        // skip a tile
                        newTile++;
                        if (newTile % 10 == 0)
                            newTile++;
                    }
                }
                targetTile = Tiles[newTile];
                if (TileFull(targetTile, pawn))
                    return false;
                pawn.CurrentTile = targetTile;
                return true;
            }

            return false;
        }
    }
}
