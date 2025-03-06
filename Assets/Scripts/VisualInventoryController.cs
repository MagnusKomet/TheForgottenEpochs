using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualInventoryController : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    //public ItemSlot[] itemSlot;
    //public ItemSO[] itemSOs;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }
        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }
}
