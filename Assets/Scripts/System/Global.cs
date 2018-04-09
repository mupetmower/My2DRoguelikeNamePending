using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global {

    public static int turnCount;




    public static float MathCombination(int n, int k)
    {
        float result = 1.0f;

        for (int i = 0; i < k; i++)
        {
            result *= (float)((n - i) / (i + 1.0));
        }

        return result;
    }
	
}
