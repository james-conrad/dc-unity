using System;
using System.IO;

using UnityEngine; // using this for Random

public class texmaps
{
    //{This will handle all of the map stuff for a RL game.}
    //{This is the ASCII version of this unit.}
    //{ *** GFX UNIT*** }

    public const int POVSize = 50;
    public const int NumTerr = 50;

    //{Constants for specific terrain types.}
    //{CONVENTION: ClosedTerr = OpenTerr + 1}
    public const int OpenDoor = 7;
    public const int ClosedDoor = 8;
    public const int OpenServicePanel = 11;
    public const int ClosedServicePanel = 12;
    public const int HiddenServicePanel = 13;
    public const int Crawlspace = 9;
    public const int SecureWall = 16;
    public const int Chair = 14;
    public const int MarbleColumn = 17;
    public const int PilotsChair = 21;
    public const int TransitLeft = 32;
    public const int TransitRight = 33;
    public const int TransitUp = 35;
    public const int TransitDown = 34;
    public const int ForceField = 43;
    public const int ForceFieldGenerator = 44;
    public const int CryoCapsule = 45;

    public static string[] TerrName = new string[NumTerr]
    {
        "Vacuum",           "Floor",           "Wall",            "Shrubbery",             "Grate",
        "Inner Workings",   "Door",            "Door",            "CrawlSpace",            "Blood Stain",
        "Service Panel",    "Service Panel",   "Wall",            "Chair",                 "Carpet",
        "Wall",             "Marble Column",   "Scarab",          "Bed",                   "Space Ship",
        "Pilot's Chair",    "Command Console", "View Port",       "Space Ship",            "Space Ship",
        "Space Ship",       "Thruster",        "Space Ship",      "Floor",                 "Transitway",
        "Transitway",       "Transitway Door", "Transitway Door", "Transitway Door",       "Transitway Door",
        "Transitway Shaft", "Table",           "Toilet",          "Altar",                 "Marble Floor",
        "Headstone",        "Grass",           "Force Field",     "Force Field Generator", "Cryogenic Casket",
        "Thruster",         "Space Ship",      "Space Ship",      "Space Ship",            "Space Ship"
    };

    public static char[] TerrChar = new char[NumTerr]
    {
        ' ',(char)250,'#', (char)5,(char)240,
        '#',':','+','=','.',
        ':','#','#','-','.',
        '#','I','i','_','#',
        '-','[',')',']','=',
        '|','>','-','.','[',
        ']','<','>','V','A',
        '*','0','-','~','.',
        '+','.','*','^','=',
        'A','|','/','!','\\',
    };

    public static Crt.Color[] TerrColor = new Crt.Color[NumTerr]
    {
        Crt.Color.Black,     Crt.Color.Blue,       Crt.Color.White,     Crt.Color.Green,    Crt.Color.DarkGray,
        Crt.Color.LightGray, Crt.Color.Cyan,       Crt.Color.Cyan,      Crt.Color.Magenta,  Crt.Color.Red,
        Crt.Color.Yellow,    Crt.Color.LightCyan,  Crt.Color.White,     Crt.Color.Yellow,   Crt.Color.LightBlue,
        Crt.Color.White,     Crt.Color.White,      Crt.Color.Blue,      Crt.Color.Yellow,   Crt.Color.Green,
        Crt.Color.White,     Crt.Color.Red,        Crt.Color.Yellow,    Crt.Color.Green,    Crt.Color.Green,
        Crt.Color.Green,     Crt.Color.LightGreen, Crt.Color.Green,     Crt.Color.Cyan,     Crt.Color.White,
        Crt.Color.White,     Crt.Color.Cyan,       Crt.Color.Cyan,      Crt.Color.Red,      Crt.Color.Red,
        Crt.Color.White,     Crt.Color.White,      Crt.Color.LightGray, Crt.Color.White,    Crt.Color.LightGray,
        Crt.Color.White,     Crt.Color.Green,      Crt.Color.Yellow,    Crt.Color.DarkGray, Crt.Color.Blue,
        Crt.Color.Yellow,    Crt.Color.Yellow,     Crt.Color.Yellow,    Crt.Color.Yellow,   Crt.Color.Yellow,
    };

    //{This array tells how easily you can see through the terrain in question.}
    //{A negative value means the terrain completely blocks LOS.}
    public static int[] TerrObscurement = new int[NumTerr]
    {
         0,  0, -1,  5,  0,
        -1,  1, -1,  1,  0,
         1, -1, -1,  1,  0,
        -1,  1,  1,  1, -1,
         1, -1, -1, -1, -1,
        -1,  1, -1,  0, -1,
        -1,  0,  0,  0,  0,
        -1,  2,  1,  2,  0,
         2,  0, -1,  0,  2,
        -1, -1, -1, -1, -1,
    };

    //{This array tells how easily you may walk through the terrain.}
    //{It is a percent chance of making it through unhindered.}
    //{CONVENTION: A negative number here indicates terrain which cannot be overwritten in the random map generator.}
    public static int[] TerrPass = new int[NumTerr]
    {
        -1, 100,   0,  30, 100,
         0, 100,  -1,  85, 100,
        75,  -1,  -1,  55, 100,
        -1,  -1,  -1,  45,  -1,
        85,  -1,  -1,  -1,  -1,
        -1,  -1,  -1, 100,  -1,
        -1, 100, 100, 100, 100,
        -1,  15,  50,  -1, 100,
        -1, 100,  -1, 100,  95,
        -1,  -1,  -1,  -1,  -1,
    };


