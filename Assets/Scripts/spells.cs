using System;
using System.IO;

public class spells
{
	//{This unit holds the definition lists for psi abilities}
    //{that the player might have.}

    //{Spell Memory - spell learned by PC}
    public class SpellMem
    {
        public SpellMem(int code, char mnem)
        {
            this.code = code;
            this.mnem = mnem;
            this.next = null;
        }

        public int code;  //{Spell #}
        public char mnem; //{QuickSpell character}
        public SpellMem next;
    }

    //{Spell Description}
    public class SpellDesc
    {
        public SpellDesc(string name, string cdesc, int eff, int step, int p1, int p2, int cost, Crt.Color c, string ATT, string Desc)
        {
            this.name = name;
            this.cdesc = cdesc;
            this.eff = eff;
            this.step = step;
            this.p1 = p1;
            this.p2 = p2;
            this.cost = cost;
            this.c = c;
            this.ATT = ATT;
            this.Desc = Desc;
        }

        public SpellDesc Clone()
        {
            return new SpellDesc(name, cdesc, eff, step, p1, p2, cost, c, ATT, Desc);
        }

        public string name;
        public string cdesc;  //{description when cast}
        public int eff;       //{Spell effect code}
        public int step;      //{Spell magnitude}
        public int p1;        //{Spell parameters}
        public int p2;
        public int cost;
        public Crt.Color c;   //{Color of the spell}
        public string ATT;    //{Spell attributes}
        public string Desc;   //{Description for menus. }
	}


	//{These constants describe various Attack Attributes.}
	//{CONVENTION: Attribute strings are UC Char + LC Char}
	//{  Support Parameters are "#" + LC Char}
	public const string AA_LineAttack = "La";   //{Attacks every target along its line of fire.}
    public const string AA_BlastAttack = "Ba";  //{Attacks every model within R of its target point.}
    public const string AA_SmokeAttack = "Sa";  //{Creates smoke.Value is smoke type.}

    public const string AA_ArmorPiercing = "Ap";    //{Armor only counts half}
    public const string AA_ArmorDoubling = "PU";    //{Armor counts double}
    public const string AA_Element = "El";  //{Attack has an associated element.}
    public const string     AA_ElemFire = "El01";
    public const string     AA_ElemCold = "El02";
    public const string     AA_ElemLit =  "El03";
    public const string     AA_ElemAcid = "El04";
    public const string     AA_ElemHoly = "El05";
    public const string AA_StatusChange = "Sc";
    public const string     AA_StatusPar = "Sc01";
    public const string     AA_StatusSleep = "SC02";
    public const string     AA_StatusPsn = "Sc03";
    public const string AA_Slaying = "Sl";	//{Slays specific critter types.}
    public const string     AA_SlayAlive = "Sl01";
    public const string     AA_SlayUndead = "Sl02";
    public const string     AA_SlayMech = "Sl03";

    public const string AA_Value = "#v";
    public const string AA_HitRoll = "#h";
    public const string AA_Duration = "#d";

	public const int EFF_ShootAttack = 0; //{Works like a shooting attack. Magic missile kind of thing.}
		                                  //{P1 = ACC, P2 = RNG}

	public const int EFF_CloseAttack = 1; //{Works like a H2H attack. UI is different from above.}
		                                  //{P1 = ACC}

	public const int EFF_Residual = 2;    //{Add status change to caster}
		                                  //{Step = SEF; P1 = Value }

	public const int EFF_Healing = 3;     //{Recover HP}

    public const int EFF_MagicMap = 5;    //{Reveal areas of the map.}
                                          //{Step = % area; P1 = Range; P2 = TerrPass filter}

	public const int EFF_StatAttack = 6;  //{Cause StatusChange in monsters within radius}
                                          //{Step = SEF; P1 = Range; P2 = HitRoll; Value in ATT}

	public const int EFF_CureStatus = 7;  //{Cure a certain status condition.}
                                          //{Step = SEF}

    public const int EFF_Teleport = 8;    //{Teleport caster.}
                                          //{Step = Range; P1 = Control (0=Random, 1=Select) }

    public const int EFF_SenseAura = 9;   //{See location of monsters on screen.}
                                          //{Step = Model kind to detect}



