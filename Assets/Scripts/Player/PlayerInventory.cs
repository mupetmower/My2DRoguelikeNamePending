using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory {

    //public static PlayerInventory instance = null;

    private Weapon weapon;
    public List<Item> HeldItems { get; set; }



    public PlayerInventory()
    {
        HeldItems = new List<Item>();
        EquippedWeapon = new Weapon("Fists", 1, 0);
    }

    public Weapon EquippedWeapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
            //GameManager.PlayerStats.CalculateBaseDamage();
        }
    }

}
