using System;

class randchar
{
    //{This unit holds the character generator. That's it.}
    //{Originally, I had that procedure placed in dcchars,}
    //{but it was pretty big, so I decided to make a spinoff}
    //{unit just for it.}

    public static void SelectPCSpells(dcchars.DCChar PC)
    {
        //{The PC has just gone up a level. Choose some spells from}
        //{the appropriate list.
        const string msg = "You can learn new psi powers";
        const Crt.Color BColor = Crt.Color.LightGray;
        const Crt.Color IColor = Crt.Color.LightMagenta;
        const Crt.Color SColor = Crt.Color.Magenta;
        const int MX1 = 16;
        const int MY1 = 6;
        const int MX2 = 65;
        const int MY2 = 18;
        const int DY1 = 18;
        const int DY2 = 22;

        rpgmenus.RPGMenu RPM;

        //{Set up the screen.}
        rpgtext.GameMessage(msg, MX1, MY1 - 2, MX2, MY1, SColor, BColor);

        //{Determine the PC's spell college. If none, pick one at}
        //{random, just for the hey of it.}
        int CoP = dcchars.JobSchool[PC.job - 1];
        if (CoP == 0)
        {
            CoP = rpgdice.Random(1, spells.NumSchool + 1);
        }

        //{Keep selecting spells for as long as the PC needs to.}
        while (PC.skill[dcchars.SKILL_LearnSpell] > 0)
        {
            //{Create the spell menu.}
            RPM = rpgmenus.CreateRPGMenu(BColor, SColor, IColor, MX1, MY1, MX2, MY2);
            RPM.dx1 = MX1;
            RPM.dy1 = DY1;
            RPM.dx2 = MX2;
            RPM.dy2 = DY2;

            //{Add an item for each unlearned spell in the PC's}
            //{school. First, loop through all the spells up to}
            //{the PC's competency, which is (lvl + 1) div 2.}
            int Max = (PC.lvl + 1) / 2;
            if (Max > spells.NumLevel)
                Max = spells.NumLevel;
            for (int l = 1; l <= Max; ++l)
            {
                for (int t = 1; t <= 5; ++t)
                {
                    //{Does the PC already know this spell?}
                    if (spells.SpellCollege[CoP - 1, l - 1, t - 1] != 0 && spells.LocateSpellMem(PC.spell, spells.SpellCollege[CoP - 1, l - 1, t - 1]) == null)
                    {
                        //{Add a menu item for this spell.}
                        rpgmenus.AddRPGMenuItem(RPM, spells.SpellMan[spells.SpellCollege[CoP - 1, l - 1, t - 1] - 1].name, spells.SpellCollege[CoP - 1, l - 1, t - 1], spells.SpellMan[spells.SpellCollege[CoP - 1, l - 1, t - 1] - 1].Desc);
                    }
                }
            }

            //{If the menu is empty, leave immediately.}
            if (RPM.numItem == 0)
                return;

            //{Sort the menu.}
            rpgmenus.RPMSortAlpha(RPM);

            //{Select a spell from the menu. No canceling allowed!}
            int S = rpgmenus.SelectMenu(RPM, rpgmenus.RPMNoCancel);

            //{Add the selected spell to the PC's list.}
            spells.AddSpellMem(ref PC.spell, S);

            //{Decrement the PC's LearnSpell number.}
            PC.skill[dcchars.SKILL_LearnSpell] -= 1;
        }
    }

    const int NumSyllables = 50;
    static string[] SyllableList = new string[NumSyllables]
    {
            "Us","Ur","An","Ai","Pia","Ae","Ga","Nep","It","Er",
            "Jup","Sat","Plu","To","Ven","Ry","Gar","Del","Phi","Esc",
            "Any","Ron","Comp","Vul","Can","Ea","A","E","I","O",
            "U","Y","Cy","Ber","Tron","Nec","Ro","Mun","Da","Mon",
            "Heim","Tal","Larn","Cad","Ia","Tuo","Mas","Bis","Kup","Mor"
    };

    const int NumDesig = 15;
    static string[] DesigList = new string[NumDesig]
    {
            "II","III","IV","V","VI","III","IV","V",
            "Alpha","Beta","Gamma","Delta","Prime","Omega","Neo"
    };

    static string Syl()
    {
        return SyllableList[rpgdice.Random(0, NumSyllables)];
    }

    public static string RandomWorld()
    {
        //{Generate a random name for a planet.}

        //{A basic name is two syllables stuck together.}
        string it = Syl() + Syl().ToLower();

        //{Uncommon names may have 3 syllables.}
        if (rpgdice.Random(3) == 1 && it.Length < 6)
        {
            it += Syl().ToLower();
        }
        else if (rpgdice.Random(10) == 1)
        {
            it += Syl().ToLower();
        }

        //{Short names may have a second part. This isn't common.}
        if (it.Length < 8 && rpgdice.Random(23) == 7)
        {
            it += ' ' + Syl();
            if (rpgdice.Random(3) != 1)
                it += Syl().ToLower();
        }
        else if (rpgdice.Random(15) > it.Length)
        {
            it += ' ' + DesigList[rpgdice.Random(NumDesig)];
        }

        return it;
    }

