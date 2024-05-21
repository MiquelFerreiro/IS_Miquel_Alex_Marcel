using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Utils 
{
    public const float TWO_PI = Mathf.PI * 2f;

    public static float GetNumberNormal()
    {
        //https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform

        float u1 = UnityEngine.Random.value;
        float u2 = UnityEngine.Random.value; 

        float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(TWO_PI * u2);

        return randStdNormal; 

    }


}
