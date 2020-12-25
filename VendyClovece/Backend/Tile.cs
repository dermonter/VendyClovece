using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Backend
{
    class Tile
    {
        public Color BgColor { get; private set; }
        public Vector2 Position { get; private set; }
        public void SetPosition(Vector2 value)
        {
            Position = value;
        }

        public Tile(Color bgColor)
        {
            BgColor = bgColor;
        }

        public Tile(Color bgColor, Vector2 position)
        {
            BgColor = bgColor;
            Position = position;
        }
    }
}
