using System;

class Program
{
    static Player player;

    static void Main()
    {
        ShowTitle();
    }

    public static void ShowTitle()
    {
        string[] menu = { "게임 시작", "종료" };
        int index = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(@"
$$$$$$$\  $$$$$$$\   $$$$$$\  
$$  __$$\ $$  __$$\ $$  __$$\ 
$$ |  $$ |$$ |  $$ |$$ /  \__|
$$$$$$$  |$$$$$$$  |$$ |$$$$\ 
$$  __$$< $$  ____/ $$ |\_$$ |
$$ |  $$ |$$ |      $$ |  $$ |
$$ |  $$ |$$ |      \$$$$$$  |
\__|  \__|\__|       \______/ 
");

            for (int i = 0; i < menu.Length; i++)
                Console.WriteLine(i == index ? $"> {menu[i]}" : $"  {menu[i]}");

            Console.WriteLine("\n방향키 ↑↓로 선택, Enter로 결정");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index = (index - 1 + menu.Length) % menu.Length;
            else if (key == ConsoleKey.DownArrow) index = (index + 1) % menu.Length;
            else if (key == ConsoleKey.Enter)
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

        while (true)
        {
            Console.Clear();
            Console.WriteLine("직업 선택\n");

            for (int i = 0; i < jobs.Length; i++)
                Console.WriteLine(i == index ? $"> {jobs[i]}" : $"  {jobs[i]}");

            Console.WriteLine("\n방향키 ↑↓로 선택, Enter로 결정");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index = (index - 1 + jobs.Length) % jobs.Length;
            else if (key == ConsoleKey.DownArrow) index = (index + 1) % jobs.Length;
            else if (key == ConsoleKey.Enter)
            {
                PlayerJob job = (index == 0) ? PlayerJob.Warrior : PlayerJob.Mage;
                player = new Player(job);
                Town.Enter(player);
                return;
            }
        }
    }
}
