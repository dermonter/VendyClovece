using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloveceApiServer.Models
{
    public class PieceModel
    {

        public int PlayerId { get; set; }
        public int TileId { get; set; }
        public TileType TileType { get; set; }

        public PieceModel(int playerId, int tileId, TileType tileType)
        {
            PlayerId = playerId;
            TileId = tileId;
            TileType = tileType;
        }

        public PieceModel(PieceModel reference)
        {
            PlayerId = reference.PlayerId;
            TileId = reference.TileId;
            TileType = reference.TileType;
        }
    }

    public enum TileType
    {
        NORMAL = 0,
        START = 1,
        END = 2
    }
}
