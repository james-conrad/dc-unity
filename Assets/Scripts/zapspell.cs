using System;

public class zapspell
{

    //{This unit handles two distinct things: Spells and Item}
    //{effects.}

    public static void ProcessSpell(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{The PC is invoking spell S. This may be through psi}
        //{powers or through the use of an item. Whatever the case,}
        //{determine the results.}
        switch (S.eff)
        {
            case spells.EFF_ShootAttack: ShootAttack(SC, S); break;
            case spells.EFF_CloseAttack: CloseAttack(SC, S); break;
            case spells.EFF_Residual: Residual(SC, S); break;
            case spells.EFF_Healing: Healing(SC, S); break;
            case spells.EFF_MagicMap: MagicMap(SC, S); break;
            case spells.EFF_StatAttack: StatAttack(SC, S); break;
            case spells.EFF_CureStatus: CureStatus(SC, S); break;
            case spells.EFF_Teleport: Teleport(SC, S); break;
            case spells.EFF_SenseAura: SenseAura(SC, S); break;
        }
    }

    public static bool CastSpell(gamebook.Scenario SC, bool UseMenu)
    {
        //{The PC wants to use a psychic ability. Select one of the}
        //{character's powers and then process it.}

        //var
        // S: Integer;	{The spell being cast.}
        // M: Integer;	{Avaliable Mojo}
        // SD: SpellDesc;

        //{Exit immediately if the player has no spells or no mojo.}
        if (SC.PC.spell == null || SC.PC.MP < 1)
            return false;

        rpgtext.DCGameMessage("Invoke");

        int S = -1;

        //{Choose a spell}
        if (UseMenu)
            S = ChooseSpell(SC);
        else
            S = QuickSpell(SC);

        //{If the menu selection wasn't cancelled...}
        if (S != -1)
        {
            //{reduce mojo by appropriate amount.}
            int M = SC.PC.MP;
            SC.PC.MP -= spells.SpellMan[S - 1].cost;
            if (SC.PC.MP < 0)
                SC.PC.MP = 0;

            if (rpgdice.rng.Next(spells.SpellMan[S - 1].cost) < M)
            {
                //{Fill in the SpellDesc record with spell data + PC stats}
                spells.SpellDesc SD = spells.SpellMan[S - 1].Clone();

                rpgtext.DCAppendMessage(SD.name + " - ");

                //{Alter SD for PC's stats depending upon type of spell.}
                switch (spells.SpellMan[S - 1].eff)
                {
                    case spells.EFF_ShootAttack:
                    case spells.EFF_CloseAttack:
                        SD.step = dcchars.PCPsiForce(SC.PC);
                        SD.p1 = dcchars.PCPsiSkill(SC.PC);
                        break;
                    case spells.EFF_Residual:
                        SD.p2 += dcchars.PCPsiForce(SC.PC);
                        break;
                    case spells.EFF_Healing:
                        SD.step += dcchars.PCPsiForce(SC.PC);
                        break;
                    case spells.EFF_MagicMap:
                        SD.step += dcchars.PCPsiForce(SC.PC);
                        break;
                    case spells.EFF_StatAttack:
                        SD.p2 = dcchars.PCPsiSkill(SC.PC);
                        break;
                }

                //{process the spell.}
                ProcessSpell(SC, SD);
            }
            else
            {
                //{Spellcasting failed due to a lack of Mojo.}
                rpgtext.DCAppendMessage("Failed!");
            }

            gamebook.PCStatLine(SC);
        };

        //{Return TRUE if a spell was selected, FALSE if Cancel was selected.}
        return S != -1;
    }

    const Crt.Color BColor = Crt.Color.Blue;
    const Crt.Color IColor = Crt.Color.LightMagenta;
    const Crt.Color SColor = Crt.Color.Magenta;
    const int MX1 = 16;
    const int MY1 = 5;
    const int MX2 = 65;
    const int MY2 = 17;
    const int DY1 = 17;
    const int DY2 = 21;

