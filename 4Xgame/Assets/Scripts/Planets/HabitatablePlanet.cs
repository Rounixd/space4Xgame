using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitatablePlanet : Planet
{

    public HabitatablePlanet()
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
        
    }
}
