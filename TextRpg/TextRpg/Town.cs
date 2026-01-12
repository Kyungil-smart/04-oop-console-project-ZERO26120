using System;

static class Town
{
    public static void Enter(Player player)
    {
        while (true)
        {
            string[] menu = { "던전 입장", "휴식" };
            int choice = BattleSystem.SelectFromMenu(menu, "=== 마을 ===");

            if (choice == 0) // 던전 입장
            {
                Dungeon.Start(player);
                return;
            }
            else // 휴식
            {
                player.Rest();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("체력과 스킬 PP 회복 완료!");
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }
    }
}