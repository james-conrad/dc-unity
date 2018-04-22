using System;

public class cbrain
{
	/*This unit handles critter behavior and stuff.*/

    public static void CritterAction(gamebook.Scenario SC, ref critters.Critter C)
    {
	    /*Critter C is about to perform some sort of action. Yay!*/
	    /*Decide what it's gonna do based on its AI type.*/
	    SC.CAct = C;

	    if (CritterActive(C))
        {
		    if (critters.MonMan[C.crit - 1].CT.Contains(critters.XCT_Breeder) && rpgdice.rng.Next(15) == 1)
            {
			    TryToBreed(SC , C);
		    }
            else if (C.Target != null && C.AIType != critters.AIT_Slime)
            {
			    ActAgressive(SC,C);
		    }
            else
            {
			    switch (C.AIType)
                {
				    case critters.AIT_Passive: ActPassive(SC); break;
				    case critters.AIT_PCHunter: ActPCHunter(SC,C); break;
				    case critters.AIT_Chaos: ActChaos(SC,C); break;
				    case critters.AIT_Guardian: ActGuardian(SC,C); break;
				    case critters.AIT_Slime: ActSlimy(SC,C); break;
				    case critters.AIT_HalfHunter: ActHalfHunter(SC,C); break;
			    }
		    }
	    }

	    /*Update the critter's status.*/
	    if (plotbase.NAttValue(C.SF,statusfx.NAG_StatusChange,statusfx.SEF_Poison) != 0)
        {
		    C.HP -= rpgdice.rng.Next(6);
	    }

        statusfx.UpdateStatusList(ref C.SF);

	    if (C.HP < 0)
            dccombat.CritterDeath(SC,C,false);

        C = SC.CAct;
	    SC.CAct = null;
    }

    public static void BrownianMotion(gamebook.Scenario SC)
    {
	    /*All of the clouds in the FOG section of the scenario are gonna*/
	    /*drift around lonely as a cloud. Forcewalls are just gonna sit*/
	    /*where they are.*/
	    cwords.Cloud C = SC.Fog;

	    while (C != null)
        {
		    /*Save the location of the next cloud in the list.*/
		    cwords.Cloud C2 = C.next;

            if (SC.ComTime >= C.Duration)
            {
                /*This cloud has reached the } of its lifespan.*/
                gamebook.Excommunicate(SC, C.M);
                cwords.RemoveCloud(ref SC.Fog, C, SC.gb);
            }
		    else if (cwords.CloudMan[C.Kind-1].pass)
            {
			    /*Clouds which cannot be moved through are forcewalls,*/
			    /*and stay where they're to. Other clouds drift.*/
			    /*Do drifting now.*/
			    if (rpgdice.rng.Next(3) != 1)
                {
				    texmaps.MoveModel(C.M,SC.gb,C.M.x+rpgdice.rng.Next(2)-rpgdice.rng.Next(2),C.M.y+rpgdice.rng.Next(2)-rpgdice.rng.Next(2));
			    }
		    }

		    /*Move to the next cloud.*/
		    C = C2;
	    }
    }

    public static int AvoidTrapTarget = 15;	/*Avoiding traps is easier for critters.*/

