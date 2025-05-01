using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace PlayerSpace
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "ScriptableObjects/InventoryData")]
    public class InventoryDataController : ScriptableObject
    {
        public List<InventoryItem> inventoryList;
               
        public void AddItemData(InventoryItem addedItem)
        {
            InventoryItem item = inventoryList.Find(i => i.itemName == addedItem.itemName);
            if (item != null)
            {
                int totalQuantity = item.quantity + addedItem.quantity;

                if (totalQuantity > 999)
                {
                    totalQuantity = 999;
                }

                item.quantity = totalQuantity;
            }
            else
            {
                inventoryList.Add(addedItem);
            }

        }

        public int GetObjectCount(string objectName)
        {
            InventoryItem item = inventoryList.Find(i => i.itemName == objectName);

            if (item != null)
            {
                return item.quantity;
            }
            else
            {
                return 0;
            }
        }

        public bool RemoveItem(string objectName, int amount = 1)
        {
            InventoryItem item = inventoryList.Find(i => i.itemName == objectName);
            if (item != null)
            {
                if (item.quantity < amount)
                {
                    return false;
                }
                else
                {
                    item.quantity -= amount;
                    return true;
                }
            }
            return false;
        }

        public void SaveData()
        {
            List<InventoryItem> inventoryDataList = inventoryList.Select(item => new InventoryItem(item.itemName, item.quantity, item.spritePath, item.itemDescription)).ToList();

            string json = JsonUtility.ToJson(new ItemSerializableList<InventoryItem>(inventoryDataList));
            PlayerPrefs.SetString("InventoryData", json);
            PlayerPrefs.Save();
        }

        public void LoadData()
        {
            if (PlayerPrefs.HasKey("InventoryData"))
            {
                string json = PlayerPrefs.GetString("InventoryData");
                ItemSerializableList<InventoryItem> inventoryDataArray = JsonUtility.FromJson<ItemSerializableList<InventoryItem>>(json);
                inventoryList = inventoryDataArray.items.Select(item => new InventoryItem(item.itemName, item.quantity, item.spritePath, item.itemDescription)).ToList();
            }
            else
            {
                inventoryList = new List<InventoryItem>();
            }
        }

        public void ClearData()
        {
            PlayerPrefs.DeleteKey("InventoryData");
            inventoryList.Clear();
        }
    }
    
}
