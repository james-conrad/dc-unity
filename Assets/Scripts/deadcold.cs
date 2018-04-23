using System;
using UnityEngine;

public class deadcold
{
    public static void RunGame()
    {
        rpgtext.SetKeyMap();

        rpgmenus.RPGMenu menu = rpgmenus.CreateRPGMenu(Crt.Color.LightBlue, Crt.Color.Green, Crt.Color.LightGreen, 20, 8, 60, 23);
        rpgmenus.AddRPGMenuItem(menu, "Start New Game", 1);
        rpgmenus.AddRPGMenuItem(menu, "Load Saved Game", 2);
        rpgmenus.AddRPGMenuItem(menu, "Quit DeadCold", -1);

        int N = 0;
        do
        {
            //{ Set up the screen display. }
            Crt.ClrScr();
            Crt.CursorOff();

            //{ Get a selection from the menu. }
            N = rpgmenus.SelectMenu(menu, rpgmenus.RPMNoCancel);

            switch (N)
            {
                case 1:
                    dcplay.StartGame();
                    break;
                case 2:
                    dcplay.RestoreGame();
                    break;
            }
        }
        while (N > 0);
    }
}
