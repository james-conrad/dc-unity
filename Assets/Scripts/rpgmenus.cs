using System;
using System.IO;

class rpgmenus
{
    //{ These two constants are used to tell the SELECT procedure whether or not}
    //{ the user is allowed to cancel.}
    public const int RPMNormal = 0;
    public const int RPMNoCancel = 1;
    public const int RPMNoCleanup = 2; //{ If you want the menu left on the screen after we've finished, use this.}

    public class RPGMenuKey
    {
        public RPGMenuKey(char k, int value)
        {
            this.k = k;
            this.value = value;
            this.next = null;
        }

        public char k;
        public int value; //{ The value returned when this key is pressed.}
        public RPGMenuKey next;
    }

    public class RPGMenuItem
    {
        public RPGMenuItem(string msg, int value, string desc)
        {
            this.msg = msg;
            this.value = value;
            this.desc = desc;
            this.next = null;
        }

        public string msg;      //{ The text which appears in the menu}
        public int value;       //{ A value, returned by SelectMenu. -1 is reserved for Cancel}
        public string desc;     //{ Item description. null if none.}
        public RPGMenuItem next;
    }

    public class RPGMenu
    {
        public RPGMenu(Crt.Color borderColor, Crt.Color itemColor, Crt.Color selColor, int x1, int y1, int x2, int y2)
        {
            this.active = false;
            this.borderColor = borderColor;
            this.itemColor = itemColor;
            this.selColor = selColor;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.dBorColor = borderColor;
            this.dTexColor = itemColor;
            this.dx1 = 0; //{ A X1 value of 0 means there is no desc window.}
            this.dy1 = 0;
            this.dx2 = 0;
            this.dy2 = 0;

            //{ TopItem refers to the highest item on the screen display.}
            this.topItem = 1;

            //{ SelectItem refers to the item currently being pointed at by the selector.}
            this.selectItem = 1;

            //{ NumItem refers to the total number of items currently in the linked list.}
            this.numItem = 0;
        }

        public bool active;
        public Crt.Color borderColor;
        public Crt.Color itemColor;
        public Crt.Color selColor;
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        //{fields relating to the optional description box.}
        public Crt.Color dBorColor;
        public Crt.Color dTexColor;
        public int dx1;
        public int dy1;
        public int dx2;
        public int dy2;
        //{fields holding info about the status of the menu.}
        public int topItem;
        public int selectItem;
        public int numItem;
        public RPGMenuItem firstItem;
        public RPGMenuKey firstKey;
    }

    public static void AddRPGMenuItem(RPGMenu menu, string msg, int value, string desc)
    {
        //{Allocate memory for it.}
        RPGMenuItem item = new RPGMenuItem(msg, value, desc);

        RPGMenuItem lastItem = LastMenuItem(menu.firstItem);
        if (lastItem != null)
        {
            lastItem.next = item;
        }
        else
        {
            menu.firstItem = item;
        }

        //{Increment the NumItem field.}
        menu.numItem++;
    }

    public static void AddRPGMenuItem(RPGMenu menu, string msg, int value)
    {
        AddRPGMenuItem(menu, msg, value, null);
    }

    public static void AddRPGMenuKey(RPGMenu menu, char k, int value)
    {
        //{ Add a dynamically defined RPGMenuKey to the menu.}

        //{ Initialize the values.}
        RPGMenuKey key = new RPGMenuKey(k, value);

        key.next = menu.firstKey;
        menu.firstKey = key;
    }

    public static RPGMenu CreateRPGMenu(Crt.Color bColor, Crt.Color iColor, Crt.Color sColor, int x1, int y1, int x2, int y2)
    {
        //{ This function creates a new RPGMenu record, and returns the address.}

        //{Allocate memory for it.}
        //{Initialize the elements of the record.}
        RPGMenu menu = new RPGMenu(bColor, iColor, sColor, x1, y1, x2, y2);

        return menu;
    }

