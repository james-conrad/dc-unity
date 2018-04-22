﻿using System;

class charts
{
	//{What does this unit do? Well, you probably know that}
	//{any pen-&-paper RPG worth its salt comes with pages and}
	//{pages and pages of charts that the GM can use to roll}
	//{up encounters, rewards, and whatnots.}

	//{Okay, maybe most good modern RPGs have done away with}
	//{the oodles of hideous charts that were the norm all}
	//{throughout the 80s... but this is a computer game, darnit,}
	//{and we need charts to supplement the CPU's inherent lack}
	//{of imagination.}

	//{So, that's what this unit is. Random generators for all}
	//{kinds of stuff. The appendicies of the 1e DMG, if you will.}
	//{And if you understand that reference, no more need be said.}

    public const int MaxMonsters = 1500; //{The maximum number of monsters that can appear on the map at once.}

	//{Our wandering monster chart is structured as follows:}
	//{ The first value is the creature number.}
	//{ The second value is the # Appearing die size.}
	public const int NumWCT = 25; //{Number of Wandering Critter Types.}
    public const int NumWChart = 6;	//{ Number of Wandering Monster Charts. }
    public static int[,,] WanderChart = new int[NumWChart, NumWCT, 2]
    {
        {   {4,4},{4,8},{11,2},{8,1},{3,1},   //{ Signature Chart -         }
		    {4,6},{16,1},{11,1},{8,2},{3,1},  //{   Module "B": Memorials   }
		    {4,6},{16,1},{10,1},{8,2},{15,4}, //{ Many vacuum critters &  }
		    {4,8},{16,1},{10,3},{8,2},{15,5}, //{ non-breathers here.     }
		    {4,10},{16,1},{18,1},{8,3},{15,6}   },

        {   {2,4},{2,4},{4,4},{6,3},{2,3},	//{ This is chart 1, and also }
		    {2,4},{2,4},{4,4},{6,4},{2,3},	//{ the signature chart for }
		    {2,4},{2,4},{4,4},{6,4},{10,1},	//{ Module "C" - Visitor Center }
		    {2,4},{8,1},{4,4},{6,4},{2,3},	//{   A lot of easy monsters. }
		    {2,6},{4,4},{2,5},{2,4},{15,3}  },

        {   {2,4},{2,4},{4,4},{6,5},{8,1},
            {2,4},{2,4},{4,4},{6,4},{8,1},
            {2,4},{2,8},{4,4},{6,4},{10,1},
            {2,4},{8,1},{4,4},{6,4},{11,1},
            {2,6},{4,4},{4,8},{16,1},{15,3} },

        {   {2,8},{3,1},{5,1},{9,1},{12,1},
            {2,8},{4,6},{6,8},{9,3},{14,1},
            {2,8},{4,8},{6,10},{10,4},{12,1},
            {2,10},{4,10},{8,3},{10,3},{15,8},
            {19,1},{18,1},{8,3},{11,6},{2,16}   },

        {   {2,12},{4,20},{9,5},{12,5},{3,1},
            {20,1},{5,1},{9,5},{12,5},{3,2},
            {16,3},{6,20},{9,5},{12,3},{3,2},
            {3,1},{8,8},{10,5},{14,1},{2,16},
            {18,4},{13,1},{10,5},{14,1},{2,16}  },

        {   {20,1},{18,8},{9,5},{12,5},{3,3},
            {19,4},{5,4},{20,1},{12,5},{3,3},
            {16,3},{6,20},{19,4},{12,3},{3,3},
            {3,1},{8,8},{10,5},{14,1},{17,1},
            {3,3},{13,1},{10,6},{14,1},{17,1}   }
    };

	//{TType stands for Treasure Type.}
	public const int NumTType = 24;
	public const int NumTChance = 25; //{Number of entries per random chart.}

	public const int TType_SecurityArea = 1;
    public const int TType_StorageRoom = 2;
    public const int TType_SpaceGear = 3;
    public const int TType_BasicWeapons = 4;
    public const int TType_RobotWeapons = 5;
    public const int TType_TechnoItems = 6;
    public const int TType_Crypt = 7;

    public const int TType_AllMedicene = 9;
    public const int TType_AllFood = 9;
    public const int TType_AllAmmo = 10;
    public const int TType_BasicGuns = 11;
    public const int TType_BasicWeps = 12;
    public const int TType_CivilianClothes = 13;
    public const int TType_C5mm = 14;
    public const int TType_C8mm = 15;
    public const int TType_C12mm = 16;
    public const int TType_C25mm = 17;
    public const int TType_CGrn = 18;
    public const int TType_UC5mm = 19;
    public const int TType_UC8mm = 20;
    public const int TType_UC12mm = 21;
    public const int TType_AdvGuns = 22;
    public const int TType_AdvWeps = 23;
    public const int TType_AdvClothes = 24;