    //{These constants tell the system how to display traps.}
    static char TrapGfx = '^';
    static Crt.Color TrapColor = Crt.Color.LightRed;

    //{This array holds the vector information for movement.The 9 directions}
    //{correspond to the keys on the numeric keypad.}
    public static int[,] VecDir = new int[9, 2]
    {
        { -1,  1 }, { 0, 1 }, { 1, 1 },
        { -1,  0 }, { 0, 0 }, { 1, 0 },
        { -1, -1 }, { 0,-1 }, { 1,-1 },
    };

    public struct Point
    {
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;
    }

    public class FrameOfReference
    {
        public bool[,] los = new bool[POVSize * 2 + 1, POVSize * 2 + 1]; //{ Each block in the array is set to True if visible from center, false otherwise.}
        public int leftX; //{ The top,left corner of the screen display.}
        public int topY;
        public texmodel.Model m; //{ The model to which the POV belongs.}
        public int range; //{ Range of visual perception.}
        public int sense; //{ The sensor rating of the FoR.}
    }

    public struct Tile
    {
        public int terr;     //{ The terrain of the tile.}
        public bool visible; //{ Has the tile been spotted yet?}
        public int trap;     //{ Is there a trap here? 0 = No trap; + = Trap visible}
        public int special;  //{ Is there something special here? This unit doesn't check the value of this field at all.}
    }

    //{ This is an image overlaid on the map.}
    public struct OverImage
    {
        public char gfx;        //{ Useful primarily for items.}
        public Crt.Color color;
    }

    public class GameBoard
    {
        public Tile[,] map = new Tile[texmodel.XMax, texmodel.YMax]; //{ The terrain in each map square.}
        public texmodel.ModelGrid mog = new texmodel.ModelGrid(); //{ The modelgrid for the map.}
        public OverImage[,] itm = new OverImage[texmodel.XMax, texmodel.YMax];
        public texmodel.Model mlist; //{ The list of models that are present on the map.}
        public FrameOfReference POV = new FrameOfReference();
    }

    public class WalkReport
    {
        public bool go; //{ Did the walker actually move anywhere?}
        public texmodel.Model m; //{ Did the walker contact another model?}
        public int trap; //{ Did the walker step on a trap?}
    }

    public static Point SolveLine(int x1, int y1, int x2, int y2, int n)
    {
        //{ Find the N'th point along a line starting at X1,Y1 and ending}
        //{ at X2,Y2. Return its location.}

        //{ ERROR CHECK- Solve the trivial case.}
        if (x1 == x2 && y1 == y2)
        {
            return new Point(x1, y1);
        }

        //{ For line determinations, we'll use a virtual grid where each game}
        //{ tile is a square 10 units across. Calculations are done from the}
        //{ center of each square.}
        int Vx1 = x1 * 10 + 5;
        int Vy1 = y1 * 10 + 5;

        //{Do the slope calculations.}
        int Rise = y2 - y1;
        int Run = x2 - x1;

        int VX = 0;
        int VY = 0;
        if (Math.Abs(x2 - x1) > Math.Abs(y2 - y1))
        {
            //{The X direction is longer than the Y axis.}
            //{Therefore, we can infer X pretty easily, then}
            //{solve the equation for Y.}
            //{Determine our X value.}
            if (Run > 0) VX = (n * 10) + Vx1;
            else VX = Vx1 - n * 10;

            VY = n * 10 * Rise / Math.Abs(Run) + Vy1;
        }
        else
        {
            //{ The Y axis is longer.}
            if (Rise > 0) VY = (n * 10) + Vy1;
            else VY = Vy1 - n * 10;

            VX = (n * 10 * Run / Math.Abs(Rise)) + Vx1;
        }

        //{Error check- DIV doesn't deal with negative numbers as I would}
        //{want it to. I'd always like a positive remainder- so, let's modify}
        //{the values.}
        if (VX < 0) VX = VX - 10;
        if (VY < 0) VY = VY - 10;

        return new Point(VX / 10, VY / 10);
    }

    public static GameBoard NewBoard()
    {
        //{ Initialize a new GameBoard array.}

        //{Allocate memory for the map}
        GameBoard gb = new GameBoard();

        //{Loop through every tile on the map and initialize it.}
        for (int x = 0; x < texmodel.XMax; ++x)
        {
            for (int y = 0; y < texmodel.YMax; ++y)
            {
                //gb.map[x, y] = new Tile();
                gb.map[x, y].terr = 0; //	{Set terrain to 0. No reason.}
                gb.map[x, y].visible = false;
                gb.map[x, y].trap = 0;
                gb.map[x, y].special = 0;
                gb.mog.Set(x + 1, y + 1, false); //{ There's no model standing here now...}
                //gb.itm[x, y] = new OverImage();
                gb.itm[x, y].gfx = ' ';
                gb.itm[x, y].color = Crt.Color.White;
            }
        }

        gb.mlist = null;

        //{ Initialize the POV elements.}
        gb.POV.topY = 1;
        gb.POV.leftX = 1;
        gb.POV.sense = 5;
        gb.POV.m = null;

        return gb;
    }

    public static bool OnTheMap(int x, int y)
    {
        //{Check the set of coordinates given, and decide whether or not}
        //{the point lies within the defined map.}

        if ((x >= 1 && x <= texmodel.XMax) &&
            (y >= 1 && y <= texmodel.YMax))
        {
            return true;
        }

        return false;
    }

