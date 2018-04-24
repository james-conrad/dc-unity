public class WDM
{
    /* A Collection of window dimenstions and positions */
    /* used througout the game */

    /* General console dimension. */
    public const int CON_WIDTH = 80;
    public const int CON_HEIGHT = 25;

    /* Text Message Window. */
    public const int RPGMsg_X = 1;
    public const int RPGMsg_Y = 1;
    public const int RPGMsg_WIDTH = CON_WIDTH;
    public const int RPGMsg_HEIGHT = 3;
    public const int RPGMsg_X2 = RPGMsg_X + RPGMsg_WIDTH - 1;
    public const int RPGMsg_Y2 = RPGMsg_Y + RPGMsg_HEIGHT - 1;

    /* Handy Map */
    public const int HM_X = 23;
    public const int HM_Y = RPGMsg_Y2 + 1;
    public const int HM_WIDTH = 36;
    public const int HM_HEIGHT = 21;
    public const int HM_X2 = HM_X + HM_WIDTH - 1;
    public const int HM_Y2 = HM_Y + HM_HEIGHT - 1;

    /* Equpment Window */
    public const int EqpWin_X = 1;
    public const int EqpWin_Y = RPGMsg_Y2 + 1;
    public const int EqpWin_WIDTH = 49;
    public const int EqpWin_HEIGHT = 7;
    public const int EqpWin_X2 = EqpWin_X + EqpWin_WIDTH - 1;
    public const int EqpWin_Y2 = EqpWin_Y + EqpWin_HEIGHT - 1;

    /* Inventory Description Window. */
    public const int DscWin_X = 52;
    public const int DscWin_Y = RPGMsg_Y2 + 1;
    public const int DscWin_WIDTH = 28;
    public const int DscWin_HEIGHT = 12;
    public const int DscWin_X2 = DscWin_X + DscWin_WIDTH - 1;
    public const int DscWin_Y2 = DscWin_Y + DscWin_HEIGHT - 1;

    /* Inventory Window. */
    public const int InvWin_X = 1;
    public const int InvWin_Y = 11;
    public const int InvWin_WIDTH = 49;
    public const int InvWin_HEIGHT = 13;
    public const int InvWin_X2 = InvWin_X + InvWin_WIDTH - 1;
    public const int InvWin_Y2 = InvWin_Y + InvWin_HEIGHT - 1;

    /* PC Stats Window. */
    public const int PCSWin_X = 51;
    public const int PCSWin_Y = 16;
    public const int PCSWin_WIDTH = 29;
    public const int PCSWin_HEIGHT = 4;
    public const int PCSWin_X2 = PCSWin_X + PCSWin_WIDTH - 1;
    public const int PCSWin_Y2 = PCSWin_X2 + PCSWin_HEIGHT - 1;

    /* PC Stat Line. */
    public const int PCStat_X = 1;
    public const int PCStat_Y = CON_HEIGHT - 1;
    public const int PCStat_WIDTH = CON_WIDTH;
    public const int PCStat_X2 = PCStat_X + PCStat_WIDTH - 1;

    /* Book Reading Window. */
    public const int Book_X = 1;
    public const int Book_Y = RPGMsg_Y2 + 1;
    public const int Book_WIDTH = CON_WIDTH;
    public const int Book_HEIGHT = CON_HEIGHT - Book_Y;
    public const int Book_X2 = Book_X + Book_WIDTH - 1;
    public const int Book_Y2 = Book_Y + Book_HEIGHT - 1;
}
