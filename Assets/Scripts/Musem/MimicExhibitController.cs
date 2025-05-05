using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayerSpace;
using MimicSpace;
using System.Linq;
using UnityEngine.UI;

public class MimicExhibitController : MonoBehaviour
{
    public string exhibitNumber;
    public TMP_Text mimicName;
    public TMP_Text mimicDescription;
    public Image mimicImage;
    public GameObject mimicInfoPanel;
    public GameObject mimicSelectorPanel;
    public Transform mimicSpawnPoint;
    private GameObject invokedMimic;
    private InventoryVisualManager inventoryVisualManager;

    private void Start()
    {
        inventoryVisualManager = InventoryVisualManager.Instance;
        LoadData();
    }

    public void ChangeMimicPanel()
    {
        if (mimicInfoPanel.activeSelf)
        {            
            if (invokedMimic != null)
            {
                inventoryVisualManager.inventoryData.AddItemData(new InventoryItem(invokedMimic.GetComponent<DropItem>()));
                Destroy(invokedMimic);
                invokedMimic = null;
                SaveData();
            }

            mimicInfoPanel.SetActive(false);
            mimicSelectorPanel.SetActive(true);
        }
        else
        {

            mimicInfoPanel.SetActive(true);
            mimicSelectorPanel.SetActive(false);
        }
    }

    public void InvokeMimic(GameObject mimic)
    {
        if (inventoryVisualManager.inventoryData.RemoveItem(mimic.GetComponent<DropItem>().itemName))
        {
            if (invokedMimic != null)
            {
                Destroy(invokedMimic);
                invokedMimic = null;
            }

            SetMimic(mimic);
            ChangeMimicPanel();
            inventoryVisualManager.inventoryData.SaveData();
            SaveData();
        }
        else
        {
            ToastNotification.Show("You don't have that mimic sirrrrrrr");
        }

    }

    public void SetMimic(GameObject mimic)
    {
        invokedMimic = Instantiate(mimic, mimicSpawnPoint.position, mimicSpawnPoint.rotation);
        mimicName.text = mimic.GetComponent<DropItem>().itemName;
        mimicDescription.text = mimic.GetComponent<DropItem>().itemDescription;
        mimicImage.sprite = Resources.Load<Sprite>(mimic.GetComponent<DropItem>().spritePath);
    }

    public void SaveData()
    { 
        string mimicName;

        if(invokedMimic != null)
        {
            mimicName = invokedMimic.name.Replace("(Clone)", "").Trim();
        }
        else
        {
            mimicName = "";
        }
        
        PlayerPrefs.SetString(exhibitNumber, mimicName);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(exhibitNumber))
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(exhibitNumber)))
            {
                SetMimic(Resources.Load<GameObject>(PlayerPrefs.GetString(exhibitNumber)));
                ChangeMimicPanel();
            }
            else
            {
                invokedMimic = null;
            }
        }
        else
        {
            invokedMimic = null;
        }
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteKey(exhibitNumber);
        if (invokedMimic != null)
        {
            Destroy(invokedMimic);
        }
        invokedMimic = null;
    }

    public void ActivatePanel()
    {
        InventoryVisualManager.Instance.MenuActivated(true);

    }

}
