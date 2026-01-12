class Status
{
    public StatusEffect Effect;
    public int Turns;

    public Status(StatusEffect eff, int t)
    {
        Effect = eff;
        Turns = t;
    }

    public int GetDamage()
    {
        return Effect switch
        {
            StatusEffect.Burn => 5,
            StatusEffect.Poison => 4,
            _ => 0
        };
    }
}