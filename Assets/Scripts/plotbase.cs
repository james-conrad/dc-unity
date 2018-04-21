using System;
using System.IO;

public class plotbase
{
    //{This unit defines the Plot Arc type, and provides several}
    //{procedures for dealing with it.}

    //{ PLOTBASE MARK II -  technology marches on and DeadCold with it. }
    //{ I have replaced the original PlotBase/PlotLine stuff with the }
    //{ considerably more elegant GearHead Arena Script code. }


    public const int NAG_ScriptVar = 0;
    public const int NAG_MonsterMemory = 1;

    //{*** STRING ATTRIBUTE ***}
    public class SAtt
    {
        public string info;
        public SAtt next;
    }

    //{*** NUMERICAL ATTRIBUTE ***}
    public class NAtt
    {
        public int G; // {General}
        public int S; // {Specific}
        public int V; // {Value}
        public NAtt next;
    }

    public static SAtt CreateSAtt(ref SAtt llist)
    {
        //{Add a new element to the head of LList.}

        //{Allocate memory for our new element.}
        SAtt it = new SAtt();

        //{Attach IT to the list.}
        it.next = llist;
        llist = it;

        //{Return a pointer to the new element.}
        return it;
    }

    public static void RemoveSAtt(ref SAtt llist, SAtt lmember)
    {
        //{Locate and extract member LMember from list LList.}
        //{Then, dispose of LMember.}

        //{ Initialize A and B}
        SAtt B = llist;
        SAtt A = null;

        //{Locate LMember in the list. A will thereafter be either Nil,}
        //{if LMember if first in the list, or it will be equal to the}
        //{element directly preceding LMember.}
        while (B != lmember && B != null)
        {
            A = B;
            B = B.next;
        }

        if (B == null)
        {
            //{Major FUBAR. The member we were trying to remove can't}
            //{be found in the list.}
            Crt.Write("ERROR- RemoveSAtt asked to remove a link that doesnt exist.");
            do { rpgtext.RPGKey(); } while (true);
        }
        else if (A == null)
        {
            //{There's no element before the one we want to remove,}
            //{i.e. it's the first one in the list.}
            llist = B.next;
            B = null;
        }
        else
        {
            //{We found the attribute we want to delete and have another}
            //{one standing before it in line. Go to work.}
            A.next = B.next;
            B = null;
        }
    }

    public static SAtt FindSAtt(SAtt llist, string code)
    {
        //{Search through the list looking for a String Attribute}
        //{whose code matches CODE and return its address.}
        //{Return Nil if no such SAtt can be found.}

        //{Initialize IT to Nil.}
        SAtt it = null;

        code = code.ToUpper();

        //{Check through all the SAtts looking for the SATT in question.}
        while (llist != null)
        {
            string S = llist.info;
            S = texutil.ExtractWord(ref S).ToUpper();
            if (S == code)
                it = llist;

            llist = llist.next;
        }

        return it;
    }

    public static SAtt SetSAtt(ref SAtt llist, string info)
    {
        //{Add string attribute Info to the list. However, a gear}
        //{may not have two string attributes with the same name.}
        //{So, check to see whether or not the list already contains}
        //{a string attribute of this type; if so, just replace the}
        //{INFO field. If not, create a new SAtt and fill it in.}

        //{Determine the CODE of the string.}
        string code = info;
        code = texutil.ExtractWord(ref code);

        //{See if that code already exists in the list,}
        //{if not create a new entry for it.}
        SAtt it = FindSAtt(llist, code);

        //{Plug in the value.}
        if (texutil.RetrieveAString(info) == "")
        {
            if (it != null)
            {
                RemoveSAtt(ref llist, it);
            }
        }
        else
        {
            if (it == null)
                it = CreateSAtt(ref llist);
            it.info = info;
        }

        //{Return a pointer to the new attribute.}
        return it;
    }

    public static SAtt StoreSAtt(ref SAtt llist, string info)
    {
        //{ Add string attribute Info to the list. This procedure }
        //{ doesn't check to make sure this attribute isn't duplicated. }
        SAtt it = CreateSAtt(ref llist);
        it.info = info;

        //{Return a pointer to the new attribute.}
        return it;
    }

    public static string SAttValue(SAtt llist, string code)
    {
        //{Find a String Attribute which corresponds to Code, then}
        //{return its embedded alligator string.}
        SAtt it = FindSAtt(llist, code);

        if (it == null)
            return "";

        return texutil.RetrieveAString(it.info);
    }

    public static NAtt CreateNAtt(ref NAtt llist)
    {
        //{Add a new element to the head of LList.}

        //{Allocate memory for our new element.}
        NAtt it = new NAtt();

        //{Initialize values.}

        it.next = llist;
        llist = it;

        //{Return a pointer to the new element.}
        return it;
    }

