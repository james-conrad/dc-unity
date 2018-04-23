using System;

public class plotline
{
	/*This unit handles storyline development in our RL.*/
	/*Specifically, it handles the scripting language which*/
	/*will be used to control everything.*/

	/* See plotref.txt for info on the scripting language used. */

    public static string[] PLConstant = {
	    /* Attempting to ">" the cryogenic capsule. */
	    "EN45 <ifYN CRYO1 else GotoLEAVECRYO print CRYO3 print CRYO4 print CRYO5>",
        "gotoLEAVECRYO <print CRYO2>",
        "msgCRYO1 <Do you want to lie down in the cryonic casket?>",
        "msgCRYO2 <You leave the casket, unwilling at this point to turn yourself into a meat popsicle.>",
        "msgCRYO3 <You lie down in the casket and close the lid...>",
        "msgCRYO4 <...>",
        "msgCRYO5 <Nothing happens. You get back out.>",

	    /*When the CORPSE is first seen, it attacks the PC.*/
	    "NC3 <Alert @>",

	    /*Transitway door handler.*/
	    "EN32 <ifkey 1 else GotoSorryNoPasscard GoLeft>",
        "EN33 <ifkey 1 else GotoSorryNoPasscard GoRight>",
        "GOTOSORRYNOPASSCARD <print SorryMsg>",
        "MSGSORRYMSG <Unregistered personnel may not use the transit system. Access Denied.>",

	    /* Pilots chair handler. */
	    "EN21 <print PilotChair>",
        "MSGPILOTCHAIR <Your ship's computer has stopped working. None of the systems will respond to your commands.>",

	    /* Toilet Humor. */
	    "EN38 <if= V1 2 else GotoTH2 V= 1 3 print ToiletHumor1>",
        "GotoTH2 <print ToiletHumor2>",
        "msgToiletHumor1 <You use the toilet. You feel better.>",
        "msgToiletHumor2 <You already did that. You don't have to any more.>",

	    /* When the alarm goes off, notify all robots and zombies. */
	    "ALARM <Alert R Alert @>",

	    /* Reliquary door plaques */
	    "MS3 <Print Reliquary1>",
        "MS4 <Print Reliquary2>",
        "MS5 <Print Reliquary3>",
        "msgRELIQUARY1 <A plaque on the door reads \"IN HOC SALUS\".>",
        "msgRELIQUARY2 <A plaque on the door reads \"MEMENTO MORI\".>",
        "msgRELIQUARY3 <A plaque on the door reads \"TO THE FATHERS\".>"
    };

    public static void HandleTriggers(gamebook.Scenario SC)
    {
	    /*Handle all of the accumulated triggers.*/

	    /*Here's how the system works. Certain PC actions can cause*/
	    /*special plot events to take place; any time the PC performs*/
	    /*such an action, a "trigger" message is added to the list.*/
	    /*This procedure checks the triggers which have accumulated since*/
	    /*last call and sees if there's a special event attached to any*/
	    /*of them. Triggers without special effects are ignored.*/

	    /*Check all the PArc lists (local, global,*/
	    /*and constant) to see if there's an effect which matches the*/
	    /*trigger. As soon as a match is found, execute the PArc string.*/
	    /*Do this for all of the triggers. Once finished, deallocate the*/
	    /*trigger list.*/

	    /* Record the level the PC started on. */
	    int StartLevel = SC.Loc_Number;

	    plotbase.SAtt TP = SC.PLTrig;
        while (TP != null)
        {
            /* If there is a SAtt in the scenario description */
            /* named after this trigger description, it will */
            /* happen now. First, see if such an event exists. */
            string E = LocateEvent(SC, TP.info);
            if (E != "") InvokeEvent(ref E, SC);

            TP = TP.next;
        }

        /* Finally, dispose of the list of triggers. */
        SC.PLTrig = null;

	    /* If the Start Level and current level aren't the same, need to add */
	    /* a "START" trigger. */
	    if (StartLevel != SC.Loc_Number)
            gamebook.SetTrigger(SC, "START");
    }

