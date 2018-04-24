using System;

public class pcaction
{
	/*This unit contains procedures which define the various*/
	/*actions that the PC can take.*/

	/*Most of these routines are boolean functions. A value*/
	/*of TRUE implies that the PC has taken some action;*/
	/*advance the time counter and let the monsters have their*/
	/*turn. A value of FALSE is for cancelled actions or game*/
	/*options; it doesn't use the PC's action.*/

    public static void PCMeleeAttack(gamebook.Scenario SC, int TX, int TY)
    {
        /*This procedure allows the PC to attack something with the*/
        /*equipped close combat weapon.*/

        /*Note that a person making melee attacks will burn up calories*/
        /*far more quickly than normal. But, it won't make you starve.*/
        if (SC.PC.carbs > 10)
            SC.PC.carbs -= 1;

        dccombat.AttackRequest AR = new dccombat.AttackRequest();
        AR.HitRoll = dcchars.PCMeleeSkill(SC.PC);
        AR.Damage = dcchars.PCMeleeDamage(SC.PC);
        AR.Range = -1;
        AR.Attacker = SC.PC.m;
        AR.TX = TX;
        AR.TY = TY;
        AR.DF = gamebook.DF_Physical;
        AR.C = Crt.Color.LightRed;
        AR.ATT = "";

        /*Generate the description for the PC's attack.*/
        if (SC.PC.eqp[dcchars.ES_MeleeWeapon - 1] == null)
        {
            if (rpgdice.Random(99) == 69)
                AR.Desc = "bite";
            else if (rpgdice.Random(3) == 2)
                AR.Desc = "kick";
            else
                AR.Desc = "punch";
        }
        else
        {
            AR.Desc = "swing " + dcitems.ItemNameShort(SC.PC.eqp[dcchars.ES_MeleeWeapon - 1]) + " at";
            AR.ATT = dcitems.CWep[SC.PC.eqp[dcchars.ES_MeleeWeapon - 1].icode - 1].ATT;
        }

        dccombat.ProcessAttack(SC, AR);
    }

    static bool IsClosedDoor(gamebook.Scenario SC, int X, int Y)
    {
        switch (texmaps.GetTerr(SC.gb, X, Y))
        {
            case texmaps.ClosedDoor: return true;
            case texmaps.ClosedServicePanel: return true;
        }

        return false;
    }

    public static bool PCOpenDoor(gamebook.Scenario SC)
    {
        /*This procedure first looks for a closed door in close*/
        /*proximity to the PC. If there is only one, that door is*/
        /*automatically selected for opening. If there is more than*/
        /*one, the player is prompted for a direction. If the door*/
        /*is locked, the player is informed of that fact.*/
        rpgtext.DCGameMessage("Open Door -");
        int DoorD = 0;

        int X = SC.PC.m.x;
        int Y = SC.PC.m.y;

        /*Locate the door*/
        for (int D = 1; D <= 9; ++D)
        {
            if (IsClosedDoor(SC, X + texmaps.VecDir[D - 1, 0], Y + texmaps.VecDir[D - 1, 1]))
            {
                if (DoorD == 0)
                    DoorD = D;
                else
                    DoorD = -1;
            }
        }

        /*Do some checks now on the state of our door.*/
        if (DoorD == -1)
        {
            rpgtext.DCAppendMessage(" Direction?");
            DoorD = rpgtext.DirKey();

            /* Check to make sure this location points to an actual door. */
            if (DoorD != 0 && !texmaps.OnTheMap(X + texmaps.VecDir[DoorD - 1, 0], Y + texmaps.VecDir[DoorD - 1, 1]))
            {
                DoorD = 0;
            }
            else if (DoorD != 0 && !IsClosedDoor(SC, X + texmaps.VecDir[DoorD - 1, 0], Y + texmaps.VecDir[DoorD - 1, 1]))
            {
                DoorD = 0;
            }
        }

        if (DoorD == 0)
        {
            /*No door found. Inform the player of this.*/
            rpgtext.DCAppendMessage(" Not found!");
            return false;
        }

        SC.gb.map[X + texmaps.VecDir[DoorD - 1, 0] - 1, Y + texmaps.VecDir[DoorD - 1, 1] - 1].terr -= 1;
        texmaps.DisplayTile(SC.gb, X + texmaps.VecDir[DoorD - 1, 0], Y + texmaps.VecDir[DoorD - 1, 1]);
        texmaps.UpdatePOV(SC.gb.POV, SC.gb);
        texmaps.ApplyPOV(SC.gb.POV, SC.gb);
        rpgtext.DCAppendMessage(" Done.");

        /*Check the Monster Memory*/
        CheckMonsterMemory(SC);

        return true;
    }

	static bool IsOpenDoor(gamebook.Scenario SC, int X, int Y)
    {
		switch(texmaps.GetTerr(SC.gb, X, Y))
        {
            case texmaps.OpenDoor: return true;
			case texmaps.OpenServicePanel: return true;
		}

        return false;
	}


