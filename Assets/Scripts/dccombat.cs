using System;

class dccombat
{
    //{This unit handles the main combat stuff for DeadCold.}
    //{This includes attacking, damaging, etc, for both critters}
    //{and the PC.}

    //{Critters and PCs have hit points. Terrain features and}
    //{inanimate models don't have hit points, but they still may}
    //{be damaged by attacks.}

    //{This unit also holds the definitions and procedures needed}
    //{for traps in the game. Combat, traps, same thing, right?}

    public class AttackRequest
    {
        public int HitRoll; //{HitRoll step number}
        public int Damage;  //{Damage Roll step number}
        public int Range;   //{Range of the attack.}
                            //{Used to calculate hit modifiers, and}
                            //{scatter in case of a miss.}
        public texmodel.Model Attacker; //{The Attacker.}
        public int TX;
        public int TY;      //{Target square.}
                            //{If there's a model in the target square,}
                            //{this procedure assumes that said model is the}
                            //{intended victim of the attack.}
        public int DF; //{What defense does this attack target?}
        public Crt.Color C; //{Color of shot}
        public string ATT; //{Attack Attributes}
        public string Desc; //{An optional description.}
    }

    public class AttackReport
    {
        //{This record tells the results of the attack.}
        public bool ItHit; //{Did the attack hit?}
        public bool Fatal; //{Is the target dead?}
        public int Damage; //{The amount of damage done.}
        public int XPV;    //{Amount of XP earned.}
    }

    public class TrapDesc
    {
        public TrapDesc(string Name, string Desc, int DMG, int Disarm)
        {
            this.Name = Name;
            this.Desc = Desc;
            this.DMG = DMG;
            this.Disarm = Disarm;
        }

        //{This record describes a trap. Duh.}
        public string Name; //{Used when looking at ID'd trap.}
        public string Desc; //{Used when trap is activated.}
        public int DMG;     //{Damage done by trap.}
        public int Disarm;  //{Target number to disarm.}
    }

    public const int CDropEqp = 50; //{% chance that killed creature will drop Eqp item.}
    public const int BlastBaseTarget = 5; //{Base target number for blast attacks.}
    public const int TrapNum = 5;
    public static TrapDesc[] TrapMan = new TrapDesc[TrapNum]
    {
        new TrapDesc( "Electrical Discharger",
            Desc: "zapped with 10,000 volts of electricity",
            DMG: 2, Disarm: 10),
        new TrapDesc( "Automated Sentry Turret",
            Desc: "shot by a sentry gun",
            DMG: 8, Disarm: 5),
        new TrapDesc( "Laser Guardwire",
            Desc: "sliced by a beam of laser light",
            DMG: 16, Disarm: 10),
        new TrapDesc("Alarm",
            Desc: "momentarily flashed by a spot light",
            DMG: 0, Disarm: 20),
        new TrapDesc( "Plasma Barrier",
            Desc: "immolated by a plasma field",
            DMG: 42, Disarm: 15),
    };

    public static bool DamagePC(gamebook.Scenario SC, int MOS, string ATT, int DMG)
    {
        //{Damage is about to be done to the PC. Woohoo! The monsters}
        //{finally accomplished something!}
        //{Return TRUE if the PC has been killed; FALSE if he's still}
        //{standing.}

        //{ If the character is asleep, he/she will take double damage}
        //{from a successful hit and be immediately woken up.}
        if (plotbase.NAttValue(SC.PC.SF, statusfx.NAG_StatusChange, statusfx.SEF_Sleep) != 0)
        {
            DMG *= 2;
            plotbase.SetNAtt(ref SC.PC.SF, statusfx.NAG_StatusChange, statusfx.SEF_Sleep, 0);
        }

        //{If the PC is performing a continual action, that is cancelled.}
        SC.PC.repCount = 0;

        //{Calculate Armor rating.}
        int Armor = dcchars.PCArmorPV(SC.PC);

        //{Reduce for MOS}
        if (MOS > 0)
        {
            if (MOS > 4)
                MOS = 4;

            Armor = (Armor * (4 - MOS)) / 4;
        }

        //{Reduce for Armor-Piercing attacks. Increase for }
        if (ATT.Contains(spells.AA_ArmorDoubling))
        {
            Armor = Armor * 2;
        }
        else if (ATT.Contains(spells.AA_ArmorPiercing))
        {
            Armor = (Armor + 1) / 2;
        }

        //{Reduce damage for armor.}
        DMG = DMG - Armor;
        if (DMG < 1)
            DMG = 1;

        SC.PC.HP = SC.PC.HP - DMG;
        gamebook.PCStatLine(SC);

        bool gameover = true;

        if (SC.PC.HP > 0)
        {
            gameover = false;

            //{There may be status change effects here.}
            int E = spells.AAVal(ATT, spells.AA_StatusChange);
            if (E != 0)
            {
                int FxRoll = spells.AAVal(ATT, spells.AA_HitRoll);
                if (FxRoll == 0 || rpgdice.RollStep(FxRoll) > rpgdice.RollStep(dcchars.PCLuckSave(SC.PC)))
                {
                    int v = rpgdice.RollStep(spells.AAVal(ATT, spells.AA_Value));
                    if (v < 5)
                        v = 5;

                    plotbase.AddNAtt(ref SC.PC.SF, statusfx.NAG_StatusChange, -E, v);
                    rpgtext.DCAppendMessage("You are " + statusfx.NegSFName[E - 1].ToLower() + '!');
                }
            }
        }
        else
        {
            gameover = true;
            gamebook.Excommunicate(SC, SC.PC.m);
        }

        return gameover;
    }

