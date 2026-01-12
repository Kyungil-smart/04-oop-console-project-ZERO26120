using System;

static class Dungeon
{
    static Random r = new Random();

    public static void Start(Player p)
    {
        for (int floor = 1; floor <= 5; floor++)
        {
            Console.WriteLine($"\n=== {floor}층 ===");

            if (floor < 5)
            {
                for (int i = 0; i < 4; i++)
                    BattleSystem.Start(p, CreateEnemy());

                Console.WriteLine("1. 휴식  2. 계속");
                if (Console.ReadLine() == "1") p.Rest();
            }
            else
            {
                BattleSystem.Start(p, new Boss());
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
}