    public static bool OnTheScreen(GameBoard gb, int x, int y)
    {
        //{ This function will tell whether or not map location X,Y}
        //{ is currently within screen boundaries. This doesn't}
        //{ necessarily mean that it's visible, just that it's present.}

        //{ Error Check- Make sure the POV is initialized.}
        if (gb.POV.m == null)
            return false;

        if ((x >= gb.POV.leftX && x < gb.POV.leftX + MapDisplayWidth) &&
            (y >= gb.POV.topY && y < gb.POV.topY + MapDisplayHeight))
        {
            return true;
        }

        return false;
    }

    public static int GetTerr(GameBoard gb, int x, int y)
    {
        //{Check location X,Y and return the terrain type found there.}
        //{If the point lies outside of the boundaries, return a one.}

        //{Check to make sure that the point lies within the boundaries}
        //{of the map.}
        if (OnTheMap(x, y))
        {
            return gb.map[x - 1, y - 1].terr;
        }

        return 0;
    }

    public static bool TileLOS(FrameOfReference pov, int x, int y)
    {
        //{Check to see whether location X,Y is visible to the frame of reference POV.}

        //{ Error Check- if the POV isn't initialized, no LoS possible.}
        if (pov.m == null)
            return false;

        //{ Calculate PX and PY.}
        //{ The coordinates of X and Y relative to the POV origin.}
        int px = x - pov.m.x;
        int py = y - pov.m.y;

        //{ Check to make sure that PX and PY are within range.}
        if (px >= -POVSize && px <= POVSize && py >= -POVSize && py <= POVSize)
        {
            return pov.los[px + POVSize, py + POVSize];
        }

        return false;
    }

    public static void DisplayTile(GameBoard gb, int x, int y)
    {
        //{ Move the cursor to the correct location, then print the tile.}
        RenderTile(gb, true, x, y);
    }

    public static void HighlightTile(GameBoard gb, int x, int y)
    {
        //{ Move the cursor to the correct location, then print the tile.}
        RenderTile(gb, false, x, y);
    }

    public static void ClearMapArea()
    {
        //{Clear the map area. Pretty straight forward.}

        //{Set the clip area for this operation.}
        Crt.Window(MOX, MOY, MOX + MapDisplayWidth - 1, MOY + MapDisplayHeight - 1);

        //{Clear the current display area.}
        Crt.ClrScr();

        //{Restore the original window.}
        Crt.Window(1, 1, 80, 25);
    }

    public static void DisplayMap(GameBoard gb)
    {
        //{Display the map. Duh. This procedure will perform a}
        //{complete refresh of the screen display.}

        ClearMapArea();

        //{Loop through every tile currently on the screen.}
        //{Display it if visible, don't display it otherwise.}
        for (int y = gb.POV.topY; y < gb.POV.topY + MapDisplayHeight; ++y)
        {
            for (int x = gb.POV.leftX; x < gb.POV.leftX + MapDisplayWidth; ++x)
            {
                if (TileVisible(gb, x, y))
                {
                    DisplayTile(gb, x, y);
                }
            }
        }
    }

    public static void RecenterPOV(GameBoard gb)
    {
        //{The model for the POV is getting close to the edge of}
        //{the screen display. Recenter it.}
        gb.POV.leftX = gb.POV.m.x - (MapDisplayWidth / 2);
        gb.POV.topY = gb.POV.m.y - (MapDisplayHeight / 2);

        if (gb.POV.leftX < 1)
        {
            gb.POV.leftX = 1;
        }
        else if (gb.POV.leftX > texmodel.XMax - MapDisplayWidth)
        {
            gb.POV.leftX = texmodel.XMax - MapDisplayWidth + 1;
        }

        if (gb.POV.topY < 1)
        {
            gb.POV.topY = 1;
        }
        else if (gb.POV.topY > texmodel.YMax - MapDisplayHeight)
        {
            gb.POV.topY = texmodel.YMax - MapDisplayHeight + 1;
        }
    }

    const int UPV_True = 1;
    const int UPV_False = -1;
    const int UPV_Maybe = 0;

    static void CheckLine(FrameOfReference pov, GameBoard gb, int[,] temp, int xt, int yt, int d)
    {
        //{The obscurement count starts out with a value of 1.}
        int O = 1;

        //{The variable WALL represents a boundary that cannot be seen through.}
        bool Wall = false;

        for (int t = 1; t <= pov.range; ++t)
        {
            //{Locate the next point on the line.}
            Point p = SolveLine(0, 0, xt, yt, t);

            if (Math.Sqrt(p.x * p.x + p.y * p.y) > d)
                return;

            //{Determine the terrain of this tile.}
            int terr = GetTerr(gb, pov.m.x + p.x, pov.m.y + p.y);

            //{Update the Obscurement count.}
            O += TerrObscurement[terr];

            //{Models also cause obscurement.}
            if (texmodel.ModelPresent(gb.mog, p.x, p.y))
                O += 1;

            //{If we have already encountered a wall, mark this square as UPV_False}
            if (Wall)
            {
                temp[p.x + POVSize, p.y + POVSize] = UPV_False;
            }

            switch (temp[p.x + POVSize, p.y + POVSize])
            {
                case UPV_False:
                    //{This LoS is blocked. No use searching any further.}
                    break;

                case UPV_Maybe:
                    //{We will mark this one as true, but check for a wall later.}
                    temp[p.x + POVSize, p.y + POVSize] = UPV_True;
                    break;

                    //{If we got a UPV_True, we just skip merrily along without doing anything.}
            }

            //{If this current square is a wall,}
            //{or if we have too much obscurement to see,}
            //{set Wall to true.}
            if (CantSeeThrough(pov, terr) || O > pov.sense)
            {
                Wall = true;
            }
        }
    }

