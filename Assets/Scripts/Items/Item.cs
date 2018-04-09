using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    [SerializeField]
    private string itemName;
    //public float BaseDamage { get; set; }


    public Item()
    {
        Name = "";
    }

    public Item(string name)
    {
        Name = name;
    }

    public string Name
    {
        get
        {
            return itemName;
        }
        set
        {
            itemName = value;
        }
    }

}