    public static bool PCCloseDoor(gamebook.Scenario SC)
    {
        /*This procedure first looks for a closed door in close*/
        /*proximity to the PC. If there is only one, that door is*/
        /*automatically selected for opening. If there is more than*/
        /*one, the player is prompted for a direction. If the door*/
        /*is locked, the player is informed of that fact.*/
        rpgtext.DCGameMessage("Close Door -");
        int DoorD = 0;

        int X = SC.PC.m.x;
        int Y = SC.PC.m.y;

        /*Locate the door*/
        for (int D = 1; D <= 9; ++D)
        {
            if (IsOpenDoor(SC, X + texmaps.VecDir[D - 1, 0], Y + texmaps.VecDir[D - 1, 1]))
            {
                if (DoorD == 0)
                    DoorD = D;
                else
                    DoorD = -1;
            }
        }

        /*Do some checks now on the state of our door.*/
        if (DoorD == -1)
        {
            rpgtext.DCAppendMessage(" Direction?");
            DoorD = rpgtext.DirKey();

            if (DoorD != 0 && !texmaps.OnTheMap(X + texmaps.VecDir[DoorD - 1, 0], Y + texmaps.VecDir[DoorD - 1, 1]))
            {
                DoorD = 0;
            }
            else if (DoorD != 0 && !IsOpenDoor(SC, X + texmaps.VecDir[DoorD - 1, 0], Y + texmaps.VecDir[DoorD - 1, 1]))
            {
                DoorD = 0;
            }
        }

        if (DoorD == 0)
        {
            /*No door found. Inform the player of this.*/
            rpgtext.DCAppendMessage(" Not found!");
            return false;
        }

        /*One last check to perform- if there's a model in the way,*/
        /*the door can't be closed.*/
        if (!texmodel.ModelPresent(SC.gb.mog, X + texmaps.VecDir[DoorD - 1, 0], Y + texmaps.VecDir[DoorD - 1, 1]))
        {
            SC.gb.map[X + texmaps.VecDir[DoorD - 1, 0] - 1, Y + texmaps.VecDir[DoorD - 1, 1] - 1].terr += 1;
            texmaps.DisplayTile(SC.gb, X + texmaps.VecDir[DoorD - 1, 0], Y + texmaps.VecDir[DoorD - 1, 1]);
            texmaps.UpdatePOV(SC.gb.POV, SC.gb);
            texmaps.ApplyPOV(SC.gb.POV, SC.gb);
            rpgtext.DCAppendMessage(" Done.");
        }
        else
        {
            rpgtext.DCAppendMessage(" Blocked!");
        }

        return true;
    }

