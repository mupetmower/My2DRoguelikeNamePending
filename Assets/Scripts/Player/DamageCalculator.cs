using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator {

	
    public DamageCalculator()
    {

    }


    public float CalculateBaseDamage(float playerBase, float wepBase)
    {
        float total;
        total = playerBase + wepBase;
        
        return total;
    }



}
