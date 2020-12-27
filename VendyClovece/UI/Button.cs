using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using VendyClovece.Graphics;

namespace VendyClovece.UI
{
    public class Button : Component
    {
        private readonly Texture2D texture;
        private Vector2 position;

        public Color PenColor { get; set; }
        public override Texture2D Texture => texture;
        public override Vector2 Position => position;
        public override ClickableType Type => ClickableType.BUTTON;

        public Button(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            PenColor = Color.Black;
            this.position = position;
        }

        public override void Draw(GameTime gameTime, SpriteBatchManager spriteBatchManager)
        {
            var color = Color.White;
            if (isHovering)
                color = Color.Gray;

            spriteBatchManager.Draw(texture, Rectangle, color);
        }
    }
}
