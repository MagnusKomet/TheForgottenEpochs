using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerSpace
{
    [System.Serializable]
    public class InventoryItem : MonoBehaviour
    {
        public string itemName;
        public int quantity;
        public string spritePath;
        public string itemDescription;

        InventoryVisualManager inventory;

        public Sprite GetSprite()
        {
            return Resources.Load<Sprite>(spritePath);
        }

        private void Start()
        {
            inventory = InventoryVisualManager.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                inventory.AddItem(gameObject.GetComponent<InventoryItem>());
                Destroy(gameObject);
            }
        }
    }



}