    public static dcchars.DCChar RollNewChar()
    {
        //{We're going to generate a new game character from scratch.}
        //{Return NIL if the character creation process was cancelled.}
        const string instructions = "Select one of the avaliable jobs from the menu. Press ESC to reroll stats, or select Cancel to exit.";
        //var
        // pc: dccharptr;
        // opt: rpgmenuptr;	{The menu holding avaliable jobs.}
        // t,tt: Integer;		{Loop counters}
        // q: boolean;		{Apparently, for this procedure, I've forgotten about useful variable names. It's hot and I'm tired.}
        // I: DCItemPtr;

        int t, tt;

        //{Allocate memory for the character.}
        dcchars.DCChar PC = new dcchars.DCChar();

        //{Initilize Job to -1}
        PC.job = -1;

        //{Clear the screen}
        Crt.ClrScr();

        //{Display the stat names}
        Crt.TextColor(Crt.Color.Cyan);
        for (t = 1; t <= 8; ++t)
        {
            Crt.GotoXY(12, t * 2 + 3);
            Crt.Write(dcchars.StatName[t - 1] + " :");
        }

        //{Start a loop. We'll stay in the loop until a character is selected.}
        while (PC.job == -1)
        {
            //{Give a short message on how to use the character generator}
            rpgtext.GameMessage(instructions, 2, 1, 79, 4, Crt.Color.Green, Crt.Color.LightBlue);

            //{Set the text color}
            Crt.TextColor(Crt.Color.White);

            //{Roll the character's stats.}
            RollGHStats(PC, 100 + rpgdice.Random(20));
            for (t = 1; t <= 8; ++t)
            {
                //{display the stat onscreen.}
                Crt.GotoXY(35, t * 2 + 3);
                Crt.Write("   ");
                Crt.GotoXY(35, t * 2 + 3);
                Crt.Write(PC.stat[t - 1].ToString());
            }

            //{determine which jobs are open to this character, and}
            //{add them to our RPGMenu.}

            //{First, allocate the menu.}
            rpgmenus.RPGMenu opt = rpgmenus.CreateRPGMenu(Crt.Color.LightBlue, Crt.Color.Blue, Crt.Color.LightCyan, 46, 7, 65, 17);

            //{Initialize the description elements.}
            opt.dx1 = 2;
            opt.dx2 = 79;
            opt.dy1 = 20;
            opt.dy2 = 24;
            opt.dTexColor = Crt.Color.Green;

            for (t = 1; t <= dcchars.NumJobs; ++t)
            {
                //{Initialize q to true}
                bool q = true;

                //{Check each stat}
                for (tt = 1; tt <= 8; ++tt)
                {
                    if (PC.stat[tt - 1] < dcchars.JobStat[t - 1, tt - 1])
                    {
                        q = false;
                    }
                }

                //{If q is still true, this job may be chosen.}
                if (q)
                {
                    rpgmenus.AddRPGMenuItem(opt, dcchars.JobName[t - 1], t, dcchars.JobDesc[t - 1]);
                }
            }

            //{Get the jobs in alphabetical order}
            rpgmenus.RPMSortAlpha(opt);

            //{Add a CANCEL to the list}
            rpgmenus.AddRPGMenuItem(opt, "  Cancel", 0, null);

            //{Ask for a selection}
            PC.job = rpgmenus.SelectMenu(opt, rpgmenus.RPMNoCleanup);

            PC.m = null;
        }

        //{If the player selected cancel, dispose of the PC record.}
        if (PC.job == 0)
        {
            PC = null;
        }
        else
        {
            //{Copy skill ranks}
            for (t = 1; t <= dcchars.NumSkill; t++)
            {
                PC.skill[t - 1] = dcchars.JobSkill[PC.job - 1, t - 1];
            }

            //{Set HP, HPMax, and other initial values.}
            PC.HPMax = PC.stat[dcchars.STAT_Toughness] + dcchars.JobHitDie[PC.job - 1] + dcchars.BaseHP;
            PC.HP = PC.HPMax;
            PC.MPMax = PC.stat[dcchars.STAT_Willpower] / 2 + dcchars.JobMojoDie[PC.job - 1] + rpgdice.Random(dcchars.JobMojoDie[PC.job - 1]);
            PC.MP = PC.MPMax;
            PC.target = null;
            PC.carbs = 50;
            PC.lvl = 1;
            PC.XP = 0;
            PC.repCount = 0;

            PC.inv = null;
            for (t = 1; t <= dcchars.NumEquipSlots; ++t)
            {
                PC.eqp[t - 1] = null;
            }
            PC.SF = null;
            PC.spell = null;

            //{Give some basic equipment.}
            DoleEquipment(PC);

            //{Add the PC's meals.}
            for (t = 1; t <= 5; ++t)
            {
                dcitems.DCItem I = new dcitems.DCItem();
                I.ikind = dcitems.IKIND_Food;
                I.icode = JobXFood[PC.job, rpgdice.Random(10)];
                I.charge = 1;
                dcitems.MergeDCItem(ref PC.inv, I);
            }

            //{Add the PC's snacks.}
            int total = rpgdice.Random(5) + 1;
            for (t = 1; t <= total; ++t)
            {
                dcitems.DCItem I = new dcitems.DCItem();
                I.ikind = dcitems.IKIND_Food;

                //{Decide upon what kind of food to give, based on job.}
                if (rpgdice.Random(3) == 2)
                {
                    I.icode = JobXFood[0, rpgdice.Random(10)];
                }
                else
                {
                    I.icode = JobXFood[PC.job, rpgdice.Random(10)];
                }

                I.charge = rpgdice.Random(3) + 1;
                dcitems.MergeDCItem(ref PC.inv, I);
            }

            //{ Input a name. }
            rpgtext.GameMessage("NAME: ", 2, 1, 79, 4, Crt.Color.LightGreen, Crt.Color.LightBlue);
            Crt.GotoXY(9, 2);
            Crt.CursorOn();
            PC.name = rpgtext.ReadLine();
            Crt.CursorOff();

            if (PC.name != "")
            {
                //{ Generate an introduction. }
                IntroStory(PC);

                //{Add spells, if appropriate.}
                if (PC.skill[dcchars.SKILL_LearnSpell] > 0)
                {
                    SelectPCSpells(PC);
                }
            }
            else
            {
                PC = null;
            }
        }

        return PC;
    }

