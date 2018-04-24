using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConsoleRenderer : MonoBehaviour
{

    public int consoleWidth = 80;
    public int consoleHeight = 26;

    public TileBase[] asciiTiles;

    public Tilemap tilemap;
    public Tilemap background;
    public SpriteRenderer cursorSprite;

    public float blinkPeriod = 0.8f;

    float blinkCounter = 0.0f;

    Crt.Glyph[,] glyphs = new Crt.Glyph[Crt.consoleWidth, Crt.consoleHeight];

    Thread runningGame;

    // Use this for initialization
    void Start()
    {
        FillBackground();

        MakeRandom();

        MakeFrame();

        if (runningGame != null)
        {
            runningGame.Abort();
        }

        StartGame();
    }

    Color[] colors = new Color[]
    {
        Color.cyan,
        Color.black,
        new Color(0.04f, 0.1f, 0.9f),
        new Color(0.54f, 0.27f, 0.07f),

        new Color(0.23f, 0.25f, 0.31f),
        new Color(0.1f, 0.65f, 0.0f),
        new Color(0.3f, 0.25f, 1.0f),
        new Color(0.2f, 0.9f, 1.0f),

        new Color(0.63f, 0.65f, 0.7f),
        new Color(0.2f, 1.0f, 0.3f),
        new Color(0.9f, 0.2f, 1.0f),
        new Color(1.0f, 0.35f, 0.2f),

        new Color(0.5f, 0.1f, 0.6f),
        new Color(0.6f, 0.0f, 0.15f),
        Color.white,
        Color.yellow,
    };

    Color CrtColorToColor(Crt.Color c)
    {
        switch (c)
        {
            case Crt.Color.Cyan: return colors[0];
            case Crt.Color.Black: return colors[1];
            case Crt.Color.Blue: return colors[2];
            case Crt.Color.Brown: return colors[3];

            case Crt.Color.DarkGray: return colors[4];
            case Crt.Color.Green: return colors[5];
            case Crt.Color.LightBlue: return colors[6];
            case Crt.Color.LightCyan: return colors[7];

            case Crt.Color.LightGray: return colors[8];
            case Crt.Color.LightGreen: return colors[9];
            case Crt.Color.LightMagenta: return colors[10];
            case Crt.Color.LightRed: return colors[11];

            case Crt.Color.Magenta: return colors[12];
            case Crt.Color.Red: return colors[13];
            case Crt.Color.White: return colors[14];
            case Crt.Color.Yellow: return colors[15];
        }

        return Color.black;
    }

    static System.Exception threadEx;

    void StartGame()
    {
        runningGame = new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;

            try
            {
                deadcold.RunGame();
            }
            catch (System.Exception e)
            {
                threadEx = e;
            }
        });

        runningGame.Start();
    }

    void FillBackground()
    {
        Vector3Int tilePos = new Vector3Int(0, 0, 0);

        for (int y = 0; y < consoleHeight; ++y)
        {
            tilePos.y = consoleHeight / 2 - y;

            for (int x = 0; x < consoleWidth; ++x)
            {
                tilePos.x = -consoleWidth / 2 + x;

                background.SetTile(tilePos, asciiTiles[219]);
                background.SetColor(tilePos, Color.black);
                Matrix4x4 m = background.GetTransformMatrix(tilePos);
                background.SetTransformMatrix(tilePos, m * Matrix4x4.Scale(new Vector3(1.1f, 1.1f, 1.1f)));
            }
        }
    }

    void MakeRandom()
    {
        Vector3Int tilePos = new Vector3Int(0, 0, 0);

        for (int y = 0; y < consoleHeight; ++y)
        {
            tilePos.y = consoleHeight / 2 - y;

            for (int x = 0; x < consoleWidth; ++x)
            {
                tilePos.x = -consoleWidth / 2 + x;

                tilemap.SetTile(tilePos, asciiTiles[Random.Range(0, 256)]);
                tilemap.SetColor(tilePos, new Color(Random.value, Random.value, Random.value));
            }
        }
    }

    void MakeFrame()
    {
        Vector3Int tilePos = new Vector3Int(0, 0, 0);

        int top = consoleHeight / 2;
        int bottom = consoleHeight / 2 - consoleHeight + 1;
        int left = -consoleWidth / 2;
        int right = -consoleWidth / 2 + consoleWidth - 1;

        for (int x = 1; x < consoleWidth - 1; ++x)
        {
            tilePos.x = -consoleWidth / 2 + x;

            tilePos.y = top;
            tilemap.SetTile(tilePos, asciiTiles[205]);
            tilemap.SetColor(tilePos, Color.white);

            tilePos.y = bottom;
            tilemap.SetTile(tilePos, asciiTiles[205]);
            tilemap.SetColor(tilePos, Color.white);
        }

        for (int y = 1; y < consoleHeight - 1; ++y)
        {
            tilePos.y = consoleHeight / 2 - y;

            tilePos.x = left;
            tilemap.SetTile(tilePos, asciiTiles[186]);
            tilemap.SetColor(tilePos, Color.white);
            tilePos.x = right;
            tilemap.SetTile(tilePos, asciiTiles[186]);
            tilemap.SetColor(tilePos, Color.white);
        }

        tilePos.x = left;

        tilePos.y = top;
        tilemap.SetTile(tilePos, asciiTiles[201]);
        tilemap.SetColor(tilePos, Color.white);

        tilePos.y = bottom;
        tilemap.SetTile(tilePos, asciiTiles[200]);
        tilemap.SetColor(tilePos, Color.white);

        tilePos.x = right;

        tilePos.y = top;
        tilemap.SetTile(tilePos, asciiTiles[187]);
        tilemap.SetColor(tilePos, Color.white);

        tilePos.y = bottom;
        tilemap.SetTile(tilePos, asciiTiles[188]);
        tilemap.SetColor(tilePos, Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            // '\b' for backspace '\n' for enter  '\r' for return.
            rpgtext.keys.AddKeypress(new rpgtext.Key(c));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rpgtext.keys.AddKeypress(new rpgtext.Key(rpgtext.Key.Type.ARROW_UP));
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rpgtext.keys.AddKeypress(new rpgtext.Key(rpgtext.Key.Type.ARROW_DOWN));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rpgtext.keys.AddKeypress(new rpgtext.Key(rpgtext.Key.Type.ARROW_LEFT));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rpgtext.keys.AddKeypress(new rpgtext.Key(rpgtext.Key.Type.ARROW_RIGHT));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            rpgtext.keys.AddKeypress(new rpgtext.Key(rpgtext.Key.Type.ESC));
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            rpgtext.keys.AddKeypress(new rpgtext.Key('\t'));
        }

        if (runningGame != null)
        {
            if (runningGame.ThreadState == ThreadState.Aborted || runningGame.ThreadState == ThreadState.Stopped)
            {
                runningGame.Join();
                Debug.Log("Thread exited..");


                if (threadEx != null)
                {
                    Debug.LogException(threadEx);
                    threadEx = null;
                    StartGame();
                }
                else
                {
                    Application.Quit();
                }
            }
        }

        Crt.GetGlyphs(glyphs);

        bool cursorOn;
        int cursorX;
        int cursorY;
        Crt.GetCursorInfo(out cursorOn, out cursorX, out cursorY);

        Vector3Int tilePos = new Vector3Int(0, 0, 0);

        for (int y = 0; y < Crt.consoleHeight; ++y)
        {
            tilePos.y = Crt.consoleHeight / 2 - y;

            for (int x = 0; x < Crt.consoleWidth; ++x)
            {
                tilePos.x = -Crt.consoleWidth / 2 + x;

                tilemap.SetTile(tilePos, asciiTiles[glyphs[x, y].c]);
                tilemap.SetColor(tilePos, CrtColorToColor(glyphs[x, y].color));
                background.SetColor(tilePos, CrtColorToColor(glyphs[x, y].bgColor));
            }
        }

        blinkCounter += Time.deltaTime;
        bool blinkOn = Mathf.Repeat(blinkCounter, blinkPeriod) > blinkPeriod * 0.5f;

        if (cursorOn && blinkOn)
        {
            cursorSprite.enabled = true;

            cursorSprite.transform.position = new Vector3(
                (float)(-Crt.consoleWidth / 2 + cursorX) - 0.5f,
                (float)((Crt.consoleHeight / 2 - cursorY + 1) + 0.5f) * 1.5f - 0.75f,
                0.0f);
        }
        else
        {
            cursorSprite.enabled = false;
        }
    }
}
