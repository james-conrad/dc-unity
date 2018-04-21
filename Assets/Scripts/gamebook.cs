using System;
using System.IO;

public class gamebook
{
    //{This unit contains a record type which is used for binding}
    //{together game information. It also contains some procedures}
    //{for correlating the information between different game}
    //{structures, such as matching models with individual}
    //{critters and so on.}

    public const int DF_Physical = 1;
    public const int DF_AvoidTrap = 2;
    public const int DF_Mystic = 3;
    public const string PLT_MapSquare = "MS";
    public const string PLT_SeeNewCritter = "NC";
    public const string PLT_EnterCom = "EN";

    //{ This constant is the number of levels- consequently, the size }
    //{ of the Frozen Levels array }
    public const int Num_Levels = 8;

    public class Frozen
    {
        //{ This record holds the level descriptions for a }
        //{ level that the player has left. }
        //{ ANy data not saved here should be deallocated when }
        //{ the level changes. }
        public texmaps.GameBoard gb;
        public dcitems.IGrid ig;
        public plotbase.SAtt PL;
        public critter.Critter CList;
        public cwords.MPU Comps;
    }

    public class Scenario
    {
        //{This record type holds pointers to all associated}
        //{data needed for a single level in the game. It also}
        //{has pointers to related data, such as the PC's}
        //{character structure.}
        public int ComTime;            //{How many seconds have passed.}
        public int Loc_Number;         //{ What level the PC is on. }
        public texmaps.GameBoard gb;   //{the map}
        public dcitems.IGrid ig;       //{the stuff on the map}
        public plotbase.SAtt PLLocal;  //{Local plot effects.}
        public plotbase.SAtt PLGlobal; //{Global plot effects.}
        public plotbase.SAtt PLTrig;   //{Triggers waiting to be processed.}
        public critter.Critter CList;  //{the monsters}
        public cwords.MPU Comps;       //{the computer terminals}
        public cwords.Cloud Fog;       //{Vapors floating around the map}
        public critter.Critter CAct;   //{the active critter, and the next to move.}
        public critter.Critter CA2;
        public dcchars.DCChar PC;      //{the PC}
        public plotbase.NAtt NA;       //{ Numeric Attributes }
        public Frozen[] Frozen_Levels = new Frozen[Num_Levels];
    }

    public static Scenario NewScenario()
    {
        //{Create a new scenario structure. Initialize everything to null.}
        Scenario SC = new Scenario();
        SC.gb = null;
        SC.ig = null;
        SC.PLLocal = null;
        SC.PLGlobal = null;
        SC.PLTrig = null;
        SC.CList = null;
        SC.Comps = null;
        SC.Fog = null;
        SC.CAct = null;
        SC.CA2 = null;
        SC.PC = null;
        SC.ComTime = 0;
        SC.NA = null;
        for (int t = 0; t < Num_Levels; ++t)
        {
            SC.Frozen_Levels[t] = new Frozen();
            SC.Frozen_Levels[t].gb = null;
            SC.Frozen_Levels[t].ig = null;
            SC.Frozen_Levels[t].PL = null;
            SC.Frozen_Levels[t].CList = null;
            SC.Frozen_Levels[t].Comps = null;
        }

        return SC;
    }

    //{*** MODEL LOOKUP FUNCTIONS ***}
    public static void Excommunicate(Scenario SC, texmodel.Model M)
    {
        //{This isn't a lookup function, but it seemed appropriate}
        //{to place it here. Model M and the thing it belongs to are}
        //{about to be removed from play. Remove all mention of this}
        //{model from game memory.}


        //{Remove all mention of this model from the Target lists}
        //{of various monsters.}
        critter.Critter CT = SC.CList;
        while (CT != null)
        {
            if (CT.Target == M)
                CT.Target = null;

            CT = CT.next;
        }

        //{Clear the PC's target.}
        if (SC.PC.target == M)
            SC.PC.target = null;

        //{Clear the active critter, if this is the active critter.}
        if (SC.CAct != null && SC.CAct.M == M)
            SC.CAct = null;

        //{If the next critter to act is the one who was killed,}
        //{move that pointer to the next critter in line.}
        if (SC.CA2 != null & SC.CA2.M == M)
        {
            SC.CA2 = SC.CA2.next;
        }
    }

