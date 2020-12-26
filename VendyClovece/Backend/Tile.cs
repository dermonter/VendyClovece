using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Backend
{
    public class Tile
    {
        private static int idCounter = 0;

        public Color BgColor { get; private set; }
        public Vector2 Position { get; private set; }
        public void SetPosition(Vector2 value)
        {
            Position = value;
        }
        public int Id { get; private set; }
        public TileType TileType { get; private set; }

        public Tile(Color bgColor, TileType tileType)
        {
            BgColor = bgColor;
            Id = idCounter++;
            TileType = tileType;
        }

        public Tile(Color bgColor, Vector2 position, TileType tileType)
        {
            BgColor = bgColor;
            Position = position;
            Id = idCounter++;
            TileType = tileType;
        }
    }

    public enum TileType
    {
        NORMAL,
        END,
        START,
        STARTBOARD
    }
}
