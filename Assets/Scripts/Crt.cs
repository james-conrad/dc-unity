using System;

public class Crt
{
    public enum Color
    {
        Cyan,
        Black,
        Blue,
        DarkGray,
        Green,
        LightBlue,
        LightCyan,
        LightGray,
        LightGreen,
        LightMagenta,
        LightRed,
        Magenta,
        Red,
        White,
        Yellow,
    }

    public static void ClrScr()
    {
        lock(consoleLock)
        {
            for (int y = windowY1 - 1; y < windowY2; ++y)
            {
                for (int x = windowX1 - 1; x < windowX2; ++x)
                {
                    console[x, y].color = textColor;
                    console[x, y].bgColor = backgroundColor;
                    console[x, y].c = ' ';
                }
            }

            cursorX = windowX1;
            cursorY = windowY1;
        }
    }

    public static void ClrEol()
    {
        lock (consoleLock)
        {
            for (int x = cursorX - 1; x < windowX2; ++x)
            {
                console[x, cursorY - 1].color = textColor;
                console[x, cursorY - 1].bgColor = backgroundColor;
                console[x, cursorY - 1].c = ' ';
            }
        }
    }

    public static void GotoXY(int x, int y)
    {
        lock (consoleLock)
        {
            cursorX = ClampWindowX(windowX1 + x - 1);
            cursorY = ClampWindowY(windowY1 + y - 1);
        }
    }

    public static void TextColor(Color c)
    {
        lock (consoleLock)
        {
            textColor = c;
        }
    }

    public static void TextBackground(Color c)
    {
        lock (consoleLock)
        {
            backgroundColor = c;
        }
    }

    public static void Window(int x1, int y1, int x2, int y2)
    {
        lock (consoleLock)
        {
            windowX1 = ClampX(x1);
            windowY1 = ClampY(y1);
            windowX2 = ClampX(x2);
            windowY2 = ClampY(y2);

            if (windowX2 < windowX1)
                windowX2 = windowX1;

            if (windowY2 < windowY1)
                windowY2 = windowY1;

            cursorX = windowX1;
            cursorY = windowY1;
        }
    }

    public static void Write(char c)
    {
        lock (consoleLock)
        {
            if (c != '\n')
            {
                console[cursorX - 1, cursorY - 1].color = textColor;
                console[cursorX - 1, cursorY - 1].bgColor = backgroundColor;
                console[cursorX - 1, cursorY - 1].c = c;
                cursorX += 1;
            }

            if (cursorX > windowX2 || c == '\n')
            {
                cursorX = windowX1;
                cursorY++;

                if (cursorY > windowY2)
                {
                    if (autoScroll)
                    {
                        Scroll();
                    }

                    cursorY = windowY2;
                }
            }
        }
    }

    public static void Write(string s)
    {
        lock (consoleLock)
        {
            foreach (char c in s)
            {
                Write(c);
            }
        }
    }

    public static int WhereX()
    {
        lock (consoleLock)
        {
            return cursorX - windowX1 + 1;
        }
    }

    public static int WhereY()
    {
        lock (consoleLock)
        {
            return cursorY - windowY1 + 1;
        }
    }

    public static void CursorOn()
    {
        lock (consoleLock)
        {
            cursorOn = true;
        }
    }

    public static void CursorOff()
    {
        lock (consoleLock)
        {
            cursorOn = false;
        }
    }

    public static void AutoScrollOn()
    {
        lock (consoleLock)
        {
            autoScroll = true;
        }
    }

    public static void AutoScrollOff()
    {
        lock (consoleLock)
        {
            autoScroll = false;
        }
    }

    public static void GetCursorInfo(out bool on, out int x, out int y)
    {
        lock (consoleLock)
        {
            on = cursorOn;
            x = cursorX;
            y = cursorY;
        }
    }

    public static void GetGlyphs(Glyph[,] glyphs)
    {
        lock (consoleLock)
        {
            int minLenX = Math.Min(glyphs.GetLength(0), console.GetLength(0));
            int minLenY = Math.Min(glyphs.GetLength(1), console.GetLength(1));

            for (int y = 0; y < minLenY; ++y)
            {
                for (int x = 0; x < minLenX; ++x)
                {
                    glyphs[x, y] = console[x, y];
                }
            }
        }
    }

    public const int consoleWidth = 80;
    public const int consoleHeight = 25;

    public struct Glyph
    {
        public char c;
        public Color color;
        public Color bgColor;
    }

    public static Glyph[,] console = new Glyph[consoleWidth, consoleHeight];

    static int ClampX(int x)
    {
        return x < 1 ? 1 : x > consoleWidth ? consoleWidth : x;
    }

    static int ClampY(int y)
    {
        return y < 1 ? 1 : y > consoleHeight ? consoleHeight : y;
    }

    static int ClampWindowX(int x)
    {
        return x < windowX1 ? windowX1 : x > windowX2 ? windowX2: x;
    }

    static int ClampWindowY(int y)
    {
        return y < windowY1 ? windowY1 : y > windowY2 ? windowY2 : y;
    }

    static void Scroll()
    {
        for (int y = windowY1 - 1; y < windowY2; ++y)
        {
            if (y < windowY2 - 1)
            {
                for (int x = windowX1 - 1; x < windowX2; ++x)
                {
                    console[x, y] = console[x, y + 1];
                }
            }
            else
            {
                for (int x = windowX1 - 1; x < windowX2; ++x)
                {
                    console[x, y].color = textColor;
                    console[x, y].bgColor = backgroundColor;
                    console[x, y].c = ' ';
                }
            }
        }
    }

    static object consoleLock = new object();

    static Color textColor = Color.White;
    static Color backgroundColor = Color.Black;
    static int cursorX = 1;
    static int cursorY = 1;
    static int windowX1 = 1;
    static int windowY1 = 1;
    static int windowX2 = consoleWidth;
    static int windowY2 = consoleHeight;
    static bool cursorOn = true;
    static bool autoScroll = false;
}

