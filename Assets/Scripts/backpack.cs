using System;

public class backpack
{
	/*This unit handles the PC's Inventory UI.*/

    public const Crt.Color DscColor = Crt.Color.LightGreen;

	/*MenuKey Constants*/
	public const char BMK_SwitchKey = '/';
    public const int BMK_SwitchCode = -2;
    public const char BMK_DropKey = 'd';
    public const int BMK_DropCode = -3;


    public static void HandyMap(gamebook.Scenario SC)
    {
        /* The PC is about to use his HANDYMAP automatic mapping system. */

        /* Start by doing the HANDYMAP case display. */
        const int X1 = WDM.HM_X;
        const int X2 = WDM.HM_X2;
        const int Y1 = WDM.HM_Y;
        const int Y2 = WDM.HM_Y2;

        Crt.Window(X1, Y1, X2, Y2);
        Crt.ClrScr();
        Crt.Window(1, 1, WDM.CON_WIDTH, WDM.CON_HEIGHT);
        rpgtext.LovelyBox(Crt.Color.Red, X1, Y1, X2, Y2);
        rpgtext.LovelyBox(Crt.Color.LightGray, X1 + 1, Y1 + 2, X2 - 1, Y2 - 1);
        Crt.TextColor(Crt.Color.LightRed);
        Crt.TextBackground(Crt.Color.Black);
        Crt.GotoXY(X1 + 1, Y1 + 1);
        Crt.Write("X     HANDYMAP v3.14");

        /* Next, go through the map in 16 x 16 blocks. */
        for (int X = 1; X <= texmodel.XMax / 8; ++X)
        {
            for (int Y = 1; Y < texmodel.YMax / 16; ++Y)
            {
                int Rev = 0;
                char C = ' ';

                for (int XX = X * 8 - 7; XX <= X * 8; ++XX)
                {
                    for (int YY = Y * 16 - 15; YY <= Y * 16; ++YY)
                    {
                        /* If this tile has been seen by the PC, increment */
                        /* the visible counter. */
                        if (SC.gb.map[XX - 1, YY - 1].visible)
                        {
                            Rev += 1;

                            /* If this tile contains a special tile, */
                            /* store it here. */
                            switch (SC.gb.map[XX - 1, YY - 1].terr)
                            {
                                case texmaps.TransitLeft: C = '<'; break;
                                case texmaps.TransitRight: C = '>'; break;
                                case texmaps.TransitUp: C = '^'; break;
                                case texmaps.TransitDown: C = 'v'; break;
                                case texmaps.PilotsChair: C = 'S'; break;
                                case texmaps.ForceField: C = 'F'; break;
                                case texmaps.ForceFieldGenerator: C = 'g'; break;
                            }
                        }
                    }
                }

                /* Set colors - if this is the map block containing the */
                /* pilot, reverse the colors. */
                if (SC.PC.m.x > (X - 1) * 8 && SC.PC.m.x <= X * 8 && SC.PC.m.y > (Y - 1) * 16 && SC.PC.m.y <= Y * 16)
                {
                    Crt.TextColor(Crt.Color.Black);
                    Crt.TextBackground(Crt.Color.LightGreen);
                }
                else
                {
                    Crt.TextColor(Crt.Color.Green);
                    Crt.TextBackground(Crt.Color.Black);
                }

                /* Print the decided-upon character. */
                Crt.GotoXY(X + 1 + X1, Y + 2 + Y1);
                if (C != ' ') Crt.Write(C);
                else if (Rev > 75) Crt.Write('#');
                else if (Rev > 50) Crt.Write('=');
                else if (Rev > 25) Crt.Write('-');
                else if (Rev > 0) Crt.Write('.');
                else Crt.Write(' ');
            }
        }

        /* Wait for a keypress. */
        rpgtext.RPGKey();
    }