    public static int[,] JobXFood = new int[dcchars.NumJobs + 1, 10]
    {
        { 1, 2, 3, 4, 5, 6,22,23, 2, 1},	//{generic snack food}
		{ 9,10,11,12,13,14,15, 9,10,11},	//{Marine}
		{ 1,17,18,19,23,21,21,21,21,21},	//{Astral Seer}
		{ 5, 5, 7, 8,14,17,18,17,18,19},	//{Navigator}
		{16, 1, 3, 7, 7, 7, 8,10,12,12},	//{Hacker}
		{21,21,21,21,21,21,21,21,21,21},	//{Demon Hunter}
		{ 1, 4, 6, 7, 8,17,18,17,18,19},	//{Explorer}
		{ 2,14,15,14,15,14,15,14,15,19},	//{Samurai}
		{17,18,17,18,19,21,21,21,21,21},	//{Bounty Hunter}
		{ 6, 7, 8,17,18,10,11,12,13,19},	//{Pirate}
		{23,17,18,19, 6,21,21,21,21,21}		//{Zeomancer}
	};

    //{ The three indices are chart number, 10 items per chart, }
    //{ then ikind/icode/points }
    public static int[,,] ItemChart = new int[23, 10, 3]
    {
			//{ Rifles }
		{   { dcitems.IKIND_Gun , 2 ,  8 } , { dcitems.IKIND_Gun , 2 ,  8 } ,
            { dcitems.IKIND_Gun , 2 ,  8 } , { dcitems.IKIND_Gun , 3 , 12 } ,
            { dcitems.IKIND_Gun , 3 , 12 } , { dcitems.IKIND_Gun , 3 , 12 } ,
            { dcitems.IKIND_Gun , 9 , 11 } , { dcitems.IKIND_Gun , 9 , 11 } ,
            { dcitems.IKIND_Gun , 14 , 10 } , { dcitems.IKIND_Gun , 9 , 11 } },

			//{ Light Pistols }
		{   { dcitems.IKIND_Gun , 1 , 1 } , { dcitems.IKIND_Gun , 1 , 1 } ,
            { dcitems.IKIND_Gun , 1 , 1 } , { dcitems.IKIND_Gun , 4 , 4 } ,
            { dcitems.IKIND_Gun , 5 , 6 } , { dcitems.IKIND_Gun , 4 , 4 } ,
            { dcitems.IKIND_Gun , 6 , 3 } , { dcitems.IKIND_Gun , 6 , 3 } ,
            { dcitems.IKIND_Gun , 1 , 1 } , { dcitems.IKIND_Gun , 4 , 4 } },

			//{ Assault Weapons }
		{   { dcitems.IKIND_Gun , 7 , 16 } , { dcitems.IKIND_Gun , 7 , 16 } ,
            { dcitems.IKIND_Gun , 8 , 5 } , { dcitems.IKIND_Gun , 8 , 5 } ,
            { dcitems.IKIND_Gun , 10 , 15 } , { dcitems.IKIND_Gun , 10 , 15 } ,
            { dcitems.IKIND_Gun , 7 , 16 } , { dcitems.IKIND_Gun , 12 , 14  } ,
            { dcitems.IKIND_Gun , 5 , 6 } , { dcitems.IKIND_Gun , 8 , 5 } },

			//{ Light Close Combat Weapons }
		{   { dcitems.IKIND_Wep , 1 , 1 }  , { dcitems.IKIND_Wep , 1 , 1 },
            { dcitems.IKIND_Wep , 2 , 1 }  , { dcitems.IKIND_Wep , 2 , 1 },
            { dcitems.IKIND_Wep , 6 , 2 }  , { dcitems.IKIND_Wep , 6 , 2 },
            { dcitems.IKIND_Wep , 1 , 1 }  , { dcitems.IKIND_Wep , 15 , 1 },
            { dcitems.IKIND_Wep , 8 , 3 }  , { dcitems.IKIND_Wep , 9 , 7 } },

			//{ Heavy Close Combat Weapons }
		{   { dcitems.IKIND_Wep , 8 , 3 }  , { dcitems.IKIND_Wep , 8 , 3 },
            { dcitems.IKIND_Wep , 3 , 5 }  , { dcitems.IKIND_Wep , 4 , 5 },
            { dcitems.IKIND_Wep , 3 , 5 }  , { dcitems.IKIND_Wep , 4 , 5 },
            { dcitems.IKIND_Wep , 3 , 5 }  , { dcitems.IKIND_Wep , 4 , 5 },
            { dcitems.IKIND_Wep , 3 , 5 }  , { dcitems.IKIND_Wep , 4 , 5 } },

			//{ Grenades }
		{   { dcitems.IKIND_Grenade , 1 , 2 }  , { dcitems.IKIND_Grenade , 1 , 2 },
            { dcitems.IKIND_Grenade , 2 , 2 }  , { dcitems.IKIND_Grenade , 2 , 2 },
            { dcitems.IKIND_Grenade , 5 , 3 }  , { dcitems.IKIND_Grenade , 6 , 1 },
            { dcitems.IKIND_Grenade , 6 , 1 }  , { dcitems.IKIND_Grenade , 7 , 3 },
            { dcitems.IKIND_Grenade , 1 , 2 }  , { dcitems.IKIND_Grenade , 4 , 3 } },

			//{ Pills - Medicene }
		{   { dcitems.IKIND_Food , 24 , 1 }  , { dcitems.IKIND_Food , 25 , 1 },
            { dcitems.IKIND_Food , 25 , 1 }  , { dcitems.IKIND_Food , 26 , 1 },
            { dcitems.IKIND_Food , 25 , 1 }  , { dcitems.IKIND_Food , 30 , 1 },
            { dcitems.IKIND_Food , 25 , 1 }  , { dcitems.IKIND_Food , 30 , 1 },
            { dcitems.IKIND_Food , 25 , 1 }  , { dcitems.IKIND_Food , 30 , 1 } },

			//{ Pills - Ability Boosters }
		{   { dcitems.IKIND_Food , 26 , 1 }  , { dcitems.IKIND_Food , 27 , 1 },
            { dcitems.IKIND_Food , 26 , 1 }  , { dcitems.IKIND_Food , 27 , 1 },
            { dcitems.IKIND_Food , 37 , 2 }  , { dcitems.IKIND_Food , 27 , 1 },
            { dcitems.IKIND_Food , 38 , 2 }  , { dcitems.IKIND_Food , 36 , 2 },
            { dcitems.IKIND_Food , 39 , 2 }  , { dcitems.IKIND_Food , 39 , 2 } },

			//{ Headgear - Spacer }
		{   { dcitems.IKIND_Cap , 1 , 1 } , { dcitems.IKIND_Cap , 1 , 1 },
            { dcitems.IKIND_Cap , 2 , 2 } , { dcitems.IKIND_Cap , 2 , 2 },
            { dcitems.IKIND_Cap , 2 , 2 } , { dcitems.IKIND_Cap , 2 , 2 },
            { dcitems.IKIND_Cap , 2 , 2 } , { dcitems.IKIND_Cap , 2 , 2 },
            { dcitems.IKIND_Cap , 2 , 2 } , { dcitems.IKIND_Cap , 2 , 2 } },

			//{ Headgear - Fighter }
		{   { dcitems.IKIND_Cap , 1 , 1 } , { dcitems.IKIND_Cap , 1 , 1 },
            { dcitems.IKIND_Cap , 2 , 2 } , { dcitems.IKIND_Cap , 3 , 2 },
            { dcitems.IKIND_Cap , 3 , 2 } , { dcitems.IKIND_Cap , 3 , 2 },
            { dcitems.IKIND_Cap , 3 , 2 } , { dcitems.IKIND_Cap , 3 , 2 },
            { dcitems.IKIND_Cap , 3 , 2 } , { dcitems.IKIND_Cap , 5 , 2 } },

			//{ Headgear - Samurai }
		{   { dcitems.IKIND_Cap , 5 , 2 } , { dcitems.IKIND_Cap , 5 , 2 },
            { dcitems.IKIND_Cap , 5 , 2 } , { dcitems.IKIND_Cap , 5 , 2 },
            { dcitems.IKIND_Cap , 5 , 2 } , { dcitems.IKIND_Cap , 5 , 2 },
            { dcitems.IKIND_Cap , 5 , 2 } , { dcitems.IKIND_Cap , 5 , 2 },
            { dcitems.IKIND_Cap , 5 , 2 } , { dcitems.IKIND_Cap , 5 , 2 } },

			//{ Armor - Spacer }
		{   { dcitems.IKIND_Armor , 2 , 2 } , { dcitems.IKIND_Armor , 2 , 2 },
            { dcitems.IKIND_Armor , 2 , 2 } , { dcitems.IKIND_Armor , 2 , 2 },
            { dcitems.IKIND_Armor , 2 , 2 } , { dcitems.IKIND_Armor , 2 , 2 },
            { dcitems.IKIND_Armor , 2 , 2 } , { dcitems.IKIND_Armor , 2 , 2 },
            { dcitems.IKIND_Armor , 2 , 2 } , { dcitems.IKIND_Armor , 2 , 2 } },

			//{ Armor - Soldier }
		{   { dcitems.IKIND_Armor , 3 , 4 } , { dcitems.IKIND_Armor , 3 , 4 },
            { dcitems.IKIND_Armor , 3 , 4 } , { dcitems.IKIND_Armor , 3 , 4 },
            { dcitems.IKIND_Armor , 3 , 4 } , { dcitems.IKIND_Armor , 3 , 4 },
            { dcitems.IKIND_Armor , 3 , 4 } , { dcitems.IKIND_Armor , 3 , 4 },
            { dcitems.IKIND_Armor , 3 , 4 } , { dcitems.IKIND_Armor , 8 , 6 } },

			//{ Armor - Civilian }
		{   { dcitems.IKIND_Armor , 1 , 0 } , { dcitems.IKIND_Armor , 1 , 0 },
            { dcitems.IKIND_Armor , 1 , 0 } , { dcitems.IKIND_Armor , 10 , 7 },
            { dcitems.IKIND_Armor , 1 , 0 } , { dcitems.IKIND_Armor , 2 , 2 },
            { dcitems.IKIND_Armor , 1 , 0 } , { dcitems.IKIND_Armor , 3 , 4 },
            { dcitems.IKIND_Armor , 1 , 0 } , { dcitems.IKIND_Armor , 4 , 0 } },

			//{ Armor - Samurai / Hunter }
		{   { dcitems.IKIND_Armor , 5 , 4 } , { dcitems.IKIND_Armor , 7 , 7 },
            { dcitems.IKIND_Armor , 5 , 4 } , { dcitems.IKIND_Armor , 7 , 7 },
            { dcitems.IKIND_Armor , 5 , 4 } , { dcitems.IKIND_Armor , 7 , 7 },
            { dcitems.IKIND_Armor , 5 , 4 } , { dcitems.IKIND_Armor , 7 , 7 },
            { dcitems.IKIND_Armor , 5 , 4 } , { dcitems.IKIND_Armor , 8 , 6 } },

			//{ Armor - Merc }
		{   { dcitems.IKIND_Armor , 3 , 4 } , { dcitems.IKIND_Armor , 8 , 6 },
            { dcitems.IKIND_Armor , 3 , 4 } , { dcitems.IKIND_Armor , 8 , 6 },
            { dcitems.IKIND_Armor , 3 , 4 } , { dcitems.IKIND_Armor , 8 , 6 },
            { dcitems.IKIND_Armor , 3 , 4 } , { dcitems.IKIND_Armor , 8 , 6 },
            { dcitems.IKIND_Armor , 7 , 7 } , { dcitems.IKIND_Armor , 8 , 6 } },

			//{ Armor - Magus }
		{   { dcitems.IKIND_Armor , 4 , 0 } , { dcitems.IKIND_Armor , 1 , 0 },
            { dcitems.IKIND_Armor , 4 , 0 } , { dcitems.IKIND_Armor , 1 , 0 },
            { dcitems.IKIND_Armor , 4 , 0 } , { dcitems.IKIND_Armor , 1 , 0 },
            { dcitems.IKIND_Armor , 4 , 0 } , { dcitems.IKIND_Armor , 2 , 2 },
            { dcitems.IKIND_Armor , 4 , 0 } , { dcitems.IKIND_Armor , 9 , 9 } },

			//{ Gloves - Spacer }
		{   { dcitems.IKIND_Glove , 1 , 1 } , { dcitems.IKIND_Glove , 1 , 1 },
            { dcitems.IKIND_Glove , 1 , 1 } , { dcitems.IKIND_Glove , 1 , 1 },
            { dcitems.IKIND_Glove , 1 , 1 } , { dcitems.IKIND_Glove , 1 , 1 },
            { dcitems.IKIND_Glove , 1 , 1 } , { dcitems.IKIND_Glove , 1 , 1 },
            { dcitems.IKIND_Glove , 1 , 1 } , { dcitems.IKIND_Glove , 1 , 1 } },

			//{ Gloves - Combat }
		{   { dcitems.IKIND_Glove , 1 , 1 } , { dcitems.IKIND_Glove , 2 , 2 },
            { dcitems.IKIND_Glove , 2 , 2 } , { dcitems.IKIND_Glove , 2 , 2 },
            { dcitems.IKIND_Glove , 2 , 2 } , { dcitems.IKIND_Glove , 2 , 2 },
            { dcitems.IKIND_Glove , 2 , 2 } , { dcitems.IKIND_Glove , 2 , 2 },
            { dcitems.IKIND_Glove , 2 , 2 } , { dcitems.IKIND_Glove , 3 , 4 } },

			//{ Shoes - Nice }
		{   { dcitems.IKIND_Shoe , 2 , 0 } , { dcitems.IKIND_Shoe , 4 , 0 },
            { dcitems.IKIND_Shoe , 2 , 0 } , { dcitems.IKIND_Shoe , 4 , 0 },
            { dcitems.IKIND_Shoe , 2 , 0 } , { dcitems.IKIND_Shoe , 3 , 1 },
            { dcitems.IKIND_Shoe , 2 , 0 } , { dcitems.IKIND_Shoe , 3 , 1 },
            { dcitems.IKIND_Shoe , 1 , 1 } , { dcitems.IKIND_Shoe , 3 , 1 } },

			//{ Shoes - Mean }
		{   { dcitems.IKIND_Shoe , 1 , 1 } , { dcitems.IKIND_Shoe , 1 , 1 },
            { dcitems.IKIND_Shoe , 1 , 1 } , { dcitems.IKIND_Shoe , 1 , 1 },
            { dcitems.IKIND_Shoe , 1 , 1 } , { dcitems.IKIND_Shoe , 3 , 1 },
            { dcitems.IKIND_Shoe , 1 , 1 } , { dcitems.IKIND_Shoe , 3 , 1 },
            { dcitems.IKIND_Shoe , 1 , 1 } , { dcitems.IKIND_Shoe , 3 , 1 } },

			//{ *VERY* Heavy Close Combat Weapons }
		{   { dcitems.IKIND_Wep , 10 , 9 }  , { dcitems.IKIND_Wep , 10 , 9 },
            { dcitems.IKIND_Wep , 10 , 9 }  , { dcitems.IKIND_Wep , 12 , 15 },
            { dcitems.IKIND_Wep , 11 , 12 }  , { dcitems.IKIND_Wep , 12 , 15 },
            { dcitems.IKIND_Wep , 11 , 12 }  , { dcitems.IKIND_Wep , 12 , 15 },
            { dcitems.IKIND_Wep , 11 , 12 }  , { dcitems.IKIND_Wep , 12 , 15 } },

			//{ Warrior Gear }
		{   { dcitems.IKIND_Wep , 6 , 2 } , { dcitems.IKIND_Grenade , 1 , 2 },
            { dcitems.IKIND_Grenade , 2 , 2 } , { dcitems.IKIND_Food , 39 , 2 },
            { dcitems.IKIND_Food , 36 , 2 }, { dcitems.IKIND_Food , 38 , 2 },
            { dcitems.IKIND_Gun , 4 , 4 }, { dcitems.IKIND_Gun , 5 , 6 },
            { dcitems.IKIND_Gun , 1 , 1 }, { dcitems.IKIND_Grenade , 3 , 3 } }

    };

