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
        return Effect == StatusEffect.Burn ? 5 : 4;
    }
}