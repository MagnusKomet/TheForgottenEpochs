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
        private Sprite emptySprite;
        public bool isFull;
        public string itemDescription;

        [SerializeField]
        private int maxNumberOfItems = 999;


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

        private void Start()
        {
            emptySprite = Resources.Load<Sprite>("emptySprite");
            EmptySlot();
        }

        public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
        {
            //Check if the slot is full

            if (isFull)
            {
                return quantity;
            }

            //Update NAME
            this.itemName = itemName;

            //Update SPRITE
            this.itemSprite = itemSprite;
            itemImage.sprite = itemSprite;

            //Update DESCRIPTION
            this.itemDescription = itemDescription;


            //Update QUANTITY
            this.quantity += quantity;
            if (this.quantity >= maxNumberOfItems)
            {
                quantityText.text = maxNumberOfItems.ToString();
                quantityText.enabled = true;
                isFull = true;

                //Return the LEFTOVERS
                int extraItems = this.quantity - maxNumberOfItems;
                this.quantity = maxNumberOfItems;
                return extraItems;
            }

            //Update QUANTITY TEXT
            quantityText.text = this.quantity.ToString();
            quantityText.enabled = true;

            return 0;

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

        public void EmptySlot()
        {
            itemSprite = emptySprite;
            quantityText.enabled = false;
            itemImage.sprite = emptySprite;
            itemDescriptionNameText.text = "";
            itemDescriptionText.text = "";
            itemDescriptionImage.sprite = emptySprite;
            itemName = "";
        }

    }

}