    public static void DisplayMenu(RPGMenu menu)
    {
        //{ Display the menu on the screen.}

        //{ Error check- make sure the menu has items in it.}
        if (menu.firstItem == null)
            return;

        //{ Check to see if the user wants a border. If so, draw it.}
        if (menu.borderColor != Crt.Color.Black)
        {
            //{ Draw a LovelyBox first for the menu.}
            rpgtext.LovelyBox(menu.borderColor, menu.x1, menu.y1, menu.x2, menu.y2);
        }

        //{ Next draw a LovelyBox for the item description, if applicable.}
        if (menu.dx1 > 0)
        {
            rpgtext.LovelyBox(menu.dBorColor, menu.dx1, menu.dy1, menu.dx2, menu.dy2);
        }

        //{ Display each menu item.}
        //{ Open an appropriately sized window and clear that area.}
        Crt.Window(menu.x1 + 1, menu.y1 + 1, menu.x2 - 1, menu.y2 - 1);
        Crt.ClrScr();

        //{ Calculate the width and the height of the menu.}
        int width = menu.x2 - menu.x1 - 1;
        int height = menu.y2 - menu.y1 - 1;

        //{ Locate the top of the menu.}
        RPGMenuItem a = RPMLocateByPosition(menu, menu.topItem);

        for (int t = 1; t <= height; ++t)
        {
            //{ If we're at the currently selected item, highlight it.}
            if (((t + menu.topItem - 1) == menu.selectItem) && menu.active)
            {
                Crt.TextColor(menu.selColor);
            }
            else
            {
                Crt.TextColor(menu.itemColor);
            }

            Crt.GotoXY(1, t);
            Crt.Write(a.msg.Substring(0, Math.Min(width, a.msg.Length)));
            a = a.next;

            //{Check to see if we've prematurely encountered the end of the list.}
            if (a == null)
            {
                break;
            }
        }

        //{Restore the window to its regular size.}
        Crt.Window(1, 1, WDM.Book_WIDTH, WDM.CON_HEIGHT);

        //{If there's an associated Desc field, display it now.}
        RPMRefreshDesc(menu);
    }

    public static RPGMenuItem RPMLocateByPosition(RPGMenu menu, int i)
    {
        //{ Locate the i'th element of the item list, then return its address.}

        //{ Error check, first off.}
        if (i > menu.numItem)
        {
            Crt.Write("ERROR: RPMLocateByPosition asked to find a message that doesnt exist.\n");
            do { rpgtext.RPGKey(); } while (true);
        }

        RPGMenuItem a = menu.firstItem;
        int t = 1;

        if (i > 1)
        {
            for (t = 2; t <= i; ++t)
            {
                a = a.next;
            }
        }

        return a;
    }

