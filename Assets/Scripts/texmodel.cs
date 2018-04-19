using System;

public class texmodel
{
    //{ This unit handles models.That is to say, things which will appear}
    //{ on the playing field, but which are not bits of terrain.}
    //{ *** GFX UNIT*** }

	//{ These values hold the size of the game map.}
    public const int XMax = 256;
    public const int YMax = 256;

    public class Model
    {
        public Model(char gfx, Crt.Color color, Crt.Color bColor, bool coHab, int x, int y, int kind)
        {
            this.gfx = gfx;
            this.color = color;
            this.aColor = color;
            this.bColor = bColor;
            this.obs = 0; //{ Default obscurement}
            this.coHab = coHab;
            this.x = x;
            this.y = y;
            this.kind = kind;
            this.next = null;
        }

        public char gfx;
        public Crt.Color color;
        public Crt.Color aColor;
        public Crt.Color bColor;
        public int obs;          //{ How much vision obscurement does this model cause?}
        public bool coHab;       //{ Can this model share its location?}
        public int x;
        public int y;
        public int kind;         //{ What KIND of a model is this?}
        public Model next;
    }

    public class ModelGrid
    {
        public bool IsSet(int x, int y)
        {
            //{Check location X,Y and see if there's a model. Check the values}
            //{of X and Y to make sure they're in the boundaries.}
            if (x >= 1 && x <= XMax && y >= 1 && y <= YMax)
            {
                return grid[x - 1, y - 1];
            }

            return false;
        }

        public void Set(int x, int y, bool present)
        {
            //{Check location X,Y and see if there's a model. Check the values}
            //{of X and Y to make sure they're in the boundaries.}
            if (x >= 1 && x <= XMax && y >= 1 && y <= YMax)
            {
                grid[x - 1, y - 1] = present;
            }
        }

        bool[,] grid = new bool[XMax, YMax];
    }

    public static Model AddModel(ref Model mp, ModelGrid mg, char gfx, Crt.Color color, Crt.Color bColor, bool coHab, int x, int y, int kind)
    {
        //{ Add a model to the model list.}

        //{ Allocate memory for IT}
        //{ Initialize all of ITs fields}
        Model it = new Model(gfx, color, bColor, coHab, x, y, kind);

        //{ Do a range check on X and Y to make sure they lie inside the playfield.}
        if (x < 1)
            x = 1;
        else if (x > XMax)
            x = XMax;

        if (y < 1)
            y = 1;
        else if (y > YMax)
            y = YMax;

        //{Modify the model grid to show that the spot is occupied.}
        mg.Set(x, y, true);

        //{Locate a good position to attach it to.}
        if (mp == null)
        {
            //{the list is currently empty. Attach it as the first model.}
            mp = it;
        }
        else
        {
            //{The list has stuff in it. Attach IT to the end.}
            LastModel(mp).next = it;
        }

        return it;
    }

    public static Model FindModelXY(Model mp, int x, int y)
    {
	    //{ Search through the models list, searching for a model in location}
	    //{ X,Y. Return a ptr to that model, or Nil if no such model exists.}

	    //{ Initialize temp to Nil}
	    Model temp = null;

        //{Loop through all of the models, searching for one that fits.}
        while (mp != null)
        {
            if (mp.x == x && mp.y == y)
            {
                //{If this is the first model we've found at this location,}
                //{save it's pointer.}
                if (temp == null)
                {
                    temp = mp;
                }
                //{ If this isn't the first, save the pointer to the model}
                //{ that doesn't normally cohabitate.}
                else
                {
                    if (temp.coHab)
                    {
                        temp = mp;
                    }
                }
            }

            mp = mp.next;
        }

        return temp;
    }

    public static bool ModelPresent(ModelGrid mg, int x, int y)
    {
        return mg.IsSet(x, y);
    }

    public static void SetModelLoc(Model m, Model mList, ModelGrid mg, int x2, int y2)
    {
        //{ Move the model M to location X2,Y2, adjusting the contents of}
        //{ the modelgrid accordingly.}

        //{ Range check. If X2,Y2 lie out of bounds, bring them back into}
        //{ bounds.}
        if (x2 < 1) x2 = 1;
        else if (x2 > XMax) x2 = XMax;

        if (y2 < 1) y2 = 1;
        else if (y2 > YMax) y2 = YMax;

	    //{ Save the initial position of the model.}
	    int x1 = m.x;
	    int y1 = m.y;

	    //{Change the position of the model.}
	    m.x = x2;
	    m.y = y2;

        if (FindModelXY(mList, x1, y1) == null)
        {
            mg.Set(x1, y1, false);
        }

        mg.Set(x2, y2, true);
    }

    public static void RemoveModel(Model lmember, ref Model llist, ModelGrid mg)
    {
	    //{Locate and extract member LMember from list LList.}
	    //{Then, dispose of LMember.}

	    //{ Initialize A and B }
	    Model b = llist;
	    Model a = null;

        //{ Save the X,Y position of the model. }
        int x = lmember.x;
	    int y = lmember.y;

        //{Locate LMember in the list. A will thereafter be either Nil,}
        //{if LMember if first in the list, or it will be equal to the}
        //{element directly preceding LMember.}
        while (b != lmember && b != null)
        {
            a = b;
            b = b.next;
        }

        if (b == null)
        {
            //{ Major FUBAR. The member we were trying to remove can't}
            //{ be found in the list.}
            Crt.Write("ERROR- RemoveModel asked to remove a model that doesnt exist.");
            do { rpgtext.RPGKey(); } while (true);
        }
        else if (a == null)
        {
            //{ There's no element before the one we want to remove,}
            //{ i.e. it's the first one in the list.}
            llist = b.next;
            b.next = null;
        }
        else
        {
            //{We found the model we want to delete and have another}
            //{one standing before it in line. Go to work.}
            a.next = b.next;
            b.next = null;
        }

        //{ Update the model grid. }
        if (FindModelXY(llist, x, y) == null)
        {
            mg.Set(x, y, false);
        }
    }

    static Model LastModel(Model mp)
    {
        //{Locate the last model in the linked list.}
        if (mp == null)
            return null;

        while (mp.next != null)
        {
            mp = mp.next;
        }

        return mp;
    }
}