    public static void CritterDeath(gamebook.Scenario SC, critters.Critter C, bool KilledByPC)
    {
        //{A critter has died. Deal with it.}
        if (C == null)
        {
            rpgtext.DCGameMessage("SHAZBOT - The attemptes killing of a nonexistant critters.");
            return;
        }

        //{Critters will only drop random treasure if they are killed}
        //{by the PC.}
        if (KilledByPC)
        {
            if (critters.MonMan[C.crit - 1].TType > 0)
            {
                int N = 0;
                while (N < critters.MonMan[C.crit - 1].TNum && rpgdice.Random(100) < critters.MonMan[C.crit - 1].TDrop)
                {
                    N += 1;
                    dcitems.DCItem I = charts.GenerateItem(SC, critters.MonMan[C.crit - 1].TType);
                    dcitems.PlaceDCItem(SC.gb, SC.ig, I, C.M.x, C.M.y);
                }
            }
        }

        if (C.Eqp != null && rpgdice.Random(100) < CDropEqp)
        {
            //{The critter dropped whatever it was carrying.}
            dcitems.PlaceDCItem(SC.gb, SC.ig, C.Eqp, C.M.x, C.M.y);

            //{Set the Eqp field to Nil, or else the RemoveCritter procedure}
            //{will delete the item. And mess up our map.}
            C.Eqp = null;
        }

        gamebook.Excommunicate(SC, C.M);
        critters.RemoveCritter(C, ref SC.CList, SC.gb);
    }

    public static AttackReport ProcessAttack(gamebook.Scenario SC, AttackRequest AR)
    {
        //{We have a filled-out AttackRequest structure. Process it.}
        AttackReport Rep;
        if (AR.ATT.Contains(spells.AA_LineAttack))
            Rep = LineAttack(SC, AR);
        else if (AR.ATT.Contains(spells.AA_BlastAttack))
            Rep = BlastAttack(SC, AR);
        else if (AR.ATT.Contains(spells.AA_SmokeAttack))
            Rep = SmokeAttack(SC, AR);
        else
            Rep = DirectFire(SC, AR);

        if (AR.Attacker.kind == dcchars.MKIND_Character && Rep.XPV > 0)
            gamebook.DoleExperience(SC, Rep.XPV);

        return Rep;
    }

    public static void RevealTrap(gamebook.Scenario SC, int TX, int TY)
    {
        //{Reveal the trap at location X,Y so that the player will}
        //{be able to see it.}
        SC.gb.map[TX - 1, TY - 1].trap = Math.Abs(SC.gb.map[TX - 1, TY - 1].trap);
        texmaps.DisplayTile(SC.gb, TX, TY);
    }

    public static void SpringTrap(gamebook.Scenario SC, int TX, int TY)
    {
        //{Spring the trap at location TX,TY, damaging creatures if}
        //{there are any present.}

	    //{Error check- make sure we have a trap to spring!}
	    int T = Math.Abs(SC.gb.map[TX - 1,TY - 1].trap);
        if (T == 0)
            return;

	    //{Find out who sprung it.}
	    texmodel.Model M = texmodel.FindModelXY(SC.gb.mlist,TX,TY);
        string TName = "";
	    if (M != null)
        {
		    TName = gamebook.ModelName(SC,M);
	    }

	    //{What happens next depends upon whether or not the PC}
	    //{is watching.}
	    if (texmaps.TileLOS(SC.gb.POV,TX,TY))
        {
		    //{The PC can see this. Better tell her what's going on.}
		    rpgtext.DCGameMessage("Trap!");

		    if (M != null)
            {
                //{Explain exactly what the trap is doing.}
                if (M.kind == critters.MKIND_Critter)
                {
                    rpgtext.DCAppendMessage(TName + " is " + TrapMan[T - 1].Desc + '!');
                }
                else if (M.kind == dcchars.MKIND_Character)
                {
                    rpgtext.DCAppendMessage("You are " + TrapMan[T - 1].Desc + "!");
                }

			    //{Do damage to target, and report on it.}
			    TheTrapStuffIsHere(SC,TX,TY);
		    }

            //{If it's the character in the trap, pause after the message.}
            if (M != null && M.kind == dcchars.MKIND_Character && SC.PC.HP > 0)
            {
                rpgtext.GamePause();
            }

		    //{Since this trap is within LOS, it's now revealed.}
		    RevealTrap(SC,TX,TY);
	    }
        else if (M != null)
        {
		    //{A trap has been sprung, but the PC can't see it.}
		    //{Just damage the creature involved.}
		    TheTrapStuffIsHere(SC,TX,TY);
	    }
    }