    public static int ModelDefenseStep(Scenario SC, texmodel.Model M, int D)
    {
        //{Given a model pointer, determine the Defense Step of the}
        //{entity to which it belongs.}
        int it = 1;
        switch (M.kind)
        {
            case critter.MKIND_Critter:
                //{The model is a critter.}
                //{Look up its defense step from the appropriate array.}
                switch (D)
                {
                    case DF_Mystic:
                        it = critter.MonMan[critter.LocateCritter(M, SC.CList).crit - 1].Mystic;
                        break;
                    case DF_AvoidTrap:
                        it = critter.MonMan[critter.LocateCritter(M, SC.CList).crit - 1].Sense;
                        break;
                    case DF_Physical:
                        it = critter.MonMan[critter.LocateCritter(M, SC.CList).crit - 1].DefStep;
                        break;
                }
                break;
            case dcchars.MKIND_Character:
                //{The model is a character.}
                switch (D)
                {
                    case DF_Physical:
                        it = dcchars.PCDefense(SC.PC);
                        break;
                    case DF_AvoidTrap:
                        it = dcchars.PCLuckSave(SC.PC);
                        break;
                    case DF_Mystic:
                        it = dcchars.PCMysticDefense(SC.PC);
                        break;
                }
                break;
            default:
                it = 1;
                break;
        }

        //{Just as a precaution, make sure that the number doesn't}
        //{fall below a certain minimal value.}
        if (it < 1)
            it = 1;

        //{Return the defense value.}
        return it;
    }

    public static string ModelName(Scenario SC, texmodel.Model M)
    {
        //{Given a model, M, look up its name.}
        //{If the model is the PC, return the string "You".}
        if (M == null)
            return "Empty Space";
        else if (M.kind == dcchars.MKIND_Character)
            return "you";
        else if (M.kind == critter.MKIND_Critter)
            return critter.MonMan[critter.LocateCritter(M, SC.CList).crit - 1].name;
        else if (M.kind == cwords.MKIND_Cloud)
            return cwords.CloudMan[cwords.LocateCloud(M, SC.Fog).Kind - 1].name;
        else if (M.kind == cwords.MKIND_MPU)
            return cwords.MPUMan[cwords.LocateMPU(M, SC.Comps).kind - 1].name;

        return "Unknown";
    }

    public static string TileName(Scenario SC, int X, int Y)
    {
        //{Given location X,Y provide a string to describe the contents.}
        if (!texmaps.OnTheMap(X, Y))
            return "Swirling Pandemonium";
        else if (texmodel.ModelPresent(SC.gb.mog, X, Y) && texmaps.TileLOS(SC.gb.POV, X, Y))
            return ModelName(SC, texmodel.FindModelXY(SC.gb.mlist, X, Y));
        else if (SC.gb.itm[X - 1, Y - 1].gfx != ' ' && texmaps.TileLOS(SC.gb.POV, X, Y))
            return "an item";
        else if (SC.gb.map[X - 1, Y - 1].trap > 0 && texmaps.TileLOS(SC.gb.POV, X, Y))
            return "a trap";
        else if (SC.gb.map[X - 1, Y - 1].visible)
            return texmaps.TerrName[SC.gb.map[X - 1, Y - 1].terr];

        return "unknown";
    }

    //{Print details about the PC in row 25 of the screen.}
    const int PCSLNumSFX = 3; // {PC Stat Line Number of Status Effects}
    static string[] SFXChar = new string[PCSLNumSFX] { "Par", "Zzz", "Psn" };
    static Crt.Color[] SFXColor = new Crt.Color[PCSLNumSFX]
    {
        Crt.Color.Magenta,
        Crt.Color.White,
        Crt.Color.LightGreen
    };

    static int[] SFXVal = new int[PCSLNumSFX]
    {
        statusfx.SEF_Paralysis,
        statusfx.SEF_Sleep,
        statusfx.SEF_Poison,
    };

