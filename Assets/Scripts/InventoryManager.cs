using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace PlayerSpace
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

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
            }
            else
            {
                Destroy(gameObject);
            }

            inventoryData.Initialize();
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

        public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
        {
            inventoryData.AddObject(itemName, quantity);
            inventoryData.SaveData();

            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (!itemSlots[i].isFull && (itemSlots[i].itemName == itemName || itemSlots[i].quantity == 0))
                {
                    int leftOverItems = itemSlots[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                    return leftOverItems > 0 ? AddItem(itemName, leftOverItems, itemSprite, itemDescription) : 0;
                }
            }
            return quantity;
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
