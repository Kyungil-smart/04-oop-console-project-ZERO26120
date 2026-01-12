using System;

class Program
{
    static void Main()
    {
        TitleMenu();
    }

    public static void TitleMenu()
    {
        string[] options = { "게임 시작", "종료" };
        int idx = 0;
        ConsoleKey key;

        do
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

            for (int i = 0; i < options.Length; i++)
            {
                if (i == idx)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("> " + options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("  " + options[i]);
                }
            }

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow) idx--;
            if (key == ConsoleKey.DownArrow) idx++;
            if (idx < 0) idx = options.Length - 1;
            if (idx >= options.Length) idx = 0;

        } while (key != ConsoleKey.Enter);

        if (idx == 0)
        {
            // 직업 선택
            string[] jobs = { "전사", "마법사" };
            int jobIdx = 0;
            ConsoleKey jkey;

            do
            {
                Console.Clear();
                Console.WriteLine("직업 선택\n");
                for (int i = 0; i < jobs.Length; i++)
                {
                    if (i == jobIdx)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("> " + jobs[i]);
                        Console.ResetColor();
                    }
                    else
                        Console.WriteLine("  " + jobs[i]);
                }

                jkey = Console.ReadKey(true).Key;
                if (jkey == ConsoleKey.UpArrow) jobIdx--;
                if (jkey == ConsoleKey.DownArrow) jobIdx++;
                if (jobIdx < 0) jobIdx = jobs.Length - 1;
                if (jobIdx >= jobs.Length) jobIdx = 0;

            } while (jkey != ConsoleKey.Enter);

            PlayerJob selectedJob = jobIdx == 0 ? PlayerJob.Warrior : PlayerJob.Mage;
            Player player = new Player(selectedJob);

            Town.Enter(player);
        }
        else
        {
            Environment.Exit(0);
        }
    }
}
