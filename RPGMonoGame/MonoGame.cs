using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using MonoGame.Extended;

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
        Sprite mainMenuButton;
        Sprite quitButton;
        #endregion
        #region initWorldAssets
        string inpText = "";
        string yearNum = "";
        string facCreate = "";
        string facDestroyed = "";
        string eventName = "";
        bool startGen = false;
        Sprite one;
        Sprite two;
        Sprite thr;
        Sprite four;
        Sprite five;
        Sprite six;
        Sprite sev;
        Sprite eight;
        Sprite nine;
        Sprite zer;
        Sprite enter;
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

            set => _state = value;
        }
        public string YearNum { get => yearNum; set => yearNum = value; }
        public string FacCreate { get => facCreate; set => facCreate = value; }
        public string FacDestroyed { get => facDestroyed; set => facDestroyed = value; }
        public string EventName { get => eventName; set => eventName = value; }

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
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();
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
            Vector2 coor = new Vector2(graphics.PreferredBackBufferWidth / 2 - this.Content.Load<Texture2D>("StartButton").Width / 2, graphics.PreferredBackBufferHeight / 2 - this.Content.Load<Texture2D>("StartButton").Height / 2);
            mainMenuButton = new Sprite(this.Content.Load<Texture2D>("StartButton"), coor);
            coor = new Vector2(graphics.PreferredBackBufferWidth / 2 - this.Content.Load<Texture2D>("QuitButton").Width / 2, graphics.PreferredBackBufferHeight / 2 - this.Content.Load<Texture2D>("QuitButton").Height / 2);
            quitButton = new Sprite(this.Content.Load<Texture2D>("QuitButton"), coor);
            one = new Sprite(Content.Load<Texture2D>("One"), new Vector2(100, 100));
            two = new Sprite(Content.Load<Texture2D>("Two"), new Vector2(210, 100));
            thr = new Sprite(Content.Load<Texture2D>("Thr"), new Vector2(320, 100));
            four = new Sprite(Content.Load<Texture2D>("For"), new Vector2(100, 200));
            five = new Sprite(Content.Load<Texture2D>("Fiv"), new Vector2(210, 200));
            six = new Sprite(Content.Load<Texture2D>("Six"), new Vector2(320, 200));
            sev = new Sprite(Content.Load<Texture2D>("Sev"), new Vector2(100, 300));
            eight = new Sprite(Content.Load<Texture2D>("Eight"), new Vector2(210, 300));
            nine = new Sprite(Content.Load<Texture2D>("Nine"), new Vector2(320, 300));
            zer = new Sprite(Content.Load<Texture2D>("Zero"), new Vector2(210, 400));
            enter = new Sprite(Content.Load<Texture2D>("Arrow"), new Vector2(320, 400));
            //290 width
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
            Rectangle mouseRec = new Microsoft.Xna.Framework.Rectangle(mouseState.X, mouseState.Y, 1, 1);
            Rectangle mainMenuRect = mainMenuButton.Texture.Bounds;
            mainMenuRect.X = (int)mainMenuButton.Position.X;
            mainMenuRect.Y = (int)mainMenuButton.Position.Y;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (mainMenuRect.Contains(mouseRec))
                {
                    graphics.GraphicsDevice.Clear(Color.White);
                    State = GameState.StartPage;
                }
            }
        }
        void UpdateStartPage(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            Rectangle mouseRec = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            Rectangle mainMenuRect = mainMenuButton.Texture.Bounds;
            mainMenuRect.X = (int)mainMenuButton.Position.X;
            mainMenuRect.Y = (int)mainMenuButton.Position.Y;
            Rectangle quitButtonRect = quitButton.Texture.Bounds;
            quitButtonRect.X = (int)quitButton.Position.X;
            quitButtonRect.Y = (int)quitButton.Position.Y;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if(mainMenuRect.Contains(mouseRec))
                {
                    graphics.GraphicsDevice.Clear(Color.White);
                    State = GameState.InitWorldPage;
                } else if (quitButtonRect.Contains(mouseRec))
                {
                    Exit();
                }
            }

        }
        void UpdateInitWorldPage(GameTime gameTime)
        {
            
            if (!startGen)
            {

            }
        }
        void UpdateBattlePage(GameTime gameTime)
        {

        }
        void DrawMainMenu(GameTime gameTime)
        {
            spriteBatch.Begin();
            mainMenuButton.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }
        void DrawStartPage(GameTime gameTime)
        {
            Vector2 coor = new Vector2(graphics.PreferredBackBufferWidth / 2 - mainMenuButton.Texture.Width / 2, graphics.PreferredBackBufferHeight / 3 - mainMenuButton.Texture.Height / 2);
            spriteBatch.Begin();
            mainMenuButton.Position = coor;
            mainMenuButton.Draw(spriteBatch, gameTime);
            coor = new Vector2(graphics.PreferredBackBufferWidth / 2 - quitButton.Texture.Width / 2, 2 * graphics.PreferredBackBufferHeight / 3 - quitButton.Texture.Height / 2);
            quitButton.Position = coor;
            quitButton.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }
        void DrawInitWorldPage(GameTime gameTime)
        {
            spriteBatch.Begin();
            one.Draw(spriteBatch, gameTime);
            two.Draw(spriteBatch, gameTime);
            thr.Draw(spriteBatch, gameTime);
            four.Draw(spriteBatch, gameTime);
            five.Draw(spriteBatch, gameTime);
            six.Draw(spriteBatch, gameTime);
            sev.Draw(spriteBatch, gameTime);
            eight.Draw(spriteBatch, gameTime);
            nine.Draw(spriteBatch, gameTime);
            zer.Draw(spriteBatch, gameTime);
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
    public class NumberBox
    {
        public Vector2 Position { get; set; }
        public Texture2D Background { get; set; }
        public string Text { get; set; }

        public NumberBox(Vector2 pos, string text, ContentManager content)
        {
            Position = pos;
            Background = content.Load<Texture2D>("TextBox");
            Text = text;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, SpriteFont sf)
        {
            spriteBatch.Draw(Background, Position, Color.White);
            spriteBatch.DrawString(sf, Text, new Vector2(Position.X + 2, Position.Y + 2), Color.Black);
        }

        public string EnterText()
        {
            return "";
        }
    }
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

        public Sprite(Texture2D texture, Vector2 initialPosition)
        {
            Texture = texture;
            Position = initialPosition;
        }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
