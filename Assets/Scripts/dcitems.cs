using System;
using System.IO;

public class dcitems
{
    //{This unit handles items. Duh.}
    //{It also handles items on the map display. Big problem.}

    public class DCItem
    {
        public DCItem()
        {
            this.ikind = 0;
            this.icode = 0;
            this.state = 0;
            this.charge = -1;
            this.ID = true;
            this.next = null;
        }

        public int ikind;   //{Type of item.}
        public int icode;   //{Specific kind.}
        public int state;   //{The state of the item.}
        public int charge;  //{This field tells how many shots/uses a device has left.}
        public bool ID;     //{Whether or not the item has been identified.}
        public DCItem next;
    }

    public class IGrid
    {
        public DCItem[,] g = new DCItem[texmodel.XMax, texmodel.YMax];
    }

    //{Data on a missile weapon.}
    public class MissileDesc
    {
        public MissileDesc(string name, int caliber, int magazine, int ACC, int DMG, int RNG, int CCM, string ATT, string Desc)
        {
            this.name = name;
            this.caliber = caliber;
            this.magazine = magazine;
            this.ACC = ACC;
            this.DMG = DMG;
            this.RNG = RNG;
            this.CCM = CCM;
            this.ATT = ATT;
            this.Desc = Desc;
        }

        public string name;
        public int caliber;  //{What kind of ammo does it take?}
        public int magazine; //{How many shots can it hold?}
        public int ACC;      //{Weapon Accuracy}
        public int DMG;      //{Damage Step}
        public int RNG;      //{Range Band}
        public int CCM;      //{Close Combat Modifier}
        public string ATT;   //{Attack Attributes.}
        public string Desc;
    }

    //{Data on a melee weapon.}
    public class WeaponDesc
    {
        public WeaponDesc(string name, int stat, int ACC, int DMG, string ATT, string Desc)
        {
            this.name = name;
            this.stat = stat;
            this.ACC = ACC;
            this.DMG = DMG;
            this.ATT = ATT;
            this.Desc = Desc;
        }

        public string name;
        public int stat;    //{What stat does it use to target?}
        public int ACC;     //{Weapon Accuracy}
        public int DMG;     //{Damage Step}
        public string ATT;  //{Attack Attributes.}
        public string Desc;
    }

    //{Data on body armor.}
    public class ArmorDesc
    {
        public ArmorDesc(string name, int PV, bool atmSealed, string Desc)
        {
            this.name = name;
            this.PV = PV;
            this.atmSealed = atmSealed;
            this.Desc = Desc;
        }

        public string name;
        public int PV;         //{Protection Value}
        public bool atmSealed; //{ Is it atmospherically sealed? }
        public string Desc;
    }

    public class FoodDesc
    {
        public FoodDesc(string name, int fk, int carbs, spells.SpellDesc fx, string Desc)
        {
            this.name = name;
            this.fx = fx;
            this.carbs = carbs;
            this.fx = fx;
            this.Desc = Desc;
        }

        public string name;
        public int fk;      //{Food kind. Rations, Pills, etc}
        public int carbs;
        public spells.SpellDesc fx;
        public string Desc;
    }

    public class SpecAmmoDesc
    {
        public SpecAmmoDesc(string name, int DMG, int ACC, string ATT, string Desc)
        {
            this.name = name;
            this.DMG = DMG;
            this.ACC = ACC;
            this.ATT = ATT;
            this.Desc = Desc;
        }

        public string name;
        public int DMG; //{Damage & Accuracy modifiers}
        public int ACC;
        public string ATT; //{Attack Attributes. Come before gun attributes, so suprecede them in the event of a conflict.}
        public string Desc;
    }

    public class GrenadeDesc
    {
        public GrenadeDesc(string name, bool sayGrenade, int DMG, int RNG, string ATT, string Desc)
        {
            this.name = name;
            this.sayGrenade = sayGrenade;
            this.DMG = DMG;
            this.RNG = RNG;
            this.ATT = ATT;
            this.Desc = Desc;
        }

        public string name;
        public bool sayGrenade; //{ Say "Grenade" in the name? }
        public int DMG;
        public int RNG;    //{This does not affect the actual range the grenade can be thrown, but does affect its accuracy.}
        public string ATT;
        public string Desc;
    }

    public class MiscDesc
    {
        public MiscDesc(string name, string Desc)
        {
            this.name = name;
            this.Desc = Desc;
        }

        public string name;
        public string Desc;
    }

    //{These constants define the different types of item.}
    //{ - positive KINDs can be equipped.}
    //{ - negative KINDs can't be equipped.}
    public const int IKIND_Electronics = -6;
    public const int IKIND_Book = -5;
    public const int IKIND_Grenade = -4;
    public const int IKIND_KeyItem = -3;
    public const int IKIND_Ammo = -2;
    public const int IKIND_Food = -1;
    public const int IKIND_Gun = 1;
    public const int IKIND_Wep = 2;
    public const int IKIND_Cap = 3;
    public const int IKIND_Armor = 4;
    public const int IKIND_Glove = 5;
    public const int IKIND_Shoe = 6;

    public const int NumGuns = 15;
    public const int NumCal = 6;
    public const int CAL_5mm = 0;
    public const int CAL_8mm = 1;
    public const int CAL_12mm = 2;
    public const int CAL_25mm = 3;
    public const int CAL_Energy = 4;
    public const int CAL_Napalm = 5;