    static texmaps.WalkReport WalkCritter(gamebook.Scenario SC, int DX, int DY)
    {
        /*Move the monster to wherever it's going. NOTE: C might be*/
        /*made null by this static void, if killed by a trap!*/

	    texmaps.WalkReport it;

	    /*Is there a door in the way? If so, forget movement... Open*/
	    /*the door instead. If the monster can't open the door, attack*/
	    /*it and maybe it'll be destroyed.*/

	    /*Perform the movement.*/
	    it = texmaps.MoveModel(SC.CAct.M,SC.gb,SC.CAct.M.x + DX,SC.CAct.M.y + DY);
	    if (it.go && !SC.CAct.Spotted)
        {
		    if (texmaps.TileLOS(SC.gb.POV, SC.CAct.M.x,SC.CAct.M.y) && texmaps.OnTheScreen(SC.gb, SC.CAct.M.x, SC.CAct.M.y))
            {
			    gamebook.UpdateMonsterMemory(SC, SC.CAct);
		    }
	    }

        /*Check for traps here. Robots & Zombies don't set off traps;*/
        /*other critter types might.*/
        if (it.go && SC.gb.map[SC.CAct.M.x - 1, SC.CAct.M.y - 1].trap != 0 && SC.CAct.M.gfx != 'R' && SC.CAct.M.gfx != '@')
        {
            if (rpgdice.RollStep(critters.MonMan[SC.CAct.crit - 1].Sense) < AvoidTrapTarget)
                dccombat.SpringTrap(SC, SC.CAct.M.x, SC.CAct.M.y);
        }

        return it;
    }

    static bool GetLockOnPC(gamebook.Scenario SC, critters.Critter C)
    {
        /*The critter in question is trying to get a "lock" on*/
        /*the PC. Make a Sense roll against the PC's Stealth*/
        /*skill.*/

        /*If the player is out of range, we can't get a lock.*/
        if (texmaps.Range(C.M, SC.PC.m) > critters.MonMan[C.crit - 1].Sense * 2)
            return false;

	    int O = texmaps.CalcObscurement(C.M,SC.PC.m,SC.gb);
        if (O > -1)
        {
            if (rpgdice.RollStep(dcchars.PCStealth(SC.PC)) < rpgdice.RollStep(critters.MonMan[C.crit - 1].Sense) - O)
            {
                return true;
            }
        }

        return false;
    }

    static bool MoveOK(gamebook.Scenario SC, int X, int Y)
    {
	    /*Answer the question- is it okay to move here?*/
	    /*{ by assuming that the move is OK.*/
	    bool it = true;

	    /*Check for models in the target space.*/
	    if (texmodel.ModelPresent(SC.gb.mog, X, Y))
        {
		    it = texmodel.FindModelXY(SC.gb.mlist,X,Y).coHab;
	    }

	    if (texmaps.TerrPass[SC.gb.map[X-1,Y-1].terr -1] < 1)
        {
            it = false;
        }

        return it;
    }

    static int TacRange(critters.Critter C)
    {
        /*Given critter C, determine its effective combat range.*/
	    if (C.Eqp != null && C.Eqp.ikind == dcitems.IKIND_Gun)
            return dcitems.CGuns[C.Eqp.icode - 1].RNG;

        return critters.MonMan[C.crit - 1].Range;
    }

    static void CritterAttack(gamebook.Scenario SC, critters.Critter C, int TX, int TY)
    {
        /*Critter C wants to attack whatever is in square TX,TY.*/

        /*Fill out the Attack Request.*/
        dccombat.AttackRequest AR = new dccombat.AttackRequest();
        AR.Attacker = C.M;
	    AR.TX = TX;
	    AR.TY = TY;
	    AR.DF = gamebook.DF_Physical;
	    AR.C = Crt.Color.LightRed;

	    /*Fill out the rest of the data dep}ant upon what equipment*/
	    /*the creature is using.*/
	    if (C.Eqp != null && C.Eqp.ikind == dcitems.IKIND_Gun)
        {
		    AR.HitRoll = critters.MonMan[C.crit - 1].HitRoll + dcitems.CGuns[C.Eqp.icode - 1].ACC;
		    AR.Damage = dcitems.CGuns[C.Eqp.icode - 1].DMG;
		    AR.Range = dcitems.CGuns[C.Eqp.icode - 1].RNG;
		    AR.ATT = dcitems.CGuns[C.Eqp.icode - 1].ATT;

            if (AR.ATT.Contains(spells.AA_LineAttack))
                AR.Desc = "fires " + dcitems.ItemNameShort(C.Eqp);
            else
                AR.Desc = "fires " + dcitems.ItemNameShort(C.Eqp) + " at";
	    }
        else if (C.Eqp != null && C.Eqp.ikind == dcitems.IKIND_Wep)
        {
		    AR.HitRoll = critters.MonMan[C.crit - 1].HitRoll + dcitems.CWep[C.Eqp.icode - 1].ACC;
		    AR.Damage = critters.MonMan[C.crit - 1].Damage + dcitems.CWep[C.Eqp.icode - 1].DMG;
		    AR.Range = -1;
		    AR.Desc = "swings " + dcitems.ItemNameShort(C.Eqp) + " at";
		    AR.ATT = dcitems.CWep[C.Eqp.icode - 1].ATT;

	    }
        else
        {
		    AR.HitRoll = critters.MonMan[C.crit - 1].HitRoll;
		    AR.Damage = critters.MonMan[C.crit - 1].Damage;
		    AR.Range = critters.MonMan[C.crit - 1].Range;
		    AR.Desc = critters.MonMan[C.crit - 1].ADesc;
		    AR.ATT = critters.MonMan[C.crit - 1].AtAt;
	    }

	    /*Process the attack. If a fatality is inflicted on the*/
	    /*critter's target, the Excommunicate static void will reset*/
	    /*the target field to null.*/
	    dccombat.ProcessAttack(SC,AR);
    }