	public const int NumSpell = 25;
	public static SpellDesc[] SpellMan = new SpellDesc[]
    {
		new SpellDesc("Flamwave",
			cdesc: "call forth a flaming arc",
			eff: EFF_ShootAttack,
			step: 2, p1: 0, p2: 6, cost: 5,
			c: Crt.Color.LightRed, ATT: AA_LineAttack + AA_ElemFire,
			Desc: "A cone of pyrokinetic flame which burns targets up to 6m away."),
		new SpellDesc("Force Bolt",
			cdesc: "project mental energy towards",
			eff: EFF_ShootAttack,
			step: 5, p1: 1, p2: 6, cost: 3,
			c: Crt.Color.White, ATT: "",
			Desc: "A bolt of telekinetic energy which does concussive damage to a target."),
		new SpellDesc("Implosion",
			cdesc: "shread",
			eff: EFF_CloseAttack,
			step: 12, p1: 3, p2: 0, cost: 2,
			c: Crt.Color.LightCyan, ATT: "",
			Desc: "Time-space is slightly pinched at the location of the target, resulting in horrible damage to its physical form."),
		new SpellDesc("Alter Perception",
			cdesc: "",
			eff: EFF_Residual,
			step: statusfx.SEF_VisionBonus, p1: 5, p2: 10, cost: 4,
			c: Crt.Color.Yellow, ATT: "",
			Desc: "Use of this talent allows the character an extended visual range for about ten minutes."),
		new SpellDesc("Heal Wounds",
			cdesc: "",
			eff: EFF_Healing,
			step: 5, p1: 0, p2: 0, cost: 2,
			c: Crt.Color.LightGreen, ATT: "",
			Desc: "Physical injuries may be mended by this talent."),

		//{ 6 - 10 }
		new SpellDesc("Remote Viewing",
			cdesc: "",
			eff: EFF_MagicMap,
			step: 25, p1: 32, p2: 100, cost: 8,
			c: Crt.Color.White, ATT: "",
			Desc: "Distant spaces may become known through this talent."),
		new SpellDesc("Armor Up",
			cdesc: "",
			eff: EFF_Residual,
			step: statusfx.SEF_ArmorBonus, p1: 5, p2: 0, cost: 3,
			c: Crt.Color.Blue, ATT: "",
			Desc: "The armor of the character will be telekinetically strengthened for about a half an hour."),
		new SpellDesc("Power Up",
			cdesc: "",
			eff: EFF_Residual,
			step: statusfx.SEF_CCDmgBonus, p1: 10, p2: 0, cost: 2,
			c: Crt.Color.Red, ATT: "",
			Desc: "For ten minutes the character's close combat attacks will do much more damage."),
		new SpellDesc("Regenerate",
			cdesc: "",
			eff: EFF_Residual,
			step: statusfx.SEF_Regeneration, p1: 15, p2: 0, cost: 10,
			c: Crt.Color.Green, ATT: "",
			Desc: "Psi energy is converted over time into life energy, speeding up the healing process."),
		new SpellDesc("Speed Up",
			cdesc: "",
			eff: EFF_Residual,
			step: statusfx.SEF_SpeedBonus, p1: 10, p2: 0, cost: 6,
			c: Crt.Color.Blue, ATT: "",
			Desc: "For ten minutes the character will move faster."),

		//{ 11 - 15 }
		new SpellDesc("Obscure Aura",
			cdesc: "",
			eff: EFF_Residual,
			step: statusfx.SEF_StealthBonus, p1: 10, p2: 0, cost: 5,
			c: Crt.Color.Blue, ATT: "",
			Desc: "It becomes far more difficult for enemies to spot the character. Lasts thirty minutes."),
		new SpellDesc("Shockwave",
			cdesc: "call forth a bolt of lightning",
			eff: EFF_ShootAttack,
			step: 3, p1: 1, p2: 8, cost: 7,
			c: Crt.Color.Yellow, ATT: AA_LineAttack + AA_ElemLit,
			Desc: "A bolt of lightening strikes all foes within 8m."),
		new SpellDesc("Guided Fire",
			cdesc: "",
			eff: EFF_Residual,
			step: statusfx.SEF_MslBonus, p1: 10, p2: 0, cost: 3,
			c: Crt.Color.LightCyan, ATT: "",
			Desc: "All missile attacks are much more likely to hit for one minute."),
		new SpellDesc("Cryoblast",
			cdesc: "hurl a freezing vortex at",
			eff: EFF_ShootAttack,
			step: 5, p1: 3, p2: 5, cost: 5,
			c: Crt.Color.LightBlue, ATT: AA_ElemCold,
			Desc: "A bolt of negative thermal potential strikes one enemy."),
		new SpellDesc("Soul Hammer",
			cdesc: "blast",
			eff: EFF_CloseAttack,
			step: 15, p1: 0, p2: 0, cost: 3,
			c: Crt.Color.LightCyan, ATT: AA_ElemHoly,
			Desc: "One nearby foe is struck with a bolt of pure spiritual energy."),

		//{ 16 - 20 }
		new SpellDesc("Sleep",
			cdesc: "",
			eff: EFF_StatAttack,
			step: statusfx.SEF_Sleep, p1: 3, p2: 0, cost: 8,
			c: Crt.Color.LightGray, ATT: AA_Value+"05",
			Desc: "All enemies within 3m will likely fall asleep."),
		new SpellDesc("Knockdown",
			cdesc: "project mental energy towards",
			eff: EFF_ShootAttack,
			step: 2, p1: 2, p2: 8, cost: 6,
			c: Crt.Color.White, ATT: AA_StatusSleep + AA_HitRoll + "06" + AA_Value + "01",
			Desc: "A wave of psi energy is shot at one foe, possibly overloading its nervous system."),
		new SpellDesc("Purge",
			cdesc: "project waves of spiritual energy",
			eff: EFF_ShootAttack,
			step: 1, p1: 1, p2: 12, cost: 9,
			c: Crt.Color.LightCyan, ATT: AA_LineAttack + AA_SlayUndead + AA_ElemHoly,
			Desc: "A column of unleashed spiritual energy blasts every foe within 12m."),
		new SpellDesc("Cure Poison",
			cdesc: "",
			eff: EFF_CureStatus,
			step: statusfx.SEF_Poison, p1: 0, p2: 0, cost: 10,
			c: Crt.Color.LightGreen, ATT: "",
			Desc: "The poison status effect may be cured."),
		new SpellDesc("Theta Bolt",
			cdesc: "project mental energy towards",
			eff: EFF_ShootAttack,
			step: 4, p1: 2, p2: 7, cost: 10,
			c: Crt.Color.LightBlue, ATT: AA_StatusSleep + AA_HitRoll + "05" + AA_Value + "03",
			Desc: "One enemy is struck by a calculated blast of psi potential. It may prove too much for their feeble senses."),

		//{ 21 - 25 }
		new SpellDesc("Stasis",
			cdesc: "",
			eff: EFF_StatAttack,
			step: statusfx.SEF_Paralysis, p1: 1, p2: 0, cost: 12,
			c: Crt.Color.LightMagenta, ATT: AA_Value+"02",
			Desc: "All foes adjacent to the character may be frozen in time."),
		new SpellDesc("Inferno",
			cdesc: "call forth a firestorm",
			eff: EFF_ShootAttack,
			step: 8, p1: 0, p2: 5, cost: 12,
			c: Crt.Color.LightRed, ATT: AA_BlastAttack + "01" + AA_ElemFire,
		    Desc: "This powerful pyrokinetic attack affects all foes within one and a half meters of its detonation point."),
		new SpellDesc("Warp Gate",
			cdesc: "",
			eff: EFF_Teleport,
			step: 30, p1: 0, p2: 0, cost: 7,
			c: Crt.Color.LightGreen, ATT: "",
			Desc: "The character can make a short jump through transreal space to a nearby random location."),
		new SpellDesc("Sense Aura",
			cdesc: "",
			eff: EFF_SenseAura,
			//{ NOTE - Manually pasting in MKIND_Critter here.}
			step: 2, p1: 0, p2: 0, cost: 4,
			c: Crt.Color.LightGreen, ATT: "",
			Desc: "The character will for a moment sense the presence of all other beings in the vicinity."),
		new SpellDesc("Etherial Mist",
			cdesc: "call forth mysterious clouds",
			eff: EFF_ShootAttack,
			step: 5, p1: -1, p2: 2, cost: 5,
			c: Crt.Color.White, ATT: AA_SmokeAttack + "01" + AA_Value + "03" + AA_Duration + "15",
			Desc: "Smoky mists formed from psychic matter can hide the caster and provide cover."),
	};


