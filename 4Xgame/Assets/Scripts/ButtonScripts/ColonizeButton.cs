using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonizeButton : MonoBehaviour
{
    public delegate void OnPlanetColonisation(GameObject go, Planet p);
    public static OnPlanetColonisation planetColonized;

    public void DoThisWhenClicked()
    {
        planetColonized?.Invoke(this.gameObject, PlayerInputManager.planetDictionary[transform.parent.gameObject]);
    }
}