    public static void Inventory(gamebook.Scenario SC, bool StartWithInv)
    {
	    /*This procedure opens up the PC's inventory display,*/
	    /*and allows all the standard RPG options, such as*/
	    /*equipping items, dropping items, etc.*/

	    /*Set up the display.*/
	    TheDisplay(SC);

	    /*Initialize misc values.*/
	    bool LC = true;

	    /*Create the Equipment menu*/
	    CreateEqpMenu(SC);

	    /*Create the Inventory menu*/
	    CreateInvMenu(SC);

	    /*Display both menus*/
	    rpgmenus.DisplayMenu(EqpRPM);
	    rpgmenus.DisplayMenu(InvRPM);

	    /*Begin loop here.*/
	    while (LC)
        {
            /*Query the active menu.*/
            if (StartWithInv)
                LC = InvMenu(SC);
            else
                LC = EqpMenu(SC);

		    /*Switch to the other menu*/
		    StartWithInv = !StartWithInv;
	    }

        /*Release the two menus that we created.*/
        EqpRPM = null;
        InvRPM = null;

        /*Restore the display.*/
        int X1 = Math.Min(WDM.EqpWin_X, Math.Min(WDM.DscWin_X, Math.Min(WDM.PCSWin_X, WDM.InvWin_X)));
        int Y1 = Math.Min(WDM.EqpWin_Y, Math.Min(WDM.DscWin_Y, Math.Min(WDM.PCSWin_Y, WDM.InvWin_Y)));
        int X2 = Math.Max(WDM.EqpWin_X2, Math.Max(WDM.DscWin_X2, Math.Max(WDM.PCSWin_X2, WDM.InvWin_X2)));
        int Y2 = Math.Max(WDM.EqpWin_Y2, Math.Max(WDM.DscWin_Y2, Math.Max(WDM.PCSWin_Y2, WDM.InvWin_Y2)));
        Crt.Window(X1, Y1, X2, Y2);
	    Crt.ClrScr();
	    Crt.Window(1,1,WDM.CON_WIDTH,WDM.CON_HEIGHT);
    }

    public static dcitems.DCItem PromptItem(gamebook.Scenario SC, int IK)
    {
        /*Create a menu, then query the user for an item which*/
        /*corresponds to the kind IK. Return null if either no*/
        /*such items are present in the inventory, or if the user*/
        /*cancels item selection. Retore map display afterwards.*/

        /*Create the menu. It's gonna use the InvWindow.*/
        rpgmenus.RPGMenu RPM = rpgmenus.CreateRPGMenu(Crt.Color.LightGray, Crt.Color.Green, Crt.Color.LightGreen, 16, 7, 65, 21);

        /*Add one menu item for each appropriate item in the Inventory.*/
        dcitems.DCItem i = SC.PC.inv;
        int t = 1;
        while (i != null)
        {
            if (i.ikind == IK)
            {
                rpgmenus.AddRPGMenuItem(RPM, dcitems.ItemNameLong(i), t, null);
            }
            i = i.next;
            t += 1;
        }

        /*Error check- make sure there are items present in the list!!!*/
        if (RPM.firstItem == null)
            return null;

        /*Sort the menu alphabetically.*/
        rpgmenus.RPMSortAlpha(RPM);

        /*Next, select the item.*/
        t = rpgmenus.SelectMenu(RPM, rpgmenus.RPMNormal);
        if (t == -1)
            i = null;
        else
            i = dcitems.LocateItem(SC.PC.inv, t);

        /*Restore the map display.*/
        texmaps.DisplayMap(SC.gb);

        return i;
    }

	static rpgmenus.RPGMenu InvRPM = null;
	static rpgmenus.RPGMenu EqpRPM = null;



    static void CreateEqpMenu(gamebook.Scenario SC)
    {
        /*Create the equipment menu, and store it in EqpRPM*/
        /*Initialize the menu.*/
        EqpRPM = rpgmenus.CreateRPGMenu(Crt.Color.Black, Crt.Color.Green, Crt.Color.LightGreen, WDM.EqpWin_X, WDM.EqpWin_Y, WDM.EqpWin_X2, WDM.EqpWin_Y2);

        EqpRPM.dBorColor = Crt.Color.White;
        EqpRPM.dTexColor = DscColor;
        EqpRPM.dx1 = WDM.DscWin_X;
        EqpRPM.dy1 = WDM.DscWin_Y;
        EqpRPM.dx2 = WDM.DscWin_X2;
        EqpRPM.dy2 = WDM.DscWin_Y2;

        /*Add the MenuKeys.*/
        rpgmenus.AddRPGMenuKey(EqpRPM, BMK_SwitchKey, BMK_SwitchCode);

        /*Add one MenuItem for each Equipment Slot.*/
        for (int t = 1; t <= dcchars.NumEquipSlots; ++t)
        {
            string m = dcchars.EquipSlotName[t - 1];
            if (SC.PC.eqp[t - 1] != null)
            {
                m = m + " " + dcitems.ItemNameLong(SC.PC.eqp[t - 1]);
            }
            rpgmenus.AddRPGMenuItem(EqpRPM, m, t, dcitems.ItemDesc(SC.PC.eqp[t - 1]));
        }
    }

