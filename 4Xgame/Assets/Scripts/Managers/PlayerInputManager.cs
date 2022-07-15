using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manages systems and planets player clicked on, and connects their data and physical versions

public class PlayerInputManager : MonoBehaviour
{
    private static PlayerInputManager _instance;

    public static PlayerInputManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject();
                go.AddComponent<PlayerInputManager>();
            }

            return _instance;
        }
    }

    public Dictionary<GameObject, StarSystem> systemDictionary { get; private set; } = new Dictionary<GameObject, StarSystem>();
    public Dictionary<GameObject, Planet> planetDictionary { get; private set; } = new Dictionary<GameObject, Planet>();
    public StarSystem focusedSystem { get; private set; }
    public Planet focusedPlanet { get; private set; }

    List<GameObject> openedWidnows = new List<GameObject>();
    [SerializeField] GameObject galaxyView;
    [SerializeField] GameObject starystemView;


    [SerializeField] GameObject planetPrefab;

    PlanetPrefabChildReferencer[] planetPrefabArray = new PlanetPrefabChildReferencer[3];


    [SerializeField] GameObject[] planets;
    [SerializeField] GameObject starType;
    [SerializeField] GameObject amOfPlanets;
    [SerializeField] GameObject mainStar;

    [SerializeField] Sprite radiationLow;
    [SerializeField] Sprite radiationMedium;
    [SerializeField] Sprite radiationHigh;
    [SerializeField] Sprite radiationExtreme;

    [SerializeField] Sprite gravityLow;
    [SerializeField] Sprite gravityMedium;
    [SerializeField] Sprite gravityHigh;

    [SerializeField] Sprite habitabilityNone;
    [SerializeField] Sprite habitabilityLow;
    [SerializeField] Sprite habitabilityMedium;
    [SerializeField] Sprite habitabilityHigh;

    [SerializeField] Sprite barrenPlanet;
    [SerializeField] Sprite habitablePlanet;

    [SerializeField] Sprite blackHole;
    [SerializeField] Sprite redStar;
    [SerializeField] Sprite yellowStar;


    private void Awake()
    {
        _instance = this;
        ColliderButton.systemClickedOn += ChangeFocusedSystem;
        GalaxyGeneration.starGenerated += AddToDictionary;
        GalaxyGeneration.starGenerated += AssignStarsystemSprites;
    }

    public void AddToDictionary(GameObject go, StarSystem ss)
    {
        systemDictionary.Add(go, ss);
    }

    public void AddToDictionary(GameObject go, Planet planet)
    {
        planetDictionary.Add(go, planet);
    }

    //Update the starsystemview when player clicks on the star
    public void ChangeFocusedSystem(GameObject go)
    {
        //Clear all the leftovers from the previous focused system, find the data of the new one.
        foreach (GameObject p in planets)
            Destroy(p);

        focusedSystem = systemDictionary[go];
        OpenWindow(starystemView);

        // This whole thing updates the starsystem view. Possible candidate for a rerrite, since the code is kind of bad.
        starType.GetComponent<TextMeshProUGUI>().text = "Star type:" + focusedSystem;
        amOfPlanets.GetComponent<TextMeshProUGUI>().text = "Amount of planets: " + focusedSystem.amountOfPlanets;   

        for (int i = 0; i < focusedSystem.amountOfPlanets; i++)
        {   
            planets[i] = Instantiate(planetPrefab, transform.position, Quaternion.identity, starystemView.transform);
            planetPrefabArray[i] = planets[i].GetComponent<PlanetPrefabChildReferencer>();
            RectTransform spawnedPlanet = planets[i].GetComponent<RectTransform>();
            spawnedPlanet.anchoredPosition = new Vector2(-500 + (500*i), 85);
            spawnedPlanet.localScale = new Vector3(100, 100, 100);


            planetPrefabArray[i].size.text = focusedSystem.listOfPlanets[i].pSize.ToString();
            planetPrefabArray[i].minerals.text = focusedSystem.listOfPlanets[i].numMinerals.ToString();
            planetPrefabArray[i].energy.text = focusedSystem.listOfPlanets[i].numEnergy.ToString();
            planetPrefabArray[i].food.text = focusedSystem.listOfPlanets[i].numFood.ToString();

            switch (focusedSystem.listOfPlanets[i].pRadiation)
            {
                case Planet.planetRadiation.RADIATION_LOW:
                    planetPrefabArray[i].radiation.sprite = radiationLow;
                    break;
                case Planet.planetRadiation.RADIATION_MEDIUM:
                    planetPrefabArray[i].radiation.sprite = radiationMedium;
                    break;
                case Planet.planetRadiation.RADIATION_HIGH:
                    planetPrefabArray[i].radiation.sprite = radiationHigh;
                    break;
                case Planet.planetRadiation.RADIATION_EXTREME:
                    planetPrefabArray[i].radiation.sprite = radiationExtreme;
                    break;
            }

            switch (focusedSystem.listOfPlanets[i].pHabitability)
            {
                case Planet.planetHabitability.HABITABILITY_ZERO:
                    planetPrefabArray[i].habitability.sprite = habitabilityNone;
                    break;
                case Planet.planetHabitability.HABITABILITY_LOW:
                    planetPrefabArray[i].habitability.sprite = habitabilityLow;
                    break;
                case Planet.planetHabitability.HABITABILITY_MEDIUM:
                    planetPrefabArray[i].habitability.sprite = habitabilityMedium;
                    break;
                case Planet.planetHabitability.HABITABILITY_HIGH:
                    planetPrefabArray[i].habitability.sprite = habitabilityHigh;
                    break;
            }

            switch (focusedSystem.listOfPlanets[i].pGravity)
            {
                case Planet.planetGravity.GRAVITY_SMALL:
                    planetPrefabArray[i].gravity.sprite = gravityLow;
                    break;
                case Planet.planetGravity.GRAVITY_MEDIUM:
                     planetPrefabArray[i].gravity.sprite = gravityMedium;
                    break;
                case Planet.planetGravity.GRAVITY_HIGH:
                    planetPrefabArray[i].gravity.sprite = gravityHigh;
                    break;
            }

            if (focusedSystem.listOfPlanets[i] is HabitatablePlanet)
                planets[i].GetComponent<Image>().sprite = habitablePlanet;
            if (focusedSystem.listOfPlanets[i] is BarrenPlanet)
                planets[i].GetComponent<Image>().sprite = barrenPlanet;
        }

        switch (focusedSystem)
        {
            case BlackHole:
                mainStar.GetComponent<Image>().sprite = blackHole;
                break;
            case RedDwarf:
                mainStar.GetComponent<Image>().sprite = redStar;
                break;
            case YellowDwarf:
                mainStar.GetComponent<Image>().sprite = yellowStar;
                break;
        }

    }

    void AssignStarsystemSprites(GameObject gObject, StarSystem system)
    {
            if (system is BlackHole)
                gObject.GetComponent<SpriteRenderer>().sprite = blackHole;
            else if (system is RedDwarf)
                gObject.GetComponent<SpriteRenderer>().sprite = redStar;
            else if (system is YellowDwarf)
                gObject.GetComponent<SpriteRenderer>().sprite = yellowStar;
    }

    ///////                ////////
    /////// BUTTON METHODS ////////
    ///////                ////////

    public void OpenWindow(GameObject window)
    {
        window.SetActive(true);
        openedWidnows.Add(window);

        galaxyView.SetActive(false);

    }

    public void CloseLastOpenedWindow()
    {
        if (openedWidnows.Count > 0) {
            GameObject lastWindow = openedWidnows[openedWidnows.Count - 1];
            lastWindow.SetActive(false);
            openedWidnows.Remove(lastWindow);

            if (openedWidnows.Count == 0)
                galaxyView.SetActive(true);
        }    
       
    }

    public void CloseAllWindows()
    {
        foreach (GameObject window in openedWidnows)
        {
            window.SetActive(false);
            openedWidnows.Remove(window);

            galaxyView.SetActive(true);
        }
    }
}