    static void FillOutCardinals(int[,] temp, int D)
    {
        //{ Travel along direction D. If the tile is set to UPV_True, }
        //{ then set the two adjacent tiles to UPV_True as well. }

        for (int t = 1; t <= POVSize; ++t)
        {
            int px = 0 + VecDir[D - 1, 0] * t + POVSize;
            int py = 0 + VecDir[D - 1, 1] * t + POVSize;

            if (temp[px, py] == UPV_True)
            {
                temp[px + VecDir[D - 1, 1], py + VecDir[D - 1, 0]] = UPV_True;
                temp[px - VecDir[D - 1, 1], py - VecDir[D - 1, 0]] = UPV_True;
            }
        }
    }

    public static void UpdatePOV(FrameOfReference pov, GameBoard gb)
    {
	    //{Given the frame of reference POV, decide what can and what}
	    //{cannot actually be seen.}

        int[,] temp = new int[POVSize * 2 + 1, POVSize * 2 + 1];

        //{Error Check- make sure there's a model attached.}
        if (pov.m == null)
            return;

        //{Error Check- make sure that the range is a legal value.}
        if (pov.range > POVSize)
            pov.range = POVSize;
        else if (pov.range < 2)
            pov.range = 2;

	    //{Make sure we're in a good display area.}
	    CheckPOV(gb);

        //{Set every square in the temp array to Maybe.}
        for (int x = 0; x < POVSize * 2 + 1; ++x)
        {
            for (int y = 0; y < POVSize * 2 + 1; ++y)
            {
                temp[x, y] = UPV_Maybe;
            }
        }

        //{Set the origin to True.}
        temp[POVSize, POVSize] = UPV_True;

        //{Check the 4 cardinal directions}
        CheckLine(pov, gb, temp, 0, pov.range, pov.range);
        CheckLine(pov, gb, temp, 0, -pov.range, pov.range);
        CheckLine(pov, gb, temp, pov.range, 0, pov.range);
        CheckLine(pov, gb, temp, -pov.range, 0, pov.range);

        //{Check the 4 diagonal directions}
        CheckLine(pov, gb, temp, pov.range, pov.range, pov.range);
        CheckLine(pov, gb, temp, pov.range, -pov.range, pov.range);
        CheckLine(pov, gb, temp, -pov.range, pov.range, pov.range);
        CheckLine(pov, gb, temp, -pov.range, -pov.range, pov.range);

        for (int x = -pov.range + 1; x <= -1; ++x)
        {
            CheckLine(pov, gb, temp, x, -pov.range, pov.range);
            CheckLine(pov, gb, temp, x, pov.range, pov.range);
        }

        for (int x = pov.range - 1; x >= 1; --x)
        {
            CheckLine(pov, gb, temp, x, -pov.range, pov.range);
            CheckLine(pov, gb, temp, x, pov.range, pov.range);
        }

        for (int y = -pov.range + 1; y <= -1; ++y)
        {
            CheckLine(pov, gb, temp, pov.range, y, pov.range);
            CheckLine(pov, gb, temp, -pov.range, y, pov.range);
        }

	    for (int y = pov.range - 1; y >= 1; --y)
        {
            CheckLine(pov, gb, temp, pov.range, y, pov.range);
            CheckLine(pov, gb, temp, -pov.range, y, pov.range);
        }

	    FillOutCardinals(temp, 8);
	    FillOutCardinals(temp, 6);
	    FillOutCardinals(temp, 2);
	    FillOutCardinals(temp, 4);

        //{Copy the results from temp to the actual LOS array.}
        for (int x = 0; x < POVSize * 2 + 1; ++x)
        {
            for (int y = 0; y < POVSize * 2 + 1; ++y)
            {
                if (temp[x, y] == UPV_True)
                {
                    pov.los[x, y] = true;
                }
                else
                {
                    pov.los[x, y] = false;
                }
            }
        }
    }

    public static void ApplyPOV(FrameOfReference pov, GameBoard gb)
    {
        //{Given the frame of reference POV, copy visibilty settings to}
        //{the game board and update the screen display.}

        //{Error check- abort if the POV has no model.}
        if (pov.m == null)
            return;

        //{Loop through all of the elements in the LoS matrix.}
        for (int x = -pov.range; x <= pov.range; ++x)
        {
            for (int y = -pov.range; y <= pov.range; ++y)
            {
                if (pov.los[x + POVSize, y + POVSize])
                {
                    if (!TileVisible(gb, x + pov.m.x, y + pov.m.y))
                    {
                        //{Set the visible flag on the map.}
                        SetVisible(gb, (x + pov.m.x), (y + pov.m.y));

                        //{This square can be seen.}
                        DisplayTile(gb, x + pov.m.x, y + pov.m.y);
                    }
                    else if (texmodel.ModelPresent(gb.mog, x + pov.m.x, y + pov.m.y) ||
                             gb.itm[x + pov.m.x - 1, y + pov.m.y - 1].gfx != ' ')
                    {
                        //{There's a model or item here. Display it.}
                        DisplayTile(gb, x + pov.m.x, y + pov.m.y);
                    }
                }
            }
        }
    }