    public static int SelectMenu(RPGMenu menu, int mode)
    {
        //{ This function will allow the user to browse through the menu and will}
        //{ return a value based upon the user's selection.}

        //{The menu is now active!}
        menu.active = true;

        //{Show the menu to the user.}
        DisplayMenu(menu);

        //{Initialize UK and r}
        bool UK = false;
        int r = -1;

        char getit = '\0';

        //{Start the loop. Remain in this loop until either the player makes a selection}
        //{or cancels the menu using the ESC key.}
        do
        {
            //{Read the input from the keyboard.}
            getit = rpgtext.RPGKey();

            //{Certain keys need processing- if so, process them.}
            switch (getit)
            {
                //{Selection Movement Keys}
                case (char)72:
                case '8':
                    RPMUpKey(menu);
                    break;

                case (char)80:
                case '2':
                    RPMDownKey(menu);
                    break;

                //{If we recieve an ESC, better check to make sure we're in a}
                //{cancelable menu. If not, convert the ESC to an unused key.}
                case (char)27:
                    if (mode == RPMNoCancel)
                    {
                        getit = 'Q';
                    }
                    break;
            }

            //{Check to see if a special MENU KEY has been pressed.}
            if (menu.firstKey != null)
            {
                RPGMenuKey m = menu.firstKey;
                while (m != null)
                {
                    if (getit == m.k)
                    {
                        UK = true;
                        r = m.value;
                        break;
                    }

                    m = m.next;
                }
            }

            //{Check for a SPACE or ESC.}
        }
        while ((getit != ' ') && (getit != (char)27) && !UK);

        //{The menu is no longer active.}
        menu.active = false;

        //{We have to send back a different value depending upon whether a selection}
        //{was made or the menu was cancelled. If an item was selected, return its}
        //{value field. The value always returned by a cancel will be -1.}
        //{If a MenuKey was pressed, r already contains the right value.}
        if (getit == ' ')
        {
            r = RPMLocateByPosition(menu, menu.selectItem).value;
        }

        if (mode != RPMNoCleanup)
        {
            //{Remove the menu from the display. I'm gonna use Window for this, since}
            //{ClrScr in this language doesn't take paramters. Bummer.}

            //{Check to see whether or not a border was used.}
            if (menu.borderColor == Crt.Color.Black)
            {
                Crt.Window(menu.x1 + 1, menu.y1 + 1, menu.x2 - 1, menu.y2 - 1);
                Crt.ClrScr();
            }
            else
            {
                Crt.Window(menu.x1, menu.y1, menu.x2, menu.y2);
                Crt.ClrScr();
            }

            //{If there's an associated description box, clear that too.}
            if (menu.dx1 > 0)
            {
                Crt.Window(menu.dx1, menu.dy1, menu.dx2, menu.dy2);
                Crt.ClrScr();
            }
        }

        //{Reset the window to normal values}
        Crt.Window(1, 1, WDM.CON_WIDTH, WDM.CON_HEIGHT);

        return r;
    }

    public static void RPMSortAlpha(RPGMenu menu)
    {
        //{Given a menu, RPM, sort its items based on the alphabetical}
        //{order of their msg fields.}
        //{I should mention here that I haven't written a sorting}
        //{algorithm in years, and only once on a linked list (CS assignment).}
        //{I think this is an insertion sort... I checked on internet for}
        //{examples of sorting techniques, found a bunch of contradictory}
        //{information, and decided to just write the easiest thing that}
        //{would work. Since we're dealing with a relatively small number}
        //{of items here, speed shouldn't be that big a concern.}

        //{Initialize A and Sorted.}
        RPGMenuItem a = menu.firstItem;
        RPGMenuItem sorted = null;

        while (a != null)
        {
            RPGMenuItem b = a; //{ b is to be added to sorted}
            a = a.next;        //{ increase A to the next item in the menu}

            //{ Give b's Next field a value of null.}
            b.next = null;

            //{Locate the correct position in Sorted to store b}
            if (sorted == null)
            {
                //{This is the trivial case- Sorted is empty.}
                sorted = b;
            }
            else if (string.Compare(b.msg, sorted.msg, true) < 0)
            {
                //{b should be the first element in the list.}
                RPGMenuItem c = sorted;
                sorted = b;
                sorted.next = c;
            }
            else
            {
                //{c and d will be used to move through Sorted.}
                RPGMenuItem c = sorted;
                RPGMenuItem d;

                //{Locate the last item lower than b}
                bool youshouldstop = false;
                do
                {
                    d = c;
                    c = c.next;

                    if (c == null)
                    {
                        youshouldstop = true;
                    }
                    else if (string.Compare(b.msg, c.msg, true) < 0)
                    {
                        youshouldstop = true;
                    }
                }
                while (!youshouldstop);

                b.next = c;
                d.next = b;
            }
        }

        menu.firstItem = sorted;
    }

    public static void BuildFileMenu(RPGMenu menu, string searchPattern)
    {
        //{ Do a DosSearch for files matching SearchPattern, then add }	
        //{ each of the files found to the menu. }

        string[] files = Directory.GetFiles(".", searchPattern);

        int N = 1;

        foreach (string file in files)
        {
            AddRPGMenuItem(menu, Path.GetFileNameWithoutExtension(file), N);
            N += 1;
        }
    }

