using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem
{
    public List<Planet> listOfPlanets = new List<Planet>();
    public Vector2 position { get; protected set; }
    public int amountOfPlanets { get; protected set; }

    protected const int MIN_PLANETS = 0;
    protected const int MAX_PLANETS = 3;

    public delegate void PlanetGenerated(int[] radWeights);
    public static PlanetGenerated planetGenerated;

    public StarSystem() {
        GalaxyGeneration.starGenerated += GenerateStarsystem;
    }

    protected virtual int[] GetRadWeights() { return null; }
    public virtual void GenerateStarsystem(GameObject go, StarSystem ss) {}

    ////              ////
    ////  GENERATION  ////
    ////              ////

    enum planetTypes
    {
        PLANET_HABITATABLE, PLANET_BARREN
    }

    //Generate planet type, return a list of thsose types as data so it can be saved in the children of the Starsystem class.
    protected List<Planet> GeneratePlanetTypes(int[] _planetWeights, int _amountOfPlanets)
    {
        List<Planet> planetList = new List<Planet>();

        for (int i = 0; i < _amountOfPlanets; i++)
        {

            switch (WeightedProbability.CalculateWeightedProbability(_planetWeights))
            {
                case (int)planetTypes.PLANET_BARREN :
                    planetList.Add(new BarrenPlanet());
                    break;

                case (int)planetTypes.PLANET_HABITATABLE:
                    planetList.Add(new HabitatablePlanet());
                    break;

            }

        }

        //Send the rad weights from children to the planets, so they can generate radiation.
        planetGenerated?.Invoke(GetRadWeights());
        return planetList;
    }

}
