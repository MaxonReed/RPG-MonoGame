using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace RPGMonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MonoGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont arialFont;
        #region MainMenuAssets
        Texture2D mainMenuButton;
        Texture2D quitButton;
        #endregion
        public enum GameState
        {
            MainMenu,
            StartPage,
            InitWorldPage,
            BattlePage
        }
        private GameState _state;
        public GameState State
        {
            get => _state;

            set => value = _state;
        }
        public MonoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            arialFont = Content.Load<SpriteFont>("StartButtonFont");
            mainMenuButton = this.Content.Load<Texture2D>("StartButton");
            quitButton = this.Content.Load<Texture2D>("QuitButton");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.
                Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
            switch (State)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.StartPage:
                    UpdateStartPage(gameTime);
                    break;
                case GameState.InitWorldPage:
                    UpdateInitWorldPage(gameTime);
                    break;
                case GameState.BattlePage:
                    UpdateBattlePage(gameTime);
                    break;
                default:
                    break;
            }
        }

        void UpdateMainMenu(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            Rectangle mouseRec = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            Rectangle mainMenuRect = mainMenuButton.Bounds;
            mainMenuRect.X = 
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine("hello");
                if (mainMenuButton.Bounds.Contains(mouseState.X, mouseState.Y))
                {
                    Debug.WriteLine("bye");
                    graphics.GraphicsDevice.Clear(Color.White);
                    State = GameState.StartPage;
                    Debug.WriteLine(State);
                }
            }
        }
        void UpdateStartPage(GameTime gameTime)
        {

        }
        void UpdateInitWorldPage(GameTime gameTime)
        {

        }
        void UpdateBattlePage(GameTime gameTime)
        {

        }
        void DrawMainMenu(GameTime gameTime)
        {
            Vector2 coor = new Vector2(graphics.PreferredBackBufferWidth / 2 - mainMenuButton.Width / 2, graphics.PreferredBackBufferHeight / 2 - mainMenuButton.Height / 2);
            spriteBatch.Begin();
            spriteBatch.Draw(mainMenuButton, coor, Color.White);
            spriteBatch.End();
        }
        void DrawStartPage(GameTime gameTime)
        {
            Vector2 coor = new Vector2(graphics.PreferredBackBufferWidth / 2 - mainMenuButton.Width / 2, graphics.PreferredBackBufferHeight / 3 - mainMenuButton.Height / 2);
            spriteBatch.Begin();
            spriteBatch.Draw(mainMenuButton, coor, Color.White);
            coor = new Vector2(graphics.PreferredBackBufferWidth / 2 - mainMenuButton.Width / 2, 2 * graphics.PreferredBackBufferHeight / 3 - mainMenuButton.Height / 2);
            spriteBatch.Draw(quitButton, coor, Color.White);
            spriteBatch.End();
        }
        void DrawInitWorldPage(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.End();
        }
        void DrawBattlePage(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.End();
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
            switch (State)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.StartPage:
                    DrawStartPage(gameTime);
                    break;
                case GameState.InitWorldPage:
                    DrawInitWorldPage(gameTime);
                    break;
                case GameState.BattlePage:
                    DrawBattlePage(gameTime);
                    break;
                default:
                    break;
            }
        }

        
    }
}
