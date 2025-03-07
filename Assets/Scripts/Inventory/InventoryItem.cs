using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public int quantity;
        public string spritePath;
        public string itemDescription;

        public InventoryItem(string itemName, int quantity, string spritePath, string itemDescription)
        {
            this.itemName = itemName;
            this.quantity = quantity;
            this.spritePath = spritePath;
            this.itemDescription = itemDescription;
        }

        public InventoryItem(DropItem dropItem)
        {
            itemName = dropItem.itemName;
            quantity = dropItem.quantity;
            spritePath = dropItem.spritePath;
            itemDescription = dropItem.itemDescription;
        }

        public Sprite GetSprite()
        {
            return Resources.Load<Sprite>(spritePath);
        }
    }

    [System.Serializable]
    public class ItemSerializableList<T>
    {
        public List<T> items;
        public ItemSerializableList(List<T> items)
        {
            this.items = items;
        }
    }
}