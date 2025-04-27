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
        [SerializeField]
        private GameObject ComboHUD;
        public bool menuActivated;
        public ItemSlot[] itemSlots;
        public InventoryDataController inventoryData;
        public GameObject deathMenu;
        [SerializeField]
        private TMP_Text interactionText;
        [SerializeField]
        private GameObject interactionTextBackground;
        GameObject playerModel;
        public bool isSpellMenuActive = false;

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
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }

        }



        void Update()
        {
            if (input.Inventory.WasPressedThisFrame() && !deathMenu.activeSelf)
            {
                DeselectAllSlots();
                LoadAllItems();
                MenuActivated();
                InventoryMenu.SetActive(menuActivated);
            }
        }

        public void LoadAllItems()
        {
            for(int i = 0; i < itemSlots.Length; i++)
            {
                if(i < inventoryData.inventoryList.Count)
                {
                    if(inventoryData.inventoryList[i].quantity <= 0)
                    {
                        inventoryData.inventoryList.RemoveAt(i);
                        itemSlots[i].EmptyItem();
                    }
                    else
                    {
                        itemSlots[i].AddItem(inventoryData.inventoryList[i]);
                    }
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

            if (current.name == "MainMenuScene")
            {
                GUI.SetActive(true);
            }
        }
        private void OnSceneLoaded(Scene current, LoadSceneMode sceneMode)
        {
            playerModel = GameObject.Find("CharacterModel");

            if (current.name == "MainMenuScene")
            {
                GUI.SetActive(false);
            }
        }

        public void EnableInteractionText(string message)
        {
            interactionText.text = message;
            interactionText.gameObject.SetActive(true);
            interactionTextBackground.gameObject.SetActive(true);
        }

        public void DisableInteractionText() 
        {
            interactionText.text = "";
            interactionText.gameObject.SetActive(false);
            interactionTextBackground.gameObject.SetActive(false);
        }

        public void MenuActivated(bool isSpell = false, bool isDeath = false)
        {
            if (isSpell)
            {
                isSpellMenuActive = !isSpellMenuActive;
            }
            else
            {
                isSpellMenuActive = false;
            }

            menuActivated = !menuActivated;            
            GUI.SetActive(!menuActivated);

            if(isDeath)
            {
                deathMenu.SetActive(menuActivated);
                ComboHUD.SetActive(!menuActivated);
            }
            else
            {
                Time.timeScale = menuActivated ? 0 : 1;
                playerModel.SetActive(!menuActivated);
            }

            Cursor.visible = menuActivated;
            Cursor.lockState = menuActivated ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public void RestartScene()
        {
            deathMenu.SetActive(false);
            MenuActivated(false,true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ReturnToMainMenu()
        {
            menuActivated = !menuActivated;
            Time.timeScale = menuActivated ? 0 : 1;
            InventoryMenu.SetActive(menuActivated);
            SceneManager.LoadScene("MainMenuScene");
        }

        public void ReturnToMuseum()
        {
            MenuActivated(false, true);
            SceneManager.LoadScene("MuseoScene");
        }
    }
}