    static string SeekCPlot(string Trigger)
    {
        /* Try to find an event matching TRIGGER in the constant plotline */
        /* section. Since the global plotlines are an array of strings, */
        /* not a list of SAtts, we can't use the SAtt procedures to */
        /* search through them. */

        /* Initialize IT, and make sure the trigger is all uppercase. */
        Trigger = Trigger.ToUpper();

        /* Go through the constant scripts. */
        for (int t = 0; t < PLConstant.Length; ++t)
        {
            string S = PLConstant[t];
            S = texutil.ExtractWord(ref S).ToUpper();

            /* Retrieve the bits from inside the alligator brackets. */
            if (S == Trigger)
                return texutil.RetrieveAString(PLConstant[t]);
        }

        return "";
    }

    static string LocateEvent(gamebook.Scenario SC, string Trigger)
    {
        /* This function will attempt to find an event matching TRIGGER. */
        /* Order of searching is Local List, then Global List, then */
        /* Constant List. The first event which matches the specified */
        /* trigger is returned. */

        string it = plotbase.SAttValue(SC.PLLocal, Trigger);
        if (it == "")
            it = plotbase.SAttValue(SC.PLGlobal, Trigger);
        if (it == "")
            it = SeekCPlot(Trigger);

        return it;
    }

    static int PlayVal_Leakage(gamebook.Scenario SC)
    {
        /* Return a Leakage value for the PC, in the range of 0 to 10. */
        /* A low value indicates that more of the PC's armor is airtight; */
        /* a high value indicates that the PC really ought to invest in */
        /* some scuba gear or something. */
        int Leak = 0;

        /* The helmet contributes 5 leakage points, the body 3, the arms and */
        /* legs one each. */
        if (SC.PC.eqp[dcchars.ES_Head - 1] == null || !dcitems.CCap[SC.PC.eqp[dcchars.ES_Head - 1].icode - 1].atmSealed)
        {
            Leak += 5;
        }

        if (SC.PC.eqp[dcchars.ES_Body - 1] == null || !dcitems.CArmor[SC.PC.eqp[dcchars.ES_Body - 1].icode - 1].atmSealed)
        {
            Leak += 3;
        }

        if (SC.PC.eqp[dcchars.ES_Hand - 1] == null || !dcitems.CGlove[SC.PC.eqp[dcchars.ES_Hand - 1].icode - 1].atmSealed)
        {
            Leak += 1;
        }

        if (SC.PC.eqp[dcchars.ES_Foot - 1] == null || !dcitems.CShoe[SC.PC.eqp[dcchars.ES_Foot - 1].icode - 1].atmSealed)
        {
            Leak += 1;
        }

        return Leak;
    }

    static int ScriptValue(ref string Event, gamebook.Scenario SC)
    {
        /* Normally, numerical values will be stored as constants. */
        /* Sometimes we may want to do algebra, or use the result of */
        /* scenario variables as the parameters for commands. That's */
        /* what this function is for. */
        string SMsg = texutil.ExtractWord(ref Event);
        int SV = 0;

        /* If the first character is one of the value commands, */
        /* process the string as appropriate. */
        if (char.ToUpper(SMsg[0]) == 'V')
        {
            /* Use the contents of a variable instead of a constant. */
            texutil.DeleteFirstChar(ref SMsg);
            int VCode = texutil.ExtractValue(ref SMsg);
            SV = plotbase.NAttValue(SC.NA, plotbase.NAG_ScriptVar, VCode);
        }
        else if (char.ToUpper(SMsg[0]) == 'P')
        {
            /* Use one of the Player values instead of a constant. */
            texutil.DeleteFirstChar(ref SMsg);
            if (char.ToUpper(SMsg[0]) == 'L')
            {
                SV = PlayVal_Leakage(SC);
            }

        }
        else
        {
            /* No command was given, so this must be a constant value. */
            SV = texutil.ExtractValue(ref SMsg);
        }

        return SV;
    }

    static void ProcessPrint(ref string Event, gamebook.Scenario SC)
    {
        /* Locate and then print the specified message. */
        /* find out the label of the message to print. */
        string L = texutil.ExtractWord(ref Event);

        /* Locate the message from the SCENE variable. */
        string msg = LocateEvent(SC, "MSG" + L);

        /* If such a message exists, print it. */
        if (msg != "")
        {
            rpgtext.DCGameMessage(msg);
            rpgtext.GamePause();
        }
    }

    static void ProcessVarEquals(ref string Event, gamebook.Scenario SC)
    {
        /* The script is going to assign a value to one of the scene */
        /* variables. */

        /* Find the variable ID number and the value to assign. */
        int idnum = ScriptValue(ref Event, SC);
        int value = ScriptValue(ref Event, SC);

        plotbase.SetNAtt(ref SC.NA, plotbase.NAG_ScriptVar, idnum, value);
    }