    static bool ShootAttack(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{This spell shoots something, just like a missile attack.}
        rpgtext.DCAppendMessage("Select Target: ");
        texmaps.Point TP = looker.SelectPoint(SC, true, true, SC.PC.target);

        //{Check to make sure a target was selected, and also}
        //{the the player isn't trying to shoot himself.}
        if (TP.x == -1)
            return false;

        if (TP.x == SC.PC.m.x && TP.y == SC.PC.m.y)
            return false;

        dccombat.AttackRequest AR = new dccombat.AttackRequest();
        AR.HitRoll = S.p1;
        AR.Damage = S.step;
        AR.Range = S.p2;
        AR.Attacker = SC.PC.m;
        AR.TX = TP.x;
        AR.TY = TP.y;
        AR.DF = gamebook.DF_Mystic;
        AR.C = S.c;
        AR.ATT = S.ATT;
        AR.Desc = S.cdesc;

        dccombat.ProcessAttack(SC, AR);

        return true;
    }

    static bool CloseAttack(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{This spell zaps something, just like a melee attack.}
        rpgtext.DCAppendMessage("Direction?");

        //{Select a direction. Make sure an appropriate direction was chosen.}
        int D;
        bool success = int.TryParse(rpgtext.RPGKey().ToString(), out D);

        if (!success || D == 5)
            return false;

        dccombat.AttackRequest AR = new dccombat.AttackRequest();
        AR.HitRoll = S.p1;
        AR.Damage = S.step;
        AR.Range = -1;
        AR.Attacker = SC.PC.m;
        AR.TX = SC.PC.m.x + texmaps.VecDir[D - 1, 0];
        AR.TY = SC.PC.m.y + texmaps.VecDir[D - 1, 1];
        AR.DF = gamebook.DF_Mystic;
        AR.C = S.c;
        AR.ATT = S.ATT;
        AR.Desc = S.cdesc;

        dccombat.ProcessAttack(SC, AR);

        return true;
    }