	public static int[,,] TTChart = new int[NumTType, NumTChance, 3]
    {
		//{SECURITY ROOM TREASURE}
	{	{dcitems.IKIND_Ammo,1,30},{dcitems.IKIND_Ammo,2,20},{TType_C12mm,-1,1},{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},
		{TType_UC5mm,-1,0},{TType_C8mm,-1,0},{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},
		{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},
		{TType_BasicGuns,-1,0},{TType_CGrn,-1,0},{TType_CGrn,-1,0},{TType_CGrn,-1,0},{TType_CGrn,-1,0},
		{TType_CGrn,-1,0},{TType_AdvGuns,-1,0},{TType_BasicGuns,-1,0},{TType_AdvWeps,-1,0},{TType_BasicWeps,-1,0}},

		//{STORAGE AREA TREASURE CHART}
	{	{TType_AllFood,-1,1},{dcitems.IKIND_Food,2,20},{dcitems.IKIND_Food,3,10},{dcitems.IKIND_Food,4,10},{TType_AllFood,-1,1},
		{TType_AllFood,-1,1},{dcitems.IKIND_Food,7,5},{dcitems.IKIND_Food,35,1},{dcitems.IKIND_Food,8,4},{dcitems.IKIND_Food,34,1},
		{TType_AllMedicene,-1,1},{TType_AllFood,-1,5},{dcitems.IKIND_Food,7,4},{TType_SpaceGear,-1,7},{TType_AllFood,-1,1},
		{TType_AllMedicene,-1,23},{TType_AllFood,-1,1},{TType_AllFood,-1,1},{dcitems.IKIND_Armor,1,1},{dcitems.IKIND_Armor,1,1},
		{TType_CivilianClothes,-1,1},{TType_CivilianClothes,-1,1},{TType_SpaceGear,-1,6},{TType_AllAmmo,-1,0},{TType_AllFood,-1,0}},

		//{GENERAL SPACE LIVING GEAR - dropped by polyps and other random treasure monsters}
	{	{TType_AllFood,-1,0},{TType_AllMedicene,-1,0},{TType_AllMedicene,-1,0},{TType_BasicWeps,-1,0},{TType_BasicWeapons,-1,0},
		{dcitems.IKIND_Food,7,1},{dcitems.IKIND_Food,8,1},{TType_TechnoItems,-1,1},{TType_AllFood,-1,1},{TType_AllFood,-1,1},
		{dcitems.IKIND_Electronics,1,0},{TType_CivilianClothes,-1,1},{TType_CivilianClothes,-1,1},{dcitems.IKIND_Armor,2,1},{dcitems.IKIND_Glove,1,1},
		{TType_CivilianClothes,-1,1},{TType_CivilianClothes,-1,1},{dcitems.IKIND_Ammo,1,20},{dcitems.IKIND_Grenade,12,8},{TType_AllMedicene,-1,0},
		{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},{TType_AllFood,-1,0},{TType_AllFood,-1,0},{TType_AllMedicene,-1,0}},

		//{BASIC WEAPONS - carried by zombies and other critters}
	{	{dcitems.IKIND_Gun,1,10},{dcitems.IKIND_Gun,2,24},{dcitems.IKIND_Gun,3,20},{dcitems.IKIND_Gun,4,8},{dcitems.IKIND_Gun,5,0},
		{dcitems.IKIND_Gun,6,30},{dcitems.IKIND_Gun,1,8},{dcitems.IKIND_Gun,4,0},{dcitems.IKIND_Gun,2,10},{dcitems.IKIND_Gun,9,7},
		{dcitems.IKIND_Gun,3,0},{dcitems.IKIND_Gun,1,8},{dcitems.IKIND_Gun,1,0},{dcitems.IKIND_Gun,3,5},{dcitems.IKIND_Wep,15,1},
		{dcitems.IKIND_Wep,2,0},{dcitems.IKIND_Wep,3,0},{dcitems.IKIND_Wep,4,0},{dcitems.IKIND_Wep,6,0},{dcitems.IKIND_Wep,8,0},
		{TType_BasicGuns,-1,0},{TType_BasicWeps,-1,0},{TType_BasicWeps,-1,0},{TType_BasicWeps,-1,0},{TType_BasicGuns,-1,0}},

		//{ROBOT WEAPONS}
	{	{dcitems.IKIND_Gun,2,10},{dcitems.IKIND_Gun,2,20},{dcitems.IKIND_Gun,3,20},{dcitems.IKIND_Gun,3,8},{dcitems.IKIND_Gun,4,0},
		{dcitems.IKIND_Gun,2,10},{dcitems.IKIND_Gun,2,20},{dcitems.IKIND_Gun,3,20},{dcitems.IKIND_Gun,3,8},{TType_BasicGuns,-1,0},
		{dcitems.IKIND_Gun,2,10},{dcitems.IKIND_Gun,2,20},{dcitems.IKIND_Gun,3,20},{dcitems.IKIND_Gun,3,8},{TType_BasicGuns,-1,2},
		{dcitems.IKIND_Gun,2,10},{dcitems.IKIND_Gun,2,20},{dcitems.IKIND_Gun,3,20},{dcitems.IKIND_Gun,3,8},{TType_BasicGuns,-1,0},
		{dcitems.IKIND_Gun,2,10},{dcitems.IKIND_Gun,12,10},{dcitems.IKIND_Gun,3,20},{dcitems.IKIND_Gun,11,8},{dcitems.IKIND_Gun,7,10}},

		//{TECHNOLOGICAL ITEMS - usually dropped by slain robots}
	{	{dcitems.IKIND_Wep,8,1},{dcitems.IKIND_Wep,15,1},{dcitems.IKIND_Wep,8,1},{dcitems.IKIND_Ammo,5,3},{dcitems.IKIND_Ammo,5,3},
		{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},{dcitems.IKIND_Ammo,5,3},{dcitems.IKIND_Ammo,5,3},{dcitems.IKIND_Ammo,5,3},
		{TType_AllAmmo,-1,0},{TType_AllAmmo,-1,0},{dcitems.IKIND_Ammo,5,3},{dcitems.IKIND_Ammo,5,3},{dcitems.IKIND_Ammo,5,3},
		{TType_AllAmmo,-1,0},{dcitems.IKIND_Ammo,5,2},{dcitems.IKIND_Ammo,5,2},{dcitems.IKIND_Ammo,5,10},{dcitems.IKIND_Ammo,5,3},
		{TType_BasicGuns,-1,6},{TType_AllAmmo,-1,2},{TType_AllAmmo,-1,2},{dcitems.IKIND_Ammo,5,3},{dcitems.IKIND_Ammo,5,3}},

		//{CRYPT TREASURES}
	{	{dcitems.IKIND_Cap,4,1},{dcitems.IKIND_Armor,5,1},{dcitems.IKIND_Shoe,5,1},{dcitems.IKIND_KeyItem,2,1},{dcitems.IKIND_KeyItem,3,1},
		{dcitems.IKIND_Wep,9,1},{dcitems.IKIND_KeyItem,4,1},{dcitems.IKIND_KeyItem,3,1},{dcitems.IKIND_KeyItem,4,1},{dcitems.IKIND_KeyItem,5,1},
		{TType_CivilianClothes,-1,0},{TType_CivilianClothes,-1,0},{TType_CivilianClothes,-1,0},{TType_CivilianClothes,-1,0},{TType_AdvClothes,-1,0},
		{TType_CivilianClothes,-1,0},{TType_CivilianClothes,-1,0},{TType_CivilianClothes,-1,0},{TType_CivilianClothes,-1,0},{TType_CivilianClothes,-1,0},
		{TType_BasicGuns,-1,0},{TType_BasicWeps,-1,0},{TType_AdvGuns,-1,0},{TType_AdvWeps,-1,0},{TType_AdvWeps,-1,0} },

		//{ALL MEDICENE}
	{	{dcitems.IKIND_Food,24,5},{dcitems.IKIND_Food,25,5},{dcitems.IKIND_Food,26,5},{dcitems.IKIND_Food,27,5},{dcitems.IKIND_Food,30,10},
		{dcitems.IKIND_Food,24,10},{dcitems.IKIND_Food,25,5},{dcitems.IKIND_Food,26,1},{dcitems.IKIND_Food,27,5},{dcitems.IKIND_Food,30,10},
		{dcitems.IKIND_Food,24,1},{dcitems.IKIND_Food,25,5},{dcitems.IKIND_Food,26,1},{dcitems.IKIND_Food,27,1},{dcitems.IKIND_Food,30,10},
		{dcitems.IKIND_Food,24,1},{dcitems.IKIND_Food,39,5},{dcitems.IKIND_Food,38,5},{dcitems.IKIND_Food,37,5},{dcitems.IKIND_Food,36,5},
		{dcitems.IKIND_Food,33,5},{dcitems.IKIND_Food,32,5},{dcitems.IKIND_Food,31,5},{dcitems.IKIND_Food,28,1},{dcitems.IKIND_Food,29,10}},

		//{ALL FOOD}
	{	{dcitems.IKIND_Food,1,6},{dcitems.IKIND_Food,2,20},{dcitems.IKIND_Food,3,10},{dcitems.IKIND_Food,4,10},{dcitems.IKIND_Food,5,10},
		{dcitems.IKIND_Food,6,8},{dcitems.IKIND_Food,7,5},{dcitems.IKIND_Food,7,3},{dcitems.IKIND_Food,8,4},{dcitems.IKIND_Food,8,5},
		{dcitems.IKIND_Food,16,4},{dcitems.IKIND_Food,24,15},{dcitems.IKIND_Food,7,4},{dcitems.IKIND_Food,7,4},{dcitems.IKIND_Food,34,3},
		{dcitems.IKIND_Food,23,8},{dcitems.IKIND_Food,22,8},{dcitems.IKIND_Food,21,5},{dcitems.IKIND_Food,15,1},{dcitems.IKIND_Food,14,1},
		{dcitems.IKIND_Food,9,1},{dcitems.IKIND_Food,10,1},{dcitems.IKIND_Food,11,1},{dcitems.IKIND_Food,12,1},{dcitems.IKIND_Food,13,1}},

		//{ALL AMMO}
	{	{dcitems.IKIND_Ammo,1,50},{dcitems.IKIND_Ammo,2,30},{dcitems.IKIND_Ammo,3,20},{dcitems.IKIND_Ammo,4,10},{dcitems.IKIND_Ammo,5,3},
		{dcitems.IKIND_Ammo,1,40},{dcitems.IKIND_Ammo,2,25},{dcitems.IKIND_Ammo,103,10},{TType_C25mm,-1,0},{dcitems.IKIND_Ammo,6,3},
		{dcitems.IKIND_Ammo,1,30},{dcitems.IKIND_Ammo,2,20},{dcitems.IKIND_Ammo,3,20},{TType_C25mm,-1,0},{dcitems.IKIND_Ammo,5,3},
		{TType_C5mm,-1,0},{TType_C8mm,-1,0},{TType_C12mm,-1,0},{TType_UC5mm,-1,0},{dcitems.IKIND_Ammo,6,3},
		{TType_C5mm,-1,0},{TType_C8mm,-1,0},{TType_C12mm,-1,0},{TType_UC8mm,-1,0},{TType_CGrn,-1,0}},

		//{BASIC GUNS}
	{	{dcitems.IKIND_Gun,1,10},{dcitems.IKIND_Gun,2,10},{dcitems.IKIND_Gun,3,10},{dcitems.IKIND_Gun,4,7},{dcitems.IKIND_Gun,5,3},
		{dcitems.IKIND_Gun,6,10},{dcitems.IKIND_Gun,7,10},{dcitems.IKIND_Gun,8,10},{dcitems.IKIND_Gun,9,10},{dcitems.IKIND_Gun,10,10},
		{dcitems.IKIND_Gun,1,0},{dcitems.IKIND_Gun,9,10},{dcitems.IKIND_Gun,2,10},{dcitems.IKIND_Gun,4,6},{dcitems.IKIND_Gun,1,9},
		{dcitems.IKIND_Gun,1,0},{dcitems.IKIND_Gun,9,10},{dcitems.IKIND_Gun,3,10},{dcitems.IKIND_Gun,5,5},{dcitems.IKIND_Gun,9,3},
		{dcitems.IKIND_Gun,11,10},{dcitems.IKIND_Gun,12,10},{dcitems.IKIND_Gun,13,8},{dcitems.IKIND_Gun,14,8},{TType_AdvGuns,-1,-1}	},

		//{BASIC WEAPONS}
	{	{dcitems.IKIND_Wep,1,0},{dcitems.IKIND_Wep,2,0},{dcitems.IKIND_Wep,3,0},{dcitems.IKIND_Wep,4,0},{dcitems.IKIND_Wep,1,1},
		{dcitems.IKIND_Wep,6,1},{dcitems.IKIND_Wep,13,1},{dcitems.IKIND_Wep,8,1},{dcitems.IKIND_Wep,1,1},{dcitems.IKIND_Wep,6,1},
		{dcitems.IKIND_Wep,8,1},{dcitems.IKIND_Wep,9,1},{dcitems.IKIND_Wep,14,1},{dcitems.IKIND_Wep,3,1},{dcitems.IKIND_Wep,4,1},
		{dcitems.IKIND_Wep,1,1},{dcitems.IKIND_Wep,15,1},{dcitems.IKIND_Wep,3,1},{dcitems.IKIND_Wep,4,1},{dcitems.IKIND_Wep,6,1},
		{dcitems.IKIND_Wep,6,1},{dcitems.IKIND_Wep,1,1},{dcitems.IKIND_Wep,8,1},{dcitems.IKIND_Wep,3,1},{TType_AdvWeps,-1,-1}	},

		//{CIVILIAN CLOTHING}
	{	{dcitems.IKIND_Armor,1,1},{dcitems.IKIND_Armor,1,1},{dcitems.IKIND_Armor,1,1},{dcitems.IKIND_Armor,1,1},{dcitems.IKIND_Armor,1,1},
		{dcitems.IKIND_Armor,2,1},{dcitems.IKIND_Armor,4,1},{dcitems.IKIND_Armor,2,1},{dcitems.IKIND_Shoe,2,1},{dcitems.IKIND_Cap,1,1},
		{dcitems.IKIND_Armor,6,1},{dcitems.IKIND_Armor,6,1},{dcitems.IKIND_Armor,6,1},{dcitems.IKIND_Armor,6,1},{dcitems.IKIND_Armor,1,1},
		{dcitems.IKIND_Cap,1,1},{dcitems.IKIND_Cap,2,1},{dcitems.IKIND_Glove,1,1},{dcitems.IKIND_Glove,1,1},{dcitems.IKIND_Shoe,1,1},
		{dcitems.IKIND_Shoe,2,1},{dcitems.IKIND_Shoe,3,1},{dcitems.IKIND_Shoe,4,1},{TType_AdvClothes,-1,-1},{TType_AdvClothes,-1,-1}	},

		//{COMMON 5mm AMMUNITION}
	{	{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1001,25},
		{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1101,25},{dcitems.IKIND_Ammo,1001,25},
		{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1,25},{dcitems.IKIND_Ammo,1201,25},{dcitems.IKIND_Ammo,1001,25},
		{dcitems.IKIND_Ammo,301,25},{dcitems.IKIND_Ammo,301,25},{dcitems.IKIND_Ammo,301,25},{dcitems.IKIND_Ammo,301,25},{dcitems.IKIND_Ammo,301,25},
		{dcitems.IKIND_Ammo,601,25},{dcitems.IKIND_Ammo,901,25},{dcitems.IKIND_Ammo,801,25},{dcitems.IKIND_Ammo,801,25},{TType_UC5mm,-1,0}	},

		//{COMMON 8mm AMMUNITION}
	{	{dcitems.IKIND_Ammo,2,17},{dcitems.IKIND_Ammo,2,17},{dcitems.IKIND_Ammo,2,17},{dcitems.IKIND_Ammo,1102,17},{dcitems.IKIND_Ammo,1002,17},
		{dcitems.IKIND_Ammo,2,17},{dcitems.IKIND_Ammo,2,17},{dcitems.IKIND_Ammo,2,17},{dcitems.IKIND_Ammo,1102,17},{dcitems.IKIND_Ammo,1002,17},
		{dcitems.IKIND_Ammo,2,17},{dcitems.IKIND_Ammo,2,17},{dcitems.IKIND_Ammo,2,17},{dcitems.IKIND_Ammo,1202,17},{dcitems.IKIND_Ammo,1002,17},
		{dcitems.IKIND_Ammo,902,17},{dcitems.IKIND_Ammo,302,17},{dcitems.IKIND_Ammo,302,17},{dcitems.IKIND_Ammo,802,17},{dcitems.IKIND_Ammo,602,17},
		{dcitems.IKIND_Ammo,802,17},{dcitems.IKIND_Ammo,802,17},{dcitems.IKIND_Ammo,302,17},{dcitems.IKIND_Ammo,802,17},{TType_UC8mm,-1,0}	},

		//{COMMON 12mm AMMUNITION}
	{	{dcitems.IKIND_Ammo,3,15},{dcitems.IKIND_Ammo,103,15},{dcitems.IKIND_Ammo,3,15},{dcitems.IKIND_Ammo,103,15},{dcitems.IKIND_Ammo,1003,15},
		{dcitems.IKIND_Ammo,3,15},{dcitems.IKIND_Ammo,103,15},{dcitems.IKIND_Ammo,3,15},{dcitems.IKIND_Ammo,103,15},{dcitems.IKIND_Ammo,1003,15},
		{dcitems.IKIND_Ammo,3,15},{dcitems.IKIND_Ammo,103,15},{dcitems.IKIND_Ammo,3,15},{dcitems.IKIND_Ammo,103,15},{dcitems.IKIND_Ammo,1203,15},
		{dcitems.IKIND_Ammo,3,15},{dcitems.IKIND_Ammo,103,15},{dcitems.IKIND_Ammo,1403,15},{dcitems.IKIND_Ammo,603,15},{dcitems.IKIND_Ammo,903,15},
		{dcitems.IKIND_Ammo,303,15},{dcitems.IKIND_Ammo,603,15},{dcitems.IKIND_Ammo,903,15},{TType_UC12mm,-1,0},{TType_UC12mm,-1,0}	},

		//{COMMON 25mm AMMUNITION}
	{	{dcitems.IKIND_Ammo,4,30},{dcitems.IKIND_Ammo,104,20},{dcitems.IKIND_Ammo,304,10},{dcitems.IKIND_Ammo,104,10},{dcitems.IKIND_Ammo,1004,10},
		{dcitems.IKIND_Ammo,4,20},{dcitems.IKIND_Ammo,104,20},{dcitems.IKIND_Ammo,604,10},{dcitems.IKIND_Ammo,104,10},{dcitems.IKIND_Ammo,1104,10},
		{dcitems.IKIND_Ammo,4,20},{dcitems.IKIND_Ammo,104,10},{dcitems.IKIND_Ammo,404,10},{dcitems.IKIND_Ammo,1504,10},{dcitems.IKIND_Ammo,1204,10},
		{dcitems.IKIND_Ammo,4,10},{dcitems.IKIND_Ammo,104,10},{dcitems.IKIND_Ammo,704,10},{dcitems.IKIND_Ammo,4,10},{dcitems.IKIND_Ammo,1204,10},
		{dcitems.IKIND_Ammo,4,10},{dcitems.IKIND_Ammo,1404,50},{dcitems.IKIND_Ammo,904,10},{dcitems.IKIND_Ammo,4,10},{dcitems.IKIND_Ammo,1204,10}	},

		//{COMMON GRENADES}
	{	{dcitems.IKIND_Grenade,1,9},{dcitems.IKIND_Grenade,2,5},{dcitems.IKIND_Grenade,3,2},{dcitems.IKIND_Grenade,3,1},{dcitems.IKIND_Grenade,12,3},
		{dcitems.IKIND_Grenade,1,5},{dcitems.IKIND_Grenade,2,3},{dcitems.IKIND_Grenade,4,2},{dcitems.IKIND_Grenade,4,1},{dcitems.IKIND_Grenade,12,9},
		{dcitems.IKIND_Grenade,1,3},{dcitems.IKIND_Grenade,2,3},{dcitems.IKIND_Grenade,5,2},{dcitems.IKIND_Grenade,5,1},{dcitems.IKIND_Grenade,12,10},
		{dcitems.IKIND_Grenade,1,3},{dcitems.IKIND_Grenade,2,3},{dcitems.IKIND_Grenade,6,3},{dcitems.IKIND_Grenade,6,3},{dcitems.IKIND_Grenade,11,3},
		{dcitems.IKIND_Grenade,1,3},{dcitems.IKIND_Grenade,2,3},{dcitems.IKIND_Grenade,1,3},{dcitems.IKIND_Grenade,7,3},{dcitems.IKIND_Grenade,9,1}	},

		//{UNCOMMON 5mm AMMUNITION}
	{	{dcitems.IKIND_Ammo,201,20},{dcitems.IKIND_Ammo,201,20},{dcitems.IKIND_Ammo,401,20},{dcitems.IKIND_Ammo,801,20},{dcitems.IKIND_Ammo,701,20},
		{dcitems.IKIND_Ammo,201,20},{dcitems.IKIND_Ammo,201,20},{dcitems.IKIND_Ammo,401,20},{dcitems.IKIND_Ammo,801,20},{dcitems.IKIND_Ammo,701,20},
		{dcitems.IKIND_Ammo,201,20},{dcitems.IKIND_Ammo,201,20},{dcitems.IKIND_Ammo,401,20},{dcitems.IKIND_Ammo,801,20},{dcitems.IKIND_Ammo,301,20},
		{dcitems.IKIND_Ammo,201,20},{dcitems.IKIND_Ammo,1201,20},{dcitems.IKIND_Ammo,401,20},{dcitems.IKIND_Ammo,801,20},{dcitems.IKIND_Ammo,301,20},
		{dcitems.IKIND_Ammo,201,20},{dcitems.IKIND_Ammo,501,20},{dcitems.IKIND_Ammo,401,20},{dcitems.IKIND_Ammo,1501,20},{dcitems.IKIND_Ammo,301,20}	},

		//{UNCOMMON 8mm AMMUNITION}
	{	{dcitems.IKIND_Ammo,202,12},{dcitems.IKIND_Ammo,402,12},{dcitems.IKIND_Ammo,402,12},{dcitems.IKIND_Ammo,802,12},{dcitems.IKIND_Ammo,702,12},
		{dcitems.IKIND_Ammo,202,12},{dcitems.IKIND_Ammo,402,12},{dcitems.IKIND_Ammo,402,12},{dcitems.IKIND_Ammo,802,12},{dcitems.IKIND_Ammo,702,12},
		{dcitems.IKIND_Ammo,202,12},{dcitems.IKIND_Ammo,402,12},{dcitems.IKIND_Ammo,402,12},{dcitems.IKIND_Ammo,802,12},{dcitems.IKIND_Ammo,702,12},
		{dcitems.IKIND_Ammo,1202,12},{dcitems.IKIND_Ammo,402,12},{dcitems.IKIND_Ammo,502,12},{dcitems.IKIND_Ammo,802,12},{dcitems.IKIND_Ammo,302,12},
		{dcitems.IKIND_Ammo,1202,12},{dcitems.IKIND_Ammo,1202,12},{dcitems.IKIND_Ammo,502,12},{dcitems.IKIND_Ammo,802,12},{dcitems.IKIND_Ammo,302,12}	},

		//{UNCOMMON 12mm AMMUNITION}
	{	{dcitems.IKIND_Ammo,403,12},{dcitems.IKIND_Ammo,503,12},{dcitems.IKIND_Ammo,303,12},{dcitems.IKIND_Ammo,303,12},{dcitems.IKIND_Ammo,703,12},
		{dcitems.IKIND_Ammo,403,12},{dcitems.IKIND_Ammo,503,12},{dcitems.IKIND_Ammo,303,12},{dcitems.IKIND_Ammo,303,12},{dcitems.IKIND_Ammo,703,12},
		{dcitems.IKIND_Ammo,1203,12},{dcitems.IKIND_Ammo,503,12},{dcitems.IKIND_Ammo,403,12},{dcitems.IKIND_Ammo,103,12},{dcitems.IKIND_Ammo,703,12},
		{dcitems.IKIND_Ammo,1203,12},{dcitems.IKIND_Ammo,503,12},{dcitems.IKIND_Ammo,403,12},{dcitems.IKIND_Ammo,103,12},{dcitems.IKIND_Ammo,703,12},
		{dcitems.IKIND_Ammo,1203,12},{dcitems.IKIND_Ammo,503,12},{dcitems.IKIND_Ammo,303,12},{dcitems.IKIND_Ammo,1503,12},{dcitems.IKIND_Ammo,703,12}	},

		//{ ADVANCED GUNS }
	{	{dcitems.IKIND_Gun,7,5},{dcitems.IKIND_Gun,8,20},{dcitems.IKIND_Gun,10,20},{dcitems.IKIND_Gun,13,8},{dcitems.IKIND_Gun,15,10},
		{dcitems.IKIND_Gun,7,5},{dcitems.IKIND_Gun,8,20},{dcitems.IKIND_Gun,10,20},{dcitems.IKIND_Gun,13,8},{dcitems.IKIND_Gun,15,10},
		{dcitems.IKIND_Gun,7,5},{dcitems.IKIND_Gun,8,20},{dcitems.IKIND_Gun,10,20},{dcitems.IKIND_Gun,13,8},{dcitems.IKIND_Gun,15,10},
		{dcitems.IKIND_Gun,7,5},{dcitems.IKIND_Gun,8,20},{dcitems.IKIND_Gun,10,20},{dcitems.IKIND_Gun,13,8},{dcitems.IKIND_Gun,15,10},
		{dcitems.IKIND_Gun,7,5},{dcitems.IKIND_Gun,8,20},{dcitems.IKIND_Gun,10,20},{dcitems.IKIND_Gun,13,8},{dcitems.IKIND_Gun,15,10} },

		//{ ADVANCED WEAPONS }
	{	{dcitems.IKIND_Wep,11,1},{dcitems.IKIND_Wep,12,1},{dcitems.IKIND_Wep,7,1},{dcitems.IKIND_Wep,9,1},{dcitems.IKIND_Wep,10,1},
		{dcitems.IKIND_Wep,11,1},{dcitems.IKIND_Wep,12,1},{dcitems.IKIND_Wep,7,1},{dcitems.IKIND_Wep,9,1},{dcitems.IKIND_Wep,10,1},
		{dcitems.IKIND_Wep,11,1},{dcitems.IKIND_Wep,12,1},{dcitems.IKIND_Wep,7,1},{dcitems.IKIND_Wep,9,1},{dcitems.IKIND_Wep,10,1},
		{dcitems.IKIND_Wep,11,1},{dcitems.IKIND_Wep,12,1},{dcitems.IKIND_Wep,7,1},{dcitems.IKIND_Wep,9,1},{dcitems.IKIND_Wep,10,1},
		{dcitems.IKIND_Grenade,9,3},{dcitems.IKIND_Wep,12,1},{dcitems.IKIND_Wep,7,1},{dcitems.IKIND_Wep,9,1},{dcitems.IKIND_Wep,10,1} },

		//{ ADVANCED CLOTHING }
	{	{dcitems.IKIND_Armor,3,1},{dcitems.IKIND_Armor,8,1},{dcitems.IKIND_Armor,9,1},{dcitems.IKIND_Armor,10,1},{dcitems.IKIND_Armor,10,1},
		{dcitems.IKIND_Glove,2,1},{dcitems.IKIND_Armor,3,1},{dcitems.IKIND_Armor,3,1},{dcitems.IKIND_Armor,3,1},{dcitems.IKIND_Armor,11,1},
		{dcitems.IKIND_Glove,2,1},{dcitems.IKIND_Glove,2,1},{dcitems.IKIND_Glove,3,1},{dcitems.IKIND_Shoe,1,1},{dcitems.IKIND_Shoe,5,1},
		{dcitems.IKIND_Cap,1,1},{dcitems.IKIND_Cap,3,1},{dcitems.IKIND_Cap,3,1},{dcitems.IKIND_Cap,3,1},{dcitems.IKIND_Cap,6,1},
		{dcitems.IKIND_Glove,2,1},{dcitems.IKIND_Armor,3,1},{dcitems.IKIND_Cap,7,1},{dcitems.IKIND_Shoe,6,1},{dcitems.IKIND_Glove,4,1} }

	};

