//{ This started out as just the text handling unit, but I might }
//{ as well admit that it has grown into an all-encompassing game }
//{ environment unit.It handles text output, keyboard input, text }
//{ storage, configuration file support, and probably a few other }
//{ things I've forgotten. And it's still under a thousand lines }
//{ long, so it isn't really even worth splitting it up. }

using System;
using System.IO;
using System.Collections.Generic;

public class rpgtext
{
    public struct GKeyDesc
    {
        public GKeyDesc(char key, string name, string desc)
        {
            this.key = key;
            this.dkey = key;
            this.name = name;
            this.desc = desc;
        }

        public char key;    //{ The KEY }
        public char dkey;   //{ The DEFAULT KEY }
        public string name; //{ The name of the command.}
        public string desc; //{ A description of the command, for help}
    }

    //{ This record describes a computer text message. }
    public struct TexDesc
    {
        public TexDesc(string title, string msg, int clearance, int XPV)
        {
            this.title = title;
            this.msg = msg;
            this.clearance = clearance;
            this.XPV = XPV;
            this.used = false;
        }

        public string title;
        public string msg;
        public int clearance;
        public int XPV;
        public bool used;
    }

    public const int NumTex = 29;
    public static TexDesc[] TexMan = new TexDesc[NumTex]
    {
        new TexDesc(
            "06/18/64 WARNING - SIDNEY JAMES WARNER",
            "Dr.S J Warner is wanted for questioning. CentSec has implicated him in the recent deaths of Twomas Draklor, Ed Patres and Moira Chak. Warner is a trained psycap and is thought to be carrying a weapon.",
            1, 15),
        new TexDesc(
            "04/12/64 MORGAN, DESCARTES, and THANATOS users",
            "DeadCold Central Server and Navcomp has been accessed by an intrusive alias. Please consult online documentation for more information.",
            7, 1),
        new TexDesc(
            "07/05/64 !!!ABANDON SHIP!!! CS N.Balordo",
            "Shut down Majordomo but outsider in core now. Nt much time. Pods may be corrupt. Get out. In secure room \"K\" deck- send res%#$ dhds v=4200 v2=16 v4=89711",
            30, 25),
        new TexDesc(
            "05/25/64 Software Upgrade Delayed",
            "The software upgrade which has been scheduled for the past few weeks has not yet been performed. There have been problems uninstalling the Sirius Intuition Interface from the Win2636 kernel.",
            2, 2),
        new TexDesc(
            "05/30/64 Software Upgrade Completed",
            "The scheduled upgrade to the station's kernel OS has been finished. Please familiarize yourself with the new operating procedures.",
            4, 2),

        new TexDesc(
            "05/31/64 Software Patch to be Installed",
            "We apologize for the station-wide computer problems that users have been reporting today. CompSec is working on the problem. Thank you%443=2",
            5, 3),
        new TexDesc(
            "06/11/64 Shuttle Launch Schedule",
            "Ships to Denoles leave on Thursday, Saturday, and Tuesday at 15:00. Ships to Mascan leave on Wednesday, Friday, and Monday at the same time.",
            0, 1),
        new TexDesc(
            "05/07/64 Shuttle Launch Schedule",
            "Ships to Denoles leave on Thursday, Saturday, and Tuesday at 15:00. Ships to Mascan leave on Wednesday, Friday, and Monday at the same time.",
            8, 1),
        new TexDesc(
            "06/05/64 Alien Burial Chamber on Display",
            "[AGUER] The Taltuo Collection will be on display at the museum on Deck \"D\" today. These items have been sent to DeadCold for catalogueing and analysis.",
            0, 1),
        new TexDesc(
            "06/07/64 No More Bugs!",
            "[NBALO] This is Nick at CS 2 conf -> all bugs from OS upg have been fixed. Gentlemen, to your workstations! haha",
            10, 2),

        new TexDesc(
            "06/08/64 Delta Patch Being Shipped",
            "Due to recent computer failures, station management has ordered the costly \"Delta Upgrade\" package. It should arrive by freight transport in a week or two.",
            14, 2),
        new TexDesc(
            "06/10/64 ALERT - Meningitis Outbreak",
            "There have been several apparent cases of viral meningitis among crew members. Residents and visitors to the station are advised to have all water sources tested before use.",
            0, 1),
        new TexDesc(
            "05/17/64 Welcome to our visitors from Sendai! [AGUER]",
            "We would like to extend our hospitality to the Sendai Gravesite Society, and issue our hope that they have an interesting and educational visit.",
            0, 1),
        new TexDesc(
            "07/06/64 Sorry for the Inconvenience",
            "[MD] We would like to apologize for several news bulletins recently made which were in error. There is no emergency on our station. Please report to security if you have any further concerns.",
            20, 30),
        new TexDesc(
            "05/33/64 Ferryman Destroyed in Transit",
            "[TDRAK] While en route to DeadCold the shuttle craft \"Ferryman\" met with an accident and was lost in space. As it was an automated shuttle, no lives were lost. The 262 deceased on board are not expected to be recovered.",
            0, 2),

        new TexDesc(
            "ANDROS GUERO",
            "Disrythmia: Atazine 15mg. Risk factor heart disease- reccomend more exercise.",
            1, 1),
        new TexDesc(
            "TWOMAS DRAKLOR",
            "Deceased. Massive head trauma possibly caused by gunshot. Legs mutilated after time of death. Forensics report pending.",
            7, 5),
        new TexDesc(
            "EDWARD PATRES",
            "Deceased. Blunt trauma over majority of body. Two incisions in lower abdomen. Forensics report pending.",
            3, 5),
        new TexDesc(
            "MOIRA CHAK",
            "Deceased. Multiple stab wounds to body plus significant damage to head and face. Forensics report pending.",
            5, 5),
        new TexDesc(
            "NICHOLAS BALORDO",
            "Hypertension: Nerve Relaxants 15mg. Stress: Counseling reccomended.",
            1, 1),

        new TexDesc(
            "SIDNEY JAMES WARNER",
            "Arthritis in 85% of bone mass: Solenol pain medicene 8mg, Anti-inflammatory tablets 16mg. Cybernetic replacement may be only option.",
            10, 15),
        new TexDesc(
            "DAEYOUNG PARK",
            "Ulcer, acid reflux. Stomach condition aggrevated by not following diet.",
            1, 1),
        new TexDesc(
            "[NBALO:>MD] Primary Generator Failed",
            "The primary power generator has gone down; switching emergency power to cryogenics & ordering an evacuation.",
            0, 5),
        new TexDesc(
            "[TDRAK:>NBALO] Security Upgrade",
            "Find out why our defense lasers targeted \"FERRYMAN\" as a hostile craft. We can't afford for this kind of accident to happen again.",
            5, 150),
        new TexDesc(
            "[DPARK:>CTOMS] Re:TS involved in AI?",
            "With regards to your recent inquiries, I do not believe that this is a case where it is nessecary to presuppose the existance of spirit (or even consciousness) to find the results of the \"Ventrue\" experiments useful.",
            1, 10),

        new TexDesc(
            "[MCHAK:>SWARN] DK17 Discrepency",
            "RE. the missing sample of DK17, it doesn't appear to have been transferred to our lab. Hades wouldn't turn over sensor reports for A-Mod so I'm afraid I can't be of any more help. Ask Twomas.",
            3, 15),
        new TexDesc(
            "[SWARN:>TDRAK] Sensor Logs \"A\" Module",
            "I need access to the sensor logs from JnIII to JnX. Hades claims security protocol, can't you circumvent him?",
            7, 10),
        new TexDesc(
            "06/05/64 Taltuo Artefacts now on display",
            "Several of the treasures recovered from the Taltuo burial site are now on display in the main hall.",
            0, 0),
        new TexDesc(
            "06/17/64 Museum Closed due to Infestation",
            "A recent shipment of ancient scrolls was infested with Algolian Dust-Jellies. The museum will remain in quarantine until this problem has been dealt with.",
            0, 0),
    };

