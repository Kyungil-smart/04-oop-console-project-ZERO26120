enum StatusEffect { Burn, Poison }

class Status
{
    public StatusEffect Effect;
    public int Turns;

    public Status(StatusEffect effect, int turns)
    {
        Effect = effect;
        Turns = turns;
    }

    public int GetDamage()
    {
        switch (Effect)
        {
            case StatusEffect.Burn: return 5;
            case StatusEffect.Poison: return 4;
            default: return 0;
        }
    }
}