    public static WalkReport MoveModel(texmodel.Model m, GameBoard gb, int x, int y)
    {
	    //{Move model M from its current location to X,Y. Update the}
	    //{display if necessary.}
	    //{THIS PROCEDURE CHECKS FOR:      }
	    //{  - Map boundaries              }
	    //{  - Model in target square      }
	    //{  - Terrain Passability         }
   
	    //{Save the initial position of the model.}
	    int x1 = m.x;
	    int y1 = m.y;

        //{Initialize values.}
        WalkReport it = new WalkReport();
	    it.go = false;
	    it.m = null;
	    it.trap = 0;

        //{Check the destination to make sure the move can take place.}
        if (OnTheMap(x, y))
        {
            //{The target square is on the map. Continue on.}
            if (texmodel.ModelPresent(gb.mog, x, y))
            {
                //{There's a model in the target square. Check}
                //{to see if it can cohabitate or not.}
                it.m = texmodel.FindModelXY(gb.mlist, x, y);
                it.go = it.m.coHab;
            }
            else
            {
                it.go = true;
            }

            //{Check the target square for terrain concerns.}
            if (it.go)
            {
                if (rpgdice.rng.Next(1, 101) > TerrPass[GetTerr(gb, x, y)])
                {
                    it.go = false;
                }
            }
        }
        else
        {

            //{The target square is off the side of the map.}
            it.go = false;
        }

        if (it.go)
        {
            //{There's no reason why the move can't take place.}
            //{Let's do it! Move the model.}
            texmodel.SetModelLoc(m, gb.mlist, gb.mog, x, y);

            //{If this model is the player's model, update the POV.}
            if (gb.POV.m == m)
            {
                UpdatePOV(gb.POV, gb);
                ApplyPOV(gb.POV, gb);
            }

            //{Update the display}
            if (TileVisible(gb, x1, y1))
            {
                DisplayTile(gb, x1, y1);
            }
            if (TileLOS(gb.POV, x, y))
            {
                DisplayTile(gb, x, y);
            }

            //{Mention if there's a trap in this square.}
            it.trap = Math.Abs(gb.map[x - 1, y - 1].trap);
        }

        return it;
    }

    public static texmodel.Model GAddModel(GameBoard gb, char gfx, Crt.Color ac, Crt.Color bc, bool coHab, int x, int y, int kind)
    {
        //{Add a model to the game board and update the graphics.}
        //{That's what the 'G' stands for.}

        //{Actually add the model to the list. This is the easy part.}

        texmodel.Model it = texmodel.AddModel(ref gb.mlist, gb.mog, gfx, ac, bc, coHab, x, y, kind);

        //{Update the display, if within LoS.}
        if (TileLOS(gb.POV, x, y))
        {
            DisplayTile(gb, x, y);
        }

        //{Return a pointer to the model we've added.}
        return it;
    }

    public static void GRemoveModel(texmodel.Model m, GameBoard gb)
    {
	    //{As above. Remove a model from the list, then update the display.}

        //{Save the location of the model.}
	    int x = m.x;
	    int y = m.y;

        //{Check- this might be the model that the PoV is attached to!}
        if (gb.POV.m == m)
        {
            //{Set the POV's model to Nil.}
            gb.POV.m = null;
        }

	    //{Remove the model.}
	    texmodel.RemoveModel(m, ref gb.mlist, gb.mog);

	    //{Refresh the display!}
	    DisplayTile(gb, x, y);
    }

    public static void SetOverImage(GameBoard gb, int x, int y, char gfx, Crt.Color color)
    {
        //{Add an image to the map. Display it if it's currently}
        //{visible.}
        if (OnTheMap(x, y))
        {
            gb.itm[x - 1, y - 1].gfx = gfx;
            gb.itm[x - 1, y - 1].color = color;
            if (TileLOS(gb.POV, x, y))
            {
                DisplayTile(gb, x, y);
            }
        }
    }

    public static void ClearOverImage(GameBoard gb, int x, int y)
    {
        //{Dispose of the OverImage at X,Y.}
        if (OnTheMap(x, y))
        {
            gb.itm[x - 1, y - 1].gfx = ' ';
            if (TileLOS(gb.POV, x, y))
            {
                DisplayTile(gb, x, y);
            }
        }
    }

    public static int CalcObscurement(int x1, int y1, int x2, int y2, GameBoard gb)
    {
        //{Check the space between X1,Y1 and X2,Y2. Calculate the total}
        //{obscurement value of the terrain there. Return 0 for a}
        //{clear LOS, a positive number for an obscured LOS, and -1}
        //{for a completely blocked LOS.}

        int N = 0; // {The number of points on the line.}
        if (Math.Abs(x2 - x1) > Math.Abs(y2 - y1))
        {
            N = Math.Abs(x2 - x1);
        }
        else
        {
            N = Math.Abs(y2 - y1);
        }

        //{The obscurement count starts out with a value of 0.}
        int O = 0;

        //{The variable WALL represents a boundary that cannot be seen through.}
        bool Wall = false;

        Point p;
        for (int t = 1; t <= N; ++t)
        {
            //{Locate the next point on the line.}
            p = SolveLine(x1, y1, x2, y2, t);

            //{Determine the terrain of this tile.}
            int terr = GetTerr(gb, p.x, p.x);

            //{Update the Obscurement count.}
            O += TerrObscurement[terr];

            //{Increase O for models in the way.}
            int mo = 0; //{Obscurement caused by an intervening model.}
            if (texmodel.ModelPresent(gb.mog, p.x, p.x))
            {
                mo = texmodel.FindModelXY(gb.mlist, p.x, p.y).obs;
                O += mo;
            }
            else
            {
                mo = 0;
            }

            //{If this current square is a wall,}
            //{or if there is a perfectly blocking model in the square,}
            //{set Wall to true.}
            if (CantSeeThrough(gb.POV, terr) || mo == -1)
            {
                Wall = true;
            }
        }

        //{If there's a wall in the way, Obscurement := -1}
        if (Wall)
        {
            O = -1;
        }

        return O;
    }