    public static string SelectFile(RPGMenu menu, int mode = RPMNormal)
    {
        //{ RPM is a menu created by the BuildFileMenu procedure. }
        //{ So, select one of the items and return the item name, which }
        //{ should be a filename. }

        //{ Do the menu selection first. }
        int N = SelectMenu(menu, mode);

        if (N == -1)
        {
            //{ Selection was canceled. So, return an empty string. }
            return "";
        }

        //{ Locate the selected element of the menu. }
        return RPMLocateByPosition(menu, menu.selectItem).msg;
    }


    static RPGMenuItem LastMenuItem(RPGMenuItem item)
    {
        //{ This procedure will find the last item in the linked list.}

        if (item == null)
            return null;

        //{ While the menu item is pointing to a next menu item, it's not the last. duh.}
        //{ So, move through the list until we hit a null.}
        while (item.next != null)
        {
            //{Check the next one.}
            item = item.next;
        }
        //{ We've found a MI.next = Nil. Yay! Return it.}
        return item;
    }


    static void RPMRefreshDesc(RPGMenu menu)
    {
        //{ Refresh the menu description box, if appropriate.}

        //{ Check to make sure that this menu has a description box, first off.}
        if (menu.dx1 > 0)
        {
            Crt.Window(menu.dx1 + 1,
                       menu.dy1 + 1,
                       menu.dx2 - 1,
                       menu.dy2 - 1);
            Crt.ClrScr();
            Crt.TextColor(menu.dTexColor);
            rpgtext.Delineate(RPMLocateByPosition(menu, menu.selectItem).desc, menu.dx2 - menu.dx1 - 1, 1);
            Crt.Window(1, 1, WDM.CON_WIDTH, WDM.CON_HEIGHT);
        }
    }

    static void RPMReposition(RPGMenu menu)
    {
        //{ The selected item has just changed, and is no longer visible on screen.}
        //{ Adjust the RPGMenu's topitem field to an appropriate value.}

        //{ When this function is called, there are two possibilities: either the}
        //{ selector has moved off the bottom of the page or the top.}

        //{ Calculate the height of the menu.}
        int height = menu.y2 - menu.y1 - 1;

        if (menu.selectItem < menu.topItem)
        {
            //{ The selector has moved off the bottom of the list. The new page}
            //{ display should start with SelectItem on the bottom.}
            menu.topItem = menu.selectItem - height + 1;

            //{ Error check- if this moves topitem below 1, that's bad.}
            if (menu.topItem < 1)
            {
                menu.topItem = 1;
            }
        }
        else
        {
            //{The selector has moved off the top of the list. The new page should}
            //{start with SelectItem at the top, unless this would make things look}
            //{funny.}
            if ((menu.selectItem + height - 1) > menu.numItem)
            {
                //{There will be whitespace at the bottom of the menu if we assign}
                //{SelectItem to TopItem. Make TopItem equal to the effective last}
                //{page.}
                menu.topItem = menu.numItem - height + 1;
                if (menu.topItem < 1)
                {
                    menu.topItem = 1;
                }
            }
            else
            {
                menu.topItem = menu.selectItem;
            }
        }
    }

