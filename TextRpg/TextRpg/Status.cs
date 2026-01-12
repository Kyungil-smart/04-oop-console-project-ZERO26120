enum StatusEffect { Burn, Poison }

class Status
{
    public StatusEffect Effect;
    public int Turns;

    public Status(StatusEffect e, int t)
    {
        Effect = e;
        Turns = t;
    }

    public int GetDamage()
    {
        return Effect == StatusEffect.Burn ? 6 : 4;
    }
}