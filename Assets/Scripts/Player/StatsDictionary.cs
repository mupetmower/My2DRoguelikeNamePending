using System;
using System.Collections.Generic;

public class StatsDictionary : Dictionary<String, float>
{
    public StatsDictionary()
    {
        Add("Vit", 0f);
        Add("Str", 0f);
        Add("Int", 0f);
        Add("Wis", 0f);
        Add("Agi", 0f);
        Add("Dex", 0f);
        Add("Luck", 0f);

        Add("MaxHP", 0f);
        Add("CurrentHP", 0f);
        Add("MaxMana", 0f);
        Add("CurrentMana", 0f);

        Add("HPRegen", 0f);
        Add("ManaRegen", 0f);

    }
}
