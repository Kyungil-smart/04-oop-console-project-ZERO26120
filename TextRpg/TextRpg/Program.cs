using System;

class Program
{
    static void Main()
    {
        ShowTitle();
    }

    public static void ShowTitle()
    {
        string[] menu = { "게임 시작", "종료" };
        int index = 0;
        ConsoleKey key;

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
    ╚═╝   ╚═╝  ╚══════╝   ╚═╝   

");
            Console.ResetColor();

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

            Console.WriteLine("\n↑↓ 방향키 선택, Enter 결정");

            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index--;
            if (key == ConsoleKey.DownArrow) index++;
            if (index < 0) index = menu.Length - 1;
            if (index >= menu.Length) index = 0;

            if (key == ConsoleKey.Enter)
            {
                if (index == 0) SelectJob();
                else Environment.Exit(0);
            }
        }
    }

    static void SelectJob()
    {
        string[] jobs = { "전사", "마법사" };
        int index = 0;
        ConsoleKey key;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("직업 선택");

            for (int i = 0; i < jobs.Length; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {jobs[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {jobs[i]}");
                }
            }

            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index--;
            if (key == ConsoleKey.DownArrow) index++;
            if (index < 0) index = jobs.Length - 1;
            if (index >= jobs.Length) index = 0;

            if (key == ConsoleKey.Enter)
            {
                PlayerJob job = index == 0 ? PlayerJob.Warrior : PlayerJob.Mage;
                Player player = new Player(job);
                Town.Enter(player);
                return;
            }
        }
    }
}