    public static int[] HatsChart = new int[dcchars.NumJobs]
    {
        10 , 9 , 9 , 9 , 11 , 9 , 11 , 10 , 9 , 10
    };

    public static int[] HatsChance = new int[dcchars.NumJobs]
    {
        50, 3, 15, 25, 75, 100, 60, 80, 45, 2
    };

    public static int[] ArmorChart = new int[dcchars.NumJobs]
    {
        13, 17, 12, 14, 15, 12, 15, 16, 13, 14
    };

    public static int[] GloveChart = new int[dcchars.NumJobs]
    {
        19 , 18 , 18 , 18 , 19 , 18 , 19 , 19 , 19 , 18
    };

    public static int[] GloveChance = new int[dcchars.NumJobs]
    {
        40, 1, 10, 15, 55, 100, 10, 50, 55, 2
    };

    public static int[] ShoeChart = new int[dcchars.NumJobs]
    {
        21 , 20 , 20 , 20 , 20 , 21 , 21 , 21 , 21 , 20
    };

    public static int[,] ExtraGear = new int[dcchars.NumJobs + 1, 10]
    {
       { 6 , 6 , 6 , 6 , 7 , 7 , 7 , 7 , 7 , 8 },    //{Default - 50%}
       { 6 , 6 , 6 , 6 , 6 , 6 , 7 , 8 , 23 , 23 },  //{Marine}
       { 7 , 7 , 7 , 7 , 7 , 7 , 7 , 7 , 8 , 8 },    //{Astral Seer}
       { 6 , 6 , 6 , 6 , 6 , 7 , 7 , 7 , 8 , 8 },    //{Navigator}
       { 6 , 6 , 7 , 7 , 7 , 7 , 8 , 8 , 8 , 8 },    //{Hacker}
       { 3 , 5 , 6 , 6 , 6 , 6 , 6 , 7 , 7 , 8 },    //{Demon Hunter}
       { 6 , 6 , 6 , 6 , 7 , 7 , 7 , 7 , 7 , 8 },    //{Explorer}
       { 6 , 6 , 6 , 6 , 6 , 7 , 7 , 8 , 8 , 23 },   //{Samurai}
       { 6 , 6 , 6 , 6 , 6 , 7 , 8 , 8 , 23 , 23 },  //{Bounty Hunter}
       { 6 , 6 , 6 , 6 , 7 , 7 , 7 , 23 , 23 , 23 }, //{Pirate}
       { 7 , 7 , 7 , 7 , 7 , 7 , 7 , 7 , 7 , 8 }     //{Zeomancer}
    };