    static bool DamageCritter(gamebook.Scenario SC, critters.Critter C, int MOS, AttackRequest AR, int DMG, ref AttackReport Rep)
    {
        //{Damages a critters. Returns TRUE if the critter has been}
        //{destroyed; returns FALSE if it is still functional.}

        //var
        // OriginalDamage,E,N,V,Armor: int;
        // snuffedit: bool;

        //{Save the original damage}
        int OriginalDamage = DMG;

        //{Scale damage for elemental types.}
        //{First, determine what element is being invoked.}
        int E = spells.AAVal(AR.ATT, spells.AA_Element);
        DMG = critters.ScaleCritterDamage(C, DMG, E);

        //{Scale damage for critter slaying attribute.}
        E = spells.AAVal(AR.ATT, spells.AA_Slaying);

        if (E > 0 && critters.MonMan[C.crit - 1].CT.Contains(critters.CTMan[E - 1]))
        {
            if (DMG < OriginalDamage)
                DMG = OriginalDamage;
            DMG *= 2 + rpgdice.Random(3);
        }

        //{Reduce damage for armor, and increase it for condition.}
        if (DMG > 0)
        {
            //{If a critter is asleep, then it will take double damage}
            //{from a successful hit and be woken up if it survives.}
            if (plotbase.NAttValue(C.SF, statusfx.NAG_StatusChange, statusfx.SEF_Sleep) != 0)
            {
                DMG *= 2;
                plotbase.SetNAtt(ref C.SF, statusfx.NAG_StatusChange, statusfx.SEF_Sleep, 0);
            }

            //{Calculate Armor rating.}
            int Armor = critters.MonMan[C.crit - 1].Armor;

            //{Reduce for MOS}
            if (MOS > 0)
            {
                if (MOS > 4)
                    MOS = 4;
                Armor = (Armor * (4 - MOS)) / 4;
                if (texmaps.TileLOS(SC.gb.POV, C.M.x, C.M.y))
                {
                    rpgtext.DCAppendMessage("Critical Hit!");
                }
            }

            //{Reduce for Armor-Piercing}
            if (AR.ATT.Contains(spells.AA_ArmorDoubling))
            {
                Armor *= 2;
            }
            else if (AR.ATT.Contains(spells.AA_ArmorPiercing))
            {
                Armor = (Armor + 1) / 2;
            }

            DMG -= Armor;
            if (DMG < 1)
                DMG = 1;
        }

        C.HP -= DMG;
        bool snuffedit = true;
        if (C.HP > 0)
        {
            //{ The target is still alive. See what else needs to be done.}
            snuffedit = false;

            //{ There may be status change effects here.}
            E = spells.AAVal(AR.ATT, spells.AA_StatusChange);
            if (E != 0)
            {
                int N = spells.AAVal(AR.ATT, spells.AA_HitRoll);
                if (N == 0 || rpgdice.RollStep(N) > rpgdice.RollStep(critters.MonMan[C.crit - 1].Mystic))
                {
                    int v = rpgdice.RollStep(spells.AAVal(AR.ATT, spells.AA_Value));
                    if (v < 5)
                        v = 5;
                    if (critters.SetCritterStatus(C, -E, v) && texmaps.TileLOS(SC.gb.POV, C.M.x, C.M.y))
                        rpgtext.DCAppendMessage(statusfx.NegSFName[E - 1] + "!");
                }
            }
        }
        else
        {
            snuffedit = true;

            //{ Add the XPV of the critter to the Attack Report's XPV field.}
            Rep.XPV = Rep.XPV + critters.MonMan[C.crit - 1].XPV;
            CritterDeath(SC, C, AR.Attacker == SC.PC.m);
        }

        return snuffedit;
    }

    static int RollDamage(int DC)
    {
        //{ Normally, this function just calls rpgdice.RollStep to do the dice }
        //{ rolling. However, if the DAMAGECAP option is set, it also }
        //{ makes sure that the damage rolled doesn't exceed a certain }
        //{ amount. }
        int DMG = rpgdice.RollStep(DC);

        //{ If DAMAGECAP is on, check to make sure the amount of damage }
        //{ rolled doesn't exceed the maximum. }
        if (rpgtext.COMBAT_DamageCap)
        {
            //{ Store the Damage Cap value in DC }
            DC = DC * 2 + 3;
            if (DMG > DC)
                DMG = DC;
        }

        return DMG;
    }