    public static int DebugCode = 0;

    public static Crt.Color DCBulletColor = Crt.Color.Green;
    public static Crt.Color DCTextColor = Crt.Color.LightGreen;
    public static int DCTextBox_X1 = 1;
    public static int DCTextBox_Y1 = 1;
    public static int DCTextBox_X2 = 80;
    public static int DCTextBox_Y2 = 3;

    //{ This is the location of the more message for GamePause. }
    public static int MORE_X = DCTextBox_X2 - 5;
    public static int MORE_Y = DCTextBox_Y2;
    public static Crt.Color MORE_Color = Crt.Color.Blue;
    public static string MORE_Msg = "(more)";

    //{ ================================= }
    //{ ***  CONFIGURATION CONSTANTS  *** }
    //{ ================================= }
    //{ This is the speed used for animations. }
    public static int FrameDelay = 50;
    public static int CHART_MaxMonsters = 1500;  //{ The maximum number of monsters that can appear on the map at once}
                                                 //{ On a P2-165 laptop, lag becomes noticeable around C = 800.}
    public static int CHART_NumGenerations = 10; //{ Controls rate of monster propogation. }
    public static int PLAY_MonsterTime = 90;     //{ Controls the speed of monster propogation. }

    public static bool COMBAT_DamageCap = false; //{ If DAMAGECAP is true, damage rolls have an upper limit. }