    static void CreateInvMenu(gamebook.Scenario SC)
    {
        /*Create the inventory menu, and store it in InvRPM*/

        /*Initialize the menu.*/
        InvRPM = rpgmenus.CreateRPGMenu(Crt.Color.Black, Crt.Color.Green, Crt.Color.LightGreen, WDM.InvWin_X, WDM.InvWin_Y, WDM.InvWin_X2, WDM.InvWin_Y2);

        InvRPM.dBorColor = Crt.Color.White;
        InvRPM.dTexColor = DscColor;
        InvRPM.dx1 = WDM.DscWin_X;
        InvRPM.dy1 = WDM.DscWin_Y;
        InvRPM.dx2 = WDM.DscWin_X2;
        InvRPM.dy2 = WDM.DscWin_Y2;

        /*Add the MenuKeys.*/
        rpgmenus.AddRPGMenuKey(InvRPM, BMK_SwitchKey, BMK_SwitchCode);
        rpgmenus.AddRPGMenuKey(InvRPM, BMK_DropKey, BMK_DropCode);

        /*Add a MenuItem for each object in the player's inventory.*/
        dcitems.DCItem i = SC.PC.inv;
        int t = 1;
        while (i != null)
        {
            rpgmenus.AddRPGMenuItem(InvRPM, dcitems.ItemNameLong(i), t, dcitems.ItemDesc(i));
            i = i.next;
            t += 1;
        }

        /*Sort the menu alphabetically.*/
        rpgmenus.RPMSortAlpha(InvRPM);
    }

    static void DisplayPCStats(gamebook.Scenario SC)
    {
	    /*Do a quick display of several of the PC's stats.*/
        const int C1 = 2;
	    const int C2 = 6;
	    const int C3 = 12;
	    const int C4 = 16;
	    const int C5 = 22;
	    const int C6 = 26;

	    Crt.Window(WDM.PCSWin_X + 1,WDM.PCSWin_Y + 1,WDM.PCSWin_X2 - 1, WDM.PCSWin_Y2 - 1);
	    Crt.ClrScr();
	    Crt.TextColor(Crt.Color.Blue);
	    Crt.GotoXY(C1,1);
	    Crt.Write("H2H");
	    Crt.GotoXY(C3,1);
	    Crt.Write("Dmg");
	    Crt.GotoXY(C1,2);
	    Crt.Write("Gun");
	    Crt.GotoXY(C3,2);
	    Crt.Write("Dmg");
	    Crt.GotoXY(C5,2);
	    Crt.Write("Rng");
	    Crt.GotoXY(C1,3);
	    Crt.Write("Armor");

	    Crt.TextColor(Crt.Color.LightBlue);
	    Crt.GotoXY(C2,1);
        Crt.Write(dcchars.PCMeleeSkill(SC.PC).ToString());
	    Crt.GotoXY(C4,1);
	    Crt.Write(dcchars.PCMeleeDamage(SC.PC).ToString());
	    Crt.GotoXY(C2,2);
	    Crt.Write(dcchars.PCMissileSkill(SC.PC).ToString());
	    Crt.GotoXY(C4,2);
	    Crt.Write(dcchars.PCMissileDamage(SC.PC).ToString());
	    Crt.GotoXY(C6,2);
	    Crt.Write(dcchars.PCMissileRange(SC.PC).ToString());
	    Crt.GotoXY(C1+7,3);
	    Crt.Write(dcchars.PCArmorPV(SC.PC).ToString());

	    Crt.Window(1,1,WDM.CON_WIDTH,WDM.CON_HEIGHT);
    }

