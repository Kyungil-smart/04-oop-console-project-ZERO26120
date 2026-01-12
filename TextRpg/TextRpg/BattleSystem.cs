using System;
using System.Collections.Generic;

static class BattleSystem
{
    static Random r = new Random();

    public static bool Start(Player player, Enemy enemy)
    {
        List<string> battleLog = new List<string>();
        int turn = 1;

        while (player.Hp > 0 && enemy.Hp > 0)
        {
            Console.Clear();

            // --- 체력 표시 ---
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"플레이어 HP: {player.Hp}/{player.MaxHp}");
            Console.ResetColor();
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{enemy.Name} HP: {enemy.Hp}");
            Console.ResetColor();

            // --- 전투 로그 ---
            Console.WriteLine("\n--- 전투 로그 ---");
            int maxLog = 8;
            int startIndex = battleLog.Count > maxLog ? battleLog.Count - maxLog : 0;
            if (battleLog.Count == 0) Console.WriteLine("(아직 로그 없음)");
            else
            {
                for (int i = startIndex; i < battleLog.Count; i++)
                    Console.WriteLine(battleLog[i]);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n--- TURN {turn} ---");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[플레이어 턴]");
            Console.ResetColor();

            ProcessStatus(player, battleLog);

            string[] options = { "공격", "스킬", "도망" };
            int choice = SelectFromMenu(options, "행동 선택:");

            if (choice == 2)
            {
                if (r.Next(100) < 30)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("도망 성공! 마을로 복귀...");
                    Console.ResetColor();
                    Console.ReadKey(true);
                    return true;
                }
                battleLog.Add(Dungeon.ColorText("도망 실패!", ConsoleColor.DarkYellow));
            }
            else if (choice == 0)
            {
                int dmg = player.TotalAttack();
                enemy.Hp -= dmg;
                battleLog.Add(Dungeon.ColorText("플레이어 일반 공격!", ConsoleColor.White) + " " +
                              Dungeon.ColorText($"-{dmg} 피해", ConsoleColor.Red));
            }
            else
            {
                string[] skillOptions = new string[4];
                for (int i = 0; i < 4; i++)
                    skillOptions[i] = $"{player.Skills[i].Name} ({player.Skills[i].PP}/{player.Skills[i].MaxPP})";

                int skChoice = SelectFromMenu(skillOptions, "스킬 선택:");
                Skill sk = player.Skills[skChoice];

                if (sk.PP <= 0)
                {
                    battleLog.Add(Dungeon.ColorText("PP 부족!", ConsoleColor.DarkYellow));
                }
                else
                {
                    sk.PP--;
                    int dmg = (int)(player.TotalAttack() * sk.Rate);
                    enemy.Hp -= dmg;
                    battleLog.Add(Dungeon.ColorText($"플레이어가 {sk.Name} 사용!", ConsoleColor.Magenta) + " " +
                                  Dungeon.ColorText($"-{dmg} 피해", ConsoleColor.Red));

                    if (sk.Effect != null && r.Next(100) < sk.Chance)
                    {
                        enemy.Statuses.Add(new Status(sk.Effect.Value, 3));
                        battleLog.Add(Dungeon.ColorText($"{sk.Effect} 상태이상!", ConsoleColor.Yellow));
                    }
                }
            }

            if (enemy.Hp <= 0) break;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[몬스터 턴]");
            Console.ResetColor();

            ProcessStatus(enemy, battleLog);
            MonsterAction(player, enemy, battleLog);

            turn++;
        }

        if (player.Hp <= 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n패배...");
            Console.ResetColor();
            Environment.Exit(0);
        }

        player.GainExp(enemy.Exp);
        battleLog.Add(Dungeon.ColorText($"경험치 {enemy.Exp} 획득!", ConsoleColor.Green));

        if (r.Next(100) < 30)
        {
            Equipment eq = new Equipment("강철 검", 4);
            player.Inventory.Add(eq);
            battleLog.Add(Dungeon.ColorText($"{eq.Name} 획득!", ConsoleColor.Cyan));
        }

        Console.ReadKey(true);
        return false;
    }

    public static int SelectFromMenu(string[] options, string title)
    {
        int index = 0;
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine(title);
            for (int i = 0; i < options.Length; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }
            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index--;
            if (key == ConsoleKey.DownArrow) index++;
            if (index < 0) index = options.Length - 1;
            if (index >= options.Length) index = 0;
        } while (key != ConsoleKey.Enter);

        return index;
    }

    static void MonsterAction(Player player, Enemy enemy, List<string> battleLog)
    {
        if (enemy is Boss b)
        {
            if (r.Next(100) < 40)
            {
                b.FireBreath(player);
                battleLog.Add(Dungeon.ColorText("드래곤 화염 브레스!", ConsoleColor.Red));
            }
            else
            {
                b.DarkStrike(player);
                battleLog.Add(Dungeon.ColorText("드래곤 암흑 타격!", ConsoleColor.Red));
            }
        }
        else
        {
            player.Hp -= enemy.Attack;
            battleLog.Add(Dungeon.ColorText($"{enemy.Name} 공격", ConsoleColor.Red) + " " +
                          Dungeon.ColorText($"-{enemy.Attack}", ConsoleColor.Red));
        }
    }

    static void ProcessStatus(object target, List<string> battleLog)
    {
        var list = target is Player p ? p.Statuses : ((Enemy)target).Statuses;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var s = list[i];
            int dmg = s.GetDamage();
            if (target is Player p1) p1.Hp -= dmg;
            else ((Enemy)target).Hp -= dmg;
            battleLog.Add(Dungeon.ColorText($"{s.Effect} 피해", ConsoleColor.DarkYellow) + " " +
                          Dungeon.ColorText($"-{dmg}", ConsoleColor.DarkYellow));
            s.Turns--;
            if (s.Turns <= 0) list.RemoveAt(i);
        }
    }
}
