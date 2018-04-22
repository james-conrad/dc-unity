using System;
using System.IO;

public class dcchars
{
    //{ This unit contains major character stuff for DeadCold.}
    //{ This includes the character generator, and functions}
    //{ to derive stuff like skill ratings, etc.}
    public const int NumJobs = 10;
    public static string[] JobName = new string[NumJobs]
    {
        "Marine","Astral Seer","Navigator","Hacker","Demon Hunter",
        "Explorer","Samurai","Bounty Hunter","Star Pirate","Zeomancer"
    };

    public static int[] JobHitDie = new int[NumJobs]
    {
        9,3,6,5,4, 6,8,4,7,3
    };

    public static int[] JobMojoDie = new int[NumJobs]
    {
        1,6,4,2,8, 3,4,3,2,6
    };

    public static int[] JobSchool = new int[NumJobs]
    {
        0, spells.SCHOOL_Astral,  spells.SCHOOL_Navigator, 0, spells.SCHOOL_DemonHunter,
        0, spells.SCHOOL_Samurai, 0,                       0, spells.SCHOOL_Zeomancy
    };

    public static string[] JobDesc = new string[NumJobs]
    {
        "A well trained killing machine. The Marine doesn't have many noncombat skills, but generally doesn't need them.",
        "The mind of an Astral Seer is naturally tuned to the vibration frequency of reality itself. They are powerful psykers, acting as advisors on spiritual matters and as the keepers of ancient traditions.",
        "Only special individuals with psychic awareness are able to guide spacecraft through transreal space. The Navigator has psi powers, plus a number of other skills which are needed in the depths of space.",
        "A computer security expert, and career criminal. Hackers are experts at finding things which other people wish would stay hidden.",
        "Member of an ancient order, sworn to eternal secrecy, the demon hunter protects this reality from those who would destroy it.",
        "Travelling the fringes of human space, searching for new worlds, the explorer leads a solitary existence. A wide variety of skills are needed for this job.",
        "An anacronistic figure, well schooled in the martial arts and mystic secrets of ancient Terra. Samurai scorn the use of guns, but none are their equal in close combat.",
        "Part lawman, part assassin, Bounty Hunters are paid to track down and eliminate other human beings. They are experts at sniping enemies from a distance.",
        "Cruising the spaceways, smuggling contraband or seeking ripe targets, pirates are the bane of interstellar trade. They are skilled combatants who rely more on speed and guile than on brute force.",
        "Trained in both math and mysticism, Zeomancers pursue their arcane craft with modern science. Study has freed their order from ancient superstition, allowing them greater control of natural forces than ever thought possible."
    };

    public const int NumStats = 8;

    public static int[,] JobStat = new int[NumJobs, NumStats]
    {
        { 11,11,  1,  1,  1,  1,  1,  1 }, //{Marine}
        { 1,  1,  1,  1,  1, 11, 11,  1 }, //{Astroseer}
        { 7,  7, 10, 10,  8, 15, 12,  7 }, //{Navigator}
        { 1,  1,  1,  1, 11,  1,  1, 11 }, //{Hacker}
        { 13,14, 13, 14, 13, 14, 14, 13 }, //{Demon Hunter}
        { 1,  1,  1,  1,  1,  1,  1,  1 }, //{Explorer}
        { 16, 1,  5, 15,  1,  1, 12,  5 }, //{Samurai}
        { 1,  1, 15, 12,  5, 15,  5,  9 }, //{Bounty Hunter}
        { 1,  1, 11, 11,  1,  1,  1,  1 }, //{Pirate}
        { 1,  1,  1,  1, 11,  1, 11,  1 }, //{Zeomancer}
    };

    public static string[] StatName = new string[NumStats]
    {
        "Strength",  "Endurance", "Speed",     "Dexterity",
        "Technical", "Awareness", "Willpower", "Luck",
    };

    public static string[] StatAbbrev = new string[NumStats]
    {
        "St","En","Sp","Dx","Tc","Aw","Wp","Lk"
    };

