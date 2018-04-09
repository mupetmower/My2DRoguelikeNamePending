using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ClassType
{
    Adventurer, Mage, Rogue, Palidin, Cleric, Samurai, SpellBlade, Monk, Custom
}


public class Stats
{

    
    public int StatPointsToUse { get; set; }

    public float CurrentExp { get; set; }
    public float NeededExp { get; set; }

    private const float BASE_EXP = 100;

    public int Level { get; set; }

    private Dictionary<string, float> baseStats = new StatsDictionary();

    private const int BASE_HP = 80;
    private const int BASE_MANA = 20;

    private const float BASE_HP_REGEN_RATE = 0.2f;
    private const float BASE_MANA_REGEN_RATE = 0.075f;


    public float BaseDamage { get; set; }

    //public float WeaponDamage { get; set; }

    public float CurrentDamage { get; set; }


    //For Default Classes
    public Stats(ClassType classType)
    {
        switch (classType)
        {
            case ClassType.Adventurer:
                BaseStats["Vit"] = 5f;
                BaseStats["Str"] = 5f;
                BaseStats["Int"] = 5f;
                BaseStats["Wis"] = 5f;
                BaseStats["Agi"] = 5f;
                BaseStats["Dex"] = 5f;
                BaseStats["Luck"] = 5f;

                BaseStats["MaxHP"] = CalculateMaxHealth(BaseStats["Vit"]);
                BaseStats["MaxMana"] = CalculateMaxMana(BaseStats["Int"]);

                BaseStats["CurrentHP"] = BaseStats["MaxHP"];
                BaseStats["CurrentMana"] = BaseStats["MaxMana"];

                BaseStats["HPRegen"] = CalculateHPRegenPerTurn(BaseStats["Vit"]);
                BaseStats["ManaRegen"] = CalculateManaRegenPerTurn(BaseStats["Wis"]);

                BaseDamage = CalculateBaseDamage();
                CalculateCurrentDamage();

                break;
            case ClassType.Mage:
                BaseStats["Vit"] = 4f;
                BaseStats["Str"] = 2f;
                BaseStats["Int"] = 9f;
                BaseStats["Wis"] = 7f;
                BaseStats["Agi"] = 4f;
                BaseStats["Dex"] = 3f;
                BaseStats["Luck"] = 6f;

                BaseStats["MaxHP"] = CalculateMaxHealth(BaseStats["Vit"]);
                BaseStats["MaxMana"] = CalculateMaxMana(BaseStats["Int"]);

                BaseStats["CurrentHP"] = BaseStats["MaxHP"];
                BaseStats["CurrentMana"] = BaseStats["MaxMana"];

                BaseStats["HPRegen"] = CalculateHPRegenPerTurn(BaseStats["Vit"]);
                BaseStats["ManaRegen"] = CalculateManaRegenPerTurn(BaseStats["Wis"]);

                BaseDamage = 2f;

                break;
            case ClassType.Rogue:
                BaseStats["Vit"] = 6f;
                BaseStats["Str"] = 5f;
                BaseStats["Int"] = 4f;
                BaseStats["Wis"] = 2f;
                BaseStats["Agi"] = 8f;
                BaseStats["Dex"] = 8f;
                BaseStats["Luck"] = 2f;

                BaseStats["MaxHP"] = CalculateMaxHealth(BaseStats["Vit"]);
                BaseStats["MaxMana"] = CalculateMaxMana(BaseStats["Int"]);

                BaseStats["CurrentHP"] = BaseStats["MaxHP"];
                BaseStats["CurrentMana"] = BaseStats["MaxMana"];

                BaseStats["HPRegen"] = CalculateHPRegenPerTurn(BaseStats["Vit"]);
                BaseStats["ManaRegen"] = CalculateManaRegenPerTurn(BaseStats["Wis"]);

                BaseDamage = 5f;
                
                break;
        }


        StatPointsToUse = 0;
        CurrentExp = 0f;
        NeededExp = BASE_EXP;
        Level = 1;

    }




    //public Stats(Dictionary<string, float> stats)
    //{
    //    BaseStats = stats;

    //    StatPointsToUse = 0;

    //    CurrentExp = 0f;
    //    NeededExp = BASE_EXP;
    //}

    //For Custom Class
    public Stats(float mVit, float mStr, float mInt, float mWis, float mAgi, float mDex, float mLuck)
    {
        BaseStats["Vit"] = mVit;
        BaseStats["Str"] = mStr;
        BaseStats["Int"] = mInt;
        BaseStats["Wis"] = mWis;
        BaseStats["Agi"] = mAgi;
        BaseStats["Dex"] = mDex;
        BaseStats["Luck"] = mLuck;

        BaseStats["MaxHP"] = CalculateMaxHealth(BaseStats["Vit"]);
        BaseStats["MaxMana"] = CalculateMaxMana(BaseStats["Int"]);

        BaseStats["CurrentHP"] = BaseStats["MaxHP"];
        BaseStats["CurrentMana"] = BaseStats["MaxMana"];

        BaseStats["HPRegen"] = CalculateHPRegenPerTurn(BaseStats["Vit"]);
        BaseStats["ManaRegen"] = CalculateManaRegenPerTurn(BaseStats["Wis"]);

        StatPointsToUse = 0;

        CurrentExp = 0f;
        NeededExp = BASE_EXP;

        Level = 1;
    }


    //These two are for the start menu
    public float CalculateMaxHealth(float mVit)
    {
        return BASE_HP + (mVit * 4.0f);
    }

    public float CalculateMaxMana(float mInt)
    {
        return BASE_MANA + (mInt * 6.0f);
    }

    public float CalculateMaxHealth()
    {
        BaseStats["MaxHP"] = BASE_HP + (BaseStats["Vit"] * 4.0f);
        return BaseStats["MaxHP"];
    }

    public float CalculateMaxMana()
    {
        BaseStats["MaxMana"] = BASE_MANA + (BaseStats["Int"] * 6.0f);
        return BaseStats["MaxMana"];
    }

    public float CalculateHPRegenPerTurn(float mVit)
    {

        return BASE_HP_REGEN_RATE * mVit;
    }

    public float CalculateManaRegenPerTurn(float mWis)
    {

        return BASE_MANA_REGEN_RATE * mWis;
    }


    public float CalcDmgWithWep(float wepBaseDmg)
    {
        return BaseDamage + wepBaseDmg;
    }

    public float CalculateBaseDamage()
    {
        return 2f * (BaseStats["Str"] * 0.5f);
    }

    public float CalculateCurrentDamage()
    {
        CurrentDamage = CalculateBaseDamage() + StatsAndItems.PlayerInventory.EquippedWeapon.BaseDamage;
        return CurrentDamage;
    }

    public float AddExp(float exp)
    {
        CurrentExp += exp;

        CheckForLevelUp();

        GameUI.instance.UpdateUIWithCurrentValues();

        return CurrentExp;
    }

    private void CheckForLevelUp()
    {
        if (CurrentExp >= NeededExp)
        {
            AddLevel();
        }
    }

    private void AddLevel()
    {
        Level += 1;
        NeededExp = CalculateNewNeededExp();

        StatPointsToUse += 1;

        GameUI.instance.UpdateUIWithCurrentValues();
    }

    private float CalculateNewNeededExp()
    {
        float newExp; ;

        newExp = BASE_EXP * (Level + (float)Global.MathCombination(Level, 2));
                
        return newExp;        
    }

    public Dictionary<string, float> BaseStats
    {
        get
        {
            return baseStats;
        } set
        {
            baseStats = value;
        }
    }

}
