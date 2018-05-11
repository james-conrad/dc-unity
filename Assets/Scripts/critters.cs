using System;
using System.IO;

public class critters
{
    //{This unit defines the creature stuff for DeadCold. It doesn't}
    //{deal with creature behavior; just some primitive creature handling}
    //{routines.}

    public struct CDesc
    {
        public CDesc(string name, string CT, string IntroText,
                     char Gfx, Crt.Color Color, int MaxHP, int Armor,
                     int DefStep, int Mystic, int AIType, int Sense,
                     int Speed, int HitRoll, int Damage, int Range,
                     string AtAt, string ADesc, int TType, int TDrop,
                     int TNum, int EType, int EChance, int XPV)
        {
            this.name = name;
            this.CT = CT;
            this.IntroText = IntroText;
            this.Gfx = Gfx;
            this.Color = Color;
            this.MaxHP = MaxHP;
            this.Armor = Armor;
            this.DefStep = DefStep;
            this.Mystic = Mystic;
            this.AIType = AIType;
            this.Sense = Sense;
            this.Speed = Speed;
            this.HitRoll = HitRoll;
            this.Damage = Damage;
            this.Range = Range;
            this.AtAt = AtAt;
            this.ADesc = ADesc;
            this.TType = TType;
            this.TDrop = TDrop;
            this.TNum = TNum;
            this.EType = EType;
            this.EChance = EChance;
            this.XPV = XPV;
        }

        public string name;
        public string CT;
        public string IntroText;
        public char Gfx;
        public Crt.Color Color;
        public int MaxHP;
        public int Armor;
        public int DefStep;
        public int Mystic;
        public int AIType;
        public int Sense; //{Sensory range and action speed}
        public int Speed;
        public int HitRoll;
        public int Damage;
        public int Range;
        public string AtAt; //{Attack Attributes.}
        public string ADesc; //{Attack description.}
        public int TType; //{Treasure type, and chance of dropping loot.}
        public int TDrop;
        public int TNum;
        public int EType; //{Equipment type, and chance of having it.}
        public int EChance;
        public int XPV; //{Experience Value}
    };

    public const int MaxCrit = 24;

    public const int NumCT = 8;
    public const string CT_Alive = "Al";
    public const string CT_Undead = "Ud";
    public const string CT_Mech = "Mk";
    public const string CT_Flying = "Fl";
    public const string CT_Etheral = "Et";
    public const string CT_Bug = "Bg";
    public const string CT_Cold = "Ec";
    public const string CT_Hot = "Eh";

    public static string[] CTMan = new string[NumCT]
    {
        CT_Alive, CT_Undead, CT_Mech, CT_Flying, CT_Etheral,
        CT_Bug, CT_Cold, CT_Hot
    };

    //{ The Extended Critter Types describe various critters, }
    //{ but they don't contribute to special attacks/defenses. }
    public const string XCT_Breeder = "Br";

    //{The numbers in the following array represent resistance to}
    //{elemental attacks. 0 is average; -5 is extreme vunerability}
    //{and 5 is element absorbtion. 4 is complete immunity.}
    public static int[,] CTResist = new int[NumCT, statusfx.NumElem + 1]
    {
        { 0, 0, 0, 0, 0, 0},
        { 0,-1, 1, 0, 0,-5},
        { 0, 0, 0,-2, 0, 1},
        { 0, 0,-1,-2, 0, 0},
        { 4, 0, 0, 0, 3,-2},
        { 0, 0,-1, 0,-2, 0},
        { 1,-5, 5, 0, 0, 0},
        { 1, 5,-5, 0, 0, 0}
    };

    public static bool[,] CTAvoid = new bool[NumCT, statusfx.NumNegSF]
    {
		//{   Par  Sleep    Psn             Attribute Draining                          }
		{  true,  true,  true, false, false, false, false, false, false, false, false},
        {  true, false, false, false, false, false, false, false, false, false, false},
        {  true, false, false, false, false, false, false, false, false, false, false},
        {  true,  true,  true, false, false, false, false, false, false, false, false},
        {  true,  true,  true, false, false, false, false, false, false, false, false},
        {  true,  true,  true, false, false, false, false, false, false, false, false},
        {  true,  true,  true, false, false, false, false, false, false, false, false},
        {  true,  true,  true, false, false, false, false, false, false, false, false}
    };

