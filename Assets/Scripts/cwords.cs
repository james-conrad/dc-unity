using System;
using System.IO;

public class cwords
{
    //{This unit is brought to you by the letter "C".}
    //{It handles two different "C" words - Clouds and}
    //{Computers.}
    public class CloudDesc
    {
        public CloudDesc(string name, Crt.Color color, int Obscurement, bool pass)
        {
            this.name = name;
            this.color = color;
            this.Obscurement = Obscurement;
            this.pass = pass;
        }

        public string name;
        public Crt.Color color;
        public int Obscurement;
        public bool pass;
    }

    public class Cloud
    {
        public int Kind;
        public int Duration; //{The ComTime at which the cloud dissipates.}
        public texmodel.Model M;
        public Cloud next;
    }

    public class MPUDesc
    {
        public MPUDesc(string name, Crt.Color color, int SecPass)
        {
            this.name = name;
            this.color = color;
            this.SecPass = SecPass;
        }

        public string name;
        public Crt.Color color;
        public int SecPass;
    }

    //{This is the record that describes computer}
    //{terminals.The name, MPU, comes from the}
    //{anime series Cowboy Bebop.}
    public class MPU
    {
        public int kind;
        public texmodel.Model M;
        public string Attr; //{ MPU Attributes }
        public MPU next;
    }

    public const int NumCloud = 3;

    //{ NOTE: A cloud with an obscurement value of -1 will completely block }
    //{ line of sight, and cannot even be fired through. Such a cloud should }
    //{ have Pass set to False, or bad things may result. }
    public static CloudDesc[] CloudMan = new CloudDesc[NumCloud]
    {
        new CloudDesc("Etherial Mist",
            color: Crt.Color.LightCyan,
            Obscurement: 3,
            pass: true
        ),
        new CloudDesc("Smoke Screen",
            color: Crt.Color.LightGray,
            Obscurement: 3,
            pass: true
        ),
        new CloudDesc("Force Field",
            color: Crt.Color.Yellow,
            Obscurement: -1,
            pass: false
        ),
    };

    public const int NumMPU = 4;
    public static MPUDesc[] MPUMan = new MPUDesc[NumMPU]
    {
        new MPUDesc( "Info Terminal",
            color: Crt.Color.Yellow,
            SecPass: 2),
        new MPUDesc( "Medical Unit",
            color: Crt.Color.Red,
            SecPass: 12),
        new MPUDesc( "Primary Server \"Morgan\"",
            color: Crt.Color.LightMagenta,
            SecPass: 9),
        new MPUDesc( "Primary Server \"DesCartes\"",
            color: Crt.Color.Green,
            SecPass: 10),
     };

    public const int MKIND_Cloud = 3;
    public const int MKIND_MPU = 4;
    public const char CloudGFX = '*';
    public const char MPUGFX = '&';

    public static Cloud AddCloud(ref Cloud CList, texmaps.GameBoard gb, int C, int X, int Y, int D)
    {
        //{Add a cloud to the map at location X,Y.}

        //{Allocate memory for IT.}
        Cloud it = new Cloud();

        //{Attach IT to the list.}
        it.next = CList;
        CList = it;

        it.Kind = C;
        it.Duration = D;
        it.M = texmaps.GAddModel(gb, CloudGFX, CloudMan[C - 1].color, Crt.Color.White, CloudMan[C - 1].pass, X, Y, MKIND_Cloud);

        //{Set the obscurement for this model.}
        it.M.obs = CloudMan[C - 1].Obscurement;

        return it;
    }

    public static void CleanCloud(texmaps.GameBoard gb, ref Cloud LList)
    {
        //{Dispose of the list, freeing all associated system resources.}
        //{The models associated with each cloud will also be removed.}
        while (LList != null)
        {
            Cloud LTemp = LList.next;
            texmodel.RemoveModel(LList.M, ref gb.mlist, gb.mog);
            LList = LTemp;
        }
    }

    public static void RemoveCloud(ref Cloud LList, Cloud LMember, texmaps.GameBoard gb)
    {
        //{Locate and extract member LMember from list LList.}
        //{Then, dispose of LMember.}

        //{Initialize A and B}
        Cloud B = LList;
        Cloud A = null;

        //{Locate LMember in the list. A will thereafter be either null,}
        //{if LMember if first in the list, or it will be equal to the}
        //{element directly preceding LMember.}
        while (B != LMember && B != null)
        {
            A = B;
            B = B.next;
        }

        if (B == null)
        {
            //{Major FUBAR. The member we were trying to remove can't}
            //{be found in the list.}
            Crt.Write("ERROR- RemoveCloud asked to remove a link that doesnt exist.");
            do { rpgtext.RPGKey(); } while (true);
        }
        else if (A == null)
        {
            //{There's no element before the one we want to remove,}
            //{i.e. it's the first one in the list.}
            LList = B.next;

            //{Get rid of the model.}
            texmaps.GRemoveModel(B.M, gb);

            B = null;
        }
        else
        {
            //{ We found the attribute we want to delete and have another}
            //{ one standing before it in line.Go to work.}
            A.next = B.next;

            //{ Get rid of the model.}
            texmaps.GRemoveModel(B.M, gb);
            B = null;
        }
    }