    //{Define the index numbers associated with each character stat.}
    public const int STAT_Strength = 0;
    public const int STAT_Toughness = 1;
    public const int STAT_Speed = 2;
    public const int STAT_Dexterity = 3;
    public const int STAT_Technical = 4;
    public const int STAT_Perception = 5;
    public const int STAT_Willpower = 6;
    public const int STAT_Luck = 7;

    public const int NumSkill = 15; //{The number of PC skills.}
    public static int[,] JobSkill = new int[NumJobs, NumSkill]
    {
        { 0, 0, 0, 0, 2, 2,-3, 0,-4,-2, 0, 0, 0, 0, 0},	//{Marine}
	    { 4, 2, 2, 0, 1,-2, 0, 1,-3, 2, 0, 0, 2, 5, 0},	//{Astral Seer}
	    { 0, 1, 1, 0, 0, 2,-1, 1,-3, 1, 0, 1, 1, 1, 0},	//{Navigator}
	    { 0, 5, 0, 0, 2, 1, 3, 2, 2,-1, 0, 2, 0, 6, 0},	//{Hacker}
	    { 1, 3, 9, 0, 1, 1,-1, 0,-1, 1, 3, 0, 2, 1, 0},	//{Demon Hunter}
	    { 1, 1, 1, 1, 2, 2,-1, 1,-1, 1, 0, 2, 0, 4, 0},	//{Explorer}
	    {-2, 0, 2, 0, 2,-3,-2, 0,-3, 1, 0, 0,-1, 0, 0},	//{Samurai}
	    { 0, 0, 0, 1, 2, 2,-2, 0,-4, 0, 0, 0, 0, 0, 0},	//{Bounty Hunter}
	    { 1, 0,-1,-1, 2, 2,-2, 0,-4,-1, 0,-1, 0, 0, 0},	//{Pirate}
	    { 2, 1, 2, 0,-3,-2, 1, 2,-2, 2, 1, 1, 2, 3, 0}	//{Zeomancer}
	};

    public const int SKILL_DodgeAttack = 0;
    public const int SKILL_LuckSave = 1;
    public const int SKILL_MysticDefense = 2;
    public const int SKILL_VisionRange = 3;
    public const int SKILL_MeleeAttack = 4;
    public const int SKILL_MissileAttack = 5;
    public const int SKILL_Stealth = 6;
    public const int SKILL_Detection = 7;
    public const int SKILL_DisarmTrap = 8;
    public const int SKILL_PsiSkill = 9;
    public const int SKILL_PsiForce = 10;
    public const int SKILL_Technical = 11;
    public const int SKILL_LearnSpell = 12;
    public const int SKILL_Identify = 13;

    public static int[,] SkillAdv = new int[NumJobs, NumSkill]
    {
        {-5,-5,-5, 0, 3, 3,-5,-6,-9, 1, 0, 0, 0,-5, 0},	//{Marine}
	    {-5,-5,-5,-9, 1, 1, 1,-5,-7, 3, 1, 0, 3,-5, 0},	//{Astral Seer}
	    {-5,-5,-5,-9, 2, 3, 1,-4,-5, 2,-4, 0,-2,-5, 0},	//{Navigator}
	    {-5, 1,-5, 0, 2, 2, 2, 1, 1, 1, 0, 2, 0,-5, 0},	//{Hacker}
	    {-5,-5, 3, 0, 3, 3, 1,-5,-7, 2, 2, 0, 2,-5, 0},	//{Demon Hunter}
	    {-5,-5,-5,-6, 2, 2, 1,-4,-4, 2, 0, 1, 0, 1, 0},	//{Explorer}
	    {-5,-5,-5, 0, 4, 1,-4,-5,-6, 2,-5, 0,-2,-5, 0},	//{Samurai}
	    { 1,-5,-5,-5, 1, 3, 2,-5,-6, 1, 0, 0, 0,-5, 0},	//{Bounty Hunter}
	    {-5,-5,-5, 0, 3, 3,-5,-6,-7, 1, 0, 0, 0,-5, 0},	//{Pirate}
	    {-5,-5, 1, 0, 1, 1, 1,-4,-5, 3, 1, 1, 3,-5, 0},	//{Zeomancer}
	};

