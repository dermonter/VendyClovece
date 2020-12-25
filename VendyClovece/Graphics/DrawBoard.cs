using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VendyClovece.Backend;

namespace VendyClovece.Graphics
{
    class DrawBoard
    {
        public static void Draw(SpriteBatchManager spriteBatchManager, Texture2D tileTexture, Vector2 origin, Board board)
        {
            var offset = tileTexture.Width;

            foreach (var tile in board.Tiles)
            {
                Vector2 pos = new Vector2(origin.X + tile.Position.X * offset, origin.Y + tile.Position.Y * offset);
                spriteBatchManager.DrawCenter(tileTexture, pos, tile.BgColor);
            }

            foreach (var tile in board.EndTiles)
            {
                Vector2 pos = new Vector2(origin.X + tile.Position.X * offset, origin.Y + tile.Position.Y * offset);
                spriteBatchManager.DrawCenter(tileTexture, pos, tile.BgColor);
            }

            foreach (var tile in board.StartTiles)
            {
                Vector2 pos = new Vector2(origin.X + tile.Position.X * offset, origin.Y + tile.Position.Y * offset);
                spriteBatchManager.DrawCenter(tileTexture, pos, tile.BgColor);
            }
        }
    }
}
