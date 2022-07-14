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


    [SerializeField] GameObject[] planetType;
    [SerializeField] GameObject[] size;
    [SerializeField] GameObject[] gravity;
    [SerializeField] GameObject[] radiation;
    [SerializeField] GameObject[] minerals;
    [SerializeField] GameObject[] energy;
    [SerializeField] GameObject[] food;
    [SerializeField] GameObject[] planets;
    [SerializeField] GameObject starType;
    [SerializeField] GameObject amOfPlanets;

    [SerializeField] Sprite radiationLow;
    [SerializeField] Sprite radiationMedium;
    [SerializeField] Sprite radiationHigh;
    [SerializeField] Sprite radiationExtreme;

    [SerializeField] Sprite gravityLow;
    [SerializeField] Sprite gravityMedium;
    [SerializeField] Sprite gravityHigh;

    private void Awake()
    {
        _instance = this;
        ColliderButton.systemClickedOn += ChangeFocusedSystem;
        GalaxyGeneration.starGenerated += AddToDictionary;
    }

    public void AddToDictionary(GameObject go, StarSystem ss)
    {
        systemDictionary.Add(go, ss);
    }

    public void AddToDictionary(GameObject go, Planet planet)
    {
        planetDictionary.Add(go, planet);
    }

    //Do this when player clicks on a star
    public void ChangeFocusedSystem(GameObject go)
    {
         foreach (GameObject p in planets)
            p.SetActive(false);

         focusedSystem = systemDictionary[go];
         OpenWindow(starystemView);

         starType.GetComponent<TextMeshProUGUI>().text = "Star type:" + focusedSystem;
        amOfPlanets.GetComponent<TextMeshProUGUI>().text = "Amount of planets: " + focusedSystem.amountOfPlanets;   

         for (int i = 0; i < focusedSystem.amountOfPlanets; i++)
         {
             planets[i].SetActive(true);
             planetType[i].GetComponent<TextMeshProUGUI>().text = "Type: " + focusedSystem.listOfPlanets[i].GetType().ToString();
             size[i].GetComponent<TextMeshProUGUI>().text = focusedSystem.listOfPlanets[i].pSize.ToString();        
             minerals[i].GetComponent<TextMeshProUGUI>().text = focusedSystem.listOfPlanets[i].numMinerals.ToString();
             energy[i].GetComponent<TextMeshProUGUI>().text = focusedSystem.listOfPlanets[i].numEnergy.ToString();
             food[i].GetComponent<TextMeshProUGUI>().text = focusedSystem.listOfPlanets[i].numFood.ToString();


            switch (focusedSystem.listOfPlanets[i].pRadiation)
            {
                case Planet.planetRadiation.RADIATION_LOW:
                    radiation[i].GetComponent<Image>().sprite = radiationLow;
                    break;
                case Planet.planetRadiation.RADIATION_MEDIUM:
                    radiation[i].GetComponent<Image>().sprite = radiationMedium;
                    break;
                case Planet.planetRadiation.RADIATION_HIGH:
                    radiation[i].GetComponent<Image>().sprite = radiationHigh;
                    break;
                case Planet.planetRadiation.RADIATION_EXTREME:
                    radiation[i].GetComponent<Image>().sprite = radiationExtreme;
                    break;
            }

            switch (focusedSystem.listOfPlanets[i].pGravity)
            {
                case Planet.planetGravity.GRAVITY_SMALL:
                    gravity[i].GetComponent<Image>().sprite = gravityLow;
                    break;
                case Planet.planetGravity.GRAVITY_MEDIUM:
                    gravity[i].GetComponent<Image>().sprite = gravityMedium;
                    break;
                case Planet.planetGravity.GRAVITY_HIGH:
                    gravity[i].GetComponent<Image>().sprite = gravityHigh;
                    break;
            }
        } 
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
