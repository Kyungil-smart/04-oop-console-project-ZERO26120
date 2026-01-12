enum StatusEffect { Burn, Poison }

class Skill
{
    public string Name;
    public double Rate;
    public int PP, MaxPP;
    public StatusEffect? Effect;
    public int Chance;

    public Skill(string name, double rate, int pp, StatusEffect? effect = null, int chance = 0)
    {
        Name = name;
        Rate = rate;
        PP = MaxPP = pp;
        Effect = effect;
        Chance = chance;
    }
}