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
                    StartGame();
                    break;
                case 2:
                    RestoreGame();
                    break;
            }
        }
        while (N > 0);
    }

    static void RestoreGame()
    {
        libram.ReadBook();
    }

    static void StartGame()
    {
        dcchars.DCChar PC = randchar.RollNewChar();

        texmaps.GameBoard gb = texmaps.NewBoard();

        int x;
        int y;

        dcitems.IGrid IG = new dcitems.IGrid();

        for (x = 0; x < texmodel.XMax; ++x)
        {
            for (y = 0; y < texmodel.YMax; ++y)
            {
                bool edge = x == 0 || y == 0 || x == texmodel.XMax - 1 || y == texmodel.YMax - 1;

                bool iEdge = x % 10 == 0 || y % 10 == 0;
                bool door = x % 10 == 5 || y % 10 == 5;

                bool wall = !door && (edge || iEdge);

                gb.map[x, y].terr = wall ? 2 : 1;
                //rpgdice.rng.Next(0, 5);// texmaps.NumTerr);

                if (rpgdice.rng.Next(0, 100) == 1)
                    dcitems.PlaceDCItem(gb, IG, new dcitems.DCItem(), x+1, y+1);
            }
        }


        x = rpgdice.rng.Next(1, texmodel.XMax + 1);
        y = rpgdice.rng.Next(1, texmodel.YMax + 1);

        //{ Set the particulars for the player's model. }
        texmodel.Model m = texmodel.AddModel(ref gb.mlist, gb.mog, (char)1, Crt.Color.Yellow, Crt.Color.White, false, x, y, dcchars.MKIND_Character);
	    gb.POV.m = m;
	    gb.POV.range = 6;
        texmaps.RecenterPOV(gb);
        texmaps.UpdatePOV(gb.POV, gb);
	    texmaps.ApplyPOV(gb.POV, gb);
        texmaps.DisplayMap(gb);

        int dir = rpgtext.DirKey();

        while (true)
        {
            if (dir == 0)
            {
                int x1 = m.x + rpgdice.rng.Next(10) - 5;
                int y1 = m.y + rpgdice.rng.Next(10) - 5;
                switch (rpgdice.rng.Next(6))
                {
                    case 0:
                        texfx.DisplayShot(gb, m.x, m.y, x1, y1, Crt.Color.Magenta, rpgdice.rng.Next(2) == 1);
                        break;
                    case 1:
                        texfx.DakkaDakka(gb, x1, y1);
                        break;
                    case 2:
                        texfx.IndicatePath(gb, m.x, m.y, x1, y1, true);
                        rpgtext.RPGKey();
                        texfx.DeIndicatePath(gb, m.x, m.y, x1, y1);
                        break;
                    case 3:
                        texfx.LaserCut(gb, m.x, m.y);
                        break;
                    case 4:
                        texfx.PikaPikaOuch(gb, m.x, m.y);
                        break;
                    case 5:
                        texfx.IndicateModel(gb, m);
                        rpgtext.RPGKey();
                        texfx.DeIndicateModel(gb, m);
                        break;
                }
            }
            else
            {
                int dx = texmaps.VecDir[dir - 1, 0];
                int dy = texmaps.VecDir[dir - 1, 1];

                texmaps.WalkReport wr = texmaps.MoveModel(m, gb, m.x + dx, m.y + dy);
                texmaps.DisplayMap(gb);
            }

            dir = rpgtext.DirKey();
        }
    }
}