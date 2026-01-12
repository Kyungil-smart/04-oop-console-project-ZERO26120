using System.Collections.Generic;

class Player
{
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

        if(job == PlayerJob.Warrior)
        {
            Skills[0] = new Skill("베기",1.2f,null,0,15);
            Skills[1] = new Skill("강타",1.5f,null,0,10);
            Skills[2] = new Skill("독 찌르기",1.0f,StatusEffect.Poison,40,8);
            Skills[3] = new Skill("분노의 일격",1.8f,null,0,5);
        }
        else
        {
            Skills[0] = new Skill("화염구",1.3f,StatusEffect.Burn,40,12);
            Skills[1] = new Skill("독 구름",1.0f,StatusEffect.Poison,45,10);
            Skills[2] = new Skill("번개",1.6f,null,0,8);
            Skills[3] = new Skill("마력 폭발",2.0f,null,0,5);
        }
    }

    public int TotalAttack()
    {
        return Attack + (Weapon != null ? Weapon.Attack : 0);
    }

    public void GainExp(int e)
    {
        Exp += e;
        if(Exp >= 100)
        {
            Exp -= 100;
            Level++;
            MaxHp += 12;
            Attack += 3;
            Hp = MaxHp;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("레벨업! HP, 공격력 증가");
            Console.ResetColor();
        }
    }

    public void Rest()
    {
        Hp = MaxHp;
        foreach(var s in Skills) s.PP = s.MaxPP;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("휴식 완료! HP와 PP 회복");
        Console.ResetColor();
    }

    public void AddStatus(Status s)
    {
        foreach(var st in Statuses)
            if(st.Effect == s.Effect) return;

        Statuses.Add(s);
    }

    public void ShowInventory()
    {
        if(Inventory.Count==0)
        {
            Console.WriteLine("인벤토리가 비어있습니다.");
            return;
        }

        for(int i=0;i<Inventory.Count;i++)
            Console.WriteLine($"{i+1}. {Inventory[i].Name} (+{Inventory[i].Attack})");

        Console.Write("장착 번호(0 취소): ");
        if(!int.TryParse(Console.ReadLine(),out int s)) return;
        if(s<=0 || s>Inventory.Count) return;

        Weapon = Inventory[s-1];
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{Weapon.Name} 장착 완료");
        Console.ResetColor();
    }
}