    public static int CalcObscurement(texmodel.Model m, int x, int y, GameBoard gb)
    {
        //{Check the space between M and X,Y. Calculate the total}
        //{obscurement value of the terrain there. Return 0 for a}
        //{clear LOS, a positive number for an obscured LOS, and -1}
        //{for a completely blocked LOS.}
        return CalcObscurement(m.x, m.y, x, y, gb);
    }

    public static int CalcObscurement(texmodel.Model m1, texmodel.Model m2, GameBoard gb)
    {
        //{Check the space between M1 and M2. Calculate the total}
        //{obscurement value of the terrain there. Return 0 for a}
        //{clear LOS, a positive number for an obscured LOS, and -1}
        //{for a completely blocked LOS.}
        return CalcObscurement(m1.x, m1.y, m2.x, m2.y, gb);
    }

    public static void MapSplat(GameBoard gb, char gfx, Crt.Color color, int x, int y, bool noLOS)
    {
        //{Display a spurious character at location X,Y. Useful for shots,}
        //{explosions, and other stuff.}
        //{Set NoLOS to TRUE if you want the image printed regardless}
        //{of whether the PC can see it or not.}

        //{Check to make sure the location lies within map bounds.}
        if (OnTheMap(x, y))
        {
            //{Check to make sure that the location is visible.}
            if ((noLOS || TileLOS(gb.POV, x, y)) && OnTheScreen(gb, x, y))
            {
                //{Go to the appropriate screen coordinates.}
                Crt.GotoXY(ScreenX(gb, x), ScreenY(gb, y));

                Crt.TextColor(color);
                Crt.TextBackground(Crt.Color.Black);

                Crt.Write(gfx);
            }
        }
    }

    public static int Range(texmodel.Model m1, texmodel.Model m2)
    {
        //{Calculate the range between M1 and M2.}
        //{Pythagorean theorem.}
        int dx = m2.x - m1.x;
        int dy = m2.y - m1.y;

        return(int)Math.Round(Math.Sqrt(dx * dx + dy * dy));
    }

    public static int Range(texmodel.Model m, int x, int y)
    {
        //{Calculate the range between the model and the point.}
        //{Pythagorean theorem.}
        int dx = m.x - x;
        int dy = m.y - y;

        return (int)Math.Round(Math.Sqrt(dx * dx + dy * dy));
    }

    public static Point LocateBlock(GameBoard gb, int x1, int y1, int x2, int y2)
    {
	    //{The Line of Sight from X1,Y1 to X2,Y2 is blocked. Locate}
	    //{the point at which this happens. Keep going until either}
	    //{a wall or the edge of the map is found.}
	    int n = 1;
	    bool Wall = false;

        Point p = new Point(x1, y1);

        while (!Wall)
        {
            p = SolveLine(x1, y1, x2, y2, n);
            n += 1;
            if (p.x == 1 || p.x == texmodel.XMax || p.y == 1 || p.y == texmodel.YMax)
            {
                Wall = true;
            }
            else if (CantSeeThrough(gb.POV, GetTerr(gb, p.x, p.y)))
            {
                Wall = true;
            }
            else if (texmodel.ModelPresent(gb.mog, p.x, p.y) &&
                     texmodel.FindModelXY(gb.mlist, p.x, p.y).obs == -1)
            {
                Wall = true;
            }
        }

        return p;
    }

    public static Point LocateStop(GameBoard gb, int x1, int y1, int x2, int y2)
    {
	    //{The Line of Sight from X1,Y1 to X2,Y2 is blocked. Locate}
	    //{the point **just before** this happens. Keep going until either}
	    //{a wall or the edge of the map is found.}
	    int n = 1;
	    bool Wall = false;
        Point p0 = new Point(x1, y1);

        while (!Wall)
        {
            Point p = SolveLine(x1, y1, x2, y2, n);
            n += 1;
            if (p.x == 1 || p.x == texmodel.XMax || p.y == 1 || p.y == texmodel.YMax)
            {
                Wall = true;
            }
            else if (CantSeeThrough(gb.POV, GetTerr(gb, p.x, p.y)))
            {
                Wall = true;
            }
            else if (texmodel.ModelPresent(gb.mog, p.x, p.y) && texmodel.FindModelXY(gb.mlist, p.x, p.y).obs == -1)
            {
                Wall = true;
            }
            else
            {
                p0 = p;
            }
        }

        return p0;
    }

