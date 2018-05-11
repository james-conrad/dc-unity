using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitTester : MonoBehaviour
{

    void Test_texutil()
    {
        {
            string s = "   Hello Something\n";
            texutil.DeleteWhiteSpace(ref s);
            if (s != "Hello Something\n")
                throw new System.Exception("DeleteWhiteSpace sucks.");
        }

        {
            string s = "This 100# <something>\n";
            string one = texutil.ExtractWord(ref s);
            if (one != "This")
                throw new System.Exception("ExtractWord sucks.");

            string two = texutil.ExtractWord(ref s);
            if (two != "100#")
                throw new System.Exception("ExtractWord sucks.");

            string three = texutil.ExtractWord(ref s);
            if (three != "<something>\n")
                throw new System.Exception("ExtractWord sucks.");

            string four = texutil.ExtractWord(ref s);
            if (four != string.Empty)
                throw new System.Exception("ExtractWord sucks.");
        }

        {
            string s = "100 horses 10.2 22blep 55";
            int one = texutil.ExtractValue(ref s);
            if (one != 100)
                throw new System.Exception("ExtractValue sucks.");

            int two = texutil.ExtractValue(ref s);
            if (two != 0)
                throw new System.Exception("ExtractValue sucks.");

            int three = texutil.ExtractValue(ref s);
            if (three != 0)
                throw new System.Exception("ExtractValue sucks.");

            if (s != "22blep 55")
                throw new System.Exception("ExtractValue sucks.");

            int four = texutil.ExtractValue(ref s);
            if (four != 0)
                throw new System.Exception("ExtractValue sucks.");

            int five = texutil.ExtractValue(ref s);
            if (five != 55)
                throw new System.Exception("ExtractValue sucks.");

            int six = texutil.ExtractValue(ref s);
            if (six != 0)
                throw new System.Exception("ExtractValue sucks.");
        }

        {
            if (texutil.RetrieveAString("How <are> you") != "are")
                throw new System.Exception("RetrieveAString sucks.");
            if (texutil.RetrieveAString("How <are you>") != "are you")
                throw new System.Exception("RetrieveAString sucks.");
            if (texutil.RetrieveAString("<  How <are> you") != "  How <are")
                throw new System.Exception("RetrieveAString sucks.");
            if (texutil.RetrieveAString("How <> you") != "")
                throw new System.Exception("RetrieveAString sucks.");
            if (texutil.RetrieveAString("How <123> you") != "123")
                throw new System.Exception("RetrieveAString sucks.");
            if (texutil.RetrieveAString("How you") != "")
                throw new System.Exception("RetrieveAString sucks.");
            if (texutil.RetrieveAString("") != "")
                throw new System.Exception("RetrieveAString sucks.");
            if (texutil.RetrieveAString("Hello <amle laks elk f") != "")
                throw new System.Exception("RetrieveAString sucks.");
            if (texutil.RetrieveAString("Hello amle laks> elk f") != "")
                throw new System.Exception("RetrieveAString sucks.");
        }

        {
            if (texutil.BStr(123) != " 123")
                throw new System.Exception("BStr sucks.");
            if (texutil.BStr(-223) != "-223")
                throw new System.Exception("BStr sucks.");
            if (texutil.BStr(0) != " 0")
                throw new System.Exception("BStr sucks.");
        }

        {
            string s = "hello123.456er";
            if (texutil.ExtractNumber(s, 0, 100) != 0)
                throw new System.Exception("ExtractNumber sucks.");
            if (texutil.ExtractNumber(s, -2, 4) != 0)
                throw new System.Exception("ExtractNumber sucks.");
            if (texutil.ExtractNumber(s, 5, 1) != 1)
                throw new System.Exception("ExtractNumber sucks.");
            if (texutil.ExtractNumber(s, 5, 2) != 12)
                throw new System.Exception("ExtractNumber sucks.");
            if (texutil.ExtractNumber(s, 5, 3) != 123)
                throw new System.Exception("ExtractNumber sucks.");
            if (texutil.ExtractNumber(s, 5, 4) != 0)
                throw new System.Exception("ExtractNumber sucks.");
            if (texutil.ExtractNumber(s, 10, 0) != 0)
                throw new System.Exception("ExtractNumber sucks.");
            if (texutil.ExtractNumber(s, 10, 1) != 5)
                throw new System.Exception("ExtractNumber sucks.");
            if (texutil.ExtractNumber(s, 10, 2) != 56)
                throw new System.Exception("ExtractNumber sucks.");
            if (texutil.ExtractNumber(s, 10, 3) != 0)
                throw new System.Exception("ExtractNumber sucks.");
        }

        {
            string s = "123abc \n";
            texutil.DeleteFirstChar(ref s);
            if (s != "23abc \n")
                throw new System.Exception("DeleteFirstChar sucks.");
            texutil.DeleteFirstChar(ref s);
            if (s != "3abc \n")
                throw new System.Exception("DeleteFirstChar sucks.");
            texutil.DeleteFirstChar(ref s);
            if (s != "abc \n")
                throw new System.Exception("DeleteFirstChar sucks.");
            texutil.DeleteFirstChar(ref s);
            if (s != "bc \n")
                throw new System.Exception("DeleteFirstChar sucks.");
            texutil.DeleteFirstChar(ref s);
            if (s != "c \n")
                throw new System.Exception("DeleteFirstChar sucks.");
            texutil.DeleteFirstChar(ref s);
            if (s != " \n")
                throw new System.Exception("DeleteFirstChar sucks.");
            texutil.DeleteFirstChar(ref s);
            if (s != "\n")
                throw new System.Exception("DeleteFirstChar sucks.");
            texutil.DeleteFirstChar(ref s);
            if (s != "")
                throw new System.Exception("DeleteFirstChar sucks.");
            texutil.DeleteFirstChar(ref s);
            if (s != "")
                throw new System.Exception("DeleteFirstChar sucks.");
        }
    }

    void Test_rpgdice()
    {
        {
            int[] high = { 0, 0 };
            int[] low = { int.MaxValue, int.MaxValue };
            float[] average = { 0.0f, 0.0f };

            // do it 1000 times..
            for (int i = 0; i < 1000; ++i)
            {
                int d4 = rpgdice.Dice(4);
                if (d4 <= 0)
                    throw new System.Exception("Dice sucks.");
                int d10 = rpgdice.Dice(10);
                if (d10 <= 0)
                    throw new System.Exception("Dice sucks.");

                if (high[0] < d4)
                    high[0] = d4;

                if (high[1] < d10)
                    high[1] = d10;

                if (low[0] > d4)
                    low[0] = d4;

                if (low[1] > d10)
                    low[1] = d10;

                average[0] += (float)d4 / 1000.0f;
                average[1] += (float)d10 / 1000.0f;
            }

            Debug.Log(string.Format("d4 high: {0} low: {1} avg: {2}", high[0], low[0], average[0]));
            Debug.Log(string.Format("d10 high: {0} low: {1} avg: {2}", high[1], low[1], average[1]));
        }

        {
            for (int s = 0; s < 20; ++s)
            {
                int high = 0;
                int low = int.MaxValue;
                float average = 0.0f;

                for (int i = 0; i < 1000; ++i)
                {
                    int roll = rpgdice.RollStep(s);
                    if (low > roll) low = roll;
                    if (high < roll) high = roll;
                    average += (float)roll / 1000.0f;
                }

                Debug.Log(string.Format("step: {0} high: {1} low: {2} avg: {3}", s, high, low, average));
            }
        }

        {
            for (int n = 0; n < 15; ++n)
            {
                int high = 0;
                int low = int.MaxValue;
                float average = 0.0f;

                for (int i = 0; i < 1000; ++i)
                {
                    int roll = rpgdice.RollStat(n);

                    if (low > roll) low = roll;
                    if (high < roll) high = roll;
                    average += (float)roll / 1000.0f;
                }

                Debug.Log(string.Format("Stat Dice: {0} high: {1} low: {2} avg: {3}", n, high, low, average));
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        Test_texutil();
        Test_rpgdice();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
