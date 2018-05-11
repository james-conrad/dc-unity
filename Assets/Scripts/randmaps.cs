using System;

public class randmaps
{

    public const int SceneSize = 12;

    public class Room /*This is the description for one room.*/
    {
        public int Style;
        public int X, Y, W, H; /*X,Y,Width,Height*/
        public int Floor, Wall;    /*What terr type should be used for the floor and the wall.*/
        public Room next;
    }

    public class RStyle /*This describes a type of room*/
    {
        public RStyle(string Name, int Floor, int Wall, int SP, int WC, int WR, int HC, int HR)
        {
            this.Name = Name;
            this.Floor = Floor;
            this.Wall = Wall;
            this.SP = SP;
            this.WC = WC;
            this.WR = WR;
            this.HC = HC;
            this.HR = HR;
        }

        public string Name;
        public int Floor, Wall;
        public int SP; /*% of halls which should be service tunnels*/
        public int WC, WR, HC, HR; /*Constants for determining size.*/
    }

    public const int EmptyTerrain = 6;

    /*Direction 1 corresponds to 3 O'Clock; 2 is 6 O'Clock, 3 is 9 O'Clock, and 4 is 12 O'Clock.*/
    /*This ordering should make Right/Left rotations easier.*/
    public static int[,] AngDir = new int[6, 2]
    {
        {0,-1},{1,0},{0,1},{-1,0},{0,-1},{1,0}
    };

    public const int RNormStyle = 1;
    public const int REngineWorks = 2;

    public const int NumStyle = 21;
    /*And now, our interior decoration manual.*/
    public static RStyle[] IntDec = new RStyle[NumStyle]
    {
        new RStyle("Normal Room",
            Floor: 2, Wall: 3, SP: 15,
            WC: 5, WR: 20, HC: 5, HR: 12    ),
        new RStyle("Engine Works",
            Floor: 5, Wall: 6, SP: 100,
            WC: 5, WR: 5, HC: 5, HR: 5  ),
        new RStyle("Lounge",
            Floor: 15, Wall: 3, SP: 20,
            WC: 12, WR: 12, HC: 7, HR: 10   ),
        new RStyle("Security Center",
            Floor: 2, Wall: 3, SP: 0,
            WC: 8, WR: 12, HC: 5, HR: 10    ),
        new RStyle("Storage Room",
            Floor: 5, Wall: 3, SP: 25,
            WC: 20, WR: 10, HC: 10, HR: 10  ),

        new RStyle("Obstructed room",
            Floor: 2, Wall: 3, SP: 45,
            WC: 5, WR: 20, HC: 5, HR: 20    ),
        new RStyle("Shuttle Bay",
            Floor: 2, Wall: 3, SP: 30,
            WC: 20, WR: 12, HC: 20, HR: 12  ),
        new RStyle("Transitway Left",
            Floor: 2, Wall: 3, SP: 5,
            WC: 9, WR: 1, HC: 6, HR: 3  ),
        new RStyle("Transitway Right",
            Floor: 2, Wall: 3, SP: 5,
            WC: 9, WR: 1, HC: 6, HR: 3  ),
        new RStyle("Residential Block",
            Floor: 2, Wall: 3, SP: 10,
            WC: 17, WR: 18, HC: 15, HR: 3   ),

        new RStyle("Andros Guero Quarters",
            Floor: 2, Wall: 3, SP: 10,
            WC: 17, WR: 18, HC: 15, HR: 3   ),
        new RStyle("Chapel",
            Floor: 40, Wall: 3, SP: 2,
            WC: 12, WR: 7, HC: 12, HR: 12   ),
        new RStyle("Reliquary",
            Floor: 2, Wall: 3, SP: 0,
            WC: 12, WR: 18, HC: 12, HR: 12  ),
        new RStyle("Medical Center",
            Floor: 15, Wall: 3, SP: 25,
            WC: 16, WR: 16, HC: 8, HR: 5    ),
        new RStyle("Gravesite",
            Floor: 40, Wall: 3, SP: 3,
            WC: 15, WR: 32, HC: 8, HR: 25   ),

        new RStyle("Computer Control Center",
            Floor: 5, Wall: 6, SP: 100,
            WC: 5, WR: 5, HC: 5, HR: 5  ),
        new RStyle("Cryogenics Lab",
            Floor: 5, Wall: 3, SP: 70,
            WC: 12, WR: 8, HC: 15, HR: 20   ),
        new RStyle("Transitway Down",
            Floor: 2, Wall: 3, SP: 5,
            WC: 9, WR: 1, HC: 6, HR: 3  ),
        new RStyle("Transitway Up",
            Floor: 2, Wall: 3, SP: 5,
            WC: 9, WR: 1, HC: 6, HR: 3  ),
        new RStyle("Museum",
            Floor: 15, Wall: 3, SP: 2,
            WC: 21, WR: 5, HC: 19, HR: 3),

        new RStyle("DesCartes Control Center",
            Floor: 5, Wall: 6, SP: 100,
            WC: 10, WR: 5, HC: 10, HR: 5),
    };

    public static int[,] RocketShip = new int[SceneSize, SceneSize]
    {
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        { 27,25,25,25,25,24, 0, 0, 0, 0, 0, 0   },
        {  0, 0,20,20,20,20, 8,24,28, 0, 0, 0   },
        {  0,27,26,29,29,26,29,20,25,25,20, 0   },
        {  0,27,20,29,29, 8,29,29,29,21,22,23   },
        {  0,27,26,29,29,26,29,20,25,25,20, 0   },
        {  0, 0,20,20,20,20, 8,24,28, 0, 0, 0   },
        { 27,25,25,25,25,24, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   }
    };