    static void TheDisplay(gamebook.Scenario SC)
    {
        /*This procedure sets up the BackPack display.*/
        Crt.Window(WDM.EqpWin_X, WDM.EqpWin_Y, WDM.PCSWin_X2, WDM.InvWin_Y2);
        Crt.ClrScr();
        Crt.Window(1, 1, WDM.CON_WIDTH, WDM.CON_HEIGHT);
        rpgtext.LovelyBox(Crt.Color.LightGray, WDM.EqpWin_X, WDM.EqpWin_Y, WDM.InvWin_X2, WDM.InvWin_Y2);
        Crt.TextColor(Crt.Color.Green);
        Crt.GotoXY(WDM.EqpWin_X + 2, WDM.EqpWin_Y2);

        Crt.TextColor(Crt.Color.LightGray);
        for (int t = 1; t <= WDM.EqpWin_WIDTH - 3; ++t)
            Crt.Write((char)205);
        Crt.TextColor(Crt.Color.Green);

        rpgtext.LovelyBox(Crt.Color.DarkGray, WDM.PCSWin_X, WDM.PCSWin_Y, WDM.PCSWin_X2, WDM.PCSWin_Y2);
        DisplayPCStats(SC);
        Crt.TextColor(Crt.Color.DarkGray);
        Crt.GotoXY(WDM.PCSWin_X, WDM.PCSWin_Y2 + 1);
        Crt.Write("/ - Mode  d - Drop");
        Crt.GotoXY(WDM.PCSWin_X, WDM.PCSWin_Y2 + 2);
        Crt.Write("[SPACE] - Default Item Action");
        Crt.GotoXY(WDM.PCSWin_X, WDM.PCSWin_Y2 + 3);
        Crt.Write("[ESC] - Exit");
    }

    static void RefreshBackPack(gamebook.Scenario SC)
    {
        /*Something has changed in the inventory/equipment lists.*/
        /*update the menus and the screen display to deal with this.*/

        /*Error check- exit immediately if EqpRPM or InvRPM are NIL.*/
        if (EqpRPM == null || InvRPM == null)
            return;

        /*Save the SelectItem number so that we can restore it later.*/
        int S = EqpRPM.selectItem;

        /*Create the Equipment menu*/
        CreateEqpMenu(SC);

        EqpRPM.selectItem = S;

        /*Save the number of items and selected item of the Inv menu.*/
        int N = InvRPM.numItem;
        S = InvRPM.selectItem;

        /*Create the Inventory menu*/
        CreateInvMenu(SC);

        if (InvRPM.numItem == N)
            InvRPM.selectItem = S;

        /*Display both menus*/
        rpgmenus.DisplayMenu(EqpRPM);
        rpgmenus.DisplayMenu(InvRPM);

        /* Display PC stats. */
        gamebook.PCStatLine(SC);
    }

    static dcitems.DCItem SelectItem(gamebook.Scenario SC, int IK)
    {
        /*Create a menu, then query the user for an item which*/
        /*corresponds to the kind IK. Return null if either no*/
        /*such items are present in the inventory, or if the user*/
        /*cancels item selection.*/

        //var
        // RPM: RPGMenuPtr;	/*Our menu.*/
        // i: DCItemPtr;
        // t: Integer;

        /*Create the menu. It's gonna use the InvWindow.*/
        rpgmenus.RPGMenu RPM = rpgmenus.CreateRPGMenu(Crt.Color.Black, Crt.Color.Green, Crt.Color.LightGreen, WDM.InvWin_X, WDM.InvWin_Y, WDM.InvWin_X2, WDM.InvWin_Y2);
        RPM.dBorColor = Crt.Color.White;
        RPM.dTexColor = DscColor;
        RPM.dx1 = WDM.DscWin_X;
        RPM.dy1 = WDM.DscWin_Y;
        RPM.dx2 = WDM.DscWin_X2;
        RPM.dy2 = WDM.DscWin_Y2;

        /*Add one menu item for each appropriate item in the Inventory.*/
        dcitems.DCItem i = SC.PC.inv;
        int t = 1;
        while (i != null)
        {
            if (i.ikind == IK)
                rpgmenus.AddRPGMenuItem(RPM, dcitems.ItemNameLong(i), t, dcitems.ItemDesc(i));
            i = i.next;
            t += 1;
        }

        /*Error check- make sure there are items present in the list!!!*/
        if (RPM.firstItem == null)
        {
            return null;
        }

        /*Sort the menu alphabetically.*/
        rpgmenus.RPMSortAlpha(RPM);

        /*Next, select the item.*/
        t = rpgmenus.SelectMenu(RPM, rpgmenus.RPMNormal);
        if (t == -1)
            i = null;
        else
            i = dcitems.LocateItem(SC.PC.inv, t);

        /*Show the complete inventory list again.*/
        rpgmenus.DisplayMenu(InvRPM);

        return i;
    }

