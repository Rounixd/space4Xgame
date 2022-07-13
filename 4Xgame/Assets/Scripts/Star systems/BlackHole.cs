using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : StarSystem
{
    readonly int[] planetWeights = new int[]
    {
        //  PLANET_HABITABLE, PLANET_BARREN
        0,100
    };

    readonly static int[] radiationWeights = new int[]
    {
        //RADIATION_LOW, RADIATION_MEDIUM, RADIATION_HIGH, RADIATION_EXTREME
        0,0,10,90

    };

    public override void GenerateStarsystem(GameObject go, StarSystem ss)
    {
        GalaxyGeneration.starGenerated -= GenerateStarsystem;
        amountOfPlanets = Mathf.FloorToInt(WeightedProbability.CalculateNormalDistribution(1, MAX_PLANETS, 0.7f, 1.35f));
        listOfPlanets = GeneratePlanetTypes(planetWeights, amountOfPlanets);
    }

    protected override int[] GetRadWeights()
    {
        return radiationWeights;
    }

}
