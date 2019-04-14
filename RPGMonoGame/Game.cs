using System;
using System.Collections.Generic;
using System.Diagnostics;
using RPGMonoGame;

namespace RPGv2
{
    internal class Game
    {
        HelperClasses hc = new HelperClasses();
        public static History hist = new History();
        internal HelperClasses HC { get => HC; set => HC = value; }
        public static Player player;

        public static void StartGame()
        {
            int years = int.Parse(GlobalValues.inpText);
            // TODO: When start button is clicked the game will start 
            GlobalValues.startGen = true;
            hist = StartHistory(years);
            Faction fac = new Faction(new Race(1, 2), " ");
            foreach(Faction f in hist.Factions.ToArray())
            {
                if (f.Pop >= fac.Pop)
                    fac = f;
            }
            Story.enemyFaction = fac;
            Faction affrieca = new Faction(new Race(1, 10000), "Affrieca");
            affrieca.Pop = 20000;
            hist.Factions.Add(affrieca);
        }

        public static History StartHistory(int years)
        {
            EventList el = new EventList();
            History h = new History();
            h.Map = new Map(@"Dependencies\Maps\NewMap.png");

            List<Race> races = new List<Race>();
            List<Faction> factions = new List<Faction>();
            for (int i = 0; i < Race.RacesAmount(); i++)
            {
                races.Add(new Race(i, new Random().Next(100000)));
            }
            int num;
            for (int i = 0; i < races.Count; i++)
            {
                Race race = races[i];
                int[] vals = race.GetVals();
                factions.Add(new Faction(races[i], "Main City: " + race.Name));
                int mainCityInd = factions.Count - 1;
                for (int j = 0; j <= vals[3]; j++)
                {
                    num = HelperClasses.RandomNumber(0, 100);
                    if (num < 75)
                    {
                        factions[mainCityInd].Pop++;
                        num = 101;
                    }
                    if (num < 90)
                    {
                        bool done = false;
                        Random rand2 = new Random();
                        while (!done)
                        {
                            int num2 = rand2.Next(factions.Count);
                            Faction f = factions[num2];
                            if (f.Race == race)
                            {
                                done = true;
                                factions[num2].Pop++;
                            }
                        }
                        num = 101;
                    }
                    if (num < 100)
                    {
                        bool exists = false;
                        Faction f = new Faction(race);
                        for (int k = 0; k < factions.Count; k++)
                        {
                            if (factions[k].Name == f.Name)
                            {
                                exists = true;
                                factions[k].Pop++;
                            }
                        }
                        if (!exists)
                        {
                            factions.Add(new Faction(race));
                            factions[factions.Count - 1].Pop++;
                        }
                    }
                }
            }

            int totalPeople = 0;
            double averagePopSeverity = 0;
            while (factions.Count >= h.Map.GetLandCount())
            {
                double avePop = 0;
                foreach (Faction f in factions.ToArray())
                {
                    avePop += f.Pop;
                }
                avePop /= factions.Count;
                foreach (Faction f in factions.ToArray())
                {
                    if (f.Pop < avePop - avePop / 2)
                        factions.Remove(f);
                }
            }
            foreach (Faction fac in factions.ToArray())
            {
                h.Map.InitFaction(fac);
            }
            for (int i = 0; i <= years; i++)
            {
                GlobalValues.yearNum = "Year: " + i;
                totalPeople = 0;
                averagePopSeverity = 0;
                foreach (Faction f in factions)
                    totalPeople += f.Pop;
                foreach (Faction f in factions)
                {
                    f.PopSeverity = (double)f.Pop / totalPeople;
                    averagePopSeverity += f.PopSeverity;
                }
                averagePopSeverity /= factions.Count;
                #region events
                int chainAmount = 0;
                for (int j = 0; j < factions.Count; j++)
                {
                    Faction f = factions[j];
                    do
                    {
                        Event newEvent = new Event(el);
                        EventVar e = newEvent.Chosen;
                        GlobalValues.eventName = e.Name;

                        switch (e.Name)
                        {
                            #region none
                            case "None":
                                break;
                            #endregion
                            #region chain event
                            case "Chain Event":
                                chainAmount += 3;
                                break;
                            #endregion
                            #region famine
                            case "Famine":
                                int deathChance = new Random().Next(70);
                                Random rando = new Random();
                                f.Pop -= Convert.ToInt32(f.Pop * (deathChance / 100.0));
                                if (deathChance < 10)
                                {
                                    f.HistoricalEvents.Add(new HistoricalEvent("slight famine", i));
                                    break;
                                }
                                if (deathChance < 30)
                                {
                                    f.HistoricalEvents.Add(new HistoricalEvent("mild famine", i));
                                    break;
                                }
                                if (deathChance < 70)
                                {
                                    f.HistoricalEvents.Add(new HistoricalEvent("severe famine", i));
                                    break;
                                }
                                if (deathChance < 100)
                                {
                                    f.HistoricalEvents.Add(new HistoricalEvent("extreme famine", i));
                                    break;
                                }
                                break;
                            #endregion
                            #region popup
                            case "Population Up":
                                int percentUp = new Random().Next(1, 20);
                                f.Pop += Convert.ToInt32(f.Pop * (percentUp / 100.0));
                                break;
                            #endregion
                            #region popdown
                            case "Population Down":
                                int percentDown = new Random().Next(1, 10);
                                f.Pop += Convert.ToInt32(f.Pop * (percentDown / 100.0));
                                break;
                            #endregion
                            case "War Declaration":
                                bool canFind = false;
                                for (int k = 0; k < factions.Count; k++)
                                {
                                    if (factions[k].Race != f.Race && factions[k] != f && f.Pop > factions[k].Pop / 2 && f.Pop < factions[k].Pop * 2)
                                    {
                                        canFind = true;
                                    }
                                }
                                if (!canFind)
                                    break;
                                bool doneFinding = false;
                                Faction opp = new Faction(new Race(0, 0));
                                while (!doneFinding)
                                {
                                    num = HelperClasses.RandomNumber(0, factions.Count);
                                    opp = factions[num];
                                    if (opp.Race != f.Race && opp != f && f.Pop > opp.Pop / 2 && f.Pop < opp.Pop * 2)
                                        doneFinding = true;
                                }
                                f.Wars.Add(new War(i, f, opp));
                                break;
                            case "Discovery":
                                break;
                            case "New Faction":
                                int breakOff = HelperClasses.RandomNumber(1, f.Pop);
                                Faction newFaction = new Faction(f.Race);
                                newFaction.Pop += breakOff;
                                f.Pop -= breakOff;
                                factions.Add(newFaction);
                                GlobalValues.facCreate = f.Name + " has been created!";
                                newFaction.HistoricalEvents.Add(new HistoricalEvent("Broke off from " + f.Name, i));
                                h.Map.InitFaction(newFaction);
                                break;
                            #region default
                            default:
                                Console.Clear();
                                Console.WriteLine("An unknown event has occured, event name: {0}", e.Name);
                                Console.ReadKey();
                                break;
                                #endregion
                        }
                        chainAmount--;
                    } while (chainAmount > 0);
                    #region pophandling
                    int popNum = Convert.ToInt32(f.PopSeverity * 100000);
                    if (popNum > 0)
                    {
                        int avePopNum = Convert.ToInt32(averagePopSeverity * 100000);
                        int popRand = HelperClasses.RandomNumber(0, popNum);
                        int avePopRand = HelperClasses.RandomNumber(0, avePopNum);
                        if (popRand > avePopNum)
                        {
                            foreach (EventVar ev in el.Events)
                            {
                                ev.Chance += ev.Rate;
                                ev.ChanceCheck();
                            }
                        }
                        else
                            foreach (EventVar ev in el.Events)
                                ev.Chance = ev.DefChance;
                    }
                    #endregion

                }
                #endregion
                #region warhandling
                for (int j = 0; j < factions.Count; j++)
                {
                    Faction f = factions[j];
                    bool inWar = false;
                    List<War> wars = new List<War>();
                    foreach (War w in f.Wars)
                        if (w.OnGoing)
                        {
                            inWar = true;
                            wars.Add(w);
                        }
                    if (inWar)
                    {
                        for (int k = 0; k < wars.Count; k++)
                        {
                            Faction warWith = wars[k].Side2;
                            WarEvent we = new WarEvent();
                            int num1;
                            int num2;
                            switch (we.Name)
                            {
                                case "None":
                                    wars[k].Length++;
                                    break;
                                case "Attack":
                                    for (int l = 0; l < f.Pop + warWith.Pop; l++)
                                    {
                                        num1 = HelperClasses.RandomNumber(0, f.Race.GetVals()[2] + (f.Race.GetVals()[1] / 2) + f.Race.GetVals()[0]);
                                        num2 = HelperClasses.RandomNumber(0, warWith.Race.GetVals()[1] + (warWith.Race.GetVals()[2] / 2) + warWith.Race.GetVals()[0]);
                                        if (num1 >= num2)
                                            warWith.Pop--;
                                        else
                                            f.Pop--;
                                    }
                                    wars[k].Length++;
                                    break;
                                case "Defend":
                                    for (int l = 0; l < f.Pop + warWith.Pop; l++)
                                    {
                                        num1 = HelperClasses.RandomNumber(0, warWith.Race.GetVals()[1] + (warWith.Race.GetVals()[2] / 2) + warWith.Race.GetVals()[0]);
                                        num2 = HelperClasses.RandomNumber(0, f.Race.GetVals()[2] + (f.Race.GetVals()[1] / 2) + f.Race.GetVals()[0]);
                                        if (num1 >= num2)
                                            f.Pop--;
                                        else
                                            warWith.Pop--;
                                    }
                                    wars[k].Length++;
                                    break;
                                case "End War":
                                    wars[k].OnGoing = false;
                                    f.HistoricalEvents.Add(new HistoricalEvent(string.Format("At war with {0} for {1} years", wars[k].Side2, wars[k].Length), i));
                                    break;
                                default:
                                    Console.Clear();
                                    Console.WriteLine("An unknown event has occured, name: {0}", we.Name);
                                    Console.ReadKey();
                                    break;
                            }
                        }
                    }
                }
                if (i == years - 1)
                {
                    foreach (Faction fac in factions.ToArray())
                    {
                        List<War> wars = new List<War>(fac.Wars);
                        if (wars.Count != 0)
                        {
                            foreach (War w in wars.ToArray())
                            {
                                if (!w.OnGoing)
                                    wars.Remove(w);
                            }
                            foreach (War w in wars)
                            {
                                fac.HistoricalEvents.Add(new HistoricalEvent(String.Format("At war with {0} since {1}", w.Side2, w.StartYear), w.StartYear));
                            }
                        }
                    }
                }
                bool exists;
                foreach (Tile t in h.Map.Tiles)
                {
                    exists = false;
                    foreach (Faction fac in factions)
                    {
                        if (fac == t.Occ || t.Type == "W")
                            exists = true;
                    }
                    if (!exists)
                    {
                        t.Occ = null;
                        t.Type = "L";
                    }
                }
                for (int i1 = 0; i1 < factions.Count; i1++)
                {
                    Faction f = factions[i1];
                    if (f.Pop <= 0)
                    {
                        GlobalValues.facDestroyed = f.Name + " has been destroyed!";
                        factions.Remove(f);
                    }
                }
                #endregion
            }

            GlobalValues.done = true;
            h.Races = races;
            h.Factions = factions;
            return h;
        }
    }
}