    static void UnEquipItem(gamebook.Scenario SC, int Slot)
    {
        /*UnEquip the item in slot Slot in the PC's equipment list.*/

        dcitems.DCItem I = SC.PC.eqp[Slot - 1];
        if (I != null)
        {
            SC.PC.eqp[Slot - 1] = null;
            dcitems.MergeDCItem(ref SC.PC.inv, I);
            RefreshBackPack(SC);
            DisplayPCStats(SC);
        }
    }

    static void EquipItem(gamebook.Scenario SC, dcitems.DCItem I)
    {
        /*Delink this item from the main Inventory list, then stick*/
        /*it in the appropriate equipment slot. If there's already*/
        /*an item there, unequip it.*/

        if (I.ikind > 0 && I.ikind <= dcchars.NumEquipSlots)
        {
            /*If something is already equipped, get rid of it.*/
            if (SC.PC.eqp[I.ikind - 1] != null)
                UnEquipItem(SC, I.ikind);

            /*Delink the item we're equipping from the Inventory.*/
            dcitems.DelinkDCItem(ref SC.PC.inv, I);

            /*Link it to the correct inventory slot.*/
            SC.PC.eqp[I.ikind - 1] = I;
            RefreshBackPack(SC);
            DisplayPCStats(SC);
        }
    }

    static void ChangeItem(gamebook.Scenario SC, int Slot)
    {
	    /*Change the item that's currently equipped in equipment*/
	    /*slot Slot. If there are other items that could go there,*/
	    /*select one of them for use. If not, just unequip the item.*/

	    /*UnEquip the item in the slot.*/
	    if (SC.PC.eqp[Slot - 1] != null)
            UnEquipItem(SC, Slot);

	    /*Select a new item, of appropriate type, from the menu.*/
	    dcitems.DCItem I = SelectItem(SC, Slot);

	    /*Equip it. Any item currently in this slot will be sent to*/
	    /*the Inventory.*/
	    if (I != null)
            EquipItem(SC, I);

	    RefreshBackPack(SC);
	    DisplayPCStats(SC);
    }

