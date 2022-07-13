using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderButton : MonoBehaviour
{
    public delegate void SystemClickedOn(GameObject go);

    public static SystemClickedOn systemClickedOn;

    private void OnMouseDown()
    {
        systemClickedOn?.Invoke(this.gameObject);
    }
}