    public static Cloud LocateCloud(texmodel.Model M, Cloud C)
    {
        //{Given model M, locate the cloud that is being referred to.}
        //{Return null if no such cloud can be found.}
        Cloud it = null;
        while (C != null && it == null)
        {
            if (C.M == M)
                it = C;
            C = C.next;
        }

        return it;
    }

    public static MPU AddMPU(ref MPU CList, texmaps.GameBoard gb, int C, int X, int Y)
    {
        //{Add a MPU to the map at location X,Y.}
        //{Allocate memory for IT.}
        MPU it = new MPU();

        //{Attach IT to the list.}
        it.next = CList;
        CList = it;

        it.kind = C;
        it.M = texmaps.GAddModel(gb, MPUGFX, MPUMan[C - 1].color, Crt.Color.White, false, X, Y, MKIND_MPU);

        return it;
    }

    public static void RemoveMPU(ref MPU LList, MPU LMember, texmaps.GameBoard gb)
    {
        //{Locate and extract member LMember from list LList.}
        //{Then, dispose of LMember.}

        //{Initialize A and B}
        MPU B = LList;
        MPU A = null;

        //{Locate LMember in the list. A will thereafter be either null,}
        //{if LMember if first in the list, or it will be equal to the}
        //{element directly preceding LMember.}
        while (B != LMember && B != null)
        {
            A = B;
            B = B.next;
        }

        if (B == null)
        {
            //{Major FUBAR. The member we were trying to remove can't}
            //{be found in the list.}
            Crt.Write("ERROR- RemoveMPU asked to remove a link that doesnt exist.\n");
            do { rpgtext.RPGKey(); } while (true);
        }
        else if (A == null)
        {
            //{There's no element before the one we want to remove,}
            //{i.e. it's the first one in the list.}
            LList = B.next;

            //{Get rid of the model.}
            texmaps.GRemoveModel(B.M, gb);

            B = null;
        }
        else
        {
            //{We found the attribute we want to delete and have another}
            //{one standing before it in line. Go to work.}
            A.next = B.next;

            //{Get rid of the model.}
            texmaps.GRemoveModel(B.M, gb);

            B = null;
        }
    }

    public static MPU LocateMPU(texmodel.Model M, MPU C)
    {
        //{Given model M, locate the MPU that is being referred to.}
        //{Return null if no such MPU can be found.}
        MPU it = null;
        while (C != null && it == null)
        {
            if (C.M == M)
                it = C;
            C = C.next;
        }

        return it;
    }

    public static void WriteClouds(Cloud CL, StreamWriter f)
    {
        //{Save the linked list of clouds to the file F.}
        while (CL != null)
        {
            f.WriteLine(CL.Kind);

            //{Record the position of the cloud}
            f.WriteLine(CL.M.x);
            f.WriteLine(CL.M.x);

            f.WriteLine(CL.Duration);
            CL = CL.next;
        }

        f.WriteLine(-1);
    }

    public static Cloud ReadClouds(StreamReader f, texmaps.GameBoard gb)
    {
        //{Read a list of clouds from disk.}
        //{Initialize the list to NIL.}
        Cloud CList = null;

        //{Keep reading data until we get a termination value, -1.}
        int N = int.Parse(f.ReadLine());
        while (N != -1)
        {
            //{Read the rest of the cloud data.}
            int X = int.Parse(f.ReadLine());
            int Y = int.Parse(f.ReadLine());
            int D = int.Parse(f.ReadLine());

            //{ Add this cloud to the list.}
            AddCloud(ref CList, gb, N, X, Y, D);

            N = int.Parse(f.ReadLine());
        }

        return CList;
    }

    public static void WriteMPU(MPU CL, StreamWriter f)
    {
        //{Save the linked list of computers to the file F.}
        while (CL != null)
        {
            f.WriteLine(CL.kind);

            //{Record the position of the computer}
            f.WriteLine(CL.M.x);
            f.WriteLine(CL.M.y);

            f.WriteLine(CL.Attr);
            CL = CL.next;
        }
        f.WriteLine(-1);
    }

    public static MPU ReadMPU(StreamReader f, texmaps.GameBoard gb)
    {
        //{Read a list of computers from disk.}
        //{Initialize the list to NIL.}
        MPU CList = null;

        //{Keep reading data until we get a termination value, -1.}
        int N = int.Parse(f.ReadLine());
        while (N != -1)
        {
            //{Read the rest of the cloud data.}
            int X = int.Parse(f.ReadLine());
            int Y = int.Parse(f.ReadLine());

            //{Add this computer to the list.}
            MPU Current = AddMPU(ref CList, gb, N, X, Y);
            Current.Attr = f.ReadLine();

            N = int.Parse(f.ReadLine());
        }

        return CList;
    }
}
