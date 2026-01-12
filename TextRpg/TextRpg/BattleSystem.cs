using System;
using System.Collections.Generic;

static class BattleSystem
{
    static Random r = new Random();
    static int logHeight = 10;

    public static bool Start(Player player, Enemy enemy)
    {
        List<string> battleLog = new List<string>();
        int turn = 1;

        while (player.Hp > 0 && enemy.Hp > 0)
        {
            Console.Clear();
            DrawBattleStatus(player, enemy, battleLog, turn);

            // 플레이어 턴
            ProcessStatus(player, battleLog);

            string[] actions = { "공격", "스킬", "도망" };
            int actionIndex = SelectMenu(actions, 15);

            if (actionIndex == 0) PlayerAttack(player, enemy, battleLog);
            else if (actionIndex == 1) PlayerUseSkill(player, enemy, battleLog);
            else if (actionIndex == 2)
            {
                if (r.Next(100) < 30)
                {
                    Console.WriteLine("도망 성공! 마을로 복귀...");
                    Console.ReadKey(true);
                    return true;
                }
                battleLog.Add("도망 실패!");
            }

            if (enemy.Hp <= 0)
            {
                battleLog.Add($"{enemy.Name} 처치!");
                DrawBattleStatus(player, enemy, battleLog, turn);
                Console.ReadKey(true);
                break;
            }

            // 몬스터 턴
            ProcessStatus(enemy, battleLog);
            MonsterAction(player, enemy, battleLog);

            turn++;
        }

        if (player.Hp <= 0)
        {
            Console.WriteLine("\n패배...");
            Environment.Exit(0);
        }

        player.GainExp(enemy.Exp);

        return false;
    }

    static int SelectMenu(string[] options, int startRow)
    {
        int index = 0;
        while (true)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(0, startRow + i);
                Console.Write(i == index ? $"> {options[i]}" : $"  {options[i]}");
                Console.Write(new string(' ', Console.WindowWidth - options[i].Length - 2));
            }

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index = (index - 1 + options.Length) % options.Length;
            else if (key == ConsoleKey.DownArrow) index = (index + 1) % options.Length;
            else if (key == ConsoleKey.Enter) return index;
        }
    }

    static void DrawBattleStatus(Player player, Enemy enemy, List<string> log, int turn)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"플레이어 HP: {player.Hp}/{player.MaxHp}");
        Console.WriteLine($"{enemy.Name} HP: {enemy.Hp}");
        Console.WriteLine("\n--- 전투 로그 ---");

        int start = Math.Max(0, log.Count - logHeight);
        for (int i = start; i < log.Count; i++)
            Console.WriteLine(log[i]);
        for (int i = log.Count; i < logHeight; i++)
            Console.WriteLine(new string(' ', Console.WindowWidth));

        Console.WriteLine($"\n--- TURN {turn} ---");
    }

    static void PlayerAttack(Player player, Enemy enemy, List<string> log)
    {
        int dmg = player.TotalAttack();
        enemy.Hp -= dmg;
        log.Add($"플레이어 일반 공격! -{dmg} 피해");
    }

    static void PlayerUseSkill(Player player, Enemy enemy, List<string> log)
    {
        int skillIndex = 0;
        while (true)
        {
            Console.SetCursorPosition(0, 15);
            Console.WriteLine("스킬 선택:");
            for (int i = 0; i < player.Skills.Count; i++)
                Console.WriteLine(i == skillIndex ? $"> {player.Skills[i].Name} ({player.Skills[i].PP}/{player.Skills[i].MaxPP})"
                                                 : $"  {player.Skills[i].Name} ({player.Skills[i].PP}/{player.Skills[i].MaxPP})");

            var k = Console.ReadKey(true).Key;
            if (k == ConsoleKey.UpArrow) skillIndex = (skillIndex - 1 + player.Skills.Count) % player.Skills.Count;
            else if (k == ConsoleKey.DownArrow) skillIndex = (skillIndex + 1) % player.Skills.Count;
            else if (k == ConsoleKey.Enter)
            {
                Skill sk = player.Skills[skillIndex];
                if (sk.PP <= 0) log.Add("PP 부족!");
                else
                {
                    sk.PP--;
                    int dmg = (int)(player.TotalAttack() * sk.Rate);
                    enemy.Hp -= dmg;
                    log.Add($"플레이어가 {sk.Name} 사용! 적에게 {dmg} 피해");

                    if (sk.Effect != null && r.Next(100) < sk.Chance)
                    {
                        enemy.Statuses.Add(new Status(sk.Effect.Value, 3));
                        log.Add($"{enemy.Name} 상태이상! {sk.Effect}");
                    }
                }
                break;
            }
        }
    }

    static void MonsterAction(Player player, Enemy enemy, List<string> log)
    {
        if (enemy is Boss b)
        {
            if (r.Next(100) < 40)
            {
                b.FireBreath(player);
                log.Add("드래곤 화염 브레스!");
            }
            else
            {
                b.DarkStrike(player);
                log.Add("드래곤 암흑 타격!");
            }
        }
        else
        {
            player.Hp -= enemy.Attack;
            log.Add($"{enemy.Name} 공격 -{enemy.Attack}");
        }
    }

    static void ProcessStatus(object target, List<string> log)
    {
        var list = target is Player p ? p.Statuses : ((Enemy)target).Statuses;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var s = list[i];
            int dmg = s.GetDamage();
            if (target is Player p1) p1.Hp -= dmg;
            else ((Enemy)target).Hp -= dmg;
            log.Add($"{s.Effect} 피해 -{dmg}");
            s.Turns--;
            if (s.Turns <= 0) list.RemoveAt(i);
        }
    }
}
