using System;

static class Town
{
    public static void Enter(Player p)
    {
        while(true)
        {
            Console.Clear();
            Console.ForegroundColor=ConsoleColor.Yellow;
            Console.WriteLine("=== 마을 ===");
            Console.ResetColor();

            Console.WriteLine("1. 던전 입장  2. 장비 관리  3. 휴식  4. 게임 종료");

            ConsoleKeyInfo key = Console.ReadKey(true);
            string sel = key.KeyChar.ToString();

            if(sel=="1"){ Dungeon.Start(p); break; }
            else if(sel=="2"){ p.ShowInventory(); Console.ReadKey(); }
            else if(sel=="3"){ p.Rest(); Console.ReadKey(); }
            else if(sel=="4"){ Environment.Exit(0); }
        }
    }
}