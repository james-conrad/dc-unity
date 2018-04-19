using System;
using System.IO;

public class plotbase
{
    //{*** NUMERICAL ATTRIBUTE ***}
    public class NAtt
    {
        public int G; // {General}
        public int S; // {Specific}
        public int V; // {Value}
        public NAtt next;
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

    public static void WriteNAtt(NAtt llist, StreamWriter f)
    {
    }

    public static NAtt ReadNAtt(StreamReader f)
    {
        return new NAtt();
    }
}
