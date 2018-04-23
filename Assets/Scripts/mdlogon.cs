using System;

public class mdlogon
{
    /*When the PC accesses a computer terminal, this procedure*/
    /*is the one that's called.*/

    /*md stands for MajorDomo, the chief AI on DeadCold. It does not*/
    /*mean "Most Dangerous".*/

    public static void MDSession(gamebook.Scenario SC, texmodel.Model M)
    {
        /* A computer session has two component windows: The metacontrol */
        /* window, in the lower right of the screen, and the main display */
        /* window in the center. */

        /* Set up the display. */
        texmaps.ClearMapArea();
        rpgtext.LovelyBox(Crt.Color.White, UCM_X1 - 1, UCM_Y1 - 1, UCM_X2 + 1, UCM_Y2 + 1);

        /* Find the computer we want. */
        cwords.MPU MP = SC.Comps;
        while (MP != null && MP.M != M)
            MP = MP.next;

        if (MP == null)
            return;

        /* Tell the player what he's doing. */
        rpgtext.DCGameMessage("Using " + cwords.MPUMan[MP.kind - 1].name + ".");

        /* Create MetaControl Menu */
        rpgmenus.RPGMenu MCM = rpgmenus.CreateRPGMenu(Crt.Color.LightGray, Crt.Color.Blue, Crt.Color.Cyan, MCM_X1, MCM_Y1, MCM_X2, MCM_Y2);
        rpgmenus.AddRPGMenuItem(MCM, "Access Terminal", 1);
        rpgmenus.AddRPGMenuItem(MCM, "Hack Logon System", 2);
        rpgmenus.AddRPGMenuItem(MCM, "Disconnect", -1);

        /* Initialize Security Clearance. */
        int Sec = 0;

        /* Start the main access loop. */
        int N = -1;
        do
        {
            /* Start with the user terminal itself. */
            rpgmenus.DisplayMenu(MCM);
            DoUserTerminal(SC, MP, Sec);

            /* Once the user terminal is exited, access metacontrol. */
            do
            {
                N = rpgmenus.SelectMenu(MCM, rpgmenus.RPMNoCleanup);

                /* If the player wants to make a hacking attempt, do that here. */
                if (N == 2)
                    AttemptHack(SC, MP, ref Sec);
            }
            while (N == 2);
        }
        while (N != -1);

        texmaps.DisplayMap(SC.gb);
    }

    public const int NumMapRow = 11;

    public static string[] StationMap = new string[NumMapRow]
    {
        /* Station Map */
        "   --- A ---",
        "  /    |    \\",
        " B     |     H",
        "/      #      \\",
        "|      #      |",
        "C    J-I-K    G",
        "|      #      |",
        "\\      #      /",
        " D     |     F",
        "  \\    |    /",
        "   --- E ---"
    };

    public const int NumNamedLevels = 11;
    public static int[,] LevelLoc = new int[NumNamedLevels, 2]
    {
        {8,1},{2,3},{1,6},{2,9},{8,11},{14,9},{15,6},{14,3},{8,6},{6,6},{10,6}
    };

    public static string[] LevelName = new string[NumNamedLevels]
    {
        "A - Primary Research Module",
        "B - Monument Construction",
        "C - Visitor Center & Dock",
        "D - Theistic Services",
        "E - Forensic Laboratories",
        "F - Compsec & Life Support",
        "G - Cold Storage Module",
        "H - Processing & Industry",
        "I - Operations Control",
        "J - Deep Orbit Massdriver",
        "K - Navigational Control",
    };


