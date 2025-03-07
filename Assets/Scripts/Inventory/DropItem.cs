using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerSpace
{
    public class DropItem : MonoBehaviour
    {
        public string itemName;
        public int quantity;
        public string spritePath;
        public string itemDescription;

        InventoryDataController inventory;
               
        private void Start()
        {
            inventory = InventoryVisualManager.Instance.inventoryData;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                inventory.AddItemData(new InventoryItem(gameObject.GetComponent<DropItem>()));
                Destroy(gameObject);
            }
        }
    }



}