    static bool DamageTarget(gamebook.Scenario SC, int TX, int TY, int MOS, AttackRequest AR, int DMG, ref AttackReport Rep)
    {
        //{Do DMG damage to whatever happens to be sitting at map}
        //{location TX,TY.}
        //{MOS is the Margin Of Success}

        //M: texmodel.Model;

        bool exparrot = false;
        if (SC.gb.mog.IsSet(TX, TY))
        {
            //{It's a model. Do something appropriate to it.}
            texmodel.Model M = texmodel.FindModelXY(SC.gb.mlist, TX, TY);
            switch (M.kind)
            {
                case critters.MKIND_Critter:
                    exparrot = DamageCritter(SC, critters.LocateCritter(M, SC.CList), MOS, AR, DMG, ref Rep);
                    break;

                case dcchars.MKIND_Character:
                    exparrot = DamagePC(SC, MOS, AR.ATT, DMG);
                    break;
            }
        }

        return exparrot;
    }

    static void AlertOthers(gamebook.Scenario SC, critters.Critter C, int DMG)
    {
        //{The PC has just attacked critter C. All other critters}
        //{of this type now have a chance to target the PC for}
        //{retribution.}

        critters.Critter CTemp = SC.CList;

        while (CTemp != null)
        {
            if (CTemp.M.gfx == C.M.gfx && CTemp.Target == null && texmaps.Range(CTemp.M, C.M) < 25)
            {
                //{This critter is a contemporary of the one}
                //{which was attacked.}
                //{Check to see whether the shot was noticed.}
                if (CTemp.AIType != critters.AIT_Passive && rpgdice.RollStep(critters.MonMan[CTemp.crit - 1].Sense) > rpgdice.RollStep(dcchars.PCStealth(SC.PC) - DMG))
                {
                    CTemp.Target = SC.PC.m;
                }
            }

            CTemp = CTemp.next;
        }
    }

    static int RollDefenses(gamebook.Scenario SC, AttackRequest AR, int TX, int TY)
    {
        //{Roll the defenses for whatever is in location TX,TY}
        int DefRoll = 0;

        if (SC.gb.mog.IsSet(TX, TY))
        {
            //{the target is a model. Yay! Determine if it's a}
            //{critter, the PC, or something else, then look up}
            //{its defense value.}
            int DfSt = gamebook.ModelDefenseStep(SC, texmodel.FindModelXY(SC.gb.mlist, TX, TY), AR.DF);

            //{Do the defense roll. Note that unlike most rolls,}
            //{defense rolls have a minimum value.}
            DefRoll = rpgdice.RollStep(DfSt);
            if (DefRoll < DfSt)
                DefRoll = DfSt;

            //{While we're here, might as well do something else}
            //{altogether. If a critter is attacked, it switches its}
            //{TARGET to whatever model attacked it.}
            if (texmodel.FindModelXY(SC.gb.mlist, TX, TY).kind == critters.MKIND_Critter)
            {
                critters.Critter C = critters.LocateCritter(texmodel.FindModelXY(SC.gb.mlist, TX, TY), SC.CList);
                if (C.M != AR.Attacker)
                {
                    C.Target = AR.Attacker;
                    if (AR.Attacker.kind == dcchars.MKIND_Character)
                    {
                        SC.PC.target = texmodel.FindModelXY(SC.gb.mlist, TX, TY);
                        AlertOthers(SC, C, AR.Damage);
                    }
                }
            }
        }
        else
        {
            //{ There's no model present, meaning that we're}
            //{ firing at a map tile.It's not likely to dodge.}
            DefRoll = 0;
        }

        return DefRoll;
    }