    static void RPMUpKey(RPGMenu menu)
    {
        //{Someone just pressed the UP key, and we're gonna process that input.}
        //{PRECONDITIONS: menu has been initialized properly, and is currently being}
        //{  displayed on the screen.}

        //{Lets set up the window.}
        Crt.Window(menu.x1 + 1, menu.y1 + 1, menu.x2 - 1, menu.y2 - 1);

        //{Calculate the width of the menu.}
        int width = menu.x2 - menu.x1 - 1;

        //{De-indicate the old selected item.}
        //{Change color to the regular item color...}
        Crt.TextColor(menu.itemColor);
        //{Then reprint the text of the previously selected item.}
        Crt.GotoXY(1, menu.selectItem - menu.topItem + 1);
        string msg = RPMLocateByPosition(menu, menu.selectItem).msg;
        Crt.Write(msg.Substring(0, Math.Min(width, msg.Length)));

        //{Decrement the selected item by one.}
        menu.selectItem--;

        //{If this causes it to go beneath one, wrap around to the last item.}
        if (menu.selectItem == 0)
        {
            menu.selectItem = menu.numItem;
        }

        //{If the movement takes the selected item off the screen, do a redisplay.}
        //{Otherwise, indicate the newly selected item.}
        if ((menu.selectItem < menu.topItem) || ((menu.selectItem - menu.topItem) > (menu.y2 - menu.y1 - 2)))
        {
            //{First, restore the normal window size, since DisplayMenu will try to resize it.}
            Crt.Window(1, 1, WDM.CON_WIDTH, WDM.CON_HEIGHT);

            //{Determine an appropriate new value for topitem.}
            RPMReposition(menu);

            //{Redisplay the menu.}
            DisplayMenu(menu);
        }
        else
        {
            Crt.TextColor(menu.selColor);
            Crt.GotoXY(1, menu.selectItem - menu.topItem + 1);
            msg = RPMLocateByPosition(menu, menu.selectItem).msg;
            Crt.Write(msg.Substring(0, Math.Min(width, msg.Length)));

            //{Restore the window to its regular size.}
            Crt.Window(1, 1, WDM.CON_WIDTH, WDM.CON_HEIGHT);

            //{If this menu features item descriptions, better refresh the text.}
            if (menu.dx1 > 0)
            {
                RPMRefreshDesc(menu);
            }
        }
    }

    static void RPMDownKey(RPGMenu menu)
    {
        //{Someone just pressed the DOWN key, and we're gonna process that input.}
        //{PRECONDITIONS: menu has been initialized properly, and is currently being}
        //{  displayed on the screen.}

        //{Lets set up the window.}
        Crt.Window(menu.x1 + 1, menu.y1 + 1, menu.x2 - 1, menu.y2 - 1);

        //{Calculate the width of the menu.}
        int width = menu.x2 - menu.x1 - 1;

        //{De-indicate the item.}
        //{Change color to the normal text color, then reprint the item's message.}
        Crt.TextColor(menu.itemColor);
        Crt.GotoXY(1, menu.selectItem - menu.topItem + 1);
        string msg = RPMLocateByPosition(menu, menu.selectItem).msg;
        Crt.Write(msg.Substring(0, Math.Min(width, msg.Length)));

        //{Increment the selected item.}
        menu.selectItem++;
        //{If this takes the selection out of bounds, restart at the first item.}
        if (menu.selectItem == menu.numItem + 1)
        {
            menu.selectItem = 1;
        }

        //{If the movement takes the selected item off the screen, do a redisplay.}
        //{Otherwise, indicate the newly selected item.}
        if ((menu.selectItem < menu.topItem) || ((menu.selectItem - menu.topItem) > (menu.y2 - menu.y1 - 2)))
        {
            //{First, restore the normal window size, since DisplayMenu will try to resize it.}
            Crt.Window(1, 1, WDM.CON_WIDTH, WDM.CON_HEIGHT);

            //{Determine an appropriate new value for topitem.}
            RPMReposition(menu);

            //{Redisplay the menu.}
            DisplayMenu(menu);
        }
        else
        {
            Crt.TextColor(menu.selColor);
            Crt.GotoXY(1, menu.selectItem - menu.topItem + 1);
            msg = RPMLocateByPosition(menu, menu.selectItem).msg;
            Crt.Write(msg.Substring(0, Math.Min(width, msg.Length)));

            //{Restore the window to its regular size.}
            Crt.Window(1, 1, WDM.CON_WIDTH, WDM.CON_HEIGHT);

            //{If this menu features item descriptions, better refresh the text.}
            if (menu.dx1 > 0)
            {
                RPMRefreshDesc(menu);
            }
        }
    }
}