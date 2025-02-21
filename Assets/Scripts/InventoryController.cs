using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlayerSpace
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "ScriptableObjects/InventoryData")]
    public class InventoryController : ScriptableObject
    {
        private Dictionary<string, int> inventory = new Dictionary<string, int>();
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
            if (inventory.ContainsKey(objectName))
            {
                inventory[objectName] += amount;
            }
            else
            {
                inventory[objectName] = amount;
            }

        }

        public int GetObjectCount(string objectName)
        {
            return inventory.ContainsKey(objectName) ? inventory[objectName] : 0;
        }

        public bool RemoveObject(string objectName, int amount = 1)
        {
            if (inventory.ContainsKey(objectName))
            {
                int newAmount = inventory[objectName] - amount;

                if (newAmount < 0)
                {
                    return false;
                }
                else
                {
                    inventory[objectName] = newAmount;

                    return true;
                }
            }

            return false;


        }

        public void SaveData()
        {
            foreach (var item in inventory)
            {
                PlayerPrefs.SetInt(item.Key, item.Value);
            }
            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            List<string> keys = new List<string>(inventory.Keys);
            foreach (var key in keys)
            {
                inventory[key] = PlayerPrefs.GetInt(key, 0);
            }
        }
    }
}