    static void ActPassive(gamebook.Scenario SC)
    {
	    /*The critter is gonna be acting passively right now.*/
	    /*Move it in a rpgdice.rng.Next direction; don't attack anything.*/
	    int D = rpgdice.rng.Next(9) + 1;

	    if (D != 5)
        {
		    int t = 1;
		    while (!MoveOK(SC,SC.CAct.M.x+texmaps.VecDir[D-1, 0], SC.CAct.M.y + texmaps.VecDir[D - 1, 1]) && t <= 3)
            {
			    D = rpgdice.rng.Next(8) + 1;
			    if (D > 4)
                    D = D + 1;
                t += 1;
		    }
	    }

	    WalkCritter(SC,texmaps.VecDir[D-1,0],texmaps.VecDir[D-1,1]);
    }

    static void ActAgressive(gamebook.Scenario SC, critters.Critter C)
    {
	    /*This critter apparently has a target. Try to get as*/
	    /*close to it as possible.*/

	    /*Check, first of all, that we have a target.*/
	    if (C.Target == null)
        {
		    ActPassive(SC);
            return;
	    }

	    /*Next, on a random whim, check to make sure the target is*/
	    /*still visible.*/
	    if (rpgdice.rng.Next(3) == 1  && C.Target == SC.PC.m && !C.Spotted)
        {
		    int O = texmaps.CalcObscurement(C.M,C.Target,SC.gb);
		    if (O == -1 || O > critters.MonMan[C.crit - 1].Sense)
            {
			    C.Target = null;
			    ActPassive(SC);
                return;
		    }
	    }
        else if (rpgdice.rng.Next(10) == 3)
        {
		    if (C.HP >= critters.MonMan[C.crit - 1].MaxHP)
            {
			    int O = texmaps.CalcObscurement(C.M,C.Target,SC.gb);
			    if (O == -1 || O > critters.MonMan[C.crit - 1].Sense)
                {
				    C.Target = null;
				    ActPassive(SC);
                    return;
			    }
		    }
	    }

	    /*Check to see whether or not our critter is gonna try a missile attack.*/
	    if (TacRange(C) > 0 && rpgdice.rng.Next(2) == 1 && texmaps.CalcObscurement(C.M,C.Target,SC.gb) > -1)
        {
		    /* In addition to the above qualifiers, the critter will */
		    /* only use a missile attack if its target is within visual */
		    /* range or if it can be seen by the PC. */
		    if (texmaps.Range(C.M , C.Target) < critters.MonMan[C.crit - 1].Sense || texmaps.TileLOS(SC.gb.POV, C.M.x, C.M.x))
            {
			    CritterAttack(SC,C,C.Target.x, C.Target.y);
                return;
		    }
	    }

        /*We aren't gonna try a missile attack.*/
        /*Move towards the target.*/
        int DX = 0;
        int DY = 0;
        if (C.Target.x < C.M.x)
            DX = -1;
        else if (C.Target.x > C.M.x)
            DX = 1;

        if (C.Target.y < C.M.y)
            DY = -1;
        else if (C.Target.y > C.M.y)
            DY = 1;

        /*Check to see if we're in attack range.*/
        if (C.M.x + DX != C.Target.x || C.M.y + DY != C.Target.y)
        {
            /*Check for obstructions*/
            if (!MoveOK(SC, C.M.x + DX, C.M.y + DY))
            {
                if (MoveOK(SC, C.M.x + DX, C.M.y)) DY = 0;
                else if (MoveOK(SC, C.M.x, C.M.y + DY)) DX = 0;
                else if (rpgdice.rng.Next(2) == 1)
                {
                    ActPassive(SC);
                    return;
                }
            }
        }

	    texmaps.WalkReport WR = WalkCritter(SC,DX,DY);
        if (SC.CAct == null)
            return;

	    if (WR.m != null)
        {
		    /*The critter has walked into a model. Decide*/
		    /*whether or not to attack.*/

		    /*If the model is in the same square as the critter's target,*/
		    /*it will be attacked. Before I just used to compare the*/
		    /*target model with WR.M, but since adding clouds to the game*/
		    /*I have to compare the position of WR.M to the position of*/
		    /*the int}ed target. This is important just in case the*/
		    /*critter is attempting to attack a hallucination-inducing*/
		    /*cloud, and instead blunders into one of its own buddies...*/

		    if (WR.m.x == C.Target.x && WR.m.y == C.Target.y)
            {
			    CritterAttack(SC,C,WR.m.x,WR.m.y);
		    }
	    } 
    }

