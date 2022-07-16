using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetPrefabChildReferencer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI size;
    [SerializeField] public Image gravity;
    [SerializeField] public Image radiation;
    [SerializeField] public TextMeshProUGUI minerals;
    [SerializeField] public TextMeshProUGUI energy;
    [SerializeField] public TextMeshProUGUI food;
    [SerializeField] public Image habitability;

    [SerializeField] public GameObject planetOwned;
    [SerializeField] public GameObject planetUncolonized;

}