    //{*** GENERAL IO ROUTINES ***}
    public static void PCStatLine(Scenario SC)
    {
        Crt.Window(1, 25, 79, 25);
        Crt.ClrScr();
        Crt.Window(1, 1, 80, 25);

        //{Print data on status effects.}
        Crt.GotoXY(2, 25);
        //{ First, if player is starving, show that. }
        if (SC.PC.carbs < 0)
        {
            Crt.TextColor(Crt.Color.LightRed);
            Crt.Write("Sta");
        }
        else if (SC.PC.carbs < 10)
        {
            Crt.TextColor(Crt.Color.Yellow);
            Crt.Write("Hgr");
        }

        int t;
        for (t = 0; t < PCSLNumSFX; ++t)
        {
            if (Crt.WhereX() < 15 && plotbase.NAttValue(SC.PC.SF, statusfx.NAG_StatusChange, SFXVal[t]) != 0)
            {
                Crt.TextColor(SFXColor[t]);
                Crt.Write(SFXChar[t]);
            }
        }

        int HP = SC.PC.HP;
        if (HP < 0)
            HP = 0;
        else if (HP > 999)
            HP = 999;
        Crt.TextColor(Crt.Color.Green);
        Crt.GotoXY(18, 25);
        Crt.Write("HP:");
        Crt.TextColor(StatusColor(SC.PC.HPMax, SC.PC.HP));
        Crt.Write(HP.ToString());

        HP = SC.PC.MP;
        if (HP < 0)
            HP = 0;
        else if (HP > 999)
            HP = 999;
        Crt.TextColor(Crt.Color.Green);
        Crt.GotoXY(25, 25);
        Crt.Write("MP:");
        Crt.TextColor(StatusColor(SC.PC.MPMax, SC.PC.MP));
        Crt.Write(HP.ToString());

        for (t = 0; t < 8; ++t)
        {
            Crt.TextColor(Crt.Color.Green);
            Crt.GotoXY(33 + t * 6, 25);
            Crt.Write(dcchars.StatAbbrev[t] + ":");
            HP = dcchars.CStat(SC.PC, t);
            if (HP < SC.PC.stat[t])
                Crt.TextColor(Crt.Color.Red);
            else if (HP > SC.PC.stat[t])
                Crt.TextColor(Crt.Color.LightBlue);
            else
                Crt.TextColor(Crt.Color.LightGreen);
            if (HP > 99) HP = 99;
            if (HP < 10) Crt.Write(' ');
            Crt.Write(HP.ToString());
        }
    }

    public static void SaveGame(Scenario SC)
    {
        //{This is it. The big one. Save everything to disk...}

        //{Open the file.}
        FileStream file = File.Create("savegame/" + SC.PC.name + ".txt");
        StreamWriter f = new StreamWriter(file);

        //{Write the savefile version first of all.}
        f.WriteLine(SaveFileVersion);

        //{Write the current ComTime.}
        f.WriteLine(SC.ComTime);

        f.WriteLine(SC.Loc_Number);

        //{Write the GameBoard}
        texmaps.WriteGameBoard(SC.gb, f);

        //{Write the PC data.}
        dcchars.WritePC(SC.PC, f);

        //{Write the Item Grid}
        dcitems.WriteIGrid(SC.ig, f);

        //{Write the Critters List}
        critter.WriteCritterList(SC.CList, f);

        //{Write the PlotLine Scripts}
        plotbase.WriteSAtt(SC.PLLocal, f);
        plotbase.WriteSAtt(SC.PLGlobal, f);

        //{Write the clouds}
        cwords.WriteClouds(SC.Fog, f);

        //{ Write the computers. }
        cwords.WriteMPU(SC.Comps, f);
        rpgtext.SaveLogon(f);

        //{ Write the numeric attributes. }
        plotbase.WriteNAtt(SC.NA, f);

        //{ Write the frozen levels. }
        f.WriteLine("*** FROZEN LEVELS ***");
        for (int t = 0; t < Num_Levels; ++t)
        {
            if (SC.Frozen_Levels[t].gb != null)
            {
                f.WriteLine(t + 1);
                texmaps.WriteGameBoard(SC.Frozen_Levels[t].gb, f);
                dcitems.WriteIGrid(SC.Frozen_Levels[t].ig, f);
                plotbase.WriteSAtt(SC.Frozen_Levels[t].PL, f);
                critter.WriteCritterList(SC.Frozen_Levels[t].CList, f);
                cwords.WriteMPU(SC.Frozen_Levels[t].Comps, f);
            }
        }
        f.WriteLine("-1");

        //{Close the file}
        file.Close();
    }

