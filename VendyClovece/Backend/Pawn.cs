﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VendyClovece.UI;

namespace VendyClovece.Backend
{
    public class Pawn : Clickable
    {
        private readonly Texture2D texture;

        public Color PlayerColor { get; set; }
        public Color DisplayColor
        {
            get
            {
                if (Clicked)
                    return Color.Black;
                else
                    return isHovering ? Color.Gray : PlayerColor;
            }
        }
        public Tile CurrentTile { get; set; }
        public override Vector2 Position => GameLogic.LocalToWorld(CurrentTile.Position);
        public override Texture2D Texture => texture;
        public override ClickableType Type => ClickableType.PAWN;

        public Pawn(Tile tile, Color playerColor, Texture2D texture)
        {
            CurrentTile = tile;
            PlayerColor = playerColor;
            this.texture = texture;
        }
    }
}