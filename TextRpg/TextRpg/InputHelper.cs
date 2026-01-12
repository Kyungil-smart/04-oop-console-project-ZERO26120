using System;

static class InputHelper
{
    public static char InputChoice(string msg, char[] allowed)
    {
        char sel = '\0';
        while(true)
        {
            Console.Write(msg);
            var key = Console.ReadKey(true);
            sel = key.KeyChar;
            if(Array.Exists(allowed,x=>x==sel)) break;
        }
        Console.WriteLine(sel);
        return sel;
    }

    public static int InputMenu(string[] options)
    {
        int index=0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            for(int i=0;i<options.Length;i++)
            {
                if(i==index) Console.WriteLine($"> {options[i]}");
                else Console.WriteLine($"  {options[i]}");
            }
            key = Console.ReadKey(true).Key;

            if(key==ConsoleKey.UpArrow) index--;
            if(key==ConsoleKey.DownArrow) index++;

            if(index<0) index = options.Length-1;
            if(index>=options.Length) index = 0;
        } while(key!=ConsoleKey.Enter);

        return index;
    }
}