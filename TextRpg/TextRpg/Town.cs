using System;

static class Town
{
    public static void Enter(Player player)
    {
        string[] options = { "던전 입장", "휴식" };
        int index = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== 마을 ===\n");

            for (int i = 0; i < options.Length; i++)
                Console.WriteLine(i == index ? $"> {options[i]}" : $"  {options[i]}");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index = (index - 1 + options.Length) % options.Length;
            else if (key == ConsoleKey.DownArrow) index = (index + 1) % options.Length;
            else if (key == ConsoleKey.Enter)
            {
                if (index == 0)
                    Dungeon.Start(player);
                else
                {
                    player.Rest();
                    Console.WriteLine("체력과 스킬 회복 완료!");
                    Console.ReadKey(true);
                }
            }
        }
    }
}