    public const int AIT_PCHunter = 1;   //{PCHunter will pursue PC if in range.}
    public const int AIT_Passive = 2;    //{Passive will move randomly and never attack.}
    public const int AIT_Chaos = 3;      //{Chaos will move randomly and attack whatever it encounters.}
    public const int AIT_Guardian = 4;   //{Creature will guard a room, attacking nearby targets.}
    public const int AIT_Slime = 5;      //{Creature can't walk, but attacks any models that pass within range.}
    public const int AIT_HalfHunter = 6; //{Half of the time, acts as PCHunter. Half of the time, acts as Chaos.}

    public static CDesc[] MonMan = new CDesc[MaxCrit]
    {
        new CDesc( "Maintenance Bot",
            CT: CT_Mech,
            IntroText: null,
            Gfx: 'R', Color: Crt.Color.LightGray,
            MaxHP: 25, Armor: 15, DefStep: 1, Mystic: 2,
            AIType: AIT_Passive, Sense: 10, Speed: 5,
            HitRoll: 2, Damage: 13, Range: -1,
            AtAt: "",
            ADesc: "rams",
            TType: 6, TDrop: 15, TNum: 2,
            EType: 5, EChance: 15,
            XPV: 15
        ),
        new CDesc( "Mutant Rat",
            CT: CT_Alive,
            IntroText: "You see a rat of enormous size, probably escaped from one of the station's science labs. It doesn't look too friendly.",
            Gfx: 'r', Color: Crt.Color.Brown,
            MaxHP: 3, Armor: 0, DefStep: 4, Mystic: 1,
            AIType: AIT_PCHunter, Sense: 6, Speed: 8,
            HitRoll: 10, Damage: 2, Range: -1,
            AtAt: "",
            ADesc: "charges",
            TType: 0, TDrop: 0, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 5
        ),
        new CDesc( "Corpse",
            CT: CT_Undead,
            IntroText: "Finally, another person! Relief from finding someone turns to horror as you realize that the being standing there is no longer alive. The corpse stares at you through unliving eyes. It begins to walk towards you...",
            Gfx: '@', Color: Crt.Color.Yellow,
            MaxHP: 200, Armor: 2, DefStep: 5, Mystic: 12,
            AIType: AIT_PCHunter, Sense: 3, Speed: 5,
            HitRoll: 12, Damage: 12, Range: -1,
            AtAt: "",
            ADesc: "claws",
            TType: 3, TDrop: 5, TNum: 1,
            EType: 4, EChance: 20,
            XPV: 45
        ),
        new CDesc( "Vacuum Worm",
            CT: CT_Alive + CT_Bug,
            IntroText: "There's a vacuum worm inside the station! These parasites live on spacecraft hulls throughout the galaxy, but defense screens usually keep them on the outside.",
            Gfx: 'w', Color: Crt.Color.Magenta,
            MaxHP: 10, Armor: 0, DefStep: 1, Mystic: 1,
            AIType: AIT_HalfHunter, Sense: 2, Speed: 6,
            HitRoll: 8, Damage: 3, Range: -1,
            AtAt: "",
            ADesc: "lunges at",
            TType: 6, TDrop: 1, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 4
        ),
        new CDesc( "Sentry Drone",
            CT: CT_Mech,
            IntroText: null,
            Gfx: 'R', Color: Crt.Color.LightGray,
            MaxHP: 20, Armor: 15, DefStep: 3, Mystic: 2,
            AIType: AIT_Guardian, Sense: 9, Speed: 4,
            HitRoll: 6, Damage: 5, Range: 3,
            AtAt: "",
            ADesc: "fires at",
            TType: 6, TDrop: 10, TNum: 1,
            EType: 5, EChance: 100,
            XPV: 15
        ),

	//{ CRITTERS 6 - 10 }
		new CDesc( "Locust",
            CT: CT_Alive + CT_Flying + CT_Bug,
            IntroText: "You see some kind of flying insect, about the size of a large cat. No idea what it is or how it got here, but it may have something to do with the emergency that has affected the station.",
            Gfx: 'i', Color: Crt.Color.LightGreen,
            MaxHP: 1, Armor: 3, DefStep: 4, Mystic: 5,
            AIType: AIT_PCHunter, Sense: 6, Speed: 8,
            HitRoll: 11, Damage: 1, Range: 2,
            AtAt: spells.AA_ElemAcid,
            ADesc: "spits bile at",
            TType: 0, TDrop: 0, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 3
        ),
        new CDesc( "Maximillian",
            CT: CT_Mech + CT_Flying,
            IntroText: null,
            Gfx: 'R', Color: Crt.Color.Red,
            MaxHP: 300, Armor: 75, DefStep: 17, Mystic: 15,
            AIType: AIT_Guardian, Sense: 8, Speed: 9,
            HitRoll: 21, Damage: 23, Range: 8,
            AtAt: "",
            ADesc: "fires Laser Cannon at",
            TType: 6, TDrop: 10, TNum: 5,
            EType: 0, EChance: 0,
            XPV: 150
        ),
        new CDesc( "Polyp",
            CT: CT_Alive,
            IntroText: "Oozing across the floor, you see a shapeless blob of a creature. Items which it has injested are visible beneath its pale orange skin. You hope that it isn't interested in eating you as well...",
            Gfx: 'P', Color: Crt.Color.LightRed,
            MaxHP: 20, Armor: 5, DefStep: 2, Mystic: 1,
            AIType: AIT_HalfHunter, Sense: 4, Speed: 5,
            HitRoll: 9, Damage: 7, Range: -1,
            AtAt: "",
            ADesc: "touches",
            TType: 3, TDrop: 25, TNum: 3,
            EType: 0, EChance: 0,
            XPV: 10
        ),
        new CDesc( "Red Gore",
            CT: CT_Alive,
            IntroText: "Standing before you is a hideous column of diseased flesh. It seems to be immobile, but the acid being excreted from its pores is eating its way through the station's hull.",
            Gfx: 'X', Color: Crt.Color.Red,
            MaxHP: 35, Armor: 5, DefStep: 1, Mystic: 4,
            AIType: AIT_Slime, Sense: 6, Speed: 9,
            HitRoll: 10, Damage: 9, Range: -1,
            AtAt: spells.AA_ElemAcid,
            ADesc: "touches",
            TType: 3, TDrop: 2, TNum: 5,
            EType: 0, EChance: 0,
            XPV: 10
        ),
        new CDesc( "Scarab",
            CT: CT_Undead + CT_Flying + CT_Bug,
            IntroText: "You see a large black beetle, like the scarabs sometimes used to decorate tombs. This one appears to be alive.",
            Gfx: 'i', Color: Crt.Color.Blue,
            MaxHP: 5, Armor: 4, DefStep: 6, Mystic: 6,
            AIType: AIT_Guardian, Sense: 4, Speed: 7,
            HitRoll: 12, Damage: 3, Range: -1,
            AtAt: spells.AA_ArmorPiercing+spells.AA_StatusPar+spells.AA_HitRoll+"02",
            ADesc: "bites",
            TType: 0, TDrop: 0, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 7
        ),

	//{ CRITTERS 11 - 15 }
		new CDesc( "Mycozoan Spore",
            CT: CT_Alive + CT_Flying,
            IntroText: "Mycozoan spores drift through the void, feeding on vacuum worms and organic debris. There's one inside the station.",
            Gfx: '`', Color: Crt.Color.Brown,
            MaxHP: 2, Armor: 7, DefStep: 1, Mystic: 1,
            AIType: AIT_Chaos, Sense: 2, Speed: 3,
            HitRoll: 11, Damage: 2, Range: -1,
            AtAt: spells.AA_StatusPsn+spells.AA_Value+"02"+spells.AA_Duration+"06"+spells.AA_HitRoll+"13",
            ADesc: "gropes",
            TType: 7, TDrop: 1, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 5
        ),
        new CDesc( "Mutant Rabbit",
            CT: CT_Alive,
            IntroText: "You spot a hideously twisted rabbit, as large as a man and covered in oozing sores. It probably escaped from the same place as all those rats.",
            Gfx: 'r', Color: Crt.Color.White,
            MaxHP: 15, Armor: 3, DefStep: 5, Mystic: 1,
            AIType: AIT_PCHunter, Sense: 8, Speed: 9,
            HitRoll: 14, Damage: 7, Range: -1,
            AtAt: "",
            ADesc: "charges",
            TType: 2, TDrop: 2, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 15
        ),
        new CDesc( "Stench of Death",
            CT: CT_Etheral,
            IntroText: null,
            Gfx: '*', Color: Crt.Color.DarkGray,
            MaxHP: 10, Armor: 0, DefStep: 3, Mystic: 10,
            AIType: AIT_HalfHunter, Sense: 5, Speed: 6,
            HitRoll: 9, Damage: 4, Range: -1,
            AtAt: spells.AA_StatusPsn + spells.AA_Value + "03" + spells.AA_HitRoll + "06" + spells.AA_Duration + "02",
            ADesc: "touches",
            TType: 0, TDrop: 0, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 20
        ),
        new CDesc( "Gelatenous Mass",
            CT: CT_Alive + CT_Bug,
            IntroText: null,
            Gfx: ',', Color: Crt.Color.LightBlue,
            MaxHP: 22, Armor: 10, DefStep: 3, Mystic: 3,
            AIType: AIT_Slime, Sense: 1, Speed: 2,
            HitRoll: 9, Damage: 3, Range: -1,
            AtAt: "",
            ADesc: "touches",
            TType: 3, TDrop: 30, TNum: 2,
            EType: 1, EChance: 70,
            XPV: 4
        ),
        new CDesc( "Spike Mushroom",
            CT: CT_Alive,
            IntroText: "There is a giant mushroom growing from a pile of debris on the floor. It is covered in spikes, better not get too close.",
            Gfx: 'm', Color: Crt.Color.Brown,
            MaxHP: 5, Armor: 0, DefStep: 2, Mystic: 3,
            AIType: AIT_Slime, Sense: 2, Speed: 4,
            HitRoll: 9, Damage: 2, Range: 5,
            AtAt: spells.AA_StatusSleep + spells.AA_HitRoll + "04",
            ADesc: "fires spines at",
            TType: 3, TDrop: 15, TNum: 3,
            EType: 0, EChance: 0,
            XPV: 6
        ),

	//{ CRITTERS 16 - 20 }
		new CDesc( "Corpse Eater",
            CT: CT_Alive,
            IntroText: "You see a long, bloated centipede chewing on what looks like the remains of one of the station's crew members.",
            Gfx: 'C', Color: Crt.Color.Green,
            MaxHP: 25, Armor: 7, DefStep: 6, Mystic: 6,
            AIType: AIT_Passive, Sense: 8, Speed: 6,
            HitRoll: 12, Damage: 7, Range: -1,
            AtAt: spells.AA_StatusPar+spells.AA_HitRoll+"05",
            ADesc: "bites",
            TType: 2, TDrop: 1, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 17
        ),
        new CDesc( "Buzzsaw Bot",
            CT: CT_Mech,
            IntroText: "That robot is acting strangely...",
            Gfx: 'R', Color: Crt.Color.LightGray,
            MaxHP: 10, Armor: 15, DefStep: 3, Mystic: 2,
            AIType: AIT_HalfHunter, Sense: 2, Speed: 10,
            HitRoll: 5, Damage: 17, Range: -1,
            AtAt: "",
            ADesc: "saws",
            TType: 6, TDrop: 5, TNum: 1,
            EType: 5, EChance: 2,
            XPV: 21
        ),
        new CDesc( "Parasite",
            CT: CT_Alive,
            IntroText: null,
            Gfx: 'w', Color: Crt.Color.Yellow,
            MaxHP: 20, Armor: 3, DefStep: 5, Mystic: 14,
            AIType: AIT_HalfHunter, Sense: 2, Speed: 4,
            HitRoll: 16, Damage: 6, Range: -1,
            AtAt: spells.AA_StatusPsn + spells.AA_Value + "01" + spells.AA_HitRoll + "03" + spells.AA_Duration + "10",
            ADesc: "lunges at",
            TType: 6, TDrop: 1, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 14
        ),
        new CDesc( "Creeping Guts",
            CT: CT_Alive,
            IntroText: null,
            Gfx: 'X', Color: Crt.Color.Red,
            MaxHP: 35, Armor: 5, DefStep: 1, Mystic: 4,
            AIType: AIT_PCHunter, Sense: 6, Speed: 3,
            HitRoll: 10, Damage: 9, Range: -1,
            AtAt: "",
            ADesc: "touches",
            TType: 3, TDrop: 2, TNum: 5,
            EType: 0, EChance: 0,
            XPV: 30
        ),
        new CDesc( "Mannikin",
            CT: CT_Undead + CT_Mech,
            IntroText: "You hear the scraping of gristle against steel. Before you stands a wretched creature, the tattered remains of its humanity held together only by wire and nails.",
            Gfx: '@', Color: Crt.Color.Cyan,
            MaxHP: 200, Armor: 10, DefStep: 7, Mystic: 10,
            AIType: AIT_PCHunter, Sense: 7, Speed: 6,
            HitRoll: 15, Damage: 10, Range: -1,
            AtAt: "",
            ADesc: "claws",
            TType: 3, TDrop: 3, TNum: 1,
            EType: 4, EChance: 35,
            XPV: 55
        ),


	//{ CRITTERS 21 - 25 }
		new CDesc( "KillBot",
            CT: CT_Mech,
            IntroText: "It's a vintage Mk.XI KillBot! Ever since the new Mk.XIII was introduced, they've been selling off the older models as bargain security units. It may be obsolete, but it's probably still deadly.",
            Gfx: 'R', Color: Crt.Color.White,
            MaxHP: 60, Armor: 15, DefStep: 5, Mystic: 17,
            AIType: AIT_Guardian, Sense: 4, Speed: 5,
            HitRoll: 7, Damage: 8, Range: 5,
            AtAt: "",
            ADesc: "fires at",
            TType: 6, TDrop: 15, TNum: 2,
            EType: 5, EChance: 100,
            XPV: 35
        ),
        new CDesc( "Sentinel",
            CT: CT_Mech,
            IntroText: null,
            Gfx: 'R', Color: Crt.Color.White,
            MaxHP: 120, Armor: 30, DefStep: 8, Mystic: 8,
            AIType: AIT_Passive, Sense: 4, Speed: 6,
            HitRoll: 11, Damage: 12, Range: 5,
            AtAt: "",
            ADesc: "fires at",
            TType: 6, TDrop: 45, TNum: 3,
            EType: 5, EChance: 30,
            XPV: 35
        ),
        new CDesc( "Dust Jelly",
            CT: CT_Alive + CT_Cold + XCT_Breeder,
            IntroText: null,
            Gfx: 'j', Color: Crt.Color.LightCyan,
            MaxHP: 2, Armor: 0, DefStep: 3, Mystic: 3,
            AIType: AIT_HalfHunter, Sense: 3, Speed: 6,
            HitRoll: 10, Damage: 1, Range: -1,
            AtAt: "",
            ADesc: "lunges at",
            TType: 6, TDrop: 1, TNum: 1,
            EType: 0, EChance: 0,
            XPV: 5
        ),
        new CDesc( "Thorny Rhizome",
            CT: CT_Alive + XCT_Breeder,
            IntroText: ".",
            Gfx: 'm', Color: Crt.Color.LightGray,
            MaxHP: 5, Armor: 0, DefStep: 2, Mystic: 3,
            AIType: AIT_Slime, Sense: 1, Speed: 2,
            HitRoll: 23, Damage: 2, Range: -1,
            AtAt: "",
            ADesc: "whips",
            TType: 3, TDrop: 2, TNum: 3,
            EType: 0, EChance: 0,
            XPV: 6
        ),
    };