    static AttackReport DirectFire(gamebook.Scenario SC, AttackRequest AR)
    {
        //{The attack being invoked is a regular, old fashioned,}
        //{direct fire attack.}

        int O = 0;
        //{Do some checking for missile attacks.}
        if (AR.Range > -1)
        {
            //{Determine obscurement. If the target can't be seen,}
            //{switch TX,TY to whatever obstacle is in the way.}
            O = texmaps.CalcObscurement(AR.Attacker, AR.TX, AR.TY, SC.gb);
            if (O == -1)
            {
                texmaps.Point P = texmaps.LocateBlock(SC.gb, AR.Attacker.x, AR.Attacker.y, AR.TX, AR.TY);
                AR.TX = P.x;
                AR.TY = P.y;
                O = texmaps.CalcObscurement(AR.Attacker, AR.TX, AR.TY, SC.gb);
            }

            //{Calculate range modifier.}
            if (AR.Range > 0)
                O += texmaps.Range(AR.Attacker, AR.TX, AR.TY) / AR.Range;
            else
                O += texmaps.Range(AR.Attacker, AR.TX, AR.TY);
        }
        else
        {
            //{It's a melee attack. No obscurement.}
            O = 0;
        }

        //{Determine the visibility status of the attacker and target.}
        bool AVis = texmaps.TileLOS(SC.gb.POV, AR.Attacker.x, AR.Attacker.y);
        bool TVis = texmaps.TileLOS(SC.gb.POV, AR.TX, AR.TY);

        //{Initialize Attack Report}
        AttackReport Rep = new AttackReport();
        Rep.Fatal = false;
        Rep.XPV = 0;
        string msg = "";
        string tname = "";

        if (SC.gb.mog.IsSet(AR.TX, AR.TY))
            tname = gamebook.ModelName(SC, texmodel.FindModelXY(SC.gb.mlist, AR.TX, AR.TY));
        else
            tname = texmaps.TerrName[SC.gb.map[AR.TX - 1, AR.TY - 1].terr - 1];

        //{Announce the attack.}
        if (AVis || TVis)
        {
            if (AVis)
                msg = gamebook.ModelName(SC, AR.Attacker);
            else
                msg = "Something";

            if (AR.Desc != "" && rpgdice.Random(3) == 1)
            {
                if (AR.Attacker.kind == dcchars.MKIND_Character)
                    msg = "You " + AR.Desc + " ";
                else
                    msg += " " + AR.Desc + " ";
            }
            else
            {
                if (AR.Attacker.kind == dcchars.MKIND_Character)
                    msg = "You attack ";
                else
                    msg += " attacks ";
            }

            if (TVis)
                msg += tname;
            else
                msg += "something";
        }

        //{Determine the defense roll of the target.}
        int DefRoll = RollDefenses(SC, AR, AR.TX, AR.TY);

        //{Determine the attack roll of the attacker.}
        int ARoll = rpgdice.RollStep(AR.HitRoll) - O;

        //{Add punctuation to our message string, then print.}
        if (msg.Length > 0)
        {
            if (rpgdice.Random(8) == 5)
                msg += "... ";
            else if (ARoll > 50)
                msg += "!!! ";
            else if (ARoll > 25)
                msg += "! ";
            else if (ARoll > 5)
                msg += ". ";
            else
            {
                if (rpgdice.Random(5) == 1)
                    msg += "? ";
                else
                    msg += ", sort of... ";
            }
        }

        int MOS = 0;

        if (ARoll > DefRoll)
        {
            //{The attack hit! Do whatever needs to be done...}
            //{Determine Margin of Success}
            if (DefRoll > 0)
                MOS = ARoll / DefRoll - 1;
            else
                MOS = ARoll / 10;

            //{There's a maximum value for MOS, based on magnitude of the roll.}
            if (ARoll < 15)
                MOS = 0;
            else if (MOS > (ARoll - 10) / 5)
                MOS = (ARoll - 10) / 5;

            //{Determine Damage Bonus}
            int DBonus = rpgdice.Random(MOS + 1);
            MOS -= DBonus;
            if (MOS > 4)
            {
                DBonus += MOS - 4;
                MOS = 4;
            }

            Rep.ItHit = true;
            Rep.Damage = RollDamage(AR.Damage + (DBonus * 3));
            if (msg.Length > 0)
                msg += "The attack hit!";
        }
        else
        {
            //{ The attack missed!Again, do whatever needs to be done...}
            Rep.ItHit = false;
            if (rpgdice.Random(3) == 2 && TVis)
                msg += tname + " dodged the attack.";
            else if (msg.Length > 0)
                msg += "The attack missed.";

        }

        if (msg.Length > 0)
        {
            rpgtext.DCGameMessage(msg);
            msg = "";
        }

        if (TVis || AVis)
        {
            texfx.DisplayShot(SC.gb, AR.Attacker.x, AR.Attacker.y, AR.TX, AR.TY, AR.C, Rep.ItHit);
        }

        //{Damage the target now, after the shot has been displayed.}
        if (Rep.ItHit)
        {
            Rep.Fatal = DamageTarget(SC, AR.TX, AR.TY, MOS, AR, Rep.Damage, ref Rep);

            if (TVis && Rep.Damage > 0)
            {
                msg += Rep.Damage.ToString() + " damage!";
            }
            else if (TVis && Rep.Damage < 0)
            {
                msg += Math.Abs(Rep.Damage).ToString() + " HP restored!";
            }
            else if (TVis && Rep.Damage == 0)
            {
                msg += " No damage!";
            }
        }

        if (Rep.Fatal && TVis)
        {
            msg += " " + tname + " died!";
        }

        if (msg.Length > 0)
            rpgtext.DCAppendMessage(msg);

        return Rep;
    }

