class Skill
{
    public string Name;
    public float Rate;
    public StatusEffect? Effect;
    public int Chance;
    public int MaxPP;
    public int PP;

    public Skill(string name, float rate, StatusEffect? effect, int chance, int maxPP)
    {
        Name = name;
        Rate = rate;
        Effect = effect;
        Chance = chance;
        MaxPP = maxPP;
        PP = maxPP;
    }
}