    //{Define the model KIND field for critters.}
    public const int MKIND_Critter = 2;

    public class Critter
    {
        public int crit; //{Defines what kind of a creature we're dealing with.}
        public int HP;   //{Current hit points.}
        public int AIType; //{Current mood.}
        public bool Spotted; //{Whether or not this critter has been seen by the PC.}
        public int TX; //{Temporary X and Y values.}
        public int TY;
        public dcitems.DCItem Eqp; //{Critter equipment.}
        public plotbase.NAtt SF; //{Critter status.}

        public texmodel.Model Target; //{This is the model the critter is gunning for.}
        public texmodel.Model M; //{The critter's model.}

        public Critter next; //{So we can do a linked list.}
    }

    public static Critter LastCritter(Critter CP)
    {
        //{Locate the last critter in the list.}

        //{To prevent errors, first solve the trivial case.}
        if (CP == null)
            return null;

        while (CP.next != null)
        {
            CP = CP.next;
        }

        return CP;
    }

    public static Critter AddCritter(ref Critter CList, texmaps.GameBoard gb, int c, int x, int y)
    {
        //{Add a new creature, of type CRIT, to the critter list.}
        //{Allocate a model for the critter, and place it on the map at position}
        //{X,Y.}

        //{Allocate memory for IT}
        Critter it = new Critter();

	    //{Initialize all of ITs fields}
	    it.crit = c;
	    it.next = null;
	    it.AIType = MonMan[c-1].AIType;
	    it.Target = null;
	    it.Spotted = false;
	    it.Eqp = null;
	    it.SF = null;

	    //{Calculate a HitPoint value for the monster. This should be}
	    //{within +-20% of the normal maximum.}
	    it.HP = MonMan[c-1].MaxHP * (100 + rpgdice.RollStep(7) - rpgdice.Random(20)) / 100;
	    if (it.HP < 1)
            it.HP = 1;

        //{Generate a model for IT}
        Crt.Color C2 = Crt.Color.Yellow;
        if (MonMan[c - 1].Color == Crt.Color.Yellow)
        {
            C2 = Crt.Color.White;
        }

	    it.M = texmaps.GAddModel(gb, MonMan[c-1].Gfx, MonMan[c-1].Color, C2, false, x, y, MKIND_Critter);

        //{If adding a model failed, we're in real trouble. Get rid}
        //{of the critter altogether.}
        if (it.M == null)
            return null;

        //{Locate a good position to attach IT to.}
        if (CList == null)
        {
            //{the list is currently empty. Attach it as the first model.}
            CList = it;
        }
        else
        {
            //{The list has stuff in it. Attach IT to the end.}
            LastCritter(CList).next = it;
        }

	    //{Return the address of the new critter, just in case}
	    //{the calling procedure wants to mess around with it.}
	    return it;
    }

