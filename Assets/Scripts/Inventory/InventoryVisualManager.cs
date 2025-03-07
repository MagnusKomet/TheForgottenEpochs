using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

namespace PlayerSpace
{
    public class InventoryVisualManager : MonoBehaviour
    {
        public static InventoryVisualManager Instance { get; private set; }

        PlayerInput.OnFootActions input;
        public GameObject InventoryMenu;
        [SerializeField]
        private GameObject GUI;
        public bool menuActivated;
        public ItemSlot[] itemSlots;
        public InventoryDataController inventoryData;

        [SerializeField]
        private TMP_Text interactionText;

        private void Awake()
        {

            if (Instance == null)
            {
                input = new PlayerInput().OnFoot;
                input.Enable();
                Instance = this;
                DontDestroyOnLoad(gameObject);
                inventoryData.LoadData();
                SceneManager.sceneUnloaded += OnSceneUnloaded;
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
                DeselectAllSlots();
                LoadAllItems();
                menuActivated = !menuActivated;
                InventoryMenu.SetActive(menuActivated);
                GUI.SetActive(!menuActivated);
                Time.timeScale = menuActivated ? 0 : 1;
                Cursor.visible = menuActivated;
                Cursor.lockState = menuActivated ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        public void LoadAllItems()
        {
            for(int i = 0; i < itemSlots.Length; i++)
            {
                if(i < inventoryData.inventoryList.Count)
                {
                    itemSlots[i].AddItem(inventoryData.inventoryList[i]);
                }
                else
                {
                    itemSlots[i].EmptyItem();
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

        private void OnSceneUnloaded(Scene current)
        {
            inventoryData.SaveData();
        }

        public void EnableInteractionText(string message)
        {
            interactionText.text = message;
            interactionText.gameObject.SetActive(true);
        }

        public void DisableInteractionText() 
        {
            interactionText.text = "";
            interactionText.gameObject.SetActive(false);
        }
    }
}
