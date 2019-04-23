using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
namespace RPGv2
{
    internal class HelperClasses
    {
        public static GlobalValues gv = new GlobalValues();
        [STAThread]
        public static void MainProgram()
        {
            StateManager sm = new StateManager();
            switch (sm.GetState())
            {
                case "start":
                    Start();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("A state error has occured. State: {0}", sm.GetState());
                    break;
            }
        }

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();



        public static int RandomNumber(int min, int max)
        {
            max = max <= min ? min + 1 : max;
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        public static bool Start()
        {
            //switch (GlobalValues.Inp)
            //{
            //    case 0:
            //        Game.StartGame();
            //        GlobalValues.Inp = -1;
            //        return false;
            //    case 1:
            //        /*
            //        string[] jsonFiles = Directory.GetFiles("Dependencies");
            //        for (int i = 0; i < jsonFiles.Length; i++)
            //            jsonFiles[i] = jsonFiles[i].Remove(0, 13);
            //        inp = MultipleChoice(false, jsonFiles);
            //        JArray arr = JArray.Parse(File.ReadAllText(string.Format(@"Dependencies\{0}", jsonFiles[inp])));
            //        int inpIndex = inp;
            //        string path = string.Format(@"Dependencies\{0}", jsonFiles[inp]);
            //        inp = MultipleChoice(false, "Add", "Modify");
            //        List<string> options = new List<string>();
            //        JObject o;
            //        string newVal;
            //        switch (inp)
            //        {
            //            case 0:
            //                options.Clear();
            //                o = JObject.Parse(arr[0].ToString());
            //                foreach (JProperty jp in o.Properties())
            //                    options.Add(jp.Name);
            //                for (int i = 0; i < options.Count; i++)
            //                {
            //                    Console.Write("Value of {0}: ", options[i]);
            //                    newVal = Console.ReadLine();
            //                    if (o[options[i]].Type == JTokenType.String)
            //                        o[options[i]] = newVal;
            //                    if (o[options[i]].Type == JTokenType.Integer)
            //                        o[options[i]] = int.Parse(newVal);
            //                    if (o[options[i]].Type == JTokenType.Float)
            //                        o[options[i]] = float.Parse(newVal);
            //                    if (o[options[i]].Type == JTokenType.Boolean)
            //                        o[options[i]] = bool.Parse(newVal);
            //                }
            //                arr.Add(o);
            //                File.WriteAllText(path, arr.ToString());
            //                break;
            //            case 1:
            //                foreach (JObject obj in arr)
            //                    options.Add((string)obj["Name"]);
            //                inp = MultipleChoice(false, options.ToArray());
            //                options.Clear();
            //                o = JObject.Parse(arr[inp].ToString());
            //                foreach (JProperty jp in o.Properties())
            //                    options.Add(jp.Name);
            //                List<string> optionsTemp = new List<string>(options);
            //                for (int i = 0; i < optionsTemp.Count; i++)
            //                    optionsTemp[i] = string.Format("{0} ({1})", optionsTemp[i], o[optionsTemp[i]]);
            //                inp = MultipleChoice(false, optionsTemp.ToArray());
            //                Console.Write("Enter new value: ");
            //                newVal = Console.ReadLine();
            //                if (o[options[inp]].Type == JTokenType.String)
            //                    o[options[inp]] = newVal;
            //                if (o[options[inp]].Type == JTokenType.Integer)
            //                    o[options[inp]] = int.Parse(newVal);
            //                if (o[options[inp]].Type == JTokenType.Float)
            //                    o[options[inp]] = float.Parse(newVal);
            //                arr[inpIndex] = o;
            //                File.WriteAllText(path, arr.ToString());
            //                break;
            //            default:
            //                break;
            //        }
            //        return false;
            //        */
            //        GlobalValues.Inp = -1;
            //        break;
            //    case 2:
            //        GlobalValues.Inp = -1;
            //        return true;
            //    default:
            //        break;
            //}
            //GlobalValues.Inp = -1;
            return false;
        }

        public static int MultipleChoice(bool canCancel, params string[] options)
        {
            const int startX = 0;
            const int startY = 0;
            const int optionsPerLine = 1;
            const int spacingPerLine = 14;

            int currentSelection = 0;

            ConsoleKey key;

            Console.CursorVisible = false;
            do
            {
                Console.Clear();

                for (int i = 0; i < options.Length; i++)
                {
                    Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i / optionsPerLine);

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write(options[i]);

                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        {
                            if (currentSelection % optionsPerLine > 0)
                                currentSelection--;
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (currentSelection % optionsPerLine < optionsPerLine - 1)
                                currentSelection++;
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (currentSelection >= optionsPerLine)
                                currentSelection -= optionsPerLine;
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (currentSelection + optionsPerLine < options.Length)
                                currentSelection += optionsPerLine;
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            if (canCancel)
                                return -1;
                            break;
                        }
                }
            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;

            Console.Clear();
            return currentSelection;
        }
    }



    public class Save
    {
        public History hist = new History();
        public Player player = new Player();

        public Save()
        {

        }

        public void SaveGame()
        {
            Debug.WriteLine("Saving game...");
            player = GamePlay.player;
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(@"Dependencies\save.json"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    string json = JsonConvert.SerializeObject(new { player, hist.Factions, hist.Races, GlobalValues.jsonVals, Story.enemyFaction, GlobalValues.battleJson }, Formatting.Indented);
                    sw.Write(json);
                }
            }
        }