    public static bool PLAY_DangerOn = true;     //{ Controls whether save files get deleted at character death. }

    public static string CfgName = "deadcold.cfg";

    public static GKeyDesc[] KMap = new GKeyDesc[]
    {
        new GKeyDesc('-', "NOPENOPE",     "FAKEFAKEFAKE"),
        new GKeyDesc('1', "SouthWest",    "Movement key."),
        new GKeyDesc('2', "South",        "Movement key."),
        new GKeyDesc('3', "SouthEast",    "Movement key."),
        new GKeyDesc('4', "West",         "Movement key."),
        new GKeyDesc('5', "Wait",         "Movement key."),
        new GKeyDesc('6', "East",         "Movement key."),
        new GKeyDesc('7', "NorthWest",    "Movement key."),
        new GKeyDesc('8', "North",        "Movement key."),
        new GKeyDesc('9', "NorthEast",    "Movement key."),
        new GKeyDesc('o', "OpenDoor",     "Open a door or service panel adjacent to the PC."),
        new GKeyDesc('c', "CloseDoor",    "Close a door or service panel adjacent to the PC."),
        new GKeyDesc('R', "Recenter",     "Recenter the display on the PC's current position."),
        new GKeyDesc('a', "Targeting",    "Fire the PC's missile weapon."),
        new GKeyDesc('t', "ThrowGrenade", "Throw a grenade."),
        new GKeyDesc('i', "Inventory",    "Access items in the PC's backpack."),
        new GKeyDesc('e', "Equipment",    "Access items which the PC has equipped."),
        new GKeyDesc(',', "PickUp",       "Pick up an item off the floor."),
        new GKeyDesc('D', "DisarmTrap",   "Attempt to disarm a trap."),
        new GKeyDesc('s', "Search",       "Search for secret doors and traps."),
        new GKeyDesc('z', "InvokePsi",    "Invoke one of the PC's psychic powers."),
        new GKeyDesc('Z', "QuickPsi",     "Invoke one of the PC's psychic powers using quick key."),
        new GKeyDesc('x', "CheckXP",      "Display the PC's level and experience."),
        new GKeyDesc('/', "Look",         "Identify characters on the screen."),
        new GKeyDesc('>', "Enter",        "Enter transitway, or activate terrain."),
        new GKeyDesc('.', "Repeat",       "Perform the same command repeditively."),
        new GKeyDesc('Q', "QuitGame",     "Stop playing and go do something productive."),
        new GKeyDesc('?', "Help",         "Display all these helpful messages."),
        new GKeyDesc('X', "SaveGame",     "Write all game data to disk."),
        new GKeyDesc('C', "CharInfo",     "Provide some detailed information about your character."),
        new GKeyDesc('M', "HandyMap",     "Provides a rough map of the areas you have explored already."),
    };

    public struct Key
    {
        public enum Type
        {
            ASCII,
            ARROW_UP,
            ARROW_DOWN,
            ARROW_LEFT,
            ARROW_RIGHT,
            ESC,
        };

        public Key(char c)
        {
            this.type = Type.ASCII;
            this.ascii = c;
        }

        public Key(Type t)
        {
            this.type = t;
            this.ascii = '\0';
        }

        public Type type;
        public char ascii;
    }

    public class SharedKeyBuffer
    {
        static object keypressLock = new object();
        static Stack<Key> keypresses = new Stack<Key>();