    public static void RemoveCritter(Critter CP, ref Critter CList, texmaps.GameBoard gb)
    {
        //{Remove critter C from the critter list, also disposing of its}
        //{associated model, and updating screen display if needed.}
        Critter B = CList;
        Critter A = null;

        while (B != CP && B != null)
        {
            A = B;
            B = B.next;
        }

        if (B == null)
        {
            //{Major FUBAR. The critter we were trying to remove can't}
            //{be found in the list.}
            Crt.Write("ERROR- RemoveCritter asked to remove a critter that dont exist.");
            do { rpgtext.RPGKey(); } while (true);
        }
        else if (A == null)
        {
            //{There's no critter before the one we want to remove,}
            //{i.e. it's the first one in the list.}
            CList = B.next;
            ZonkCritter(B, gb);
        }
        else
        {
            //{We found the critter we want to delete and have a critter}
            //{standing before it in line. Go to work.}
            A.next = B.next;
            ZonkCritter(B, gb);
        }
    }

    public static Critter LocateCritter(texmodel.Model MP, Critter CList)
    {
	    //{Search through the critters list and return a pointer to the}
	    //{critter whose model is at MP. Return null if no such critter can}
	    //{be found.}

	    //{Initialize Temp}
	    Critter temp = null;

        //{Loop through all of the models, looking for the right one.}
        while (CList != null)
        {
            if (CList.M == MP)
            {
                temp = CList;
            }

            CList = CList.next;
        }

        //{Return Temp}
        return temp;
    }