    static AttackReport LineAttack(gamebook.Scenario SC, AttackRequest AR)
    {
        //{The attack being processed is a line attack. It keeps going,}
        //{affecting every model it touches, until it runs out of range}
        //{or until it hits a wall.}

        //{Begin by making sure the Range is an appropriate value.}
        if (AR.Range < 2)
            AR.Range = 2;

        //{Initialize values.}
        AttackReport Rep = new AttackReport();
        Rep.ItHit = false;
        Rep.Fatal = false;
        Rep.Damage = 0;
        Rep.XPV = 0;

        //{Do the initial message here, if appropriate}
        if (texmaps.TileLOS(SC.gb.POV, AR.Attacker.x, AR.Attacker.y))
        {
            //{Print message saying what's going on.}
            if (AR.Attacker.kind == dcchars.MKIND_Character)
                rpgtext.DCGameMessage("You " + AR.Desc + ".");
            else
                rpgtext.DCGameMessage(gamebook.ModelName(SC, AR.Attacker) + " " + AR.Desc + ".");
        }

        for (int t = 1; t <= AR.Range; ++t)
        {
            //{Calculate the current target square.}
            texmaps.Point P = texmaps.SolveLine(AR.Attacker.x, AR.Attacker.y, AR.TX, AR.TY, t);

            if (texmodel.ModelPresent(SC.gb.mog, P.x, P.y))
            {
                //{Determine the defense roll of the target.}
                int DRoll = RollDefenses(SC, AR, P.x, P.y);

                //{Determine the attack roll of the attacker.}
                int ARoll = rpgdice.RollStep(AR.HitRoll);

                //{Determine the target's name. Modify for PC.}
                string tname = gamebook.ModelName(SC, texmodel.FindModelXY(SC.gb.mlist, P.x, P.y));
                if (tname == "you")
                    tname = "You";

                int Dmg = 0;
                if (ARoll > DRoll)
                {
                    //{Hit!}
                    Rep.ItHit = true;
                    Dmg = RollDamage(AR.Damage);

                    texmaps.MapSplat(SC.gb, '*', AR.C, P.x, P.y, false);
                }
                else if (ARoll > DRoll / 2)
                {
                    //{Partial hit! Half damage!}
                    Rep.ItHit = true;
                    Dmg = RollDamage((AR.Damage + 1) / 2);

                    texmaps.MapSplat(SC.gb, '+', AR.C, P.x, P.y, false);
                }
                else
                {
                    //{Complete miss!}
                    Dmg = 0;

                    texmaps.MapSplat(SC.gb, '-', AR.C, P.x, P.y, false);
                }

                if (Dmg > 0)
                {
                    //{Line Attacks never score critical hits.}
                    bool f = DamageTarget(SC, P.x, P.y, 0, AR, Dmg, ref Rep);
                    Rep.Damage += Dmg;

                    if (f)
                    {
                        Rep.Fatal = true;
                    }

                    if (texmaps.TileLOS(SC.gb.POV, P.x, P.y))
                    {
                        if (f)
                            rpgtext.DCAppendMessage(tname + " died!");
                        else if (tname == "You")
                            rpgtext.DCAppendMessage("You are hit!");
                        else if (Dmg > 0)
                            rpgtext.DCAppendMessage(tname + " is hit!");
                        else if (Dmg < 0)
                            rpgtext.DCAppendMessage(tname + " is healed!");
                    }
                }
            }
            else
            {
                //{ There's no model here. Just do the gfx.}
                texmaps.MapSplat(SC.gb, '+', AR.C, P.x, P.y, false);
            }

            //{ If visible, do an animation delay.
            if (texmaps.TileLOS(SC.gb.POV, P.x, P.y))
                texfx.DelayDiv(2);

            //{If there's a wall here, break the loop.}
            if (texmaps.TerrPass[texmaps.GetTerr(SC.gb, P.x, P.y) - 1] < 1)
                break;
        }

        //{Clean up the display.}
        for (int t = 1; t <= AR.Range; ++t)
        {
            //{Calculate the current target square.}
            texmaps.Point P = texmaps.SolveLine(AR.Attacker.x, AR.Attacker.y, AR.TX, AR.TY, t);
            texmaps.DisplayTile(SC.gb, P.x, P.y);
        }

        return Rep;
    }

