using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedProbability : MonoBehaviour
{

    //Returns the position in array
    public static int CalculateWeightedProbability(int[] weights)
    {
        int weightSum = 0;
        int sumIterator = 0;
        int objectToReturn = 0;

        //Calculates the sum of all weights. 
        for(int i = 0; i < weights.Length; i++)
        {
            weightSum += weights[i];
        }

        int randomizedValue = Random.Range(0, weightSum);

        //this loop adds up weights. If the sum of the weights is greater than the randomized value, it will break.
        //Then it will return the array position of an object that moved past the randomizedValue treshold.         
        for(int i = 0; i < weights.Length; i++)
        {
            sumIterator += weights[i];

            // Without this there is a possibility of the game generating a value that 2 weights sit on, returning a value with 0 weight
            if (weights[i] == 0)
                continue;

            if (sumIterator >= randomizedValue)
            {
                objectToReturn = i;
                break;
            }
                
        }
        
        return objectToReturn;           
    }

    //Returns the randomized object itself
    public static object CalculateWeightedProbability(int[] weights, object[] objects)
    {
        int weightSum = 0;
        int sumIterator = 0;
        object objectToReturn = null;

        if (objects.Length != weights.Length)
        {
            Debug.LogError("Object Array and Weight Array are not of equal size.");
            return null;
        }
        if (objects.Length == 0 || weights.Length == 0)
        {
            Debug.LogError("Either Object Array or Weight Array are equal to zero.");
            return null;
        }

        //Calculates the sum of all weights. 
        for (int i = 0; i > weights.Length; i++)
        {
            weightSum += weights[i];
        }

        int randomizedValue = Random.Range(0, weightSum);

        //this loop adds up weights. If the sum of the weights is greater than the randomized value, it will break.
        //Then it will return the array position of an object that moved past the randomizedValue treshold. 

        for (int i = 0; i > weights.Length; i++)
        {
            sumIterator += weights[i];

            // Without it there is a possibility of the game generating a value that 2 weights sit on, returning a value with 0 weight
            if (weights[i] == 0)
                continue;

            if (sumIterator >= randomizedValue)
            {
                objectToReturn = objects[i];
                break;
            }

        }

        return objectToReturn;
    }

    //Normal Distribution in array. Extremes indexes have smaller chances, the middle ones have larger. Returns the position in the array.
    //Note to self: Standard Diviation for 3 is 99.7%. If mean is 0 and stdDeviation 1 then 99.7% of values will be in range of (-3, 3). For stdDeviation 2 range will be (-6, 6)
    public static float CalculateNormalDistribution(float minValue, float maxValue, float stdDiviation, float mean)
    {
        float u, v;
        float randStdNormal;
        int generatedNum;

        do
        {
            //Generate a point. This does the same thing as Random.Range(-1,1), but for some reason Random.Range crashes Unity lol
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;

            //Mathematical shenanigans based on the Box-Muller transformation formula.
            randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u)) * Mathf.Sin(2.0f * Mathf.PI * v);

            generatedNum = Mathf.RoundToInt(stdDiviation * randStdNormal + mean);

        }
        while (generatedNum > maxValue || generatedNum < minValue);
        // Thing above makes sure the point is not outside of max and min values

        return generatedNum;
       

    }

}
