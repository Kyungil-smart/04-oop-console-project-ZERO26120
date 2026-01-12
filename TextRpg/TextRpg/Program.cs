using System;

class Program
{
    static void Main()
    {
        while (true)
        {
            int sel = Title();

            if (sel == 0)
            {
                Player p = new Player();
                Dungeon.Start(p);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }

    static int Title()
    {
        string[] menu = { "게임 시작", "종료" };
        int index = 0;

        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
 ████████╗██████╗ ███████╗████████╗
 ╚══██╔══╝██╔══██╗██╔════╝╚══██╔══╝
    ██║   ██████╔╝█████╗     ██║   
    ██║   ██╔══██╗██╔══╝     ██║   
    ██║   ██║  ██║███████╗   ██║   
    ╚═╝   ╚═╝  ╚═╝╚══════╝   ╚═╝   

 ██████╗ ██████╗  ██████╗ 
 ██╔══██╗██╔══██╗██╔════╝ 
 ██████╔╝██████╔╝██║  ███╗
 ██╔══██╗██╔═══╝ ██║   ██║
 ██║  ██║██║     ╚██████╔╝
 ╚═╝  ╚═╝╚═╝      ╚═════╝ 
");
            Console.ResetColor();

            Console.WriteLine();

            for (int i = 0; i < menu.Length; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {menu[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {menu[i]}");
                }
            }

            Console.WriteLine("\n↑ ↓ 이동  Enter 선택");

            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow) index--;
            if (key == ConsoleKey.DownArrow) index++;

            if (index < 0) index = menu.Length - 1;
            if (index >= menu.Length) index = 0;

            if (key == ConsoleKey.Enter)
                return index;
        }
    }
}