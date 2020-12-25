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
            int currTile = 0;

            for (int i = 0; i < 4; i++)
            {
                float angle = (float)Math.PI / 2 * i;
                float originX = origin.X + offset;
                float originY = origin.Y - offset * 5;

                for (int j = 0; j < 5; j++)
                {
                    Vector2 pos = new Vector2(originX, originY + j * offset);
                    pos = RotateAroundOrigin(pos, origin, angle);
                    spriteBatchManager.DrawCenter(tileTexture, pos, board.Tiles[currTile++]);
                }

                for (int j = 0; j < 4; j++)
                {
                    Vector2 pos = new Vector2(originX + (j + 1) * offset, originY + offset * 4);
                    pos = RotateAroundOrigin(pos, origin, angle);
                    spriteBatchManager.DrawCenter(tileTexture, pos, board.Tiles[currTile++]);
                }

                Vector2 lastPos = RotateAroundOrigin(new Vector2(originX + 4 * offset, originY + 5 * offset), origin, angle);
                spriteBatchManager.DrawCenter(tileTexture, lastPos, board.Tiles[currTile++]);
            }
        }

        private static Vector2 RotateAroundOrigin(Vector2 point, Vector2 origin, float angle)
        {
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);

            // translate point back to origin:
            float px = point.X - origin.X;
            float py = point.Y - origin.Y;

            // rotate point
            float xnew = px * c - py * s;
            float ynew = px * s + py * c;

            // translate point back:
            px = xnew + origin.X;
            py = ynew + origin.Y;
            return new Vector2(px, py);
        }
    }
}
