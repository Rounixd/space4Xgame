using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player
{
    int energyOwned, mineralsOwned, foodOwned;

    abstract protected void ClaimPlanet();
    

}
