public class WDM
{
    /* A Collection of window dimenstions and positions */
    /* used througout the game */

    /* General console dimension. */
    public const int CON_WIDTH = 100;
    public const int CON_HEIGHT = 41;

    /* Text Message Window. */
    public const int RPGMsg_X = 1;
    public const int RPGMsg_Y = 1;
    public const int RPGMsg_WIDTH = CON_WIDTH;
    public const int RPGMsg_HEIGHT = 6;
    public const int RPGMsg_X2 = RPGMsg_X + RPGMsg_WIDTH - 1;
    public const int RPGMsg_Y2 = RPGMsg_Y + RPGMsg_HEIGHT - 1;

    /* Map Display Window. */
    public const int Map_X = 1;
    public const int Map_Y = RPGMsg_Y2 + 1;
    public const int Map_WIDTH = CON_WIDTH;
    public const int Map_HEIGHT = CON_HEIGHT - RPGMsg_HEIGHT - 1;
    public const int Map_X2 = Map_X + Map_WIDTH - 1;
    public const int Map_Y2 = Map_Y + Map_HEIGHT - 1;

    /* Handy Map */
    public const int HM_X = 23;
    public const int HM_Y = RPGMsg_Y2 + 1;
    public const int HM_WIDTH = 36;
    public const int HM_HEIGHT = 21;
    public const int HM_X2 = HM_X + HM_WIDTH - 1;
    public const int HM_Y2 = HM_Y + HM_HEIGHT - 1;

    /* Equpment Window */
    public const int EqpWin_X = 3;
    public const int EqpWin_Y = RPGMsg_Y2 + 1;
    public const int EqpWin_WIDTH = 50;
    public const int EqpWin_HEIGHT = 8;
    public const int EqpWin_X2 = EqpWin_X + EqpWin_WIDTH - 1;
    public const int EqpWin_Y2 = EqpWin_Y + EqpWin_HEIGHT - 1;

    /* Inventory Description Window. */
    public const int DscWin_X = InvWin_X2 + 2;
    public const int DscWin_Y = RPGMsg_Y2 + 1;
    public const int DscWin_WIDTH = 29;
    public const int DscWin_HEIGHT = 20;
    public const int DscWin_X2 = DscWin_X + DscWin_WIDTH - 1;
    public const int DscWin_Y2 = DscWin_Y + DscWin_HEIGHT - 1;

    /* Inventory Window. */
    public const int InvWin_X = EqpWin_X;
    public const int InvWin_Y = EqpWin_Y2;
    public const int InvWin_WIDTH = EqpWin_WIDTH;
    public const int InvWin_HEIGHT = InvWin_Y2 - InvWin_Y + 1;
    public const int InvWin_X2 = InvWin_X + InvWin_WIDTH - 1;
    public const int InvWin_Y2 = CON_HEIGHT - 2;

    /* PC Stats Window. */
    public const int PCSWin_X = DscWin_X;
    public const int PCSWin_Y = DscWin_Y2 + 1;
    public const int PCSWin_WIDTH = DscWin_WIDTH;
    public const int PCSWin_HEIGHT = 5;
    public const int PCSWin_X2 = PCSWin_X + PCSWin_WIDTH - 1;
    public const int PCSWin_Y2 = PCSWin_Y + PCSWin_HEIGHT - 1;

    /* PC Stat Line. */
    public const int PCStat_X = 1;
    public const int PCStat_Y = CON_HEIGHT;
    public const int PCStat_WIDTH = CON_WIDTH;
    public const int PCStat_X2 = PCStat_X + PCStat_WIDTH - 1;

    /* Book Reading Window. */
    public const int Book_X = 1;
    public const int Book_Y = RPGMsg_Y2 + 1;
    public const int Book_WIDTH = CON_WIDTH;
    public const int Book_HEIGHT = CON_HEIGHT - Book_Y;
    public const int Book_X2 = Book_X + Book_WIDTH - 1;
    public const int Book_Y2 = Book_Y + Book_HEIGHT - 1;

    /* Computer Main Display window. */
    public const int UCM_X = 5;
    public const int UCM_Y = RPGMsg_Y2 + 3; //7;
    public const int UCM_WIDTH = 49;
    public const int UCM_HEIGHT = 15;
    public const int UCM_X2 = UCM_X + UCM_WIDTH - 1; //53;
    public const int UCM_Y2 = UCM_Y + UCM_HEIGHT - 1; //22;

    /* Computer Meta Control menu window. */
    public const int MCM_X = UCM_X2 + 3; //55;
    public const int MCM_Y = UCM_Y + 7; //14;
    public const int MCM_WIDTH = 23;
    public const int MCM_HEIGHT = 9;
    public const int MCM_X2 = MCM_X + MCM_WIDTH - 1; //77;
    public const int MCM_Y2 = MCM_Y + MCM_HEIGHT - 1; //22;
}