    static void LoadAmmo(gamebook.Scenario SC, dcitems.DCItem I)
    {
        /*Load ammunition item I into the currently equipped gun.*/
        /*Fill the gun to its full capacity, or as full as it can*/
        /*get given the current number of cartridges in inventory.*/
        /*If the gun is currently loaded with a different ammo type,*/
        /*unload that ammo. If the selected ammo won't fit in the*/
        /*current gun, choose a different gun from the inventory.*/

        //   var
        //cal,spec: Integer;
        //gun,ul: DCItemPtr;
        //N: Integer;
        //ID: bool;

        /*Determine the Caliber and Special Type of the ammo.*/
        int cal = I.icode % 100;
        int spec = I.icode / 100;
        bool ID = I.ID;

        dcitems.DCItem gun = SC.PC.eqp[dcchars.ES_MissileWeapon - 1];

        if (gun == null || dcitems.CGuns[gun.icode - 1].caliber != cal)
        {
            /*The gun currently equipped is either inappropriate*/
            /*or doesn't exist. Either way, we need to choose a new gun.*/
            gun = SelectItem(SC, dcitems.IKIND_Gun);
        }

        if (gun == null || dcitems.CGuns[gun.icode - 1].caliber != cal || gun.charge == -1)
            return;

        /*We have a gun to load. Let's get to it!*/

        /*If the gun is currently loaded with a different sort of*/
        /*ammunition, unload it.*/
        if (gun.state != spec && gun.charge > 0)
        {
            if (cal != dcitems.CAL_Energy && cal != dcitems.CAL_Napalm)
            {
                dcitems.DCItem UL = new dcitems.DCItem();
                UL.ikind = dcitems.IKIND_Ammo;
                UL.icode = (Math.Abs(gun.state) * 100) + cal;
                UL.charge = gun.charge;
                gun.charge = 0;

                if (gun.state < 0)
                    UL.ID = false;

                dcitems.MergeDCItem(ref SC.PC.inv, UL);
            }
            else
            {
                gun.charge = 0;
                gun.state = 0;
            }
        }

        /*Figure out how many rounds are needed to fill the gun.*/
        int N = 0;
        if (dcitems.CGuns[gun.icode - 1].caliber == dcitems.CAL_Energy || dcitems.CGuns[gun.icode - 1].caliber == dcitems.CAL_Napalm)
            N = 1;
        else
            N = dcitems.CGuns[gun.icode - 1].magazine - gun.charge;

        if (N > 0)
        {
            /*Consume the ammo, add it to the magazine.*/
            rpgtext.DCGameMessage("You load " + dcitems.ItemNameShort(gun) + ".");

            N = dcitems.ConsumeDCItem(ref SC.PC.inv, I, N);
            if (dcitems.CGuns[gun.icode - 1].caliber == dcitems.CAL_Energy)
            {
                /*Energy guns can store a large number of shots,*/
                /*depending upon how many E-Cells are loaded*/
                /*into them.*/
                gun.charge += dcitems.CGuns[gun.icode - 1].magazine;

                if (ID)
                    gun.state = spec;
                else
                    gun.state = -spec;

                /*If the weapon is overloaded, well that's bad...*/
                if (gun.charge > 600 / dcitems.CGuns[gun.icode].DMG)
                {
                    rpgtext.DCAppendMessage(" Weapon is overcharged!");
                    gun.charge = 0;
                    gun.state = 0;
                }
            }
            else if (dcitems.CGuns[gun.icode - 1].caliber == dcitems.CAL_Napalm)
            {
                /*One cannister reloads the weapon to full capacity.*/
                gun.charge = dcitems.CGuns[gun.icode].magazine;
                if (rpgdice.Random(10) == 7)
                    rpgtext.DCAppendMessage(" Ready to cook.");
                if (ID)
                    gun.state = spec;
                else
                    gun.state = -spec;
            }
            else
            {
                /*In this, the default case, the gun gains*/
                /*as many shots as bullets you put into it.*/
                gun.charge += N;
                if (ID)
                    gun.state = spec;
                else
                    gun.state = -spec;
            }
        }

        RefreshBackPack(SC);
    }

    static void DropItem(gamebook.Scenario SC, dcitems.DCItem I)
    {
	    /*The player wants to drop an item.*/
	    dcitems.DelinkDCItem(ref SC.PC.inv, I);
	    dcitems.PlaceDCItem(SC.gb, SC.ig, I, SC.PC.m.x,SC.PC.m.y);
	    RefreshBackPack(SC);
    }

