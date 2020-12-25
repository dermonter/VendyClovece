using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VendyClovece.Graphics
{
    public class SpriteBatchManager : SpriteBatch
    {
        public SpriteBatchManager(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
        }

        public void DrawCenter(Texture2D texture, Vector2 position, Color color)
        {
            Vector2 newOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Draw(texture, position, null, color, 0f, newOrigin, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