    static AttackReport BlastAttack(gamebook.Scenario SC, AttackRequest AR)
    {
        //{This attack is, like, a big explosion or something.}

        //{Initialize values.}
        AttackReport Rep = new AttackReport();
        Rep.ItHit = false;
        Rep.Fatal = false;
        Rep.Damage = 0;
        Rep.XPV = 0;

        int BRad = spells.AAVal(AR.ATT, spells.AA_BlastAttack);
        if (BRad < 0)
            BRad = 0;

        bool Vis = false;

        //{Check to see if the shot hit the desired spot}
        if (AR.Range != 0 && rpgdice.RollStep(AR.HitRoll) > texmaps.Range(AR.Attacker, AR.TX, AR.TY) / AR.Range + BlastBaseTarget)
        {
            //{roll for deviation}
            //{We'll use X for the total range right now.}
            int X = texmaps.Range(AR.Attacker, AR.TX, AR.TY) / 2;
            if (X < 2)
                X = 2;
            AR.TX += rpgdice.Random(X) - rpgdice.Random(X);
            AR.TY += rpgdice.Random(X) - rpgdice.Random(X);
        }

        //{Check to make sure our grenade isn't trying to bounce through a wall.}
        if (texmaps.CalcObscurement(AR.Attacker, AR.TX, AR.TY, SC.gb) == -1)
        {
            texmaps.Point P = texmaps.LocateStop(SC.gb, AR.Attacker.x, AR.Attacker.y, AR.TX, AR.TY);
            AR.TX = P.x;
            AR.TY = P.y;
        }

        //{Do the initial message here, if appropriate}
        if (texmaps.TileLOS(SC.gb.POV, AR.Attacker.x, AR.Attacker.y))
        {
            //{Print message saying what's going on.}
            if (AR.Attacker.kind == dcchars.MKIND_Character)
                rpgtext.DCGameMessage("You " + AR.Desc + ".");
            else
                rpgtext.DCGameMessage(gamebook.ModelName(SC, AR.Attacker) + AR.Desc + ".");
        }

        //{Display the path of the projectile, if appropriate.}
        if (texmaps.TileLOS(SC.gb.POV, AR.Attacker.x, AR.Attacker.y) || texmaps.TileLOS(SC.gb.POV, AR.TX, AR.TY))
        {
            texfx.DisplayShot(SC.gb, AR.Attacker.x, AR.Attacker.y, AR.TX, AR.TY, AR.C, true);
        }

        for (int X = AR.TX - BRad; X <= AR.TX + BRad; ++X)
        {
            for (int Y = AR.TY - BRad; Y <= AR.TY + BRad; ++Y)
            {
                int O = texmaps.CalcObscurement(X, Y, AR.TX, AR.TY, SC.gb);
                if (O > -1 && O < AR.Damage)
                {
                    if (texmaps.TileLOS(SC.gb.POV, X, Y))
                    {
                        //{This square will be affected by the blast.}
                        texmaps.MapSplat(SC.gb, '*', AR.C, X, Y, true);
                        Vis = true;
                    }

                    if (texmodel.ModelPresent(SC.gb.mog, X, Y))
                    {
                        //{Determine the defense roll of the target.}
                        int DRoll = RollDefenses(SC, AR, X, Y);
                        //{Determine the attack roll of the attacker.}
                        int ARoll = rpgdice.RollStep(AR.HitRoll);
                        //{Determine the target's name. Modify for PC.}
                        string tname = gamebook.ModelName(SC, texmodel.FindModelXY(SC.gb.mlist, X, Y));
                        if (tname == "you")
                            tname = "You";

                        int Dmg = 0;

                        if (ARoll > DRoll)
                        {
                            //{Hit!}
                            Rep.ItHit = true;
                            Dmg = RollDamage(AR.Damage - O);
                        }
                        else if (ARoll > DRoll / 2)
                        {
                            //{Partial hit! Half damage!}
                            Rep.ItHit = true;
                            Dmg = RollDamage(AR.Damage + 1 - O) / 2;
                        }

                        if (Dmg > 0)
                        {
                            //{Blast Attacks never score critical hits.}
                            bool f = DamageTarget(SC, X, Y, 0, AR, Dmg, ref Rep);
                            Rep.Damage += Dmg;

                            if (f)
                                Rep.Fatal = true;

                            if (texmaps.TileLOS(SC.gb.POV, X, Y))
                            {
                                if (f) rpgtext.DCAppendMessage(tname + " died!");
                                else if (tname == "You") rpgtext.DCAppendMessage("You are hit!");
                                else if (Dmg > 0) rpgtext.DCAppendMessage(tname + " is hit!");
                                else if (Dmg < 0) rpgtext.DCAppendMessage(tname + " is healed!");
                            }
                        }
                    }
                }
            }
        }

        if (Vis)
        {
            texfx.Delay();

            //{Restore the display.}
            for (int X = AR.TX - BRad; X <= AR.TX + BRad; ++X)
            {
                for (int Y = AR.TY - BRad; Y <= AR.TY; ++Y)
                {
                    texmaps.DisplayTile(SC.gb, X, Y);
                }
            }
        }

        return Rep;
    }

