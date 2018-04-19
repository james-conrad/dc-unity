using System;

// { This unit contains various useful functions for dealing }
// { with strings. }

public class texutil
{
    public static void DeleteWhiteSpace(ref string s)
    {
        //{ Delete any whitespace which is at the beginning of }
        //{ string s. If s is nothing but whitespace, or if it }
        //{ contains nothing, return an empty string. }

        s = s.TrimStart();
    }

    public static string ExtractWord(ref string s)
    {
        //{ Extract the next word from string S. }
        //{ Return this substring as the function's result; }
        //{ truncate S so that it is now the remainder of the string. }
        //{ If there is no word to extract, both S and the function }
        //{ result will be set to empty strings. }

        //{ To start the process, strip all whitespace from the }
        //{ beginning of the string. }
        DeleteWhiteSpace(ref s);

        string[] split = s.Split(new char[] { ' ' }, 2);

        if (split.Length < 2)
        {
            s = string.Empty;
        }
        else
        {
            s = split[1];
        }

        return split[0];
    }

    public static int ExtractValue(ref string s)
    {
        //{ This is similar to the above procedure, but }
        //{ instead of a word it extracts a numeric value. }
        //{ Return 0 if the extraction should fail for any reason. }
        string word = ExtractWord(ref s);

        int n = 0;

        if (!int.TryParse(word, out n))
        {
            n = 0;
        }

        return n;
    }

    public static string RetrieveAString(string s)
    {
        //{ Retrieve an Alligator String from S. }
        //{ Alligator Strings are defined as the part of the string }
        //{ that both alligarors want to eat, i.e.between<and>. }

        int start = s.IndexOf('<');
        if (start < 0)
        {
            return string.Empty;
        }

        int end = s.IndexOf('>');
        if (end < 0)
        {
            return string.Empty;
        }

        return s.Substring(start + 1, end - start - 1);
    }

    public static string BStr(int n)
    {
        //{ This function functions as the BASIC Str function. }
        if (n >= 0)
            return String.Format(" {0}", n);

        return String.Format("{0}", n);
    }

    public static int ExtractNumber(string s, int start, int l)
    {
        //{ This procedure will extract a numerical value from the }
        //{ string, starting at point Start and continuing for L }
        //{ characters.Return a 0 for a failed conversion. }
        int n = 0;

        try
        {
            if (!int.TryParse(s.Substring(start, l), out n))
            {
                n = 0;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            n = 0;
        }

        return n;
    }

    public static void DeleteFirstChar(ref string s)
    {
        //{ Remove the first character from string S. }

        if (s != String.Empty)
        {
            s = s.Substring(1);
        }
    }
}
