using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VendyClovece.Backend;
using VendyClovece.Graphics;

namespace VendyClovece
{
    public class GameLogic : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatchManager _spriteBatch;

        private Board board;

        private Texture2D tileTexture;
        private Texture2D pawnTexture;

        public GameLogic()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            // TODO: use this.Content to load your game content here
            tileTexture = Content.Load<Texture2D>("tile");
            pawnTexture = Content.Load<Texture2D>("pawn");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Vector2 center = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            DrawBoard.Draw(_spriteBatch, tileTexture, center, board);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