    static void ActPCHunter(gamebook.Scenario SC, critters.Critter C)
    {
	    /*The critter is gonna attempt to hunt down and destroy*/
	    /*the player character, to the exclusion of all other*/
	    /*targets. If the PC is not in sight, either track him*/
	    /*(if within tracking range) or act passively.*/

	    /*Determine whether or not the monster can get a 'lock'*/
	    /*on the player. To do this, we use the monster's Sense*/
	    /*rating versus the player's Stealth rating.*/
	    if (GetLockOnPC(SC,C))
            C.Target = SC.PC.m;

        if (C.Target != null)
            ActAgressive(SC, C);
        else
            ActPassive(SC);
    }

    static void ActChaos(gamebook.Scenario SC, critters.Critter C)
    {
	    /*The critter is gonna be acting chaotically right now.*/

	    /*Determine a direction to move in. We don't want dir 5*/
	    /*to be a valid choice.*/
	    int D = rpgdice.rng.Next(8) + 1;
	    if (D > 4) D += 1;

	    int t = 1;
	    while (texmaps.TerrPass[SC.gb.map[C.M.x + texmaps.VecDir[D-1,0] - 1, C.M.y + texmaps.VecDir[D-1 ,1] - 1].terr - 1] < 1 && t <= 3)
        {
		    D = rpgdice.rng.Next(8) + 1;
		    if (D > 4) D += 1;
            t += 1;
	    }

	    texmaps.WalkReport WR = WalkCritter(SC,texmaps.VecDir[D-1,0],texmaps.VecDir[D-1,1]);
        if (SC.CAct == null)
            return;

	    if (WR.m != null)
        {
		    /*Chaotic critters usually won't attack others of*/
		    /*their own kind... usually. As a lazy way of*/
		    /*checking this and making alliances, assume that any*/
		    /*two models using the same letter to represent them*/
		    /*are friendly.*/
		    if (WR.m.kind == critters.MKIND_Critter && WR.m.gfx == C.M.gfx && rpgdice.rng.Next(100) != 23)
            {
			    if (texmaps.TileLOS(SC.gb.POV, C.M.x, C.M.y) && texmaps.OnTheScreen(SC.gb,C.M.x,C.M.y))
                {
				    rpgtext.DCGameMessage(critters.MonMan[C.crit - 1].name + " growls.");
			    }
		    }
            else if (WR.m.kind == dcchars.MKIND_Character || WR.m.kind == critters.MKIND_Critter)
            {
			    if (rpgdice.rng.Next(8) != 5)
                    C.Target = WR.m;

			    CritterAttack(SC,C,WR.m.x, WR.m.y);
		    }
	    }
    }