    static AttackReport SmokeAttack(gamebook.Scenario SC, AttackRequest AR)
    {
        //{This attack will just cause lots of smoke.}

        //   Rep: AttackReport;
        //SKind,BRad,Dur: int;	{Smoke Kind, Blast Radius, Duration}
        //X,Y: int;
        //P: Point;

        //{Initialize values.}
        AttackReport Rep = new AttackReport();
        Rep.ItHit = true;
        Rep.Fatal = false;
        Rep.Damage = 0;
        Rep.XPV = 0;

        int SKind = spells.AAVal(AR.ATT, spells.AA_SmokeAttack);
        int BRad = spells.AAVal(AR.ATT, spells.AA_Value);
        int Dur = spells.AAVal(AR.ATT, spells.AA_Duration);
        if (Dur < 1)
            Dur = 1;

        //{Check to see if the shot hit the desired spot}
        if (AR.Range != 0 && rpgdice.RollStep(AR.HitRoll) > texmaps.Range(AR.Attacker, AR.TX, AR.TY) / AR.Range + BlastBaseTarget)
        {
            //{roll for deviation}
            //{We'll use X for the total range right now.}
            int X = texmaps.Range(AR.Attacker, AR.TX, AR.TY) / 2;
            if (X < 2)
                X = 2;
            AR.TX += rpgdice.Random(X) - rpgdice.Random(X);
            AR.TY += rpgdice.Random(X) - rpgdice.Random(X);
            Rep.ItHit = false;
        }

        //{Check to make sure our grenade isn't trying to bounce through a wall.}
        if (texmaps.CalcObscurement(AR.Attacker, AR.TX, AR.TY, SC.gb) == -1)
        {
            texmaps.Point P = texmaps.LocateStop(SC.gb, AR.Attacker.x, AR.Attacker.y, AR.TX, AR.TY);
            AR.TX = P.x;
            AR.TY = P.y;
        }

        //{Do the initial message here, if appropriate}
        if (texmaps.TileLOS(SC.gb.POV, AR.Attacker.x, AR.Attacker.y))
        {
            //{Print message saying what's going on.}
            if (AR.Attacker.kind == dcchars.MKIND_Character)
                rpgtext.DCGameMessage("You " + AR.Desc + ".");
            else
                rpgtext.DCGameMessage(gamebook.ModelName(SC, AR.Attacker) + AR.Desc + ".");
        }

        //{Display the path of the projectile, if appropriate.}
        if (texmaps.TileLOS(SC.gb.POV, AR.Attacker.x, AR.Attacker.y) || texmaps.TileLOS(SC.gb.POV, AR.TX, AR.TY))
        {
            texfx.DisplayShot(SC.gb, AR.Attacker.x, AR.Attacker.y, AR.TX, AR.TY, AR.C, true);
        }

        //{Go through every point in the blast radius. If appropriate, add}
        //{a cloud to each one.}
        for (int X = AR.TX - BRad; X <= AR.TX + BRad; ++X)
        {
            for (int Y = AR.TY - BRad; Y <= AR.TY + BRad; ++Y)
            {
                if (texmaps.CalcObscurement(X, Y, AR.TX, AR.TY, SC.gb) > -1 && texmaps.TerrPass[texmaps.GetTerr(SC.gb, X, Y) - 1] > 0)
                {
                    cwords.AddCloud(ref SC.Fog, SC.gb, SKind, X, Y, SC.ComTime + (Dur * 12) + rpgdice.RollStep(6));
                }
            }
        }

        //{Return the attack report, for what it's worth.}
        return Rep;
    }

    static void TheTrapStuffIsHere(gamebook.Scenario SC, int TX, int TY)
    {
        //{Do the actual causing of damage trap stuff now.}

        //{Do the trap animation here.}
        if (texmaps.TileLOS(SC.gb.POV, TX, TY))
        {
            switch (Math.Abs(SC.gb.map[TX - 1, TY - 1].trap))
            {
                case 1: texfx.PikaPikaOuch(SC.gb, TX, TY); break;
                case 2: texfx.DakkaDakka(SC.gb, TX, TY); break;
                case 3: texfx.LaserCut(SC.gb, TX, TY); break;
            }
        }

        AttackRequest AR = new AttackRequest();
        AR.ATT = "";
        AR.Attacker = null;

        texmodel.Model M = texmodel.FindModelXY(SC.gb.mlist, TX, TY);

        if (M != null)
        {
            //{Do the damage.}
            if (TrapMan[Math.Abs(SC.gb.map[TX - 1, TY - 1].trap) - 1].DMG > 0)
            {
                int D = rpgdice.RollStep(TrapMan[Math.Abs(SC.gb.map[TX - 1, TY - 1].trap) - 1].DMG);

                AttackReport Rep = new AttackReport();
                DamageTarget(SC, TX, TY, 4, AR, D, ref Rep);

                if (texmaps.TileLOS(SC.gb.POV, TX, TY))
                {
                    rpgtext.DCAppendMessage(" " + D.ToString() + " damage!");
                }
            }
            else
            {
                if (M == SC.PC.m && TrapMan[Math.Abs(SC.gb.map[TX - 1, TY - 1].trap) - 1].DMG == 0)
                {
                    gamebook.SetTrigger(SC, "ALARM");
                }
            }
        }
    }
}
