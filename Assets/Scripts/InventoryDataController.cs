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
        private List<InventoryItem> inventory;

        public void AddItemData(InventoryItem addedItem)
        {
            InventoryItem item = inventory.Find(i => i.itemName == addedItem.name);
            if (item != null)
            {
                item.quantity += addedItem.quantity;
            }
            else
            {
                inventory.Add(addedItem);
            }
        }

        public int GetObjectCount(string objectName)
        {
            InventoryItem item = inventory.Find(i => i.itemName == objectName);

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
            InventoryItem item = inventory.Find(i => i.itemName == objectName);
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
            string json = JsonHelper.ToJson<InventoryItem>(inventory.ToArray());
            PlayerPrefs.SetString("InventoryData", json);
            PlayerPrefs.Save();
        }

        public void LoadData()
        {
            if (PlayerPrefs.HasKey("InventoryData"))
            {
                string json = PlayerPrefs.GetString("InventoryData");
                inventory = JsonHelper.FromJson<InventoryItem>(json).ToList();
                Debug.Log("Inventory data loaded successfully.");
            }
            else
            {
                inventory = new List<InventoryItem>();
                Debug.Log("No inventory data found.");
            }
        }
    }

    public static class JsonHelper
    {
        public static string ToJson<T>(T[] array, bool prettyPrint = false)
        {
            Wrapper<T> wrapper = new Wrapper<T> { Items = array };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

}
