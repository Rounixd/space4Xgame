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

    public static Dictionary<GameObject, StarSystem> systemDictionary { get; private set; } = new Dictionary<GameObject, StarSystem>();
    public static Dictionary<GameObject, Planet> planetDictionary { get; private set; } = new Dictionary<GameObject, Planet>();
    public static StarSystem focusedSystem { get; private set; }

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
        GalaxyGeneration.starGenerated = (GameObject go, StarSystem ss) => { systemDictionary.Add(go, ss); };       
        GalaxyGeneration.starGenerated += AssignStarsystemSprites;
        ColliderButton.systemClickedOn += ChangeFocusedSystem;      

        for (int i = 0; i < 3; i++)
            planetPrefabArray[i] = planets[i].GetComponent<PlanetPrefabChildReferencer>();
    }

    private void Start()
    {
        ColonizeButton.planetColonized += HideUncolonizedPlanetButton;
        TurnManager.onNewTurn += UpdatePopSliderValues;
    }

    public void AddToDictionary(GameObject go, StarSystem ss)
    {
        systemDictionary.Add(go, ss);
    }

    //Update the starsystemview when player clicks on the star
    public void ChangeFocusedSystem(GameObject go)
    {
        //Clear all the leftovers from the previous focused system, find the data of the new one.
        planetDictionary.Clear();
        foreach (GameObject p in planets)
            p.SetActive(false);
        
        focusedSystem = systemDictionary[go];
        OpenWindow(starystemView);

        // This whole thing updates the starsystem view. Possible candidate for a rerrite, since the code is kind of bad.
        starType.GetComponent<TextMeshProUGUI>().text = "Star type:" + focusedSystem;
        amOfPlanets.GetComponent<TextMeshProUGUI>().text = "Amount of planets: " + focusedSystem.amountOfPlanets;   

        for (int i = 0; i < focusedSystem.amountOfPlanets; i++)
        {
            PlanetPrefabChildReferencer currentPlanetPrefab = planetPrefabArray[i];
            Planet currentPlanet = focusedSystem.listOfPlanets[i];

            planets[i].SetActive(true);
            planetDictionary.Add(planets[i], currentPlanet);
            currentPlanetPrefab.size.text = currentPlanet.pSize.ToString();
            currentPlanetPrefab.minerals.text = currentPlanet.numMinerals.ToString();
            currentPlanetPrefab.energy.text = currentPlanet.numEnergy.ToString();
            currentPlanetPrefab.food.text = currentPlanet.numFood.ToString();
            UpdatePopSliderValues();

            if (currentPlanet.isColonized)
            {
                currentPlanetPrefab.planetUncolonized.SetActive(false);
                currentPlanetPrefab.planetOwned.SetActive(true);

            }
            else
            {
                currentPlanetPrefab.planetUncolonized.SetActive(true);
                currentPlanetPrefab.planetOwned.SetActive(false);
            }

            switch (currentPlanet.pRadiation)
            {
                case Planet.planetRadiation.RADIATION_LOW:
                    currentPlanetPrefab.radiation.sprite = radiationLow;
                    break;
                case Planet.planetRadiation.RADIATION_MEDIUM:
                    currentPlanetPrefab.radiation.sprite = radiationMedium;
                    break;
                case Planet.planetRadiation.RADIATION_HIGH:
                    currentPlanetPrefab.radiation.sprite = radiationHigh;
                    break;
                case Planet.planetRadiation.RADIATION_EXTREME:
                    currentPlanetPrefab.radiation.sprite = radiationExtreme;
                    break;
            }

            switch (currentPlanet.pHabitability)
            {
                case Planet.planetHabitability.HABITABILITY_ZERO:
                    currentPlanetPrefab.habitability.sprite = habitabilityNone;
                    break;
                case Planet.planetHabitability.HABITABILITY_LOW:
                    currentPlanetPrefab.habitability.sprite = habitabilityLow;
                    break;
                case Planet.planetHabitability.HABITABILITY_MEDIUM:
                    currentPlanetPrefab.habitability.sprite = habitabilityMedium;
                    break;
                case Planet.planetHabitability.HABITABILITY_HIGH:
                    currentPlanetPrefab.habitability.sprite = habitabilityHigh;
                    break;
            }

            switch (currentPlanet.pGravity)
            {
                case Planet.planetGravity.GRAVITY_SMALL:
                    currentPlanetPrefab.gravity.sprite = gravityLow;
                    break;
                case Planet.planetGravity.GRAVITY_MEDIUM:
                    currentPlanetPrefab.gravity.sprite = gravityMedium;
                    break;
                case Planet.planetGravity.GRAVITY_HIGH:
                    currentPlanetPrefab.gravity.sprite = gravityHigh;
                    break;
            }

            if (currentPlanet is HabitatablePlanet)
                currentPlanetPrefab.gameObject.GetComponent<Image>().sprite = habitablePlanet;
            if (currentPlanet is BarrenPlanet)
                currentPlanetPrefab.gameObject.GetComponent<Image>().sprite = barrenPlanet;
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

    void UpdatePopSliderValues()
    {
        for (int i = 0; i < focusedSystem.amountOfPlanets; i++)
        {
            PlanetPrefabChildReferencer currentPlanetPrefab = planetPrefabArray[i];
            Planet currentPlanet = focusedSystem.listOfPlanets[i];

            currentPlanetPrefab.popsNum.text = currentPlanet.listOfPops.Count.ToString();
            currentPlanetPrefab.popSlider.maxValue = currentPlanet.popGrowthNeeded;
            currentPlanetPrefab.popSlider.value = currentPlanet.currentGrowth;

            if (currentPlanet.growthSpeed + currentPlanet.currentGrowth != 0)
            {
                /*int tempModulo = currentPlanet.popGrowthNeeded % currentPlanet.growthSpeed;
                int tempDividingCalc = currentPlanet.popGrowthNeeded / (currentPlanet.growthSpeed + currentPlanet.currentGrowth);

                if (tempModulo >= 0 && tempDividingCalc != 0)
                    currentPlanetPrefab.turnsTillNextPop.text = (tempDividingCalc + 1).ToString();

                if (currentPlanet.popGrowthNeeded <= currentPlanet.growthSpeed + currentPlanet.currentGrowth)
                    currentPlanetPrefab.turnsTillNextPop.text = "1";*/

                if(currentPlanet.isColonized)
                {
                    Debug.Log(currentPlanet.popGrowthNeeded);
                    Debug.Log(currentPlanet.growthSpeed);
                    Debug.Log(currentPlanet.currentGrowth);
                }

                int tempNum = (currentPlanet.popGrowthNeeded - currentPlanet.currentGrowth) / currentPlanet.growthSpeed;

                if (currentPlanet.growthSpeed + currentPlanet.currentGrowth < currentPlanet.popGrowthNeeded)
                {
                    if ((currentPlanet.popGrowthNeeded - currentPlanet.currentGrowth) % currentPlanet.growthSpeed != 0)
                        currentPlanetPrefab.turnsTillNextPop.text = (tempNum + 1).ToString();
                    else currentPlanetPrefab.turnsTillNextPop.text = tempNum.ToString();
                }
                else currentPlanetPrefab.turnsTillNextPop.text = "1";
            }
            else currentPlanetPrefab.turnsTillNextPop.text = Mathf.Infinity.ToString();

        }
    }

    void HideUncolonizedPlanetButton(GameObject go, Planet p)
    {
        go.GetComponentInParent<PlanetPrefabChildReferencer>().planetOwned.SetActive(true);
        go.GetComponentInParent<PlanetPrefabChildReferencer>().planetUncolonized.SetActive(false);
        UpdatePopSliderValues();

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
