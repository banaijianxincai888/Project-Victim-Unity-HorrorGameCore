using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns
{
    ///summary
    ///
    ///summary 
    public class BagSlot : MonoBehaviour
	{
		public ItemSO itemSO;

        public Image image;

        public TMP_Text quantityText;

        public TMP_Text nameText;

        public CanvasGroup nameGroup;
    
        public Transform selectedUI;


		public int slotQuantity;

        private void Start()
        {
            UpdateUI();
        }


       


        public void UpdateUI()
        {
            if (itemSO == null || slotQuantity == 0)
            {
                //特别处理数量为0的情况
                if (itemSO != null)
                {
                    itemSO = null;
                }
                
                Color color = image.color;
                color.a = 0;
                image.color = color;
                nameGroup.alpha = 0f;
                nameText.text = string.Empty;
                quantityText.text = string.Empty;
            }
            else
            {
                Color color = image.color;
                color.a = 1;
                image.color = color;
                nameText.text =itemSO.itemName;
                image.sprite = itemSO.sprite;
                if(itemSO.maxCount >1)
                    quantityText.text = slotQuantity.ToString();
                else
                    quantityText.text = string.Empty;
            }
        }

        public void Selected()
        {
            selectedUI.gameObject.SetActive(true);
            UpdateUI();
        }

        public void CancelSelected()
        {
            selectedUI.gameObject.SetActive(false);
            UpdateUI();

        }

    }
}

