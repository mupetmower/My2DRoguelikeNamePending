using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeDamageType
{
    Slash, Stab, Blunt, 
}


public class Weapon : Item {

    //protected MeleeDamageType DamageType { get; set; }

    

    public float baseDamage;
    public int damageModifier;


    public Weapon()
    {
        Name = "";
        //BaseDamage = baseDamage;
        //DamageModifier = 0;
    }

    public Weapon(string name, float baseDmg)
    {
        Name = name;
        BaseDamage = baseDmg;
        DamageModifier = 0;
    }

    public Weapon(string name, float baseDmg, int dmgMod)
    {
        Name = name;
        BaseDamage = baseDmg;
        DamageModifier = dmgMod;
    }


    public float BaseDamage
    {
        get
        {
            return baseDamage;
        }
        set
        {
            baseDamage = value;
        }
    }

    public int DamageModifier
    {
        get
        {
            return damageModifier;
        }
        set
        {
            damageModifier = value;
        }
    }


}