    public static void RemoveNAtt(ref NAtt llist, NAtt lmember)
    {
        //{Locate and extract member LMember from list LList.}
        //{Then, dispose of LMember.}

        //{Initialize A and B}
        NAtt B = llist;
        NAtt A = null;

        //{Locate LMember in the list. A will thereafter be either Nil,}
        //{if LMember if first in the list, or it will be equal to the}
        //{element directly preceding LMember.}
        while (B != lmember && B != null)
        {
            A = B;
            B = B.next;
        }

        if (B == null)
        {
            //{Major FUBAR. The member we were trying to remove can't}
            //{be found in the list.}
            Crt.Write("ERROR- RemoveLink asked to remove a link that doesnt exist.");
            do { rpgtext.RPGKey(); } while (true);
        }
        else if (A == null)
        {
            //{There's no element before the one we want to remove,}
            //{i.e. it's the first one in the list.}
            llist = B.next;
            B = null;
        }
        else
        {
            //{We found the attribute we want to delete and have another}
            //{one standing before it in line. Go to work.}
            A.next = B.next;
            B = null;
        }
    }

    public static NAtt FindNAtt(NAtt llist, int G, int S)
    {
        //{Locate the numerical attribute described by G,S and}
        //{return a pointer to it. If no such attribute exists}
        //{in the list, return Nil.}

        //{Initialize it to Nil.}
        NAtt it = null;

        //{Loop through all the elements.}
        while (llist != null)
        {
            if (llist.G == G && llist.S == S)
            {
                it = llist;
            }
            llist = llist.next;
        }

        //{Return the value.}
        return it;
    }

    public static NAtt SetNAtt(ref NAtt llist, int G, int S, int V)
    {
        //{Set the Numerical Attribute described by G,S to value V.}
        //{If the attribute already exists, change its value. If not,}
        //{create the attribute.}
        NAtt it = FindNAtt(llist, G, S);

        if (it == null)
        {
            //{The attribute doesn't currently exist. Create it.}
            it = CreateNAtt(ref llist);
            it.G = G;
            it.S = S;
            it.V = V;
        }
        else
        {
            //{The attribute is already posessed. Just change}
            //{its Value field.}
            it.V = V;
        }

        return it;
    }

    public static NAtt AddNAtt(ref NAtt llist, int G, int S, int V)
    {
        //{Add value V to the value field of the Numerical Attribute}
        //{described by G,S. If the attribute does not exist, create}
        //{it and set its value to V.}
        //{If, as a result of this operation, V drops below 0,}
        //{the numerical attribute will be removed and Nil will}
        //{be returned.}
        NAtt it = FindNAtt(llist, G, S);

        if (it == null)
        {
            //{The attribute doesn't currently exist. Create it.}
            it = CreateNAtt(ref llist);
            it.G = G;
            it.S = S;
            it.V = V;
        }
        else
        {
            it.V += V;
        }

        if (it.V < 0)
        {
            RemoveNAtt(ref llist, it);
        }

        return it;
    }

    public static int NAttValue(NAtt llist, int G, int S)
    {
        //{Return the value of Numeric Attribute G,S. If this}
        //{attribute is not posessed, return 0.}

        int it = 0;
        while (llist != null)
        {
            if (llist.G == G && llist.S == S)
            {
                it = llist.V;
            }
            llist = llist.next;
        }

        return it;
    }

    const int SaveFileContinue = 0;
    const int SaveFileSentinel = -1;

    public static void WriteNAtt(NAtt NA, StreamWriter f)
    {
        //{ Output the provided list of string attributes. }

        //{ Export Numeric Attributes }
        while (NA != null)
        {
            f.WriteLine(String.Format("{0} {1} {2} {3}", SaveFileContinue, NA.G, NA.S, NA.V));
            NA = NA.next;
        }

        //{ Write the sentinel line here. }
        f.WriteLine(SaveFileSentinel.ToString());
    }

    public static void WriteSAtt(SAtt SA, StreamWriter f)
    {
        //{ Output the provided list of string attributes. }

        //{ Export String Attributes }
        while (SA != null)
        {
            //{ Error check- only output valid string attributes. }
            if (SA.info.IndexOf('<') >= 0)
            {
                f.WriteLine(SA.info);
            }
            SA = SA.next;
        }

        //{ Write the sentinel line here. }
        f.WriteLine("Z");
    }

    public static NAtt ReadNAtt(StreamReader f)
    {
        //{ Read some numeric attributes from the file. }

        //{ Initialize the list to Nil. }
        NAtt it = null;

        //{ Keep processing this file until either the sentinel }
        //{ is encountered or we run out of data. }
        int N = SaveFileSentinel;
        do
        {
            string line = f.ReadLine();

            //{ Extract the action code. }
            N = texutil.ExtractValue(ref line);

            //{ If this action code implies that there's a gear }
            //{ to load, get to work. }
            if (N == SaveFileContinue)
            {
                //{ Read the specific values of this NAtt. }
                int G = texutil.ExtractValue(ref line);
                int S = texutil.ExtractValue(ref line);
                int V = texutil.ExtractValue(ref line);
                SetNAtt(ref it, G, S, V);
            }
        }
        while (N != SaveFileSentinel);

        return it;
    }

    public static SAtt ReadSAtt(StreamReader f)
    {
        //{ Read some string attributes from the file. }
        SAtt it = null;

        //{ Keep processing this file until either the sentinel }
        //{ is encountered or we run out of data. }
        string line = "Z";
        do
        {
            //{ read the next line of the file. }
            line = f.ReadLine();

            //{ If this is a valid string attribute, file it. }
            if (line.IndexOf('<') >= 0)
            {
                StoreSAtt(ref it, line);
            }
        }
        while (line != "Z");

        return it;
    }
}
