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
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

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
        int damagePlayer = 0;
        int damageEnemy = 0;
        bool dispPlayerDamage = true;
        public static bool calledBattleFinish = false;
        string droppedItem = "";
        double receivedMoney = 0;
        #endregion
        #region factionAssets
        Button wildButton;
        Button contButton;
        Button playerButton;
        Button shopButton;
        #endregion
        #region levelUpAssets
        int GainedHealth = 0;
        int GainedAttack = 0;
        int GainedMAtk = 0;
        int GainedDefense = 0;
        int GainedMDef = 0;
        int GainedIntelligence = 0;
        int GainedLuck = 0;
        int GainedEvasion = 0;
        int GainedSpeed = 0;
        #endregion
        public enum GameState
        {
            MainMenu,
            StartPage,
            InitWorldPage,
            BattlePage,
            StoryText,
            CharacterCreate,
            InFaction,
            PlayerMenu
        }
        public static GameState _state;
        public static GameState State
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
            contButton = new Button(Content.Load<Texture2D>("DefaultButton"), Content.Load<Texture2D>("DefaultButtonHover"), "Continue", storyFont, new Vector2(middle, 150));
            wildButton = new Button(Content.Load<Texture2D>("DefaultButton"), Content.Load<Texture2D>("DefaultButtonHover"), "Wild", storyFont, new Vector2(middle, 225));
            playerButton = new Button(Content.Load<Texture2D>("DefaultButton"), Content.Load<Texture2D>("DefaultButtonHover"), "Player", storyFont, new Vector2(middle, 300));
            shopButton = new Button(Content.Load<Texture2D>("DefaultButton"), Content.Load<Texture2D>("DefaultButtonHover"), "Shop", storyFont, new Vector2(middle, 375));

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
                if (GlobalValues.saveEnabled)
                {
                    Battle.SetVals(GlobalValues.battleJson);
                    GlobalValues.GetVals();
                    GlobalValues.save.SaveGame();
                }
            }
            if (GlobalValues.free && State != GameState.BattlePage)
                State = GameState.InFaction;
            //Debug.WriteLine(GlobalValues.storyState);
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
                case GameState.InFaction:
                    UpdateInFaction(gameTime);
                    break;
                case GameState.PlayerMenu:
                    UpdatePlayerMenu(gameTime);
                    break;
                default:
                    break;
            }
        }

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
                case GameState.InFaction:
                    DrawInFaction(gameTime);
                    break;
                case GameState.PlayerMenu:
                    DrawPlayerMenu(gameTime);
                    break;
                default:
                    break;
            }
        }

        void UpdatePlayerMenu(GameTime gameTime)
        {

        }

        void DrawPlayerMenu(GameTime gameTime)
        {

        }

        void UpdateInFaction(GameTime gameTime)
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
            {
                switch (GlobalValues.inFactionState)
                {
                    case "start":
                        if (contButton.Contains(mousePoint))
                        {
                            Story.Progress();
                        }
                        if (wildButton.Contains(mousePoint))
                        {
                            int chanceTotal = 0;
                            int num = 0;
                            List<int> minMax = new List<int>();
                            JArray enemies = JArray.Parse(File.ReadAllText("Dependencies\\enemy.json"));
                            foreach (JObject enemy in enemies.ToArray())
                            {
                                if (!enemy["Id"].Value<string>().Contains("Rngen"))
                                    enemies.Remove(enemy);
                                else
                                {
                                    int start = 1;
                                    string[] strArr = enemy["Id"].Value<string>().Split(' ');
                                    if (strArr[1] == "MiniBoss")
                                        start++;
                                    for (int i = start; i < strArr.Length; i++)
                                    {
                                        if (int.Parse(strArr[i]) == GlobalValues.storyState)
                                            break;
                                        else if (i == strArr.Length - 1)
                                        {
                                            Debug.WriteLine(GlobalValues.storyState);
                                            Debug.WriteLine((string)enemy["Id"]);
                                            enemies.Remove(enemy);
                                        }
                                    }
                                }
                            }
                            foreach (JObject enemy in enemies.ToArray())
                                chanceTotal += (int)enemy["EnRate"];
                            num = HelperClasses.RandomNumber(0, chanceTotal);
                            minMax.Add(0);
                            for (int i = 1, j = 0; j < enemies.Count; i++)
                            {
                                if (i % 2 == 1)
                                {
                                    minMax.Add((int)enemies[j]["EnRate"] + minMax[i - 1] - 1);
                                    j++;
                                }
                                else
                                    minMax.Add(minMax[i - 1] + 1);
                            }
                            Debug.WriteLine(enemies.Count);
                            for (int i = 0; i < minMax.Count - 1; i++)
                            {
                                Debug.WriteLine(num);
                                Debug.WriteLine(minMax[i]);
                                Debug.WriteLine(minMax[i + 1]);
                                if (num >= minMax[i] && num <= minMax[i + 1])
                                {
                                    int index = 0;
                                    JArray tempEnemies = JArray.Parse(File.ReadAllText("Dependencies\\enemy.json"));
                                    for (int j = 0; j < tempEnemies.Count; j++)
                                    {

                                        if (tempEnemies[j] == enemies[i / 2])
                                            index = j;
                                    }
                                    Battle.enemy = new Enemy(index);
                                    Debug.WriteLine(Battle.enemy.Name);
                                }
                            }
                            Battle.player = GamePlay.player;
                            Battle.playerHP = GamePlay.player.Health;
                            Battle.enemyHP = Battle.enemy.Health;
                            Debug.WriteLine(Battle.enemyHP);
                            State = GameState.BattlePage;
                            break;
                        }
                        if (shopButton.Contains(mousePoint))
                        {
                            GlobalValues.inFactionState = "shop";
                        }
                        if (playerButton.Contains(mousePoint))
                        {
                            State = GameState.PlayerMenu;
                        }
                        break;
                    case "shop":

                        break;
                    default:
                        break;
                }
            }
        }

        void DrawInFaction(GameTime gameTime)
        {
            spriteBatch.Begin();
            Faction fac = GlobalValues.locationFaction;
            spriteBatch.DrawString(storyFont, "Faction: " + fac.Name, new Vector2(340, 25), Color.Black);
            switch (GlobalValues.inFactionState)
            {
                case "start":
                    contButton.Draw(spriteBatch, gameTime);
                    wildButton.Draw(spriteBatch, gameTime);
                    shopButton.Draw(spriteBatch, gameTime);
                    playerButton.Draw(spriteBatch, gameTime);
                    break;
                default:
                    break;
            }
            spriteBatch.End();
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
                    GlobalValues.locationFaction = Story.enemyFaction;
                    GlobalValues.saveEnabled = true;
                }

                if (classSelectWarrior.Contains(mousePoint))
                {
                    GamePlay.player = new Player(1, 2);
                    State = GameState.StoryText;
                    GlobalValues.locationFaction = Story.enemyFaction;
                    GlobalValues.saveEnabled = true;
                }

                if (classSelectRogue.Contains(mousePoint))
                {
                    GamePlay.player = new Player(1, 3);
                    State = GameState.StoryText;
                    GlobalValues.locationFaction = Story.enemyFaction;
                    GlobalValues.saveEnabled = true;
                }

            }
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

        void DrawMainMenu(GameTime gameTime)
        {
            spriteBatch.Begin();
            mainMenuButton.Draw(spriteBatch, gameTime);
            loadGame.Draw(spriteBatch, gameTime);
            spriteBatch.End();
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
                                damageEnemy = Battle.RegularAttack();
                            }
                            if (secondSpecial.Contains(mousePoint))
                            {
                                damagePlayer = Battle.HandleSpecial(GamePlay.player.Special[1]);
                                GlobalValues.battleState = "damageDealt";
                                damageEnemy = Battle.RegularAttack();

                            }
                            if (thirdSpecial.Contains(mousePoint))
                            {
                                damagePlayer = Battle.HandleSpecial(GamePlay.player.Special[2]);
                                GlobalValues.battleState = "damageDealt";
                                damageEnemy = Battle.RegularAttack();
                            }
                            if (fourthSpecial.Contains(mousePoint))
                            {
                                damagePlayer = Battle.HandleSpecial(GamePlay.player.Special[3]);
                                GlobalValues.battleState = "damageDealt";
                                damageEnemy = Battle.RegularAttack();
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
                        {
                            dispPlayerDamage = true;
                            GlobalValues.battleState = "battle";
                            switch (Battle.player.Class)
                            {
                                case "Warrior":
                                    Sword sword = (Sword)GamePlay.player.Equip[0];
                                    foreach (string str in sword.attr.ToArray())
                                    {
                                        if (str == @"Night's Bane")
                                            if (GamePlay.player.Health > Battle.playerHP)
                                                Battle.playerHP = 0;
                                        if (str == "Bloodthirsty")
                                        {
                                            Battle.player.Defense -= 1;
                                            Battle.enemy.Defense -= 5;
                                            if (Battle.player.Defense <= 0)
                                                Battle.player.Defense = 1;
                                            if (Battle.enemy.Defense <= 0)
                                                Battle.enemy.Defense = 1;
                                        }
                                        if (str == "Frozen")
                                        {
                                            if (Battle.round <= 3)
                                                Battle.player.Health += damageEnemy;
                                        }
                                    }
                                    break;
                                case "Mage":
                                    Staff staff = (Staff)GamePlay.player.Equip[0];
                                    foreach (string str in staff.attr.ToArray())
                                    {
                                        if (str == "Commander")
                                            Battle.player.Attack++;
                                        if (str == "Infect")
                                            if (Battle.player.Defense > Battle.enemy.Defense)
                                                Battle.enemyHP -= damageEnemy;
                                        if (str == "Frozen")
                                        {
                                            if (Battle.round <= 2)
                                                Battle.player.Health += damageEnemy;
                                        }
                                        if (str == "Conflux")
                                        {
                                            Battle.enemy.Attack -= 1;
                                            Battle.enemy.Defense -= 10;
                                            if (Battle.enemy.Attack <= 0)
                                                Battle.enemy.Attack = 1;
                                            if (Battle.enemy.Defense <= 0)
                                                Battle.enemy.Defense = 1;
                                        }
                                    }
                                    break;
                                case "Rogue":
                                    Knife knife = (Knife)GamePlay.player.Equip[0];
                                    foreach (string str in knife.attr.ToArray())
                                    {
                                        if (str == "Bloodthirsty")
                                        {
                                            Battle.player.Defense -= 1;
                                            Battle.enemy.Defense -= 5;
                                            if (Battle.player.Defense <= 0)
                                                Battle.player.Defense = 1;
                                            if (Battle.enemy.Defense <= 0)
                                                Battle.enemy.Defense = 1;
                                        }
                                        if (str == "Infect")
                                            if (Battle.player.Defense > Battle.enemy.Defense)
                                                Battle.enemyHP -= damageEnemy;
                                        if (str == "Frozen")
                                        {
                                            if (Battle.round <= 2)
                                                Battle.player.Health += damageEnemy;
                                        }
                                    }
                                    break;
                                default:

                                    break;
                            }
                            Battle.round++;
                        }
                        else
                            dispPlayerDamage = false;
                    }
                    break;
                case "winner":
                    if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                    {
                        if (Battle.outcome == 1 || Battle.outcome == -1)
                        {
                            GamePlay.player.Money += Math.Round(Math.Pow(Battle.enemy.Level, 1.2), 2);
                            receivedMoney = GamePlay.player.Money + Math.Round(Math.Pow(Battle.enemy.Level, 1.2), 2);
                            if (GamePlay.player.Exp >= GamePlay.player.NextLevel())
                            {
                                GlobalValues.battleState = "levelup";
                            }
                            else if (!GlobalValues.free)
                            {
                                Battle.ResetVals();
                                Story.Progress();
                                State = GameState.StoryText;
                            }
                            else if (GlobalValues.free)
                            {
                                Battle.ResetVals();
                                State = GameState.InFaction;
                            }

                        }
                        else if (Battle.outcome == 0)
                        {
                            Battle.ResetVals();
                            GlobalValues.storyIndex = 0;
                            State = GameState.StoryText;
                            receivedMoney = 0;
                        }
                    }
                    break;
                case "levelup":
                    if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                    {
                        receivedMoney = 0;
                        Battle.ResetVals();
                        if (GlobalValues.free)
                        {
                            State = GameState.InFaction;
                        }
                        else
                        {
                            Story.Progress();
                            State = GameState.StoryText;
                        }
                    }
                    break;
                default:
                    break;
            }
            if (GlobalValues.battleState != "prologue" && Battle.round != -1 && !calledBattleFinish)
            {
                if (Battle.playerHP <= 0)
                {
                    Battle.BattleFinish(false);
                    calledBattleFinish = true;
                }
                else if (Battle.enemyHP <= 0)
                {
                    Battle.BattleFinish(true);
                    calledBattleFinish = true;
                }

            }
        }

        void DrawBattlePage(GameTime gameTime)
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            graphics.GraphicsDevice.Clear(Color.Red);


            spriteBatch.Begin();
            switch (GlobalValues.battleID)
            {
                case "firstBattle":
                    if (Battle.round == -1)
                    {
                        Battle.fightText = "You dare fight me fool?!";
                        Battle.enemy = new Enemy("Unknown StartGame", "?");
                        Battle.player = new Player(GamePlay.player);
                        Battle.enemyHP = Battle.enemy.Health;
                        Battle.playerHP = Battle.player.Health;
                        Battle.turn = Battle.player.Speed > Battle.enemy.Speed;
                        //attribute handling
                        switch (Battle.player.Class)
                        {
                            case "Warrior":
                                Sword sword = (Sword)GamePlay.player.Equip[0];
                                foreach (string str in sword.attr.ToArray())
                                {
                                    if (str == "Curse")
                                        Battle.turn = false;
                                }
                                break;
                            case "Mage":
                                Staff staff = (Staff)GamePlay.player.Equip[0];
                                foreach (string str in staff.attr.ToArray())
                                {
                                    if (str == "Curse")
                                        Battle.turn = false;
                                }
                                break;
                            case "Rogue":
                                Knife knife = (Knife)GamePlay.player.Equip[0];
                                foreach (string str in knife.attr.ToArray())
                                {
                                    if (str == "Curse")
                                        Battle.turn = false;
                                }
                                break;
                            default:

                                break;
                        }

                        if (!Battle.turn)
                        {
                            damageEnemy = Battle.RegularAttack();
                            damagePlayer = Battle.RegularAttack();
                            GlobalValues.battleState = "damageDealt";
                        }
                        Battle.round++;
                        Battle.SetVals(GlobalValues.battleJson);
                    }
                    else
                        GlobalValues.battleID = "";
                    break;
                default:
                    if (Battle.round == -1)
                    {
                        Battle.enemyHP = Battle.enemy.Health;
                        Battle.playerHP = Battle.player.Health;
                        Battle.turn = Battle.player.Speed > Battle.enemy.Speed;
                        switch (Battle.player.Class)
                        {
                            case "Warrior":
                                Sword sword = (Sword)GamePlay.player.Equip[0];
                                foreach (string str in sword.attr.ToArray())
                                {
                                    if (str == "Curse")
                                        Battle.turn = false;
                                }
                                break;
                            case "Mage":
                                Staff staff = (Staff)GamePlay.player.Equip[0];
                                foreach (string str in staff.attr.ToArray())
                                {
                                    if (str == "Curse")
                                        Battle.turn = false;
                                }
                                break;
                            case "Rogue":
                                Knife knife = (Knife)GamePlay.player.Equip[0];
                                foreach (string str in knife.attr.ToArray())
                                {
                                    if (str == "Curse")
                                        Battle.turn = false;
                                }
                                break;
                            default:
                                Battle.round++;
                                Battle.SetVals(GlobalValues.battleJson);
                                break;
                        }
                        if (!Battle.turn)
                        {
                            damageEnemy = Battle.RegularAttack();
                            damagePlayer = Battle.RegularAttack();
                            GlobalValues.battleState = "damageDealt";
                        }
                        Battle.round++;
                        Battle.SetVals(GlobalValues.battleJson);
                    }
                    break;
            }
            switch (GlobalValues.battleState)
            {
                case "prologue":
                    textBox.Draw(spriteBatch, gameTime);
                    spriteBatch.DrawString(storyFont, Battle.fightText, new Vector2(110, 319), Color.Black);
                    break;
                case "battle":
                    #region battleCase
                    spriteBatch.DrawString(storyFont, "Enemy HP: " + Battle.enemyHP + " / " + Battle.enemy.Health, new Vector2(92, 59), Color.Black);
                    spriteBatch.DrawString(storyFont, "Player HP: " + Battle.playerHP + " / " + Battle.player.Health, new Vector2(673, 59), Color.Black);
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
                    if (clickedRun)
                    {
                        graphics.GraphicsDevice.Clear(Color.Blue);
                        textBox.Draw(spriteBatch, gameTime);
                        if (!canRun)
                        {
                            spriteBatch.DrawString(storyFont, "You can't run.", new Vector2(textBox.Position.X + 5, textBox.Position.Y + 5), Color.Black);
                        }
                    }
                    #endregion
                    break;
                case "damageDealt":
                    graphics.GraphicsDevice.Clear(Color.Red);
                    textBox.Draw(spriteBatch, gameTime);
                    if (dispPlayerDamage)
                    {
                        spriteBatch.DrawString(storyFont, "You dealt " + damagePlayer + " damage to " + Battle.enemy.Name + ".", new Vector2(110, 319), Color.Black);
                    }
                    else
                    {
                        if (damageEnemy != 0)
                            spriteBatch.DrawString(storyFont, Battle.enemy.Name + " dealt " + damageEnemy + " to you.", new Vector2(110, 319), Color.Black);
                        else
                            spriteBatch.DrawString(storyFont, "You dodged the attack.", new Vector2(110, 319), Color.Black);
                    }
                    spriteBatch.DrawString(storyFont, "Enemy HP: " + Battle.enemyHP + " / " + Battle.enemy.Health, new Vector2(92, 59), Color.Black);
                    spriteBatch.DrawString(storyFont, "Player HP: " + Battle.playerHP + " / " + Battle.player.Health, new Vector2(673, 59), Color.Black);
                    break;
                case "winner":
                    if (Battle.outcome == 1)
                    {
                        GamePlay.player.Exp += HelperClasses.RandomNumber(1, HelperClasses.RandomNumber(1, HelperClasses.RandomNumber(0, Battle.enemy.Level)));
                        Battle.enemy = new Enemy();
                        Battle.outcome = -1;
                        droppedItem = GamePlay.player.ItemDrop();
                    }
                    else if (Battle.outcome == 0)
                    {
                        textBox.Draw(spriteBatch, gameTime);
                        spriteBatch.DrawString(storyFont, "You have lost...", new Vector2(110, 319), Color.Black);
                    }
                    if (Battle.outcome == -1)
                    {
                        GraphicsDevice.Clear(Color.LightGreen);
                        textBox.Draw(spriteBatch, gameTime);
                        spriteBatch.DrawString(storyFont, "You won! Exp: " + GamePlay.player.Exp + " / " + GamePlay.player.NextLevel(), new Vector2(110, 319), Color.Black);
                        spriteBatch.DrawString(storyFont, "You have obtained: " + droppedItem, new Vector2(110, 343), Color.Black);
                        spriteBatch.DrawString(storyFont, "You have received " + receivedMoney + " gold.", new Vector2(110, 367), Color.Black);
                    }
                    break;
                case "levelup":
                    if (GamePlay.player.Exp >= GamePlay.player.NextLevel())
                    {
                        GamePlay.player.Exp -= GamePlay.player.NextLevel();
                        GamePlay.player.Level++;
                        int level = GamePlay.player.Level;
                        switch (GamePlay.player.Class)
                        {
                            case "Warrior":
                                GainedHealth = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 1.5));
                                GainedAttack = HelperClasses.RandomNumber(0, level * 3);
                                GainedDefense = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 2.5));
                                GainedIntelligence = HelperClasses.RandomNumber(0, level);
                                GainedLuck = HelperClasses.RandomNumber(0, level);
                                GainedEvasion = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 0.75));
                                GainedSpeed = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 0.75));
                                break;
                            case "Rogue":
                                GainedHealth = HelperClasses.RandomNumber(0, level);
                                GainedAttack = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 1.25));
                                GainedDefense = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 0.75));
                                GainedIntelligence = HelperClasses.RandomNumber(0, level);
                                GainedLuck = HelperClasses.RandomNumber(0, level);
                                GainedEvasion = HelperClasses.RandomNumber(0, level * 2);
                                GainedSpeed = HelperClasses.RandomNumber(0, level * 3);
                                break;
                            case "Mage":
                                GainedHealth = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 0.75));
                                GainedAttack = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 0.5));
                                GainedMAtk = HelperClasses.RandomNumber(0, level * 3);
                                GainedDefense = HelperClasses.RandomNumber(0, level);
                                GainedMDef = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 2.5));
                                GainedIntelligence = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 2.5));
                                GainedLuck = HelperClasses.RandomNumber(0, level);
                                GainedEvasion = HelperClasses.RandomNumber(0, Convert.ToInt32(level * 0.75));
                                GainedSpeed = HelperClasses.RandomNumber(0, level);
                                break;
                            default:
                                break;
                        }
                        GamePlay.player.Health += GainedHealth;
                        GamePlay.player.Attack += GainedAttack;
                        GamePlay.player.Defense += GainedDefense;
                        GamePlay.player.Intelligence += GainedIntelligence;
                        GamePlay.player.Luck += GainedLuck;
                        GamePlay.player.Evasion += GainedEvasion;
                        GamePlay.player.Speed += GainedSpeed;
                    }
                    GraphicsDevice.Clear(Color.LightGreen);
                    spriteBatch.DrawString(storyFont, "Level Up! Level: " + GamePlay.player.Level, new Vector2(80, 40), Color.Black);
                    int increment = 1;
                    int baseY = 90;
                    spriteBatch.DrawString(storyFont, "HP: " + (GamePlay.player.Health - GainedHealth) + " + " + GainedHealth + " = " + GamePlay.player.Health, new Vector2(80, baseY + (increment * 30)), Color.Black);
                    increment++;
                    spriteBatch.DrawString(storyFont, "Attack: " + (GamePlay.player.Attack - GainedAttack) + " + " + GainedAttack + " = " + GamePlay.player.Attack, new Vector2(80, baseY + (increment * 30)), Color.Black);
                    increment++;
                    spriteBatch.DrawString(storyFont, "Magic Attack: " + (GamePlay.player.MAtk - GainedMAtk) + " + " + GainedMAtk + " = " + GamePlay.player.MAtk, new Vector2(80, baseY + (increment * 30)), Color.Black);
                    increment++;
                    spriteBatch.DrawString(storyFont, "Defense: " + (GamePlay.player.Defense - GainedDefense) + " + " + GainedDefense + " = " + GamePlay.player.Defense, new Vector2(80, baseY + (increment * 30)), Color.Black);
                    increment++;
                    spriteBatch.DrawString(storyFont, "Magic Defense: " + (GamePlay.player.MDef - GainedMDef) + " + " + GainedMDef + " = " + GamePlay.player.MDef, new Vector2(80, baseY + (increment * 30)), Color.Black);
                    increment++;
                    spriteBatch.DrawString(storyFont, "Intelligence: " + (GamePlay.player.Intelligence - GainedIntelligence) + " + " + GainedIntelligence + " = " + GamePlay.player.Intelligence, new Vector2(80, baseY + (increment * 30)), Color.Black);
                    increment++;
                    spriteBatch.DrawString(storyFont, "Luck: " + (GamePlay.player.Luck - GainedLuck) + " + " + GainedLuck + " = " + GamePlay.player.Luck, new Vector2(80, baseY + (increment * 30)), Color.Black);
                    increment++;
                    spriteBatch.DrawString(storyFont, "Evasion: " + (GamePlay.player.Evasion - GainedEvasion) + " + " + GainedEvasion + " = " + GamePlay.player.Evasion, new Vector2(80, baseY + (increment * 30)), Color.Black);
                    increment++;
                    spriteBatch.DrawString(storyFont, "Speed: " + (GamePlay.player.Speed - GainedSpeed) + " + " + GainedSpeed + " = " + GamePlay.player.Speed, new Vector2(80, baseY + (increment * 30)), Color.Black);
                    break;
                default:
                    break;
            }

            spriteBatch.End();
        }

        void UpdateStoryText(GameTime gameTime)
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
            {
                Story.Progress();
            }
        }

        void DrawStoryText(GameTime gameTime)
        {
            if (GlobalValues.free)
            {
                State = GameState.InFaction;
                return;
            }
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
            if (text.StartsWith(":free="))
            {
                if (text[6] == 'f')
                    GlobalValues.free = false;
                else if (text[6] == 't')
                    GlobalValues.free = true;
                State = GameState.InFaction;
                Story.Progress();
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



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>


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

    public class Button
    {
        Sprite img = new Sprite();
        Sprite hoverImg = new Sprite();

        public Sprite Img { get => img; set => img = value; }
        public Sprite HoverImg { get => hoverImg; set => hoverImg = value; }
        public string Text { get; set; }
        public SpriteFont Font { get; set; }

        public Button(Sprite image, Sprite hoverImage, string text, SpriteFont spriteFont)
        {
            Img = image;
            HoverImg = hoverImage;
            Text = text;
            Font = spriteFont;
        }

        public Button(Texture2D image, Texture2D hoverImage, string text, SpriteFont spriteFont, Vector2 position)
        {
            Img.Texture = image;
            HoverImg.Texture = hoverImage;
            Img.Position = position;
            HoverImg.Position = position;
            Text = text;
            Font = spriteFont;
        }

        public bool Contains(Point pos)
        {
            var rect = Img.Texture.Bounds;
            rect.X = (int)Img.Position.X;
            rect.Y = (int)Img.Position.Y;
            return rect.Contains(pos);
        }

        public bool Contains(Vector2 pos)
        {
            var rect = Img.Texture.Bounds;
            rect.X = (int)Img.Position.X;
            rect.Y = (int)Img.Position.Y;
            return rect.Contains(pos);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (Img.Contains(mouseState.Position))
                spriteBatch.Draw(HoverImg.Texture, Img.Position, Color.White);
            else
                spriteBatch.Draw(Img.Texture, Img.Position, Color.White);
            spriteBatch.DrawString(Font, Text, new Vector2(Img.Position.X + 7, Img.Position.Y + 7), Color.Black);
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

        public Sprite()
        {
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