    static bool Residual(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{Add a residual type to the PC's status list.}
        plotbase.SetNAtt(ref SC.PC.SF, statusfx.NAG_StatusChange, S.step, S.p1 * 10);

        //{Just in case this is a FarSight type spell, do an update.}
        if (S.step == statusfx.SEF_VisionBonus)
        {
            SC.gb.POV.range = dcchars.PCVisionRange(SC.PC);
            texmaps.UpdatePOV(SC.gb.POV, SC.gb);
            texmaps.ApplyPOV(SC.gb.POV, SC.gb);
        };

        //{Display message.}
        if (S.step >= 0)
            rpgtext.DCAppendMessage("Done.");
        else
            rpgtext.DCAppendMessage("You are " + statusfx.NegSFName[Math.Abs(S.step) + 1].ToLower() + "!");

        return true;
    }

    static bool Healing(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{Restore HP to the PC.}
        int dhp = SC.PC.HP;

        //{Restore HP; make sure it doesn't go over HPMax.}
        SC.PC.HP += rpgdice.RollStep(S.step);
        if (SC.PC.HP > SC.PC.HPMax)
            SC.PC.HP = SC.PC.HPMax;
        gamebook.PCStatLine(SC);

        dhp = SC.PC.HP - dhp;
        rpgtext.DCAppendMessage(dhp.ToString() + " hit points restored.");

        return true;
    }

    static bool MagicMap(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{Go through every map tile within range. If the tile is}
        //{a floor, and the % chance is rolled, reveal that tile.}
        for (int X = SC.PC.m.x - S.p1; X <= SC.PC.m.x + S.p1; ++X)
        {
            for (int Y = SC.PC.m.y - S.p1; Y <= SC.PC.m.y + S.p1; ++Y)
            {
                if (texmaps.OnTheMap(X, Y))
                {
                    //{Only terrain which is within the cutoff range may be sensed.}
                    if (texmaps.TerrPass[texmaps.GetTerr(SC.gb, X, Y) - 1] >= S.p2 && rpgdice.rng.Next(100) < S.step)
                    {
                        SC.gb.map[X, Y].visible = true;
                    }
                }
            }
        }

        rpgtext.DCAppendMessage("You sense distant locations.");
        texmaps.DisplayMap(SC.gb);
        return true;
    }

    static bool StatAttack(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{Affect every enemy model within range with a given}
        //{status change condition.}

        bool itworked = false;

        //{Determine the accuracy of the attack, also its Value and}
        //{Duration. Use defaults if these values can't be found.}
        int V = rpgdice.RollStep(spells.AAVal(S.ATT, spells.AA_Value));
        if (V < 5)
            V = 5;

        for (int X = SC.PC.m.x - S.p1; X <= SC.PC.m.x + S.p1; ++X)
        {
            for (int Y = SC.PC.m.y - S.p1; Y <= SC.PC.m.y + S.p1; ++Y)
            {
                if (texmodel.ModelPresent(SC.gb.mog, X, Y))
                {
                    critters.Critter C = critters.LocateCritter(texmodel.FindModelXY(SC.gb.mlist, X, Y), SC.CList);
                    if (C != null)
                    {
                        if (rpgdice.RollStep(S.p2) > rpgdice.RollStep(critters.MonMan[C.crit - 1].Mystic))
                        {
                            if (critters.SetCritterStatus(C, S.step, V))
                            {
                                texmaps.MapSplat(SC.gb, '*', S.c, X, Y, false);
                                itworked = true;
                            }
                        }
                    }
                }
            }
        }

        if (itworked)
        {
            rpgtext.DCAppendMessage("Done.");
            texfx.Delay();
            texmaps.DisplayMap(SC.gb);

            //{Give the PC a few points for successfully using}
            //{this spell.}
            gamebook.DoleExperience(SC, rpgdice.rng.Next(3));
        }
        else
        {
            rpgtext.DCAppendMessage("Failed!");
        }

        return true;
    }

    static bool CureStatus(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{Cure the PC of status S.}

        if (plotbase.NAttValue(SC.PC.SF, statusfx.NAG_StatusChange, S.step) == 0)
        {
            rpgtext.DCAppendMessage("No effect!");
            return false;
        }

        plotbase.SetNAtt(ref SC.PC.SF, statusfx.NAG_StatusChange, S.step, 0);
        rpgtext.DCAppendMessage("Cured!");

        gamebook.PCStatLine(SC);

        return true;
    }


    static bool GoodSpot(gamebook.Scenario SC, int X, int Y)
    {
        //{Check spot X,Y and see if this is a good place to}
        //{teleport to.}

        if (texmaps.TerrPass[SC.gb.map[X - 1, Y - 1].terr - 1] < 1)
            return false;

        if (texmodel.ModelPresent(SC.gb.mog, X, Y))
            return false;

        return true;
    }

    static bool Teleport(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{Allow the player to teleport, either randomly or}
        //{controlledly.}

        //{Select destination point.}
        int Tries = 0;
        int X, Y;
        do
        {
            int D = rpgdice.rng.Next(8) + 1;
            if (D > 4)
                D += 1;

            X = SC.PC.m.x + texmaps.VecDir[D - 1, 0] * S.step + rpgdice.rng.Next(5) - rpgdice.rng.Next(5);
            Y = SC.PC.m.y + texmaps.VecDir[D - 1, 1] * S.step + rpgdice.rng.Next(5) - rpgdice.rng.Next(5);
            Tries += 1;
        }
        while (!texmaps.OnTheMap(X, Y) && Tries < 5);

        if (texmaps.OnTheMap(X, Y))
        {
            while (Tries < 1000 && !GoodSpot(SC, X, Y))
            {
                Tries += 1;
                X += rpgdice.rng.Next(10) - rpgdice.rng.Next(10);
                if (X > texmodel.XMax)
                    X = texmodel.XMax;
                else if (X < 1)
                    X = 1;
                Y += rpgdice.rng.Next(10) - rpgdice.rng.Next(10);
                if (Y > texmodel.YMax)
                    Y = texmodel.YMax;
                else if (Y < 1)
                    Y = 1;
            }

            if (GoodSpot(SC, X, Y))
            {
                texmaps.MoveModel(SC.PC.m, SC.gb, X, Y);
                rpgtext.DCAppendMessage("Done.");
            }
            else
            {
                rpgtext.DCAppendMessage("Failed.");
            }
        }
        else
        {
            rpgtext.DCAppendMessage("Failed!");
        };

        return true;
    }

    static bool SenseAura(gamebook.Scenario SC, spells.SpellDesc S)
    {
        //{The PC gets to see every monster currently on screen.}

        //   var
        //M: ModelPtr;
        //success: bool;

        texmaps.ClearMapArea();
        bool success = false;

        //{Scan through every model in the list, looking for models}
        //{to display.}
        texmodel.Model M = SC.gb.mlist;

        while (M != null)
        {
            if (texmaps.OnTheScreen(SC.gb, M.x, M.y) && M.kind == S.step)
            {
                texmaps.MapSplat(SC.gb, M.gfx, M.color, M.x, M.y, true);
                success = true;
            }
            M = M.next;
        }

        if (success)
            rpgtext.DCAppendMessage("Done.");
        else
            rpgtext.DCAppendMessage("Failed.");

        rpgtext.GamePause();

        //{Restore the map display.}
        texmaps.DisplayMap(SC.gb);
        return true;
    }

    static void SetQuickLink(gamebook.Scenario SC, int SCode)
    {
	    //{Assign a letter to one of the PC's spells, for quick}
	    //{casting later.}

	    string instr = "Select a letter to represent this spell.";
	    string qmerr ="Invalid character.";

    //var
	   // S: SpellMemPtr;
	   // C: Char;

	    //{Display instructions}
	    rpgtext.GameMessage(instr, MX1, MY2, MX2, MY2+2, IColor, BColor);

	    char C = rpgtext.RPGKey();
        char uC = char.ToUpper(C);

	    if (uC >= 'A' && uC <= 'Z')
        {
		    //{Make sure no other spellmem has this key linked.}
		    spells.SpellMem S = SC.PC.spell;
		    while (S != null)
            {
			    if (S.mnem == C)
                    S.mnem = ' ';

			    S = S.next;
		    }

		    S = spells.LocateSpellMem(SC.PC.spell, SCode);
		    if (S != null)
            {
			    S.mnem = C;
		    }
	    }
        else
        {
		    rpgtext.GameMessage(qmerr,MX1,MY2,MX2,MY2+2,IColor,BColor);
            rpgtext.RPGKey();
	    }
    }

    static int ChooseSpell(gamebook.Scenario SC)
    {
        //{Create a menu from the PC's spell list. Query for a spell.}
        //{Return whatever spell was chosen, or -1 for Cancel.}

        string instr = "[SPACE] to cast, [/] to quickmark";
        int QMval = -10;

        rpgtext.DCPointMessage(" which spell?");

        int it = QMval;
        do
        {
            //{Display instructions}
            rpgtext.GameMessage(instr, MX1, DY2, MX2, DY2 + 2, IColor, BColor);

            //{Create the menu.}
            rpgmenus.RPGMenu RPM = rpgmenus.CreateRPGMenu(BColor, SColor, IColor, MX1, MY1, MX2, MY2);
            RPM.dx1 = MX1;
            RPM.dy1 = DY1;
            RPM.dx2 = MX2;
            RPM.dy2 = DY2;

            rpgmenus.AddRPGMenuKey(RPM, '/', QMval);
            spells.SpellMem S = SC.PC.spell;
            while (S != null)
            {
                if (S.mnem == ' ')
                {
                    rpgmenus.AddRPGMenuItem(RPM, spells.SpellMan[S.code - 1].name, S.code, spells.SpellMan[S.code - 1].Desc);
                }
                else
                {
                    rpgmenus.AddRPGMenuItem(RPM, spells.SpellMan[S.code - 1].name + " [" + S.mnem + "]", S.code, spells.SpellMan[S.code - 1].Desc);
                    rpgmenus.AddRPGMenuKey(RPM, S.mnem, S.code);
                }

                S = S.next;
            }

            rpgmenus.RPMSortAlpha(RPM);

            //{Make a menu selection.}
            it = rpgmenus.SelectMenu(RPM, rpgmenus.RPMNoCleanup);

            //{Check to see if the PC wants to QuickMark a spell.}
            if (it == QMval)
            {
                SetQuickLink(SC, rpgmenus.RPMLocateByPosition(RPM, RPM.selectItem).value);
            }

        }
        while (it == QMval);

        //{Redisplay the map.}
        texmaps.DisplayMap(SC.gb);
        rpgtext.DCPointMessage(" ");

        return it;
    }

    static int QuickSpell(gamebook.Scenario SC)
    {
        //{Locate a spell based on its quicklink char.}
	    rpgtext.DCPointMessage(" which spell? [a-z/A-Z] code, or [*] for menu");
	    char A = rpgtext.RPGKey();
	    rpgtext.DCPointMessage(" ");

        int it = -1;
        spells.SpellMem S;

        if (A == '*')
        {
            it = ChooseSpell(SC);
        }
        else if (char.ToUpper(A) >= 'A' && char.ToUpper(A) <= 'Z')
        {
		    it = -1;
		    S = SC.PC.spell;
		    while (S != null)
            {
			    if (S.mnem == A)
                    it = S.code;

			    S = S.next;
		    }
	    }

        return it;
    }
}