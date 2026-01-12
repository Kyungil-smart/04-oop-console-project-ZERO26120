using System;
using System.Collections.Generic;

static class BattleSystem
{
    static Random r = new Random();

    public static bool Start(Player player, Enemy enemy)
    {
        List<string> log = new List<string>();
        int currentTurn = 1;

        while (player.Hp > 0 && enemy.Hp > 0)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"플레이어 HP: {player.Hp}/{player.MaxHp}");
            Console.ResetColor();
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{enemy.Name} HP: {enemy.Hp}");
            Console.ResetColor();

            Console.WriteLine("\n--- 전투 로그 ---");
            foreach (var l in log)
                Console.WriteLine(l);
            log.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n--- TURN {currentTurn} ---");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[플레이어 턴]");
            Console.ResetColor();

            ProcessStatus(player, log);

            string[] actions = { "공격", "스킬", "도망" };
            int action = MenuSelect(actions);

            if (action == 2)
            {
                if (r.Next(100) < 30)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("도망 성공! 마을로 복귀...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return true;
                }
                log.Add("도망 실패!");
            }
            else if (action == 0)
            {
                int dmg = player.TotalAttack();
                enemy.Hp -= dmg;
                log.Add(Dungeon.ColorText("플레이어 일반 공격!", ConsoleColor.White) + " " +
                        Dungeon.ColorText($"-{dmg}", ConsoleColor.Red));
            }
            else if (action == 1)
            {
                string[] skillNames = new string[4];
                for (int i = 0; i < 4; i++)
                    skillNames[i] = $"{player.Skills[i].Name} ({player.Skills[i].PP}/{player.Skills[i].MaxPP})";
                int sk = MenuSelect(skillNames);

                Skill skill = player.Skills[sk];
                if (skill.PP <= 0)
                {
                    log.Add(Dungeon.ColorText("PP 부족!", ConsoleColor.DarkYellow));
                }
                else
                {
                    skill.PP--;
                    int dmg = (int)(player.TotalAttack() * skill.Rate);
                    enemy.Hp -= dmg;
                    log.Add(Dungeon.ColorText($"플레이어 {skill.Name} 사용!", ConsoleColor.Magenta) + " " +
                            Dungeon.ColorText($"-{dmg} 피해", ConsoleColor.Red));

                    if (skill.Effect != null && r.Next(100) < skill.Chance)
                    {
                        enemy.Statuses.Add(new Status(skill.Effect.Value, 3));
                        log.Add(Dungeon.ColorText($"적 {skill.Effect} 상태이상!", ConsoleColor.Yellow));
                    }
                }
            }

            if (enemy.Hp <= 0) break;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[몬스터 턴]");
            Console.ResetColor();

            ProcessStatus(enemy, log);
            MonsterAction(player, enemy, log);

            currentTurn++;
        }

        if (player.Hp <= 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n패배...");
            Console.ResetColor();
            Environment.Exit(0);
        }

        player.GainExp(enemy.Exp);

        if (r.Next(100) < 30)
        {
            Equipment eq = new Equipment("강철 검", 4);
            player.Inventory.Add(eq);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{eq.Name} 획득!");
            Console.ResetColor();
        }

        Console.ReadKey();
        return false;
    }

    static void MonsterAction(Player player, Enemy enemy, List<string> log)
    {
        if (enemy is Boss b)
        {
            if (r.Next(100) < 40)
            {
                b.FireBreath(player);
                log.Add(Dungeon.ColorText("드래곤 화염 브레스!", ConsoleColor.Red));
            }
            else
            {
                b.DarkStrike(player);
                log.Add(Dungeon.ColorText("드래곤 암흑 타격!", ConsoleColor.Red));
            }
        }
        else
        {
            player.Hp -= enemy.Attack;
            log.Add(Dungeon.ColorText($"{enemy.Name} 공격", ConsoleColor.Red) + " " +
                    Dungeon.ColorText($"-{enemy.Attack}", ConsoleColor.Red));
        }
    }

    static void ProcessStatus(object target, List<string> log)
    {
        var list = target is Player p ? p.Statuses : ((Enemy)target).Statuses;

        for (int i = list.Count - 1; i >= 0; i--)
        {
            var s = list[i];
            int dmg = s.GetDamage();

            if (target is Player player) player.Hp -= dmg;
            else ((Enemy)target).Hp -= dmg;

            log.Add(Dungeon.ColorText($"{s.Effect} 피해", ConsoleColor.DarkYellow) + " " +
                    Dungeon.ColorText($"-{dmg}", ConsoleColor.DarkYellow));

            s.Turns--;
            if (s.Turns <= 0)
                list.RemoveAt(i);
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
                {
                    Console.WriteLine("  " + options[i]);
                }
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
