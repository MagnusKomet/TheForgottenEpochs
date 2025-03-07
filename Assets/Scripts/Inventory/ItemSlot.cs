using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PlayerSpace
{

    public class ItemSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private InventoryVisualManager inventoryManager;

        // ------ITEM DATA------  //

        public string itemName;
        public int quantity;
        public Sprite itemSprite;
        public Sprite emptySprite;
        public string itemDescription;


        // ------ITEM DESCRIPTION SLOT------  //
        public Image itemDescriptionImage;
        public TMP_Text itemDescriptionText;
        public TMP_Text itemDescriptionNameText;


        // ------ITEM SLOT------  //
        [SerializeField]
        private TMP_Text quantityText;

        [SerializeField]
        private Image itemImage;

        public GameObject selectedShader;
        public bool thisItemSelected;

        public void AddItem(InventoryItem item)
        {
            //Update NAME
            itemName = item.itemName;

            //Update SPRITE
            itemSprite = item.GetSprite();
            itemImage.sprite = itemSprite;

            //Update DESCRIPTION
            itemDescription = item.itemDescription;

            //Update QUANTITY
            quantity = item.quantity;
            quantityText.text = quantity.ToString();
            quantityText.enabled = true;
        }

        public void EmptyItem()
        {
            itemName = "";
            itemSprite = emptySprite;
            itemImage.sprite = emptySprite;
            itemDescription = "";
            quantity = 0;
            quantityText.text = "";
            quantityText.enabled = false;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick();
            }

        }

        public void OnLeftClick()
        {
            if (!thisItemSelected)
            {
                inventoryManager.DeselectAllSlots();
                selectedShader.SetActive(true);
                thisItemSelected = true;


                itemDescriptionImage.sprite = itemSprite;
                itemDescriptionText.text = itemDescription;
                itemDescriptionNameText.text = itemName;

            }

        }



    }

}