    public static int NumberOfCritters(Critter C)
    {
        //{Scan through the list of critters and tell us how many}
        //{there are.}
        int N = 0;
        while (C != null)
        {
            C = C.next;
            N += 1;
        }

        return N;
    }

    public static int ScaleCritterDamage(Critter C, int DMG, int E)
    {
        //{Scale the damage being done based on the critter's resistances.}

        //{First determine the critter's succeptability to this attack}
        //{type. See what kind of a critter this is.}
        int M = 0;
        for (int t = 0; t < NumCT; t++)
        {
            if (MonMan[C.crit - 1].CT.Contains(CTMan[t]))
            {
                M += CTResist[t, E];
            }
        }

        if (M < 0)
        {
            DMG = (DMG * (2 + Math.Abs(M))) / 2;
        }
        else if (M > 0)
        {
            if (M < 4)
                DMG = (DMG * (4 - M)) / 4;
            else if (M > 9)
                DMG = -DMG;
            else
                DMG = 0;
        }

        return DMG;
    }

    public static bool SetCritterStatus(Critter C, int S, int V)
    {
        //{Attempt to give status S to critter C. Return TRUE if the}
        //{critter now has this status, FALSE if it failed.}

        //{Check the critter's type string, to see if it gets an immunity}
        //{to this status change.}
        bool it = true;
        for (int t = 0; t < NumCT; ++t)
        {
            if (MonMan[C.crit - 1].CT.Contains(CTMan[t]))
            {
                it &= CTAvoid[t, Math.Abs(S) - 1];
            }
        }

        if (it)
        {
            plotbase.AddNAtt(ref C.SF, statusfx.NAG_StatusChange, S, V);
        }

        return it;
    }