    public static int[,] SecondaryGear = new int[dcchars.NumJobs, 10]
    {
        { 2 , 4 , 5 , 5 , 5 , 5 , 5 , 5 , 5 , 6 },    //{Marine}
    	{ 4 , 4 , 4 , 4 , 4 , 4 , 4 , 4 , 5 , 5 },    //{Astral Seer}
		{ 2 , 4 , 4 , 4 , 5 , 5 , 5 , 5 , 5 , 5 },    //{Navigator}
		{ 4 , 4 , 4 , 4 , 4 , 4 , 4 , 5 , 5 , 5 },    //{Hacker}
		{ 5 , 5 , 5 , 5 , 5 , 5 , 5 , 22 , 22 , 22 }, //{Demon Hunter}
		{ 2 , 4 , 4 , 4 , 4 , 4 , 5 , 5 , 6 , 6 },    //{Explorer}
		{ 2 , 3 , 4 , 6 , 23 , 6 , 7 , 8 , 23 , 1 },  //{Samurai - Since Samurai start the game with a Katana, their secondary gear isn't nessecarily a second weapon. }
		{ 1 , 2 , 2 , 2 , 2 , 2 , 4 , 4 , 5 , 6 },    //{Bounty Hunter}
		{ 2 , 3 , 4 , 4 , 5 , 5 , 5 , 5 , 5 , 22 },   //{Pirate}
		{ 4 , 4 , 4 , 4 , 4 , 4 , 4 , 4 , 4 , 6 }     //{Zeomancer}
	};