    //{Derived Statistic Constants}
    public const int BaseHP = 6;

    //{define the KIND field for a character model.}
    public const int MKIND_Character = 1;

    public const int NumEquipSlots = 6;

    public const int ES_MissileWeapon = dcitems.IKIND_Gun;
    public const int ES_MeleeWeapon = dcitems.IKIND_Wep;
    public const int ES_Head = dcitems.IKIND_Cap;
    public const int ES_Body = dcitems.IKIND_Armor;
    public const int ES_Hand = dcitems.IKIND_Glove;
    public const int ES_Foot = dcitems.IKIND_Shoe;

    public static string[] EquipSlotName = new string[NumEquipSlots]
    {
        "Missile Weapon:","Melee Weapon:  ","Head:          ",
        "Body:          ","Hands:         ","Feet:          ",
    };

    public class DCChar
    {
        //{Primary Characteristics. These are saved to disk.}
        public string name; //{The character's name.}
        public string bgOrigin;    //{Character history}
        public string bgHistory;   //{Family/Planet status}
        public string bgArrival;   //{Character arrival at DeadCold}
        public int gender;     //{The character's gender.}
        public int[] stat = new int[NumStats]; //{The character's stats.}
        public int[] skill = new int[NumSkill]; //{The character's skill ranks.}
        public int lvl; //{Experience Level}
        public int XP; //{Experience Points}
        public dcitems.DCItem inv; //{Inventory}
        public dcitems.DCItem[] eqp = new dcitems.DCItem[NumEquipSlots]; //{Equipment}
        public plotbase.NAtt SF; //{The PC's status.}
        public spells.SpellMem spell; //{The PC's psi powers.}
        public int job; //{The character's job.}
        public int HP; //{Hit Points current and maximum.}
        public int HPMax;
        public int MP; //{Mojo Points current and maximum.}
        public int MPMax;

        public int carbs; //{Carbohydrate levels. i.e. food.}

        //{Runtime Characteristics. These are generated for the character during the game.}
        public texmodel.Model m; //{A pointer to the character's model.}
        public texmodel.Model target; //{The most recently attacked enemy.}
        public int repCount;
        public char lastCmd;
        public int repState; //{Set to 0 when a repeat is first issued; may be altered by other procedures.}
    }

    public static int CStat(DCChar pc, int stat)
    {
        //{Calculate the player's modified stat value.}
        int it = pc.stat[stat];

        //{Modify for attribute draining attack.}
        it -= PCStatusValue(pc.SF, -(statusfx.SEF_DrainBase + stat));

        //{Modify for attribute boost effects.}
        it += PCStatusValue(pc.SF, statusfx.SEF_BoostBase + stat);

        //{Modify for hunger/starvation.}
        if (pc.carbs < 0)
        {
            if (stat < STAT_Technical)
            {
                it += pc.carbs;
            }
            else
            {
                it += pc.carbs / 2;
            }
        }

        if (it < 1)
            it = 1;

        return it;
    }

    public static int PCVisionRange(DCChar pc)
    {
	    //{Calculate the PC's vision range, for use in the POV.}
	    int it = CStat(pc, STAT_Perception) / 2 + pc.skill[SKILL_VisionRange] + 1;

	    //{Adjust for the FarSight ability.}
	    it += ((PCStatusValue(pc.SF, statusfx.SEF_VisionBonus) + 4) / 5);

        if (it < 2)
            it = 2;

        return it;
    }

    public static int PCDefense(DCChar pc)
    {
	    //{Calculate the PC's defense step, taking into account such}
	    //{things as stats, skills, and equipment.}

	    //{Defense Step := Def Skill + Speed Bonus}
	    //{              + Dex and Per slight bonuses}
	    int it = CStat(pc, STAT_Speed) / 3 + pc.skill[SKILL_DodgeAttack];

	    if (CStat(pc, STAT_Luck) > 14)
            it += (CStat(pc, STAT_Luck) - 12) / 3;

	    if (CStat(pc, STAT_Dexterity) > 16)
            it += (CStat(pc, STAT_Dexterity) - 12) / 5;

	    if (CStat(pc, STAT_Perception) > 19)
            it += (CStat(pc, STAT_Perception) - 15) / 5;

        //{If the player is not wearing shoes, movement over the hard}
        //{metal floors of the space station is adversely affected.}
        if (pc.eqp[ES_Foot - 1] == null)
        {
            it -= 2;
        }

        if (it < 1)
            it = 1;

        return it;
    }

