using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VendyClovece.Graphics;

namespace VendyClovece.UI
{
    public abstract class Component : Clickable
    {
        public abstract void Draw(GameTime gameTime, SpriteBatchManager spriteBatchManager);
    }
}
