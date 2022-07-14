using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitatablePlanet : Planet
{
    protected int[] resourceWeights = new int[]
    {   //MINERALS, ENERGY, FOOD
        25,25,35
    };

    public override void GeneratePlanetarySurface(int[] radWeights)
    {
        StarSystem.planetGenerated -= GeneratePlanetarySurface;
        pSize = GeneratePlanetSize();
        pGravity = GenerateGravity(pSize);
        pRadiation = GenerateRadiation(radWeights);
        GenerateResources(resourceWeights);

    }
}