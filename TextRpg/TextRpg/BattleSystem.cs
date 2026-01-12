using System;
using System.Collections.Generic;

static class BattleSystem
{
    static Random r = new Random();

    public static void Start(Player p, Enemy e)
    {
        Console.WriteLine($"{e.Name} 등장!");

        while (p.Hp > 0 && e.Hp > 0)
        {
            ProcessStatus(p);
            PlayerTurn(p, e);

            if (e.Hp <= 0) break;

            ProcessStatus(e);

            if (e is Boss b)
            {
                if (r.Next(100) < 40) b.FireBreath(p);
                else b.DarkStrike(p);
            }
            else
            {
                p.Hp -= e.Attack;
            }
        }

        if (p.Hp <= 0)
        {
            Console.WriteLine("패배...");
            Environment.Exit(0);
        }

        if (e is Boss)
        {
            Console.WriteLine("=== GAME CLEAR ===");
            Environment.Exit(0);
        }

        p.GainExp(e.Exp);

        if (r.Next(100) < 30)
            p.Weapon = new Equipment("강철 검", 4);
    }

    static void PlayerTurn(Player p, Enemy e)
    {
        Console.WriteLine("1. 공격  2. 스킬");
        string sel = Console.ReadLine();

        if (sel == "1")
        {
            e.Hp -= p.TotalAttack();
        }
        else
        {
            for (int i = 0; i < 4; i++)
                Console.WriteLine($"{i + 1}. {p.Skills[i].Name} ({p.Skills[i].PP})");

            int s = int.Parse(Console.ReadLine()) - 1;
            Skill sk = p.Skills[s];
            if (sk.PP <= 0) return;

            sk.PP--;
            int dmg = (int)(p.TotalAttack() * sk.Rate);
            e.Hp -= dmg;

            if (sk.Effect != null && r.Next(100) < sk.Chance)
                e.Statuses.Add(new Status(sk.Effect.Value, 3));
        }
    }

    static void ProcessStatus(object target)
    {
        List<Status> list = target is Player p ? p.Statuses : ((Enemy)target).Statuses;

        for (int i = list.Count - 1; i >= 0; i--)
        {
            int dmg = list[i].Effect == StatusEffect.Burn ? 5 : 4;

            if (target is Player p1) p1.Hp -= dmg;
            else ((Enemy)target).Hp -= dmg;

            list[i].Turns--;
            if (list[i].Turns <= 0) list.RemoveAt(i);
        }
    }
}