    public static void WriteGameBoard(GameBoard gb, StreamWriter f)
    {
	    //{Write all of the important info in GB to the file F.}
	    //{Not everything in gb will be copied- the model list,}
	    //{for instance, won't be. I'm assuming that new models}
	    //{will be generated for everything that needs one when the}
	    //{saved game is loaded.}
 
        //{First, a descriptive message.}
	    f.WriteLine("*** DeadCold GameBoard Record ***");

	    //{Output the terrain of the map, compressed using}
	    //{run length encoding.}
	    int T = gb.map[0,0].terr;
	    int C = 0;
        Point p = new Point(1, 1);

        while (p.y <= texmodel.YMax)
        {
            if (gb.map[p.x - 1, p.y - 1].terr == T)
            {
                C += 1;
            }
            else
            {
                f.WriteLine(C);
                f.WriteLine(T);
                T = gb.map[p.x - 1, p.y - 1].terr;
                C = 1;
            }

            NextPoint(ref p);
        }

	    //{Output the last terrain stretch}
	    f.WriteLine(C);
	    f.WriteLine(T);

	    f.WriteLine("***");

        //{write the traps.}
        for (int x = 1; x <= texmodel.XMax; ++x)
        {
            for (int y = 1; y <= texmodel.YMax; ++y)
            {
                if (gb.map[x - 1, y - 1].trap != 0)
                {
                    f.WriteLine(x);
                    f.WriteLine(y);
                    f.WriteLine(gb.map[x - 1, y - 1].trap);
                }
            }
        }
	    //{write the sentinel.}
	    f.WriteLine(0);

        //{write the specials.}
        for (int x = 1; x <= texmodel.XMax; ++x)
        {
            for (int y = 1; y < texmodel.YMax; ++y)
            {
                if (gb.map[x - 1, y - 1].special != 0)
                {
                    f.WriteLine(x);
                    f.WriteLine(y);
                    f.WriteLine(gb.map[x - 1, y - 1].special);
                }
            }
        }

	    //{write the sentinel.}
	    f.WriteLine(0);

	    //{Output the Visibility of the map, again using run}
	    //{length encoding. Since there are only two possible}
	    //{values, just flop between them.}
	    bool vis = false;
	    C = 0;
        p = new Point(1, 1);

        while (p.y <= texmodel.YMax)
        {
            if (gb.map[p.x - 1, p.y - 1].visible)
            {
                C += 1;
            }
            else
            {
                f.WriteLine(C);
                vis = !vis;
                C = 1;
            }

            NextPoint(ref p);
        }

	    //{Output the last terrain stretch}
	    f.WriteLine(C);
    }

    public static GameBoard ReadGameBoard(StreamReader f)
    {
        //{We're reading the gameboard from disk.}
        GameBoard gb = NewBoard();

        //{First, get rid of the descriptive message.}
        f.ReadLine();

        int x;

        Point p = new Point(1, 1);

        while (p.y <= texmodel.YMax)
        {
            int C = int.Parse(f.ReadLine()); //{Read Count}
            int T = int.Parse(f.ReadLine()); //{Read Terrain}

            //{Fill the map with this terrain up to Count.}
            for (x = 1; x <= C; ++x)
            {
                gb.map[p.x - 1, p.y - 1].terr = T;
                NextPoint(ref p);
            }
        }

        //{Read the second descriptive label.}
        f.ReadLine();

        //{Read the traps.}
        x = 0;
        do
        {
            x = int.Parse(f.ReadLine());
            if (x != 0)
            {
                int y = int.Parse(f.ReadLine());
                gb.map[x - 1, y - 1].trap = int.Parse(f.ReadLine());
            }
        }
        while (x != 0);

        //{Read the specials.}
        do
        {
            x = int.Parse(f.ReadLine());
            if (x != 0)
            {
                int y = int.Parse(f.ReadLine());
                gb.map[x - 1, y - 1].special = int.Parse(f.ReadLine());
            }
        }
        while (x != 0);


        //{Read the visibility data.}
        bool vis = false;
        p = new Point(1, 1);

        while (p.y <= texmodel.YMax)
        {
            int C = int.Parse(f.ReadLine()); //{Read Count}

            //{Fill the map with this terrain up to Count.}
            for (x = 1; x <= C; ++x)
            {
                gb.map[p.x - 1, p.y - 1].visible = vis;
                NextPoint(ref p);
            }

            vis = !vis;
        }

        return gb;
    }

    const int MapDisplayWidth = 80;  //{ Map Display Width}
    const int MapDisplayHeight = 21; //{ Map Display Height}
    const int MOX = 1; //{Map Origin X}
    const int MOY = 4; //{Map Origin Y}


    static int ScreenX(GameBoard gb, int x)
    {
        //{ This function will return the screen coordinate of map column X.}
        int DX = x - gb.POV.leftX; //{Delta X... the distance from the column to the UL corner.}
        return DX + MOX;
    }

    static int ScreenY(GameBoard gb, int y)
    {
        //{ This function will return the screen coordinate of map column X.}
        int DY = y - gb.POV.topY; //{ Delta Y... see above.}

        return DY + MOY;
    }

    static void SetVisible(GameBoard gb, int x, int y)
    {
        //{ Set the visibility flag to true on tile X,Y on the gameboard.}

        //{ Check to make sure that the point lies within the boundaries}
        //{ of the map.}
        if (OnTheMap(x, y))
        {
            gb.map[x - 1, y - 1].visible = true;
        }
    }

    static bool TileVisible(GameBoard gb, int x, int y)
    {
        //{Check the VISIBLE field of the tile in question, and}
        //{return its value.}

        if (OnTheMap(x, y))
        {
            return gb.map[x - 1, y - 1].visible;
        }

        return false;
    }

