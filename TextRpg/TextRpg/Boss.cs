class Boss : Enemy
{
    public Boss() : base("드래곤", 200, 18, 180) { }

    public void FireBreath(Player player)
    {
        player.Hp -= 22;
        player.Statuses.Add(new Status(StatusEffect.Burn, 3));
    }

    public void DarkStrike(Player player)
    {
        player.Hp -= 18;
    }
}