    public static string[] LevelDesc = new string[NumNamedLevels]
    {
        "Deadcold is the galaxy's foremost leader in the science of necrology. In this module, our scientists work hard to keep our edge.",
        "In this module lasting tributes to those who have passed on are created. Our workshops are equipped to create everything from marble tombs to self-sustaining cryogenic probes.",
        "This module is our visitor's gateway to DeadCold. There are chapels, lounges, and bereavement counselors to welcome you to our station.",
        "DeadCold is proud to offer funerary rites according to over five hundred different planetary traditions. This module houses our denominational offices and chapels, as well as the DeadCold Cortege Museum.",
        "Our forensic research facilities are the best in the Eastmost Stellar March. Currently we are examining bug corpses obtained from the western Aradar war. Research has developed several new weapons which may prove particularly effective.",
        "This module houses the machinery which keeps our station running. It is also the station's primary residential area, featuring the majority of the crew quarters.",
        "Located on the opposite side of the station from the visitor center, this module features DeadCold's other docking bay. This is where our unliving clients first enter the station, and where they are kept until ready for processing.",
        "We are proud to offer a wide range of interrment options. This module holds the tools and machinery needed for embalming, cremation, preservation, excarnation, reprocessing, and harvesting.",
        "At the center of the station ring is DeadCold control center. Most administrative functions have been completely automated, requiring only minimal human supervision.",
        "The station mass driver is powered by a 120Gz gravitic coil. It can be used to place caskets in deep orbit burial, ship materials to nearby planets such as Mascan and Denoles, or to make corrections to the station's orbit.",
        "This module houses the thrusters and verniers which help keep DeadCold in a stable orbit. It is also sometimes necessary to move the station, as solar flares and meteor storms could seriously damage our solar arrays."
    };

    public const int UCM_X1 = 5;
    public const int UCM_Y1 = 7;
    public const int UCM_X2 = 53;
    public const int UCM_Y2 = 22;

    public const int MCM_X1 = 55;
    public const int MCM_Y1 = 14;
    public const int MCM_X2 = 77;
    public const int MCM_Y2 = 22;

    static void ClearUCM()
    {
        /* Cls on the UCM zone described above. */
        Crt.Window(UCM_X1, UCM_Y1, UCM_X2, UCM_Y2);
        Crt.ClrScr();
        Crt.Window(1, 1, 80, 25);
    }

    static void DoMapDisplay()
    {
        /* This procedure handles the map displayer. */
        /* Create the menu. */
        rpgmenus.RPGMenu MM = rpgmenus.CreateRPGMenu(Crt.Color.Blue, Crt.Color.Green, Crt.Color.LightGreen, UCM_X1 + 19, UCM_Y1 + 1, UCM_X2 - 1, UCM_Y2 - 1);
        for (int t = 1; t <= NumNamedLevels; ++t)
            rpgmenus.AddRPGMenuItem(MM, LevelName[t - 1], t);

        rpgmenus.AddRPGMenuItem(MM, "  Exit", -1);

        /* Display the map itself */
        ClearUCM();
        Crt.Window(UCM_X1, UCM_Y1, UCM_X2, UCM_Y2);
        Crt.TextColor(Crt.Color.Green);
        for (int t = 1; t <= NumMapRow; ++t)
        {
            Crt.GotoXY(3, t + 2);
            Crt.Write(StationMap[t - 1]);
        }
        Crt.Window(1, 1, 80, 25);

        /* Enter the main loop. Keep processing until an Exit is recieved. */
        int N = -1;
        do
        {
            N = rpgmenus.SelectMenu(MM, rpgmenus.RPMNormal);

            if (N != -1)
            {
                Crt.GotoXY(UCM_X1 + LevelLoc[N - 1, 0] + 1, UCM_Y1 + LevelLoc[N - 1, 1] + 1);
                Crt.TextColor(Crt.Color.Yellow);
                Crt.Write(LevelName[N - 1][0]);

                rpgtext.GameMessage(LevelDesc[N - 1], UCM_X1 + 19, UCM_Y1 + 1, UCM_X2 - 1, UCM_Y2 - 1, Crt.Color.Green, Crt.Color.Blue);
                rpgtext.RPGKey();

                Crt.GotoXY(UCM_X1 + LevelLoc[N - 1, 0] + 1, UCM_Y1 + LevelLoc[N - 1, 1] + 1);
                Crt.TextColor(Crt.Color.Green);
                Crt.Write(LevelName[N - 1][1]);
            }
        }
        while (N != -1);
    }

