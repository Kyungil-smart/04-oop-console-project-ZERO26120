using System.Collections.Generic;

class Enemy
{
    public string Name;
    public int Hp, MaxHp, Attack, Exp;
    public List<Status> Statuses = new List<Status>();

    public Enemy(string name, int hp, int atk, int exp)
    {
        Name = name;
        Hp = MaxHp = hp;
        Attack = atk;
        Exp = exp;
    }
}