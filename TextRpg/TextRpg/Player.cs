using System.Collections.Generic;

class Player
{
    public string Name;
    public PlayerJob Job;
    public int Level = 1;
    public int MaxHp = 110;
    public int Hp = 110;
    public int Attack = 11;
    public int Exp = 0;

    public Equipment Weapon;
    public List<Equipment> Inventory = new List<Equipment>();
    public List<Status> Statuses = new List<Status>();
    public Skill[] Skills = new Skill[4];

    public Player(PlayerJob job)
    {
        Job = job;
        if (job == PlayerJob.Warrior)
        {
            Skills[0] = new Skill("베기", 1.2f, null, 0, 15);
            Skills[1] = new Skill("강타", 1.6f, null, 0, 5);
            Skills[2] = new Skill("화염 타격", 1.3f, StatusEffect.Burn, 35, 10);
            Skills[3] = new Skill("독 찌르기", 1.0f, StatusEffect.Poison, 40, 10);
        }
        else
        {
            Skills[0] = new Skill("마법 미사일", 1.2f, null, 0, 15);
            Skills[1] = new Skill("파이어볼", 1.5f, StatusEffect.Burn, 30, 10);
            Skills[2] = new Skill("아이스 샷", 1.3f, null, 0, 10);
            Skills[3] = new Skill("독 마법", 1.0f, StatusEffect.Poison, 40, 10);
        }
    }

    public int TotalAttack()
    {
        return Attack + (Weapon != null ? Weapon.Attack : 0);
    }

    public void GainExp(int exp)
    {
        Exp += exp;
        if (Exp >= 100)
        {
            Exp -= 100;
            Level++;
            MaxHp += 12;
            Attack += 3;
            Hp = MaxHp;
        }
    }

    public void Rest()
    {
        Hp = MaxHp;
        foreach (var sk in Skills)
            sk.PP = sk.MaxPP;
    }
}