    public static void AttemptToIdentify(gamebook.Scenario SC, dcitems.DCItem I)
    {
        //{ The PC will try to figure out what this item is. }
        if (SC.PC != null)
        {
            //{ The PC must make a tech skill roll against the item's }
            //{ difficulcy number, as calculated above. }
            I.ID = rpgdice.RollStep(dcchars.PCIDSkill(SC.PC)) >= IDTarget(I);
        }
        else
        {
            I.ID = false;
        }
    }

    public static dcitems.DCItem GenerateItem(gamebook.Scenario SC, int TT)
    {
	    //{Generate a random item from chart TT.}
	    //{ The ScenarioPtr is used for the PC's tech skill, to see if items }
	    //{ start out identified or not. }

	    //{Decide which chart entry will be generated.}
	    int R = rpgdice.rng.Next(NumTChance) + 1;

        //{If the ICode listed in -1, jump instead to a different}
        //{item list, as indicated by the IKind field. Normally}
        //{item lists can only access item lists which occur after}
        //{them in the series; a check will be performed here to make}
        //{sure the procedure can't get stuck in an infinite loop.}
        dcitems.DCItem i;

        if (TTChart[TT - 1, R - 1, 1] == -1 && TTChart[TT - 1, R - 1, 0] > TT)
        {
            i = GenerateItem(SC, TTChart[TT - 1, R - 1, 0]);
        }
        else if (TTChart[TT - 1, R - 1, 1] != -1)
        {
            //{Allocate the item.}
            i = new dcitems.DCItem();

            i.ikind = TTChart[TT - 1, R - 1, 0];
            i.icode = TTChart[TT - 1, R - 1, 1];
            if (TTChart[TT - 1, R - 1, 2] == 0)
                i.charge = 0;
            else
                i.charge = rpgdice.rng.Next(TTChart[TT - 1, R - 1, 2]) + 1;
        }
        else
        {
            //{There's apparently an error in our random chart,}
            //{with one treasure list trying to access an earlier}
            //{one or somesuch. Let's make sure the error is noticed.}
            //{Drop 999 bananas.}
            i = new dcitems.DCItem();

			i.ikind = dcitems.IKIND_Food;
			i.icode = 6;
			i.charge = 999;
	    }

	    //{ Finally, see whether or not the item is identified by the PC. }
	    AttemptToIdentify( SC , i );

        return i;
    }

