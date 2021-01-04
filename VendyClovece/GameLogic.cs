using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VendyClovece.Backend;
using VendyClovece.Graphics;
using VendyClovece.UI;
using System.Linq;
using VendyClovece.Client;

namespace VendyClovece
{
    public class GameLogic : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatchManager _spriteBatch;
        private readonly GameMaster _gameMaster;

        public ProgramState ProgramState;

        private Texture2D tileTexture;
        private Texture2D pawnTexture;
        private Texture2D diceTexture;
        private SpriteFont font;

        private readonly List<(ProgramState layer, Clickable click)> clickables;
        private readonly List<(ProgramState layer, Component component)> uiComponents;

        private float offset;
        private Vector2 origin;

        private string rolledText => _gameMaster.roll == -1 ? null : _gameMaster.roll.ToString();

        public static GameLogic Instance { get; private set; }

        public GameLogic()
        {
            _graphics = new GraphicsDeviceManager(this);
            _gameMaster = new GameMaster();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            clickables = new List<(ProgramState layer, Clickable click)>();
            uiComponents = new List<(ProgramState layer, Component component)>();
            if (Instance != null)
                throw new NullReferenceException("More than one instance found!!!");
            Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ProgramState = ProgramState.MAIN_MENU;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatchManager(GraphicsDevice);
            tileTexture = Content.Load<Texture2D>("tile");
            pawnTexture = Content.Load<Texture2D>("pawn");
            diceTexture = Content.Load<Texture2D>("dice");
            font = Content.Load<SpriteFont>("font");
            offset = tileTexture.Width;
            origin = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            var startGameButton = new Button(diceTexture, new Vector2(200, 200));
            startGameButton.Click += StartGameButton_Click;
            clickables.Add((ProgramState.MAIN_MENU, startGameButton));
            uiComponents.Add((ProgramState.MAIN_MENU, startGameButton));

            var diceButton = new Button(diceTexture, new Vector2(50, 50));
            diceButton.Click += DiceButton_Click;
            clickables.Add((ProgramState.GAME, diceButton));
            uiComponents.Add((ProgramState.GAME, diceButton));

            var c = _gameMaster.InitPlayers(pawnTexture);
            clickables.AddRange(c.Select(cl => (ProgramState.GAME, cl)));

            foreach (var clickable in clickables)
            {
                switch (clickable.click.Type)
                {
                    case ClickableType.PAWN:
                        clickable.click.Click += Pawn_Click;
                        break;
                    case ClickableType.BUTTON:
                        break;
                    default:
                        break;
                }
            }
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            ClientSend.GetGameState();
            ClientSend.GetBoard();
            ProgramState = ProgramState.GAME;
        }

        private void DiceButton_Click(object sender, EventArgs e)
        {
            _gameMaster.Roll();
        }

        private void Pawn_Click(object sender, EventArgs e)
        {
            Pawn pawn = (Pawn)sender;

            _gameMaster.SelectPawn(pawn);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (var clickable in clickables)
            {
                if (clickable.layer != ProgramState)
                    continue;

                clickable.click.Update(gameTime);
            }

            switch (ProgramState)
            {
                case ProgramState.MAIN_MENU:
                    break;
                case ProgramState.GAME:

                    _gameMaster.EmulateEnemy();
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (var componenet in uiComponents)
            {
                if (componenet.layer != ProgramState)
                    continue;

                componenet.component.Draw(gameTime, _spriteBatch);
            }

            switch (ProgramState)
            {
                case ProgramState.MAIN_MENU:
                    break;
                case ProgramState.GAME:
                    DrawBoard.Draw(_spriteBatch, tileTexture, origin, _gameMaster.Board, offset);
                    DrawBoard.DrawPlayers(_spriteBatch, origin, _gameMaster.Players, offset);
                    if (!string.IsNullOrEmpty(rolledText))
                        _spriteBatch.DrawString(font, rolledText, new Vector2(50, 5), Color.Black);
                    break;
                default:
                    break;
            }

            _spriteBatch.DrawString(font, ClientTcp.myPlayerId.ToString(), new Vector2(0, 0), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Vector2 LocalToWorld(Vector2 local) => new Vector2(Instance.origin.X + Instance.offset * local.X, Instance.origin.Y + Instance.offset * local.Y);
    }

    public enum ProgramState
    {
        MAIN_MENU,
        GAME
    }
}
