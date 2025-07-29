using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Tutorial : MonoBehaviour
{
    public GameObject Inventory;

    public void Start()
    {
        Inventory.gameObject.SetActive(false);
    }

    public void Switch()
    {
        if (Inventory.activeSelf == false) Inventory.SetActive(true);
        else Inventory.SetActive(false);
    }
}