using System;
using System.IO;

public class dcplay
{
    public static void StartGame()
    {
        /* Begin a new game. Generate a character, make a level, etc. */
        /* Pass all info on to the PLAYSCENE procedure above. If character */
        /* creation is cancelled, or for some reason isn't successful, */
        /* exit this procedure without calling PLAYSCENE. */
        gamebook.Scenario SC = gamebook.NewScenario();
        rpgtext.ResetLogon();

        SC.PC = randchar.RollNewChar();
        if (SC.PC == null)
        {
            return;
        }

        SC.Loc_Number = 1;

        randmaps.GotoLevel(SC, 1, texmaps.PilotsChair);

        gamebook.SaveGame(SC);
        PlayScene(SC);
    }

    public static void RestoreGame()
    {
        /* Load a saved game file from the SAVEGAME directory. */
        /* If this action is cancelled, return to the main menu. */
        /* If there are no files to load, call the STARTGAME procedure. */
        /* Otherwise, pass the scenario data on to PLAYSCENE. */

        if (!Directory.Exists("savegame"))
        {
            /* No savegame directory, Jump to STARTGAME. */
            StartGame();
        }
        else
        {
            /* Create the menu. */
            rpgmenus.RPGMenu RPM = rpgmenus.CreateRPGMenu(Crt.Color.LightBlue, Crt.Color.Green, Crt.Color.LightGreen, 20, 8, 60, 23);
            rpgmenus.BuildFileMenu(RPM, "savegame/*.txt");

            if (RPM.numItem < 1)
            {
                /* No save game files were found. Jump to STARTGAME, */
                /* after deallocating the empty menu... */
                StartGame();
            }
            else
            {
                /* Select a file, then dispose of the menu. */
                rpgmenus.RPMSortAlpha(RPM);
                string FName = rpgmenus.SelectFile(RPM);

                /* If selection was cancelled, just fall back out to the */
                /* main menu. Otherwise, load the file and pass the */
                /* scenario to PLAYSCENE. */
                if (FName != "")
                {
                    gamebook.Scenario SC = gamebook.LoadGame("savegame/" + FName + ".txt");
                    PlayScene(SC);
                }
            }
        }
    }

    static bool HelpScreen(gamebook.Scenario SC)
    {
        /*Just print a list of keys.*/
        rpgtext.DCGameMessage("Help - Here are the implemented command keys.");

        rpgmenus.RPGMenu RPM = rpgmenus.CreateRPGMenu(Crt.Color.LightGray, Crt.Color.Green, Crt.Color.LightGreen, 20, 7, 40, 19);
        RPM.dx1 = 45;
        RPM.dy1 = 10;
        RPM.dx2 = 65;
        RPM.dy2 = 15;
        RPM.dBorColor = Crt.Color.LightGray;
        RPM.dTexColor = Crt.Color.LightBlue;

        for (int t = 1; t < rpgtext.KMap.Length; ++t)
        {
            rpgmenus.AddRPGMenuItem(RPM, rpgtext.KMap[t].key + ": " + rpgtext.KMap[t].name, t, rpgtext.KMap[t].desc);
        }

        rpgmenus.SelectMenu(RPM, rpgmenus.RPMNormal);
        pcaction.PCReCenter(SC);

        return false;
    }

    static bool PCCanAct(dcchars.DCChar PC)
    {
        /*Return TRUE if the PC is capable of acting*/
        /*right now, FALSE if it is for any reason incapacitated.*/
        if (plotbase.NAttValue(PC.SF, statusfx.NAG_StatusChange, statusfx.SEF_Paralysis) != 0)
            return false;
        else if (plotbase.NAttValue(PC.SF, statusfx.NAG_StatusChange, statusfx.SEF_Sleep) != 0)
            return false;

        return true;
    }

