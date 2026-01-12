using System;
using System.Collections.Generic;

static class BattleSystem
{
    static Random r = new Random();
    static int turnCount;

    public static void Start(Player p, Enemy e)
    {
        turnCount = 1;

        Console.Clear();
        SetColor(ConsoleColor.Yellow);
        Console.WriteLine($"=== {e.Name} 전투 시작 ===");
        ResetColor();

        while (p.Hp > 0 && e.Hp > 0)
        {
            Console.WriteLine($"\n--- TURN {turnCount} ---");

            ShowStatus(p, e);

            // 플레이어 턴
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("[플레이어 턴]");
            ResetColor();

            ProcessStatus(p, true);
            PlayerTurn(p, e);

            if (e.Hp <= 0) break;

            // 몬스터 턴
            SetColor(ConsoleColor.Red);
            Console.WriteLine("\n[몬스터 턴]");
            ResetColor();

            ProcessStatus(e, false);

            MonsterAction(p, e);

            turnCount++;
        }

        BattleResult(p, e);
    }

    static void ShowStatus(Player p, Enemy e)
    {
        SetColor(ConsoleColor.Green);
        Console.Write($"플레이어 HP: {p.Hp}/{p.MaxHp}");
        ResetColor();

        Console.Write(" | ");

        SetColor(ConsoleColor.DarkRed);
        Console.WriteLine($"{e.Name} HP: {e.Hp}");
        ResetColor();
    }

    static void PlayerTurn(Player p, Enemy e)
    {
        Console.WriteLine("1. 공격  2. 스킬");
        string sel = Console.ReadLine();

        if (sel == "1")
        {
            int dmg = p.TotalAttack();
            e.Hp -= dmg;

            SetColor(ConsoleColor.White);
            Console.Write("기본 공격! ");
            SetColor(ConsoleColor.Red);
            Console.WriteLine($"-{dmg}");
            ResetColor();
        }
        else
        {
            for (int i = 0; i < 4; i++)
                Console.WriteLine($"{i + 1}. {p.Skills[i].Name} ({p.Skills[i].PP}/{p.Skills[i].MaxPP})");

            int s = int.Parse(Console.ReadLine()) - 1;
            Skill sk = p.Skills[s];

            if (sk.PP <= 0)
            {
                SetColor(ConsoleColor.DarkYellow);
                Console.WriteLine("PP 부족!");
                ResetColor();
                return;
            }

            sk.PP--;

            int dmg = (int)(p.TotalAttack() * sk.Rate);
            e.Hp -= dmg;

            SetColor(ConsoleColor.Magenta);
            Console.Write($"{sk.Name}! ");
            SetColor(ConsoleColor.Red);
            Console.WriteLine($"-{dmg}");
            ResetColor();

            if (sk.Effect != null && r.Next(100) < sk.Chance)
            {
                SetColor(ConsoleColor.DarkYellow);
                Console.WriteLine($"{sk.Effect} 상태이상!");
                ResetColor();

                e.Statuses.Add(new Status(sk.Effect.Value, 3));
            }
        }
    }

    static void MonsterAction(Player p, Enemy e)
    {
        if (e is Boss b)
        {
            if (r.Next(100) < 40)
            {
                b.FireBreath(p);
                SetColor(ConsoleColor.DarkRed);
                Console.WriteLine("드래곤의 화염 브레스!");
                ResetColor();
            }
            else
            {
                b.DarkStrike(p);
                SetColor(ConsoleColor.DarkRed);
                Console.WriteLine("드래곤 피어!");
                ResetColor();
            }
        }
        else
        {
            p.Hp -= e.Attack;

            SetColor(ConsoleColor.Red);
            Console.WriteLine($"{e.Name} 공격! -{e.Attack}");
            ResetColor();
        }
    }

    static void ProcessStatus(object target, bool isPlayer)
    {
        List<Status> list = target is Player p ? p.Statuses : ((Enemy)target).Statuses;

        for (int i = list.Count - 1; i >= 0; i--)
        {
            Status s = list[i];
            int dmg = s.Effect == StatusEffect.Burn ? 5 : 4;

            if (target is Player p1) p1.Hp -= dmg;
            else ((Enemy)target).Hp -= dmg;

            SetColor(ConsoleColor.DarkYellow);
            Console.WriteLine($"{s.Effect} 피해 -{dmg}");
            ResetColor();

            s.Turns--;
            if (s.Turns <= 0)
            {
                list.RemoveAt(i);
                SetColor(ConsoleColor.Gray);
                Console.WriteLine($"{s.Effect} 해제");
                ResetColor();
            }
        }
    }

    static void BattleResult(Player p, Enemy e)
    {
        if (p.Hp <= 0)
        {
            SetColor(ConsoleColor.DarkRed);
            Console.WriteLine("\n패배...");
            ResetColor();
            Environment.Exit(0);
        }

        if (e is Boss)
        {
            SetColor(ConsoleColor.Yellow);
            Console.WriteLine("\n드레곤 격파!");
            Console.WriteLine("=== GAME CLEAR ===");
            ResetColor();
            Environment.Exit(0);
        }

        SetColor(ConsoleColor.Green);
        Console.WriteLine("\n승리!");
        ResetColor();

        p.GainExp(e.Exp);

        if (r.Next(100) < 30)
        {
            p.Weapon = new Equipment("강철 검", 4);
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("강철 검 획득!");
            ResetColor();
        }
    }

    static void SetColor(ConsoleColor c)
    {
        Console.ForegroundColor = c;
    }

    static void ResetColor()
    {
        Console.ResetColor();
    }
}
