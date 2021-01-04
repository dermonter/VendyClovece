using SimpleServer.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleServer.Backend
{
    public class GameMaster
    {
        private int currentPlayer;

        private static GameMaster instance = null;
        public static GameMaster Instance => instance;

        private const int MAX_PLAYERS = 4;
        public Dictionary<int, Player> Players { get; private set; }

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

        public IEnumerable<(TileType tile, int tileId)> GetPawns(int playerId)
        {
            var nt = Tiles
                .Select((t, tileIndex) => (tileIndex, TileType.NORMAL, t));
            var st = StartTiles
                .Select((t, tileIndex) => (tileIndex, TileType.START, t));
            var et = EndTiles
                .Select((t, tileIndex) => (tileIndex, TileType.END, t));
            var r = nt
                .Concat(st)
                .Concat(et)
                .Where(triple => triple.t.PlayerId.GetValueOrDefault(-1) == playerId)
                .OrderBy(triple => triple.t.PawnId)
                .Select(triple => (triple.Item2, triple.tileIndex));
            return r;
        }

        public int RegisterPlayer()
        {
            for (int i = 0; i < 4; i++)
            {
                if (Players.ContainsKey(i))
                    continue;

                Player player = new Player(i, i == currentPlayer ? GameState.YOUR_TURN_NOT_ROLLED : GameState.OPONENT_TURN, GetPawns(i).ToArray());
                Players.Add(i, player);
                return i;
            }

            return -1;
        }

        public GameMaster()
        {
            if (Instance != null)
                throw new Exception("More than one instance of singleton");
            instance = this;

            Players = new Dictionary<int, Player>();

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

            currentPlayer = 0;
        }

        public void InitGame(int playerCount)
        {
            for (int i = 0; i < playerCount; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    StartTiles[j + i * 4].PlayerId = i;
                    StartTiles[j + i * 4].PawnId = j;

                }
            }
        }

        public void AdvancePlayer()
        {
            while (!Players.ContainsKey(++currentPlayer % MAX_PLAYERS)) ;
            Players[currentPlayer].YourTurn();
        }

        public bool Move(int playerId, int pawnId)
        {
            Player player = Players[playerId];
            var pawn = player.Pawns[pawnId];
            int rolled = player.Rolled;

            switch (pawn.tile)
            {
                case TileType.START:
                    if (rolled < 6)
                        return false;
                    int newTileId = playerId * 10 + (rolled - 6);
                    int newPos = newTileId % Tiles.Length;

                    // Check if full

                    Tiles[newPos].PlayerId = playerId;
                    Tiles[newPos].PawnId = pawnId;
                    StartTiles[pawn.index].PlayerId = null;
                    StartTiles[pawn.index].PawnId = -1;
                    player.Pawns[pawnId] = (TileType.NORMAL, newPos);
                    break;
                case TileType.END:
                    if ((pawn.index % 4) + rolled >= 4)
                        return false;

                    int targetTileIndex = pawn.index + rolled;
                    // Check if full
                    // if (TileFull(targetTile, pawn))
                    //    return false;
                    EndTiles[targetTileIndex].PlayerId = playerId;
                    EndTiles[targetTileIndex].PawnId = pawnId;
                    EndTiles[pawn.index].PlayerId = null;
                    EndTiles[pawn.index].PawnId = -1;
                    player.Pawns[pawnId] = (TileType.END, targetTileIndex);
                    break;
                case TileType.NORMAL:
                    int newTileIndex = (pawn.index + rolled) % Tiles.Length;
                    if (pawn.index / 10 != newTileIndex / 10)
                    {
                        if (newTileIndex / 10 == playerId)
                        {
                            // move to end
                            int endIndex = newTileIndex % 10;
                            if (endIndex >= 4)
                                return false;

                            // move to the end tile
                            // targetTile = EndTiles[pawn.PlayerId * 4 + endIndex];

                            // check if end tile is full

                            // Move to the tile
                            int endTilePos = playerId * 4 + endIndex;
                            EndTiles[endTilePos].PlayerId = playerId;
                            EndTiles[endTilePos].PawnId = pawnId;
                            Tiles[pawn.index].PlayerId = null;
                            Tiles[pawn.index].PawnId = -1;
                            player.Pawns[pawnId] = (TileType.END, endTilePos);
                            break;
                        }
                        else
                        {
                            // skip a tile
                            newTileIndex++;
                            if (newTileIndex % 10 == 0)
                                newTileIndex++;
                        }
                    }
                    // check if tile is full

                    // Move to the tile
                    Tiles[newTileIndex].PlayerId = playerId;
                    Tiles[newTileIndex].PawnId = pawnId;
                    Tiles[pawn.index].PlayerId = null;
                    Tiles[pawn.index].PawnId = -1;
                    player.Pawns[pawnId] = (TileType.NORMAL, newTileIndex);
                    break;
                default:
                    break;
            }

            player.Moved();
            return true;
        }
    }

    public enum TileType
    {
        START,
        END,
        NORMAL
    }

    public enum GameState
    {
        YOUR_TURN_NOT_ROLLED = 0,
        YOUR_TURN_ROLLED = 1,
        OPONENT_TURN = 2
    }
}
