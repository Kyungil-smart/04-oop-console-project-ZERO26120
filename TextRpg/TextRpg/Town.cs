using System;

static class Town
{
    public static void Enter(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== 마을 ===");
            Console.ResetColor();

            string[] options = { "던전 입장", "휴식" };
            int index = MenuSelect(options);

            if (index == 0)
            {
                Dungeon.Start(player);
                return;
            }
            else if (index == 1)
            {
                player.Rest();
                Console.WriteLine("체력과 스킬 회복 완료!");
                Console.ReadKey();
            }
        }
    }

    static int MenuSelect(string[] options)
    {
        int idx = 0;
        ConsoleKey key;
        do
        {
            Console.Clear();
            for (int i = 0; i < options.Length; i++)
            {
                if (i == idx)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("> " + options[i]);
                    Console.ResetColor();
                }
                else
                    Console.WriteLine("  " + options[i]);
            }

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow) idx--;
            if (key == ConsoleKey.DownArrow) idx++;
            if (idx < 0) idx = options.Length - 1;
            if (idx >= options.Length) idx = 0;

        } while (key != ConsoleKey.Enter);
        return idx;
    }
}