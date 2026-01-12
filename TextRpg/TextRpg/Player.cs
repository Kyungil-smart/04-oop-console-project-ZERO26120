using System;
using System.Collections.Generic;

class Player
{
    public string Name { get; private set; }
    public PlayerJob Job { get; private set; }
    public int Hp, MaxHp;
    public int Attack;
    public int Exp;
    public int Level;
    public List<Skill> Skills = new List<Skill>();
    public List<Status> Statuses = new List<Status>();

    public Player(PlayerJob job)
    {
        Job = job;
        Level = 1;
        Exp = 0;

        if (job == PlayerJob.Warrior)
        {
            Name = "전사";
            MaxHp = Hp = 110;
            Attack = 10;
            Skills.Add(new Skill("베기", 1.2, 15));
            Skills.Add(new Skill("강타", 1.5, 5));
            Skills.Add(new Skill("화염 타격", 1.8, 10, StatusEffect.Burn, 30));
            Skills.Add(new Skill("독 찌르기", 1.4, 10, StatusEffect.Poison, 40));
        }
        else
        {
            Name = "마법사";
            MaxHp = Hp = 80;
            Attack = 12;
            Skills.Add(new Skill("파이어볼", 1.5, 15, StatusEffect.Burn, 40));
            Skills.Add(new Skill("매직 미사일", 1.3, 10));
            Skills.Add(new Skill("독 마법", 1.2, 10, StatusEffect.Poison, 50));
            Skills.Add(new Skill("마법 폭발", 1.8, 5));
        }
    }

    public int TotalAttack()
    {
        return Attack;
    }

    public void GainExp(int amount)
    {
        Exp += amount;
        if (Exp >= 100)
        {
            Level++;
            Exp -= 100;
            MaxHp += 10;
            Hp = MaxHp;
            Attack += 2;
            Console.WriteLine($"{Name} 레벨업! 현재 Lv.{Level}");
        }
    }

    public void Rest()
    {
        Hp = MaxHp;
        foreach (var sk in Skills)
            sk.PP = sk.MaxPP;
    }
}