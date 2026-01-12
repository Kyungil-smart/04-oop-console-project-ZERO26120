using System.Collections.Generic;

class Player
{
    public int Level = 1;
    public int MaxHp = 110;
    public int Hp = 110;
    public int Attack = 11;
    public int Exp = 0;

    public Equipment Weapon;
    public List<Status> Statuses = new List<Status>();
    public Skill[] Skills = new Skill[4];

    public Player()
    {
        Skills[0] = new Skill("베기", 1.2f, null, 0, 15);
        Skills[1] = new Skill("독 찌르기", 1.0f, StatusEffect.Poison, 40, 10);
        Skills[2] = new Skill("화염 타격", 1.3f, StatusEffect.Burn, 35, 10);
        Skills[3] = new Skill("강타", 1.6f, null, 0, 5);
    }

    public int TotalAttack()
    {
        return Attack + (Weapon != null ? Weapon.Attack : 0);
    }

    public void GainExp(int e)
    {
        Exp += e;
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
        foreach (var s in Skills)
            s.PP = s.MaxPP;
    }
}