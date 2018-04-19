using System;
using System.Threading;

public class texfx
{
    //{This unit handles special graphics effects in ASCII mode.}

    const Crt.Color PathColor = Crt.Color.Yellow;
    const Crt.Color ShotColor = Crt.Color.LightRed;

    public static void IndicateModel(texmaps.GameBoard gb, texmodel.Model M)
    {
        //{Set the model's color to BColor.}
        M.color = M.bColor;
        texmaps.DisplayTile(gb, M.x, M.y);
    }

    public static void DeIndicateModel(texmaps.GameBoard gb, texmodel.Model M)
    {
        //{Set the model's color to AColor.}
        M.color = M.aColor;
        texmaps.DisplayTile(gb, M.x, M.y);
    }

    public static void IndicatePath(texmaps.GameBoard gb, int X1, int Y1, int X2, int Y2, bool LOS)
    {
        //{Draw a line to indicate the distance between point 1 and}
        //{point 2. Do not indicate the points 1 and 2 themselves,}
        //{just the points in between.}

        //{Calculate Length.}
        int L = 0;
        if (Math.Abs(X2 - X1) > Math.Abs(Y2 - Y1))
        {
            L = Math.Abs(X2 - X1);
        }
        else
        {
            L = Math.Abs(Y2 - Y1);
        }

        int t;
        texmaps.Point P;

        if (L > 1)
        {
            for (t = 1; t <= L - 1; ++t)
            {
                P = texmaps.SolveLine(X1, Y1, X2, Y2, t);
                texmaps.MapSplat(gb, '-', PathColor, P.x, P.y, LOS);
            }
        }

        //{Indicate the terminus of the line.}
        P = texmaps.SolveLine(X1, Y1, X2, Y2, L);
        if (gb.mog.IsSet(P.x, P.y) && texmaps.TileLOS(gb.POV, P.x, P.y))
        {
            IndicateModel(gb, texmodel.FindModelXY(gb.mlist, P.x, P.y));
        }
        else
        {
            texmaps.MapSplat(gb, '+', PathColor, P.x, P.y, LOS);
        }
    }

    public static void DeIndicatePath(texmaps.GameBoard gb, int X1, int Y1, int X2, int Y2)
    {
        //{There's a big line currently marring our screen display.}
        //{Clean it up, wouldja?}

        //{Calculate Length.}
        int L = 0;
        if (Math.Abs(X2 - X1) > Math.Abs(Y2 - Y1))
        {
            L = Math.Abs(X2 - X1);
        }
        else
        {
            L = Math.Abs(Y2 - Y1);
        }

        int t;
        texmaps.Point P;

        if (L > 1)
        {
            for (t = 1; t <= L - 1; ++t)
            {
                P = texmaps.SolveLine(X1, Y1, X2, Y2, t);
                texmaps.DisplayTile(gb, P.x, P.y);
            }
        }

        //{Indicate the terminus of the line.}
        P = texmaps.SolveLine(X1, Y1, X2, Y2, L);
        if (gb.mog.IsSet(P.x, P.y) && texmaps.TileLOS(gb.POV, P.x, P.y))
        {
            DeIndicateModel(gb, texmodel.FindModelXY(gb.mlist, P.x, P.y));
        }
        else
        {
            texmaps.DisplayTile(gb, P.x, P.y);
        }
    }

    static void Delay()
    {
        if (rpgtext.FrameDelay > 0)
        {
            Thread.Sleep(rpgtext.FrameDelay);
        }
    }

    static void DelayDiv(int d)
    {
        if (rpgtext.FrameDelay > 0)
        {
            Thread.Sleep(rpgtext.FrameDelay / d);
        }
    }