    public static int PCLuckSave(DCChar pc)
    {
        //{Calculate the PC's Luck Save. This is the defense used}
        //{against traps, explosions, breath weapons, etc.}
        int it = CStat(pc, STAT_Luck) / 3 + pc.skill[SKILL_LuckSave];

        if (CStat(pc, STAT_Perception) > 14)
            it += (CStat(pc, STAT_Perception) - 12) / 3;

        if (CStat(pc, STAT_Speed) > 16)
            it += (CStat(pc, STAT_Speed) - 12) / 5;

        return it;
    }

    public static int PCMysticDefense(DCChar pc)
    {
        //{Calculate the PC's Mystic Defense. This is the defense used}
        //{against psi powers and certain monster abilities.}
        int it = CStat(pc, STAT_Luck) / 3 + pc.skill[SKILL_MysticDefense];

        if (CStat(pc, STAT_Willpower) > 14)
            it += (CStat(pc, STAT_Willpower) - 12) / 3;

        return it;
    }

    public static int PCMeleeSkill(DCChar pc)
    {
	    //{Calculate the PC's melee skill step.}
	    int it = pc.skill[SKILL_MeleeAttack];

        if (pc.eqp[ES_MeleeWeapon - 1] != null)
        {
            it += dcitems.CWep[pc.eqp[ES_MeleeWeapon - 1].icode - 1].ACC;
            it += CStat(pc, dcitems.CWep[pc.eqp[ES_MeleeWeapon - 1].icode - 1].stat) / 3;
        }
        else
        {
            it += CStat(pc, STAT_Strength) / 3;
        }

        //{Add the CCM of the PC's missile weapon.}
        if (pc.eqp[ES_MissileWeapon - 1] != null)
        {
            it += dcitems.CGuns[pc.eqp[ES_MissileWeapon - 1].icode - 1].CCM;
        }

	    it += PCStatusValue(pc.SF, statusfx.SEF_H2HBonus);

        if (it < 1)
            it = 1;

        return it;
    }

    public static int PCMeleeDamage(DCChar pc)
    {
        //{Calculate the damage of the PC's basic melee attack.}

        int it = 0;

        //{Calculate base weapon damage.}
        if (pc.eqp[ES_MeleeWeapon - 1] != null)
        {
            it = dcitems.CWep[pc.eqp[ES_MeleeWeapon - 1].icode - 1].DMG;
        }

        //{Add Strength bonus.}
        if (CStat(pc, STAT_Strength) > 12)
        {
            it += CStat(pc, STAT_Strength) - 12;
        }
        else if (it == 0)
        {
            it = 1;
        }

	    //{Add a bonus for Status Change effects.}
	    it += PCStatusValue(pc.SF, statusfx.SEF_CCDmgBonus);

        return it;
    }

    public static int PCMissileSkill(DCChar pc)
    {
        //{Calculate the PC's melee skill step.}
        int it = pc.skill[SKILL_MissileAttack] + CStat(pc, STAT_Dexterity) / 3;
        if (pc.eqp[ES_MissileWeapon - 1] != null)
        {
            it += dcitems.CGuns[pc.eqp[ES_MissileWeapon - 1].icode - 1].ACC;
        }

        it += PCStatusValue(pc.SF, statusfx.SEF_MslBonus);

        if (it < 1)
            it = 1;

        return it;
    }

    public static int PCMissileDamage(DCChar pc)
    {
        //{Calculate the damage of the PC's basic missile attack.}
        int it = 0;

        if (pc.eqp[ES_MissileWeapon - 1] != null)
        {
            it = dcitems.CGuns[pc.eqp[ES_MissileWeapon - 1].icode - 1].DMG;
        }

        return it;
    }