    public static void WriteCritterList(Critter C, StreamWriter f)
    {
	    //{Write the list of critters C to file F.}

	    //{First, do an informative message that may help in debugging.}
	    f.WriteLine("*** The Critters List ***");

        while (C != null)
        {
            f.WriteLine(C.crit);

            //{Record the position of the critters.}
            f.WriteLine(C.M.x);
            f.WriteLine(C.M.y);

            f.WriteLine(C.HP);
            f.WriteLine(C.AIType);

            dcitems.WriteItemList(C.Eqp, f);
            plotbase.WriteNAtt(C.SF, f);

            //{Record whether or not the critter has been spotted.}
            if (C.Spotted)
                f.WriteLine("T");
            else
                f.WriteLine("F");

            //{Record the whereabouts of the critter's target, if any.}
            if (C.Target == null)
            {
                f.WriteLine(-1);
                f.WriteLine(-1);
            }
            else
            {
                f.WriteLine(C.Target.x);
                f.WriteLine(C.Target.y);
            }

            C = C.next;
        }

	    //{Send a -1 to indicate the end of the critter list.}
	    f.WriteLine(-1);
    }

    public static Critter ReadCritterList(StreamReader f, texmaps.GameBoard gb, int SFV)
    {
	    //{Load a bunch of critters from file F and stick them onto}
	    //{the Game Board. Return a pointer to the new list.}
	    Critter CList = null;

        //{Get rid of the info line.}
        f.ReadLine();

        int N = -1;
        do
        {
            N = int.Parse(f.ReadLine());

            if (N != -1)
            {
                int X = int.Parse(f.ReadLine());
                int Y = int.Parse(f.ReadLine());

                //{Allocate memory for the critter now.}
                Critter C = AddCritter(ref CList, gb, N, X, Y);

                C.HP = int.Parse(f.ReadLine());
                C.AIType = int.Parse(f.ReadLine());

                C.Eqp = dcitems.ReadItemList(f);

                C.SF = plotbase.ReadNAtt(f);

                //{Determine whether or not the critter has been spotted.}
                C.Spotted = f.ReadLine() == "T";

                C.TX = int.Parse(f.ReadLine());
                C.TY = int.Parse(f.ReadLine());
            }
        }
        while (N != -1);

	    //{Assign all the correct targets for all our critters.}
	    Critter Cit = CList;
        while (Cit != null)
        {
            Cit.Target = texmodel.FindModelXY(gb.mlist, Cit.TX, Cit.TY);
            Cit = Cit.next;
        }

        return CList;
    }

    static void ZonkCritter(Critter C, texmaps.GameBoard gb)
    {
        //{Delete the critter record at C, along with its associated model.}

        //{Get rid of the model}
        texmaps.GRemoveModel(C.M, gb);
    }
}