    static void PrintCap(string msg)
    {
        Crt.Window(UCM_X1, UCM_Y1, UCM_X2, UCM_Y1 + 2);
        Crt.ClrScr();
        rpgtext.LovelyBox(Crt.Color.Blue, 3, 1, UCM_X2 - UCM_X1 - 2, 3);
        Crt.TextColor(Crt.Color.Green);
        int X = (UCM_X2 - UCM_X1 - msg.Length) / 2;
        if (X < 1)
            X = 1;
        Crt.GotoXY(X, 2);
        Crt.Write(msg);
        Crt.Window(1, 1, 80, 25);
    }

    static void TexBrowser(gamebook.Scenario SC, cwords.MPU MP, int Sec, string Cap)
    {
        /* This computer apparently has a list of text messages which the */
        /* player might or might not be able to access. */

        /* Prepare the display. */
        ClearUCM();

        int N;

        /* Create the menu. The items this menu will have in it are determined */
        /* by the SEC score that the player achieved. */
        rpgmenus.RPGMenu TBM = rpgmenus.CreateRPGMenu(Crt.Color.Black, Crt.Color.Green, Crt.Color.LightGreen, UCM_X1, UCM_Y1 + 3, UCM_X2, UCM_Y2);
        string S = MP.Attr;
        while (S != "")
        {
            N = texutil.ExtractValue(ref S);
            if (N > 0 && N <= rpgtext.NumTex)
            {
                /* Only add those messages for which the player */
                /* has obtained clearance. */
                if (rpgtext.TexMan[N - 1].clearance <= Sec)
                    rpgmenus.AddRPGMenuItem(TBM, rpgtext.TexMan[N - 1].title, N);
            }
        }
        rpgmenus.RPMSortAlpha(TBM);

        /* If the player does not have clearance to see any messages at */
        /* all, show a brief message then exit this procedure. */
        if (TBM.numItem < 1)
        {
            PrintCap("NO AVALIABLE MESSAGES");
            rpgtext.RPGKey();
            return;
        }

        do
        {
            PrintCap(Cap);
            N = rpgmenus.SelectMenu(TBM, rpgmenus.RPMNormal);

            if (N > -1)
            {
                PrintCap(rpgtext.TexMan[N - 1].title);
                rpgtext.GameMessage(rpgtext.TexMan[N].msg, UCM_X1, UCM_Y1 + 3, UCM_X2, UCM_Y2, Crt.Color.Green, Crt.Color.Black);
                rpgtext.RPGKey();

                if (!rpgtext.TexMan[N - 1].used)
                {
                    rpgtext.TexMan[N - 1].used = true;
                    gamebook.DoleExperience(SC, rpgtext.TexMan[N - 1].XPV);
                }
            }
        }
        while (N != -1);
    }

    const int NumLogoRows = 10;
    const int LogoWidth = 38;

    static string[] DCLogo = new string[NumLogoRows]
    {
            "            ..",
            "           .%%.",
            "          .%##%.",
            "         .%%##%%.",
            "        .%#!''!#%.",
            "       .%#%!..!##%.     ::. .:: :  :::",
            "      .%#%#%##%%##%.    : : :   : ::'",
            "     .%#%#%#%#%#%##%.   : : :   :  .::",
            "    .%#%#%#%#%#%#%##%.  ::' ':: : :::",
            "     ----------------"
    };