    public static int PCMissileRange(DCChar pc)
    {
        //{Calculate the range of the PC's basic missile attack.}
        int it = 0;

        if (pc.eqp[ES_MissileWeapon - 1] != null)
        {
            it = dcitems.CGuns[pc.eqp[ES_MissileWeapon - 1].icode - 1].RNG;
        }

        return it;
    }

    public static int PCThrowSkill(DCChar pc)
    {
        //{Determine the PC's grenade throwing skill. This is the}
        //{average between the PC's Missile and Melee skills.}
        int it = (pc.skill[SKILL_MissileAttack] + pc.skill[SKILL_MeleeAttack]) / 2;
        it += CStat(pc, STAT_Dexterity) / 3;

        //{Throwing skill gets the Missile Bonus from spells.}
        it += PCStatusValue(pc.SF, statusfx.SEF_MslBonus);

        if (it < 1)
            it = 1;

        return it;
    }

    public static int PCThrowRange(DCChar pc)
    {
        //{Determine the maximum range at which the PC can throw}
        //{a grenade.}
        int it = CStat(pc, STAT_Strength) / 2 + 3;
        return it;
    }

    public static int PCArmorPV(DCChar pc)
    {
	    //{Add up the protection value of all the bits of armor that}
	    //{the PC is wearing.}
	    int it = 0;
        if (pc.eqp[ES_Head - 1] != null && pc.eqp[ES_Head - 1].ikind == dcitems.IKIND_Cap)
        {
            it += dcitems.CCap[pc.eqp[ES_Head - 1].icode - 1].PV;
        }
        if (pc.eqp[ES_Body - 1] != null && pc.eqp[ES_Body - 1].ikind == dcitems.IKIND_Armor)
        {
            it += dcitems.CArmor[pc.eqp[ES_Body - 1].icode - 1].PV;
        }
        if (pc.eqp[ES_Hand - 1] != null && pc.eqp[ES_Hand - 1].ikind == dcitems.IKIND_Glove)
        {
            it += dcitems.CGlove[pc.eqp[ES_Hand - 1].icode - 1].PV;
        }
        if (pc.eqp[ES_Foot - 1] != null && pc.eqp[ES_Foot - 1].ikind == dcitems.IKIND_Shoe)
        {
            it += dcitems.CShoe[pc.eqp[ES_Foot - 1].icode - 1].PV;
        }

	    //{Add the bonus for mystic armor, i.e. residual spells.}
	    it += PCStatusValue(pc.SF, statusfx.SEF_ArmorBonus);

        return it;
    }

    public static int PCStealth(DCChar pc)
    {
	    //{Determine the PC's stealth rating.}
	    int it = CStat(pc, STAT_Perception) / 3 + pc.skill[SKILL_Stealth];
	    it += CStat(pc, STAT_Luck) / 5 + CStat(pc, STAT_Dexterity) / 9;

        //{If the player is not wearing shoes, stealth is improved.}
        if (pc.eqp[ES_Foot - 1] == null)
        {
            it += 1;
        }

	    //{Add a bonus for spells benefiting the PC.}
	    it += PCStatusValue(pc.SF, statusfx.SEF_StealthBonus);

        if (it < 1)
            it = 1;

        return it;
    }

    public static int PCDetection(DCChar pc)
    {
        //{Determine the PC's detection rating.}
        int it = CStat(pc, STAT_Perception) / 3 + pc.skill[SKILL_Detection];
        it += CStat(pc, STAT_Luck) / 8;
        return it;
    }

    public static int PCDisarmSkill(DCChar pc)
    {
	    //{Determine the PC's disarm trap rating.}
	    int it = CStat(pc, STAT_Technical) / 3 + pc.skill[SKILL_DisarmTrap];
        if (CStat(pc, STAT_Perception) > 16)
        {
            it += (CStat(pc, STAT_Perception) - 12) / 5;
        }

        if (it < 1)
            it = 1;

        return it;
    }