        public void AddKeypress(Key key)
        {
            lock (keypressLock)
            {
                keypresses.Push(key);
            }
        }

        public Key GetKeypress()
        {
            Key k = new Key('\0');

            bool done = false;
            do
            {
                lock (keypressLock)
                {
                    if (keypresses.Count > 0)
                    {
                        k = keypresses.Pop();
                        done = true;
                    }
                }

                if (!done)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
            while (!done);

            return k;
        }
    }

    public static SharedKeyBuffer keys = new SharedKeyBuffer();

    static int GM_X = 1; // {Game Message Cursor Pos.}
    static int GM_Y = 1;

    public static char RPGKey()
    {
        //{ Read a keypress from the keyboard. Convert it into a form}
        //{ that my other procedures would be willing to call useful.}
        Key k = keys.GetKeypress();

        char c = (char)0;

        switch (k.type)
        {
            case Key.Type.ASCII:
                c = k.ascii;
                break;
            case Key.Type.ARROW_UP:
                //c = (char)72; //{Up Cursor Key}
                c = KMap[8].key;
                break;
            case Key.Type.ARROW_DOWN:
                //c = (char)80; //{Down Cursor Key}
                c = KMap[2].key;
                break;
            case Key.Type.ARROW_LEFT:
                //c = (char)75; //{Left Cursor Key}
                c = KMap[4].key;
                break;
            case Key.Type.ARROW_RIGHT:
                //c = (char)77; //{Right Cursor Key}
                c = KMap[6].key;
                break;
            case Key.Type.ESC:
                c = (char)27;
                break;
        }

        //{Convert the Backspace character to ESCape.}
        if (c == '\b')
            c = (char)27;

        //{ Normally, SPACE is the selection button, but ENTER should}
        //{ work as well. Therefore, convert all enter codes to spaces.}
        if (c == '\r' || c == '\n')
            c = ' ';

        return c;
    }

    public static int DirKey()
    {
        //{ This procedure will input a single keypress, then return }
        //{ whatever direction was indicated. }
        //{ If the key pressed does not correspond to a direction, }
        //{ or if there were any other errors, return 0. }

        //{ Input a keypress. }
        char k = RPGKey();

        if (k == KMap[1].key) return 1;
        else if (k == KMap[2].key) return 2;
        else if (k == KMap[3].key) return 3;
        else if (k == KMap[4].key) return 4;
        else if (k == KMap[5].key) return 5;
        else if (k == KMap[6].key) return 6;
        else if (k == KMap[7].key) return 7;
        else if (k == KMap[8].key) return 8;
        else if (k == KMap[9].key) return 9;
        else return 0;
    }

    public static string ReadLine()
    {
        bool done = false;
        string line = "";

        while (!done)
        {
            Key k = keys.GetKeypress();
            if (k.type != Key.Type.ASCII)
            {
                continue;
            }

            if (k.ascii == '\n' || k.ascii == '\r')
            {
                done = true;
            }
            else if (k.ascii == (char)8)
            {
                // backspace...
                if (line.Length > 0)
                {
                    line = line.Substring(0, line.Length - 1);
                    int x = Crt.WhereX() - 1;
                    int y = Crt.WhereY();
                    Crt.GotoXY(x, y);
                    Crt.Write(' ');
                    Crt.GotoXY(x, y);
                }
            }
            else
            {
                line += k.ascii;
                Crt.Write(k.ascii);
            }
        }

        return line;
    }

    public static void GamePause()
    {
        //{ Pause the game until the player hits space.}
        Crt.GotoXY(MORE_X, MORE_Y);
        Crt.TextColor(MORE_Color);
        Crt.Write(MORE_Msg);

        char a = '\0';
        do
        {
            a = RPGKey();
        }
        while (a != ' ' && a != (char)27);

        Crt.GotoXY(MORE_X, MORE_Y);
        Crt.TextColor(Crt.Color.Black);
        Crt.Write(MORE_Msg);
    }

    public static bool YesNo()
    {
        //{ Get a Y / N answer from the player. Return TRUE for Y,}
        //{ FALSE for N.}
        char a = (char)0;
        do
        {
            a = char.ToUpper(RPGKey());
        }
        while (a != 'Y' && a != 'N');

        return a == 'Y';
    }

