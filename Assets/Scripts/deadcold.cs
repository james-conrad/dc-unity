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
        libram.ReadBook(null, 1);
    }

    static void StartGame()
    {
        int x;
        int y;

        texmaps.GameBoard gb = texmaps.NewBoard();
        dcitems.IGrid IG = new dcitems.IGrid();

        for (x = 0; x < texmodel.XMax; ++x)
        {
            for (y = 0; y < texmodel.YMax; ++y)
            {
                bool edge = x == 0 || y == 0 || x == texmodel.XMax - 1 || y == texmodel.YMax - 1;

                bool iEdge = x % 10 == 0 || y % 10 == 0;
                bool door = x % 10 == 5 || y % 10 == 5;

                bool wall = !door && (edge || iEdge);

                gb.map[x, y].terr = wall ? 3 : 2;
                //rpgdice.rng.Next(0, 5);// texmaps.NumTerr);

                if (rpgdice.rng.Next(0, 100) == 1)
                    dcitems.PlaceDCItem(gb, IG, new dcitems.DCItem(), x + 1, y + 1);
            }
        }

        dcchars.DCChar PC = randchar.RollNewChar();

        x = rpgdice.rng.Next(1, texmodel.XMax + 1);
        y = rpgdice.rng.Next(1, texmodel.YMax + 1);
        texmodel.Model m = texmodel.AddModel(ref gb.mlist, gb.mog, (char)1, Crt.Color.Yellow, Crt.Color.White, false, x, y, dcchars.MKIND_Character);
        PC.m = m;

        gamebook.Scenario SC = gamebook.NewScenario();
        SC.gb = gb;
        SC.PC = PC;
        SC.ig = IG;

        for (int i = 0; i < 10; ++i)
        {
            critters.AddCritter(ref SC.CList, SC.gb, rpgdice.rng.Next(critters.MaxCrit) + 1, x - 15 + rpgdice.rng.Next(30), y - 15 + rpgdice.rng.Next(30));
        }

        gamebook.PCStatLine(SC);


        //{ Set the particulars for the player's model. }
	    gb.POV.m = m;
	    gb.POV.range = 12;
        texmaps.RecenterPOV(gb);
        texmaps.UpdatePOV(gb.POV, gb);
	    texmaps.ApplyPOV(gb.POV, gb);
        texmaps.DisplayMap(gb);

        int dir = rpgtext.DirKey();

        while (true)
        {
            if (dir == 0)
            {
                //int x1 = m.x + rpgdice.rng.Next(10) - 5;
                //int y1 = m.y + rpgdice.rng.Next(10) - 5;
                //switch (rpgdice.rng.Next(6))
                //{
                //    case 0:
                //        texfx.DisplayShot(gb, m.x, m.y, x1, y1, Crt.Color.Magenta, rpgdice.rng.Next(2) == 1);
                //        break;
                //    case 1:
                //        texfx.DakkaDakka(gb, x1, y1);
                //        break;
                //    case 2:
                //        texfx.IndicatePath(gb, m.x, m.y, x1, y1, true);
                //        rpgtext.RPGKey();
                //        texfx.DeIndicatePath(gb, m.x, m.y, x1, y1);
                //        break;
                //    case 3:
                //        texfx.LaserCut(gb, m.x, m.y);
                //        break;
                //    case 4:
                //        texfx.PikaPikaOuch(gb, m.x, m.y);
                //        break;
                //    case 5:
                //        texfx.IndicateModel(gb, m);
                //        rpgtext.RPGKey();
                //        texfx.DeIndicateModel(gb, m);
                //        break;
                //}
                //rpgtext.DCGameMessage("Pick a Target:");

                //texmaps.Point p = looker.SelectPoint(SC, true, true, null);
                //texmaps.Point b = texmaps.LocateBlock(SC.gb, m.x, m.y, p.x, p.y);
                //texfx.DakkaDakka(gb, b.x, b.y);

                backpack.HandyMap(SC);

                backpack.Inventory(SC, true);

                zapspell.CastSpell(SC, true);
            }
            else
            {
                int dx = texmaps.VecDir[dir - 1, 0];
                int dy = texmaps.VecDir[dir - 1, 1];

                texmaps.WalkReport wr = texmaps.MoveModel(m, gb, m.x + dx, m.y + dy);
                texmaps.DisplayMap(gb);

                gamebook.DoleExperience(SC, 1);
            }

            cbrain.BrownianMotion(SC);

            critters.Critter Cr = SC.CList;
            while (Cr != null)
            {
                //{Save the position of the next critter,}
                //{since the critter we're processing might}
                //{accidentally kill itself during its move.}
                SC.CA2 = Cr.next;
                for (int t = 1; t <= gamebook.NumberOfActions(SC.ComTime, critters.MonMan[Cr.crit - 1].Speed); ++t)
                {
                    cbrain.CritterAction(SC, ref Cr);
                    if (Cr == null)
                        break;
                }
                Cr = SC.CA2;

                //if (SC.PC.HP < 1)
                //    Cr = null;
            }

            PC.HP = PC.HPMax;
            SC.ComTime += 1;
            dir = rpgtext.DirKey();
        }
    }
}