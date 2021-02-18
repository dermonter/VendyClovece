using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloveceApiServer.Models
{
    public class GameModel
    {
        private const int PLAYER_COUNT = 2;
        private const int PIECE_COUNT = PLAYER_COUNT * 4;
        private const int TILES_PER_SIDE = 10;

        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public int LastRolled { get; set; }
        public int CurrentPlayer { get; set; }
        public PieceModel[] Pieces { get; set; }

        private Random generator;
        private GameState gameState;

        public GameModel(long id, string name, bool isRunning)
        {
            Id = id;
            Name = name;
            IsRunning = isRunning;
            generator = new Random();
            CurrentPlayer = 0;
            Pieces = new PieceModel[PIECE_COUNT];
            for (int i = 0; i < Pieces.Length; i++)
            {
                Pieces[i] = new PieceModel(i / PLAYER_COUNT, i % (PIECE_COUNT / PLAYER_COUNT), TileType.START);
            }
            gameState = GameState.NOT_ROLLED;
        }

        public GameModel(GameModel reference)
        {
            Id = reference.Id;
            Name = reference.Name;
            IsRunning = reference.IsRunning;
            LastRolled = reference.LastRolled;
            CurrentPlayer = reference.CurrentPlayer;
            Pieces = new PieceModel[PIECE_COUNT];
            for (int i = 0; i < Pieces.Length; i++)
            {
                Pieces[i] = new PieceModel(reference.Pieces[i]);
            }
        }

        public int Roll(int playerId)
        {
            if (playerId != CurrentPlayer)
                return -1;

            if (gameState != GameState.NOT_ROLLED)
                return -1;

            LastRolled = generator.Next(1, 7);
            gameState = GameState.ROLLED;
            return LastRolled;
        }

        public bool Move(int playerId, int pieceId)
        {
            if (playerId != CurrentPlayer)
                return false;

            if (gameState != GameState.ROLLED)
                return false;

            if (pieceId < 0 || pieceId >= 4)
                return false;

            PieceModel piece = Pieces[playerId * PLAYER_COUNT + pieceId];
            int targetPieceId;
            switch (piece.TileType)
            {
                case TileType.START:
                    if (LastRolled < 6)
                        return false;
                    int newTileId = playerId * TILES_PER_SIDE + (LastRolled - 6);
                    int newPos = newTileId % (TILES_PER_SIDE * PLAYER_COUNT);

                    // TODO: Check if full
                    targetPieceId = pieceIdAtTile(newPos, TileType.NORMAL);
                    if (targetPieceId != -1)
                    {
                        if (Pieces[targetPieceId].PlayerId == playerId)
                            return false;

                        int startId = firstEmptyStart(Pieces[targetPieceId].PlayerId);
                        Pieces[targetPieceId].TileType = TileType.START;
                        Pieces[targetPieceId].TileId = startId;
                    }

                    piece.TileType = TileType.NORMAL;
                    piece.TileId = newPos;
                    break;
                case TileType.END:
                    if (piece.TileId + LastRolled >= 4)
                        return false;

                    int targetTileIndex = piece.TileId + LastRolled;

                    // TODO: Check if full
                    targetPieceId = pieceIdAtTile(targetTileIndex, TileType.END);
                    if (targetPieceId != -1)
                    {
                        if (Pieces[targetPieceId].PlayerId == playerId)
                            return false;

                        int startId = firstEmptyStart(Pieces[targetPieceId].PlayerId);
                        Pieces[targetPieceId].TileType = TileType.START;
                        Pieces[targetPieceId].TileId = startId;
                    }

                    piece.TileType = TileType.END;
                    piece.TileId = targetTileIndex;
                    break;
                case TileType.NORMAL:
                    int newTileIndex = (piece.TileId + LastRolled) % (TILES_PER_SIDE * PLAYER_COUNT);
                    if (piece.TileId / TILES_PER_SIDE != newTileIndex / TILES_PER_SIDE)
                    {
                        if (newTileIndex / TILES_PER_SIDE == playerId)
                        {
                            // move to end
                            int endIndex = newTileIndex % TILES_PER_SIDE;
                            if (endIndex >= 4)
                                return false;

                            // TODO: check if end tile is full
                            targetPieceId = pieceIdAtTile(endIndex, TileType.END);
                            if (targetPieceId != -1)
                            {
                                if (Pieces[targetPieceId].PlayerId == playerId)
                                    return false;

                                int startId = firstEmptyStart(Pieces[targetPieceId].PlayerId);
                                Pieces[targetPieceId].TileType = TileType.START;
                                Pieces[targetPieceId].TileId = startId;
                            }

                            // Move to the tile
                            piece.TileType = TileType.END;
                            piece.TileId = endIndex;
                            break;
                        }
                        else
                        {
                            // skip a tile
                            newTileIndex++;
                            if (newTileIndex % TILES_PER_SIDE == 0)
                                newTileIndex++;
                        }
                    }

                    // TODO: check if tile is full
                    targetPieceId = pieceIdAtTile(newTileIndex, TileType.NORMAL);
                    if (targetPieceId != -1)
                    {
                        if (Pieces[targetPieceId].PlayerId == playerId)
                            return false;

                        int startId = firstEmptyStart(Pieces[targetPieceId].PlayerId);
                        Pieces[targetPieceId].TileType = TileType.START;
                        Pieces[targetPieceId].TileId = startId;
                    }

                    // Move to the tile
                    piece.TileType = TileType.NORMAL;
                    piece.TileId = newTileIndex;
                    break;
                default:
                    break;
            }

            CurrentPlayer = (CurrentPlayer + 1) % PLAYER_COUNT;

            return true;
        }

        private int pieceIdAtTile(int tileId, TileType tileType)
        {
            for (int i = 0; i < Pieces.Length; i++)
            {
                if (Pieces[i].TileType == tileType && Pieces[i].TileId == tileId)
                    return i;
            }

            return -1;
        }

        private int firstEmptyStart(int playerId)
        {
            int result = 0;
            bool valChanged;
            do
            {
                valChanged = false;
                for (int i = 0; i < 4; i++)
                {
                    PieceModel piece = Pieces[playerId * 4 + i];
                    if (piece.TileType != TileType.START)
                        continue;

                    if (result == piece.TileId)
                    {
                        result++;
                        valChanged = true;
                    }
                }
            } while (valChanged);
            return result;
        }
    }

    public enum GameState
    {
        NOT_ROLLED = 0,
        ROLLED = 1
    }
}
