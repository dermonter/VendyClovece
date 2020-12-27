using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using VendyClovece.Backend;

namespace VendyClovece.Graphics
{
    public class DrawBoard
    {
        public static void Draw(SpriteBatchManager spriteBatchManager, Texture2D tileTexture, Vector2 origin, Board board, float offset)
        {
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

        public static void DrawPlayers(SpriteBatchManager spriteBatchManager, Vector2 origin, List<Pawn[]> players, float offset)
        {
            foreach (var player in players)
            {
                foreach (var pawn in player)
                {
                    var tile = pawn.CurrentTile;
                    Vector2 pos = new Vector2(origin.X + tile.Position.X * offset, origin.Y + tile.Position.Y * offset);
                    spriteBatchManager.DrawCenter(pawn.Texture, pos, pawn.DisplayColor);
                }
            }
        }
    }
}
