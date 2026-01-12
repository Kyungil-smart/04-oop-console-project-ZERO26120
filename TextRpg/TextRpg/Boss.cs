class Boss : Enemy
{
    public Boss() : base("드래곤", 200, 18, 180) { }

    public void FireBreath(Player p)
    {
        p.Hp -= 22;
        p.Statuses.Add(new Status(StatusEffect.Burn, 3));
    }

    public void DarkStrike(Player p)
    {
        p.Hp -= 18;
    }
}