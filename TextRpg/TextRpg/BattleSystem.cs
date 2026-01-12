using System;
using System.Collections.Generic;

static class BattleSystem
{
    static Random r = new Random();

    public static bool Start(Player p, Enemy e)
    {
        List<string> log = new List<string>();
        int turnCount = 1;

        while(p.Hp>0 && e.Hp>0)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"플레이어 HP: {p.Hp}/{p.MaxHp}");
            Console.ResetColor();
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{e.Name} HP: {e.Hp}");
            Console.ResetColor();

            Console.WriteLine();
            foreach(var l in log) Console.WriteLine(l);
            log.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n--- TURN {turnCount} ---");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[플레이어 턴]");
            Console.ResetColor();

            ProcessStatus(p,log);

            Console.WriteLine("1. 기본 공격  2. 스킬  3. 도망");
            ConsoleKeyInfo key = Console.ReadKey(true);
            string sel = key.KeyChar.ToString();

            if(sel=="3")
            {
                if(r.Next(100)<30)
                {
                    Console.ForegroundColor=ConsoleColor.Yellow;
                    Console.WriteLine("도망 성공! 마을로 복귀...");
                    Console.ResetColor();
                    Console.ReadKey();
                    Town.Enter(p);
                    return true;
                }
                log.Add("도망 실패!");
            }
            else if(sel=="1")
            {
                int dmg=p.TotalAttack();
                e.Hp-=dmg;
                log.Add(ColorText("기본 공격",ConsoleColor.White)+" "+ColorText($"-{dmg}",ConsoleColor.Red));
            }
            else if(sel=="2")
            {
                Console.WriteLine("스킬 선택:");
                for(int i=0;i<4;i++)
                    Console.WriteLine($"{i+1}. {ColorText(p.Skills[i].Name,ConsoleColor.Magenta)} ({p.Skills[i].PP}/{p.Skills[i].MaxPP})");

                key=Console.ReadKey(true);
                if(!int.TryParse(key.KeyChar.ToString(),out int s)) continue;
                s--;
                if(s<0 || s>3) continue;

                Skill sk=p.Skills[s];
                if(sk.PP<=0)
                {
                    log.Add(ColorText("PP 부족!",ConsoleColor.DarkYellow));
                    continue;
                }

                sk.PP--;
                int dmg=(int)(p.TotalAttack()*sk.Rate);
                e.Hp-=dmg;
                log.Add(ColorText($"{sk.Name}!",ConsoleColor.Magenta)+" "+ColorText($"-{dmg}",ConsoleColor.Red));

                if(sk.Effect!=null && r.Next(100)<sk.Chance)
                {
                    e.Statuses.Add(new Status(sk.Effect.Value,3));
                    log.Add(ColorText($"{sk.Effect} 상태이상!",ConsoleColor.Yellow));
                }
            }

            if(e.Hp<=0) break;

            Console.ForegroundColor=ConsoleColor.Red;
            Console.WriteLine("\n[몬스터 턴]");
            Console.ResetColor();

            ProcessStatus(e,log);
            MonsterAction(p,e,log);

            turnCount++;
        }

        if(p.Hp<=0)
        {
            Console.ForegroundColor=ConsoleColor.DarkRed;
            Console.WriteLine("\n패배...");
            Console.ResetColor();
            Environment.Exit(0);
        }

        p.GainExp(e.Exp);

        if(r.Next(100)<30)
        {
            Equipment eq=new Equipment("강철 검",4);
            p.Inventory.Add(eq);
            Console.ForegroundColor=ConsoleColor.Cyan;
            Console.WriteLine($"{eq.Name} 획득!");
            Console.ResetColor();
        }

        Console.ReadKey();
        return false;
    }

    static void MonsterAction(Player p, Enemy e, List<string> log)
    {
        if(e is Boss b)
        {
            if(r.Next(100)<40){ b.FireBreath(p); log.Add(ColorText("드래곤 화염 브레스!",ConsoleColor.Red)); }
            else{ b.DarkStrike(p); log.Add(ColorText("드래곤 피어!",ConsoleColor.Red)); }
        }
        else
        {
            p.Hp-=e.Attack;
            log.Add(ColorText($"{e.Name} 공격",ConsoleColor.Red)+" "+ColorText($"-{e.Attack}",ConsoleColor.Red));
        }
    }

    static void ProcessStatus(object target,List<string> log)
    {
        var list = target is Player p ? p.Statuses : ((Enemy)target).Statuses;

        for(int i=list.Count-1;i>=0;i--)
        {
            var s=list[i];
            int dmg=s.GetDamage();

            if(target is Player p1) p1.Hp-=dmg;
            else ((Enemy)target).Hp-=dmg;

            log.Add(ColorText($"{s.Effect} 피해",ConsoleColor.DarkYellow)+" "+ColorText($"-{dmg}",ConsoleColor.DarkYellow));

            s.Turns--;
            if(s.Turns<=0) list.RemoveAt(i);
        }
    }

    static string ColorText(string text,ConsoleColor color)
    {
        Console.ForegroundColor=color;
        string t=text;
        Console.ResetColor();
        return t;
    }
}