        public void LoadGame()
        {
            JObject obj = JObject.Parse(File.ReadAllText(@"Dependencies\save.json"));
            player = JsonConvert.DeserializeObject<Player>(obj["player"].ToString());
            GamePlay.player = player;
            hist.Factions = JsonConvert.DeserializeObject<List<Faction>>(obj["Factions"].ToString());
            hist.Races = JsonConvert.DeserializeObject<List<Race>>(obj["Races"].ToString());
            GlobalValues.jsonVals = JsonConvert.DeserializeObject<JsonValues>(obj["jsonVals"].ToString());
            Story.enemyFaction = JsonConvert.DeserializeObject<Faction>(obj["enemyFaction"].ToString());
            GlobalValues.battleJson = JsonConvert.DeserializeObject<BattleJson>(obj["battleJson"].ToString());
            GlobalValues.SetVals(GlobalValues.jsonVals);
            Battle.GetVals(GlobalValues.battleJson);
            GlobalValues.battleState = "battle";
        }
    }

    public class SceneText
    {
        public static List<bool> wrapText = new List<bool>();
        public static List<string> strArr;

        public static void GetText()
        {
            strArr = Story.GetScene(GlobalValues.storyState);
            foreach (string str in strArr.ToArray())
                wrapText.Add(str.Length >= 50);
        }
    }

    public class Story
    {
        public static int MaxIndex { get; set; }
        public static Faction enemyFaction;