    public static bool PCMove(gamebook.Scenario SC, int D)
    {
        /*This procedure allows the PC to walk.*/


        int X2 = SC.PC.m.x + texmaps.VecDir[D - 1, 0];
        int Y2 = SC.PC.m.y + texmaps.VecDir[D - 1, 1];
        texmaps.WalkReport WR = texmaps.MoveModel(SC.PC.m, SC.gb, X2, Y2);

        if (WR.m != null)
        {
            if (WR.m.kind == critters.MKIND_Critter)
            {
                PCMeleeAttack(SC, WR.m.x, WR.m.y);
            }
            else if (WR.m.kind == cwords.MKIND_MPU)
            {
                mdlogon.MDSession(SC, WR.m);
            }
            else if (WR.m != SC.PC.m)
            {
                rpgtext.DCGameMessage("Blocked.");
            }
        }
        else if (WR.go)
        {
            /*Add special terrain messages and effects here.*/
            if (SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1] != null)
            {
                /*There's an item on the floor here.*/
                if (SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1].next != null)
                {
                    /*There are multiple items on the floor here.*/
                    rpgtext.DCGameMessage("There are several items on the floor here.");
                }
                else
                {
                    /*There is a single item on the floor here.*/
                    if (dcitems.Mergeable(SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1]) && SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1].charge > 1)
                    {
                        rpgtext.DCGameMessage("You find " + dcitems.ItemNameLong(SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1]) + " on the floor here.");
                    }
                    else
                    {
                        rpgtext.DCGameMessage("There is a " + dcitems.ItemNameShort(SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1]) + " on the floor here.");
                    }
                }
            }
            else if (texmaps.GetTerr(SC.gb, SC.PC.m.x, SC.PC.m.y) == 10)
            {
                if (rpgdice.Dice(10) > StepOnBlood)
                    rpgtext.DCGameMessage("There are blood stains on the floor here.");
                if (StepOnBlood < 20)
                    StepOnBlood += 1;
            }
            else if (texmaps.GetTerr(SC.gb, SC.PC.m.x, SC.PC.m.y) == texmaps.Chair)
            {
                rpgtext.DCGameMessage("There is a chair here.");
            }

            if (WR.trap != 0)
            {
                /*There's a trap here. The PC might just set it off!*/
                MightActivateTrap(SC);
            }

            /*Make sure the PC is still alive before providing*/
            /*more information.*/
            if (SC.PC.HP > 0)
            {
                /*Check the Monster Memory*/
                CheckMonsterMemory(SC);
                CheckForTraps(SC, 0);
                CheckForSecretDoors(SC, 0);

                /*Activate any plot points here.*/
                if (SC.gb.map[SC.PC.m.x - 1, SC.PC.m.y - 1].special > 0)
                {
                    gamebook.SetTrigger(SC, gamebook.PLT_MapSquare, SC.gb.map[SC.PC.m.x - 1, SC.PC.m.y - 1].special);
                }
            }
        }
        else if (WR.m == null && texmaps.TerrPass[texmaps.GetTerr(SC.gb, X2, Y2) - 1] > 0)
        {
            /*We haven't moved, and we didn't hit a model, so...*/
            /*it must be terrain stopping us!!!*/
            rpgtext.DCGameMessage("Slow progress...");
        }
        else if (D != 5)
        {
            /*This is a catch all, for walls and other miscellaneous things.*/
            rpgtext.DCGameMessage("Blocked.");
        }

        return true;
    }

    public static bool PCReCenter(gamebook.Scenario SC)
    {
	    /*The player wants to recenter the display on his current position.*/
	    /*Facilitate this request.*/
	    texmaps.RecenterPOV(SC.gb);
	    texmaps.DisplayMap(SC.gb);
	    CheckMonsterMemory(SC);

        return false;
    }

    public static bool PCShooting(gamebook.Scenario SC, bool SeekModel)
    {
        /*The player wants to shoot something. Select a target and*/
        /*let fly!*/


        /*Error check- make sure the PC has a missile weapon equipped!*/
        if (SC.PC.eqp[dcchars.ES_MissileWeapon - 1] == null)
        {
            rpgtext.DCGameMessage("No missile weapon equipped!");
            return false;
        }
        else if (SC.PC.eqp[dcchars.ES_MissileWeapon - 1].charge == 0)
        {
            rpgtext.DCGameMessage("Out of ammo!");
            return false;
        }

        rpgtext.DCGameMessage("Targeting - Select Target: ");
        texmaps.Point TP = looker.SelectPoint(SC, true, SeekModel, SC.PC.target);

        /*Check to make sure a target was selected, and also*/
        /*the the player isn't trying to shoot himself.*/
        if (TP.x == -1)
            return false;
        if (TP.x == SC.PC.m.x && TP.y == SC.PC.m.y)
            return false;

        dccombat.AttackRequest AR = new dccombat.AttackRequest();
        AR.HitRoll = dcchars.PCMissileSkill(SC.PC);
        AR.Damage = dcchars.PCMissileDamage(SC.PC);
        AR.Range = dcchars.PCMissileRange(SC.PC);
        AR.Attacker = SC.PC.m;
        AR.TX = TP.x;
        AR.TY = TP.y;
        AR.DF = gamebook.DF_Physical;
        AR.C = Crt.Color.LightRed;
        if (SC.PC.eqp[dcchars.ES_MissileWeapon - 1].state != 0)
        {
            /*If special ammunition is being used, add its attack attributes to the string.*/
            AR.Damage = AR.Damage + dcitems.CSpecAmmo[Math.Abs(SC.PC.eqp[dcchars.ES_MissileWeapon - 1].state) - 1].DMG;
            if (AR.Damage < 1)
                AR.Damage = 1;
            AR.HitRoll = AR.HitRoll + dcitems.CSpecAmmo[Math.Abs(SC.PC.eqp[dcchars.ES_MissileWeapon - 1].state) - 1].ACC;
            if (AR.HitRoll < 1)
                AR.HitRoll = 1;
            AR.ATT = dcitems.CSpecAmmo[Math.Abs(SC.PC.eqp[dcchars.ES_MissileWeapon - 1].state) - 1].ATT + dcitems.CGuns[SC.PC.eqp[dcchars.ES_MissileWeapon - 1].icode - 1].ATT;
        }
        else
        {
            AR.ATT = dcitems.CGuns[SC.PC.eqp[dcchars.ES_MissileWeapon - 1].icode - 1].ATT;
        }
        if (AR.ATT.Contains(spells.AA_LineAttack) || AR.ATT.Contains(spells.AA_BlastAttack) || AR.ATT.Contains(spells.AA_SmokeAttack))
            AR.Desc = "fire " + dcitems.ItemNameShort(SC.PC.eqp[dcchars.ES_MissileWeapon - 1]);
        else
            AR.Desc = "fire " + dcitems.ItemNameShort(SC.PC.eqp[dcchars.ES_MissileWeapon - 1]) + " at";

        dccombat.AttackReport Rep = dccombat.ProcessAttack(SC, AR);

        /*Reduce the weapon's AMMO count, unless using infinite shot weapon.*/
        if (SC.PC.eqp[dcchars.ES_MissileWeapon - 1].charge > -1)
            SC.PC.eqp[dcchars.ES_MissileWeapon - 1].charge -= 1;

        return true;
    }

    public static bool PCTosser(gamebook.Scenario SC)
    {
        /*The PC wants to throw a grenade.*/
        /*The majority of this unit was simply copied from above.*/

        /*Select a grenade to toss.*/
        dcitems.DCItem Grn = backpack.PromptItem(SC, dcitems.IKIND_Grenade);
        if (Grn == null)
            return false;

        /*Start the standard firing stuff.*/
        rpgtext.DCGameMessage("Throw grenade - Select Target: ");
        texmaps.Point TP = looker.SelectPoint(SC, true, true, SC.PC.target);

        /*Check to make sure a target was selected, and also*/
        /*the the player isn't trying to shoot himself.*/
        if (TP.x == -1)
            return false;
        if (TP.x == SC.PC.m.x && TP.y == SC.PC.m.y)
            return false;

        /*Check to make sure the target point is within the PC's*/
        /*maximum throwing range.*/
        if (texmaps.Range(SC.PC.m, TP.x, TP.y) > dcchars.PCThrowRange(SC.PC))
        {
            rpgtext.DCPointMessage("Out of range!");
            return false;
        }

        dccombat.AttackRequest AR = new dccombat.AttackRequest();
        AR.HitRoll = dcchars.PCThrowSkill(SC.PC);
        AR.Damage = dcitems.CGrn[Grn.icode - 1].DMG;
        AR.Range = dcitems.CGrn[Grn.icode - 1].DMG;
        AR.Attacker = SC.PC.m;
        AR.TX = TP.x;
        AR.TY = TP.y;
        AR.DF = gamebook.DF_Physical;
        AR.C = Crt.Color.Yellow;
        AR.ATT = dcitems.CGrn[Grn.icode].ATT;
        if (AR.ATT.Contains(spells.AA_LineAttack) || AR.ATT.Contains(spells.AA_BlastAttack) || AR.ATT.Contains(spells.AA_SmokeAttack))
            AR.Desc = "throw " + dcitems.ItemNameShort(Grn);
        else
            AR.Desc = "throw " + dcitems.ItemNameShort(Grn) + " at";

        dccombat.AttackReport Rep = dccombat.ProcessAttack(SC, AR);

        /*Consume the grenade.*/
        dcitems.ConsumeDCItem(ref SC.PC.inv, Grn, 1);

        return true;
    }

    public static bool PCInvScreen(gamebook.Scenario SC, bool StartWithInv)
    {
	    /*Do the PC's backpack.Inventory screen. I moved that to a separate*/
	    /*unit, since the whole shebang is a little involved.*/
	    backpack.Inventory(SC,StartWithInv);
	    PCReCenter(SC);
	    gamebook.PCStatLine(SC);
        return false;
    }

    public static bool PCPickUp(gamebook.Scenario SC)
    {
        /*The PC is gonna pick up something. Return FALSE if there*/
        /*is no item present, or if the picking up is canceled.*/
        bool it = false;

        rpgtext.DCGameMessage("Get Item - ");

        if (SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1] != null)
        {
            /*There's at least one item here. See if there's more.*/
            if (SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1].next == null)
            {
                /*Simple case. There's only one item here.*/
                /*Grab it.*/
                dcitems.DCItem I = SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1];
                rpgtext.DCAppendMessage("Got " + dcitems.ItemNameLong(I) + ".");
                dcitems.RetrieveDCItem(SC.gb, SC.ig, I, SC.PC.m.x, SC.PC.m.y);
                dcitems.MergeDCItem(ref SC.PC.inv, I);
                it = true;
            }
            else
            {
                /*Difficult case. There's multiple items.*/
                /*List through them and prompt for picking up.*/
                rpgtext.DCAppendMessage("Multiple items.");
                dcitems.DCItem I2 = SC.ig.g[SC.PC.m.x - 1, SC.PC.m.y - 1];
                while (I2 != null)
                {
                    dcitems.DCItem I = I2;
                    I2 = I2.next;
                    rpgtext.DCGameMessage("Pick up " + dcitems.ItemNameLong(I) + "? (Y/N)");
                    if (rpgtext.YesNo())
                    {
                        dcitems.RetrieveDCItem(SC.gb, SC.ig, I, SC.PC.m.x, SC.PC.m.y);
                        dcitems.MergeDCItem(ref SC.PC.inv, I);
                        rpgtext.DCAppendMessage(" Done.");
                    } else
                    {
                        rpgtext.DCAppendMessage(" Nope.");
                    }
                }
            }
        }
        else
        {
            rpgtext.DCAppendMessage("Not found!");
        }

        return it;
    }

    public static bool PCDisarmTrap(gamebook.Scenario SC)
    {
	    /*The player wants to disarm a trap. Make the appropriate*/
	    /*roll, then either remove the selected trap (success) or*/
	    /*walk the player over onto it (failure).*/
	    rpgtext.DCGameMessage("Disarm Trap -");
	    int TrapD = 0;

	    int X = SC.PC.m.x;
	    int Y = SC.PC.m.y;

	    /*Locate the trap*/
	    for (int D = 1; D <= 9; ++D)
        {
		    if (SC.gb.map[X + texmaps.VecDir[D-1,0] - 1,Y + texmaps.VecDir[D-1,1]-1].trap > 0)
            {
                if (TrapD == 0)
                    TrapD = D;
                else
                    TrapD = -1;
		    }
	    }

	    /*Do some checks now on the state of our trap.*/
	    if (TrapD == -1)
        {
		    rpgtext.DCAppendMessage(" Direction?");

		    TrapD = rpgtext.DirKey();

		    if (TrapD != 0 && !texmaps.OnTheMap(X + texmaps.VecDir[TrapD-1,0],Y + texmaps.VecDir[TrapD-1,1]))
            {
			    TrapD = 0;
		    }
            else if (TrapD !=0 && SC.gb.map[X + texmaps.VecDir[TrapD-1,0]-1,Y + texmaps.VecDir[TrapD-1,1]-1].trap < 1)
            {
			    TrapD = 0;
		    }
	    }

	    if (TrapD == 0)
        {
		    /*No trap found. Inform the player of this.*/
		    rpgtext.DCAppendMessage(" Not found!");
            return false;
	    }

	    /*Set X and Y to the location of the trap.*/
	    X = X + texmaps.VecDir[TrapD-1,0];
	    Y = Y + texmaps.VecDir[TrapD-1,1];

	    /*One last check to perform- if there's a model in the way,*/
	    /*the trap can't be disarmed.*/
	    if (!texmodel.ModelPresent(SC.gb.mog, X, Y))
        {
		    /*Do the trap disarming stuff here.*/
		    /*Roll the skill dice first.*/
		    int DT = rpgdice.RollStep(dcchars.PCDisarmSkill(SC.PC));

		    if (DT >= dccombat.TrapMan[Math.Abs(SC.gb.map[X-1,Y-1].trap) - 1].Disarm)
            {
			    /*The player gets some XP for having done this.*/
			    gamebook.DoleExperience(SC,dccombat.TrapMan[Math.Abs(SC.gb.map[X-1,Y-1].trap)-1].Disarm / 2);

			    /*Disarming was successful.*/
			    SC.gb.map[X-1,Y-1].trap = 0;
			    texmaps.DisplayTile(SC.gb,X,Y);
			    rpgtext.DCAppendMessage("Done.");

			    /*A Player who disarms a trap will stop repeated actions.*/
			    SC.PC.repCount = 0;
		    }
            else if (DT < dccombat.TrapMan[Math.Abs(SC.gb.map[X-1,Y-1].trap)-1].Disarm / 3)
            {
			    /*Disarming critically failed.*/
			    rpgtext.DCAppendMessage("Failed.");
			    PCMove(SC,TrapD);

			    /*A Player who sets off a trap will stop repeated actions.*/
			    SC.PC.repCount = 0;
		    }
            else
            {
			    /*Disarming failed.*/
			    rpgtext.DCAppendMessage("Failed.");
		    }
	    }
        else
        {
		    rpgtext.DCAppendMessage(" Blocked!");
	    }

        return false;
    }

    public static bool PCSearch(gamebook.Scenario SC)
    {
	    /*The player wants to perform a deliberate search for*/
	    /*traps and secret doors.*/
	    rpgtext.DCGameMessage("Searching...");
	    CheckForTraps(SC,1);
	    CheckForSecretDoors(SC,1);
        return true;
    }

    public static bool PCUsePsi(gamebook.Scenario SC, bool UseMenu)
    {
        /*The PC wants to invoke a psychic power. Call the procedure*/
	    /*in the psi powers unit...*/
	    return zapspell.CastSpell(SC,UseMenu);
    }

    public static bool PCLookAround(gamebook.Scenario SC)
    {
        /*The PC is just looking at the stuff around.*/
        rpgtext.DCGameMessage("Look: ");
        looker.SelectPoint(SC, false, false, SC.PC.target);
        rpgtext.DCPointMessage("Done.");
        return true;
    }

    public static bool PCEnter(gamebook.Scenario SC)
    {
	    /*The player just hit the "enter location" key- < or >*/
	    /*All this procedure does is to set up a trigger.*/
	    gamebook.SetTrigger(SC, gamebook.PLT_EnterCom, texmaps.GetTerr(SC.gb,SC.PC.m.x,SC.PC.m.y));
        return true;
    }

    public static bool PCRepeat(gamebook.Scenario SC)
    {
	    /*The PC wants to do something or another repeatedly.*/
	    SC.PC.repCount = 80;
	    SC.PC.repState = 0;
        return false;
    }

    public static bool PCProcessRepeat(gamebook.Scenario SC)
    {
        /*The PC has set up a repeating action. Process it.*/

        if (SC.PC.lastCmd == rpgtext.KMap[1].key) RepMove(SC, 1);
        else if (SC.PC.lastCmd == rpgtext.KMap[2].key) RepMove(SC, 2);
        else if (SC.PC.lastCmd == rpgtext.KMap[3].key) RepMove(SC, 3);
        else if (SC.PC.lastCmd == rpgtext.KMap[4].key) RepMove(SC, 4);
        else if (SC.PC.lastCmd == rpgtext.KMap[5].key) RepMove(SC, 5);
        else if (SC.PC.lastCmd == rpgtext.KMap[6].key) RepMove(SC, 6);
        else if (SC.PC.lastCmd == rpgtext.KMap[7].key) RepMove(SC, 7);
        else if (SC.PC.lastCmd == rpgtext.KMap[8].key) RepMove(SC, 8);
        else if (SC.PC.lastCmd == rpgtext.KMap[9].key) RepMove(SC, 9);
        else if (SC.PC.lastCmd == rpgtext.KMap[19].key) PCSearch(SC);
        /*If the command entered was not a repeatable one,*/
        /*set the counter to 0.*/
        else SC.PC.repCount = 0;

        return true;
    }

    public static bool PCCheckXP(gamebook.Scenario SC)
    {
        /*Display XP level, current XP, and XP needed.*/
        rpgtext.DCGameMessage(String.Format("Level {0} : {1} / {2} XP.", SC.PC.lvl, SC.PC.XP, gamebook.XPNeeded(SC.PC.lvl + 1)));

        return false;
    }

    static string[] QD = {
        "miserable","bad","fair","okay","good",
        "very good","excellent"
    };
    static string[] FSN = {
        "Fighting","Shooting","Detection","Stealth",
        "Hacking","Technology","Identify","Psi Control"
    };

    static int[] FSI = {
        dcchars.SKILL_MeleeAttack, dcchars.SKILL_MissileAttack, dcchars.SKILL_Detection, dcchars.SKILL_Stealth,
        dcchars.SKILL_DisarmTrap, dcchars.SKILL_Technical, dcchars.SKILL_Identify, dcchars.SKILL_PsiSkill
    };

    public static bool PCInfoScreen(gamebook.Scenario SC)
    {
        /*Display all important data for the PC.*/
        const int C2X1 = 41;
        const int Y1 = 5;

        texmaps.ClearMapArea();

        Crt.TextColor(Crt.Color.LightGreen);
        Crt.GotoXY(5, Y1);
        Crt.Write(SC.PC.name + "\n");
        Crt.TextColor(Crt.Color.Green);
        Crt.Write("          level " + SC.PC.lvl.ToString() + " " + dcchars.JobName[SC.PC.job - 1]);
        Crt.GotoXY(45, Y1 + 1);
        Crt.Write("HP: " + SC.PC.HP.ToString() + "/" + SC.PC.HPMax.ToString());
        Crt.GotoXY(60, Y1 + 1);
        Crt.Write("MP: " + SC.PC.MP.ToString() + "/" + SC.PC.MPMax.ToString());

        for (int T = 1; T <= 8; ++T)
        {
            Crt.GotoXY(5, Y1 + 2 + T);
            Crt.TextColor(Crt.Color.Green);
            Crt.Write(dcchars.StatName[T - 1]);
            Crt.GotoXY(20, Y1 + 2 + T);
            Crt.TextColor(Crt.Color.LightGreen);
            Crt.Write(SC.PC.stat[T - 1].ToString());
        }

        /* Display the Featured Skills */
        for (int T = 1; T <= FSN.Length; ++T)
        {
            Crt.GotoXY(C2X1, Y1 + 2 + T);
            Crt.TextColor(Crt.Color.Green);
            Crt.Write(FSN[T - 1]);
            Crt.GotoXY(C2X1 + 12, Y1 + 2 + T);
            Crt.TextColor(Crt.Color.LightGreen);
            int N = SC.PC.skill[FSI[T - 1] - 1];
            if (N < 1)
                N = 1;
            else if (N > QD.Length)
                N = QD.Length;

            Crt.Write(QD[N - 1]);
        }

        rpgtext.RPGKey();

        /*Restore the display, and exit the function.*/
        PCReCenter(SC);
        gamebook.PCStatLine(SC);
        return false;
    }

    public static void ScanUnknownInv(gamebook.Scenario SC)
    {
        /* Check through the items in the PC's inventory, and try to ID any */
        /* of them that are currently unknown. */

        /* Initialize values. */
        bool didit = false;
        dcitems.DCItem I = SC.PC.inv;

        /* Scan inventory */
        while (I != null)
        {
            dcitems.DCItem I2 = I.next;

            /* If this item hasn't been identified, we want to examine it. */
            if (!I.ID)
            {
                charts.AttemptToIdentify(SC, I);

                /* If the attempt was successful, merge this item into */
                /* the main inventory. */
                if (I.ID)
                {
                    didit = true;
                    dcitems.DelinkDCItem(ref SC.PC.inv, I);
                    dcitems.MergeDCItem(ref SC.PC.inv, I);
                }
            }

            I = I2;
        }

        for (int t = 1; t <= dcchars.NumEquipSlots; ++t)
        {
            if (SC.PC.eqp[t - 1] != null && !SC.PC.eqp[t - 1].ID)
            {
                charts.AttemptToIdentify(SC, SC.PC.eqp[t - 1]);

                if (SC.PC.eqp[t - 1].ID)
                {
                    didit = true;
                }
            }
        }

        if (didit)
            rpgtext.DCGameMessage("You learn something about the items you are carrying...");
    }

    public static bool PCHandyMap(gamebook.Scenario SC)
    {
        /* Do the HandyMap display, then restore the view afterwards. */
        if (dcitems.HasItem(SC.PC.inv, dcitems.IKIND_Electronics, 1))
        {
            rpgtext.DCGameMessage("Accessing Handymap.");
            backpack.HandyMap(SC);

            /*Restore the display, and exit the function.*/
            PCReCenter(SC);
            gamebook.PCStatLine(SC);
        }
        else
        {
            rpgtext.DCGameMessage("You don't have a Handymap!");
        }

        return false;
    }

    public const int SpotDoorTarget = 30;
    public const int SpotTrapTarget = 25;
    public const int AvoidTrapTarget = 35;
    public const int AvoidVisibleTrap = 10;
    public static int StepOnBlood = 0; /*How many bloody tiles the PC has walked on.*/
                                       /*This constant is used to prevent boring the*/
                                       /*PC by printing the same message over and over again.*/

    static void CheckMonsterMemory(gamebook.Scenario SC)
    {
        /*The player has either moved or otherwise changed the*/
        /*field of vision.*/
        texmodel.Model M;
        critters.Critter C;

        /*Set the boundaries for our search.*/
        int X1 = SC.PC.m.x - SC.gb.POV.range;
        if (X1 < 1)
            X1 = 1;
        int Y1 = SC.PC.m.y - SC.gb.POV.range;
        if (Y1 < 1)
            Y1 = 1;
        int X2 = SC.PC.m.x + SC.gb.POV.range;
        if (X2 > texmodel.XMax)
            X2 = texmodel.XMax;
        int Y2 = SC.PC.m.y + SC.gb.POV.range;
        if (Y2 > texmodel.YMax)
            Y2 = texmodel.YMax;

        for (int X = X1; X <= X2; ++X)
        {
            for (int Y = Y1; Y <= Y2; ++Y)
            {
                /*First, check that the square is visible, there's a model present and that it's on the screen.*/
                if (texmaps.TileLOS(SC.gb.POV, X, Y) && texmodel.ModelPresent(SC.gb.mog, X, Y) && texmaps.OnTheScreen(SC.gb, X, Y))
                {
                    M = texmodel.FindModelXY(SC.gb.mlist, X, Y);
                    if (M.kind == critters.MKIND_Critter)
                    {
                        C = critters.LocateCritter(M, SC.CList);
                        if (C.Target == SC.PC.m)
                            SC.PC.repCount = 0;
                        if (!C.Spotted)
                        {
                            /*Seeing an unknown creature will cause the PC to stop repeditive actions.*/
                            SC.PC.repCount = 0;
                            gamebook.UpdateMonsterMemory(SC, C);
                        }
                    }
                }
            }
        }
    }

    static string[] TrapDetectionMsg =
    {
        "You have detected a trap!",
        "There are security countermeasures in use here.",
        "You notice a sensor beam."
    };

    static void CheckForTraps(gamebook.Scenario SC, int Mode)
    {
        /*Check the PC's immediate vicinity for traps. Reveal any*/
        /*that are found. Use Mode == 0 for walking, Mode == 1 for*/
        /*searching.*/
        for (int X = SC.PC.m.x - 1; X <= SC.PC.m.x + 1; ++X)
        {
            for (int Y = SC.PC.m.y - 1; Y <= SC.PC.m.y + 1; ++Y)
            {
                if (texmaps.OnTheMap(X, Y) && (SC.gb.map[X - 1, Y - 1].trap < 0))
                {
                    if (rpgdice.RollStep(dcchars.PCDetection(SC.PC)) >= (SpotTrapTarget - Mode * 10))
                    {
                        /*A trap has been detected!*/
                        rpgtext.DCGameMessage(TrapDetectionMsg[rpgdice.Random(TrapDetectionMsg.Length)]);
                        dccombat.RevealTrap(SC, X, Y);
                        rpgtext.GamePause();
                        /*A Player who detects a trap will stop repeated actions.*/
                        SC.PC.repCount = 0;
                        gamebook.DoleExperience(SC, 2);
                    }
                }
            }
        }
    }

    static void CheckForSecretDoors(gamebook.Scenario SC, int Mode)
    {
        /*Check the PC's immediate vicinity for secret doors. Reveal*/
        /*any that are found. Use Mode == 0 for walking, Mode == 1 for*/
        /*searching.*/
        for (int X = SC.PC.m.x - 1; X <= SC.PC.m.x + 1; ++X)
        {
            for (int Y = SC.PC.m.y - 1; Y <= SC.PC.m.y + 1; ++Y)
            {
                if (texmaps.OnTheMap(X, Y) && SC.gb.map[X - 1, Y - 1].terr == texmaps.HiddenServicePanel)
                {
                    if (rpgdice.RollStep(dcchars.PCDetection(SC.PC)) >= (SpotDoorTarget - Mode * 5))
                    {
                        /*A door has been detected!*/
                        rpgtext.DCGameMessage("You have discovered a service panel.");
                        SC.gb.map[X - 1, Y - 1].terr = texmaps.ClosedServicePanel;
                        texmaps.DisplayTile(SC.gb, X, Y);
                        rpgtext.GamePause();
                        gamebook.DoleExperience(SC, 1);
                        /*A Player who detects a secret door will stop repeated actions.*/
                        SC.PC.repCount = 0;
                    }
                }
            }
        }
    }

    static void MightActivateTrap(gamebook.Scenario SC)
    {
        /*The PC has just stepped on a trap. It might be activated.*/
        /*Check and see.*/

        /*R stands for Revealed. It's true if the trap is visible*/
        /*to the player, false if it's still hidden.*/
        bool R = SC.gb.map[SC.PC.m.x - 1, SC.PC.m.y - 1].trap > 0;

        /*A trap which the player has detected isn't likely to go off,*/
        /*but it still might. A trap which hasn't been detected*/
        /*by the PC will almost certainly go off.*/

        int LS = rpgdice.RollStep(dcchars.PCLuckSave(SC.PC));

        if (R && LS < AvoidVisibleTrap)
            dccombat.SpringTrap(SC, SC.PC.m.x, SC.PC.m.y);
        else if (!R && LS < AvoidTrapTarget)
            dccombat.SpringTrap(SC, SC.PC.m.x, SC.PC.m.y);
        else if (!R)
            dccombat.RevealTrap(SC, SC.PC.m.x, SC.PC.m.y);
    }

    static bool MoveBlocked(gamebook.Scenario SC, int MX, int MY)
    {
        /*Check location MX,MY to see if the PC can move there.*/
        return texmaps.TerrPass[texmaps.GetTerr(SC.gb, MX, MY) - 1] < 1 || texmodel.ModelPresent(SC.gb.mog, MX, MY);
    }

    static char[] MvCmd = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    static bool RepMove(gamebook.Scenario SC, int D)
    {
        /*The player is using a repeat command with movement.*/
        const int In_A_Room = 1;
        const int In_A_Hall = 2;

        /*Local Procedures*/

        bool Act = false;

        if (D == 5)
        {
            /*This is a special case.*/
            PCMove(SC, D);

            /*Check to see if the PC is fully recovered.*/
            /*If so, stop resting.*/
            if (SC.PC.HP >= SC.PC.HPMax && SC.PC.MP >= SC.PC.MPMax)
            {
                SC.PC.repCount = 0;
            }

            /*Exit before any of the rest of this stuff can execute.*/
            /*I know, I could stick the main part of this procedure*/
            /*in an ELSE BEGIN...END block, but this way of doing*/
            /*things looks prettier to me.*/
            return true;
        }

        /*Determine the Repeat State. If 0, we need to set it.*/
        if (SC.PC.repState == 0)
        {
            /*Determine whether the PC is in a hall or in*/
            /*a room.*/
            if (MoveBlocked(SC, SC.PC.m.x + texmaps.VecDir[D - 1, 1], SC.PC.m.y - texmaps.VecDir[D - 1, 0]) &&
                MoveBlocked(SC, SC.PC.m.x - texmaps.VecDir[D - 1, 1], SC.PC.m.y + texmaps.VecDir[D - 1, 0]))
            {
                SC.PC.repState = In_A_Hall;
            }
            else
            {
                SC.PC.repState = In_A_Room;
            }
        }

        if (SC.PC.repState == In_A_Hall)
        {
            int X = SC.PC.m.x + texmaps.VecDir[D - 1, 0];
            int Y = SC.PC.m.y + texmaps.VecDir[D - 1, 1];

            /* Movement will stop if the PC steps on an item or trap. */
            if (SC.ig.g[X - 1, Y - 1] != null || SC.gb.map[X - 1, Y - 1].trap > 0)
            {
                SC.PC.repCount = 0;
            }
            else if (MoveBlocked(SC, X, Y))
            {
                int D2 = 0;
                for (int N = 1; N <= 9; ++N)
                {
                    if (!MoveBlocked(SC, SC.PC.m.x + texmaps.VecDir[N - 1, 0], SC.PC.m.y + texmaps.VecDir[N - 1, 1]))
                    {
                        /*Check to make sure this isn't the same direction we just came from.*/
                        if (N != (10 - D))
                        {
                            if (D2 == 0)
                                D2 = N;
                            else
                                D2 = -1;
                        }
                    }
                }

                if (D2 > 0)
                {
                    /*There's only one direction to go in.*/
                    SC.PC.lastCmd = MvCmd[D2];
                    PCMove(SC, D2);
                    Act = true;
                }
                else
                {
                    /*End the movement here.*/
                    SC.PC.repCount = 0;
                }
            }
            else
            {
                /*Movement isn't blocked, but maybe there's*/
                /*an intersection here. That will stop movement too.*/
                /*Check the two normals to see if that is the case.*/
                if (!MoveBlocked(SC, SC.PC.m.x + texmaps.VecDir[D - 1, 1], SC.PC.m.y - texmaps.VecDir[D - 1, 0]))
                {
                    SC.PC.repCount = 0;
                }
                else if (!MoveBlocked(SC, SC.PC.m.x - texmaps.VecDir[D - 1, 1], SC.PC.m.y + texmaps.VecDir[D - 1, 0]))
                {
                    SC.PC.repCount = 0;
                }
                else
                {
                    PCMove(SC, D);
                    Act = true;
                }
            }
        }
        else
        {
            /*The PC must be in a room. Or something is*/
            /*seriously wrong. In any case, assume a room,*/
            /*since that's the safest bet.*/
            int X = SC.PC.m.x + texmaps.VecDir[D - 1, 0];
            int Y = SC.PC.m.y + texmaps.VecDir[D - 1, 1];

            if (MoveBlocked(SC, X, Y) || SC.ig.g[X - 1, Y - 1] != null || SC.gb.map[X - 1, Y - 1].trap > 0)
            {
                /* The path is blocked, or there's an item on the */
                /* floor, or a visible trap or something. */
                /* Movement ends here.*/
                SC.PC.repCount = 0;
            }
            else
            {
                /*Actually complete the movement.*/
                PCMove(SC, D);
                Act = true;
            }
        }

        return Act;
    }

}