    public static int[,] PrimaryGear = new int[dcchars.NumJobs, 10]
    {
        { 1 , 1 , 1 , 1 , 1 , 1 , 1 , 3 , 3 , 3 }, //{Marine}
        { 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 }, //{Astral Seer}
        { 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 2 , 2 }, //{Navigator}
        { 1 , 1 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 }, //{Hacker}
        { 1 , 1 , 1 , 1 , 1 , 2 , 2 , 2 , 3 , 3 }, //{Demon Hunter}
        { 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 2 , 2 }, //{Explorer}
        { 1 , 1 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 }, //{Samurai}
        { 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 3 , 3 }, //{Bounty Hunter}
        { 1 , 1 , 1 , 1 , 1 , 1 , 2 , 2 , 3 , 3 }, //{Pirate}
        { 1 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 }, //{Zeomancer}
    };


    static void StashItem(dcchars.DCChar PC, dcitems.DCItem it)
    {
        //{ The new PC has been given an item. Decide where to stick it, }
        //{ and provide whatever accessories it has coming. }

        if (it.ikind == dcitems.IKIND_Gun)
        {
            //{The weapon starts out fully loaded.}
            it.charge = dcitems.CGuns[it.icode - 1].magazine;

            //{Give the player some ammo for the gun.}
            dcitems.DCItem I = new dcitems.DCItem();
            I.ikind = dcitems.IKIND_Ammo;
            I.icode = dcitems.CGuns[it.icode - 1].caliber;

            //{Decide how many bullets to dole out.}
            if (dcitems.CGuns[it.icode - 1].caliber >= dcitems.CAL_Energy)
            {
                //{Energy, napalm, and other special ammo weapons}
                //{don't get as many reloads.}
                I.charge = 3 + rpgdice.Random(6);
            }
            else if (it.icode == 9 || it.icode == 14)
            {
                //{ Ammo for a shotgun. }
                I.charge = dcitems.CGuns[it.icode - 1].magazine * 2 + rpgdice.Random(10);
            }
            else
            {
                I.charge = dcitems.CGuns[it.icode - 1].magazine;
                if (I.charge < 10)
                    I.charge = 10;
                I.charge = (I.charge * 3) + rpgdice.Random(20);
            }

            dcitems.MergeDCItem(ref PC.inv, I);

            //{ For shotguns, include some scatter ammunition. }
            if (it.icode == 9 || it.icode == 14)
            {
                I = new dcitems.DCItem();
                I.ikind = dcitems.IKIND_Ammo;
                I.icode = 100 + dcitems.CGuns[it.icode - 1].caliber;
                I.charge = dcitems.CGuns[it.icode - 1].magazine + rpgdice.Random(10);
                dcitems.MergeDCItem(ref PC.inv, I);
            }
        }

        //{ If equippable, equip the item. Otherwise, stash it. }
        if (it.ikind > 0 && PC.eqp[it.ikind - 1] == null)
        {
            PC.eqp[it.ikind - 1] = it;
        }
        else
        {
            dcitems.MergeDCItem(ref PC.inv, it);
        }
    }

