using System;

public class statusfx
{
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
}
