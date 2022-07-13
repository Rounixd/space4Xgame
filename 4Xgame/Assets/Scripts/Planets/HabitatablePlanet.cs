using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitatablePlanet : Planet
{

    public override void GeneratePlanetarySurface(int[] radWeights)
    {
        StarSystem.planetGenerated -= GeneratePlanetarySurface;
        pSize = GeneratePlanetSize();
        pGravity = GenerateGravity(pSize);
        pRadiation = GenerateRadiation(radWeights);

        for (int i = 0; i <= pSize - 1; i++)
        {
            planetResources pRes = GenerateResourceType(resourceWeights);

            switch ((int)pRes)
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

        if (pGravity == planetGravity.GRAVITY_SMALL)
            numMinerals = Mathf.RoundToInt(numMinerals / 1.5f);
        if (pGravity == planetGravity.GRAVITY_HIGH)
            numMinerals = Mathf.RoundToInt(numMinerals * 1.5f);

        if (pRadiation == planetRadiation.RADIATION_LOW)
            numEnergy = Mathf.RoundToInt(numEnergy / 1.5f);
        if (pRadiation == planetRadiation.RADIATION_HIGH)
            numEnergy = Mathf.RoundToInt(numEnergy * 1.5f);
        if (pRadiation == planetRadiation.RADIATION_EXTREME)
            numEnergy = numEnergy * 2;

        Debug.Log("Food: " + numFood);
        Debug.Log("Energy: " + numEnergy);
        Debug.Log("Minerals: " + numMinerals);

    }
}
