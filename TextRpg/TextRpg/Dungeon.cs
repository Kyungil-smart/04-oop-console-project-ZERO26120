using System;

static class Dungeon
{
    static Random r = new Random();

    public static void Start(Player player)
    {
        for (int floor = 1; floor <= 5; floor++)
        {
            Console.Clear();
            Console.WriteLine($"\n=== {floor}층 ===");

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
                Console.WriteLine("휴식 후 던전 계속 진행 가능");
                player.Rest();
            }
            else
            {
                Boss boss = new Boss();
                BattleSystem.Start(player, boss);
                Console.WriteLine("보스를 처치했습니다! 타이틀로 복귀...");
                Console.ReadKey(true);
                Program.ShowTitle();
            }
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