    public static MissileDesc[] CGuns = new MissileDesc[NumGuns]
    {
        /*              name                 caliber         magazine  ACC   DMG  RNG    CCM   ATT */
        new MissileDesc("Sidekick Pistol",   CAL_5mm,        12,       2,    4,   3,     0,    "",
            "A light pistol, favored by merchants and navigators. It is very accurate at short range, but not too powerful."),
        new MissileDesc("Long Rifle",        CAL_5mm,        30,       1,    5,   12,   -1,    "",
            "This rifle has excellent range and accuracy, but lacks power. It is primarily used for hunting."),
        new MissileDesc("Assault Rifle",     CAL_8mm,        24,       0,    8,   6,    -1,    "",
            "This is the main combat weapon of the Republic Marine Corps. It is designed for short range firefights."),
        new MissileDesc("Headhunter Pistol", CAL_8mm,        10,       0,    9,   3,     0,    "",
            "A heavy pistol. This is the favorite sidearm of most security forces in the republic."),
        new MissileDesc("Khan Heavy Pistol", CAL_12mm,       5,       -1,    12,  3,     0,    "",
            "A sidearm capable of taking down a heavily armored target in a single shot. Designed as a backup support weapon for the marines."),
        new MissileDesc("Sonic Pistol",      CAL_Energy,     100,      0,    5,   2,     0,    "",
            "This weapon fires a concentrated burst of sound."),
        new MissileDesc("SK-3 Burner",       CAL_Napalm,     20,       1,    5,   8,    -2,    spells.AA_LineAttack + spells.AA_ElemFire,
            "This flamethrower is often used to clear vacuum worms and fungal infestations from the surface of spaceships."),
        new MissileDesc("Flame Pistol",      CAL_Napalm,     50,       2,    3,   4,     0,    spells.AA_LineAttack + spells.AA_ElemFire,
            "A light flame weapon."),
        new MissileDesc("Shotgun",           CAL_12mm,       16,       1,    6,   5,    -1,    "",
            "Primitive but effective. Shotguns are durable, can use a wide variety of ammunition types, and cause enough damage to seriously threaten an armored foe."),
        new MissileDesc("IDF Sonic Rifle",   CAL_Energy,     20,       0,    9,   5,    -1,    "",
            "The sound waves produced by this weapon are comprable in effect to a bullet."),
        new MissileDesc("ICE Rifle",         CAL_8mm,        28,       0,    10,  6,    -1,    spells.AA_ArmorPiercing + spells.AA_ElemCold,
            "The bullets for this rifle are kept in a supercooled state. This improves the efficeincy of the magnetic accelerator, and allows the weapon to shatter armor."),
        new MissileDesc("ICE Carbine",       CAL_8mm,        20,       0,    7,   4,     0,    spells.AA_ArmorPiercing + spells.AA_ElemCold,
            "This is a smaller version of the ICE Rifle. The bullets for this gun are kept in a supercooled state. This improves the efficeincy of the magnetic accelerator."),
        new MissileDesc("Cone Rifle",        CAL_25mm,       9,        0,    15,  8,    -3,    "",
            "A large, shoulder mounted magnetic accelerator which fires heavy 25mm shells."),
        new MissileDesc("Sawn-Off Shotgun",  CAL_12mm,       16,       1,    6,   3,     1,    "",
            "Primitive but effective. This shotgun has been modified for close assault."),
        new MissileDesc("Thumper",           CAL_25mm,       20,      -1,    13,  4,    -1,    "",
            "A high caliber heavy damage support weapon designed for tunnel fighting."),
    };

    public const int NumWep = 16;

    public static WeaponDesc[] CWep = new WeaponDesc[]
    {
        new WeaponDesc(
            name: "Knife",
            stat: 4,
            ACC: 1, DMG: 1,
            ATT: "",
            Desc: "Though intended as a tool, a knife can make a passable weapon for self defense."),
        new WeaponDesc("Staff",
            stat: 4,
            ACC: 0, DMG: 2,
            ATT: "",
            Desc: "An intricately carved wooden staff."),
        new WeaponDesc("Cutlass",
            stat: 4,
            ACC: 1, DMG: 4,
            ATT: "",
            Desc: "A short sword with a slightly curved blade."),
        new WeaponDesc("Boarding Axe",
            stat: 1,
            ACC: -1, DMG: 4,
            ATT: spells.AA_ArmorPiercing,
            Desc: "A collapsable plasteel axe intended for use during ship to ship boarding actions."),
        new WeaponDesc("Katana",
            stat: 4,
            ACC: 2, DMG: 8,
            ATT: spells.AA_ElemHoly,
            Desc: "An old and treasured family heirloom, created by a master weaponsmith in days long ago."),

        new WeaponDesc("Survival Knife",
            stat: 4,
            ACC: 1, DMG: 2,
            ATT: "",
            Desc: "A large serrated knife."),
        new WeaponDesc("Vibro Maul",
            stat: 1,
            ACC: -1, DMG: 14,
            ATT: spells.AA_StatusPar + spells.AA_HitRoll + "03",
            Desc: "The head of this baton is surrounded by a disruptive sonic field."),
        new WeaponDesc("Steel Pipe",
            stat: 1,
            ACC: -1, DMG: 3,
            ATT: "",
            Desc: "It's just a length of steel pipe, but it might make a decent club if you should need to defend yourself."),
        new WeaponDesc("Silver Dagger",
            stat: 4,
            ACC: 1, DMG: 3,
            ATT: spells.AA_ElemHoly,
            Desc: "An ornamental dagger, crafted in silver and covered with jewels."),
        new WeaponDesc("Arc Mattock",
            stat: 1,
            ACC: 0, DMG: 6,
            ATT: spells.AA_ElemLit,
            Desc: "This is a massive hammer with a built in electrical discharger. It's used by station maintenance for repair work."),

        new WeaponDesc("Chainsaw",
            stat: 1,
            ACC: -2, DMG: 12,
            ATT: "",
            Desc: "An industrial cutting tool. It can probably be used for cutting other things."),
        new WeaponDesc("Chainsword",
            stat: 4,
            ACC: 0, DMG: 9,
            ATT: "",
            Desc: "This sword features a serrated microfusion powered cutting chain."),
        new WeaponDesc("Ice Pick",
            stat: 4,
            ACC: 0, DMG: 2,
            ATT: spells.AA_Slaying + "07",	//{ Slays cold-based creatures. }
            Desc: "A metal spike used for crushing ice." ),
        new WeaponDesc("Letter Opener",
            stat: 4,
            ACC: 2, DMG: 1,
            ATT: "",
            Desc: "A brass knife made for cutting open envelopes." ),
        new WeaponDesc("Spanner",
            stat: 1,
            ACC: 0, DMG: 2,
            ATT: "",
            Desc: "A steel wrench."),

        new WeaponDesc("Taltuo Dire Sword",
            stat: 4,
            ACC: 0, DMG: 16,
            ATT: spells.AA_ElemHoly,
            Desc: "This massive blue greatsword of unknown composition was recovered from an ancient grave site on the moon of Taltuo."),
    };