    static void PickItemFromChart(dcchars.DCChar PC, int Chart, ref int pts)
    {
        //{ Select an item from one of the starting equipment charts, }
        //{ stash it in the PC's inventory, then decrement PTS by an }
        //{ appropriate amount. }

        //{ Decide what item from the chart to generate. }
        int N = rpgdice.Random(10);

        //{ Actually create the item record. }
        dcitems.DCItem I = new dcitems.DCItem();
        I.ikind = ItemChart[Chart - 1, N, 0];
        I.icode = ItemChart[Chart - 1, N, 1];
        pts -= ItemChart[Chart - 1, N, 2];

        //{ Stick the item in the PC's inventory. }
        StashItem(PC, I);
    }

    static void GiveBasicStuff(dcchars.DCChar PC)
    {
        //{ All PCs start with a few free items. }

        //{ 3 Trauma Fixes, 3 Antidotes. }
        dcitems.DCItem I = new dcitems.DCItem();
        I.ikind = dcitems.IKIND_Food;
        I.icode = 25;
        I.charge = 3;
        StashItem(PC, I);

        I = new dcitems.DCItem();
        I.ikind = dcitems.IKIND_Food;
        I.icode = 30;
        I.charge = 3;
        StashItem(PC, I);
    }

    static void GiveKatana(dcchars.DCChar PC)
    {
        //{ All Samurai start the game with a Katana. }
        dcitems.DCItem I = new dcitems.DCItem();
        I.ikind = dcitems.IKIND_Wep;
        I.icode = 5;
        StashItem(PC, I);
    }

    static void DoleEquipment(dcchars.DCChar PC)
    {
        //{ Give out starting equipment to the PC based on job and luck. }

        //   { First, determine how many points the character will get }
        //{ for generation. }
        int Pts = 25 + rpgdice.Random(PC.stat[dcchars.STAT_Luck]);

        //{ Generate the needed equipment - primary weapon, clothes, }
        //{ and shoes. }

        //{ If the character is a Samurai, give him his Katana now. }
        if (PC.job == 7)
        {
            Pts = Pts - 12;
            GiveKatana(PC);
        }

        //{ All characters get some free stuff. Add that now. }
        GiveBasicStuff(PC);

        //{ Primary Weapon - decide which chart to use. }
        int N = rpgdice.Random(10);
        PickItemFromChart(PC, PrimaryGear[PC.job - 1, N], ref Pts);

        //{ Clothes and Shoes }
        PickItemFromChart(PC, ArmorChart[PC.job - 1], ref Pts);
        PickItemFromChart(PC, ShoeChart[PC.job - 1], ref Pts);

        //{ If there are points left over, give secondary equipment. }
        if (Pts > 0)
        {
            N = rpgdice.Random(10);
            PickItemFromChart(PC, SecondaryGear[PC.job - 1, N], ref Pts);
        }

        //{ Roll to see if this character gets gloves or a hat. }
        if (Pts > 0 && rpgdice.Random(100) < HatsChance[PC.job - 1])
            PickItemFromChart(PC, HatsChart[PC.job - 1], ref Pts);
        if (Pts > 0 && rpgdice.Random(100) < GloveChance[PC.job - 1])
            PickItemFromChart(PC, GloveChart[PC.job - 1], ref Pts);

        //{ Spend the remaining points on tertiary equipment. }
        while (Pts > 0)
        {
            //{ Extra equipment has a 50% chance of coming from the }
            //{ job-specific chart and a 50% chance of coming from }
            //{ the general items chart. }
            if (rpgdice.Random(2) == 1)
            {
                N = rpgdice.Random(10);
                PickItemFromChart(PC, ExtraGear[PC.job, N], ref Pts);
            }
            else
            {
                N = rpgdice.Random(10);
                PickItemFromChart(PC, ExtraGear[0, N], ref Pts);
            }
        }
    }

    const int NumFrm = 3;
    const int NumEco = 8;
    const int NumFun = 6;
    static string[] frm = new string[NumFrm]
    {
        "planet","station","moon"
    };

    static string[] fun = new string[NumFun]
    {
        "an agricultural","an industrial","a military",
        "a trade","a holiday","an administrative"
    };

    static string[] eco = new string[NumEco]
    {
        "a jungle","an ice","a desert","a barely habitable",
        "a heavily forested","a beautiful and pristene",
        "an oceanic","a barren"
    };

    static string[] loc = new string[10]
    {
        "Western","North Western","South Western","Northern",
        "Southern","North Eastern","South Eastern","Eastern",
        "Imperial","Coreward"
    };


    static string RandomWDesc()
    {
        //{Return a random world description.}
        //{The description has three parts- first, is it a planet,}
        //{a moon, or a space station? Secondly, does the world have}
        //{a predominant ecosystem or industry? Finally, where in the}
        //{galaxy is it located?}
        string it = "";

        //{First, decide upon the form of the world. 90% will be planets.}
        int F = 0;
        if (rpgdice.Random(10) == 1)
        {
            F = rpgdice.Random(NumFrm);
        }

        //{Next, decide whether or not the world has a predominant}
        //{characteristic. 50% of planets don't have one.}
        if (F == 0 && rpgdice.Random(2) == 1)
        {
            //{no primary characteristic.}
            it = "a " + frm[F];
        }
        else
        {
            if (F == 1 || rpgdice.Random(3) == 1)
            {
                it = fun[rpgdice.Random(NumFun)] + " " + frm[F];
            }
            else
            {
                it = eco[rpgdice.Random(NumEco)] + " " + frm[F];
            }
        }

        it += " in the " + loc[rpgdice.Random(10)] + " Stellar March";

        return it;
    }

