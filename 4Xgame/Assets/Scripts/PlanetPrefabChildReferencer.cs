using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Class holding all the references of all the objects of the planet prefab
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

    [SerializeField] public Slider popSlider;
    [SerializeField] public TextMeshProUGUI popsNum;
    [SerializeField] public TextMeshProUGUI turnsTillNextPop;

    [SerializeField] public GameObject buildingsView;
    [SerializeField] public GameObject employmentView;


    public void SwitchToEmploymentView()
    {
        buildingsView.SetActive(false);
        employmentView.SetActive(true);
    }

    public void SwitchToBuildingsView()
    {
        buildingsView.SetActive(true);
        employmentView.SetActive(false);
    }
}