    static void DoInfoKiosk(gamebook.Scenario SC, cwords.MPU MP, int Sec)
    {
        /* This computer contains a list of TEX messages. If the player */
        /* has the correct security clearance, let her see them. */

        /* Create the menu. */
        rpgmenus.RPGMenu IKM = rpgmenus.CreateRPGMenu(Crt.Color.Black, Crt.Color.Green, Crt.Color.LightGreen, UCM_X1, UCM_Y2 - 4, UCM_X2, UCM_Y2);
        rpgmenus.AddRPGMenuItem(IKM, "Public Service Messages", 2);
        rpgmenus.AddRPGMenuItem(IKM, "Station Map", 1);

        int N;
        do
        {
            /* Set up the display. */
            ClearUCM();
            Crt.TextColor(Crt.Color.Green);
            for (N = 1; N <= NumLogoRows; ++N)
            {
                Crt.GotoXY((UCM_X1 + UCM_X2 - LogoWidth) / 2, UCM_Y1 + N);
                Crt.Write(DCLogo[N - 1]);
            }

            N = rpgmenus.SelectMenu(IKM, rpgmenus.RPMNoCleanup);

            switch (N)
            {
                case 1: DoMapDisplay(); break;
                case 2: TexBrowser(SC, MP, Sec, "STATION NEWS"); break;
            }
        }
        while (N != -1);

        /* Freeze the display, and dispose of the menu. */
        rpgmenus.DisplayMenu(IKM);
    }

    static void DoCrashedTerminal()
    {
        /* Simulate a crashed & nonfunctioning terminal. */
        string msg = "System Error 255 Main interface unit is offline";
        rpgtext.GameMessage(msg, UCM_X1 + 10, UCM_Y1 + 5, UCM_X2 - 10, UCM_Y2 - 5, Crt.Color.LightRed, Crt.Color.LightRed);
    }

    static void HealAllInjuries(gamebook.Scenario SC)
    {
        /* The medical terminal is going to fix everything that is */
        /* wrong with the PC. */
        rpgtext.DCGameMessage("Your injuries are treated by the medical unit.");
        SC.PC.HP = SC.PC.HPMax;
        plotbase.NAtt SFX = SC.PC.SF;
        while (SFX != null)
        {
            plotbase.NAtt SF2 = SFX.next;
            if (SFX.G == statusfx.NAG_StatusChange && SFX.S < 0)
            {
                plotbase.RemoveNAtt(ref SC.PC.SF, SFX);
            }
            SFX = SF2;
        }
        gamebook.PCStatLine(SC);
    }

    static void DoMedUnit(gamebook.Scenario SC, cwords.MPU MP, int Sec)
    {
        /* The medical unit is the player character's best friend. It will */
        /* heal all injuries and status conditions instantly... until the */
        /* player crashes it by trying to hack the medical database, that is. */

        rpgmenus.RPGMenu RPM = rpgmenus.CreateRPGMenu(Crt.Color.Red, Crt.Color.Red, Crt.Color.LightRed, UCM_X1 + 2, UCM_Y1 + 2, UCM_X2 - 2, UCM_Y2 - 2);
        rpgmenus.AddRPGMenuItem(RPM, "Treat Injuries", 1);
        rpgmenus.AddRPGMenuItem(RPM, "View Records", 2);
        rpgmenus.AddRPGMenuItem(RPM, "Standby Mode", -1);

        int N;
        do
        {
            N = rpgmenus.SelectMenu(RPM, rpgmenus.RPMNoCleanup);

            switch (N)
            {
                case 1: HealAllInjuries(SC); break;
                case 2: TexBrowser(SC, MP, Sec, "MEDICAL RECORDS"); break;
            }
        }
        while (N != -1);
    }