    static void EatFood(gamebook.Scenario SC, dcitems.DCItem I)
    {
        /*Eat the food. Go for it.*/

        /*Error check- make sure we have actual food.*/
        if (I.ikind != dcitems.IKIND_Food)
            return;

        if (SC.PC.carbs + dcitems.CFood[I.icode - 1].carbs < 102)
        {
            SC.PC.carbs += dcitems.CFood[I.icode - 1].carbs;
            if (SC.PC.carbs > 100)
                SC.PC.carbs = 100;

            /* Display a different message depending upon whether the */
            /* food item being eaten is a pill or not. */
            if (dcitems.CFood[I.icode - 1].fk == 2)
            {
                rpgtext.DCGameMessage("You take the " + dcitems.CFood[I.icode - 1].name + ".");
            }
            else
            {
                rpgtext.DCGameMessage("You eat the " + dcitems.CFood[I.icode - 1].name + ".");
            }

            if (dcitems.CFood[I.icode - 1].fx != null)
            {
                zapspell.ProcessSpell(SC, dcitems.CFood[I.icode - 1].fx);
            }

            /* Just in case this hasn't been identified yet, ID it now. */
            I.ID = true;

            dcitems.ConsumeDCItem(ref SC.PC.inv, I, 1);
        }
        else
        {
            /* The PC is too full to eat. Print a message depending upon */
            /* whether the food item is a pill or something else. */
            if (dcitems.CFood[I.icode - 1].fk == 2)
            {
                rpgtext.DCGameMessage("You're too full to take the " + dcitems.CFood[I.icode - 1].name + " now.");
            }
            else
            {
                rpgtext.DCGameMessage("You're too full to eat the " + dcitems.CFood[I.icode - 1].name + " now.");
            }
        }

        RefreshBackPack(SC);
    }

    static void BPReadBook(gamebook.Scenario SC, dcitems.DCItem I)
    {
        /*The PC wants to read book I. Call the procedure to do so,*/
        /*and restore the display afterwards.*/

        libram.ReadBook(SC, I.icode);
        TheDisplay(SC);
        RefreshBackPack(SC);
    }

    static void BPElectronics(gamebook.Scenario SC, dcitems.DCItem I)
    {
        /*The PC wants to use item I. Call the procedure to do so,*/
        /*and restore the display afterwards.*/
        HandyMap(SC);
        TheDisplay(SC);
        RefreshBackPack(SC);
    }

    static bool EqpMenu(gamebook.Scenario SC)
    {
        /*This procedure will do all the stuff needed for the*/
        /*Equipment menu. Return TRUE if the player should remain*/
        /*in the inventory screen, FALSE otherwise.*/

        int n = -1;
        do
        {
            n = rpgmenus.SelectMenu(EqpRPM, rpgmenus.RPMNoCleanup);
            rpgmenus.DisplayMenu(EqpRPM);

            if (n > 0)
                ChangeItem(SC, n);
        }
        while (n != -1 && n != BMK_SwitchCode);

        if (n == BMK_SwitchCode)
            return true;

        return false;
    }

    static bool InvMenu(gamebook.Scenario SC)
    {
        /*This procedure will do all the stuff needed for the*/
        /*Inventory menu. Return TRUE to keep doing inventory,*/
        /*FALSE otherwise.*/

        /*Error Check- if there's nothing present in the inventory,*/
        /*boot the player back out to the Equipment menu.*/
        if (InvRPM.firstItem == null)
            return true;

        int n = -1;
        do
        {
            n = rpgmenus.SelectMenu(InvRPM, rpgmenus.RPMNoCleanup);
            rpgmenus.DisplayMenu(InvRPM);

            if (n > -1)
            {
                /*An actual item was selected. Do something*/
                /*with it.*/
                dcitems.DCItem I = dcitems.LocateItem(SC.PC.inv, n);

                /*Check to see if this is an equippable item.*/
                if (I.ikind > 0)
                {
                    EquipItem(SC, I);
                }
                else if (I.ikind == dcitems.IKIND_Ammo)
                {
                    LoadAmmo(SC, I);
                }
                else if (I.ikind == dcitems.IKIND_Food)
                {
                    EatFood(SC, I);
                }
                else if (I.ikind == dcitems.IKIND_Book)
                {
                    BPReadBook(SC, I);
                }
                else if (I.ikind == dcitems.IKIND_Electronics)
                {
                    BPElectronics(SC, I);
                }
            }
            else if (n == BMK_DropCode)
            {
                dcitems.DCItem I = dcitems.LocateItem(SC.PC.inv, rpgmenus.RPMLocateByPosition(InvRPM, InvRPM.selectItem).value);
                DropItem(SC, I);
            }

            /*Check to make sure there are items left in the inventory.*/
            if (InvRPM.firstItem == null)
            {
                n = -1;
            }
        }
        while (n != -1 && n != BMK_SwitchCode);

        if (n == BMK_SwitchCode)
            return true;

        return false;
    }





}