    public static void DisplayShot(texmaps.GameBoard gb, int X1, int Y1, int X2, int Y2, Crt.Color c, bool hit)
    {
        //{A projectlie attack has just been launched. Display its}
        //{trajectory in glorious ASCII graphics.}
        //{At the terminus of the shot, display a * if the attack}
        //{hit and a - if it didn't. This info is contained in the}
        //{parameter named HIT, of course.}

        //{Calculate Length.}
        int L;
        if (Math.Abs(X2 - X1) > Math.Abs(Y2 - Y1))
        {
            L = Math.Abs(X2 - X1);
        }
        else
        {
            L = Math.Abs(Y2 - Y1);
        }

        int t;
        texmaps.Point P;

        if (L > 1)
        {
            for (t = 1; t <= L - 1; ++t)
            {
                P = texmaps.SolveLine(X1, Y1, X2, Y2, t);

                //{Display bullet...}
                texmaps.MapSplat(gb, '+', c, P.x, P.y, false);

                //{Wait a bit...}
                Delay();

                //{Restore the display.}
                texmaps.DisplayTile(gb, P.x, P.y);
            }
        }

        //{Display the terminus.}
        if (hit)
        {
            texmaps.MapSplat(gb, '*', c, X2, Y2, false);
        }
        else
        {
            texmaps.MapSplat(gb, '-', c, X2, Y2, false);
        }

        //{Wait a bit...}
        Delay();
        Delay();
        Delay();

        texmaps.DisplayTile(gb, X2, Y2);
    }

    public static void ModelFlash(texmaps.GameBoard gb, texmodel.Model M)
    {
        //{Flash the POV model, then flash the indicated model.}
        int t;
        for (t = 1; t <= 3; ++t)
        {
            IndicateModel(gb, gb.POV.m);
            DelayDiv(2);
            DeIndicateModel(gb, gb.POV.m);
            DelayDiv(2);
        }

        for (t = 1; t <= 3; ++t)
        {
            IndicateModel(gb, M);
            DelayDiv(2);
            DeIndicateModel(gb, M);
            DelayDiv(2);
        }
    }

    static void Stroke(texmaps.GameBoard gb, int X, int Y, Crt.Color C)
    {
        texmaps.MapSplat(gb, '|', C, X, Y, false);
        DelayDiv(2);
        texmaps.MapSplat(gb, '/', C, X, Y, false);
        DelayDiv(2);
        texmaps.MapSplat(gb, '-', C, X, Y, false);
        DelayDiv(2);
        texmaps.MapSplat(gb, '/', C, X, Y, false);
        DelayDiv(2);
    }

    public static void LaserCut(texmaps.GameBoard gb, int X, int Y)
    {
        //{Do the laser cut animation at location X,Y.}
        Stroke(gb, X, Y, Crt.Color.LightGreen);
        Stroke(gb, X, Y, Crt.Color.Yellow);
        Stroke(gb, X, Y, Crt.Color.White);
        texmaps.DisplayTile(gb, X, Y);
    }

    public static void DakkaDakka(texmaps.GameBoard gb, int X, int Y)
    {
        //{Do a machinegun type animation at the desired spot.}

        for (int t = 1; t <= 5; ++t)
        {
            texmaps.MapSplat(gb, '+', Crt.Color.Yellow, X, Y, false);
            DelayDiv(2);
            texmaps.MapSplat(gb, 'x', Crt.Color.Yellow, X, Y, false);
            DelayDiv(2);
        }

        texmaps.DisplayTile(gb, X, Y);
    }

    static Crt.Color[] PikaColor = new Crt.Color[5]
    {
        Crt.Color.LightBlue, Crt.Color.LightCyan, Crt.Color.White, Crt.Color.LightCyan, Crt.Color.White
    };

    public static void PikaPikaOuch(texmaps.GameBoard gb, int X, int Y)
    {
        //{Do an electrocution effect at the desired point.}
        for (int t = 1; t <= 5; ++t)
        {
            texmaps.MapSplat(gb, 'X', PikaColor[rpgdice.rng.Next(5)], X, Y, false);
            DelayDiv(2);
            texmaps.MapSplat(gb, '%', PikaColor[rpgdice.rng.Next(5)], X, Y, false);
            DelayDiv(2);
        }

        texmaps.DisplayTile(gb, X, Y);
    }
}