    public static int[,] TransitChassis = new int[SceneSize, SceneSize]
    {
        { 36,36,36, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        { 30, 0,31, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   }
    };

    public static int[,] Apartment = new int[SceneSize, SceneSize]
    {
        { 16,16,16, 8,16,16,16, 0, 0, 0, 0, 0   },
        { 16,15,15,15,15,15,16, 0, 0, 0, 0, 0   },
        { 16,15,15,15,14,15,16, 0, 0, 0, 0, 0   },
        { 16,15,15,14,37,14,16, 0, 0, 0, 0, 0   },
        { 16,15,15,15,14,15,16, 0, 0, 0, 0, 0   },
        { 16,16,16,15,15,15,16, 0, 0, 0, 0, 0   },
        { 16,38, 8,15,15,19,16, 0, 0, 0, 0, 0   },
        { 16,16,16,16,16,16,16, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   }
    };

    public static int[,] Capsule = new int[SceneSize, SceneSize]
    {
        { 48,49,50, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        { 47,45,47, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        { 46,12,46, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   },
        {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0   }
    };


    public static string[] Level_Contents = new string[gamebook.Num_Levels + 1]
    {
        "8 9",
        " 7 10 14  8 12  9 11  5",
        " 4  8 15  9 12 15 16 17",
        " 8  9 18",
        " 8  5  9",
        " 8  9",
        " 4  8  4  9",
        " 8  9 19",
        "12  8 20 10 13  9 12 21"
    };

    static bool Odd(int num)
    {
        return num % 2 == 1;
    }

    public static void RectFill(texmaps.GameBoard gb, int X1, int Y1, int X2, int Y2, int terr)
    {
        /*Fill the given rectangular area with terrain type terr*/
        for (int x = X1; x <= X2; ++x)
            for (int y = Y1; y <= Y2; ++y)
                gb.map[x - 1, y - 1].terr = terr;
    }

    public static void RoomFill(texmaps.GameBoard gb, int X1, int Y1, int X2, int Y2, int W, int F)
    {
        /*Create a room, with wall type W and floor type F.*/
        for (int x = X1; x <= X2; ++x)
            for (int y = Y1; y <= Y2; ++y)
                if (x == X1 || x == X2 || y == Y1 || y == Y2)
                    gb.map[x - 1, y - 1].terr = W;
                else
                    gb.map[x - 1, y - 1].terr = F;
    }

    public static Room AddRoom(ref Room LList)
    {
        /*Add a new element to the end of LList.*/

        /*Allocate memory for our new element.*/
        Room it = new Room();

        /*Initialize values.*/
        it.next = null;
        it.Style = 0;
        it.X = -1;
        it.Y = -1;
        it.Floor = 2;
        it.Wall = 3;

        /*Attach IT to the list.*/
        if (LList == null)
            LList = it;
        else
            LastRoom(LList).next = it;

        /*Return a pointer to the new element.*/
        return it;
    }

    public static void RandomLevel(gamebook.Scenario SC, Room R)
    {
        /*Generate a random map for use in DeadCold.*/
        SC.gb = texmaps.NewBoard();
        SC.ig = new dcitems.IGrid();

        /*Fill every square with the basic wall.*/
        RectFill(SC.gb, 1, 1, texmodel.XMax, texmodel.YMax, EmptyTerrain);

        int t;
        /*Add some rooms to the basic list.*/
        for (t = 1; t <= 5 + rpgdice.Random(10); ++t)
        {
            NewRoom(ref R);
        }

        /*Scramble the order of the rooms.*/

        /*Place the remaining rooms on the map.*/
        Room RTemp = R;
        while (RTemp != null)
        {
            PlaceRoom(SC, RTemp);
            RTemp = RTemp.next;
        }

        /*Fill in any empty spots with extra stuff.*/
        GapFiller(SC);

        /*Connect the rooms with halls.*/
        RTemp = R;
        while (RTemp != null)
        {
            ConnectRoom(SC.gb, RTemp, NextRoom(R, RTemp));
            RTemp = RTemp.next;
        }

        /* Add some traps. */
        for (t = 1; t <= 100; ++t)
        {
            int X = rpgdice.Random(texmodel.XMax) + 1;
            int Y = rpgdice.Random(texmodel.YMax) + 1;
            if (texmaps.GetTerr(SC.gb, X, Y) == 2)
            {
                SC.gb.map[X - 1, Y - 1].trap = -1;
            }
            else if (texmaps.GetTerr(SC.gb, X, Y) == texmaps.Crawlspace)
            {
                SC.gb.map[X - 1, Y - 1].trap = -2;
            }
        }

        /*Add some stains and other details to the floor. Maybe.*/
        for (t = 1; t <= 1000; ++t)
        {
            int X = rpgdice.Random(texmodel.XMax) + 1;
            int Y = rpgdice.Random(texmodel.YMax) + 1;
            if (texmaps.GetTerr(SC.gb, X, Y) == 2)
                SC.gb.map[X - 1, Y - 1].terr = 10;
            else if (texmaps.GetTerr(SC.gb, X, Y) == texmaps.Crawlspace)
            {
                if (rpgdice.Random(20) == 7) PlaceItemCache(SC, X, Y);
                else if (rpgdice.Random(10) == 7) RenderCorner(SC.gb, X, Y);
                else
                {
                    HallStyle = ServTunnel;
                    RenderWTunnel(SC.gb, X, Y, rpgdice.Random(4) + 1);
                }
            }
            else if (texmaps.GetTerr(SC.gb, X, Y) == EmptyTerrain)
            {
                HallStyle = ServTunnel;
                RenderWTunnel(SC.gb, X, Y, rpgdice.Random(4) + 1);
            }
        }
    }

    public static void GenerateCurrentLevel(gamebook.Scenario SC)
    {
        /* Generate a random map for the level which is indicated as being the */
        /* current one. */

        /* Start by determining the level plan for this module. */
        string LevelPlan;
        if (SC.Loc_Number < 1 || SC.Loc_Number > gamebook.Num_Levels)
        {
            LevelPlan = Level_Contents[0];
        }
        else
        {
            LevelPlan = Level_Contents[SC.Loc_Number];
        }

        /* Generate rooms for each room indicated in the plans. */
        Room RList = null;
        while (LevelPlan != "")
        {
            int RID = texutil.ExtractValue(ref LevelPlan);
            Room R = AddRoom(ref RList);
            R.Style = RID;
            InitRoom(R);
        }

        /* Actually call the random level procedure to put it all together. */
        RandomLevel(SC, RList);
    }

    public static void GotoLevel(gamebook.Scenario SC, int N, int Entry_Terrain)
    {
        /* - Freeze current level, if one exists */
        /* - Set Loc_Number field to the requested number */
        /* - If requested level exists, unfreeze it */
        /* - If requested level is empty, generate it */
        /* - Deploy a PC model on the game board */

        /* Check the current level to see if it needs to be frozen. */
        if (SC.gb != null)
        {
            /* If it exists, freeze it. */
            FreezeCurrentLevel(SC);
        }

        /* Set the location number. */
        SC.Loc_Number = N;

        /* Unfreeze the level, if it exists. */
        if (N >= 1 && N <= gamebook.Num_Levels && SC.Frozen_Levels[N - 1].gb != null)
        {
            UnfreezeLevel(SC, N);
        }
        else
        {
            /* It doesn't exist. Generate it. */
            GenerateCurrentLevel(SC);
        }

        /* Deploy a model for the PC. Start the model on the first */
        /* tile found with the specified terrain. If no tile with */
        /* this terrain is found, we're in serious trouble. */
        int X = 1;
        int Y = 1;
        do
        {
            X = X + 1;
            if (X > texmodel.XMax)
            {
                X = 1;
                Y = Y + 1;
            }
        }
        while (SC.gb.map[X - 1, Y - 1].terr != Entry_Terrain && texmaps.OnTheMap(X, Y));

        if (!texmaps.OnTheMap(X, Y))
        {
            X = rpgdice.Random(texmodel.XMax) + 1;
            Y = rpgdice.Random(texmodel.YMax) + 1;
        }

        /* Set the particulars for the player's model. */
        texmodel.Model m = texmodel.AddModel(ref SC.gb.mlist, SC.gb.mog, '@', Crt.Color.LightGreen, Crt.Color.White, false, X, Y, 1);
        SC.gb.POV.m = m;
        SC.PC.m = m;
        SC.gb.POV.range = dcchars.PCVisionRange(SC.PC);
        texmaps.RecenterPOV(SC.gb);
        texmaps.UpdatePOV(SC.gb.POV, SC.gb);
        texmaps.ApplyPOV(SC.gb.POV, SC.gb);
        texmaps.DisplayMap(SC.gb);

        /* Add the START trigger. */
        gamebook.SetTrigger(SC, "START");
    }

    static bool ItsAFloor = false;
    const int NormHall = 1;
    const int ServTunnel = 2;
    static int HallStyle = NormHall;
    const int WallStyle = 3;
    const int StorageRoomItem = 20; /*% chance of item in each pile.*/
    const int StorageRoomMaxRolls = 3;  /*Max # of items to be generated per pile.*/
    const int StorageRoomRobot = 8; /*% chance of a robot, if no item.*/
    const int SecurityCellItem = 88;    /*% chance of a stockpile in a security cell.*/
    const int SecurityCellNum = 4;  /*Max number of items per cell.*/
    const int SecurityCellZombie = 35; /*% chance of a zombie, if no stockpile.*/
    const int CryptItem = 25;       /*% chance of a stockpile in a crypt tile.*/
    const int CryptNum = 3;     /*Max number of items per cell.*/
    const int CryptCritter = 70;    /*% chance of a zombie, if no stockpile.*/



    static void AddScenery(texmaps.GameBoard gb, int X0, int Y0, int[,] Scene, bool Reveal)
    {
        /*Add the scenery to the map*/
        for (int X = 1; X <= SceneSize; ++X)
        {
            for (int Y = 1; Y <= SceneSize; ++Y)
            {
                if (texmaps.OnTheMap(X + X0 - 1, Y + Y0 - 1))
                {
                    /*Apparently, I made a boo-boo when defining things, so now*/
                    /*Scenery data is stored with its coordinates as Y,X instead of X,Y.*/
                    if (Scene[Y - 1, X - 1] != 0)
                    {
                        gb.map[X + X0 - 1 - 1, Y + Y0 - 1 - 1].terr = Scene[Y - 1, X - 1];
                        if (Reveal)
                            gb.map[X + X0 - 1 - 1, Y + Y0 - 1 - 1].visible = true;
                    }
                }
            }
        }
    }


    static Room LastRoom(Room LList)
    {
        /*Search through the linked list, and return the last element.*/
        /*If LList is empty, return null.*/
        if (LList != null)
            while (LList.next != null)
                LList = LList.next;

        return LList;
    }

    static Room NextRoom(Room RList, Room R)
    {
        /*Return the address of the next room in the list.*/
        /*If we're already looking at the last room, loop around*/
        /*to the first one again.*/
        Room it = R.next;
        if (it == null)
            it = RList;

        return it;
    }

    static void InitRoom(Room R)
    {
        /*Given room R, whose Style field has been filled out,*/
        /*set appropriate values for each of the other fields.*/
        R.W = IntDec[R.Style - 1].WC + rpgdice.Random(IntDec[R.Style - 1].WR);
        R.H = IntDec[R.Style - 1].HC + rpgdice.Random(IntDec[R.Style - 1].HR);
        R.Wall = IntDec[R.Style - 1].Wall;
        R.Floor = IntDec[R.Style - 1].Floor;

        /*Special check- make sure that a lounge area will look pretty!*/
        if (R.Style == 3 || R.Style == 12)
        {
            if (!Odd(R.H))
                R.H += 1;
            if (!Odd(R.W))
                R.W += 1;
        }
    }

    static Room NewRoom(ref Room RList)
    {
        /*Add a random new room to the list.*/
        Room R = AddRoom(ref RList);

        /*Decide upon the style of this room. For now,*/
        /*just pick one at random.*/
        R.Style = rpgdice.Random(6) + 1;

        InitRoom(R);

        return R;
    }

    static bool AreaClear(texmaps.GameBoard gb, int X1, int Y1, int W, int H)
    {
        /*Search the area defined by X,Y,W and H on the game map.*/
        /*Return true if the area consists of blank terrain.*/
        /*Return false if there's anything else there.*/

        /*Initialize variables.*/
        int X2 = X1 + W - 1;
        int Y2 = Y1 + H - 1;

        for (int X = X1; X <= X2; ++X)
        {
            for (int Y = Y1; Y <= Y2; ++Y)
            {
                if (gb.map[X - 1, Y - 1].terr != EmptyTerrain)
                    return false;
            }
        }

        return true;
    }

    static bool IsASpace(texmaps.GameBoard gb, int X, int Y)
    {
        /*Check the space indicated. Return true if it is passable*/
        /*terrain, false otherwise.*/
        if (!texmaps.OnTheMap(X, Y))
            return true;

        if (HallStyle != ServTunnel && gb.map[X - 1, Y - 1].terr == texmaps.Crawlspace)
            return false;
        else if (texmaps.TerrPass[gb.map[X - 1, Y - 1].terr - 1] > 0)
            return true;
        else if (gb.map[X - 1, Y - 1].terr == texmaps.ClosedDoor || gb.map[X - 1, Y - 1].terr == texmaps.ClosedServicePanel || gb.map[X - 1, Y - 1].terr == texmaps.HiddenServicePanel)
            return true;

        return false;
    }

    static bool IsAWall(texmaps.GameBoard gb, int X, int Y)
    {
        /*Check the space indicated. Return true if it is a plain*/
        /*wall, false otherwise.*/
        if (texmaps.TerrPass[gb.map[X - 1, Y - 1].terr - 1] == 0)
            return true;

        return false;
    }


    static int PathClear(texmaps.GameBoard gb, int X, int Y, int D, int L)
    {
        /*Check the path indicated by the details given. In particular,*/
        /*check to see whether or not this path will intersect with*/
        /*a room or corridor in a bad way.*/
        int r = 1;
        int t = 1;
        for (t = 1; t <= L; ++t)
        {
            if (IsAWall(gb, X, Y))
            {
                if (IsASpace(gb, X + AngDir[D + 1, 0], Y + AngDir[D + 1, 1]))
                {
                    r = 1;
                    break;
                }
                if (IsASpace(gb, X + AngDir[D - 1, 0], Y + AngDir[D - 1, 1]))
                {
                    r = -1;
                    break;
                }
            }
            X = X + AngDir[D, 0];
            Y = Y + AngDir[D, 1];
        }

        return t * r;
    }

    static void PlotWall(texmaps.GameBoard gb, int X, int Y)
    {
        /*Draw a wall in the current wall style. The rule is as*/
        /*follows: If intersecting empty terrain or default wall*/
        /*type, draw the wall as normal. If intersecting a crawlspace*/
        /*then set down a texmaps.HiddenServicePanel instead of a wall.*/
        /*If intersecting any other kind of space, don't do anything.*/

        if (IsAWall(gb, X, Y))
            gb.map[X - 1, Y - 1].terr = WallStyle;
        else if (gb.map[X - 1, Y - 1].terr == texmaps.Crawlspace)
            gb.map[X - 1, Y - 1].terr = texmaps.HiddenServicePanel;
    }

    static void PlotDoor(texmaps.GameBoard gb, int X, int Y)
    {
        /*Plot a door at location X,Y. Select one of the possible*/
        /*door types randomly. It may be open or closed, or even*/
        /*a hidden service panel.*/

        /*There's a very small chance that instead of a regular door,*/
        /*we'll plot a service panel instead.*/
        if (rpgdice.Random(100) == 23)
        {
            /*Plot service panel.*/
            if (rpgdice.Random(5) == 1)
            {
                if (rpgdice.Random(3) == 1)
                {
                    gb.map[X - 1, Y - 1].terr = texmaps.OpenServicePanel;
                }
                else
                {
                    gb.map[X - 1, Y - 1].terr = texmaps.ClosedServicePanel;
                }
            }
            else
            {
                gb.map[X - 1, Y - 1].terr = texmaps.HiddenServicePanel;
            }
        }
        else
        {
            /*Plot door.*/
            if (rpgdice.Random(3) == 1)
            {
                /*Open door.*/
                gb.map[X - 1, Y - 1].terr = texmaps.OpenDoor;
            }
            else
            {
                /*Closed door.*/
                gb.map[X - 1, Y - 1].terr = texmaps.ClosedDoor;
            }
        }
    }

    static void RenderCorner(texmaps.GameBoard gb, int X, int Y)
    {
        /*Render a corner, in the current wall style.*/
        if (HallStyle == ServTunnel)
            return;

        /*Make sure that our center point is within bounds.*/
        if (X < 2)
            X = 2;
        else if (X >= texmodel.XMax)
            X = texmodel.XMax - 1;
        if (Y < 2)
            Y = 2;
        else if (Y >= texmodel.YMax)
            Y = texmodel.YMax - 1;

        /*Do the rendering. Just replace all walls in the vicinity*/
        /*with the wall style currently selected.*/
        for (int XX = X - 1; XX <= X + 1; ++XX)
            for (int YY = Y - 1; YY <= Y + 1; ++YY)
                PlotWall(gb, XX, YY);
    }

    static void RenderCorridorSection(texmaps.GameBoard gb, int X, int Y, int D)
    {
        /*Render a single block of corridor.*/

        /*Plot the floor, if appropriate.*/
        if (HallStyle == NormHall)
        {
            if (texmaps.TerrPass[gb.map[X - 1, Y - 1].terr - 1] == 0 || gb.map[X - 1, Y - 1].terr == texmaps.Crawlspace)
            {
                /*If we have a wall or a crawlspace, draw a corridor through it.*/
                if (ItsAFloor && rpgdice.Random(2) == 1)
                    PlotDoor(gb, X, Y);
                else
                {
                    if (texmaps.TerrPass[gb.map[X + AngDir[D, 0] - 1, Y + AngDir[D, 1] - 1].terr - 1] > 0 && rpgdice.Random(2) == 1)
                        PlotDoor(gb, X, Y);
                    else
                        gb.map[X - 1, Y - 1].terr = 2;
                }
                ItsAFloor = false;
            }
            else if (texmaps.TerrPass[gb.map[X - 1, Y - 1].terr - 1] < 0)
            {
                ItsAFloor = false;
            }
            else
            {
                ItsAFloor = true;
            }
        }
        else if (HallStyle == ServTunnel)
        {
            /*Here are the rules. If the current square is a*/
            /*standard wall, change it to a texmaps.HiddenServicePanel.*/
            /*If it's empty terrain, draw a crawlspace.*/
            if (gb.map[X - 1, Y - 1].terr == EmptyTerrain)
            {
                gb.map[X - 1, Y - 1].terr = texmaps.Crawlspace;
            }
            else if (texmaps.TerrPass[gb.map[X - 1, Y - 1].terr - 1] == 0)
            {
                gb.map[X - 1, Y - 1].terr = texmaps.HiddenServicePanel;
            }
        }

        if (HallStyle != ServTunnel)
        {
            /*Service tunnels don't get walls surrounding them.*/
            PlotWall(gb, X + AngDir[D + 1, 0], Y + AngDir[D + 1, 1]);
            PlotWall(gb, X + AngDir[D - 1, 0], Y + AngDir[D - 1, 1]);
        }
    }

    static void RenderCorridor(texmaps.GameBoard gb, int X1, int Y1, int D, int L)
    {
        /*Draw a corridor onto the map, starting at X,Y and*/
        /*proceeding L units in direction D.*/
        int X = X1;
        int Y = Y1;
        ItsAFloor = false;

        for (int t = 1; t <= L; ++t)
        {
            RenderCorridorSection(gb, X, Y, D);
            X = X + AngDir[D, 0];
            Y = Y + AngDir[D, 1];
        }

        /*Finish off the corner of the hall.*/
        RenderCorner(gb, X - AngDir[D, 0], Y - AngDir[D, 1]);
    }

    static void ChangeDirection(texmaps.GameBoard gb, int X, int Y, ref int D)
    {
        D += rpgdice.Random(2) - rpgdice.Random(2);
        if (D < 1)
            D = 4;
        else if (D > 4)
            D = 1;

        RenderCorner(gb, X, Y);
    }

    static void RenderWTunnel(texmaps.GameBoard gb, int X, int Y, int D)
    {
        /*Start rendering a worm tunnel. Yay!*/
        /*This tunnel has no preset length or destination.*/
        /*It keeps going until either it runs out of map or it*/
        /*intersects a room/corridor.*/

        bool KeepGoing = true;

        /*Because WTunnels will be set up on room walls, initialize*/
        /*this value to true.*/
        ItsAFloor = true;

        /*Check- if this is not a good place to start doing a WTunnel,*/
        /*exit without rendering anything.*/
        if (X == 1 || X == texmodel.XMax || Y == 1 || Y == texmodel.YMax)
        {
            KeepGoing = false;
        }
        else if (IsASpace(gb, X, Y) || IsASpace(gb, X + AngDir[D + 1, 0], Y + AngDir[D + 1, 1]) || IsASpace(gb, X + AngDir[D - 1, 0], Y + AngDir[D - 1, 1]))
        {
            KeepGoing = false;
        }

        int t = 0;
        while (KeepGoing)
        {
            t += 1;
            if (IsASpace(gb, X, Y) || IsASpace(gb, X + AngDir[D + 1, 0], Y + AngDir[D + 1, 1]) || IsASpace(gb, X + AngDir[D - 1, 0], Y + AngDir[D - 1, 1]))
            {
                KeepGoing = false;
            }
            else if (rpgdice.Random(1000) == 0)
            {
                KeepGoing = false;
            }

            RenderCorridorSection(gb, X, Y, D);

            /*Maybe change direction now.*/
            /*First, check for the edge of the map.*/
            if (X + AngDir[D, 0] == 1 || X + AngDir[D, 0] == texmodel.XMax || Y + AngDir[D, 1] == 1 || Y + AngDir[D, 1] == texmodel.YMax)
            {
                ChangeDirection(gb, X, Y, ref D);
            }
            else if (rpgdice.Random(33) == 12 && t > 1)
            {
                ChangeDirection(gb, X, Y, ref D);
            }

            /*Move to the next square.*/
            X = X + AngDir[D, 0];
            Y = Y + AngDir[D, 1];

            /*Check for the edge of the map*/
            if (X == 1 || X == texmodel.XMax || Y == 1 || Y == texmodel.YMax)
            {
                KeepGoing = false;
            }
        }

        RenderCorner(gb, X, Y);
    }

    static void AddWTunnels(texmaps.GameBoard gb, Room R, int N)
    {
        /*Attempt to add dN wormtunnels to room R.*/

        int E = rpgdice.Dice(N);

        for (int t = 1; t <= E; ++t)
        {
            /*Determine the starting direction of this tunnel.*/
            /*This will decide what wall it's starting in.*/
            int D = rpgdice.Random(4) + 1;
            int X = R.X;
            int Y = R.Y;

            if (D == 1)
            {
                Y = Y + rpgdice.Random(R.H - 2) + 1;
                X = X + R.W - 1;
            }
            else if (D == 3)
            {
                Y = Y + rpgdice.Random(R.H - 2) + 1;
            }
            else if (D == 2)
            {
                X = X + rpgdice.Random(R.W - 2) + 1;
                Y = Y + R.H - 1;
            }
            else
            {
                X = X + rpgdice.Random(R.W - 2) + 1;
            }

            /*Determine the style of the tunnel.*/
            if (rpgdice.Random(100) + 1 <= IntDec[R.Style - 1].SP)
                HallStyle = ServTunnel;
            else
                HallStyle = NormHall;

            RenderWTunnel(gb, X, Y, D);
        }
    }

    static void ConnectRoom(texmaps.GameBoard gb, Room R1, Room R2)
    {
        /*Connect R1 to R2 by means of a hallway.*/

        /*Before we start on the connection tunnels, let's add some*/
        /*random tunnels to the room.*/
        AddWTunnels(gb, R1, 4);

        /*Select the origin points of the corridor in both rooms.*/
        int X1 = rpgdice.Random(R1.W - 2) + R1.X + 1;
        int Y1 = rpgdice.Random(R1.H - 2) + R1.Y + 1;
        int X2 = rpgdice.Random(R2.W - 2) + R2.X + 1;
        int Y2 = rpgdice.Random(R2.H - 2) + R2.Y + 1;

        int DH = 3;
        if (X2 > X1)
            DH = 1;

        int T = 0;
        while (Math.Abs(PathClear(gb, X1, Y1, DH, Math.Abs(X2 - X1) + 1)) != Math.Abs(X2 - X1) + 1 && T < 30)
        {
            Y1 = rpgdice.Random(R1.H - 2) + R1.Y + 1;
            T += 1;
        }

        int DV = 4;
        if (Y2 > Y1)
            DV = 2;

        T = 0;
        while (Math.Abs(PathClear(gb, X2, Y1, DV, Math.Abs(Y2 - Y1) + 1)) != Math.Abs(Y2 - Y1) + 1 && T < 30)
        {
            X2 = rpgdice.Random(R2.W - 2) + R2.X + 1;
            T += 1;
        }

        /*These halls, which connect the key rooms, are more likely*/
        /*to be service tunnels than they are to be normal hallways.*/
        if (R1.Style == REngineWorks || R2.Style == REngineWorks)
            HallStyle = ServTunnel;
        else if (rpgdice.Random(100) + 1 > IntDec[R1.Style - 1].SP)
            HallStyle = NormHall;
        else
            HallStyle = ServTunnel;

        /*Horizontal Tunnel*/
        RenderCorridor(gb, X1, Y1, DH, Math.Abs(X2 - X1) + 1);

        /*Vertical Tunnel*/
        RenderCorridor(gb, X2, Y1, DV, Math.Abs(Y2 - Y1) + 1);
    }

    /* This array tells what treasure type chart to use. */
    static int[] TTL =
    {
        charts.TType_AllMedicene, charts.TType_AllMedicene, charts.TType_AllAmmo,
        charts.TType_AllAmmo, charts.TType_AllAmmo, charts.TType_BasicGuns,
        charts.TType_BasicWeps, charts.TType_TechnoItems, charts.TType_SpaceGear,
        charts.TType_StorageRoom
    };

    static void PlaceItemCache(gamebook.Scenario SC, int X, int Y)
    {
        /* Place a number of random, highly valuable items in this spot. */

        /* Determine how many items to generate. */
        int N = rpgdice.Random(10) + 1;

        /* Place N items. */
        for (int T = 1; T <= N; ++T)
        {
            dcitems.DCItem I = charts.GenerateItem(SC, TTL[rpgdice.Random(10)]);
            dcitems.PlaceDCItem(SC.gb, SC.ig, I, X, Y);
        }
    }

    static void DetailLounge(gamebook.Scenario SC, Room R)
    {
        /*Add details needed for a lounge area.*/

        /*Determine how many chairs to add, horiz and vert.*/
        int NX = (R.W - 3) / 2;
        int NY = (R.H - 3) / 2;

        for (int t = 1; t <= NX; ++t)
        {
            SC.gb.map[R.X + t * 2 - 1, R.Y + 2 - 1].terr = texmaps.Chair;
            SC.gb.map[R.X + t * 2 - 1, R.Y + 2 * NY - 1].terr = texmaps.Chair;
        }
        for (int t = 1; t <= NY; ++t)
        {
            SC.gb.map[R.X + 2 - 1, R.Y + t * 2 - 1].terr = texmaps.Chair;
            SC.gb.map[R.X + 2 * NX - 1, R.Y + t * 2 - 1].terr = texmaps.Chair;
        }
    }


    static int[] SecTrap = { 3, 2, 2, 2, 2, 2, 2, 3, 3, 3 };

    static void SecCell(gamebook.Scenario SC, Room R, int X, int Y, int D)
    {
        /*Draw a 3 x 3 security cell at location X,Y.*/
        RoomFill(SC.gb, X, Y, X + 2, Y + 2, texmaps.SecureWall, R.Floor);
        /*Add a door... with alarm.*/
        SC.gb.map[X + 1 + D - 1, Y + 1 - 1].terr = texmaps.ClosedDoor;
        SC.gb.map[X + 1 + D * 2 - 1, Y + 1 - 1].trap = -4;

        if (rpgdice.Random(100) + 1 <= SecurityCellItem)
        {
            /*Add an item...*/
            int N = rpgdice.Random(SecurityCellNum) + 1;
            for (int t = 1; t <= N; ++t)
            {
                dcitems.DCItem I = charts.GenerateItem(SC, charts.TType_SecurityArea);
                dcitems.PlaceDCItem(SC.gb, SC.ig, I, X + 1, Y + 1);
            }

            /*... then add a trap*/
            if (rpgdice.Random(100) != 74)
                SC.gb.map[X + 1 - 1, Y + 1 - 1].trap = -SecTrap[rpgdice.Random(10)];

        }
        else if (rpgdice.Random(100) + 1 <= SecurityCellZombie)
        {
            critters.AddCritter(ref SC.CList, SC.gb, 3, X + 1, Y + 1);
        }
        else if (rpgdice.Random(3) != 1)
        {
            /*Just add a trap.*/
            SC.gb.map[X + 1 - 1, Y + 1 - 1].trap = -SecTrap[rpgdice.Random(10)];
        }
    }

    static void DetailSecArea(gamebook.Scenario SC, Room R)
    {
        /*Draw in the details appropriate for the security area.*/

        if (R.W >= 11)
        {
            /*Add two lines of security cells*/
            for (int t = 1; t <= (R.H - 4) / 3; ++t)
            {
                SecCell(SC, R, R.X + 2, R.Y - 1 + t * 3, 1);
                SecCell(SC, R, R.X + R.W - 5, R.Y - 1 + t * 3, -1);
            }
        }
        else if (R.W >= 8)
        {
            /*Add one line of security cells*/
            for (int t = 1; t <= (R.H - 4) / 3; ++t)
            {
                SecCell(SC, R, R.X + 2, R.Y - 1 + t * 3, 1);
            }
        }

        /*Add security robots.*/
        if (rpgdice.Random(2) == 1) critters.AddCritter(ref SC.CList, SC.gb, 5, R.X + 1, R.Y + 1);
        if (rpgdice.Random(2) == 1) critters.AddCritter(ref SC.CList, SC.gb, 5, R.X + 1, R.Y + R.H - 2);
        if (rpgdice.Random(2) == 1) critters.AddCritter(ref SC.CList, SC.gb, 5, R.X + R.W - 2, R.Y + 1);
        if (rpgdice.Random(2) == 1) critters.AddCritter(ref SC.CList, SC.gb, 5, R.X + R.W - 2, R.Y + R.H - 2);
        int X = R.X + (R.W / 2);
        int Y = R.Y + (R.H / 2);
        if (IsASpace(SC.gb, X, Y) && rpgdice.Random(10) == 1)
        {
            critters.AddCritter(ref SC.CList, SC.gb, 21, X, Y);
        }

        /* Add some alarms and/or traps. */
        int N = rpgdice.Random(10);
        for (int t = 1; t <= N; ++t)
        {
            X = rpgdice.Random(R.W - 2) + R.X + 1;
            Y = rpgdice.Random(R.H - 2) + R.Y + 1;
            if (IsASpace(SC.gb, X, Y) && SC.gb.map[X - 1, Y - 1].trap == 0)
            {
                if (rpgdice.Random(10) == 1)
                    SC.gb.map[X - 1, Y - 1].trap = -2;
                else
                    SC.gb.map[X - 1, Y - 1].trap = -4;
            }
        }
    }

    static int[] Robo = { 1, 1, 1, 1, 5, 1, 1, 1, 1, 5 };

    static void DetailStorage(gamebook.Scenario SC, Room R)
    {
        /*Draw in the details appropriate for the storage room.*/

        /*Loop through each avaliable storage pile position. For*/
        /*each one, add a pile of goods, a robot of some type, or*/
        /*nothing at all.*/
        for (int X = 1; X <= (R.W - 5) / 2; ++X)
        {
            for (int Y = 1; Y <= (R.H - 5) / 2; ++Y)
            {
                if (rpgdice.Random(100) + 1 <= StorageRoomItem)
                {
                    /*We are going to stick one (or more) items on the floor at this position.*/
                    int N = 1;
                    do
                    {
                        N += 1;
                        dcitems.DCItem I = charts.GenerateItem(SC, charts.TType_StorageRoom);
                        dcitems.PlaceDCItem(SC.gb, SC.ig, I, R.X + (X * 2) + 1, R.Y + (Y * 2) + 1);
                    }
                    while (rpgdice.Random(100) + 1 < StorageRoomItem && N < StorageRoomMaxRolls);
                }
                else if (rpgdice.Random(100) + 1 <= StorageRoomRobot)
                {
                    /*There's no item here... so, we're gonna add a robot instead.*/
                    critters.AddCritter(ref SC.CList, SC.gb, Robo[rpgdice.Random(10)], R.X + (X * 2) + 1, R.Y + (Y * 2) + 1);
                }
            }
        }
    }

    static void DetailBigBox(gamebook.Scenario SC, Room R)
    {
        /*Why such a strange name? Because that's all this room*/
        /*is- a regular room with a huge box-shaped obstacle in*/
        /*the middle.*/
        if (rpgdice.Random(3) == 1)
        {
            /*Hollow box.*/
            if (rpgdice.Random(99) == 23)
                RoomFill(SC.gb, R.X + 2, R.Y + 2, R.X + R.W - 3, R.Y + R.H - 3, texmaps.SecureWall, 4);
            else
                RoomFill(SC.gb, R.X + 2, R.Y + 2, R.X + R.W - 3, R.Y + R.H - 3, texmaps.SecureWall, 5);

            /*Add some entry points, maybe.*/
            int N = rpgdice.RollStep(2) - 1;
            for (int t = 1; t <= N; ++t)
            {
                int D = rpgdice.Random(4) + 1;
                int X = R.X + 2;
                int Y = R.Y + 2;

                if (D == 1)
                {
                    Y = Y + rpgdice.Random(R.H - 6) + 1;
                    X = X + R.W - 5;
                }
                else if (D == 3)
                {
                    Y = Y + rpgdice.Random(R.H - 6) + 1;
                }
                else if (D == 2)
                {
                    X = X + rpgdice.Random(R.W - 6) + 1;
                    Y = Y + R.H - 5;
                }
                else
                {
                    X = X + rpgdice.Random(R.W - 6) + 1;
                }
                SC.gb.map[X, Y].terr = texmaps.HiddenServicePanel;
            }
        }
        else
        {
            /*Filled box.*/
            RoomFill(SC.gb, R.X + 2, R.Y + 2, R.X + R.W - 3, R.Y + R.H - 3, texmaps.SecureWall, R.Wall);
        }
    }

    static void DetailShuttleBay(gamebook.Scenario SC, Room R)
    {
        AddScenery(SC.gb, R.X + 5, R.Y + 5, RocketShip, true);

        /*Add the plot arcs and the triggers for the message to be*/
        /*printed when the PC first exits the spaceship.*/
        /*7,3 7,9 8,3 8,9*/
        SC.gb.map[R.X + 11 - 1, R.Y + 7 - 1].special = 1;
        SC.gb.map[R.X + 11 - 1, R.Y + 13 - 1].special = 1;
        SC.gb.map[R.X + 12 - 1, R.Y + 7 - 1].special = 1;
        SC.gb.map[R.X + 12 - 1, R.Y + 13 - 1].special = 1;

        for (int T = R.X; T <= R.X + R.W - 1; ++T)
        {
            SC.gb.map[T - 1, R.Y - 1].special = 2;
            SC.gb.map[T - 1, R.Y + R.H - 1 - 1].special = 2;
        }
        for (int T = R.Y; T <= R.Y + R.H - 1; ++T)
        {
            SC.gb.map[R.X - 1, T - 1].special = 2;
            SC.gb.map[R.X + R.W - 1 - 1, T - 1].special = 2;
        }

        /* Add the entry messages. */
        plotbase.SetSAtt(ref SC.PLLocal, "MS1 <if= V1 0 V= 1 1 print ExitShipMsg>");
        plotbase.SetSAtt(ref SC.PLLocal, "msgEXITSHIPMSG <You step out into the shuttle bay. There are no maintenance crews, no security teams, to greet your arrival. The only noise is the distant hum of the ventilators.>");
        plotbase.SetSAtt(ref SC.PLLocal, "MS2 <if= V1 1 V= 1 2 print ExitHangarMsg>");
        plotbase.SetSAtt(ref SC.PLLocal, "msgEXITHANGARMSG <The design of this level is characteristic of many old space stations. New modules were simply welded on as needed, resulting in a confusing and unplanned mass of tunnels. Finding your way may be difficult.>");

        /* Add the maintenance robot. */
        critters.AddCritter(ref SC.CList, SC.gb, 1, R.X + 1, R.Y + 1);

        /* Add a handymap. */
        dcitems.DCItem I = new dcitems.DCItem();
        I.ikind = dcitems.IKIND_Electronics;
        I.icode = 1;
        dcitems.PlaceDCItem(SC.gb, SC.ig, I, R.X + 8, R.Y + 10);

        cwords.MPU MP = cwords.AddMPU(ref SC.Comps, SC.gb, 1, R.X + R.W - 3, R.Y + R.H - 3);
        MP.Attr = "1 2 3 4 5 6 7 8 9 10 11 12 13 14 15";
    }

    static void DetailTransitway(gamebook.Scenario SC, Room R, int Door)
    {
        /*Draw a left-leading transit tube.*/
        AddScenery(SC.gb, R.X + 3, R.Y + 2, TransitChassis, false);
        SC.gb.map[R.X + 4 - 1, R.Y + 3 - 1].terr = Door;
    }

    static void DetailResidence(gamebook.Scenario SC, Room R)
    {
        /*Draw some living quarters.*/

        /* Do a big internal box. */
        RoomFill(SC.gb, R.X + 2, R.Y + 2, R.X + R.W - 3, R.Y + R.H - 3, 16, 6);

        /*Figure out how many apartments we have space to add.*/
        int N = (R.W - 5) / 6;

        for (int T = 0; T <= N - 1; ++T)
        {
            AddScenery(SC.gb, R.X + 2 + T * 6, R.Y + 2, Apartment, false);
        }
    }

    static void DetailAGQ(gamebook.Scenario SC, Room R)
    {
        /*Detail Andros Guero's quarters. This is where the PC*/
        /*can find Andros's security pass and also his diary.*/

        /*Start by detailing this area as you would any residence.*/
        DetailResidence(SC, R);

        /*Next, detail Andros's place. Put blood stains on the floor,*/
        /*and add the needed items.*/
        int N = rpgdice.Random((R.W - 5) / 6);

        /*Add the security card.*/
        dcitems.DCItem I = new dcitems.DCItem();
        I.ikind = dcitems.IKIND_KeyItem;
        I.icode = 1;
        dcitems.PlaceDCItem(SC.gb, SC.ig, I, R.X + 5 + N * 6, R.Y + 7);

        /*Add the diary.*/
        I = new dcitems.DCItem();
        I.ikind = dcitems.IKIND_Book;
        I.icode = 1;
        dcitems.PlaceDCItem(SC.gb, SC.ig, I, R.X + 5 + N * 6, R.Y + 8);

        /*Add some blood stains.*/
        SC.gb.map[R.X + 5 + N * 6 - 1, R.Y + 7 - 1].terr = 10;
        SC.gb.map[R.X + 5 + N * 6 - 1, R.Y + 8 - 1].terr = 10;
        SC.gb.map[R.X + 6 + N * 6 - 1, R.Y + 7 - 1].terr = 10;
        SC.gb.map[R.X + 4 + N * 6 - 1, R.Y + 6 - 1].terr = 10;
    }

    static void DetailChapel(gamebook.Scenario SC, Room R)
    {
        /*Draw in the details for the chapel type room.*/

        /*Determine the midpoint of the chapel.*/
        int CX = R.X + ((R.W + 1) / 2) - 1;

        /*Draw the columns and pews.*/
        for (int t = 1; t <= (R.H - 3) / 2; ++t)
        {
            SC.gb.map[R.X + 2 - 1, R.Y + t * 2 - 1].terr = texmaps.MarbleColumn;
            SC.gb.map[R.X + R.W - 3 - 1, R.Y + t * 2 - 1].terr = texmaps.MarbleColumn;

            if (t == 1)
            {
                /*Do the altar & Holy Water.*/
                SC.gb.map[CX - 1, R.Y + 1 + t * 2 - 1].terr = 39;
                dcitems.DCItem I = new dcitems.DCItem();
                I.ikind = dcitems.IKIND_Grenade;
                I.icode = 8;
                I.charge = 1 + rpgdice.Random(3);
                dcitems.PlaceDCItem(SC.gb, SC.ig, I, CX, R.Y + 2 + t * 2);

                SC.gb.map[CX - 1 - 1, R.Y + 1 + t * 2 - 1].terr = 18;

                if (!Odd(R.W))
                {
                    SC.gb.map[CX + 1 - 1, R.Y + 1 + t * 2 - 1].terr = 39;
                    I = new dcitems.DCItem();
                    I.ikind = dcitems.IKIND_Grenade;
                    I.icode = 8;
                    I.charge = 1 + rpgdice.Random(3);
                    dcitems.PlaceDCItem(SC.gb, SC.ig, I, CX + 1, R.Y + 2 + t * 2);

                    SC.gb.map[CX + 2 - 1, R.Y + 1 + t * 2 - 1].terr = 18;
                }
                else
                {
                    SC.gb.map[CX + 1 - 1, R.Y + 1 + t * 2 - 1].terr = 18;
                    I = new dcitems.DCItem();
                    I.ikind = dcitems.IKIND_Grenade;
                    I.icode = 8;
                    I.charge = 1 + rpgdice.Random(3);
                    dcitems.PlaceDCItem(SC.gb, SC.ig, I, CX, R.Y + 2 + t * 2);
                }
            }
            else if (t < (R.H - 3) / 2)
            {
                /*Do the pews.*/
                for (int X = 4; X <= R.W - 5; ++X)
                {
                    SC.gb.map[R.X + X - 1, R.Y + 1 + t * 2 - 1].terr = texmaps.Chair;
                }
            }
        }
    }

    static int[] CryptCrit = { 3, 3, 3, 3, 3, 20, 22, 22, 16, 16 };

    static void RenderReliquary(gamebook.Scenario SC, int X1, int Y1, int X2, int Y2)
    {
        /* Actually draw on the map a reliquary chamber. It's kind of */
        /* like a Moria treasure room. */

        int N;

        /* Create the basic structure. */
        RoomFill(SC.gb, X1, Y1, X2, Y2, texmaps.SecureWall, 40);

        /* Scan through the interior tiles alternating walls and floors. */
        for (int X = X1 + 1; X <= X2 - 1; ++X)
        {
            for (int Y = Y1 + 1; Y <= Y2 - 1; ++Y)
            {
                if (Odd(X + Y))
                {
                    /* Add a wall here. */
                    SC.gb.map[X - 1, Y - 1].terr = texmaps.SecureWall;
                }
                else
                {
                    /* Add a pile of treasure or a monster here. */
                    if (rpgdice.Random(100) < CryptItem)
                    {
                        /*Add an item...*/
                        N = rpgdice.Random(CryptNum) + 1;
                        for (int t = 1; t <= N; ++t)
                        {
                            dcitems.DCItem I = charts.GenerateItem(SC, charts.TType_Crypt);
                            dcitems.PlaceDCItem(SC.gb, SC.ig, I, X, Y);
                        }
                    }
                    else if (rpgdice.Random(100) < CryptCritter)
                    {
                        /*...or a monster.*/
                        critters.AddCritter(ref SC.CList, SC.gb, CryptCrit[rpgdice.Random(CryptCrit.Length)], X, Y);
                    }
                    else if (rpgdice.Random(100) == 23)
                    {
                        /* On a very rare chance, there'll be a big item cache here. */
                        PlaceItemCache(SC, X, Y);
                    }
                }
            }
        }

        /* Add the doors and traps. */
        N = rpgdice.Random(3) + 3;
        if (rpgdice.Random(2) == 1)
        {
            /* Vertical Orientation - Place the doors. */
            int X = (X1 + X2) / 2;
            SC.gb.map[X - 1, Y1 - 1].terr = texmaps.ClosedDoor;
            SC.gb.map[X - 1, Y2 - 1].terr = texmaps.ClosedDoor;
            SC.gb.map[X - 1, Y1 - 1].special = N;
            SC.gb.map[X - 1, Y2 - 1].special = N;

            /* Inside the door is the super-nasty Plasma Barrier trap. */
            SC.gb.map[X, Y1 + 1 - 1].terr = 40;
            SC.gb.map[X, Y2 - 1 - 1].terr = 40;
            SC.gb.map[X, Y1 + 1 - 1].trap = -5;
            SC.gb.map[X, Y2 - 1 - 1].trap = -5;

            /* Outside the door is an alarm. */
            SC.gb.map[X - 1, Y1 - 1 - 1].trap = -4;
            SC.gb.map[X - 1, Y2 + 1 - 1].trap = -4;
        }
        else
        {
            /* Horizontal Orientation */
            int Y = (Y1 + Y2) / 2;
            SC.gb.map[X1 - 1, Y - 1].terr = texmaps.ClosedDoor;
            SC.gb.map[X2 - 1, Y - 1].terr = texmaps.ClosedDoor;
            SC.gb.map[X1 - 1, Y - 1].special = N;
            SC.gb.map[X2 - 1, Y - 1].special = N;

            /* Inside the door is the super-nasty Plasma Barrier trap. */
            SC.gb.map[X1 + 1 - 1, Y - 1].terr = 40;
            SC.gb.map[X2 - 1 - 1, Y - 1].terr = 40;
            SC.gb.map[X1 + 1 - 1, Y - 1].trap = -5;
            SC.gb.map[X2 - 1 - 1, Y - 1].trap = -5;

            /* Outside the door is an alarm. */
            SC.gb.map[X1 - 1, Y].trap = -4;
            SC.gb.map[X2 + 1, Y].trap = -4;
        }

        /* Add some alarms and/or traps. */
        N = rpgdice.Random(30);
        for (int t = 1; t <= N; ++t)
        {
            int X = rpgdice.Random(X2 - X1 + 2) + X1 - 1;
            int Y = rpgdice.Random(Y2 - Y1 + 2) + Y1 - 1;
            if (IsASpace(SC.gb, X, Y) && SC.gb.map[X - 1, Y - 1].trap == 0)
            {
                SC.gb.map[X - 1, Y - 1].trap = -4;
            }
        }
    }

    static void DetailReliquary(gamebook.Scenario SC, Room R)
    {
        /* Provide the details for a reliquary type room. */
        RenderReliquary(SC, R.X + 2, R.Y + 2, R.X + R.W - 3, R.Y + R.H - 3);
    }

    static void DetailMedCenter(gamebook.Scenario SC, Room R)
    {
        /* Provide the details for the medical center- some beds and */
        /* a medical unit. */

        /* Add some beds. */
        int X;
        for (X = R.X + 4; X <= R.X + R.W - 5; ++X)
        {
            for (int Y = R.Y + 2; Y <= R.Y + R.H - 3; ++Y)
            {
                if (Odd(X) && Odd(Y) && rpgdice.Random(3) != 1)
                {
                    SC.gb.map[X - 1, Y - 1].terr = 19;
                }
            }
        }

        /* Add the computer. */
        X = rpgdice.Random(4);
        cwords.MPU MP;

        if (X == 0)
        {
            MP = cwords.AddMPU(ref SC.Comps, SC.gb, 2, R.X + R.W - 3, R.Y + R.H - 3);
        }
        else if (X == 1)
        {
            MP = cwords.AddMPU(ref SC.Comps, SC.gb, 2, R.X + R.W - 3, R.Y + 2);
        }
        else if (X == 2)
        {
            MP = cwords.AddMPU(ref SC.Comps, SC.gb, 2, R.X + 2, R.Y + R.H - 3);
        }
        else
        {
            MP = cwords.AddMPU(ref SC.Comps, SC.gb, 2, R.X + 2, R.Y + 2);
        }
        MP.Attr = "16 17 18 19 20 21 22";
    }

    static void DetailGravesite(gamebook.Scenario SC, Room R)
    {
        /* Fill in the details for one of DeadCold's memorial gounds. */

        /* Fill the interior of the room with grass. */
        RectFill(SC.gb, R.X + 2, R.Y + 2, R.X + R.W - 3, R.Y + R.H - 3, 42);

        /* Add tombstones and shubbery. */
        for (int X = R.X + 3; X <= R.X + R.W - 4; ++X)
        {
            for (int Y = R.Y + 3; Y <= R.Y + R.H - 4; ++Y)
            {
                if (Odd(X) && Odd(Y) && rpgdice.Random(3) == 1)
                {
                    SC.gb.map[X - 1, Y - 1].terr = 41;
                }
                else if (rpgdice.Random(5) == 2)
                {
                    SC.gb.map[X - 1, Y - 1].terr = 4;
                }
            }
        }

        /* If the room is big enough, add a reliquary. */
        if (R.W > 24 + rpgdice.Random(10) && R.H > 16 + rpgdice.Random(10))
        {
            int X = R.X + 2 + rpgdice.Random(R.W - 20);
            int Y = R.Y + 2 + rpgdice.Random(R.H - 12);

            RenderReliquary(SC, X, Y, X + 15, Y + 7);
        }
    }

    static void DetailControlCenter(gamebook.Scenario SC, Room R)
    {
        /* Fill out the life support control center. From here, the */
        /* player can re-activate life support for this level. */
        /* Start by actually adding the life support breakdown plot scripts. */

        plotbase.SetSAtt(ref SC.PLLocal, "START <ifG PL 0 if= V2 0 print 1>");
        plotbase.SetSAtt(ref SC.PLLocal, "MSG1 <The air on this level doesn't smell fresh...>");

        /* If the life support is off, if the player leakage is greater than 0, */
        /* add the leakage score to variable 3. If V3 > 500, the PC will begin */
        /* to choke. */
        plotbase.SetSAtt(ref SC.PLLocal, "HOUR <if= V2 0 ifG PL 0 ifG V3 100 else GotoLEAK Choke>");
        plotbase.SetSAtt(ref SC.PLLocal, "10MIN <if= V2 0 ifG PL 0 ifG V3 500 else GotoLEAK Choke>");
        plotbase.SetSAtt(ref SC.PLLocal, "MINUTE <if= V2 0 ifG PL 0 ifG V3 2500 else GOTOLeak Choke>");
        plotbase.SetSAtt(ref SC.PLLocal, "GotoLEAK <V+ 3 PL>");

        /* Add MORGAN to the room. */
        cwords.MPU MP = cwords.AddMPU(ref SC.Comps, SC.gb, 3, R.X + (R.W / 2), R.Y + (R.H / 2));
        MP.Attr = "23 24 25 26 27";
    }

    static void DetailCryoLab(gamebook.Scenario SC, Room R)
    {
        /* Fill out the criogenics lab. From here, the player may be */
        /* able to escape the station in a cryogenics pod. */

        /* Add the cryogenic space probe capsules. */
        for (int T = 1; T <= (R.H - 4) / 5; ++T)
        {
            AddScenery(SC.gb, R.X + 3, R.Y + 2 + T * 5, Capsule, false);
        }
    }

    static int RanDoor()
    {
        if (rpgdice.Random(3) == 1)
            return texmaps.OpenDoor;

        return texmaps.ClosedDoor;
    }

    static void DetailMuseum(gamebook.Scenario SC, Room R)
    {
        /* Draw the stuff for the museum, including the sealed-off */
        /* display chamber. */

        /* The museum has been infested by Algon Dust-Jellies, so a */
        /* force field has been erected around it. To deactivate the */
        /* field, the player will have to hack DesCartes. */

        cwords.MPU MP = cwords.AddMPU(ref SC.Comps, SC.gb, 1, R.X + 2, R.Y + 2);
        MP.Attr = "28 29";

        /* Draw the field box, then the interior chamber. */
        RoomFill(SC.gb, R.X + 5, R.Y + 5, R.X + R.W - 6, R.Y + R.H - 6, texmaps.ForceField, 15);
        RoomFill(SC.gb, R.X + 6, R.Y + 6, R.X + R.W - 7, R.Y + R.H - 7, texmaps.SecureWall, 15);

        /* Add some doors. */
        /* Left Wall */
        SC.gb.map[R.X + 6 - 1, R.Y + 7 - 1].terr = RanDoor();
        SC.gb.map[R.X + 6 - 1, R.Y + R.H - 8 - 1].terr = RanDoor();
        /* Right Wall */
        SC.gb.map[R.X + R.W - 7 - 1, R.Y + 7 - 1].terr = RanDoor();
        SC.gb.map[R.X + R.W - 7 - 1, R.Y + R.H - 8 - 1].terr = RanDoor();
        /* Top Wall */
        SC.gb.map[R.X + 7 - 1, R.Y + 6 - 1].terr = RanDoor();
        SC.gb.map[R.X + R.W - 8 - 1, R.Y + 6 - 1].terr = RanDoor();
        /* Bottom Wall */
        SC.gb.map[R.X + 7 - 1, R.Y + R.H - 7 - 1].terr = RanDoor();
        SC.gb.map[R.X + R.W - 8 - 1, R.Y + R.H - 7 - 1].terr = RanDoor();


        /* Add the jellies */
        for (int X = R.X + 7; X <= R.X + R.W - 8; ++X)
        {
            for (int Y = R.Y + 7; Y <= R.Y + R.H - 8; ++Y)
            {
                critters.AddCritter(ref SC.CList, SC.gb, 23, X, Y);
            }
        }

        /* Add the payoff. */
        dcitems.DCItem I = new dcitems.DCItem();
        I.ikind = dcitems.IKIND_Wep;
        I.icode = 16;
        I.ID = false;
        dcitems.PlaceDCItem(SC.gb, SC.ig, I, R.X + (R.W / 2), R.Y + (R.H / 2));

        /* Add some traps. */
        SC.gb.map[R.X + (R.W / 2) - 1, R.Y + (R.H / 2) - 1].trap = 5;
        for (int X = R.X + (R.W / 2) - 1; X <= R.X + (R.W / 2) + 1; ++X)
        {
            for (int Y = R.Y + (R.H / 2) - 1; Y <= R.Y + (R.H / 2) + 1; ++Y)
            {
                SC.gb.map[X - 1, Y - 1].trap = -4;
            }
        }
        SC.gb.map[R.X + (R.W / 2) - 1, R.Y + (R.H / 2) - 1].trap = 5;
    }

    static void DetailDesCartes(gamebook.Scenario SC, Room R)
    {
        /* Fill out the life support control center. From here, the */
        /* player can de-activate the force field around the museum. */

        plotbase.SetSAtt(ref SC.PLLocal, "GotoTURNOFFFIELD <ChangeTerr 43 44>");

        critters.AddCritter(ref SC.CList, SC.gb, 22, R.X + 1, R.Y + 1);
        critters.AddCritter(ref SC.CList, SC.gb, 22, R.X + 1, R.Y + R.H - 2);
        critters.AddCritter(ref SC.CList, SC.gb, 22, R.X + R.W - 2, R.Y + 1);
        critters.AddCritter(ref SC.CList, SC.gb, 22, R.X + R.W - 2, R.Y + R.H - 2);


        /* Add DESCARTES to the room. */
        cwords.MPU MP = cwords.AddMPU(ref SC.Comps, SC.gb, 4, R.X + (R.W / 2), R.Y + (R.H / 2));
        MP.Attr = " ";
    }

    static void RenderRoom(gamebook.Scenario SC, Room R)
    {
        /*Render the room in question. Detail it appropriately.*/
        /*Add some items if you really must.*/

        /*First, draw the walls and the floor.*/
        RoomFill(SC.gb, R.X, R.Y, R.X + R.W - 1, R.Y + R.H - 1, R.Wall, R.Floor);

        switch (R.Style)
        {
            case 3: DetailLounge(SC, R); break;
            case 4: DetailSecArea(SC, R); break;
            case 5: DetailStorage(SC, R); break;
            case 6: DetailBigBox(SC, R); break;
            case 7: DetailShuttleBay(SC, R); break;
            case 8: DetailTransitway(SC, R, 32); break;
            case 9: DetailTransitway(SC, R, 33); break;
            case 10: DetailResidence(SC, R); break;
            case 11: DetailAGQ(SC, R); break;
            case 12: DetailChapel(SC, R); break;
            case 13: DetailReliquary(SC, R); break;
            case 14: DetailMedCenter(SC, R); break;
            case 15: DetailGravesite(SC, R); break;
            case 16: DetailControlCenter(SC, R); break;
            case 17: DetailCryoLab(SC, R); break;
            case 18: DetailTransitway(SC, R, 34); break;
            case 19: DetailTransitway(SC, R, 35); break;
            case 20: DetailMuseum(SC, R); break;
            case 21: DetailDesCartes(SC, R); break;
        }
    }

    static void PlaceRoom(gamebook.Scenario SC, Room R)
    {
        /*Place room R on the map GB. Locate an empty spot for it,*/
        /*then render it to map memory.*/

        /*Keep looping until we find an empty spot.*/
        int C = 0;
        while (R.X == -1 && C < 10000)
        {
            C += 1;
            int X1 = rpgdice.Random(texmodel.XMax - R.W) + 1;
            int Y1 = rpgdice.Random(texmodel.YMax - R.H) + 1;
            if (AreaClear(SC.gb, X1, Y1, R.W, R.H))
            {
                R.X = X1;
                R.Y = Y1;
            }
        }

        /*Add the room*/
        RenderRoom(SC, R);
    }

    static void GFRoom(gamebook.Scenario SC, int X, int Y, int Sens)
    {
        /*Create a random room in the area bounded by X,Y - X+S,Y+S.*/
        Room R = null;
        NewRoom(ref R);

        /*Check to make sure the new room is small enough to fit*/
        /*into our alloted space.*/
        if (R.W >= Sens)
            R.W = Sens - 1;
        if (R.H >= Sens)
            R.H = Sens - 1;

        /*Make sure Lounge type rooms still look pretty.*/
        if (R.Style == 3 || R.Style == 12)
        {
            if (!Odd(R.H)) R.H -= 1;
            if (!Odd(R.W)) R.W -= 1;
        }

        /*Select a position within our alloted space for the room.*/
        R.X = X + rpgdice.Random(Sens - R.W);
        R.Y = Y + rpgdice.Random(Sens - R.H);

        RenderRoom(SC, R);
        AddWTunnels(SC.gb, R, 5);
    }

    static void GapFiller(gamebook.Scenario SC)
    {
        /*Check through the map as it currently exists. Upon finding*/
        /*large patches of unused space, stick something interesting*/
        /*there. These interesting things will not be guaranteed*/
        /*accessable, but so what? They're not key rooms. And, if*/
        /*you really want to get to them, you'll own a las-cutter.*/
        const int sens = 32;    /*The sensitivity of the search.*/

        for (int XB = 0; XB <= (texmodel.XMax / sens) - 1; ++XB)
        {
            for (int YB = 0; YB <= (texmodel.YMax / sens) - 1; ++YB)
            {
                /*Check this block for stuff.*/
                if (AreaClear(SC.gb, XB * sens + 1, YB * sens + 1, sens, sens))
                {
                    /*There's nothing in this region. Add a random room,*/
                    /*then connect it to something using WTunnels.*/
                    GFRoom(SC, XB * sens + 1, YB * sens + 1, sens);
                }
            }
        }
    }


    static void FreezeCurrentLevel(gamebook.Scenario SC)
    {
        /* Take the current level, store it in its proper FROZEN slot, */
        /* then clear whatever & prepare for a new level. */

        /* If the level number is in the legal range, freeze it. */
        if (SC.Loc_Number >= 1 && SC.Loc_Number <= gamebook.Num_Levels)
        {
            SC.Frozen_Levels[SC.Loc_Number - 1].gb = SC.gb;
            SC.Frozen_Levels[SC.Loc_Number - 1].ig = SC.ig;
            SC.Frozen_Levels[SC.Loc_Number - 1].PL = SC.PLLocal;
            SC.Frozen_Levels[SC.Loc_Number - 1].CList = SC.CList;
            SC.Frozen_Levels[SC.Loc_Number - 1].Comps = SC.Comps;

            /* Get rid of the player's model from the gameboard. */
            texmodel.RemoveModel(SC.PC.m, ref SC.gb.mlist, SC.gb.mog);
            SC.gb.POV.m = null;

            /* Dispose of the clouds on the board. */
            cwords.CleanCloud(SC.gb, ref SC.Fog);

            /* Clear all stuff. */
            SC.gb = null;
            SC.ig = null;
            SC.PLLocal = null;
            SC.CList = null;
            SC.Comps = null;

        }
        else
        {
            /* If the level number isn't in the legal range, just */
            /* dispose of the level. */

            /* Clear all stuff. */
            SC.gb = null;
            SC.ig = null;
            SC.PLLocal = null;
            SC.CList = null;
            SC.Comps = null;
        }
    }

    static void UnfreezeLevel(gamebook.Scenario SC, int N)
    {
        /* Restore level N from its frozen state to full playability. */
        /* - Copy pointers from the Frozen record to the Scenario */
        /* - Set pointers in the Frozen record to null */

        /* Set the location number. */
        SC.Loc_Number = N;

        /* Error check - if the requested level is outside of the */
        /* valid range, generate a new completely random level. */
        if (N < 1 || N > gamebook.Num_Levels)
        {
            GenerateCurrentLevel(SC);
        }
        else
        {
            /* Move the frozen stuff to the scenario main record... */
            SC.gb = SC.Frozen_Levels[N - 1].gb;
            SC.ig = SC.Frozen_Levels[N - 1].ig;
            SC.PLLocal = SC.Frozen_Levels[N - 1].PL;
            SC.CList = SC.Frozen_Levels[N - 1].CList;
            SC.Comps = SC.Frozen_Levels[N - 1].Comps;

            /*...then set the FROZEN records to null. */
            SC.Frozen_Levels[N-1].gb = null;
            SC.Frozen_Levels[N-1].ig = null;
            SC.Frozen_Levels[N-1].PL = null;
            SC.Frozen_Levels[N-1].CList = null;
            SC.Frozen_Levels[N-1].Comps = null;
        }
    }

}