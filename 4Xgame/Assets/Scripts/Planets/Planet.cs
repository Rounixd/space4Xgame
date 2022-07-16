using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{

    protected int[] radiatonWeights;

    public int pSize { get; protected set; }
    public StarSystem starType { get; protected set; }

    public bool isColonized = false;
    public List<Pop> listOfPops = new List<Pop>();

    protected const int MIN_SIZE = 8;
    protected const int MAX_SIZE = 22;

    public Planet()
    {
        StarSystem.planetGenerated += GeneratePlanetarySurface;

        growthSpeed = 4;
        popGrowthNeeded = 10;
    }

    public virtual void GeneratePlanetarySurface(int[] radWeights) { }

    protected int GeneratePlanetSize()
    {
        return Mathf.RoundToInt(WeightedProbability.CalculateNormalDistribution(MIN_SIZE, MAX_SIZE, 3f, 12f));
    }

#region GenerateRadiation

    public enum planetRadiation
    {
        RADIATION_LOW, RADIATION_MEDIUM, RADIATION_HIGH, RADIATION_EXTREME
    }

    public planetRadiation pRadiation { get; protected set; }

    //Radiation is based off on star the planet orbits. 
    //radWeights are passed all the way from Starsystem children, so there is a potential are for code imporvement.
    protected planetRadiation GenerateRadiation(int[] radWeights)
    {
        planetRadiation pRad = planetRadiation.RADIATION_MEDIUM;

        switch (WeightedProbability.CalculateWeightedProbability(radWeights))
        {
            case (int)planetRadiation.RADIATION_LOW:
                pRad = planetRadiation.RADIATION_LOW;
                break;

            case (int)planetRadiation.RADIATION_MEDIUM:
                pRad = planetRadiation.RADIATION_MEDIUM;
                break;

            case (int)planetRadiation.RADIATION_HIGH:
                pRad = planetRadiation.RADIATION_HIGH;
                break;
            case (int)planetRadiation.RADIATION_EXTREME:
                pRad = planetRadiation.RADIATION_EXTREME;
                break;
        }
        return pRad;
    }
#endregion GenerateRadiation

#region GravityGeneration

    public enum planetGravity
    {
        GRAVITY_SMALL, GRAVITY_MEDIUM, GRAVITY_HIGH
    }

    readonly int[] gravityWeightsSmall = new int[]
    {50, 20, 5 };

    readonly int[] gravityWeightsMedium = new int[]
    {15, 50, 15};

    readonly int[] gravityWeightsLarge = new int[]
    {5, 20, 50};

    public planetGravity pGravity { get; protected set; }

    // Gravity is based off on planet size. 
    // It assigns different weights for bottom, middle, and top 1/3 of the planet range.
    protected planetGravity GenerateGravity(int _planetSize)
    {
        planetGravity pGravity = planetGravity.GRAVITY_SMALL;
        int planetSize = _planetSize;
        int planetRangeSize = MAX_SIZE - MIN_SIZE;
        int dividedRange = planetRangeSize / 3;

        if (planetSize >= MIN_SIZE && planetSize < MIN_SIZE + dividedRange)
            pGravity = WeightedGravity(gravityWeightsSmall);

        else if (planetSize >= MIN_SIZE + dividedRange && planetSize < MIN_SIZE + dividedRange * 2)
            pGravity = WeightedGravity(gravityWeightsMedium);

        else if (planetSize >= MIN_SIZE + dividedRange * 2 && planetSize <= MAX_SIZE)
            pGravity = WeightedGravity(gravityWeightsLarge);

        return pGravity;
        
    }

    planetGravity WeightedGravity(int[] weights)
    {
        planetGravity g = planetGravity.GRAVITY_SMALL;

        switch (WeightedProbability.CalculateWeightedProbability(weights))
        {

            case (int)planetGravity.GRAVITY_SMALL:
                g = planetGravity.GRAVITY_SMALL;
                break;

            case (int)planetGravity.GRAVITY_MEDIUM:
                g = planetGravity.GRAVITY_MEDIUM;
                break;

            case (int)planetGravity.GRAVITY_HIGH:
                g = planetGravity.GRAVITY_HIGH;
                break;
        }

        return g;
    }
#endregion GravityGeneration

#region GenerateResources

    public int numMinerals { get; protected set; } //0
    public int numFood { get; protected set; } //1
    public int numEnergy { get; protected set; } //2

    public enum planetResources
    {
        MINERALS, ENERGY, FOOD
    }

    //Generate resources. 
    protected void GenerateResources(int[] rWeights)
    {
        //calculate the total number of resources.
        for (int i = 0; i <= pSize - 1; i++)
        {
            switch (WeightedProbability.CalculateWeightedProbability(rWeights))
            {
                case (int)planetResources.MINERALS:
                    numMinerals++;
                    break;

                case (int)planetResources.ENERGY:
                    numEnergy++;
                    break;

                case (int)planetResources.FOOD:
                    numFood++;
                    break;
            }
        }

        //adjust num of resources based off on gravity, habitability, radiation, etc.
        if (pGravity == planetGravity.GRAVITY_SMALL)
            numMinerals = Mathf.RoundToInt(numMinerals * 0.5f);
        else if (pGravity == planetGravity.GRAVITY_HIGH)
            numMinerals = Mathf.RoundToInt(numMinerals * 1.5f);

        if (pRadiation == planetRadiation.RADIATION_LOW)
            numEnergy = Mathf.RoundToInt(numEnergy * 0.5f);
        else if (pRadiation == planetRadiation.RADIATION_HIGH)
            numEnergy = Mathf.RoundToInt(numEnergy * 1.5f);
        else if (pRadiation == planetRadiation.RADIATION_EXTREME)
            numEnergy = numEnergy * 2;

        if (pHabitability == planetHabitability.HABITABILITY_ZERO) {
            numFood = 0;
            numEnergy = Mathf.RoundToInt(numEnergy * 1.25f);
            numMinerals = Mathf.RoundToInt(numMinerals * 1.25f);
        }
        else if (pHabitability == planetHabitability.HABITABILITY_LOW)
            numFood = Mathf.RoundToInt(numFood * 0.5f);
        else if (pHabitability == planetHabitability.HABITABILITY_HIGH)
            numFood = Mathf.RoundToInt(numFood * 1.5f);
        
    }


    #endregion GenerateResources

#region GenerateHabitability

    public planetHabitability pHabitability { get; protected set; }

    public enum planetHabitability {
       HABITABILITY_ZERO, HABITABILITY_LOW, HABITABILITY_MEDIUM, HABITABILITY_HIGH
    }

    int[] habWeightsRadLow = new int[] 
    {0, 20,30,50};

    int[] habWeightsRadMedium = new int[] 
    {0, 30, 50, 20};

    int[] habWeightsRadHigh = new int[] 
    {0, 50, 30, 20};

    protected planetHabitability GenerateHabitability()
    {
        planetHabitability tempHabitability = planetHabitability.HABITABILITY_ZERO;
        
        if (starType is BlackHole || this is BarrenPlanet || pRadiation == planetRadiation.RADIATION_EXTREME)
            tempHabitability = planetHabitability.HABITABILITY_ZERO;

        else { 
            switch(pRadiation)
            {
                case planetRadiation.RADIATION_LOW:
                    tempHabitability = (planetHabitability)WeightedProbability.CalculateWeightedProbability(habWeightsRadLow);
                    break;
                case planetRadiation.RADIATION_MEDIUM:
                    tempHabitability = (planetHabitability)WeightedProbability.CalculateWeightedProbability(habWeightsRadMedium);
                    break;
                case planetRadiation.RADIATION_HIGH:
                    tempHabitability = (planetHabitability)WeightedProbability.CalculateWeightedProbability(habWeightsRadHigh);
                    break;
            }
        
        }

        return tempHabitability;
        
    }


    #endregion GenerateHabitability

    #region PopManagement

    public int growthSpeed { get; protected set; }
    public int popGrowthNeeded { get; protected set; }
    public int currentGrowth { get; protected set; }

    protected void GrowPops()
    {
        if (isColonized)
        {
            currentGrowth += growthSpeed;

            if (currentGrowth >= popGrowthNeeded)
            {
                currentGrowth -= popGrowthNeeded;
                listOfPops.Add(new Pop());
            }
        }
    }

    #endregion PopManagement
}