    static void PowerAllocation(gamebook.Scenario SC)
    {
        rpgmenus.RPGMenu PAM = rpgmenus.CreateRPGMenu(Crt.Color.DarkGray, Crt.Color.Blue, Crt.Color.LightBlue, UCM_X1 + 2, UCM_Y1 + 2, UCM_X2 - 2, UCM_Y2 - 2);
        rpgmenus.AddRPGMenuItem(PAM, "Module \"B\" Emergency Power: Security", 0);
        rpgmenus.AddRPGMenuItem(PAM, "Module \"B\" Emergency Power: Cryogenics", 0);
        rpgmenus.AddRPGMenuItem(PAM, "Module \"B\" Emergency Power: Infratap", 0);
        rpgmenus.AddRPGMenuItem(PAM, "Module \"B\" Emergency power: Life Support", 1);
        rpgmenus.AddRPGMenuItem(PAM, "Module \"B\" Emergency Power: Network", 0);
        int N = rpgmenus.SelectMenu(PAM, rpgmenus.RPMNoCancel);
        plotbase.SetNAtt(ref SC.NA, plotbase.NAG_ScriptVar, 2, N);
    }

    static void DoMorgan(gamebook.Scenario SC, cwords.MPU MP, int Sec)
    {
        rpgmenus.RPGMenu RPM = rpgmenus.CreateRPGMenu(Crt.Color.DarkGray, Crt.Color.Magenta, Crt.Color.LightMagenta, UCM_X1 + 2, UCM_Y1 + 2, UCM_X2 - 2, UCM_Y2 - 2);
        rpgmenus.AddRPGMenuItem(RPM, "Power Allocation", 1);
        rpgmenus.AddRPGMenuItem(RPM, "Mail Core Memory", 2);
        rpgmenus.AddRPGMenuItem(RPM, "Log Off", -1);

        int N = -1;
        do
        {
            N = rpgmenus.SelectMenu(RPM, rpgmenus.RPMNoCleanup);

            switch (N)
            {
                case 1: PowerAllocation(SC); break;
                case 2: TexBrowser(SC, MP, Sec, "MEDICAL RECORDS"); break;
            }
        }
        while (N != -1);
    }

    static void EmergencyStatus(gamebook.Scenario SC, cwords.MPU MP, int Sec)
    {
        /* Determine current alert status, and create menu accordingly. */
        int N = plotbase.NAttValue(SC.NA, plotbase.NAG_ScriptVar, 4);

        rpgmenus.RPGMenu RPM;

        switch (N)
        {
            case 1:
                RPM = rpgmenus.CreateRPGMenu(Crt.Color.LightRed, Crt.Color.Cyan, Crt.Color.Yellow, UCM_X1 + 3, UCM_Y1 + 3, UCM_X2 - 3, UCM_Y2 - 3);
                break;
            case 0:
                RPM = rpgmenus.CreateRPGMenu(Crt.Color.Yellow, Crt.Color.Cyan, Crt.Color.Yellow, UCM_X1 + 3, UCM_Y1 + 3, UCM_X2 - 3, UCM_Y2 - 3);
                break;
            default:
                RPM = rpgmenus.CreateRPGMenu(Crt.Color.LightGreen, Crt.Color.Cyan, Crt.Color.Yellow, UCM_X1 + 3, UCM_Y1 + 3, UCM_X2 - 3, UCM_Y2 - 3);
                break;
        }

        rpgmenus.AddRPGMenuItem(RPM, "Alert Status: Red", 2);
        rpgmenus.AddRPGMenuItem(RPM, "Alert Status: Yellow", 1);
        rpgmenus.AddRPGMenuItem(RPM, "Alert Status: Green", 0);

        N = rpgmenus.SelectMenu(RPM, rpgmenus.RPMNormal);

        if (N > -1 && Sec > 0)
        {
            rpgtext.DCGameMessage("Alert status changed.");
            plotbase.SetNAtt(ref SC.NA, plotbase.NAG_ScriptVar, 4, N - 1);

            /* If the alert has been turned off, set the trigger. */
            if (N == 0)
            {
                gamebook.SetTrigger(SC, "GotoTURNOFFFIELD");
            }
            /* If, on the other hand, the player set red alert... */
            /* alert all robots. */
            else if (N == 2)
            {
                gamebook.SetTrigger(SC, "ALARM");
            }

        }
        else if (Sec == 0)
        {
            /* Lock up the terminal, then exit with an alarm. */
            rpgtext.DCGameMessage("Denied! Unauthorized use of security systems is prohibited.");
            MP.Attr = "X" + MP.Attr;
            gamebook.SetTrigger(SC, "ALARM");
        }
    }

