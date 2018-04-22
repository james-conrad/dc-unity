using System;

public class looker
{
    //{What is this? It's the unit which supports the 'Look'}
    //{command. Basically, it provides a UI for the user to select}
    //{a map tile which is currently on-screen.}


    public static texmaps.Point SelectPoint(gamebook.Scenario SC, bool Render, bool SeekModel, texmodel.Model M)
    {
        //{This function is a UI utility. It allows a target}
        //{square to be chosen, centered on the POV model.}
        //{If CANCEL is chosen instead of a target, the X value}
        //{of the returned point will be set to -1.}
        if (SeekModel)
        {
            if (M == null)
                M = NextVisibleModel(SC.gb, M);
            else if (!texmaps.TileLOS(SC.gb.POV, M.x, M.y) || !texmaps.OnTheScreen(SC.gb, M.x, M.y))
                M = NextVisibleModel(SC.gb, M);
        }

        texmaps.Point p = new texmaps.Point();

        if (M != null)
        {
            //{Start the point selector centered on the selected model.}
            p.x = M.x;
            p.y = M.y;
        }
        else
        {
            //{Start the point centered on the POV origin.}
            p.x = SC.gb.POV.m.x;
            p.y = SC.gb.POV.m.y;
        }

        //{Start the loop.}
        char A = ' ';
        do
        {
            //{Indicate the point.}
            if (Render)
            {
                texfx.IndicatePath(SC.gb, SC.gb.POV.m.x, SC.gb.POV.m.y, p.x, p.y, true);
            }
            else
            {
                texmaps.HighlightTile(SC.gb, p.x, p.y);
            }

            rpgtext.DCPointMessage(gamebook.TileName(SC, p.x, p.y));

            //{Get player input and act upon it.}
            A = rpgtext.RPGKey();

            //{Deindicate the point.}
            if (Render)
            {
                texfx.DeIndicatePath(SC.gb, SC.gb.POV.m.x, SC.gb.POV.m.y, p.x, p.y);
            }
            else
            {
                texmaps.DisplayTile(SC.gb, p.x, p.y);
            }

            if (A == rpgtext.KMap[0].key)
                MoveMapCursor(SC.gb, 1, ref p);
            else if (A == rpgtext.KMap[1].key)
                MoveMapCursor(SC.gb, 2, ref p);
            else if (A == rpgtext.KMap[2].key)
                MoveMapCursor(SC.gb, 3, ref p);
            else if (A == rpgtext.KMap[3].key)
                MoveMapCursor(SC.gb, 4, ref p);
            else if (A == rpgtext.KMap[5].key)
                MoveMapCursor(SC.gb, 6, ref p);
            else if (A == rpgtext.KMap[6].key)
                MoveMapCursor(SC.gb, 7, ref p);
            else if (A == rpgtext.KMap[7].key)
                MoveMapCursor(SC.gb, 8, ref p);
            else if (A == rpgtext.KMap[8].key)
                MoveMapCursor(SC.gb, 9, ref p);
            else if (A == (char)9)
            { 
                M = NextVisibleModel(SC.gb, M);
                if (M != null)
                {
                    p.x = M.x;
                    p.y = M.y;
                }
            }
        }
        while (A != ' ' && A != (char)27 && A != rpgtext.KMap[13].key && A != rpgtext.KMap[14].key);

        if (A == (char)27)
            p.x = -1;

        return p;
    }

    static void MoveMapCursor(texmaps.GameBoard gb, int D, ref texmaps.Point p)
    {
	    //{Move the map cursor point. This can be used for the Select}
	    //{Target routine, or for the PCLook routine. In any case,}
	    //{the big point is to make sure that the point doesn't go off}
	    //{the screen.}
	    if (texmaps.OnTheScreen(gb,p.x + texmaps.VecDir[D-1,0],p.y + texmaps.VecDir[D-1,1]))
        {
		    p.x += texmaps.VecDir[D-1,0];
		    p.y += texmaps.VecDir[D-1,1];
        }
    }

    static texmodel.Model NextVisibleModel(texmaps.GameBoard gb, texmodel.Model M)
    {
        //{Locate the next visible model in the models list. If}
        //{the end of the list is encountered, start looking again}
        //{at the beginning. If no visible models are found,}
        //{return Nil.}

        //{ERROR CHECK- exit immediately if there are no models present.}
        if (gb.mlist == null)
            return null;

	    texmodel.Model M2 = null;
	    texmodel.Model M1 = M;
        if (M == null)
            M = gb.mlist;

	    bool GetOutOfLoopFree = false;

        do
        {
            if (M1 != null)
            {
                //{Move to the next model in the list.}
                M = M.next;
                if (M == null)
                    M = gb.mlist;
            }

            if (texmaps.TileLOS(gb.POV, M.x, M.y) && M != gb.POV.m && texmaps.OnTheScreen(gb, M.x, M.y) && M.kind == critters.MKIND_Critter)
            {
                M2 = M;
            }

            if (M1 == null)
            {
                M = M.next;
                if (M == null)
                    GetOutOfLoopFree = true;
            }
            else
            {
                if (M == M1)
                    GetOutOfLoopFree = true;
            }
        }
        while (M2 == null && !GetOutOfLoopFree);

        return M2;
    }
}