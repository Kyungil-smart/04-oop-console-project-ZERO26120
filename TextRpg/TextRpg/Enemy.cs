using System.Collections.Generic;

class Enemy
{
    public string Name;
    public int Hp;
    public int Attack;
    public int Exp;
    public List<Status> Statuses = new List<Status>();

    public Enemy(string n,int h,int a,int e)
    {
        Name=n;
        Hp=h;
        Attack=a;
        Exp=e;
    }
}