    static void DoPCAction(gamebook.Scenario SC)
    {
        /*Input a command from the PC, and do it.*/

        /*Initialize values*/
        bool Act = false;
        char a = '&';
        gamebook.PCStatLine(SC);

        /*Check to make sure the PC is capable of acting right now.*/
        /*If paralyzed or asleep, no action is possible.*/
        if (PCCanAct(SC.PC))
        {
            /*Check to see whether or not the PC is currently doing*/
            /*a continual action. If so, do that.*/
            if (SC.PC.repCount > 0)
            {
                /*Decrement the repeat counter.*/
                SC.PC.repCount -= 1;

                /*Call the Repeat Handler.*/
                Act = pcaction.PCProcessRepeat(SC);
            }
            else
            {
                do
                {
                    a = rpgtext.RPGKey();
                    if (a == rpgtext.KMap[1].key) Act = pcaction.PCMove(SC, 1);
                    else if (a == rpgtext.KMap[2].key) Act = pcaction.PCMove(SC, 2);
                    else if (a == rpgtext.KMap[3].key) Act = pcaction.PCMove(SC, 3);
                    else if (a == rpgtext.KMap[4].key) Act = pcaction.PCMove(SC, 4);
                    else if (a == rpgtext.KMap[5].key) Act = pcaction.PCMove(SC, 5);
                    else if (a == rpgtext.KMap[6].key) Act = pcaction.PCMove(SC, 6);
                    else if (a == rpgtext.KMap[7].key) Act = pcaction.PCMove(SC, 7);
                    else if (a == rpgtext.KMap[8].key) Act = pcaction.PCMove(SC, 8);
                    else if (a == rpgtext.KMap[9].key) Act = pcaction.PCMove(SC, 9);
                    else if (a == rpgtext.KMap[10].key) Act = pcaction.PCOpenDoor(SC);

                    else if (a == rpgtext.KMap[11].key) Act = pcaction.PCCloseDoor(SC);
                    else if (a == rpgtext.KMap[12].key) Act = pcaction.PCReCenter(SC);
                    else if (a == rpgtext.KMap[13].key) Act = pcaction.PCShooting(SC, true);
                    else if (a == rpgtext.KMap[14].key) Act = pcaction.PCTosser(SC);
                    else if (a == rpgtext.KMap[15].key) Act = pcaction.PCInvScreen(SC, true);

                    else if (a == rpgtext.KMap[16].key) Act = pcaction.PCInvScreen(SC, false);
                    else if (a == rpgtext.KMap[17].key) Act = pcaction.PCPickUp(SC);
                    else if (a == rpgtext.KMap[18].key) Act = pcaction.PCDisarmTrap(SC);
                    else if (a == rpgtext.KMap[19].key) Act = pcaction.PCSearch(SC);
                    else if (a == rpgtext.KMap[20].key) Act = pcaction.PCUsePsi(SC, true);

                    else if (a == rpgtext.KMap[21].key) Act = pcaction.PCUsePsi(SC, false);
                    else if (a == rpgtext.KMap[22].key) Act = pcaction.PCCheckXP(SC);
                    else if (a == rpgtext.KMap[23].key) Act = pcaction.PCLookAround(SC);
                    else if (a == rpgtext.KMap[24].key) Act = pcaction.PCEnter(SC);
                    else if (a == rpgtext.KMap[25].key) Act = pcaction.PCRepeat(SC);

                    else if (a == rpgtext.KMap[26].key)
                    {
                        /* It's quitting time. */
                        Act = true;
                        rpgtext.DCGameMessage("Save the game first? (Y/N)");
                        if (rpgtext.YesNo())
                            gamebook.SaveGame(SC);
                    }
                    else if (a == rpgtext.KMap[27].key) Act = HelpScreen(SC);
                    else if (a == rpgtext.KMap[28].key)
                    {
                        Act = false;
                        rpgtext.DCGameMessage("Saving game...");
                        gamebook.SaveGame(SC);
                        rpgtext.DCAppendMessage("Done.");
                    }
                    else if (a == rpgtext.KMap[29].key) Act = pcaction.PCInfoScreen(SC);
                    else if (a == rpgtext.KMap[30].key) Act = pcaction.PCHandyMap(SC);
                    else if (a == '!')
                    {
                        rpgtext.DCGameMessage("Cheat Code Alpha!");
                        gamebook.DoleExperience(SC, 100);
                    }
                    else if (a == '@')
                    {
                        rpgtext.DCGameMessage("Cheat Code Beta!");
                        Crt.GotoXY(1, 25);
                        Crt.TextColor(Crt.Color.Yellow);
                        Crt.Write(critters.NumberOfCritters(SC.CList).ToString());
                        dccombat.CritterDeath(SC, SC.CList, true);
                    }
                    else if (a == '#')
                    {
                        rpgtext.DCGameMessage("Cheat Code Gamma!");
                        Crt.GotoXY(1, 25);
                        Crt.TextColor(Crt.Color.Yellow);
                        gamebook.SetTrigger(SC, "CHEATRIGHT");
                    }
                    else if (a == '$')
                    {
                        rpgtext.DCGameMessage("Cheat Code Theta!");
                        Crt.GotoXY(1, 25);
                        Crt.TextColor(Crt.Color.Yellow);
                        gamebook.SetTrigger(SC, "CHEATLEFT");
                    }
                }
                while (!Act && PCCanAct(SC.PC));
                SC.PC.lastCmd = a;
            }
        }
        else
        {
            rpgtext.DCGameMessage("Can't move!");
        }

        /*Check for poisoning here.*/
        if (plotbase.NAttValue(SC.PC.SF, statusfx.NAG_StatusChange, statusfx.SEF_Poison) > 0 && SC.PC.HP > 0)
        {
            /*Make a Luck roll to avoid the effect of poison.*/
            int D = (60 - rpgdice.RollStep(dcchars.PCLuckSave(SC.PC))) / 10;
            if (D > 0)
            {
                SC.PC.HP = SC.PC.HP - D;
                rpgtext.DCGameMessage("Poison!");
                if (SC.PC.HP < 1)
                    rpgtext.DCAppendMessage("You have died!");
                gamebook.PCStatLine(SC);
            }
        }
        /* Check for regeneration here - is cancelled by poison. */
        else if (plotbase.NAttValue(SC.PC.SF, statusfx.NAG_StatusChange, statusfx.SEF_Regeneration) > 0 && SC.PC.HP < SC.PC.HPMax)
        {
            SC.PC.HP += 1 + rpgdice.Random(3);
            if (SC.PC.HP > SC.PC.HPMax)
                SC.PC.HP = SC.PC.HPMax;
        }
    }


