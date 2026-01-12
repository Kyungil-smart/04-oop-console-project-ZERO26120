using System;

static class Dungeon
{
    static Random r = new Random();

    public static void Start(Player player)
    {
        for (int floor = 1; floor <= 5; floor++)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n=== {floor}층 ===");
            Console.ResetColor();

            if (floor < 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    Enemy enemy = CreateEnemy();
                    bool ranAway = BattleSystem.Start(player, enemy);
                    if (ranAway)
                    {
                        Town.Enter(player);
                        return;
                    }
                }
                player.Rest();
            }
            else
            {
                Boss boss = new Boss();
                BattleSystem.Start(player, boss);
                Program.ShowTitle(); // 보스 격파 후 타이틀 복귀
            }
        }
    }

    static Enemy CreateEnemy()
    {
        int n = r.Next(3);
        if (n == 0) return new Enemy("슬라임", 28, 5, 30);
        if (n == 1) return new Enemy("고블린", 38, 8, 40);
        return new Enemy("늑대", 34, 7, 35);
    }

    public static string ColorText(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        string t = text;
        Console.ResetColor();
        return t;
    }
}