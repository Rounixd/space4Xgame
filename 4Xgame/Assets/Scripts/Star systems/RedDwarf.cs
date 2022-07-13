using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDwarf : StarSystem
{
    readonly int[] planetWeights = new int[]
    {
        //  PLANET_HABITATABLE, PLANET_BARREN
        70,30
    };

    readonly static int[] radiationWeights = new int[]
    {
        //RADIATION_LOW, RADIATION_MEDIUM, RADIATION_HIGH, RADIATION_EXTREME
        15,40,40,5

    };

    public override void GenerateStarsystem(GameObject go, StarSystem ss)
    {
        GalaxyGeneration.starGenerated -= GenerateStarsystem;
        amountOfPlanets = Mathf.FloorToInt(WeightedProbability.CalculateNormalDistribution(MIN_PLANETS, MAX_PLANETS, 0.7f, 1.3f));
        listOfPlanets = GeneratePlanetTypes(planetWeights, amountOfPlanets);
    }

    protected override int[] GetRadWeights()
    {
        return radiationWeights;
    }

}
