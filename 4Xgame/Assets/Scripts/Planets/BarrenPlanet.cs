using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrenPlanet : Planet
{
    protected int[] resourceWeights = new int[]
    {   //MINERALS, ENERGY, FOOD
        40,40,5
    };

    public BarrenPlanet()
    {
        TurnManager.onNewTurn += GrowPops;
        ColonizeButton.planetColonized = (go, _planet) => {_planet.isColonized = true; };
    }

    public override void GeneratePlanetarySurface(int[] radWeights)
    {
        StarSystem.planetGenerated -= GeneratePlanetarySurface;
        pSize = GeneratePlanetSize();
        pGravity = GenerateGravity(pSize);
        pRadiation = GenerateRadiation(radWeights);
        pHabitability = GenerateHabitability(); //Habitability gen is based off on radiation
        GenerateResources(resourceWeights); // This always at the end

    }
}
