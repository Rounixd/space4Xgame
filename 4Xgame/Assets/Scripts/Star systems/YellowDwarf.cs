using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowDwarf : StarSystem
{
    readonly int[] planetWeights = new int[]
    {
        //  PLANET_HABITATABLE, PLANET_BARREN
        25, 75
    };

    readonly static int[] radiationWeights = new int[]
   {
        //RADIATION_LOW, RADIATION_MEDIUM, RADIATION_HIGH, RADIATION_EXTREME
        40,40,20,0

   };

    public override void GenerateStarsystem(GameObject go, StarSystem ss)
    {
        GalaxyGeneration.starGenerated -= GenerateStarsystem;
        amountOfPlanets = Mathf.FloorToInt(WeightedProbability.CalculateNormalDistribution(MIN_PLANETS, MAX_PLANETS, 0.75f, 1.3f));
        listOfPlanets = GeneratePlanetTypes(planetWeights, amountOfPlanets);
    }

    protected override int[] GetRadWeights()
    {
        return radiationWeights;
    }
}
