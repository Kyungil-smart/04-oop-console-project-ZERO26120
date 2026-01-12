using System;

static class Dungeon
{
    static Random r = new Random();

    public static void Start(Player p)
    {
        for(int floor=1;floor<=5;floor++)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n=== {floor}층 ===");
            Console.ResetColor();

            Console.WriteLine("1. 주변 탐색  2. 복귀");
            ConsoleKeyInfo key = Console.ReadKey(true);
            string sel = key.KeyChar.ToString();

            if(sel=="2")
            {
                Console.WriteLine("마을로 복귀합니다...");
                Console.ReadKey();
                Town.Enter(p);
                return;
            }

            // 조우 확률
            if(r.Next(100)<70)
            {
                bool escape = BattleSystem.Start(p,CreateEnemy());
                if(escape)
                {
                    Town.Enter(p);
                    return;
                }
            }
            else
            {
                Console.WriteLine("주변에 적이 없습니다.");
                Console.ReadKey();
            }

            if(floor<5)
            {
                Console.WriteLine("1. 휴식  2. 계속");
                key = Console.ReadKey(true);
                if(key.KeyChar.ToString()=="1") p.Rest();
            }
            else
            {
                BattleSystem.Start(p,new Boss());
            }
        }
    }

    static Enemy CreateEnemy()
    {
        int n=r.Next(3);
        if(n==0) return new Enemy("슬라임",28,5,30);
        if(n==1) return new Enemy("고블린",38,8,40);
        return new Enemy("늑대",34,7,35);
    }
}