    static void ProcessVarPlus(ref string Event, gamebook.Scenario SC)
    {
        /* The script is going to add a value to one of the scene */
        /* variables. */

        /* Find the variable ID number and the value to assign. */
        int idnum = ScriptValue(ref Event, SC);
        int value = ScriptValue(ref Event, SC);

        plotbase.AddNAtt(ref SC.NA, plotbase.NAG_ScriptVar, idnum, value);
    }

    static void IfSuccess(ref string Event)
    {
        /* An IF call has generated a "TRUE" result. Just get rid of */
        /* any ELSE clause that the event string might still be holding. */

        /* Extract the next word from the script. */
        string cmd = texutil.ExtractWord(ref Event);

        /* If the next word is ELSE, we have to also extract the label. */
        /* If the next word isn't ELSE, better re-assemble the line... */
        if (cmd.ToUpper() == "ELSE")
            texutil.ExtractWord(ref Event);
        else
            Event = cmd + " " + Event;
    }

    static void IfFailure(ref string Event, gamebook.Scenario SC)
    {
        /* An IF call has generated a "FALSE" result. See if there's */
        /* a defined ELSE clause, and try to load the next line. */

        /* Extract the next word from the script. */
        string cmd = texutil.ExtractWord(ref Event);

        if (cmd.ToUpper() == "ELSE")
        {
            /* There's an else clause. Attempt to jump to the */
            /* specified script line. */
            cmd = texutil.ExtractWord(ref Event);
            Event = LocateEvent(SC, cmd);
        }
        else
        {
            /* There's no ELSE clause. Just cease execution of this */
            /* line by setting it to an empty string. */
            Event = "";
        }
    }

    static void ProcessIfEqual(ref string Event, gamebook.Scenario SC)
    {
        /* Two values are supplied as the arguments for this procedure. */
        /* If they are equal, that's a success. */

        /* Determine the two values. */
        int A = ScriptValue(ref Event, SC);
        int B = ScriptValue(ref Event, SC);

        if (A == B)
            IfSuccess(ref Event);
        else
            IfFailure(ref Event, SC);
    }

    static void ProcessIfGreater(ref string Event, gamebook.Scenario SC)
    {
        /* Two values are supplied as the arguments for this procedure. */
        /* If the first is biggest, that's a success. */

        /* Determine the two values. */
        int A = ScriptValue(ref Event, SC);
        int B = ScriptValue(ref Event, SC);

        if (A > B)
            IfSuccess(ref Event);
        else
            IfFailure(ref Event, SC);
    }

    static void ProcessIfKeyItem(ref string Event, gamebook.Scenario SC)
    {
	    /*Check to see whether or not the PC has a*/
	    /*certain Key Item.*/

	    /*Extract the item number, and increase C.*/
	    int I = ScriptValue(ref Event , SC);

        if (dcitems.HasItem(SC.PC.inv, dcitems.IKIND_KeyItem, I))
            IfSuccess(ref Event);
        else
            IfFailure(ref Event, SC);
    }

    static void ProcessIfYesNo(ref string Event, gamebook.Scenario SC)
    {
        /* Two values are supplied as the arguments for this procedure. */
        /* If the first is biggest, that's a success. */

        /* find out the label of the prompt to print. */
        string L = texutil.ExtractWord(ref Event);

        /* Locate the message from the SCENE variable. */
        string msg = LocateEvent(SC, "MSG" + L);

        /* If such a message exists, print it. */
        if (msg != "")
        {
            rpgtext.DCGameMessage(msg + " (Y/N)");
        }
        else
        {
            rpgtext.DCGameMessage("Yes or No? (Y/N)");
        }

        /* Check for success or failure. */
        if (rpgtext.YesNo())
            IfSuccess(ref Event);
        else
            IfFailure(ref Event, SC);
    }


    static void ProcessAlertCritters(ref string Event, gamebook.Scenario SC)
    {
        /*Alert all critters of the given type to*/
        /*the PC's presence.*/

        /* Find out what sort of critter to alert. */
        string CType = texutil.ExtractWord(ref Event);

        /* If the parameter was supplied, go on to alert those critters. */
        if (CType != "")
        {
            critters.Critter CTemp = SC.CList;
            while (CTemp != null)
            {
                if (CTemp.M.gfx == CType[0])
                    CTemp.Target = SC.PC.m;

                CTemp = CTemp.next;
            }
        }
    }

