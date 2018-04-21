using System;
using System.IO;

public class statusfx
{
	//{This unit deals with status change effects.}

	//{ By the new system, status changes don't have a value and }
	//{ duration; they have a value, and that's about it. }
	public const int NAG_StatusChange = 10;

    //{Status Change Values}
    //{Positive ones are good for you, Negative ones are bad}
    //{ Zero is used as the status change save file sentinel. }
    public const int SEF_DrainBase = 3; //{Used to calculate Attribute Drain codes}
    public const int SEF_Poison = -3;
    public const int SEF_Sleep = -2;
    public const int SEF_Paralysis = -1;
    public const int SEF_VisionBonus = 1;
    public const int SEF_Regeneration = 2;
    public const int SEF_ArmorBonus = 3;
    public const int SEF_CCDmgBonus = 4;
    public const int SEF_StealthBonus = 5;
    public const int SEF_SpeedBonus = 6;
    public const int SEF_H2HBonus = 7;
    public const int SEF_MslBonus = 8;
    public const int SEF_Restoration = 9;
    public const int SEF_BoostBase = 9; //{ Used to calculate Attribute Boost codes }

	public const int NumNegSF = 11;
	public static string[] NegSFName = new string[NumNegSF]
    {
        "Paralyzed","Asleep","Poisoned","Weakened","Fatigued",
        "Slowed","Dizzy","Jinxed","Dazed","Light Headed",
        "Cursed",
    };

	//{Element values.}
	public const int NumElem = 5;
    public const int ELEM_Normal = 0;
    public const int ELEM_Fire = 1;
    public const int ELEM_Cold = 2;
    public const int ELEM_Lit = 3;
    public const int ELEM_Acid = 4;
    public const int ELEM_Holy = 5;


    public static void UpdateStatusList(ref plotbase.NAtt SL)
    {
        //{ Scan through the status list SL. }
        //{ For each status whose value isn't -1, value gets decremented }
        //{ by one. If value has reached zero, the status change is }
        //{ removed from the list. }
        plotbase.NAtt l = SL;
        plotbase.NAtt l2 = null;
        while (l != null)
        {
            l2 = l.next;
            if (l.V > 0)
            {
                l.V -= 1;
            }
            if (l.V == 0)
            {
                plotbase.RemoveNAtt(ref SL, l);
            }
            l = l2;
        }
    }

    public static void ReadObsoleteSFX(StreamReader f)
    {
        //{ From file F, read a list of obsolete status FX descriptions. }
        string s = f.ReadLine();
        while (s != "0")
        {
            s = f.ReadLine();
        }
    }



}
