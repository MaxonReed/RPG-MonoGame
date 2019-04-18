using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using MonoGame.Extended;
using System.Threading.Tasks;
using System.Collections.Generic;
using RPGv2;
using System.Threading;

namespace RPGMonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MyGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont arialFont;
        MouseState mouseState = Mouse.GetState();
        MouseState prevState;
        bool saveEnabled = false;
        Stopwatch timer = new Stopwatch();
        #region MainMenuAssets
        Sprite mainMenuButton;
        Sprite quitButton;
        Sprite loadGame;
        #endregion
        #region initWorldAssets
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
        Sprite initBox;
        #endregion
        #region storyAssets
        SpriteFont storyFont;
        SpriteFont storyFontI;
        Sprite textBox;
        #endregion
        #region characterCreationAssets
        Sprite classSelectMage;
        Sprite classSelectMageHover;
        Sprite classSelectRogue;
        Sprite classSelectRogueHover;
        Sprite classSelectWarrior;
        Sprite classSelectWarriorHover;
        #endregion
        #region battleAssets
        Sprite battleButton;
        Sprite battleButtonHover;
        Sprite runButton;
        Sprite runButtonHover;
        Sprite skillButton;
        Sprite skillButtonHover;
        Sprite magicButton;
        Sprite magicButtonHover;
        Sprite firstSpecial;
        Sprite secondSpecial;
        Sprite thirdSpecial;
        Sprite fourthSpecial;
        Sprite defHover1;
        Sprite defHover2;
        Sprite defHover3;
        Sprite defHover4;
        bool clickedSpecial = false;
        bool clickedRun = false;
        bool canRun = true;
        int damagePlayer = -1;
        int damageEnemy = -1;
        bool dispPlayerDamage = true;
        #endregion
        public enum GameState
        {
            MainMenu,
            StartPage,
            InitWorldPage,
            BattlePage,
            StoryText,
            CharacterCreate
        }
        private GameState _state;
        public GameState State
        {
            get => _state;

            set => _state = value;
        }

        public MyGame()
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
            timer.Start();
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
            storyFont = Content.Load<SpriteFont>("Story");
            storyFontI = Content.Load<SpriteFont>("StoryItalics");
            Vector2 coor = new Vector2(graphics.PreferredBackBufferWidth / 2 - Content.Load<Texture2D>("StartButton").Width / 2, graphics.PreferredBackBufferHeight / 2 - Content.Load<Texture2D>("StartButton").Height / 2 - 70);
            mainMenuButton = new Sprite(Content.Load<Texture2D>("StartButton"), coor);
            coor = new Vector2(graphics.PreferredBackBufferWidth / 2 - Content.Load<Texture2D>("QuitButton").Width / 2, graphics.PreferredBackBufferHeight / 2 - Content.Load<Texture2D>("QuitButton").Height / 2);
            quitButton = new Sprite(Content.Load<Texture2D>("QuitButton"), coor);
            loadGame = new Sprite(Content.Load<Texture2D>("LoadGame"), new Vector2(mainMenuButton.Position.X, mainMenuButton.Position.Y + 140));
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
            initBox = new Sprite(Content.Load<Texture2D>("NumInput"), new Vector2(100, 20));
            textBox = new Sprite(Content.Load<Texture2D>("TextBox"), new Vector2(100, 315));
            int middle = graphics.PreferredBackBufferWidth / 2 - Content.Load<Texture2D>("ClassSelectRogue").Width / 2;
            int middleY = graphics.PreferredBackBufferHeight / 2 - Content.Load<Texture2D>("ClassSelectRogue").Height / 2;
            classSelectMage = new Sprite(Content.Load<Texture2D>("ClassSelectMage"), new Vector2(middle, middleY - 150));
            classSelectMageHover = new Sprite(Content.Load<Texture2D>("ClassSelectMageHover"), new Vector2(middle, middleY - 150));
            classSelectRogue = new Sprite(Content.Load<Texture2D>("ClassSelectRogue"), new Vector2(middle, middleY));
            classSelectRogueHover = new Sprite(Content.Load<Texture2D>("ClassSelectRogueHover"), new Vector2(middle, middleY));
            classSelectWarrior = new Sprite(Content.Load<Texture2D>("ClassSelectWarrior"), new Vector2(middle, middleY + 150));
            classSelectWarriorHover = new Sprite(Content.Load<Texture2D>("ClassSelectWarriorHover"), new Vector2(middle, middleY + 150));
            battleButton = new Sprite(Content.Load<Texture2D>("battleButton"), new Vector2(200, 350));
            battleButtonHover = new Sprite(Content.Load<Texture2D>("battleButtonHover"), battleButton.Position);
            runButton = new Sprite(Content.Load<Texture2D>("runButton"), new Vector2(600, 350));
            runButtonHover = new Sprite(Content.Load<Texture2D>("runButtonHover"), runButton.Position);
            skillButton = new Sprite(Content.Load<Texture2D>("skillsButton"), new Vector2(400, 350));
            skillButtonHover = new Sprite(Content.Load<Texture2D>("skillsButtonHover"), skillButton.Position);
            magicButton = new Sprite(Content.Load<Texture2D>("magicButton"), new Vector2(400, 350));
            magicButtonHover = new Sprite(Content.Load<Texture2D>("magicButtonHover"), magicButton.Position);
            firstSpecial = new Sprite(Content.Load<Texture2D>("DefaultButton"), new Vector2(100, 350));
            secondSpecial = new Sprite(Content.Load<Texture2D>("DefaultButton"), new Vector2(300, 350));
            thirdSpecial = new Sprite(Content.Load<Texture2D>("DefaultButton"), new Vector2(500, 350));
            fourthSpecial = new Sprite(Content.Load<Texture2D>("DefaultButton"), new Vector2(700, 350));
            defHover1 = new Sprite(Content.Load<Texture2D>("DefaultButtonHover"), new Vector2(100, 350));
            defHover2 = new Sprite(Content.Load<Texture2D>("DefaultButtonHover"), new Vector2(300, 350));
            defHover3 = new Sprite(Content.Load<Texture2D>("DefaultButtonHover"), new Vector2(500, 350));
            defHover4 = new Sprite(Content.Load<Texture2D>("DefaultButtonHover"), new Vector2(700, 350));

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (timer.ElapsedMilliseconds >= 5000)
            {
                timer.Restart();
                if (saveEnabled)
                    GlobalValues.save.SaveGame();
            }
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
                case GameState.StoryText:
                    UpdateStoryText(gameTime);
                    break;
                case GameState.CharacterCreate:
                    UpdateCharacterCreate(gameTime);
                    break;
                default:
                    break;
            }
        }
        void UpdateCharacterCreate(GameTime gameTime)
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
            {
                if (classSelectMage.Contains(mousePoint))
                {
                    GamePlay.player = new Player(1, 1);
                    State = GameState.StoryText;
                    saveEnabled = true;
                }
                if (classSelectRogue.Contains(mousePoint))
                {
                    GamePlay.player = new Player(1, 2);
                    State = GameState.StoryText;
                    saveEnabled = true;
                }
                if (classSelectWarrior.Contains(mousePoint))
                {
                    GamePlay.player = new Player(1, 3);
                    State = GameState.StoryText;
                    saveEnabled = true;
                }

            }
        }
        void UpdateMainMenu(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (mainMenuButton.Contains(mousePoint))
                {
                    graphics.GraphicsDevice.Clear(Color.White);
                    State = GameState.StartPage;
                }
                if (loadGame.Contains(mousePoint))
                {
                    GlobalValues.save.LoadGame();
                    State = GameState.StoryText;
                }
            }
        }
        void UpdateStartPage(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (mainMenuButton.Contains(mousePoint))
                {
                    graphics.GraphicsDevice.Clear(Color.White);
                    State = GameState.InitWorldPage;
                }
                else if (quitButton.Contains(mousePoint))
                {
                    Exit();
                }
            }

        }
        void UpdateInitWorldPage(GameTime gameTime)
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            if (!GlobalValues.startGen)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                {
                    if (enter.Contains(mousePoint))
                    {
                        if (GlobalValues.inpText != "")
                            Task.Run(() => GamePlay.StartGame());
                    }
                    if (one.Contains(mousePoint))
                        GlobalValues.inpText += "1";
                    if (two.Contains(mousePoint))
                        GlobalValues.inpText += "2";
                    if (thr.Contains(mousePoint))
                        GlobalValues.inpText += "3";
                    if (four.Contains(mousePoint))
                        GlobalValues.inpText += "4";
                    if (five.Contains(mousePoint))
                        GlobalValues.inpText += "5";
                    if (six.Contains(mousePoint))
                        GlobalValues.inpText += "6";
                    if (sev.Contains(mousePoint))
                        GlobalValues.inpText += "7";
                    if (eight.Contains(mousePoint))
                        GlobalValues.inpText += "8";
                    if (nine.Contains(mousePoint))
                        GlobalValues.inpText += "9";
                    if (zer.Contains(mousePoint))
                        GlobalValues.inpText += "0";
                }

            }
            if (GlobalValues.done)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (mainMenuButton.Contains(mousePoint))
                    {
                        State = GameState.CharacterCreate;
                        GlobalValues.storyState = 1;
                    }
                }
            }
        }
        void UpdateBattlePage(GameTime gameTime)
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            switch (GlobalValues.battleState)
            {
                case "prologue":
                    if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                    {
                        Battle.round++;
                        GlobalValues.battleState = "battle";
                    }
                    break;
                case "battle":
                    if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                    {
                        if (clickedRun)
                            clickedRun = false;
                        if (!clickedSpecial)
                        {
                            if (magicButton.Contains(mousePoint))
                                clickedSpecial = true;
                            if (runButton.Contains(mousePoint))
                                clickedRun = true;
                            if (battleButton.Contains(mousePoint))
                            {
                                damagePlayer = Battle.RegularAttack();
                                damageEnemy = Battle.RegularAttack();
                                GlobalValues.battleState = "damageDealt";
                            }
                        }
                        else
                        {
                            if (firstSpecial.Contains(mousePoint))
                            {
                                damagePlayer = Battle.HandleSpecial(GamePlay.player.Special[0]);
                                GlobalValues.battleState = "damageDealt";
                            }
                            if (secondSpecial.Contains(mousePoint))
                            {
                                damagePlayer = Battle.HandleSpecial(GamePlay.player.Special[1]);
                                GlobalValues.battleState = "damageDealt";
                            }
                            if (thirdSpecial.Contains(mousePoint))
                            {
                                damagePlayer = Battle.HandleSpecial(GamePlay.player.Special[2]);
                                GlobalValues.battleState = "damageDealt";
                            }
                            if (fourthSpecial.Contains(mousePoint))
                            {
                                damagePlayer = Battle.HandleSpecial(GamePlay.player.Special[3]);
                                GlobalValues.battleState = "damageDealt";
                            }
                            if (quitButton.Contains(mousePoint))
                                clickedSpecial = false;
                        }

                    }
                    break;
                case "damageDealt":
                    if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                    {
                        if (!dispPlayerDamage)
                            GlobalValues.battleState = "battle";
                        else
                            dispPlayerDamage = false;
                    }
                    break;
                default:
                    break;
            }
        }
        void UpdateStoryText(GameTime gameTime)
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
            {
                GlobalValues.storyIndex++;
            }
        }
        void DrawMainMenu(GameTime gameTime)
        {
            spriteBatch.Begin();
            mainMenuButton.Draw(spriteBatch, gameTime);
            loadGame.Draw(spriteBatch, gameTime);
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
            enter.Draw(spriteBatch, gameTime);
            initBox.Draw(spriteBatch, gameTime);
            spriteBatch.DrawString(arialFont, GlobalValues.inpText, new Vector2(initBox.Position.X + 4, initBox.Position.Y + 4), Color.Black);
            if (GlobalValues.startGen)
            {
                spriteBatch.DrawString(arialFont, GlobalValues.yearNum, new Vector2(500, 100), Color.Black);
                spriteBatch.DrawString(arialFont, GlobalValues.eventName, new Vector2(500, 200), Color.Black);
                spriteBatch.DrawString(arialFont, GlobalValues.facCreate, new Vector2(500, 300), Color.Black);
                spriteBatch.DrawString(arialFont, GlobalValues.facDestroyed, new Vector2(500, 400), Color.Black);
            }
            if (GlobalValues.done)
            {
                mainMenuButton.Draw(spriteBatch, gameTime);
            }
            spriteBatch.End();
        }
        void DrawStoryText(GameTime gameTime)
        {
            SceneText.GetText();
            //get array of text
            List<string> strArray = SceneText.strArr;
            //get text to be displayed
            string text = strArray[GlobalValues.storyIndex];
            spriteBatch.Begin();
            //draw text box
            textBox.Draw(spriteBatch, gameTime);
            //get specieal events
            if (text.StartsWith("|battle:"))
            {
                string id = text.Substring(11);
                GlobalValues.battleID = id;
                State = GameState.BattlePage;
                spriteBatch.End();
                return;
            }
            //text wrapping
            if (!SceneText.wrapText[GlobalValues.storyIndex])
            {
                if (text[0] == '*')
                    spriteBatch.DrawString(storyFontI, text.Substring(1), new Vector2(110, 319), Color.Black);
                else
                    spriteBatch.DrawString(storyFont, text, new Vector2(110, 319), Color.Black);
            }
            else
            {
                List<string> textWrappers = new List<string>();
                int iterations = text.Length / 50;
                int begin = 0;
                int next = 0;
                for (int i = 0; i <= iterations; i++)
                {
                    begin += next;
                    next = 50;
                    if (50 + begin > text.Length)
                        next = text.Length - begin;
                    //dont split sentences/words
                    bool done = false;
                    while (!done)
                    {
                        if (begin + next < text.Length)
                        {
                            if (text[begin + next] != ' ' && text[begin + next] != '.')
                            {
                                next++;
                            }
                            else
                                done = true;
                        }
                        else
                            done = true;
                    }
                    textWrappers.Add(text.Substring(begin, next));
                }
                if (textWrappers[textWrappers.Count - 1] == ".")
                {
                    textWrappers.RemoveAt(textWrappers.Count - 1);
                    textWrappers[textWrappers.Count - 1] += ".";
                }
                for (int i = 0; i < textWrappers.Count; i++)
                {
                    if (textWrappers[i].StartsWith(" "))
                        textWrappers[i] = textWrappers[i].Substring(1);
                    if (textWrappers[i][0] == '*')
                        spriteBatch.DrawString(storyFontI, textWrappers[i].Substring(1), new Vector2(110, 319 + (i * 22)), Color.Black);
                    else
                        spriteBatch.DrawString(storyFont, textWrappers[i], new Vector2(110, 319 + (i * 22)), Color.Black);
                }
            }
            spriteBatch.End();
        }
        void DrawBattlePage(GameTime gameTime)
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            string fightText = "";
            graphics.GraphicsDevice.Clear(Color.Red);


            spriteBatch.Begin();
            switch (GlobalValues.battleID)
            {
                case "firstBattle":
                    if (Battle.round == -1)
                    {
                        fightText = "You dare fight me fool?!";
                        Battle.e = new Enemy("Unknown StartGame");
                        Battle.p = new Player(GamePlay.player);
                        Battle.enemyHP = Battle.e.Health;
                        Battle.playerHP = Battle.p.Health;
                        Battle.turn = Battle.p.Speed > Battle.e.Speed;
                        if (!Battle.turn)
                        {
                            damageEnemy = Battle.RegularAttack();
                            GlobalValues.battleState = "damageDealt";
                        }
                    }
                    break;
                default:
                    break;
            }
            switch (GlobalValues.battleState)
            {
                case "prologue":
                    textBox.Draw(spriteBatch, gameTime);
                    spriteBatch.DrawString(storyFont, fightText, new Vector2(110, 319), Color.Black);
                    break;
                case "battle":
                    #region battleCase
                    if (clickedRun)
                    {
                        graphics.GraphicsDevice.Clear(Color.Blue);
                        textBox.Draw(spriteBatch, gameTime);
                        if (!canRun)
                        {
                            spriteBatch.DrawString(storyFont, "You can't run.", new Vector2(textBox.Position.X + 5, textBox.Position.Y + 5), Color.Black);
                        }
                    }
                    if (!clickedSpecial)
                    {
                        if (battleButton.Contains(mousePoint))
                            battleButtonHover.Draw(spriteBatch, gameTime);
                        else
                            battleButton.Draw(spriteBatch, gameTime);
                        if (runButton.Contains(mousePoint))
                            runButtonHover.Draw(spriteBatch, gameTime);
                        else
                            runButton.Draw(spriteBatch, gameTime);
                        if (GamePlay.player.Class == "Mage")
                        {
                            if (magicButton.Contains(mousePoint))
                                magicButtonHover.Draw(spriteBatch, gameTime);
                            else
                                magicButton.Draw(spriteBatch, gameTime);
                        }
                        else
                        {
                            if (skillButton.Contains(mousePoint))
                                skillButtonHover.Draw(spriteBatch, gameTime);
                            else
                                skillButton.Draw(spriteBatch, gameTime);
                        }
                    }
                    else
                    {
                        quitButton.Draw(spriteBatch, gameTime);
                        if (firstSpecial.Contains(mousePoint))
                            defHover1.Draw(spriteBatch, gameTime);
                        else
                            firstSpecial.Draw(spriteBatch, gameTime);
                        if (secondSpecial.Contains(mousePoint))
                            defHover2.Draw(spriteBatch, gameTime);
                        else
                            secondSpecial.Draw(spriteBatch, gameTime);
                        if (thirdSpecial.Contains(mousePoint))
                            defHover3.Draw(spriteBatch, gameTime);
                        else
                            thirdSpecial.Draw(spriteBatch, gameTime);
                        if (fourthSpecial.Contains(mousePoint))
                            defHover4.Draw(spriteBatch, gameTime);
                        else
                            fourthSpecial.Draw(spriteBatch, gameTime);
                        int sp;
                        string name = "";
                        for (int i = 0; i < 4; i++)
                        {
                            sp = GamePlay.player.Special[i];
                            switch (sp)
                            {
                                case 0:
                                    name = "None";
                                    break;
                                case 1:
                                    name = "Fire Ball";
                                    break;
                                case 2:
                                    name = "Hide";
                                    break;
                                case 3:
                                    name = "Hard Hit";
                                    break;
                                case 4:
                                    name = "Butter Up";
                                    break;
                                default:
                                    break;
                            }
                            switch (i)
                            {
                                case 0:
                                    spriteBatch.DrawString(storyFont, name, new Vector2(firstSpecial.Position.X + 5, firstSpecial.Position.Y + 7), Color.Black);
                                    break;
                                case 1:
                                    spriteBatch.DrawString(storyFont, name, new Vector2(secondSpecial.Position.X + 5, secondSpecial.Position.Y + 7), Color.Black);
                                    break;
                                case 2:
                                    spriteBatch.DrawString(storyFont, name, new Vector2(thirdSpecial.Position.X + 5, thirdSpecial.Position.Y + 7), Color.Black);
                                    break;
                                case 3:
                                    spriteBatch.DrawString(storyFont, name, new Vector2(fourthSpecial.Position.X + 5, fourthSpecial.Position.Y + 7), Color.Black);
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                    #endregion
                    break;
                case "damageDealt":
                    graphics.GraphicsDevice.Clear(Color.White);
                    textBox.Draw(spriteBatch, gameTime);
                    if (damagePlayer != -1)
                    {
                        if (dispPlayerDamage)
                        {
                            spriteBatch.DrawString(storyFont, "You dealt " + damagePlayer + " damage to " + Battle.e.Name + ".", new Vector2(110, 319), Color.Black);
                        }
                        else
                        {
                            spriteBatch.DrawString(storyFont, Battle.e.Name + " dealt " + damageEnemy + " to you.", new Vector2(110, 319), Color.Black);
                        }
                    }
                    else
                    {
                        spriteBatch.DrawString(storyFont, Battle.e.Name + " dealt " + damagePlayer + " to you.", new Vector2(110, 319), Color.Black);
                    }
                    Debug.WriteLine($"Enemy HP: {Battle.enemyHP}/{Battle.e.Health}");
                    Debug.WriteLine($"Player HP: {Battle.playerHP}/{Battle.p.Health}");
                    break;
                default:
                    break;
            }
            spriteBatch.End();
        }
        void DrawCharacterCreate(GameTime gameTime)
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            spriteBatch.Begin();
            if (classSelectMage.Contains(mousePoint))
                classSelectMageHover.Draw(spriteBatch, gameTime);
            else
                classSelectMage.Draw(spriteBatch, gameTime);
            if (classSelectRogue.Contains(mousePoint))
                classSelectRogueHover.Draw(spriteBatch, gameTime);
            else
                classSelectRogue.Draw(spriteBatch, gameTime);
            if (classSelectWarrior.Contains(mousePoint))
                classSelectWarriorHover.Draw(spriteBatch, gameTime);
            else
                classSelectWarrior.Draw(spriteBatch, gameTime);
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
                case GameState.StoryText:
                    DrawStoryText(gameTime);
                    break;
                case GameState.CharacterCreate:
                    DrawCharacterCreate(gameTime);
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

        public bool Contains(Point pos)
        {
            var rect = Texture.Bounds;
            rect.X = (int)Position.X;
            rect.Y = (int)Position.Y;
            return rect.Contains(pos);
        }

        public bool Contains(Vector2 pos)
        {
            var rect = Texture.Bounds;
            rect.X = (int)Position.X;
            rect.Y = (int)Position.Y;
            return rect.Contains(pos);
        }
    }
}
