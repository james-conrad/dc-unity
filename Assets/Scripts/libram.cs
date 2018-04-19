using System;

class libram
{
    //{This unit contains all of the books which the player}
    //{can find over the course of the game.}

    public static void ReadBook()
    {
        //{ Read book # N.}

        //{Set up the screen.}
        Crt.Window(1, 4, 80, 24);
        Crt.ClrScr();
        Crt.Window(1, 1, 80, 24);

        //{Create the menu.}
        rpgmenus.RPGMenu menu = rpgmenus.CreateRPGMenu(Crt.Color.LightGray, Crt.Color.Green, Crt.Color.LightGreen, 55, 8, 75, 20);
        for (int i = 0; i < ThePages.Length; ++i)
        {
            rpgmenus.AddRPGMenuItem(menu, ThePages[i].name, i);
        }

        int p = -1;
        do
        {
            p = rpgmenus.SelectMenu(menu, rpgmenus.RPMNoCleanup);

            if (p != -1)
            {
                Crt.TextColor(Crt.Color.Black);
                Crt.TextBackground(Crt.Color.Yellow);
                Crt.Window(6, 5, 33, 23);
                Crt.ClrScr();
                Crt.Window(9, 7, 30, 21);
                rpgtext.Delineate(ThePages[p].page, 21, 1);
                Crt.TextBackground(Crt.Color.Black);
                Crt.Window(1, 1, 80, 25);
            }
        }
        while (p >= 0);
    }

    struct BPage
    {
        public BPage(string name, string page)
        {
            this.name = name;
            this.page = page;
        }

        public string name; //{ A name for the page, for the menu}
        public string page; //{ The contents of the page.}
    }

    static BPage[] ThePages = new BPage[]
    {
        new BPage("05/33/64",
                  "Today I led the visitors group on a tour of the archive. It's so nice to get the chance to talk with fellow taphophiles."),
        new BPage("05/36/64 (1)",
                  "An archaeological dig site on Taltuo has uncovered an ancient burial site. The artefacts are being sent here for analysis- I'll be in charge of catelogueing the inventory. I am so excited."),
        new BPage("05/36/64 (2)",
                  "There are several monument stones and a time capsule full of books which will need to be recorded and translated. This is the kind of research I've always dreamed of doing- it doesn't come to DeadCold often."),
        new BPage("06/01/64 (1)",
                   "The computers were shut down again, today. Moira couldn't do her work without the research database so the two of us managed to spend most of the day together. Technology does improve our lives, sometimes."),
        new BPage("06/01/64 (2)",
                  "Balordo promised the system would be back up tomorrow. Later on, I saw off several members of the tour group as this was the last day of their visit."),
        new BPage("06/02/64",
                  "Today is a sad day. My task for this morning is to record the identity chips obtained from the casulties at Nogun-3. I am always saddened by the senseless loss of young lives."),
        new BPage("06/05/64 (1)",
                  "The artefacts from Taltuo have arrived at last! It's an impressive collection. Four stone urns engraved with alien writing; jewelry, masks, swords, and other ceremonial items; a cabinet containing a book."),
        new BPage("06/05/64 (2)",
                  "The book is especially interesting. It is constructed from embossed metal sheets, held together with an elaborate hinge. Once translated, it could answer many questions about this culture."),
        new BPage("06/07/64",
                  "It never rains on board the station. I mention this because it's raining on Earth today, according to the net. Next weekend I'll take the shuttle to Mascan and look at the sky."),
        new BPage("06/08/64 (1)",
                  "It is odd for a technological society, such as that which obviously existed on the moon of Taltuo, to bury their dead with so much paraphenalia. Were they hoping to provide useful information for future historians? The answer will have to wait..."),
        new BPage("06/08/64 (2)",
                  "The computers are on the blink again, and the translation software won't run. Nick threatened that if I call him one more time asking about it, he'll brain me. I hate the computers. I need this info now."),
        new BPage("06/09/64 (1)",
                  "Everything is up and running, the first of the translations have been produced. The four corpses were priests in some kind of religious sect, which explains all the gear buried along with them."),
        new BPage("06/09/64 (2)",
                  "Writing on the urns confirms that the four corpses were interred one after the other over a period of about 50My."),
        new BPage("06/12/64",
                  "I have been reading the holy book from Taltuo for the past few days. Earlier on, Moira dragged me out of the archive to go have lunch with her. I came back here immediately afterwards."),
        new BPage("06/13/64",
                  "This page is blank. All the pages following this one have been ripped out of the book."),
    };
}