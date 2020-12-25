using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Graphics
{
    class SpriteBatchManager : SpriteBatch
    {
        public SpriteBatchManager(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
        }

        public void DrawCenter(Texture2D texture, Vector2 position, Color color)
        {
            Vector2 newPos = position - new Vector2(texture.Width / 2, texture.Height / 2);
            Draw(texture, newPos, color);
        }
    }
}