    static void ActGuardian(gamebook.Scenario SC, critters.Critter C)
    {
	    /*This critter is the guardian of a room.*/

	    /*The guardian may try to acquire a target, or may remain in*/
	    /*standby mode.*/
	    if (rpgdice.rng.Next(10) == 1)
        {
		    /*Try to acquire a target.*/
		    for (int X = C.M.x - critters.MonMan[C.crit - 1].Sense; X <= C.M.x + critters.MonMan[C.crit - 1].Sense; ++X)
            {
			    for (int Y = C.M.y - critters.MonMan[C.crit - 1].Sense; Y <= C.M.y + critters.MonMan[C.crit - 1].Sense; ++Y)
                {
				    if (texmodel.ModelPresent(SC.gb.mog, X, Y))
                    {
					    texmodel.Model M = texmodel.FindModelXY(SC.gb.mlist, X, Y);
					    if (M.kind == dcchars.MKIND_Character)
                        {
						    ActPCHunter(SC, C);
					    }
                        else if (M.kind == critters.MKIND_Critter)
                        {
                            if (M.gfx != C.M.gfx && rpgdice.rng.Next(3) == 1 && texmaps.CalcObscurement(C.M, M, SC.gb) > -1)
                            {
                                C.Target = M;
                            }
					    }
				    }
			    }
		    }
	    }

	    if (C.Target != null)
            ActAgressive(SC,C);

        if (rpgdice.rng.Next(5) == 3)
            ActChaos(SC, C);
        else if (rpgdice.rng.Next(3) == 2)
            ActPassive(SC);

	    /*Else, just sit there and do nothing.*/
    }

    /*The slime is gonna do nothing, a la ACS.*/
    static string[] SlimeAct = new string[5]
    {
        "quivers.","twitches.","emits a low groaning sound.","drips acid onto the floor.","suddenly goes very still..."
    };

    static void SlimeDoNothing(gamebook.Scenario SC, critters.Critter C)
    {
        if (rpgdice.rng.Next(64) == 9)
        {
            if (texmaps.TileLOS(SC.gb.POV, C.M.x, C.M.y) && texmaps.OnTheScreen(SC.gb, C.M.x, C.M.y))
            {
                rpgtext.DCGameMessage(critters.MonMan[C.crit - 1].name + " " + SlimeAct[rpgdice.rng.Next(5)]);
            }
        }
    }

	static void SlimeAttack(gamebook.Scenario SC, critters.Critter C)
    {
        /*The Slime wants to attack something. Of course,*/
        /*given the fact that slimes can't move, this might*/
        /*not be possible.*/

        /*Slimes with ranged attacks may attack anyhow.*/
        if (TacRange(C) > -1 &&
            texmaps.Range(C.M, C.Target) <= critters.MonMan[C.crit - 1].Sense &&
            texmaps.CalcObscurement(C.M, C.Target, SC.gb) > -1)
        {
            CritterAttack(SC, C, C.Target.x, C.Target.y);
        }
        else if (Math.Abs(C.M.x - C.Target.x) <= 1 && Math.Abs(C.M.y - C.Target.y) <= 1)
        {
            CritterAttack(SC, C, C.Target.x, C.Target.y);
        }
        else
        {
            SlimeDoNothing(SC, C);
        }
	}