	//{The following array holds spell advancement lists for}
	//{all the different types of spellcaster in the game.}
	public const int NumSchool = 5;
	public const int NumLevel = 12;
    public const int MaxNewSpellsPerLevel = 5;
	public const int SCHOOL_Astral = 1;
	public const int SCHOOL_Zeomancy = 2;
	public const int SCHOOL_Navigator = 3;
	public const int SCHOOL_Samurai = 4;
	public const int SCHOOL_DemonHunter = 5;

	public static int [,,] SpellCollege = new int[NumSchool, NumLevel, MaxNewSpellsPerLevel]
    {
		{	{  2,  5,  6, 11,  0},	//{Astral Seer}
			{  7, 15, 24,  0,  0},
			{ 16, 19,  0,  0,  0},
			{ 18, 23,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0}		},

		{	{  1,  3,  4, 24, 25},	//{Zeomancer}
			{ 23, 13,  0,  0,  0},
			{ 14,  9,  0,  0,  0},
			{ 12, 20,  0,  0,  0},
			{ 19, 21,  0,  0,  0},
			{ 22,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0}		},

		{	{  2,  4,  5, 13,  0},	//{Navigator}
			{  6,  3,  0,  0,  0},
			{ 10, 23,  0,  0,  0},
			{ 16, 20,  0,  0,  0},
			{ 11, 19,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0},
			{  0,  0,  0,  0,  0}		},

		{   {  0,  0,  0,  0,  0},	//{Samurai}
			{  0,  0,  0,  0,  0},
            {  7,  8,  0,  0,  0},
            {  9,  1,  0,  0,  0},
            { 10, 13,  0,  0,  0},
            { 17, 19,  0,  0,  0},
            { 23,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0}		},

        {   { 13, 18,  9,  8, 15},	//{Demon Hunter}
			{ 19,  6, 23,  0,  0},
            { 10, 22,  0,  0,  0},
            { 17,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0}       }


	};