        public static List<string> GetScene(int index)
        {
            List<string> strArr;
            strArr = File.ReadAllLines("Dependencies/Story.txt").ToList();
            bool found = false;
            List<string> temp = new List<string>();
            foreach (string str in strArr.ToArray())
            {
                if (str.StartsWith("\"") || string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                    strArr.Remove(str);
            }
            foreach (string str in strArr.ToArray())
            {
                if (str.StartsWith(string.Format("[{0}", index)))
                {
                    if (!found)
                        found = true;
                    else
                        break;
                }
                if (found && str[0] != '[')
                    temp.Add(str);
            }
            if (strArr.Count - 1 == GlobalValues.storyIndex)
            {
                GlobalValues.storyIndex = 0;
                GlobalValues.storyState++;
                foreach (string str in strArr.ToArray())
                {
                    if (str.StartsWith(string.Format("[{0}", index)))
                    {
                        if (!found)
                            found = true;
                        else
                            break;
                    }
                    if (found && str[0] != '[')
                        temp.Add(str);
                }
            }
            strArr = temp;
            string obj = "";
            bool done = false;
            string tempStr = "";
            string replacement;
            foreach (string str in strArr.ToArray())
            {
                while (!done)
                {
                    if (str.Contains("{"))
                    {
                        tempStr = str.Substring(str.IndexOf("{") + 1);
                        foreach (char c in tempStr)
                        {
                            if (c != '}')
                                obj += c;
                            else
                                break;
                        }
                        switch (obj)
                        {
                            case "WarFactionName":
                                replacement = enemyFaction.Name;
                                strArr[strArr.IndexOf(str)] = str.Replace("{WarFactionName}", replacement);
                                done = true;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        done = true;
                    }
                }
                done = false;
            }
            return strArr;
        }

        public static void Progress()
        {
            int length = 0;
            List<string> strArray = SceneText.strArr;
            length = strArray.Count - 1;
            if (GlobalValues.storyIndex == length)
            {
                GlobalValues.storyIndex = 0;
                GlobalValues.storyState++;
            }
            else
            {
                GlobalValues.storyIndex++;
            }
        }

        public static string Talker(string s)
        {
            if (/*name present*/true)
            {
                return "name";
            }
            return null;
        }
    }

    public class BattleJson
    {
        public Enemy enemy = new Enemy();
        public Player player = GamePlay.player;
        public int playerHP = 0;
        public int enemyHP = 0;
        public bool turn = false;
        public int round = -1;
        public int outcome = -1;
        public string fightText = "";

        public BattleJson()
        {

        }

        [JsonConstructor]
        public BattleJson(Enemy jEnemy, Player jPlayer, int jPlayerHP, int jEnemyHP, bool jTurn, int jRound, int jOutcome, string jFightText)
        {
            enemy = jEnemy;
            player = jPlayer;
            playerHP = jPlayerHP;
            enemyHP = jEnemyHP;
            turn = jTurn;
            round = jRound;
            outcome = jOutcome;
            fightText = jFightText;
        }

    }

    public class Battle
    {
        public static Enemy enemy = new Enemy();
        public static Player player = GamePlay.player;
        public static int playerHP = 0;
        public static int enemyHP = 0;
        public static bool turn = false;
        public static int round = -1;
        public static int outcome = -1;
        public static string fightText = "";

        /*
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
             name = "Gloss";
             break;
        */

        public static void GetVals(BattleJson battleJson)
        {
            enemy = battleJson.enemy;
            player = battleJson.player;
            playerHP = battleJson.playerHP;
            enemyHP = battleJson.enemyHP;
            turn = battleJson.turn;
            round = battleJson.round;
            outcome = battleJson.outcome;
            fightText = battleJson.fightText;
        }

        public static void SetVals(BattleJson battleJson)
        {
            battleJson.enemy = enemy;
            battleJson.player = player;
            battleJson.playerHP = playerHP;
            battleJson.enemyHP = enemyHP;
            battleJson.turn = turn;
            battleJson.round = round;
            battleJson.outcome = outcome;
            battleJson.fightText = fightText;
        }

        public static void HandleAttr()
        {

        }

        public static int HandleSpecial(int sp)
        {
            int damage = -1;
            switch (sp)
            {
                case 0:
                    break;
                case 1:
                    //Fire ball
                    damage = Convert.ToInt32(HelperClasses.RandomNumber(1, Convert.ToInt32(player.MAtk * 1.5)) - HelperClasses.RandomNumber(0, enemy.Defense));
                    if (damage <= 0)
                        damage = 1;
                    enemyHP -= damage;
                    break;
                case 2:
                    //Hide
                    player.Evasion *= 2.3;
                    break;
                case 3:
                    //Hard hit
                    damage = Convert.ToInt32(HelperClasses.RandomNumber(1, player.Attack) - HelperClasses.RandomNumber(0, enemy.Defense));
                    if (damage <= 0)
                        damage = 1;
                    enemyHP -= Convert.ToInt32(damage * 1.5);
                    break;
                case 4:
                    //Gloss
                    player.Speed = Convert.ToInt32(player.Speed * 1.2);
                    break;
                default:
                    break;
            }
            
            SetVals(GlobalValues.battleJson);
            turn = false;
            return damage;
        }

        public static int RegularAttack()
        {
            int damage = 0;
            if (turn)
            {
                damage = Convert.ToInt32(HelperClasses.RandomNumber(1, player.Attack) - HelperClasses.RandomNumber(0, enemy.Defense));
                if (damage <= 0)
                    damage = 1;
                enemyHP -= damage;
            }
            else
            {
                damage = Convert.ToInt32(HelperClasses.RandomNumber(1, enemy.Attack) - HelperClasses.RandomNumber(0, player.Defense));
                if (damage <= 0)
                    damage = 1;
                if (DodgeChance(player.Evasion, enemy.Speed) > (HelperClasses.RandomNumber(0, 100) / 100.0))
                    damage = 0;
                playerHP -= damage;
            }
            turn = !turn;
            SetVals(GlobalValues.battleJson);
            return damage;
        }

        public static double DodgeChance(double defenderEvasion, double attackerSpeed)
        {
            double x = defenderEvasion / attackerSpeed;
            double a = 10.0 / Math.Sqrt(111111.0);
            double b = 111071.0 / 40000.0;
            return a * Math.Sqrt(x + b);
        }

        public static void BattleFinish(bool winner)
        {
            if (winner)
                outcome = 1;
            else
                outcome = 0;
            GlobalValues.battleState = "winner";
            //SetVals(GlobalValues.battleJson);
        }
        public static void ResetVals()
        {
            enemy = new Enemy();
            player = GamePlay.player;
            playerHP = 0;
            enemyHP = 0;
            turn = false;
            round = -1;
            outcome = -1;
            fightText = "";
        }
    }

    public class Enemy
    {
        public int Health { get; set; }
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public int MDef { get; set; }
        public int Level { get; set; }


        public Enemy()
        {

        }

        public Enemy(string id)
        {
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\enemy.json"));
            JObject obj = new JObject();
            foreach (JObject jObj in array)
            {
                if ((string)jObj["Id"] == id)
                    obj = jObj;
            }
            Name = (string)obj["Name"];
            Attack = (int)obj["Attack"];
            Defense = (int)obj["Defense"];
            Speed = (int)obj["Speed"];
            Health = (int)obj["Health"];
            MDef = (int)obj["MDef"];
            Level = (int)obj["Level"];
        }
    }


    public class JsonValues
    {
        public int inp = -1;
        public string inpText = "";
        public string yearNum = "";
        public string facCreate = "";
        public string facDestroyed = "";
        public string eventName = "";
        public string battleID = "null";
        public string battleState = "prologue";
        public bool startGen = false;
        public bool done = false;
        public bool free = false;
        public string[] strArray;
        public int storyIndex = 0;
        public int storyState = 0;
        public Faction locationFaction = Story.enemyFaction;

        public JsonValues()
        {

        }

        [JsonConstructor]
        public JsonValues(int jInp, string jInpText, string jYearNum, string jFacCreate, string jFacDestroyed, string jEventName, string jBattleID,
            string jBattleState, bool jStartGen, bool jDone, string[] jStrArray, int jStoryIndex, int jStoryState, Faction faction)
        {
            inp = jInp;
            inpText = jInpText;
            yearNum = jYearNum;
            facCreate = jFacCreate;
            facDestroyed = jFacDestroyed;
            eventName = jEventName;
            battleID = jBattleID;
            battleState = jBattleState;
            startGen = jStartGen;
            done = jDone;
            strArray = jStrArray;
            storyIndex = jStoryIndex;
            storyState = jStoryState;
            locationFaction = faction;
        }
    }


    public class GlobalValues
    {
        public static int inp = -1;
        public static string inpText = "";
        public static string yearNum = "";
        public static string facCreate = "";
        public static string facDestroyed = "";
        public static string eventName = "";
        public static string battleID = "null";
        public static string battleState = "prologue";
        public static bool startGen = false;
        public static bool done = false;
        public static bool free = false;
        public static string[] strArray;
        public static int storyIndex = 0;
        public static int storyState = 0;
        public static Save save = new Save();
        public static JsonValues jsonVals = new JsonValues();
        public static BattleJson battleJson = new BattleJson();
        public static Faction locationFaction = Story.enemyFaction;

        public static void SetVals(JsonValues jVals)
        {
            inp = jVals.inp;
            inpText = jVals.inpText;
            yearNum = jVals.yearNum;
            facCreate = jVals.facCreate;
            facDestroyed = jVals.facDestroyed;
            eventName = jVals.eventName;
            battleID = jVals.battleID;
            battleState = jVals.battleState;
            startGen = jVals.startGen;
            done = jVals.done;
            strArray = jVals.strArray;
            storyIndex = jVals.storyIndex;
            storyState = jVals.storyState;
            locationFaction = jVals.locationFaction;
            free = jVals.free;
        }

        public static void GetVals()
        {
            jsonVals.inp = inp;
            jsonVals.inpText = inpText;
            jsonVals.yearNum = yearNum;
            jsonVals.facCreate = facCreate;
            jsonVals.facDestroyed = facDestroyed;
            jsonVals.eventName = eventName;
            jsonVals.battleID = battleID;
            jsonVals.battleState = battleState;
            jsonVals.startGen = startGen;
            jsonVals.done = done;
            jsonVals.strArray = strArray;
            jsonVals.storyIndex = storyIndex;
            jsonVals.storyState = storyState;
            jsonVals.locationFaction = locationFaction;
            jsonVals.free = free;
        }

        public static int Inp
        {
            get => inp;

            set => inp = value;
        }

    }

    public class Map
    {
        Image img;
        List<Tile> tiles = new List<Tile>();

        internal List<Tile> Tiles { get => tiles; set => tiles = value; }
        public Image Img { get => img; set => img = value; }

        public int GetLandCount()
        {
            int count = 0;
            foreach (Tile t in Tiles.ToArray())
                if (t.Type == "L")
                    count++;
            return count;
        }

        public Map()
        {
            string[] mapFiles = Directory.GetFiles("Dependencies\\Maps");
            string mapFile = mapFiles[new Random().Next(mapFiles.Length)];
            Img = Image.FromFile(mapFile);
        }

        public Map(int maxX, int maxY)
        {
            string[] mapFiles = Directory.GetFiles("Dependencies\\Maps");
            string mapFile = mapFiles[new Random().Next(mapFiles.Length)];
            Img = Image.FromFile(mapFile);
        }

        public Map(string s)
        {
            Img = Image.FromFile(s);
            Bitmap bmp = new Bitmap(Img);
            for (int y = 0; y <= Img.Height - 1; y++)
                for (int x = 0; x <= Img.Width - 1; x++)
                {
                    Color clr = bmp.GetPixel(x, y);
                    int g = clr.G;
                    int b = clr.B;
                    if (g > b)
                    {
                        Tiles.Add(new Tile(x, y, "L"));
                    }
                    else
                    {
                        Tiles.Add(new Tile(x, y, "W"));
                    }
                }
        }

        public List<Tile> GetTiles() => Tiles;

        public void OutputMap()
        {
            int currY = 0;
            Tile currTile;
            for (int i = 0; i < Tiles.Count; i++)
            {
                currTile = Tiles[i];
                Console.Write(currTile.Type);
                if (currTile.Y != currY && currTile.X == Img.Width - 1)
                {
                    Console.WriteLine();
                    currY = currTile.Y;
                }
            }
        }

        public Tile FindTile(int x, int y)
        {
            foreach (Tile t in Tiles)
            {
                if (t.X == x && t.Y == y)
                    return t;
            }
            return Tiles[0];
        }

        public void InitFaction(Faction fac)
        {
            Tile t;
            do
            {
                t = Tiles[HelperClasses.RandomNumber(0, Tiles.Count - 1)];
            } while (t.Occ != null || t.Type == "W");
            fac.Loc.X = t.X;
            fac.Loc.Y = t.Y;
            t.Type = "F";
            t.Occ = fac;
            Bitmap bmp = new Bitmap(Img);
            bmp.SetPixel(fac.Loc.X, fac.Loc.Y, Color.Red);
            Img = bmp;
        }
    }

    public class Tile
    {
        int x;
        int y;
        string type;
        Faction occ;

        public Tile()
        {

        }

        public Tile(int xCoord, int yCoord, string t)
        {
            X = xCoord;
            Y = yCoord;
            Type = t;
        }

        public override string ToString()
        {
            return String.Format("X: {0}\nY: {1}\nType: {2}", X, Y, Type);
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public string Type { get => type; set => type = value; }
        public Faction Occ { get => occ; set => occ = value; }
    }

    internal class StateManager
    {
        private string state = "start";
        public StateManager()
        {

        }
        public string GetState()
        {
            return state;
        }

        public void SetState(string s) { state = s; }
    }

    public class Race
    {
        string name;
        int intelligence;
        int baseAtt;
        int baseDef;
        int pop;

        public string Name { get => name; set => name = value; }

        [JsonConstructor]
        public Race(string Name)
        {
            this.Name = Name;
        }

        public Race(int index, int p)
        {
            pop = p;
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\race.json"));
            JObject obj = JObject.Parse(array[index].ToString());
            Name = (string)obj["Name"];
            intelligence = (int)obj["Intelligence"];
            baseAtt = (int)obj["BaseAttack"];
            baseDef = (int)obj["BaseDefense"];
        }
        public Race(string n, int p)
        {
            pop = p;
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\race.json"));
            foreach (JObject obj in array)
            {
                if (obj["Name"].ToString() == n)
                {
                    Name = (string)obj["Name"];
                    intelligence = (int)obj["Intelligence"];
                    baseAtt = (int)obj["BaseAttack"];
                    baseDef = (int)obj["BaseDefense"];
                }
            }
        }
        public int[] GetVals() => new int[] { intelligence, baseAtt, baseDef, pop };
        public static int RacesAmount() => JArray.Parse(File.ReadAllText(@"Dependencies\race.json")).Count;
    }

    class War
    {
        private int length = 0;
        private string name;
        private int startYear;
        private Faction side1;
        private Faction side2;
        private bool onGoing = true;

        public War(int year, Faction s1, Faction s2)
        {
            string[] warNames = File.ReadAllLines(@"Dependencies\WarNames.txt");
            Name = warNames[new Random().Next(warNames.Length)];
            StartYear = year;
            Side1 = s1;
            Side2 = s2;
        }

        public int Length { get => length; set => length = value; }
        public string Name { get => name; set => name = value; }
        public int StartYear { get => startYear; set => startYear = value; }
        public bool OnGoing { get => onGoing; set => onGoing = value; }
        internal Faction Side1 { get => side1; set => side1 = value; }
        internal Faction Side2 { get => side2; set => side2 = value; }
    }

    class EventVar
    {
        string name;
        string desc;
        int chance;
        int defChance;
        int rate;

        public EventVar(string n, string d, int c, int r)
        {
            Name = n;
            Desc = d;
            Chance = c;
            DefChance = c;
            Rate = r;
        }

        public void ChanceCheck()
        {
            if (Chance <= 0)
                Chance = DefChance;
        }

        public string Name { get => name; set => name = value; }
        public string Desc { get => desc; set => desc = value; }
        public int Chance { get => chance; set => chance = value; }
        public int Rate { get => rate; set => rate = value; }
        public int DefChance { get => defChance; set => defChance = value; }
    }

    class EventList
    {
        List<EventVar> events = new List<EventVar>();

        public EventList()
        {
            JArray eventArray = JArray.Parse(File.ReadAllText(@"Dependencies\events.json"));
            foreach (JObject o in eventArray)
            {
                events.Add(new EventVar((string)o["Name"], (string)o["Desc"], (int)o["Chance"], (int)o["Rate"]));
            }
        }

        internal List<EventVar> Events { get => events; set => events = value; }

        public void ChangeChance(string e, int val)
        {
            foreach (EventVar v in Events)
            {
                if (v.Name == e)
                {
                    v.Chance = val;
                    return;
                }
            }

        }
    }

    class Event
    {
        EventVar chosen;

        public Event(EventList el)
        {
            int chanceTotal = 0;
            List<EventVar> events = el.Events;
            foreach (EventVar ev in events)
                chanceTotal += ev.Chance;
            int num = HelperClasses.RandomNumber(0, chanceTotal);
            List<int> minMax = new List<int>();
            minMax.Add(0);
            for (int i = 1, j = 0; j < events.Count; i++)
            {
                if (i % 2 == 1)
                {
                    minMax.Add(events[j].Chance + minMax[i - 1] - 1);
                    j++;
                }
                else
                    minMax.Add(minMax[i - 1] + 1);
            }
            for (int i = 0; i < minMax.Count - 1; i++)
            {
                if (num >= minMax[i] && num <= minMax[i + 1])
                {
                    Chosen = events[i / 2];
                }
            }

        }

        internal EventVar Chosen { get => chosen; set => chosen = value; }
    }

    public interface Item
    {

    }

    class WarEvent
    {
        string name;
        string desc;


        public WarEvent()
        {
            JArray events = JArray.Parse(File.ReadAllText(@"Dependencies\warevents.json"));
            int chanceTotal = 0;
            foreach (JObject o in events)
                chanceTotal += (int)o["Chance"];
            int num = HelperClasses.RandomNumber(0, chanceTotal);
            List<int> minMax = new List<int>();
            minMax.Add(0);
            for (int i = 1, j = 0; j < events.Count; i++)
            {
                if (i % 2 == 1)
                {
                    minMax.Add((int)events[j]["Chance"] + minMax[i - 1] - 1);
                    j++;
                }
                else
                    minMax.Add(minMax[i - 1] + 1);
            }
            for (int i = 0; i < minMax.Count - 1; i++)
            {
                if (num >= minMax[i] && num <= minMax[i + 1])
                {
                    JObject eventObj = JObject.Parse(events[i / 2].ToString());
                    Name = (string)eventObj["Name"];
                    Desc = (string)eventObj["Desc"];
                }
            }

        }



        public string Name { get => name; set => name = value; }
        public string Desc { get => desc; set => desc = value; }
    }
    class HistoricalEvent
    {
        string nameEvent;
        int yearEvent;

        public HistoricalEvent(string name, int year)
        {
            Name = name;
            Year = year;
        }

        public override string ToString()
        {
            return String.Format("{0}\nYear: {1}", nameEvent, yearEvent);
        }

        public string Name { get => nameEvent; set => nameEvent = value; }
        public int Year { get => yearEvent; set => yearEvent = value; }
    }
    class PointClass
    {
        int x;
        int y;
        string type;

        public PointClass(int xCoord, int yCoord)
        {
            X = xCoord;
            Y = yCoord;
        }

        public double Distance(PointClass p) => Math.Sqrt(Math.Pow(p.X - X, 2) + Math.Pow(p.Y - Y, 2));

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public string Type { get => type; set => type = value; }
    }



    public class Faction
    {
        string name;
        Race race;
        string raceName;
        int pop = 0;
        double popSeverity;
        List<string> advances = new List<string>();
        List<War> wars = new List<War>();
        List<HistoricalEvent> historicalEvents = new List<HistoricalEvent>();
        PointClass loc = new PointClass(0, 0);

        public int Pop { get => pop; set => pop = value; }
        internal Race Race { get => race; set => race = value; }
        public string Name { get => name; set => name = value; }
        internal List<HistoricalEvent> HistoricalEvents
        {
            get => historicalEvents;
            set => historicalEvents = value;
        }
        internal List<War> Wars { get => wars; set => wars = value; }
        public List<string> Advances { get => advances; set => advances = value; }
        public double PopSeverity { get => popSeverity; set => popSeverity = value; }
        internal PointClass Loc { get => loc; set => loc = value; }

        public Faction()
        {

        }

        [JsonConstructor]
        public Faction(int Pop, string Name, string[] Advances, double PopSeverity)
        {
            this.Pop = Pop;
            this.Name = Name;
            this.PopSeverity = PopSeverity;
            this.Advances = Advances.ToList<string>();
        }

        public Faction(Race r, string n)
        {
            Name = n;
            Race = r;
        }

        public Faction(Race r)
        {
            string[] factionNames = File.ReadAllLines(@"Dependencies\FactionNames.txt");
            Name = factionNames[new Random().Next(factionNames.Length)];
            Race = r;
        }

        public void AddPop(int add) { Pop += add; }

        public override string ToString()
        {
            return String.Format("Name: {0}\nRace: {1}\nPop: {2}\nSeverity: {3}", name, race.Name, pop, popSeverity);
        }
    }

    public class History
    {
        List<Race> races = new List<Race>();
        List<Faction> factions = new List<Faction>();
        Map map;

        internal List<Race> Races { get => races; set => races = value; }
        internal List<Faction> Factions { get => factions; set => factions = value; }
        internal Map Map { get => map; set => map = value; }
    }

    public class Player
    {
        int exp = 0;
        int level = 1;

        public string Class { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int MAtk { get; set; }
        public int Defense { get; set; }
        public int MDef { get; set; }
        public int Intelligence { get; set; }
        public double Money { get; set; }
        public double Luck { get; set; }
        public double Evasion { get; set; }
        public int Speed { get; set; }
        public int Level { get => level; set => level = value; }
        public int Exp { get => exp; set => exp = value; }
        public List<string> invString;
        [JsonIgnore] public List<Item> Inv { get; set; }
        public List<string> equipString;
        [JsonIgnore] public List<Item> Equip { get; set; }
        public List<int> Special { get; set; }

        /*
        equip:
        0: weapon
        1: armor
        2: arms
        3: gloves
        4: pants
        5: boots
        */

        public Player()
        {

        }

        public Player(Player p)
        {
            Attack = p.Attack;
            Class = p.Class;
            Defense = p.Defense;
            Evasion = p.Evasion;
            Health = p.Health;
            MAtk = p.MAtk;
            MDef = p.MDef;
            Intelligence = p.Intelligence;
            Money = p.Money;
            Luck = p.Luck;
            Speed = p.Speed;
            Inv = p.Inv;
            invString = p.invString;
            Equip = p.Equip;
            equipString = p.equipString;
            Special = p.Special;
            Exp = p.Exp;
            Level = p.Level;
        }

        public Player(int slot, int c)
        {
            JArray saves = JArray.Parse(File.ReadAllText(@"Dependencies\player.json"));
            JObject save = JObject.Parse(saves[slot - 1].ToString());
            CreateCharacter(slot, saves, c);
        }
        public Player(int slot)
        {
            JArray saves = JArray.Parse(File.ReadAllText(@"Dependencies\player.json"));
            JObject save = JObject.Parse(saves[slot - 1].ToString());
            if (string.IsNullOrEmpty(save["Name"].ToString()))
            {
                return;
            }
        }

        public void InitEquip()
        {
            string[] split;
            foreach (string str in equipString.ToArray())
            {
                split = str.Split(':');
                switch (split[0])
                {
                    case "sword":
                        Equip.Add(new Sword(split[1]));
                        break;
                    case "staff":
                        Equip.Add(new Staff(split[1]));
                        break;
                    case "knife":
                        Equip.Add(new Knife(split[1]));
                        break;
                    default:
                        break;
                }
            }
        }

        public int NextLevel() => Convert.ToInt32((4 * (Level * Level * Level)) / 5.0);

        public void InitInv()
        {
            string[] split;
            foreach (string str in invString.ToArray())
            {
                split = str.Split(':');
                switch (split[0])
                {
                    case "sword":
                        Inv.Add(new Sword(split[1]));
                        break;
                    case "staff":
                        Inv.Add(new Staff(split[1]));
                        break;
                    case "knife":
                        Inv.Add(new Knife(split[1]));
                        break;
                    default:
                        break;
                }
            }
        }

        public void CreateCharacter(int slot, JArray arr, int inp)
        {
            JObject save = JObject.Parse(arr[slot - 1].ToString());
            switch (inp)
            {
                case 1:
                    Class = "Mage";
                    Attack = 10;
                    MAtk = 18;
                    Defense = 7;
                    MDef = 15;
                    Intelligence = 9;
                    Money = 0;
                    Luck = 4;
                    Evasion = 3;
                    Health = 40;
                    Speed = 6;
                    invString = new List<string> { };
                    equipString = new List<string> { "staff:Wooden Staff" };
                    Special = new List<int> { 1, 0, 0, 0 };
                    break;
                case 2:
                    Class = "Warrior";
                    Attack = 20;
                    MAtk = 2;
                    Defense = 13;
                    MDef = 3;
                    Intelligence = 4;
                    Money = 0;
                    Luck = 2;
                    Evasion = 2;
                    Health = 60;
                    Speed = 2;
                    invString = new List<string> { };
                    equipString = new List<string> { "sword:Bronze Sword" };
                    Special = new List<int> { 3, 0, 0, 0 };
                    break;
                case 3:
                    Class = "Rogue";
                    Attack = 17;
                    MAtk = 4;
                    Defense = 10;
                    MDef = 3;
                    Intelligence = 6;
                    Money = 0;
                    Luck = 8;
                    Evasion = 8;
                    Speed = 9;
                    Health = 40;
                    invString = new List<string> { };
                    equipString = new List<string> { "knife:Bronze Dagger" };
                    Special = new List<int> { 4, 0, 0, 0 };
                    break;
                default:
                    break;
            }
            save["Class"] = Class;
            save["Attack"] = Attack;
            save["Defense"] = Defense;
            save["Health"] = Health;
            save["Magic Attack"] = MAtk;
            save["Magic Defense"] = MDef;
            save["Intelligence"] = Intelligence;
            save["Money"] = Money;
            save["Luck"] = Luck;
            save["Evasion"] = Evasion;
            save["Speed"] = Speed;
            save["Inventory"] = JsonConvert.SerializeObject(invString);
            save["Equipped"] = JsonConvert.SerializeObject(equipString);
            Equip = new List<Item>();
            Inv = new List<Item>();
            InitInv();
            InitEquip();
            arr[slot - 1] = JObject.Parse(save.ToString());
            File.WriteAllText(@"Dependencies\player.json", arr.ToString());
        }

        public string ItemDrop()
        {
            int chanceTotal = 0;
            int num = 0;
            List<int> minMax = new List<int>();
            switch (Class)
            {
                case "Warrior":
                    JArray swords = JArray.Parse(File.ReadAllText("Dependencies\\sword.json"));
                    foreach (JObject sword in swords)
                        chanceTotal += (int)sword["Rarity Level"];
                    num = HelperClasses.RandomNumber(0, chanceTotal);
                    minMax = new List<int>();
                    minMax.Add(0);
                    for (int i = 1, j = 0; j < swords.Count; i++)
                    {
                        if (i % 2 == 1)
                        {
                            minMax.Add((int)swords[j]["Rarity Level"] + minMax[i - 1] - 1);
                            j++;
                        }
                        else
                            minMax.Add(minMax[i - 1] + 1);
                    }
                    for (int i = 0; i < minMax.Count - 1; i++)
                    {
                        if (num >= minMax[i] && num <= minMax[i + 1])
                        {
                            Sword sw = new Sword(i / 2);
                            if (sw.GetName() != "None")
                                Inv.Add(new Sword(i / 2));
                            return new Sword(i / 2).GetName();
                        }
                    }
                    break;
                case "Mage":
                    JArray staves = JArray.Parse(File.ReadAllText("Dependencies\\staff.json"));
                    foreach (JObject staff in staves)
                        chanceTotal += (int)staff["Rarity Level"];
                    num = HelperClasses.RandomNumber(0, chanceTotal);
                    minMax = new List<int>();
                    minMax.Add(0);
                    for (int i = 1, j = 0; j < staves.Count; i++)
                    {
                        if (i % 2 == 1)
                        {
                            minMax.Add((int)staves[j]["Rarity Level"] + minMax[i - 1] - 1);
                            j++;
                        }
                        else
                            minMax.Add(minMax[i - 1] + 1);
                    }
                    for (int i = 0; i < minMax.Count - 1; i++)
                    {
                        if (num >= minMax[i] && num <= minMax[i + 1])
                        {
                            Staff st = new Staff(i / 2);
                            if (st.GetName() != "None")
                                Inv.Add(new Staff(i / 2));
                            return new Staff(i / 2).GetName();
                        }
                    }
                    break;
                case "Rogue":
                    JArray knives = JArray.Parse(File.ReadAllText("Dependencies\\knife.json"));
                    foreach (JObject knife in knives)
                        chanceTotal += (int)knife["Rarity Level"];
                    num = HelperClasses.RandomNumber(0, chanceTotal);
                    minMax = new List<int>();
                    minMax.Add(0);
                    for (int i = 1, j = 0; j < knives.Count; i++)
                    {
                        if (i % 2 == 1)
                        {
                            minMax.Add((int)knives[j]["Rarity Level"] + minMax[i - 1] - 1);
                            j++;
                        }
                        else
                            minMax.Add(minMax[i - 1] + 1);
                    }
                    for (int i = 0; i < minMax.Count - 1; i++)
                    {
                        if (num >= minMax[i] && num <= minMax[i + 1])
                        {
                            Knife k = new Knife(i / 2);
                            if(k.GetName() != "None")
                                Inv.Add(new Knife(i / 2));
                            return new Knife(i / 2).GetName();
                        }
                    }
                    break;
                default:

                    break;
            }
            return "";
        }
    }

    class DefaultRestore
    {
        private static string eventJson;

        public static void BackupEvent()
        {
            eventJson = File.ReadAllText(@"Dependencies\events.json");
        }

        public static void SetEventDefault()
        {

            File.WriteAllText(@"Dependencies\events.json", eventJson);
        }
    }

    public class Staff : Item
    {
        private readonly string name;
        private readonly int att;
        private readonly int def;
        private readonly int buyPrice;
        private readonly int sellPrice;
        private readonly int rarity;
        private readonly string attr;

        public Staff(int index)
        {
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\staff.json"));
            JObject obj = JObject.Parse(array[index].ToString());
            name = (string)obj["Name"];
            att = (int)obj["Attack"];
            def = (int)obj["Defense"];
            buyPrice = (int)obj["Buy Price"];
            sellPrice = (int)obj["Sell Price"];
            rarity = (int)obj["Rarity Level"];
            attr = (string)obj["Attr"];
        }
        public Staff(string n)
        {
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\staff.json"));
            foreach (JObject obj in array)
            {
                if (obj["Name"].ToString() == n)
                {
                    name = (string)obj["Name"];
                    att = (int)obj["Attack"];
                    def = (int)obj["Defense"];
                    buyPrice = (int)obj["Buy Price"];
                    sellPrice = (int)obj["Sell Price"];
                    rarity = (int)obj["Rarity Level"];
                    attr = (string)obj["Attr"];
                }
            }
            if (string.IsNullOrEmpty(name))
                throw new InvalidOperationException("Unable to find sword: " + n);
        }

        public override string ToString()
        {
            string output = "";
            string.Format(output, "Name: {0}\nAttack: {1}\nDefense: {2}\nBuy Price:{3}\n Sell Price: {4}\nRarity Level: {5}", name, att, def, buyPrice, sellPrice, rarity);
            return output;
        }

        public string GetName()
        {
            return name;
        }

        public int[] GetVals()
        {
            return new int[] { att, def, buyPrice, sellPrice };
        }
    }

    public class Knife : Item
    {
        private readonly string name;
        private readonly int att;
        private readonly int def;
        private readonly int buyPrice;
        private readonly int sellPrice;
        private readonly int rarity;
        private readonly string attr;

        public Knife(int index)
        {
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\knife.json"));
            JObject obj = JObject.Parse(array[index].ToString());
            name = (string)obj["Name"];
            att = (int)obj["Attack"];
            def = (int)obj["Defense"];
            buyPrice = (int)obj["Buy Price"];
            sellPrice = (int)obj["Sell Price"];
            rarity = (int)obj["Rarity Level"];
            attr = (string)obj["Attr"];
        }
        public Knife(string n)
        {
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\knife.json"));
            foreach (JObject obj in array)
            {
                if (obj["Name"].ToString() == n)
                {
                    name = (string)obj["Name"];
                    att = (int)obj["Attack"];
                    def = (int)obj["Defense"];
                    buyPrice = (int)obj["Buy Price"];
                    sellPrice = (int)obj["Sell Price"];
                    rarity = (int)obj["Rarity Level"];
                    attr = (string)obj["Attr"];
                }
            }
            if (string.IsNullOrEmpty(name))
                throw new InvalidOperationException("Unable to find sword: " + n);
        }

        public override string ToString()
        {
            string output = "";
            string.Format(output, "Name: {0}\nAttack: {1}\nDefense: {2}\nBuy Price:{3}\n Sell Price: {4}\nRarity Level: {5}", name, att, def, buyPrice, sellPrice, rarity);
            return output;
        }

        public string GetName()
        {
            return name;
        }

        public int[] GetVals()
        {
            return new int[] { att, def, buyPrice, sellPrice };
        }
    }

    public class Sword : Item
    {
        private readonly string name;
        private readonly int att;
        private readonly int def;
        private readonly int buyPrice;
        private readonly int sellPrice;
        private readonly int rarity;
        private readonly string attr;

        public Sword(int index)
        {
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\sword.json"));
            JObject obj = JObject.Parse(array[index].ToString());
            name = (string)obj["Name"];
            att = (int)obj["Attack"];
            def = (int)obj["Defense"];
            buyPrice = (int)obj["Buy Price"];
            sellPrice = (int)obj["Sell Price"];
            rarity = (int)obj["Rarity Level"];
            attr = (string)obj["Attr"];
        }
        public Sword(string n)
        {
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\sword.json"));
            foreach (JObject obj in array)
            {
                if (obj["Name"].ToString() == n)
                {
                    name = (string)obj["Name"];
                    att = (int)obj["Attack"];
                    def = (int)obj["Defense"];
                    buyPrice = (int)obj["Buy Price"];
                    sellPrice = (int)obj["Sell Price"];
                    rarity = (int)obj["Rarity Level"];
                    attr = (string)obj["Attr"];
                }
            }
            if (string.IsNullOrEmpty(name))
                throw new InvalidOperationException("Unable to find sword: " + n);
        }

        public Sword()
        {
        }

        public override string ToString()
        {
            string output = "";
            string.Format(output, "Name: {0}\nAttack: {1}\nDefense: {2}\nBuy Price:{3}\n Sell Price: {4}\nRarity Level: {5}", name, att, def, buyPrice, sellPrice, rarity);
            return output;
        }

        public string GetName()
        {
            return name;
        }

        public int[] GetVals()
        {
            return new int[] { att, def, buyPrice, sellPrice };
        }
    }
}