    public const int NumFood = 39;
    public const int NumFSpell = 12;

    public static spells.SpellDesc[] FSpellMan = new spells.SpellDesc[NumFSpell]
    {
        new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Healing,
            step: 12, p1: 0, p2: 0, cost: 0,
            c: Crt.Color.LightGreen, ATT: "", Desc: ""),
        new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: statusfx.SEF_Regeneration, p1: 9, p2: 0, cost: 0,
            c: Crt.Color.LightGreen, ATT: "", Desc: ""),
        new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: statusfx.SEF_VisionBonus, p1: 6, p2: 0, cost: 0,
            c: Crt.Color.Yellow, ATT: "", Desc: ""),
        new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: statusfx.SEF_Poison, p1: 5, p2: 0, cost: 0,
            c: Crt.Color.Yellow, ATT: "", Desc: ""),
        new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_CureStatus,
            step: statusfx.SEF_Poison, p1: 0, p2: 0, cost: 0,
            c: Crt.Color.Yellow, ATT: "", Desc: ""),

        new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: statusfx.SEF_Paralysis, p1: 3, p2: 0, cost: 0,
            c: Crt.Color.Magenta, ATT: "", Desc: ""),
        new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: statusfx.SEF_Sleep, p1: 5, p2: 0, cost: 0,
            c: Crt.Color.White, ATT: "", Desc: ""),
        new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: -6, p1: 36, p2: 0, cost: 0,
            c: Crt.Color.White, ATT: "", Desc: ""),
		//{ Effect 8 - Boost Dexterity }
		new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: statusfx.SEF_BoostBase + 4, p1: 10, p2: 0, cost: 0,
            c: Crt.Color.LightGreen, ATT: "", Desc: ""),
        new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: statusfx.SEF_SpeedBonus, p1: 7, p2: 0, cost: 0,
            c: Crt.Color.LightGreen, ATT: "", Desc: ""),

		//{ Effect 10 - Boost Strength }
		new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: statusfx.SEF_BoostBase + 1, p1: 25, p2: 0, cost: 0,
            c: Crt.Color.LightGreen, ATT: "", Desc: ""),
		//{ Effect 11 - Boost Speed }
		new spells.SpellDesc( "", cdesc: "",
            eff: spells.EFF_Residual,
            step: statusfx.SEF_BoostBase + 3, p1: 9, p2: 0, cost: 0,
            c: Crt.Color.LightGreen, ATT: "", Desc: ""),
    };

    public static string[] FKName = new string[2]
    {
        "Rations","Pill"
    };

    public static FoodDesc[] CFood = new FoodDesc[NumFood]
    {
        new FoodDesc("NutriSnax", fk: 0,
            carbs: 12, fx: null,
            Desc: "A bag of NutriSnax chips. According to the label, this snack is supposed to provide a balanced diet for most humanoid life forms."),
        new FoodDesc("Hard Biscuit", fk: 0,
            carbs: 5, fx: null,
            Desc: "Often used as survival rations on spaceships. They last forever, provide enough nutrition to keep someone alive, and don't take up too much space or weight."),
        new FoodDesc("Meat Jerky", fk: 0,
            carbs: 5, fx: null,
            Desc: "Dessicated meat from some kind of animal, or a synthetic approximation thereof. Often kept as emergency rations since it will remain edible indefinitely."),
        new FoodDesc("Sausage", fk: 0,
            carbs: 8, fx: null,
            Desc: "A sausage, supposedly created from the meat of some kind of animal."),
        new FoodDesc("Trail Mix", fk: 0,
            carbs: 10, fx: null,
            Desc: "A mixture of dried fruits, nuts, and other healthy foods in a convenient serving-size packet."),

        new FoodDesc("Banana", fk: 0,
            carbs: 8, fx: null,
            Desc: "Fresh fruit is a healthy and delicious snack."),
        new FoodDesc("Canned Ravioli", fk: 0,
            carbs: 22, fx: null,
            Desc: "A self heating can of ravioli."),
        new FoodDesc("Canned Cream Soup", fk: 0,
            carbs: 19, fx: null,
            Desc: "A self heating can of cream soup."),
        new FoodDesc("Meat and Starchlog", fk: 1,
            carbs: 50, fx: null,
            Desc: "These rations represent the most palatable form of portable nutrition that the modern military has to offer. Expected shelf life is five thousand years."),
        new FoodDesc("Irish Stew", fk: 1,
            carbs: 50, fx: null,
            Desc: "These rations represent the most palatable form of portable nutrition that the modern military has to offer. Expected shelf life is five thousand years."),

        new FoodDesc("Lentilsoy Steak", fk: 1,
            carbs: 50, fx: null,
            Desc: "These rations represent the most palatable form of portable nutrition that the modern military has to offer. Expected shelf life is five thousand years."),
        new FoodDesc("Cheesy Noodles", fk: 1,
            carbs: 50, fx: null,
            Desc: "These rations represent the most palatable form of portable nutrition that the modern military has to offer. Expected shelf life is five thousand years."),
        new FoodDesc("Lasagne", fk: 1,
            carbs: 50, fx: null,
            Desc: "These rations represent the most palatable form of portable nutrition that the modern military has to offer. Expected shelf life is five thousand years."),
        new FoodDesc("Curry Rice", fk: 1,
            carbs: 50, fx: null,
            Desc: "These rations represent the most palatable form of portable nutrition that the modern military has to offer. Expected shelf life is five thousand years."),
        new FoodDesc("Bokum Rice", fk: 1,
            carbs: 50, fx: null,
            Desc: "These rations represent the most palatable form of portable nutrition that the modern military has to offer. Expected shelf life is five thousand years."),

        new FoodDesc("Crunchy Critters", fk: 0,
            carbs: 15, fx: null,
            Desc: "The bag appears to be full of some kind of arthropod, dipped in batter and deep fried."),
        new FoodDesc("Tubelunch Roast Chicken", fk: 0,
            carbs: 35, fx: null,
            Desc: "A complete meal, in tube form. Portable nutrition for today's space traveller."),
        new FoodDesc("Tubelunch Salisbury Steak", fk: 0,
            carbs: 35, fx: null,
            Desc: "A complete meal, in tube form. Portable nutrition for today's space traveller."),
        new FoodDesc("Tubelunch Yams", fk: 0,
            carbs: 42, fx: null,
            Desc: "A complete meal, in tube form. Portable nutrition for today's space traveller. Yams are the best product in the Tubelunch range."),
        new FoodDesc("Dead Rat", fk: 0,
            carbs: 10, fx: null,
            Desc: "A dead rodent."),

        new FoodDesc("Meal Wafer", fk: 0,
            carbs: 30, fx: null,
            Desc: "A light, octagonal wafer which contains all the nutrients required for life in an easily digestable form."),
        new FoodDesc("Apple", fk: 0,
            carbs: 8, fx: null,
            Desc: "Fresh fruit is a healthy and delicious snack."),
        new FoodDesc("Orange", fk: 0,
            carbs: 8, fx: null,
            Desc: "Fresh fruit is a healthy and delicious snack."),
        new FoodDesc("Dietary Supplement", fk: 2,
            carbs: 20, fx: null,
            Desc: "This pill contains all the nutrients required for human life."),
        new FoodDesc("Trauma Fix", fk: 2,
            carbs: 1, fx: FSpellMan[0],
            Desc: "This drug is a multiaction stabilizer for physical injury. Use it if you get hurt."),

        new FoodDesc("Speed Heal", fk: 2,
            carbs: 1, fx: FSpellMan[1],
            Desc: "This drug works by boosting a patient's natural regenerative processes. Use it if you expect to get hurt."),
        new FoodDesc("Retinal Booster", fk: 2,
            carbs: 1, fx: FSpellMan[2],
            Desc: "These pills can increase a person's field of vision for a short time."),
        new FoodDesc("Placebo", fk: 2,
            carbs: 3, fx: null,
            Desc: "Sugar pills are often used in double blind scientific research. Placebos look and taste like real medicene, but have no effect upon the body."),
        new FoodDesc("Rat Poison", fk: 2,
            carbs: 1, fx: FSpellMan[3],
            Desc: "A tablet of cyanide. Better be careful with this."),
        new FoodDesc("Antidote", fk: 2,
            carbs: 1, fx: FSpellMan[4],
            Desc: "Broad spectrum antitoxin. This drug is an effective remedy for most injested or injected poisons."),

        new FoodDesc("Muscle Relaxant", fk: 2,
            carbs: 1, fx: FSpellMan[5],
            Desc: "This is a very powerful medication and should only be taken if prescribed by a doctor."),
        new FoodDesc("Tranquilizer", fk: 2,
            carbs: 1, fx: FSpellMan[6],
            Desc: "A useful drug for people suffering from insomnia. May be habit forming."),
        new FoodDesc("Anti-Nauseant", fk: 2,
            carbs: 1, fx: FSpellMan[7],
            Desc: "This medicene helps to prevent the onset of space-sickness. May cause drowsiness. Effects last six hours."),
        new FoodDesc("Spoiled", fk: 1,
            carbs: 15, fx: FSpellMan[3],
            Desc: "A label on the side claims that these rations should remain edible for five thousand solar years. The thriving colony of green slime growing inside argues otherwise."),
        new FoodDesc("Rancid Sandwich", fk: 0,
            carbs: 10, fx: FSpellMan[3],
            Desc: "This sub sandwich has turned black and is starting to grow hair. You probably shouldn't eat it."),

        new FoodDesc("Combat Spur", fk: 2,
            carbs: 1, fx: FSpellMan[8],
            Desc: "This drug was initially developed for the military's \"Heavy Damage\" program. It temporarily boosts a persons motor reflexes and muscle control."),
        new FoodDesc("Zeno Zip", fk: 2,
            carbs: 1, fx: FSpellMan[9],
            Desc: "Using all natural ingredients, this pill is supposed to improve a persons movement speed by up to 50%."),
        new FoodDesc("Onslaught", fk: 2,
            carbs: 1, fx: FSpellMan[10],
            Desc: "This combat drug allows a person to perform greater feats of strength than would otherwise be possible. It is illegal on most worlds."),
        new FoodDesc("React", fk: 2,
            carbs: 1, fx: FSpellMan[11],
            Desc: "This drug temporarily increases the speed of a persons nervous system."),
    };

    public const int NumCap = 7;
    public static ArmorDesc[] CCap = new ArmorDesc[NumCap]
    {
        new ArmorDesc("Field Cap",
            PV: 1, atmSealed: false,
            Desc: "A visored cap with shock resistant gel pads."),
        new ArmorDesc("Vac Helmet",
            PV: 1, atmSealed: true,
            Desc: "A fully enclosed helmet with an incorporated air scrubber."),
        new ArmorDesc("Combat Helmet",
            PV: 2, atmSealed: false,
            Desc: "A heavy blast helmet."),
        new ArmorDesc("Iron Mask",
            PV: 3, atmSealed: false,
            Desc: "An iron mask bearing a grim visage."),
        new ArmorDesc("Ancestral Helm",
            PV: 2, atmSealed: false,
            Desc: "An ancient master crafted war helm."),

        new ArmorDesc("Starvisor Pilot Helm",
            PV: 2, atmSealed: true,
            Desc: "This full featured space helmet features an LCD monitor which can interface with most starship computers, as well as an emergency air supply to guard against cockpit rupture." ),
        new ArmorDesc("Pioneer Space Helmet",
            PV: 3, atmSealed: true,
            Desc: "A heavy work helmet designed for prolonged exposure to vacuum and space dust."),
    };

    public const int NumArmor = 11;
    public static ArmorDesc[] CArmor = new ArmorDesc[NumArmor]
    {
        new ArmorDesc("Clothes",
            PV: 1, atmSealed: false,
            Desc: "A shirt and some pants."),
        new ArmorDesc( "Vac Suit",
            PV: 2, atmSealed: true,
            Desc: "A light space suit. It can protect its wearer against hard vacuum."),
        new ArmorDesc("Flak Jacket",
            PV: 3, atmSealed: false,
            Desc: "A light armored jacket."),
        new ArmorDesc( "Robe",
            PV: 1, atmSealed: false,
            Desc: "A long robe."),
        new ArmorDesc("Ancestral Armor",
            PV: 3, atmSealed: false,
            Desc: "An ancient set of ornate armor."),

        new ArmorDesc("Dress",
            PV: 1, atmSealed: false,
            Desc: "A stylish dress."),
        new ArmorDesc( "Acroweave Kimono",
            PV: 5, atmSealed: false,
            Desc: "A stylish garment woven from the latest armor-class polymer fabrics."),
        new ArmorDesc( "Tact Hardsuit",
            PV: 4, atmSealed: true,
            Desc: "This suit is constructed from a tough mesh fabric, and is covered in rigid armor plates."),
        new ArmorDesc( "Acroweave Robes",
            PV: 6, atmSealed: false,
            Desc: "Sturdy robes woven from the latest armor-class polymer fabrics."),
        new ArmorDesc( "Acroweave Suit",
            PV: 4, atmSealed: false,
            Desc: "A stylish garment woven from the latest armor-class polymer fabrics. Suitable for both work and play."),

        new ArmorDesc( "Pioneer Spacesuit",
            PV: 4,  atmSealed: true,
            Desc: "An industrial heavy space suit. The Pioneer can withstand prolonged exposure to vacuum and micrometeor abrasion."),
    };

    public const int NumGlove = 4;
    public static ArmorDesc[] CGlove = new ArmorDesc[NumGlove]
    {
        new ArmorDesc("Vac Gloves",
            PV: 1, atmSealed: true,
            Desc: "A pair of thick gloves with sealing cuffs. They are designed to protect a person from vacuum exposure."),
        new ArmorDesc("Combat Gauntlets",
            PV: 2, atmSealed: false,
            Desc: "A pair of heavy, armored gloves."),
        new ArmorDesc("Sanctified Fist",
            PV: 3, atmSealed: true,
            Desc: "An ornate ceremonial gauntlet."),
        new ArmorDesc("Pioneer Space Gloves",
            PV: 2, atmSealed: true,
            Desc: "A pair of thick gloves with sealing cuffs and integrated tool ports.")
    };

    public const int NumShoe = 6;
    public static ArmorDesc[] CShoe = new ArmorDesc[NumShoe]
    {
        new ArmorDesc("Steel-Toed Boots",
            PV: 1, atmSealed: false,
            Desc: "A pair of steel toed work boots."),
        new ArmorDesc("Shoes",
            PV: 0, atmSealed: false,
            Desc: "A nice pair of simuleather shoes."),
        new ArmorDesc("Vac Boots",
            PV: 1, atmSealed: true,
            Desc: "A pair of heavy boots with magnetic soles. They are designed to connect to a vacuum suit."),
        new ArmorDesc("Sandals",
            PV: 0, atmSealed: false,
            Desc: "A simple pair of open topped sandals."),
        new ArmorDesc("Dragon Boots",
            PV: 2, atmSealed: false,
            Desc: "Large ornate spiked boots. The kind a rock star would probably want to be buried in."),

        new ArmorDesc("Pioneer Work Boots",
            PV: 2, atmSealed: true,
            Desc: "A pair of magnetic soled space boots. The Pioneer range of vac clothes is favored by most external repair technicians.")
    };

    //{Ammunition is handled differently from the other item types.}
    //{The ICode for an ammo item is in two parts. ICode mod 100}
    //{gives the caliber of the bullet; this tells what kind of}
    //{gun it will fit. ICode div 100 gives the special attribute}
    //{of the ammo.}
    public static string[] AmmoName = new string[NumCal]
    {
        "5mm Bullet","8mm Bullet","12-gauge Shell","25mm Shell",
        "Energy Cell","Fuel Cannister"
    };

    public const int NumSpecAmmo = 16;
    public static SpecAmmoDesc[] CSpecAmmo = new SpecAmmoDesc[NumSpecAmmo]
    {
        new SpecAmmoDesc( "Normal. If you can read this, there must be a bug.",
            DMG: 0, ACC: 0,
            ATT: "",
            Desc: "Ammunition." ),
        new SpecAmmoDesc( "Scatter",
            DMG: -1, ACC: 1,
            ATT: spells.AA_LineAttack,
            Desc: "Wide dispersal fragmentation ammunition."),
        new SpecAmmoDesc( "Hollowpoint",
            DMG: 0, ACC: 0,
            ATT: spells.AA_SlayAlive + spells.AA_ArmorDoubling,
            Desc: "Antipersonnel hollowpoint ammunition."),
        new SpecAmmoDesc( "Slick",
            DMG: 0, ACC: 0,
            ATT: spells.AA_ArmorPiercing,
            Desc: "Lubricated coating armor piercing ammunition."),
        new SpecAmmoDesc( "SMRT",
            DMG: 0, ACC: 7,
            ATT: "",
            Desc: "Sensor Module Remote Targeting ammunition."),
        new SpecAmmoDesc( "Tranq Dart",
            DMG: -10, ACC: 0,
            ATT: spells.AA_StatusSleep+spells.AA_Value+"30"+spells.AA_HitRoll+"25",
            Desc: "Darts containing a powerful sedative."),

        new SpecAmmoDesc( "Tesla",
            DMG: 0, ACC: 0,
            ATT: spells.AA_ElemLit,
            Desc: "Electrical discharge anti-mech ammunition."),
        new SpecAmmoDesc( "Scour",
            DMG: 3, ACC: 0,
            ATT: spells.AA_ElemAcid + spells.AA_ArmorPiercing,
            Desc: "Ammunition containing a highly corrosive molecular solvent."),
        new SpecAmmoDesc( "Flechette",
            DMG: -1, ACC: 0,
            ATT: spells.AA_LineAttack,
            Desc: "Scatter flechette ammunition."),
        new SpecAmmoDesc( "Incendiary",
            DMG: 1, ACC: 0,
            ATT: spells.AA_ElemFire,
            Desc: "Exo-Phosphorous based incendiary ammunition."),
        new SpecAmmoDesc( "Practice",
            DMG: -3, ACC: 0,
            ATT: spells.AA_ArmorDoubling,
            Desc: "Cheap bullets made for firing practice. They won't do much damage against a real target."),

        new SpecAmmoDesc( "Blank",
            DMG: -25, ACC: -5,
            ATT: spells.AA_ArmorDoubling,
            Desc: "Empty rounds made for ceremonial salutes."),
        new SpecAmmoDesc( "Rubber",
            DMG: -7, ACC: 1,
            ATT: spells.AA_ArmorDoubling + spells.AA_StatusPar + spells.AA_HitRoll + "01",
            Desc: "Rubber bullets designed for nonlethal crowd control."),
        new SpecAmmoDesc( "Explosive",
            DMG: 0, ACC: 0,
            ATT: spells.AA_BlastAttack + "01",
            Desc: "High explosive fragmentation shell rounds." ),
        new SpecAmmoDesc( "Cover",
            DMG: 0, ACC: 2,
            ATT: spells.AA_SmokeAttack + "01" + spells.AA_Value + "03" + spells.AA_Duration + "08",
            Desc: "Metallic smoke generating defensive rounds." ),
        new SpecAmmoDesc( "Ab-Zero",
            DMG: 1, ACC: 0,
            ATT: spells.AA_BlastAttack + "02" + spells.AA_StatusPar + spells.AA_HitRoll + "02" + spells.AA_Value + "03" + spells.AA_ElemCold,
            Desc: "Thermally implosive flash-freeze ammunition."),
    };

    public const int NumKeyItem = 5;
    public static MiscDesc[] KCat = new MiscDesc[NumKeyItem]
    {
        new MiscDesc("Pass Card",
            Desc: "A station ID card. The name \"Andros Guero\" is hand written on the back."),
        new MiscDesc("Skull",
            Desc: "The skull bone of a human being."),
        new MiscDesc("Urn",
            Desc: "An ornate funeral urn. There are ashes inside."),
        new MiscDesc("Shroud",
            Desc: "A burial cloth, now relieved of its occupant."),
        new MiscDesc("Cybernetic Heart",
            Desc: "A replacement heart. The rest of the body has apparently long since decayed away."),
    };

    public const int NumElectronics = 1;
    public static MiscDesc[] ElecCat = new MiscDesc[NumElectronics]
    {
        new MiscDesc("HandyMap Navigation Unit",
            Desc: "A combination sensor pack and PDA which can help keep track of where you've been."),
    };

    public const int NumBook = 1;
    public static MiscDesc[] CBook = new MiscDesc[NumBook]
    {
        new MiscDesc("Diary",
            Desc: "An old fashioned pen-and-paper diary."),
    };

    public const int NumGrn = 12;
    public static GrenadeDesc[] CGrn = new GrenadeDesc[NumGrn]
    {
        new GrenadeDesc("Frag",
            sayGrenade: true,
            DMG: 9, RNG: 3,
            ATT: spells.AA_BlastAttack + "01",
            Desc: "Fragmentation grenade. Scatters shrapnel over a wide area."),
        new GrenadeDesc("Shatter",
            sayGrenade: true,
            DMG: 16, RNG: 7,
            ATT: spells.AA_ArmorPiercing + spells.AA_BlastAttack + "00",
            Desc: "Implosive anti-armor grenade." ),
        new GrenadeDesc("Toxin",
            sayGrenade: true,
            DMG: 3, RNG: 2,
            ATT: spells.AA_BlastAttack + "02" + spells.AA_ElemAcid + spells.AA_StatusPsn + spells.AA_HitRoll + "21" + spells.AA_Value + "09",
            Desc: "Corrosive gas grenade."),
        new GrenadeDesc("Choke",
            sayGrenade: true,
            DMG: 6, RNG: 3,
            ATT: spells.AA_BlastAttack + "02" + spells.AA_StatusSleep + spells.AA_HitRoll + "03" + spells.AA_Value + "09",
            Desc: "Asphyxiating gas grenade."),
        new GrenadeDesc("Haywire",
            sayGrenade: true,
            DMG: 8, RNG: 3,
            ATT: spells.AA_BlastAttack + "01" + spells.AA_ArmorPiercing + spells.AA_ElemLit + spells.AA_SlayMech,
            Desc: "Electromagnetic pulse anti-mech grenade."),

        new GrenadeDesc("Smoke",
            sayGrenade: true,
            DMG: 0, RNG: 3,
            ATT: spells.AA_SmokeAttack + "02" + spells.AA_Value + "03" + spells.AA_Duration + "05",
            Desc: "Smokescreen grenade."),
        new GrenadeDesc("Forcewall",
            sayGrenade: true,
            DMG: 0, RNG: 3,
            ATT: spells.AA_SmokeAttack + "03" + spells.AA_Value + "00" + spells.AA_Duration + "10",
            Desc: "Force field generating tactical barrier grenade."),
        new GrenadeDesc("Holy Water",
            sayGrenade: false,
            DMG: 4, RNG: 6,
            ATT: spells.AA_BlastAttack + "00" + spells.AA_ArmorPiercing + spells.AA_ElemHoly + spells.AA_SlayUndead,
            Desc: "Glass decanter full of holy water."),
        new GrenadeDesc("Lost Hope",
            sayGrenade: true,
            DMG: 31, RNG: 2,
            ATT: spells.AA_BlastAttack + "12" + spells.AA_ElemFire,
            Desc: "Heavy matter, tactical nuclear blast grenade. Also known as a Suicide Stick."),
        new GrenadeDesc("Thermal",
            sayGrenade: true,
            DMG: 12, RNG: 3,
            ATT: spells.AA_BlastAttack + "02" + spells.AA_ElemFire,
            Desc: "High yield controlled range thermal grenade."),

        new GrenadeDesc("Flask of Acid",
            sayGrenade: false,
            DMG: 9, RNG: 2,
            ATT: spells.AA_BlastAttack + "01" + spells.AA_ElemAcid,
            Desc: "A beaker of molecular acid."),
        new GrenadeDesc("Molotov Cocktail",
            sayGrenade: false,
            DMG: 7, RNG: 2,
            ATT: spells.AA_BlastAttack + "01" + spells.AA_ElemFire,
            Desc: "A makeshift grenade, apparently constructed by the station residents to fend off some threat."),
    };

    public static DCItem AddDCItem(ref DCItem llist)
    {
        //{Add a new element to the end of LList.}
        DCItem it = new DCItem();

        //{Attach IT to the list.}
        if (llist == null)
        {
            llist = it;
        }
        else
        {
            LastItem(llist).next = it;
        }

	    //{Return a pointer to the new element.}
	    return it;
    }

    public static void DelinkDCItem(ref DCItem llist, DCItem i)
    {
        //{Take item I and remove it from the list IList.}

        //{Initialize B to the head of the list.}
        DCItem B = llist;

	    //{Initialize A to Nil.}
	    DCItem A = null;

        while (B != null && B != i)
        {
            A = B;
            B = B.next;
        }

        //{Now, check to see what's just happened.}
        if (B == null)
        {
            //{Oh dear. The item wasn't found.}
            Crt.Write("ERROR- DelinkItem asked to delink an item that doesn't exist!\n");
            do { rpgtext.RPGKey(); } while (true);
        }
        else if (A == null)
        {
            //{The item that we're delinking is first in the list.}
            llist = B.next;
            B.next = null;
        }
        else
        {
            //{The item we want to delink is B; A is right behind it.}
            A.next = B.next;
            B.next = null;
        }
    }

    public static bool Mergeable(DCItem i)
    {
        //{Return TRUE if the item is mergeable, FALSE if not.}
        return i.ikind == IKIND_Food || i.ikind == IKIND_Ammo || i.ikind == IKIND_Grenade;
    }

    public static void MergeDCItem(ref DCItem llist, DCItem mlist)
    {
        //{Add item mlist, and any siblings it may have, to list llist.}
        //{But wait! There's a catch! If the item is a quantity-type}
        //{item, and one or more of them are already in the list,}
        //{add the new quantity then dispose of I.}

        while (mlist != null)
        {
            DCItem i = mlist;
            DelinkDCItem(ref mlist, i);

            //{Check to see if our item is Mergeable.}
            if (Mergeable(i) && i.ID)
            {
                //{Error check- make sure that I is at least 1.}
                if (i.charge == -1)
                    i.charge = 1;

                //{Look for another item of the same type in the list.}
                DCItem a = llist;
                DCItem b = null;
                while (a != null)
                {
                    if (a.ikind == i.ikind && a.icode == i.icode)
                    {
                        b = a;
                    }
                    a = a.next;
                }
                if (b != null)
                {
                    //{Another item of the same type as I has}
                    //{been found. Merge this item into it.}
                    if (b.charge < 1)
                    {
                        b.charge = 1;
                    }
                    b.charge += i.charge;
                    i = null;
                }
            }

            if (i != null)
            {
                //{No merge was performed, add it to the end.}}
                if (llist == null)
                {
                    llist = i;
                }
                else
                {
                    LastItem(llist).next = i;
                }
            }
        }
    }

    public static int ConsumeDCItem(ref DCItem llist, DCItem i, int n)
    {
        //{The player wants to use N units of item I. If the units}
        //{are avaliable, remove them from the Charge field of the}
        //{item. If they're not avaliable, consume as many items as}
        //{possible. If the Charge field is reduced to 0 by consumption,}
        //{delete the item record. Return the actual number of items}
        //{used.}

        //{error check- make sure we're dealing with a consumable item!}
        if (!Mergeable(i))
            return 0;

        if (i.charge < 0)
            i.charge = 1;

        if (n > i.charge)
            n = i.charge;

        i.charge -= n;

        if (i.charge < 1)
            DelinkDCItem(ref llist, i);

        return n;
    }

    public static void PlaceDCItem(texmaps.GameBoard gb, IGrid IG, DCItem i, int x, int y)
    {
        //{Stick item I on the gameboard at location X,Y.}
        //{update the display as needed.}

        //{ Error check.}
        if (i == null)
            return;

        MergeDCItem(ref IG.g[x - 1, y - 1], i);
	    texmaps.SetOverImage(gb, x, y, ',', Crt.Color.LightBlue);
    }

    public static void RetrieveDCItem(texmaps.GameBoard gb, IGrid IG, DCItem i, int x, int y)
    {
        //{Remove item I from the board. Clear the graphics display}
        //{if needed.}
        //{BUGS: item I had better well be in the right spot...}

        DelinkDCItem(ref IG.g[x - 1, y - 1], i);
        if (IG.g[x - 1, y - 1] == null)
        {
            texmaps.ClearOverImage(gb, x, y);
        }
    }


    public static DCItem LocateItem(DCItem ilist, int n)
    {
        //{This function will find the Nth item in list IList.}
        //{If N is too big, it will return the last item in the list.}
        //{If N is too small, it will return the first.}

        if (n > 1)
        {
            for (int t = 2; t <= n; ++t)
            {
                if (ilist != null)
                {
                    ilist = ilist.next;
                }
            }
        }

        return ilist;
    }

    public static bool HasItem(DCItem ilist, int k, int c)
    {
	    //{Search through list IList for an item matching Kind}
	    //{and Code values K and C.}
	    bool it = false;
        while (ilist != null)
        {
            if (ilist.ikind == k && ilist.icode == c)
            {
                it = true;
            }
            ilist = ilist.next;
        }

        return it;
    }

    public static string ItemNameShort(DCItem i)
    {
        //{Provide the terse name for the item in question.}
        string it;

        if (i.ID)
        {
            switch (i.ikind)
            {
                case IKIND_Gun:
                    return CGuns[i.icode].name;
                case IKIND_Wep:
                    return CWep[i.icode].name;
                case IKIND_Cap:
                    return CCap[i.icode].name;
                case IKIND_Armor:
                    return CArmor[i.icode].name;
                case IKIND_Glove:
                    return CGlove[i.icode].name;
                case IKIND_Shoe:
                    return CShoe[i.icode].name;
                case IKIND_Food:
                    //{Food which belongs to one of the special groups gets}
                    //{a special addition to the start of its name.}
                    if (CFood[i.icode].fk != 0)
                    {
                        return FKName[CFood[i.icode].fk] + ": " + CFood[i.icode].name;
                    }
                    else
                    {
                        return CFood[i.icode].name;
                    }
                case IKIND_Ammo:
                    {
                        it = AmmoName[i.icode % 100];
                        if (i.icode / 100 != 0)
                        {
                            it += " (" + CSpecAmmo[i.icode / 100].name + ")";
                        }
                        return it;
                    }
                case IKIND_KeyItem:
                    return KCat[i.icode].name;
                case IKIND_Book:
                    return CBook[i.icode].name;
                case IKIND_Grenade:
                    {
                        it = CGrn[i.icode].name;
                        if (CGrn[i.icode].sayGrenade)
                        {
                            it = " Grenade";
                        }
                        return it;
                    }
                case IKIND_Electronics:
                    return ElecCat[i.icode].name;
                default:
                    return string.Format("kind: {0} /code:{1}", i.ikind, i.icode);
            }
        }
        else
        {
            //{ Provide generic names for unidentified items. }
            switch (i.ikind)
            {
                case IKIND_Gun:
                    return "?Gun";
                case IKIND_Wep:
                    return "?Weapon";
                case IKIND_Cap:
                    return "?Hat";
                case IKIND_Armor:
                    return "?Clothing";
                case IKIND_Glove:
                    return "?Gloves";
                case IKIND_Shoe:
                    return "?Shoes";
                case IKIND_Food:
                    {
                        //{Food which belongs to one of the special groups gets}
                        //{a special addition to the start of its name.}
                        if (CFood[i.icode].fk != 0)
                        {
                            it = "?" + FKName[CFood[i.icode].fk];
                        }
                        else
                        {
                            it = "?Food";
                        }
                        return it;
                    }
                case IKIND_Ammo:
                    return "?" + AmmoName[i.icode % 100];
                case IKIND_KeyItem:
                    return "?Item";
                case IKIND_Book:
                    return "?Book";
                case IKIND_Grenade:
                    {
                        if (CGrn[i.icode].sayGrenade)
                            return "?Grenade";
                        else
                            return "?Item";
                    }
                case IKIND_Electronics:
                    return "?Item";
                default:
                    return String.Format("?kind:{0}/code{1}", i.ikind, i.icode);
            }
        }
    }

    public static string ItemNameLong(DCItem i)
    {
        //{Provide the verbose item name for the item in question.}

        string it = ItemNameShort(i);

        if (Mergeable(i))
        {
            it += String.Format(" x{1}", it, i.charge);
        }
        else if (i.ikind == IKIND_Gun && i.ID)
        {
            if (i.charge != -1)
            {
                it += String.Format(" [{1}]", i.charge);
            }
            else
            {
                it += " [+]";
            }

            if (i.state > 0)
            {
                it += String.Format(" ({0})", CSpecAmmo[i.state].name);
            }
            else if (i.state < 0)
            {
                it += " (?)";
            }
        }

        return it;
    }

    public static string ItemDesc(DCItem i)
    {
        //{Provide the description for the item in question.}

        //{Error check}
        if (i == null)
            return string.Empty;

        if (!i.ID)
            return "Unknown item";

        switch (i.ikind)
        {
            case IKIND_Gun: return CGuns[i.icode].Desc;
            case IKIND_Wep: return CWep[i.icode].Desc;
            case IKIND_Cap: return CCap[i.icode].Desc;
            case IKIND_Armor: return CArmor[i.icode].Desc;
            case IKIND_Glove: return CGlove[i.icode].Desc;
            case IKIND_Shoe: return CShoe[i.icode].Desc;
            case IKIND_Food: return CFood[i.icode].Desc;
            case IKIND_Ammo: return CSpecAmmo[i.icode / 100].Desc;
            case IKIND_KeyItem: return KCat[i.icode].Desc;
            case IKIND_Book: return CBook[i.icode].Desc;
            case IKIND_Grenade: return CGrn[i.icode].Desc;
            case IKIND_Electronics: return ElecCat[i.icode].Desc;
        }

        return string.Empty;
    }

    public static void WriteItemList(DCItem llist, StreamWriter f)
    {
        //{Save the linked list of items I to the file F.}
        while (llist != null)
        {
            f.WriteLine(llist.icode);
            f.WriteLine(llist.ikind);
            f.WriteLine(llist.charge);
            f.WriteLine(llist.state);
            if (llist.ID)
                f.WriteLine('T');
            else
                f.WriteLine('F');

            llist = llist.next;
        }
        f.WriteLine(-1);
    }

    public static DCItem ReadItemList(StreamReader f)
    {
        //{Load a list of items saved by the above procedure from}
        //{the file F.}

        DCItem ilist = null;

        int icode = int.Parse(f.ReadLine());
        while (icode != -1)
        {
            if (icode != -1)
            {
                DCItem i = AddDCItem(ref ilist);
                i.icode = icode;

                i.ikind = int.Parse(f.ReadLine());
                i.charge = int.Parse(f.ReadLine());
                i.state = int.Parse(f.ReadLine());

                char C = char.Parse(f.ReadLine());
                if (C == 'T')
                    i.ID = true;
                else
                    i.ID = false;
            }

            icode = int.Parse(f.ReadLine());
        }

        return ilist;
    }

    public static void WriteIGrid(IGrid IG, StreamWriter f)
    {
        //{Save the Item Grid IG to the file F.}

        //{write the specials.}
        for (int x = 0; x < texmodel.XMax; ++x)
        {
            for (int y = 0; y < texmodel.YMax; ++y)
            {
                if (IG.g[x, y] != null)
                {
                    f.WriteLine(x + 1);
                    f.WriteLine(y + 1);
                    WriteItemList(IG.g[x, y], f);
                }
            }
        }

        //{write the sentinel.}
        f.WriteLine(0);
    }

    public static IGrid ReadIGrid(StreamReader f, texmaps.GameBoard gb)
    {
        //{Read the Item Grid IG from the file F.}
        IGrid IG = new IGrid();


        int x = int.Parse(f.ReadLine());
        while (x != 0)
        {
            int y = int.Parse(f.ReadLine());
            IG.g[x - 1, y - 1] = ReadItemList(f);
            texmaps.SetOverImage(gb, x, y, ',', Crt.Color.LightBlue);
        }

        return IG;
    }

    static DCItem LastItem(DCItem llist)
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