	static bool GoodSpot(gamebook.Scenario SC, int X, int Y)
    {
        //{Check spot X,Y and see if this is a good place to}
        //{stick a new monster. A good spot is a space, not}
        //{in the player's LOS, with no other monster currently}
        //{standing in it.}

        if (texmaps.TerrPass[SC.gb.map[X - 1, Y - 1].terr - 1] < 1)
            return false;
        else if (texmaps.TileLOS(SC.gb.POV, X, Y))
            return false;
        else if (texmodel.ModelPresent(SC.gb.mog, X, Y))
            return false;

        return true;
	}

    public static void WanderingCritters(gamebook.Scenario SC)
    {
        //{Add some random monsters to the map, if appropriate.}

	    //{Decide how many random generations we're gonna perform.}
	    int N = critters.NumberOfCritters(SC.CList);
        if (N >= MaxMonsters)
            return;

        int Gen = 0;
	    if (critters.NumberOfCritters(SC.CList) < MaxMonsters / 2)
            Gen = rpgtext.CHART_NumGenerations + rpgdice.rng.Next(rpgtext.CHART_NumGenerations);
	    else
            Gen = rpgdice.rng.Next(rpgtext.CHART_NumGenerations) + 1;

        while (Gen > 0)
        {
            Gen -= 1;

		    //{Check to see if there is any room for more monsters.}
		    //{The more monsters we have, the less likely we are to add more.}
		    if (critters.NumberOfCritters(SC.CList) < rpgdice.rng.Next(MaxMonsters / 2) + (MaxMonsters / 2) + 1)
            {
                //{Roll on the random monster chart.}
                //{ First decide what chart to use. Either pick a chart }
                //{ based on PC level, or use the "signature chart" for }
                //{ the currrent location. }
                int Chart;
                if (rpgdice.rng.Next(3) != 1)
                {
                    //{ Pick chart based on level. }
                    Chart = (rpgdice.rng.Next(SC.PC.lvl) / 3) + 1;
                }
			    else
                {
				    //{ The Signature Charts start at 1 and go down. }
				    Chart = 2 - SC.Loc_Number;
			    }

                //{ Range check the selected chart... }
                if (Chart > NumWChart)
                    Chart = NumWChart;
                else if (Chart < 1)
                    Chart = 1;

			    //{ Roll the Entry and Number. }
			    int E = rpgdice.rng.Next(NumWCT);
			    N = rpgdice.rng.Next(WanderChart[Chart - 1, E, 1]) + 1;

			    //{Decide upon a nice place to put our critters.}
			    //{Select an origin spot - the generated critters will be centered here.}
			    int X0 = rpgdice.rng.Next(texmodel.XMax)+1;
			    int Y0 = rpgdice.rng.Next(texmodel.YMax)+1;

			    //{We're gonna give up if we can't find an appropriate}
			    //{tile after 10,000 tries.}
			    int tries = 0;
			    while (texmaps.TerrPass[SC.gb.map[X0 - 1,Y0 - 1].terr - 1] < 1 &&  tries < 10000)
                {
                    X0 += 1;
                    tries += 0;

                    if (X0 > texmodel.XMax)
                    {
                        Y0 += 1;
                        X0 = 0;
                    }
                    if (Y0 > texmodel.YMax)
                    {
                        Y0 = 1;
                    }
			    }

			    for (int t = 1; t <= N; t++)
                {
				    //{Starting position for the swarm is the origin determined earlier.}
				    int X = X0;
				    int Y = Y0;

				    //{Check to see if this is an appropriate spot.}
				    tries = 0;
				    while (tries < 10 && !GoodSpot(SC, X,Y))
                    {
                        tries += 1;
                        X += rpgdice.rng.Next(3) - rpgdice.rng.Next(3);
                        if (X < 1)
                            X = 1;
                        else if (X > texmodel.XMax)
                            X = texmodel.XMax;

					    Y += rpgdice.rng.Next(3) - rpgdice.rng.Next(3);
                        if (Y < 1)
                            Y = 1;
                        else if (Y > texmodel.YMax)
                            Y = texmodel.YMax;
				    }

				    //{If we have a good spot, render the monster.}
				    //{Otherwise, just forget it.}
				    if (GoodSpot(SC, X, Y))
                    {
					    critters.Critter C = critters.AddCritter(ref SC.CList, SC.gb, WanderChart[Chart - 1, E, 0], X, Y);

					    //{Check to see whether the monster is equipped with a weapon.}
					    if (C != null && critters.MonMan[C.crit - 1].EType > 0 && rpgdice.rng.Next(100) < critters.MonMan[C.crit - 1].EChance)
                        {
						    C.Eqp = GenerateItem(SC, critters.MonMan[C.crit - 1].EType);
					    }
				    }
			    }
		    }
	    }
    }

    static int IDTarget(dcitems.DCItem I)
    {
        //{ Examine item I and return the difficulcy number needed to identify it. }
        switch (I.ikind)
        {
            case dcitems.IKIND_Gun:
            case dcitems.IKIND_Wep:
                return 12;
            case dcitems.IKIND_Cap:
            case dcitems.IKIND_Armor:
            case dcitems.IKIND_Glove:
            case dcitems.IKIND_Shoe:
                return 9;
            case dcitems.IKIND_Food:
                //{ Pills are notoriously difficult to ID. }
                if (dcitems.CFood[I.icode - 1].fk == 2)
                    return 16;
                return 5;
            case dcitems.IKIND_Ammo:
            case dcitems.IKIND_Grenade:
                return 7;
        }

        return 10;
    }

}