    static void ActSlimy(gamebook.Scenario SC, critters.Critter C)
    {
        /*The big thing about a slime is that it never moves.*/
        /*It just sits there, and attacks whatever is within reach.*/
        /*Slimes give prefrence to attacking the PC. If the PC*/
        /*isn't nearby, it may attack other targets rpgdice.rng.Nextly.*/

        /*I just noticed something. For lower-order organisms,*/
        /*slimes sure is pretty complicated. Their behavior static void*/
        /*is the biggest one so far. Maybe that's just because I*/
        /*like them...*/

        /*If the slime has a target, then attack it.*/
        if (C.Target != null)
            SlimeAttack(SC, C);

	    /*If the slime has no target, try to get a lock on the PC.*/
	    if (GetLockOnPC(SC,C))
        {
		    C.Target = SC.PC.m;
            SlimeAttack(SC, C);
	    }

	    /*The slime hasn't got a target. Just lash out at anything nearby!*/
	    int D = rpgdice.rng.Next(8) + 1;
        if (D > 4) D += 1;
        if (texmodel.ModelPresent(SC.gb.mog, C.M.x + texmaps.VecDir[D - 1, 0], C.M.y + texmaps.VecDir[D - 1, 1]))
        {
            /*Aha! There's a model here! Thwack it! Uhh... unless it's another slime, of course.*/
            texmodel.Model M = texmodel.FindModelXY(SC.gb.mlist, C.M.x + texmaps.VecDir[D - 1, 0], C.M.y + texmaps.VecDir[D - 1, 1]);
            if (M.gfx != C.M.gfx && M.kind == critters.MKIND_Critter)
            {
                CritterAttack(SC, C, C.M.x + texmaps.VecDir[D - 1, 0], C.M.y + texmaps.VecDir[D - 1, 1]);
            }
        }
        else
        {
            SlimeDoNothing(SC, C);
        }
    }

    static void ActHalfHunter(gamebook.Scenario SC, critters.Critter C)
    {
        /*This one is easy. Make a random roll, then branch to*/
        /*a different static void.*/
        if (rpgdice.rng.Next(2) == 1)
            ActPCHunter(SC, C);
        else
            ActChaos(SC, C); ;
    }

    static void TryToBreed(gamebook.Scenario SC, critters.Critter C)
    {
	    /* A breeding monster will reproduce if: */
	    /*  - there are less than the max number of monsters on board */
	    /*  - there is a free spot somewhere close to the breeder */

	    if (critters.NumberOfCritters(SC.CList) < rpgtext.CHART_MaxMonsters)
        {
		    /* Determine a direction in which to generate the new */
		    /* monster. The direction can't be "5". */
		    int D = rpgdice.rng.Next( 8 ) + 1;
            if (D > 4) D += 1;
		    int X = C.M.x + texmaps.VecDir[D - 1, 0];
		    int Y = C.M.y + texmaps.VecDir[D - 1, 1];

		    /* Do the checks to make sure this spot is good for adding */
		    /* a monster. */
		    if (texmaps.OnTheMap(X, Y) && texmaps.TerrPass[SC.gb.map[X-1,Y-1].terr-1] > 0)
            {
			    if (!texmodel.ModelPresent(SC.gb.mog, X, Y))
                {
				    critters.AddCritter(ref SC.CList, SC.gb, C.crit, X, Y);
			    }
		    }
	    }
    }

    static bool CritterActive(critters.Critter C)
    {
	    /*Return TRUE if a given critter is capable of acting*/
	    /*right now, FALSE if it is for any reason incapacitated.*/
        if (plotbase.NAttValue(C.SF, statusfx.NAG_StatusChange, statusfx.SEF_Paralysis) != 0)
            return false;
        else if (plotbase.NAttValue(C.SF, statusfx.NAG_StatusChange, statusfx.SEF_Sleep) != 0)
            return false;

        return true;
    }
}