    public static Scenario LoadGame(string FName)
    {
        //{Load everything from disk and make sure that it's the}
        //{same as when it was saved.}
        Scenario SC = new Scenario();

        Crt.Write("Loading...\n");

        FileStream file = File.OpenRead(FName);
        StreamReader f = new StreamReader(file);

        int SFV = int.Parse(f.ReadLine());

        // For now, just complain if the SaveVersion doesn't match..
        if (SFV != SaveFileVersion)
        {
            Crt.Write("Incompatible file format");
            return null;
        }

        SC.ComTime = int.Parse(f.ReadLine());

        SC.Loc_Number = int.Parse(f.ReadLine());

        SC.gb = texmaps.ReadGameBoard(f);

        SC.PC = dcchars.ReadPC(f, SC.gb, SFV);

        SC.ig = dcitems.ReadIGrid(f, SC.gb);

        SC.CList = critter.ReadCritterList(f, SC.gb, SFV);

        SC.PLLocal = plotbase.ReadSAtt(f);
        SC.PLGlobal = plotbase.ReadSAtt(f);

        SC.Fog = cwords.ReadClouds(f, SC.gb);
        SC.Comps = cwords.ReadMPU(f, SC.gb);
        rpgtext.LoadLogon(f);

        SC.NA = plotbase.ReadNAtt(f);

        //{ Read frozen levels. }

        //{ Dispose of the header. }
        f.ReadLine();

        int T = int.Parse(f.ReadLine());
        while (T != -1)
        {
            SC.Frozen_Levels[T - 1].gb = texmaps.ReadGameBoard(f);
            SC.Frozen_Levels[T - 1].ig = dcitems.ReadIGrid(f, SC.Frozen_Levels[T - 1].gb);
            SC.Frozen_Levels[T - 1].PL = plotbase.ReadSAtt(f);
            SC.Frozen_Levels[T - 1].CList = critter.ReadCritterList(f, SC.Frozen_Levels[T - 1].gb, SFV);
            SC.Frozen_Levels[T - 1].Comps = cwords.ReadMPU(f, SC.Frozen_Levels[T - 1].gb);

            T = int.Parse(f.ReadLine());
        }

        file.Close();

        return SC;
    }

    //{*** GAMING ENVIRONMENT ROUTINES ***}
    public static void SetTrigger(Scenario SC, string Trigger)
    {
        //{ Place the requested trigger in the triggers list. }
        //{ This is pretty easy. }
        plotbase.StoreSAtt(ref SC.PLTrig, Trigger);
    }

    public static void SetTrigger(Scenario SC, string Trigger, int P1)
    {
        //{ Place the requested trigger with parameter P1 in the list. }
        SetTrigger(SC, Trigger + P1.ToString());
    }

    public static void SetTrigger(Scenario SC, string Trigger, int P1, int P2)
    {
        //{ Place the requested trigger with parameter P1 in the list. }
        SetTrigger(SC, String.Format("{0}{1}%{2}", Trigger, P1, P2));
    }

    public static void UpdateMonsterMemory(Scenario SC, critter.Critter C)
    {
        //{This monster has apparently just walked into the player's}
        //{view. Update the player's Monster Memory, and maybe print}
        //{the monster's introductory text.}
        if (plotbase.NAttValue(SC.NA, plotbase.NAG_MonsterMemory, C.crit) < 100)
        {
            plotbase.AddNAtt(ref SC.NA, plotbase.NAG_MonsterMemory, C.crit, 1);
            if (plotbase.NAttValue(SC.NA, plotbase.NAG_MonsterMemory, C.crit) == 1)
            {
                SetTrigger(SC, PLT_SeeNewCritter, C.crit);
                if (critter.MonMan[C.crit - 1].IntroText != null)
                {
                    rpgtext.DCGameMessage(critter.MonMan[C.crit - 1].IntroText, true);
                    texfx.ModelFlash(SC.gb, C.M);
                    rpgtext.GamePause();
                }
            }
        }
        C.Spotted = true;
    }

    static int[,] Chart = new int[12, 12]
    {
        {   0,0,0,0,0,0,0,0,0,0,0,0 }, //{Speed 0}
	    {   1,0,0,0,0,0,0,0,0,0,0,0 }, //{Speed 1}
	    {   0,0,1,0,0,0,0,0,1,0,0,0 }, //{Speed 2}
	    {   0,1,0,0,0,1,0,0,0,1,0,0 }, //{Speed 3}
	    {   1,0,0,1,0,0,1,0,0,1,0,0 }, //{Speed 4}
	    {   0,1,0,0,1,0,1,0,1,0,0,1 }, //{Speed 5}
	    {   1,0,1,0,1,0,1,0,1,0,1,0 }, //{Speed 6}
	    {   1,0,1,0,1,0,1,0,1,1,0,1 }, //{Speed 7}
	    {   1,1,0,0,1,1,0,1,1,1,1,0 }, //{Speed 8}
	    {   1,1,0,1,1,1,0,1,1,1,0,1 }, //{Speed 9}
	    {   1,1,0,1,1,1,1,1,1,1,1,0 }, //{Speed 10}
	    {   1,1,1,1,0,1,1,1,1,1,1,1 }  //{Speed 11}
	 };

