class Skill
{
    public string Name;
    public float Rate;
    public StatusEffect? Effect;
    public int Chance;
    public int MaxPP;
    public int PP;

    public Skill(string n, float r, StatusEffect? e, int c, int pp)
    {
        Name = n;
        Rate = r;
        Effect = e;
        Chance = c;
        MaxPP = pp;
        PP = pp;
    }
}