    static string DCall = " There could be people in danger. Immediately, you altered course for the station.";
    static string Pir = " This might be a good oppurtunity to claim some salvage. You altered course to the station.";
    static string Mys = " Perhaps fate is calling you in this direction. All other plans can wait; you altered course to the station.";
    static string Harbor = " Navcomp indicated a friendly space station nearby. You altered course for Dead Cold.";
    const int NumAtt = 3;
    static string[] Att = new string[NumAtt] { "pirates", "the bugs", "raiders" };
    const int NumCar = 3;
    static string[] Car = new string[NumCar]
    {
        "the body of your mentor. It was his request that interrment be performed here.",
        "a shipment of fresh produce from the Coreward sector.",
        "the fallen from your homeworld, slain when the bugs attacked."
    };

    static string RandomArrival(dcchars.DCChar PC)
    {
        //{Print a story explaining how the character arrived at Dead Cold.}
        //{There are three possible paths- the PC could have responded to}
        //{the station's defense call, the PC could have happened here by}
        //{accident, or the PC could have come here willingly.}
        string it = "";

        //{Decide which of the three paths the PC will take.}
        int P = rpgdice.Random(3);
        if (P == 0)
        {
            //{Distress call}
            it = "While travelling to " + RandomWorld() + ", your ship recieved a distress call from space station Dead Cold.";
            if (PC.job == 4 || PC.job == 9)
            {
                if (rpgdice.Random(3) == 1)
                    it += Pir;
                else
                    it += DCall;
            }
            else if (PC.job == 2 || PC.job == 5)
            {
                if (rpgdice.Random(3) == 1)
                    it += Mys;
                else
                    it += DCall;
            }
            else
            {
                if (rpgdice.Random(20) == 7)
                {
                    it += Mys;
                }
                else if (rpgdice.Random(20) == 13)
                {
                    it += Pir;
                }
                else
                {
                    it += DCall;
                }
            }
        }
        else if (P == 1)
        {
            //{Accidental arrival}
            it = "While travelling to " + RandomWorld() + ", your ship ";
            if (rpgdice.Random(2) == 1)
            {
                //{Attacked!}
                it += "was attacked by " + Att[rpgdice.Random(NumAtt)] + " and seriously damaged.";
            }
            else if (rpgdice.Random(2) == 1)
            {
                //{Meteor!}
                it += "was hit by a meteor.";
            }
            else
            {
                //{Out of gas!}
                it += "started to leak fuel.";
            }

            it += Harbor;
        }
        else
        {
            //{Purposeful arrival}
            it = "You have come to Dead Cold bearing " + Car[rpgdice.Random(NumCar)];
        }

        return it;
    }

    static void IntroStory(dcchars.DCChar PC)
    {
        //{Create an introductory story for the PC.}
        //{Display it in a special message window.}
        string Or1 = "You are from ";
        string Or2 = ", ";
        string Or3 = ".";
        string Ar2 = "Oddly, the station gave no response to your docking request. You pull into an open shuttle bay and prepare to disembark.";
        const int X1 = 15;
        const int Y1 = 5;
        const int X2 = 65;
        const int Y2 = 20;

        //{Set up the screen.}
        Crt.ClrScr();
        rpgtext.LovelyBox(Crt.Color.LightBlue, X1, Y1, X2, Y2);
        Crt.Window(X1 + 1, Y1 + 1, X2 - 1, Y2 - 1);
        Crt.TextColor(Crt.Color.Green);

        //{Generate the PC's origin.}
        PC.bgOrigin = Or1 + RandomWorld() + Or2 + RandomWDesc() + Or3;

        //{Generate the PC's history.}

        //{Generate the PC's reason for arriving at DeadCold.}
        PC.bgArrival = RandomArrival(PC);

        //{Generate an introduction to the station.}

        //{Print the information.}
        rpgtext.Delineate(PC.bgOrigin, X2 - X1 - 1, 1);
        if (Crt.WhereX() != 1)
            Crt.Write('\n');
        Crt.Write('\n');

        rpgtext.Delineate(PC.bgArrival, X2 - X1 - 1, 1);
        if (Crt.WhereX() != 1)
            Crt.Write('\n');
        Crt.Write('\n');

        rpgtext.Delineate(Ar2, X2 - X1 - 1, 1);

        rpgtext.RPGKey();

        Crt.Window(1, 1, WDM.CON_WIDTH, WDM.CON_HEIGHT);
        Crt.ClrScr();
    }

    static void RollGHStats(dcchars.DCChar PC, int Pts)
    {
        //{ Randomly allocate PTS points to all of the character's }
        //{ stats using the same basic method as in my other game, GearHead. }
        //{ *** NOTE: IF THE CHAR ALREADY HAD STAT VALUES SET, THESE WILL BE LOST *** }

        //{ Error Check - Is this a character!? }
        if (PC == null)
            return;

        int t;

        //{ Set all stat values to minimum. }
        for (t = 0; t < 8; ++t)
        {
            PC.stat[t] = 1;
        }
        Pts -= 8;

        //{ Keep processing until we run out of stat points to allocate. }
        while (Pts > 0)
        {
            //{ T will now point to the stat slot to improve. }
            t = rpgdice.Random(8);

            //{ If the stat selected is under the max value, }
            //{ improve it. If it is at or above the max value, }
            //{ there's a one in three chance of improving it. }
            if (PC.stat[t] < 15)
            {
                PC.stat[t] += 1;
                Pts -= 1;
            }
            else if (rpgdice.Random(2) == 1)
            {
                PC.stat[t] += 1;
                Pts -= 1;
            }
        }
    }
}
