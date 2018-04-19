using System;
using System.Collections.Generic;

// { This unit handles some of my frequently-wanted dice }
// { routines. }

public class rpgdice
{
    public static Random rng = new Random();

    static int[] DieSize = { 4, 6, 8, 10, 12 };
    static int[][] DieStep = {
        //        {  d4  d6  d8 d10 d12 }
        new int[] {   1,  0,  0,  0,  0 },
        new int[] {   0,  1,  0,  0,  0 },
        new int[] {   0,  0,  1,  0,  0 },
        new int[] {   0,  0,  0,  1,  0 },
        new int[] {   0,  0,  0,  0,  1 },

        new int[] {   0,  1,  1,  0,  0 },
        new int[] {   0,  1,  0,  1,  0 },
        new int[] {   0,  0,  1,  1,  0 },
        new int[] {   0,  0,  0,  2,  0 },
        new int[] {   0,  0,  0,  1,  1 }
    };


    public static int Dice(int die)
    {
        //{ Roll a die- D(6), D(8), D(100), whatever. }
        //{ Die rolling is done as per Earthdawn- whenever a maximum is }
        //{ rolled, the score is kept and the die rerolled.We're gonna }
        //{ use recursion to handle this. }

        //{ Range check}
        if (die < 2)
        {
            die = 2;
        }

        return VictoriasSecret(die, 1);
    }

    static int VictoriasSecret(int die, int it)
    {
        //{ WHat's this!? It's the function's underwear of course. }

        //{ Roll the die. }
        int r = rng.Next(1, die + 1);

        //{ If you've rolled the maximum, and we've recursed less }
        //{ than 100 times, recurse again! }
        if (r == die && it < 100)
        {
            return r + VictoriasSecret(die, it + 1);
        }

        //{ Slip the address into the underwear. }
        return r;
    }

    public static int RollStep(int n)
    {
        //{ Roll a dice step number, a la Earthdawn. }

        int RS = 0;

        while (n > 0)
        {
            int n2 = n > 10 ? 10 : n;

            for (int t1 = 0; t1 < 5; ++t1)
            {
                for (int t2 = 0; t2 < DieStep[n2 - 1][t1]; ++t2)
                {
                    RS += Dice(DieSize[t1]);
                }
            }

            // { Decrease N by 10. }
            n -= 10;
        }

        return RS;
    }


    public static int RollStat(int n)
    {
        //{ Roll Nd6; take the three highest values, add them together, }
        //{ and return the result. N must be in the range of 1 to 10. }

        //{ Range check. }
        if (n > 10)
        {
            n = 10;
        }

        //{ Initialize stat }
        int stat = 0;

        //{ Roll the indicated number of dice. }
        List<int> rolls = new List<int>(n);

        for (int t = 0; t < n; ++t)
        {
            //{ Roll the die }
            int roll = rng.Next(1, 6 + 1);
            rolls.Add(roll);

            //{ Add it to the total}
            stat += roll;
        }

        //{ If we rolled more dice than we need, go through and eliminate }
        //{ the low rolls.}
        if (n > 3)
        {
            for (int t = 0; t < n - 3; ++t)
            {
                //{ locate the first nonzero value for l}
                int l = 0;

                while (rolls[l] == 0)
                {
                    ++l;
                }

                for (int tt = 0; tt < n; ++tt)
                {
                    if (rolls[tt] > 0 && rolls[tt] < rolls[l])
                    {
                        l = tt;
                    }
                }

                stat = stat - rolls[l];
                rolls[l] = 0;
            }
        }

        return stat;
    }
}