    public static SpellMem LocateSpellMem(SpellMem llist, int s)
    {
	    //{Search through list LList looking for SpellMem S.}
	    //{If found, return the address of that list member.}
	    //{If not found, return Nil.}
	    SpellMem it = null;

        while (llist != null)
        {
            if (llist.code == s)
            {
                it = llist;
            }

            llist = llist.next;
        }

        return it;
    }

    public static SpellMem AddSpellMem(ref SpellMem llist, int c)
    {
	    //{Add a new element to the end of LList.}

	    //{Check first to see if the spell is already known. If so,}
	    //{do nothing now.}
	    SpellMem it = LocateSpellMem(llist, c);

        if (it == null)
        {
            //{Allocate memory for our new element.}
            //{Initialize values.}
            it = new SpellMem(c, ' ');

            //{Attach IT to the list.}
            if (llist == null)
            {
                llist = it;
            }
            else
            {
                LastSpellMem(llist).next = it;
            }
        }

        //{Return a pointer to the new element.}
        return it;
    }

    public static void RemoveSpellMem(ref SpellMem llist, SpellMem lmember)
    {
	    //{Locate and extract member LMember from list LList.}
	    //{Then, dispose of LMember.}

	    //{Initialize A and B}
	    SpellMem b = llist;
	    SpellMem a = null;

        //{Locate LMember in the list. A will thereafter be either Nil,}
        //{if LMember if first in the list, or it will be equal to the}
        //{element directly preceding LMember.}
        while (b != lmember && b != null)
        {
            a = b;
            b = b.next;
        }

        if (b == null)
        {
            //{Major FUBAR. The member we were trying to remove can't}
            //{be found in the list.}
            Crt.Write("ERROR- RemoveSpellMem asked to remove a link that doesnt exist.\n");
            do { rpgtext.RPGKey(); } while (true);
        }
        else if (a == null)
        {
            //{There's no element before the one we want to remove,}
            //{i.e. it's the first one in the list.}
            llist = b.next;
        }
        else
        {
            //{We found the attribute we want to delete and have another}
            //{one standing before it in line. Go to work.}
            a.next = b.next;
        }
    }

    public static int AAVal(string AttList, string A)
    {
        //{Given attribute list AttList and attribute A, retrieve}
        //{the numerical value associated with said attribute.}
        //{This value occupies two chars in the string immediately}
        //{following the attribute code. Return 0 if no such value}
        //{can be found.}
        int i = AttList.IndexOf(A);
        int v = 0;

        if (i >= 0)
        {
            string s = AttList.Substring(i + A.Length, 2);
            v = int.Parse(s);
        }

        return v;
    }

    public static void WriteSpellMem(SpellMem SL, StreamWriter f)
    {
        //{Save the linked list of spells to the file F.}
        while (SL != null)
        {
            f.WriteLine(SL.code);
            f.WriteLine(SL.mnem);
            SL = SL.next;
        }

	    f.WriteLine(-1);
    }

    public static SpellMem ReadSpellMem(StreamReader f)
    {
        //{Load a list of items saved by the above procedure from}
        //{the file F.}
        SpellMem SL = null;

        int code = int.Parse(f.ReadLine());
        
        while (code != -1)
        {
            SL = AddSpellMem(ref SL, code);
            SL.mnem = char.Parse(f.ReadLine());

            code = int.Parse(f.ReadLine());
        }

        return SL;
    }

    static SpellMem LastSpellMem(SpellMem llist)
    {
        //{Search through the linked list, and return the last element.}
        //{If LList is empty, return Nil.}
        if (llist != null)
        {
            while (llist.next != null)
            {
                llist = llist.next;
            }
        }

        return llist;
    }
}