    static void PlayScene(gamebook.Scenario SC)
    {
        /* This procedure holds the actual game loop. */
        /* Note that at the } of this procedure, the scenario is */
        /* deallocated. */

        Crt.ClrScr();

        texmaps.UpdatePOV(SC.gb.POV, SC.gb);
        texmaps.ApplyPOV(SC.gb.POV, SC.gb);
        texmaps.DisplayMap(SC.gb);

        gamebook.PCStatLine(SC);
        SC.PC.lastCmd = ' ';

        rpgtext.DCGameMessage("Welcome to the game.");

        do
        {
            SC.ComTime += 1;

            /* Set time triggers here. */
            if (SC.ComTime % 720 == 0)
            {
                gamebook.SetTrigger(SC, "HOUR");
            }
            else if (SC.ComTime % 120 == 0)
            {
                gamebook.SetTrigger(SC, "10MIN");
            }
            else if (SC.ComTime % 12 == 0)
            {
                gamebook.SetTrigger(SC, "MINUTE");
            }

            /*Update the PC's Status List.*/
            statusfx.UpdateStatusList(ref SC.PC.SF);

            /*Check the PCs food status. A check is performed*/
            /*every 10 minutes.*/
            if (SC.ComTime % 120 == 81)
            {
                if (SC.PC.carbs > -10)
                    SC.PC.carbs -= 1;
                if (SC.PC.carbs < 0)
                    rpgtext.DCGameMessage("You are starving!");
                else if (SC.PC.carbs < 10)
                    rpgtext.DCGameMessage("You are hungry.");

                gamebook.PCStatLine(SC);
            }

            /* Check for PC regeneration. A check is performed every minute. */
            /* The PC does _not_ regenerate while poisoned. Ouch. */
            if (SC.ComTime % 12 == 0)
            {
                /*See if the PC gets any HPs back this click...*/
                if (SC.PC.HP < SC.PC.HPMax && plotbase.NAttValue(SC.PC.SF, statusfx.NAG_StatusChange, statusfx.SEF_Poison) == 0)
                {
                    if (gamebook.NumberOfActions(SC.ComTime / 12, dcchars.PCRegeneration(SC.PC)) > 0)
                    {
                        SC.PC.HP += gamebook.NumberOfActions(SC.ComTime / 12, dcchars.PCRegeneration(SC.PC));

                        /*If the PC is starving and injured, perminant damage to health may result.*/
                        if (SC.PC.carbs < 0 && rpgdice.Random(Math.Abs(SC.PC.carbs)) > rpgdice.Random(SC.PC.stat[8 - 1]) && SC.PC.HPMax > 10)
                        {
                            SC.PC.HPMax -= 1;
                            rpgtext.DCGameMessage("You feel seriously ill.");
                        }

                        if (SC.PC.HP > SC.PC.HPMax)
                            SC.PC.HP = SC.PC.HPMax;

                        gamebook.PCStatLine(SC);
                    }
                }

                /*Check for PC MP restoration.*/
                if (SC.PC.MP < SC.PC.MPMax)
                {
                    SC.PC.MP += gamebook.NumberOfActions(SC.ComTime / 12, dcchars.PCRestoration(SC.PC));
                    if (SC.PC.MP > SC.PC.MPMax)
                        SC.PC.MP = SC.PC.MPMax;

                    gamebook.PCStatLine(SC);
                }
            }

            /*Check for random monsters every 5 minutes.*/
            if (SC.ComTime % rpgtext.PLAY_MonsterTime == 0)
                charts.WanderingCritters(SC);

            /* Check for spontaneous identification of items every hour. */
            if (SC.ComTime % 720 == 553)
                pcaction.ScanUnknownInv(SC);


            /*If the player gets an action this second, use it.*/
            for (int t = 1; t <= gamebook.NumberOfActions(SC.ComTime, dcchars.PCMoveSpeed(SC.PC)); ++t)
            {
                DoPCAction(SC);

                /* If QUIT was the command, or if the PC is dead, */
                /* break this loop. */
                if (SC.PC.lastCmd == rpgtext.KMap[26].key || SC.PC.HP <= 0)
                    break;

                plotline.HandleTriggers(SC);

                SC.gb.POV.range = dcchars.PCVisionRange(SC.PC);
            }

            /* If a QUIT request wan't recieved, handle clouds and critters. */
            if (SC.PC.lastCmd != rpgtext.KMap[26].key)
            {
                /*Cloud handling. Happens every 4 seconds.*/
                if (SC.ComTime % 4 == 1)
                    cbrain.BrownianMotion(SC);

                /*Critter handling*/
                critters.Critter Cr = SC.CList;
                while (Cr != null)
                {
                    /*Save the position of the next critter,*/
                    /*since the critter we're processing might*/
                    /*accidentally kill itself during its move.*/
                    SC.CA2 = Cr.next;
                    for (int t = 1; t <= gamebook.NumberOfActions(SC.ComTime, critters.MonMan[Cr.crit - 1].Speed); ++t)
                    {
                        cbrain.CritterAction(SC, ref Cr);
                        if (Cr == null)
                            break;
                    }

                    Cr = SC.CA2;
                    if (SC.PC.HP < 1)
                        Cr = null;
                }
            }
        }
        while (SC.PC.lastCmd != rpgtext.KMap[26].key && SC.PC.HP > 0);

        if (SC.PC.HP < 1)
        {
            rpgtext.DCGameMessage("Game Over.");
            rpgtext.GamePause();

            string fname = "savegame/" + SC.PC.name + ".txt";

            if (rpgtext.PLAY_DangerOn)
            {
                File.Delete(fname);
            }
        }
    }

}