    public static int PCTechSkill(DCChar pc)
    {
	    //{Determine the PC's technology rating.}
	    int it = CStat(pc, STAT_Technical) / 3 + pc.skill[SKILL_Technical];
        if (CStat(pc, STAT_Perception) > 16)
        {
            it += (CStat(pc, STAT_Perception) - 12) / 5;
        }

	    if (it < 1)
            it = 1;

        return it;
    }

    public static int PCMoveSpeed(DCChar pc)
    {
	    //{Determine the movement rate of the PC. The number returned}
	    //{will be the number of actions that the player will get over}
	    //{the course of 12 clicks.}
	    int it = CStat(pc, STAT_Speed) / 3 + 3;
	    it += PCStatusValue(pc.SF, statusfx.SEF_SpeedBonus);

        //{If the player is not wearing shoes, movement over the hard}
        //{metal floors of the space station is adversely affected.}
        if (pc.eqp[ES_Foot - 1] == null)
        {
            it -= 1;
        }

	    if (it < 1)
            it = 1;

        return it;
    }

    public static int PCRegeneration(DCChar pc)
    {
	    //{Determine the speed of the PC's natural healing.}
	    int it = pc.HPMax / 8;
	    it += PCStatusValue(pc.SF, statusfx.SEF_Regeneration);
	    if (it < 1)
            it = 1;

        //{Modify for the PC's action.}
        //{HP are restored more quickly when the PC is standing still.}
        if (pc.lastCmd == '5')
        {
            it += 3;

            if (pc.repCount > 0)
            {
                it += 2;
            }
        }

        return it;
    }

    public static int PCRestoration(DCChar pc)
    {

	    //{Determine the speed of the PC's natural mojo renewal.}
	    int it = pc.MPMax / 3;
	    it += PCStatusValue(pc.SF, statusfx.SEF_Restoration);
	    if (it < 1)
            it = 1;

        //{Modify for actions.}
        if (pc.lastCmd == '5')
        {
            it += 3;
            if (pc.repCount > 0)
                it += 2;
        }

        return it;
    }

    public static int PCPsiSkill(DCChar pc)
    {
        //{Calculate the PC's spellcasting skill.}
        int it = pc.skill[SKILL_PsiSkill] + CStat(pc, STAT_Willpower) / 3;
        if (it < 1)
            it = 1;

        return it;
    }

    public static int PCPsiForce(DCChar pc)
    {
	    //{Calculate the PC's spellcasting effect step.}
	    int it = pc.skill[SKILL_PsiForce];
        if (CStat(pc, STAT_Willpower) > 12)
        {
            it += (CStat(pc, STAT_Willpower) - 11) / 2;
        }

        return it;
    }

    public static int PCIDSkill(DCChar pc)
    {
	    //{Calculate the PC's item identification skill.}
	    int it = pc.skill[SKILL_Identify] + CStat(pc, STAT_Technical) / 3;
        if (it < 1)
            it = 1;

        return it;
    }

    public static int PCHPBonus(DCChar pc)
    {
	    //{Calculate the level-up HP bonus of the character. This is}
	    //{based on true Toughness, not the modified score.}
	    int it = (pc.stat[STAT_Toughness] - 11) / 2;
        if (it < -1)
            it = -1;

        return it;
    }

    public static int PCMPBonus(DCChar pc)
    {
	    //{Calculate the level-up MP bonus of the character. This is}
	    //{based on true Willpower, not the modified score.}
	    int it = (pc.stat[STAT_Willpower] - 11) / 2;
        if (it < -1)
            it = -1;

        return it;
    }