    public static void Delineate(string msg, int width, int offset)
    {
        //{ This is the breaking-into-lines section of the prettyprinter.}
        //{ Take a null-terminated string, MSG, and break it into}
        //{ sections of width characters or less.}
        //{ The offset parameter, if more than one, indicates the column}
        //{ that text display starts at.}

        //{ PRE-CONDITIONS: The CRT display is set up exactly as you want it with}
        //{   regards to window, text color, cursor placement, and so on.}

        if (msg == null || msg == string.Empty)
        {
            return;
        }

        int lineStart = 0;

        do
        {
            int lineBreak = lineStart;

            int wordCount = 0;
            int wordEnd = 0;

            do
            {
                wordCount += 1;

                wordEnd = msg.IndexOf(' ', lineBreak);

                if (wordEnd < 0)
                {
                    wordEnd = msg.Length;
                }

                //{if there's enough room for this to fit on a}
                //{line, it becomes linebreak.}
                //{Also, if this is the first word, print it}
                //{anyways. This should, in theory, deal with}
                //{words too long to fit on a single line.}
                if ((wordEnd - lineStart + offset - 1 <= width) || wordCount == 1)
                {
                    lineBreak = wordEnd + 1;
                }
            }
            while ((wordEnd - lineStart + offset <= width) && (lineBreak < msg.Length + 1));

            if (Crt.WhereX() != offset)
            {
                Crt.Write("\n");
            }

            string theLine = msg.Substring(lineStart, lineBreak - lineStart - 1);
            Crt.Write(theLine);

            lineStart = lineBreak;
            offset = 1;
        }
        while (lineStart < msg.Length);
    }

    public static void LovelyBox(Crt.Color edgeColor, int x1, int y1, int x2, int y2)
    {
        Crt.AutoScrollOff();

        //{ Draw a lovely box!}
        Crt.TextColor(edgeColor);

        //{ Print the four corners.}
        Crt.GotoXY(x1, y1);
        Crt.Write("+");
        Crt.GotoXY(x2, y1);
        Crt.Write("+");
        Crt.GotoXY(x1, y2);
        Crt.Write("+");
        Crt.GotoXY(x2, y2);
        Crt.Write("+");

        //{ Print the two horizontal edges.}
        for (int x = x1 + 1; x < x2; ++x)
        {
            Crt.GotoXY(x, y1);
            Crt.Write("-");
            Crt.GotoXY(x, y2);
            Crt.Write("-");
        }

        //{ Print the two vertical edges.}
        for (int y = y1 + 1; y < y2; ++y)
        {
            Crt.GotoXY(x1, y);
            Crt.Write("|");
            Crt.GotoXY(x2, y);
            Crt.Write("|");
        }
    }

    public static void GameMessage(string msg, int x1, int y1, int x2, int y2, Crt.Color textColor, Crt.Color edgeColor)
    {
        //{ Take a text string, MSG, and prettyprint }
        //{ it within the box defined by X1,Y1 - X2,Y2. }

        //{ Set the background color to black.}
        Crt.TextBackground(Crt.Color.Black);

        //{ Print the border}
        if (edgeColor != Crt.Color.Black)
        {
            LovelyBox(edgeColor, x1, y1, x2, y2);
        }

        //{ Set the window to the desired print area, and clear everything.}
        Crt.Window(x1 + 1, y1 + 1, x2 - 1, y2 - 1);
        Crt.ClrScr();

        //{ call the Delineate procedure to prettyprint it.}
        Crt.TextColor(textColor);
        Delineate(msg, x2 - x1 - 1, 1);

        //{ restore the window to its original, full dimensions.}
        Crt.Window(1, 1, 80, 25);
    }

    public static void DCNewMessage()
    {
        //{ Start a new line in the DCMessage area.}
        Crt.Window(DCTextBox_X1, DCTextBox_Y1, DCTextBox_X2, DCTextBox_Y2);

        Crt.GotoXY(GM_X, GM_Y);

        Crt.AutoScrollOn();
        if (GM_X != 1)
        {
            Crt.Write("\n");
        }
        Crt.TextColor(DCBulletColor);
        Crt.Write("> ");

        Crt.AutoScrollOff();

        //{ Reset the Cursor Pos.}
        GM_X = Crt.WhereX();
        GM_Y = Crt.WhereY();

        Crt.Window(1, 1, 80, 25);
    }