    static void DoDescartes(gamebook.Scenario SC, cwords.MPU MP, int Sec)
    {
	    /* The player is accessing primary server DESCARTES. */

	    rpgmenus.RPGMenu RPM = rpgmenus.CreateRPGMenu(Crt.Color.Green, Crt.Color.Cyan, Crt.Color.Yellow , UCM_X1 + 2 , UCM_Y1 + 2 , UCM_X2 - 2 , UCM_Y2 - 2 );
	    rpgmenus.AddRPGMenuItem( RPM , "Emergency Status" , 1 );
        rpgmenus.AddRPGMenuItem( RPM , "Mail Core Memory" , 2 );
        rpgmenus.AddRPGMenuItem( RPM , "Log Off" , -1 );

        int N;
        do
        {
            N = rpgmenus.SelectMenu(RPM, rpgmenus.RPMNoCleanup);

            switch (N)
            {
                case 1: EmergencyStatus(SC, MP, Sec); break;
                case 2: TexBrowser(SC, MP, Sec, "MEDICAL RECORDS"); break;
            }

        }
        while (N != -1 || MP.Attr[0] == 'X');
    }


    static void DoUserTerminal(gamebook.Scenario SC, cwords.MPU MP, int Sec)
    {
        /* The player wants to use the user terminal. Branch to an appropriate */
        /* procedure. */

        /* If this terminal has locked up, it cannot be used. */
        if (MP.Attr[0] == 'X')
        {
            DoCrashedTerminal();
        }
        else
        {
            switch (MP.kind)
            {
                case 1: DoInfoKiosk(SC, MP, Sec); break;
                case 2: DoMedUnit(SC, MP, Sec); break;
                case 3: DoMorgan(SC, MP, Sec); break;
                case 4: DoDescartes(SC, MP, Sec); break;
            }
        }
    }

    static void AttemptHack(gamebook.Scenario SC, cwords.MPU MP, ref int Sec)
    {
	    /* The player wants to hack this terminal. Give it a try, and hope */
	    /* there are no disasterous results... */
	    rpgtext.DCGameMessage( "Attempting to hack " + cwords.MPUMan[MP.kind - 1].name + "...");

	    /* Do the animation for hacking. */
	    Crt.Window( MCM_X1 + 1 , MCM_Y1 + 1 , MCM_X2 - 1 , MCM_Y2 - 1 );
	    Crt.ClrScr();
	    Crt.TextColor(Crt.Color.Blue);
	    int N = rpgdice.Random(250) + 250;
        for (int t = 1; t <= N; ++t)
            Crt.Write(rpgdice.Random(256).ToString("X2") + " ");
        texfx.Delay();
	    N = rpgdice.Random(250) + 250;
	    for (int t = 1; t <= N; ++t)
            Crt.Write(rpgdice.Random(256).ToString("X2") + " ");
	    texfx.Delay();

	    /* Actually figure out if it worked. */
	    int R = rpgdice.RollStep(dcchars.PCTechSkill(SC.PC));
	    int T = Sec + cwords.MPUMan[MP.kind - 1].SecPass;
	    if (R > cwords.MPUMan[MP.kind - 1].SecPass && R > T )
        {
		    rpgtext.DCAppendMessage(" You did it.");
		    Sec = R - cwords.MPUMan[MP.kind - 1].SecPass;
	    }
        else
        {
		    rpgtext.DCAppendMessage(" You failed.");
		    if (R < T - 5)
            {
			    MP.Attr = "X" + MP.Attr;
			    if (R < T - 10)
                    gamebook.SetTrigger( SC , "ALARM");
		    }
	    }
    }

}