    public static void WritePC(DCChar pc, StreamWriter f)
    {
	    //{F is an open text file. Write all the data for the given PC}
	    //{to that file.}

	    //{Write an identifier, to make debugging and savefile cheating}
	    //{so much easier.}
	    f.WriteLine("*** DCChar Block ***");

	    //{General Data block}
	    f.WriteLine(pc.m.x);
	    f.WriteLine(pc.m.y);
	    f.WriteLine(pc.name);
	    f.WriteLine(pc.gender);
	    f.WriteLine(pc.job);
	    f.WriteLine(pc.HP);
	    f.WriteLine(pc.HPMax);
	    f.WriteLine(pc.MP);
	    f.WriteLine(pc.MPMax);
	    f.WriteLine(pc.carbs);
	    f.WriteLine(pc.lvl);
	    f.WriteLine(pc.XP);

        int t;
        //{Stats block}
        for (t = 0; t < NumStats; ++t)
        {
            f.WriteLine(pc.stat[t]);
        }

        //{Skills block}
        for (t = 0; t < NumSkill; ++t)
        {
            f.WriteLine(pc.skill[t]);
        }

        //{Equipment Slots block}
        for (t = 0; t < NumEquipSlots; ++t)
        {
            dcitems.WriteItemList(pc.eqp[t], f);
        }

        //{Inventory block}
        dcitems.WriteItemList(pc.inv, f);

	    //{Status block}
	    plotbase.WriteNAtt(pc.SF, f);

	    //{Spells block}
	    spells.WriteSpellMem(pc.spell, f);
    }

    public static DCChar ReadPC(StreamReader f, texmaps.GameBoard gb, int SFV)
    {
        //{F is an open text file. Read in all the data needed for}
        //{a character, as written to the file by the above procedure.}
        //{Also, initialize the model for the PC.}
        //var
        // PC: DCCharPtr;
        // T: Integer;
        // A: String;
        // X,Y: Integer;

        //{Allocate memory for the character to be loaded.}
        DCChar pc = new DCChar();
	    pc.target = null;

	    //{Read in the identification line.}
	    f.ReadLine();

	    //{General Data block}
	    int x = int.Parse(f.ReadLine());
	    int y = int.Parse(f.ReadLine());
	    pc.name = f.ReadLine();
	    pc.gender = int.Parse(f.ReadLine());
	    pc.job = int.Parse(f.ReadLine());
	    pc.HP = int.Parse(f.ReadLine());
	    pc.HPMax = int.Parse(f.ReadLine());
	    pc.MP = int.Parse(f.ReadLine());
	    pc.MPMax = int.Parse(f.ReadLine());
	    pc.carbs = int.Parse(f.ReadLine());
	    pc.lvl = int.Parse(f.ReadLine());
	    pc.XP = int.Parse(f.ReadLine());

        int t;

        //{Stats block}
        for (t = 0; t < NumStats; ++t)
        {
            pc.stat[t] = int.Parse(f.ReadLine());
        }

        //{Skills block}
        for (t = 0; t < NumSkill; ++t)
        {
            pc.skill[t] = int.Parse(f.ReadLine());
        }

        //{Equipment Slots block}
        for (t = 0; t < NumEquipSlots; ++t)
        {
            pc.eqp[t] = dcitems.ReadItemList(f);
        }

	    //{Inventory block}
	    pc.inv = dcitems.ReadItemList(f);

	    //{Status block}
		pc.SF = plotbase.ReadNAtt(f);

	    //{Spells block}
	    pc.spell = spells.ReadSpellMem(f);

	    //{Now, finally, we have all the info we need for the PC.}
	    //{Let's initialize some values so that things can start working.}
	    pc.repCount = 0;
	    pc.lastCmd = '&';

	    //{Add the model here, then set the POV data.}
	    pc.m = texmodel.AddModel(ref gb.mlist, gb.mog, '@', Crt.Color.LightGreen, Crt.Color.White, false, x, y, MKIND_Character);

	    gb.POV.m = pc.m;
	    gb.POV.range = PCVisionRange(pc);
	    texmaps.UpdatePOV(gb.POV, gb);

        return pc;
    }

    static int PCStatusValue(plotbase.NAtt SL, int sfx)
    {
        //{ Determine the value of this status effect. }
        int N = plotbase.NAttValue(SL, statusfx.NAG_StatusChange, sfx);
        if (N > 0)
        {
            N = (N + 9) / 10;
        }
        else if (N == -1)
        {
            N = 3;
        }

        return N;
    }
}