    static void ProcessGoLeft(gamebook.Scenario SC)
    {
        /* The PC is taking a transitway counterclockwise around one of */
        /* the station rings. */

        /* Right now, since only two levels are "stocked", restrict travel. */
        /* Comment out actual code. */
        /*	NewLevel = SC.Loc_Number + 1; */
        /*	if NewLevel > 8 then NewLevel = 1; */

        int NewLevel;
        switch (SC.Loc_Number)
        {
            case 1: NewLevel = 2; break;
            case 2: NewLevel = 8; break;
            default: NewLevel = 1; break;
        }

        randmaps.GotoLevel(SC, NewLevel, 33);
        texmaps.DisplayMap(SC.gb);
    }

    static void ProcessGoRight(gamebook.Scenario SC)
    {
        /* The PC is taking a transitway clockwise around one of */
        /* the station rings. */
        /* See above for comments. */
        /*	NewLevel = SC.Loc_Number - 1;
	        if NewLevel < 1 then NewLevel = 8;
        */

        int NewLevel;
        switch (SC.Loc_Number)
        {
            case 1: NewLevel = 8; break;
            case 8: NewLevel = 2; break;
            default: NewLevel = 1; break;
        }

        randmaps.GotoLevel(SC, NewLevel, 32);
        texmaps.DisplayMap(SC.gb);
    }


    static string[] ChokeMsg = {
            "You're choking!",
            "You can't breathe!",
            "You're suffocating!",
            "The air is too thin... you are passing out.",
            "You begin to gasp for breath.",
            "Your lungs scream for air."
    };

    static void ProcessChoke(gamebook.Scenario SC)
    {
        /* The PC is suffocating to death! */

        /* Start by printing a jovial message to let the PC know what's going on. */
        rpgtext.DCGameMessage(ChokeMsg[rpgdice.Random(ChokeMsg.Length)]);
        SC.PC.repCount = 0;

        /* Roll damage, then deal it out. */
        int DMG = rpgdice.Random(5) + rpgdice.Random(5) + 2;
        bool dead = dccombat.DamagePC(SC, 4, "", DMG);

        rpgtext.GamePause();

        if (dead)
        {
            rpgtext.DCAppendMessage("You die...");
        }
    }

    static void ProcessChangeTerr(ref string Event, gamebook.Scenario SC)
    {
        /* Change Terrain1 into Terrain2 all over the map. */

        /* Determine the two terrain values. */
        int T1 = ScriptValue(ref Event, SC);
        int T2 = ScriptValue(ref Event, SC);

        /* Actually change the terrain. */
        for (int X = 1; X <= texmodel.XMax; ++X)
        {
            for (int Y = 1; Y <= texmodel.YMax; ++Y)
            {
                if (SC.gb.map[X - 1, Y - 1].terr == T1)
                {
                    SC.gb.map[X - 1, Y - 1].terr = T2;
                }
            }
        }
    }

    static void InvokeEvent(ref string Event, gamebook.Scenario SC)
    {
        /* Do whatever is requested by game script EVENT. */

        /* Keep processing the EVENT until we run out of commands. */
        while (Event != "")
        {
            string cmd = texutil.ExtractWord(ref Event).ToUpper();

            if (cmd == "PRINT")
                ProcessPrint(ref Event, SC);
            else if (cmd == "V=")
                ProcessVarEquals(ref Event, SC);
            else if (cmd == "V+")
                ProcessVarPlus(ref Event, SC);
            else if (cmd == "IF=")
                ProcessIfEqual(ref Event, SC);
            else if (cmd == "IFG")
                ProcessIfGreater(ref Event, SC);
            else if (cmd == "IFKEY")
                ProcessIfKeyItem(ref Event, SC);
            else if (cmd == "IFYN")
                ProcessIfYesNo(ref Event, SC);
            else if (cmd == "ALERT")
                ProcessAlertCritters(ref Event, SC);
            else if (cmd == "GOLEFT")
                ProcessGoLeft(SC);
            else if (cmd == "GORIGHT")
                ProcessGoRight(SC);
            else if (cmd == "CHOKE")
                ProcessChoke(SC);
            else if (cmd == "CHANGETERR")
                ProcessChangeTerr(ref Event, SC);
        }
    }
}