    public static int NumberOfActions(int CT, int Spd)
    {
        //{Return the number of actions which a model moving at}
        //{speed SPD would be able to perform during this click.}
        //{SPD indicates how many actions a model will take during}
        //{a 12 click period.}
        int RT = CT % 12;
        int it = 0;

        //{Check for minimum speed}
        if (Spd < 1)
        {
            Spd = 1;
        }
        //{If more than 12 actions are to be taken, sort that out first.}
        else if (Spd >= 12)
        {
            it += Spd / 12;
            Spd %= 12;
        }

        //{Determine whether or not this is one of the clicks in which}
        //{the model gets to take an action.}
        if (Chart[Spd, RT] > 0)
            it += 1;

        return it;
    }

    public static int XPNeeded(int lvl)
    {
        //{Calculate the XP needed to reach level Lvl.}
        return (lvl * lvl * 25) + (lvl * 15);
    }

    public static void DoleExperience(Scenario SC, int XPV)
    {
        //{Give XPV experience points to the character.}
        //{Check for going up a level.}

        SC.PC.XP = SC.PC.XP + XPV;
        if (SC.PC.XP >= XPNeeded(SC.PC.lvl + 1))
        {
            LevelUp(SC);
            PCStatLine(SC);
        }
    }

    const int SaveFileVersion = -1010;

    static Crt.Color StatusColor(int M, int C)
    {
        //{Given a maximum value of M and a current value of C, return the}
        //{appropriate status color. The three colors being used are red,}
        //{yellow, and green.}
        if (C < 1 && M > 0)
        {
            //{If the part is out of hits, and this isn't normal for}
            //{said part, color will be grey, implying complete stoppage}
            //{of function.}
            return Crt.Color.DarkGray;
        }
        else if (C < M / 4)
        {
            return Crt.Color.Red;
        }
        else if (C < M / 2)
        {
            return Crt.Color.Yellow;
        }
        else if (C < M)
        {
            return Crt.Color.Green;
        }
        else if (C == M)
        {
            return Crt.Color.LightGreen;
        }

        return Crt.Color.LightCyan;
    }

    static int[,] SkChart = new int[3, 3]
    {
        { 0, 0, 0},
        { 0, 0, 1},
        { 1, 1, 0}
    };

    static void LevelUp(Scenario SC)
    {
        //{The PC has just advanced an experience level. Do whatever}
        //{you have to.}
        rpgtext.DCGameMessage("You have gained a level!");
        rpgtext.GamePause();

        SC.PC.lvl += 1;

        //{Calculate the bonus HitPoints}
        int P = dcchars.JobHitDie[SC.PC.job - 1] + rpgdice.rng.Next(dcchars.JobHitDie[SC.PC.job - 1]) + dcchars.PCHPBonus(SC.PC);
        SC.PC.HPMax += P;
        SC.PC.HP += P;

        //{Calculate the bonus MojoPoints}
        P = dcchars.JobMojoDie[SC.PC.job - 1] + rpgdice.rng.Next(dcchars.JobMojoDie[SC.PC.job - 1]) + dcchars.PCMPBonus(SC.PC);
        SC.PC.MPMax += P;
        SC.PC.MP += P;

        //{Improve skill ratings}
        for (int T = 1; T <= dcchars.NumSkill; T++)
        {
            //{If the adv number is > 0, this indicates rate.}
            //{if <0, this indicates slow progress.}
            //{if ==0, this indicates no improvement.}
            if (dcchars.SkillAdv[SC.PC.job - 1, T - 1] > 0)
            {
                P = dcchars.SkillAdv[SC.PC.job - 1, T - 1];
                if (P >= 3)
                {
                    //{The PC gains more than 1 pt/Level}
                    SC.PC.skill[T - 1] += (P / 3);
                    P = P % 3;
                }

                SC.PC.skill[T - 1] += SkChart[P, SC.PC.lvl % 3];

                if (SC.PC.lvl == 2)
                {
                    //{A level 2 character gets the level 1 bonuses as well.}
                    if (dcchars.SkillAdv[SC.PC.job - 1, T - 1] >= 3)
                    {
                        //{The PC gains more than 1 pt/Level}
                        SC.PC.skill[T - 1] += dcchars.SkillAdv[SC.PC.job - 1, T - 1] / 3;
                    }
                    SC.PC.skill[T - 1] += SkChart[P, 1];
                }
            }
            else if (dcchars.SkillAdv[SC.PC.job - 1, T - 1] < 0)
            {
                if (SC.PC.lvl % Math.Abs(dcchars.SkillAdv[SC.PC.job - 1, T - 1]) == 0)
                {
                    SC.PC.skill[T - 1]++;
                }
            }
        }

        //{Give spells to whoever qualifies for them.}
        if (SC.PC.skill[dcchars.SKILL_LearnSpell] > 0)
        {
            randchar.SelectPCSpells(SC.PC);
            texmaps.DisplayMap(SC.gb);
        }
    }

}

