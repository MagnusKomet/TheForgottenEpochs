using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace PlayerSpace
{
    public class InventoryVisualManager : MonoBehaviour
    {
        public static InventoryVisualManager Instance { get; private set; }

        PlayerInput.OnFootActions input;
        public GameObject InventoryMenu;
        [SerializeField]
        private GameObject Crosshair;
        public bool menuActivated;
        public ItemSlot[] itemSlots;
        public InventoryDataController inventoryData;

        private void Awake()
        {
            // Asegura que solo haya una instancia del InventoryManager
            if (Instance == null)
            {
                input = new PlayerInput().OnFoot;
                input.Enable();
                Instance = this;
                DontDestroyOnLoad(gameObject);
                if (inventoryData != null)
                {
                    inventoryData.LoadData();
                    Debug.Log("InventoryData initialized successfully.");
                }
                else
                {
                    Debug.LogError("InventoryData is not assigned in InventoryManager.");
                }
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        void Update()
        {
            if (input.Inventory.WasPressedThisFrame())
            {
                menuActivated = !menuActivated;
                InventoryMenu.SetActive(menuActivated);
                Crosshair.SetActive(!menuActivated);
                Time.timeScale = menuActivated ? 0 : 1;
                Cursor.visible = menuActivated; 
                Cursor.lockState = menuActivated ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        public void AddItem(InventoryItem addedItem)
        {
            inventoryData.AddItemData(addedItem);
            inventoryData.SaveData();

            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (!itemSlots[i].isFull && (itemSlots[i].itemName == addedItem.name || itemSlots[i].quantity == 0))
                {
                    itemSlots[i].AddItem(addedItem.name, addedItem.quantity,addedItem.GetSprite(),addedItem.itemDescription);

                }
            }

        }

        public void DeselectAllSlots()
        {
            foreach (var slot in itemSlots)
            {
                slot.selectedShader.SetActive(false);
                slot.thisItemSelected = false;
            }
        }
    }
}