    static void RenderTile(GameBoard gb, bool norm, int x, int y)
    {
        //{ This procedure actually does the work for DisplayTile.}
        //{ Set Norm to FALSE to print the tile in reversed color.}

        //{ Error Check- Make sure that the tile in question is}
        //{ actually on the screen.}
        if (OnTheScreen(gb, x, y))
        {
            //{Goto the correct screen location.}
            Crt.GotoXY(ScreenX(gb, x), ScreenY(gb, y));

            //{If there's a model here, display that.}
            if (texmodel.ModelPresent(gb.mog, x, y) && TileLOS(gb.POV, x, y))
            {
                //{There's a model. Show it.}
                texmodel.Model m = texmodel.FindModelXY(gb.mlist, x, y);
                if (m != null)
                {
                    if (norm)
                    {
                        Crt.TextColor(m.color);
                        Crt.TextBackground(Crt.Color.Black);
                    }
                    else
                    {
                        Crt.TextBackground(m.color);
                        if (m.color == Crt.Color.White)
                            Crt.TextColor(Crt.Color.LightCyan);
                        else
                            Crt.TextColor(Crt.Color.White);
                    }

                    Crt.Write(m.gfx);
                }
            }
            else if (TileLOS(gb.POV, x, y) && gb.itm[x - 1, y - 1].gfx != ' ')
            {
                //{There's an OverImage. Show it.}
                if (norm)
                {
                    Crt.TextColor(gb.itm[x - 1, y - 1].color);
                    Crt.TextBackground(Crt.Color.Black);
                }
                else
                {
                    Crt.TextBackground(gb.itm[x - 1, y - 1].color);
                    if (gb.itm[x - 1, y - 1].color == Crt.Color.White)
                    {
                        Crt.TextColor(Crt.Color.LightCyan);
                    }
                    else
                    {
                        Crt.TextColor(Crt.Color.White);
                    }
                }
                Crt.Write(gb.itm[x - 1, y - 1].gfx);
            }
            else if (TileVisible(gb, x, y) && gb.map[x - 1, y - 1].trap > 0)
            {
                if (norm)
                {
                    Crt.TextColor(TrapColor);
                    Crt.TextBackground(Crt.Color.Black);
                }
                else
                {
                    Crt.TextBackground(TrapColor);
                    if (TrapColor == Crt.Color.White)
                    {
                        Crt.TextColor(Crt.Color.LightCyan);
                    }
                    else
                    {
                        Crt.TextColor(Crt.Color.White);
                    }
                }
                Crt.Write(TrapGfx);
            }
            else if (TileVisible(gb, x, y))
            {
                //{ There's no model. Show the terrain.}
                int t = GetTerr(gb, x, y);
                if (TileLOS(gb.POV, x, y))
                {
                    if (norm)
                    {
                        Crt.TextColor(TerrColor[t]);
                        Crt.TextBackground(Crt.Color.Black);
                    }
                    else
                    {
                        Crt.TextBackground(TerrColor[t]);
                        if (TerrColor[t] == Crt.Color.White)
                        {
                            Crt.TextColor(Crt.Color.LightCyan);
                        }
                        else
                        {
                            Crt.TextColor(Crt.Color.White);
                        }
                    }
                }
                else
                {
                    if (norm)
                    {
                        Crt.TextColor(Crt.Color.DarkGray);
                        Crt.TextBackground(Crt.Color.Black);
                    }
                    else
                    {
                        Crt.TextBackground(Crt.Color.DarkGray);
                        Crt.TextColor(Crt.Color.Black);
                    }
                }

                Crt.Write(TerrChar[t]);
            }
            else
            {
                //{This tile has not yet been revealed. Print a space.}
                if (!norm)
                {
                    Crt.TextBackground(Crt.Color.White);
                }
                Crt.Write(' ');
            }
        }

        if (!norm)
        {
            Crt.TextBackground(Crt.Color.Black);
        }
    }


    static bool CantSeeThrough(FrameOfReference pov, int terr)
    {
        //{Check the terrain in question and see whether or not the}
        //{observer can see through/over this type.}

        //{Error check- if POV is empty, can't see anything.}
        if (pov.m == null)
            return false;

        if (TerrObscurement[terr] == -1)
            return true;

        return false;
    }

    static void CheckPOV(GameBoard gb)
    {
        //{ Check to see whether or not the POV is getting close to}
        //{ the edge of the screen. If it is, recenter it.}

        //{Check to see whether or not the screen needs to be recentered.}
        bool RC = false;

        //{Calculate the current screen coordinates of the POV.}
        int SX = gb.POV.m.x - gb.POV.leftX + 1;
        int SY = gb.POV.m.y - gb.POV.topY + 1;

        //{The screen will be recentered if X or Y are within 3 squares}
        //{of the edge of the display area, and that said edge is not}
        //{the edge of the map.}
        if (SX <= 3 && gb.POV.leftX > 1)
        {
            RC = true;
        }
        else if (SX >= MapDisplayWidth - 3 && gb.POV.leftX < texmodel.XMax - MapDisplayWidth + 1)
        {
            RC = true;
        }

        if (SY <= 3 && gb.POV.topY > 1)
        {
            RC = true;
        }
        else if (SY >= MapDisplayHeight - 3 && gb.POV.topY < texmodel.YMax - MapDisplayHeight + 1)
        {
            RC = true;
        }

        if (RC)
        {
            RecenterPOV(gb);
            DisplayMap(gb);
        }
    }


    static void NextPoint(ref Point p)
    {
        //{We're stepping through the map. Increment the tile}
        //{by one. If the edge of the map is reached, start on}
        //{the other side.}
        p.x += 1;
        if (p.x > texmodel.XMax)
        {
            p.y += 1;
            p.x = 1;
        }
    }
}
