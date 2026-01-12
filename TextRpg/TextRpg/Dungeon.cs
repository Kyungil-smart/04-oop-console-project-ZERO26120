using System;

static class Dungeon
{
    static Random r = new Random();

    public static void Start(Player player)
    {
        for (int floor = 1; floor <= 5; floor++)
        {
            int fightsThisFloor = (floor < 5) ? 4 : 1;

            for (int fight = 1; fight <= fightsThisFloor; fight++)
            {
                Enemy enemy = (floor < 5) ? CreateEnemy() : new Boss();

                bool ranAway = BattleSystem.Start(player, enemy, floor, fight, fightsThisFloor);
                if (ranAway)
                {
                    Town.Enter(player);
                    return;
                }
            }

            if (floor < 5)
            {
                int choice = FloorClearMenu(floor);
                if (choice == 0)
                {
                    player.Rest();
                    Console.Clear();
                    Console.WriteLine($"=== {floor}층 클리어 ===\n체력과 스킬 회복 완료!");
                    Console.ReadKey(true);
                }
                else
                {
                    Town.Enter(player);
                    return;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("보스를 처치했습니다! 타이틀로 복귀...");
                Console.ReadKey(true);
                Program.ShowTitle();
                return;
            }
        }
    }

    static int FloorClearMenu(int floor)
    {
        string[] options = { "휴식", "마을로 복귀" };
        int index = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== {floor}층 클리어 ===\n");
            Console.WriteLine("다음 행동을 선택하세요.\n");

            for (int i = 0; i < options.Length; i++)
                Console.WriteLine(i == index ? $"> {options[i]}" : $"  {options[i]}");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index = (index - 1 + options.Length) % options.Length;
            else if (key == ConsoleKey.DownArrow) index = (index + 1) % options.Length;
            else if (key == ConsoleKey.Enter) return index;
        }
    }

    static Enemy CreateEnemy()
    {
        int n = r.Next(3);
        return n switch
        {
            0 => new Enemy("슬라임", 28, 5, 30),
            1 => new Enemy("고블린", 38, 8, 40),
            _ => new Enemy("늑대", 34, 7, 35)
        };
    }
}
