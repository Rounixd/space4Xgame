using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOwnershipManager : MonoBehaviour
{
    //Make sure its a singleton
    private static PlayerOwnershipManager _instance;
    public static PlayerOwnershipManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                go.AddComponent<PlayerInputManager>();
            }

            return _instance;
        }
    }

    public static Dictionary<Player, List<Planet>> planetsOwnedDictionary = new Dictionary<Player, List<Planet>>();

    private void Start()
    {
        ColonizeButton.planetColonized += AddToPlanetsOwnedDict;
       
    }


    public void AddToPlanetsOwnedDict(GameObject go, Planet planet)
    {

        Player player = TurnManager.currentPlayer;

        if (planetsOwnedDictionary.ContainsKey(player))
        {
            planetsOwnedDictionary[player].Add(planet);
        }
        else
        {
            List<Planet> tempList = new List<Planet>();
            tempList.Add(planet);

            planetsOwnedDictionary.Add(player, tempList);
        }
    }
}