    public static void DCGameMessage(string msg, bool lf)
    {
        //{ Print a standard text message for the game GearHead.}

	    //{ Set the background color to black.}
	    Crt.TextBackground(Crt.Color.Black);

        //{ If needed, go to the next line.}
        if (lf)
        {
            DCNewMessage();
        }

        //{ Set the text color.}
        Crt.TextColor(DCTextColor);

	    //{ Set the window to the desired print area, and move to the right pos.}
	    Crt.Window(DCTextBox_X1,DCTextBox_Y1,DCTextBox_X2,DCTextBox_Y2);
	    Crt.GotoXY(GM_X, GM_Y);

        //{call the Delineate procedure to prettyprint it.}
        Crt.AutoScrollOn();
        Delineate(msg, DCTextBox_X2 - DCTextBox_X1 + 1, GM_X);
        Crt.AutoScrollOff();

        //{Save the current cursor position.}
        GM_X = Crt.WhereX();
	    GM_Y = Crt.WhereY();

	    //{restore the window to its original, full dimensions.}
	    Crt.Window(1,1,80,25);
    }

    public static void DCGameMessage(string msg)
    {
        //{ As above, but starts new message.}
        DCGameMessage(msg, true);
    }


    public static void DCAppendMessage(string msg)
    {
        //{ As above, but appends this string to the last one.}
        DCGameMessage(" " + msg, false);
    }

    public static void DCPointMessage(string msg)
    {
        //{ Display a message without affecting GM_X,GM_Y.}

        //{ Set the background color to black.}
        Crt.TextBackground(Crt.Color.Black);
        Crt.TextColor(DCTextColor);

        //{ Set the window to the desired print area, and move to the right pos.}
        Crt.Window(DCTextBox_X1, DCTextBox_Y1, DCTextBox_X2, DCTextBox_Y2);
        Crt.GotoXY(GM_X, GM_Y);

        Crt.ClrEol();

        int width = Math.Min(DCTextBox_X2 - DCTextBox_X1 - GM_X, msg.Length);
        if (width < 1)
        {
            return;
        }

        //{ Write out as much of the message as will fit on the line.}
        Crt.Write(msg.Substring(0, width));

        //{restore the window to its original, full dimensions.}
        Crt.Window(1, 1, 80, 25);
    }

    public static void SetKeyMap()
    {
        //{ Set up the key map for this game.}

        //{ Set the default keys for all commands.}
        for (int i = 1; i < KMap.Length; ++i)
        {
            KMap[i].key = KMap[i].dkey;
        }

        //{ See whether or not there's a configuration file.}
        if (File.Exists(CfgName))
        {
            Crt.Write("Loading config file...\n");

            FileStream f = File.OpenRead(CfgName);
            StreamReader r = new StreamReader(f);

            while (!r.EndOfStream)
            {
                string line = r.ReadLine();
                string[] words = line.Split(new char[] { ' ' }, 2);
                if (words.Length < 1)
                {
                    continue;
                }

                string cmd = words[0].ToUpper();

                //{ Check to see if CMD is one of the standard keys.}
                for (int i = 1; i < KMap.Length; ++i)
                {
                    if (KMap[i].name.ToUpper() == cmd)
                    {
                        if (words.Length != 2)
                        {
                            Crt.Write(string.Format("Error reading cfg file - {0} bad key.\n", cmd));
                        }
                        else
                        {
                            KMap[i].key = char.Parse(words[1]);
                        }
                    }
                }
                //{ Check to see if CMD is the animation speed throttle. }
                if (cmd == "ANIMSPEED")
                {
                    FrameDelay = int.Parse(words[1]);
                    if (FrameDelay < 0)
                        FrameDelay = 0;
                }
                else if (cmd == "NUMMONSTERS")
                {
                    CHART_MaxMonsters = int.Parse(words[1]);
                    if (CHART_MaxMonsters < 100)
                        CHART_MaxMonsters = 100;
                }
                else if (cmd == "SWARMRATE")
                {
                    CHART_NumGenerations = int.Parse(words[1]);
                    if (CHART_NumGenerations < 2)
                        CHART_NumGenerations = 2;
                }
                else if (cmd == "MONSTERTIME")
                {
                    PLAY_MonsterTime = int.Parse(words[1]);
                    if (PLAY_MonsterTime < 10)
                        PLAY_MonsterTime = 10;
                }
                else if (cmd == "SAFEMODE")
                {
                    PLAY_DangerOn = false;
                }
                else if (cmd == "DAMAGECAP")
                {
                    COMBAT_DamageCap = true;
                }
            }
        }
    }

