using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour {

    public Item ItemInList { get; set; }

     
    public void OnClick(Button button)
    {
        GameObject.Find("Player(Clone)").GetComponent<Player>().TakeItem(ItemInList, button);
    }
    
}
