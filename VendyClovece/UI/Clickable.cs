using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace VendyClovece.UI
{
    public abstract class Clickable
    {
        private MouseState currentMouseState;
        protected bool isHovering;
        private MouseState previousMouseState;
        public abstract Texture2D Texture { get; }

        public event EventHandler Click;
        public bool Clicked { get; set; }
        public abstract Vector2 Position { get; }
        public abstract ClickableType Type { get; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X - Texture.Width / 2, (int)Position.Y - Texture.Height / 2, Texture.Width, Texture.Height);
            }
        }

        public void Update(GameTime gameTime)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);

            isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }

    public enum ClickableType
    {
        PAWN,
        BUTTON
    }
}