    public static void ResetLogon()
    {
        //{ Set the USED field of each TEX entry to FALSE. }
        for (int i = 0; i < TexMan.Length; ++i)
        {
            TexMan[i].used = false;
        }
    }

    public static void SaveLogon(StreamWriter w)
    {
        //{ Write the index number of every TEX the player has accessed to }
        //{ disk, followed by a -1 sentinel. }

        for (int i = 0; i < TexMan.Length; ++i)
        {
            if (TexMan[i].used)
            {
                w.WriteLine(i + 1);
            }
        }

        w.WriteLine(-1);
    }

    public static void LoadLogon(StreamReader r)
    {
        //{ Load the player's logon data from disk. Basically, this is }
        //{ just a list of read messages. }
        int n = -1;
        do
        {
            string line = r.ReadLine();
            n = int.Parse(line);
            if (n > 0 && n <= TexMan.Length)
            {
                TexMan[n - 1].used = true;
            }
        }
        while (n > 0);
    }

    public static void PlayGame()
    {
        string saveFile = "deadcold.sav";

        if (File.Exists(saveFile))
        {
            StreamReader r = File.OpenText(saveFile);
            LoadLogon(r);
            r.Close();
        }

        SetKeyMap();

        Crt.TextColor(Crt.Color.Green);
        Crt.ClrScr();

        rpgmenus.RPGMenu menu = rpgmenus.CreateRPGMenu(Crt.Color.White, Crt.Color.Green, Crt.Color.LightGreen, 5, 3, 60, 14);
        rpgmenus.BuildFileMenu(menu, "*.*");
        string filename = rpgmenus.SelectFile(menu, rpgmenus.RPMNoCancel);

        Crt.Write("You selected the file: ");
        Crt.Write(filename);
        Crt.Write("\n");
        Crt.Write("You are about to play the game.\n");

        rpgmenus.RPGMenu items = rpgmenus.CreateRPGMenu(Crt.Color.Blue, Crt.Color.Red, Crt.Color.White, 15, 12, 40, 20);
        rpgmenus.AddRPGMenuItem(items, "World", 5);
        rpgmenus.AddRPGMenuItem(items, "Hello", 3);
        rpgmenus.AddRPGMenuItem(items, "Dingus", 2);
        rpgmenus.AddRPGMenuItem(items, "Cake", 1);
        rpgmenus.AddRPGMenuItem(items, "ZooFlower", 4);

        rpgmenus.AddRPGMenuKey(items, 'd', 3);
        rpgmenus.AddRPGMenuKey(items, 'w', 5);

        rpgmenus.RPMSortAlpha(items);
        int value = rpgmenus.SelectMenu(items, rpgmenus.RPMNormal);
        Crt.Write("You selected the value: ");
        Crt.Write(value.ToString());
        Crt.Write("\n");

        bool done = false;
        while (!done)
        {
            Crt.TextColor(Crt.Color.White);
            Crt.Write("Would you like to quit?\n(Y/N):");
            if (YesNo())
            {
                done = true;
                Crt.TextColor(Crt.Color.Red);
                Crt.Write("\nOkay... the game is over.");
            }
            else
            {
                GameMessage("nOh, goody, you've deaalsdjflaskdjfdecided to keep playing the game... That is very good news, we should play and play and play..", 5, 5, 24, 12, Crt.Color.LightGreen, Crt.Color.Blue);
                RPGKey();
                Crt.ClrScr();
                GamePause();
                Crt.TextColor(Crt.Color.LightGreen);
                Crt.Write("This is a very long line that is meant to wrap at least once... blah blhal blabhlhaslkdfasdfjalskdfjlakdjf\n\n");
                char c;
                do
                {
                    c = RPGKey();
                    Crt.Write(c.ToString());
                }
                while (c != '!');
            }
        }

        StreamWriter f = File.CreateText(saveFile);
        SaveLogon(f);
        f.Close();
    }
}
