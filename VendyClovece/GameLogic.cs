using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VendyClovece.Backend;
using VendyClovece.Graphics;
using VendyClovece.UI;

namespace VendyClovece
{
    public class GameLogic : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatchManager _spriteBatch;

        private Board board;
        private List<Pawn[]> players;

        private Texture2D tileTexture;
        private Texture2D pawnTexture;

        private List<Clickable> clickables;

        private float offset;
        private Vector2 origin;

        public static GameLogic Instance { get; private set; }

        public GameLogic()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            if (Instance != null)
                throw new NullReferenceException("More than one instance found!!!");
            Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            board = new Board();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatchManager(GraphicsDevice);

            clickables = new List<Clickable>();
            tileTexture = Content.Load<Texture2D>("tile");
            pawnTexture = Content.Load<Texture2D>("pawn");
            offset = tileTexture.Width;
            origin = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            players = new List<Pawn[]> { new Pawn[4], new Pawn[4] };
            var c = board.InitPlayers(players, pawnTexture);
            clickables.AddRange(c);

            foreach (var clickable in clickables)
            {
                clickable.Click += Pawn_Click;
            }
        }

        private void Pawn_Click(object sender, EventArgs e)
        {
            Pawn pawn = (Pawn)sender;

            pawn.Clicked = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (var clickable in clickables)
            {
                clickable.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            DrawBoard.Draw(_spriteBatch, tileTexture, origin, board, offset);
            DrawBoard.DrawPlayers(_spriteBatch, origin, players, offset);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Vector2 LocalToWorld(Vector2 local) => new Vector2(Instance.origin.X + Instance.offset * local.X, Instance.origin.Y + Instance.offset * local.Y);
    }

    public enum GameState
    {
        YOUR_TURN_NOT_ROLLED,
        YOUR_TURN_ROLLED,
        OPONENT_TURN
    }
}
