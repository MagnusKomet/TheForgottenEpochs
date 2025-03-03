using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlayerSpace
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "ScriptableObjects/InventoryData")]
    public class InventoryController : ScriptableObject
    {
        [System.Serializable]
        public class Item
        {
            public string itemName;
            public int amount;

            public Item(string itemName, int amount)
            {
                this.itemName = itemName;
                this.amount = amount;
            }
        }

        private List<Item> inventory = new List<Item>();
        private bool isInitialized = false;

        public void Initialize()
        {
            if (!isInitialized)
            {
                LoadData();
                isInitialized = true;
            }
        }

        public void AddObject(string objectName, int amount = 1)
        {
            Item item = inventory.Find(i => i.itemName == objectName);
            if (item != null)
            {
                item.amount += amount;
            }
            else
            {
                inventory.Add(new Item(objectName, amount));
            }
        }

        public int GetObjectCount(string objectName)
        {
            Item item = inventory.Find(i => i.itemName == objectName);
            return item != null ? item.amount : 0;
        }

        public bool RemoveObject(string objectName, int amount = 1)
        {
            Item item = inventory.Find(i => i.itemName == objectName);
            if (item != null)
            {
                if (item.amount < amount)
                {
                    return false;
                }
                else
                {
                    item.amount -= amount;
                    return true;
                }
            }
            return false;
        }

        public void SaveData()
        {
            foreach (var item in inventory)
            {
                PlayerPrefs.SetInt(item.itemName, item.amount);
            }
            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            foreach (var item in inventory)
            {
                item.amount = PlayerPrefs.GetInt(item.itemName, 0);
            }
        }
    }

}
