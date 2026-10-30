using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ns
{   
	///summary
	///背包管理器
	///summary 
	public class BagManager :MonoBehaviour
	{
        public BagSlot[] slots;

        [Header("所有物品SO配置表（读档时按名字寻回对象）")]
        [SerializeField] private List<ItemSO> itemDatabase = new List<ItemSO>();

        public CanvasGroup canvasGroup;

        private int index;


        public float lastTime = 5f;


        private Coroutine coroutine;

        private Coroutine bagCoroutine;

        public QuestTipSO bagFullTip;

        public bool hasPickUpBattery;

        public bool hasPickUpBox;


        public bool hasPickUpFusil;





        private void Start()
        {
            canvasGroup.alpha = 0f;
            index = 0;
            
        }






        public void AddItem(ItemSO itemSO, int quantity)
        {

           
            //需要注意的是要避免直接更改itemSO里的数量，把它存到临时变量中去即可

            int tempCount = quantity;

            foreach (var slot in slots)
            {
                if (slot.itemSO == itemSO && slot.slotQuantity < itemSO.maxCount)
                {
                    int addCount = Mathf.Min(tempCount, itemSO.maxCount - slot.slotQuantity);
                    slot.slotQuantity += addCount;
                    tempCount -= addCount;
                    slot.UpdateUI();
                    if (tempCount <= 0)
                        return;
                }

            }

            if (tempCount > 0)
            {
                foreach (var slot in slots)
                {
                    if (slot.itemSO == null)
                    {
                        slot.itemSO = itemSO;
                        slot.image.sprite = itemSO.sprite;

                        if (tempCount > itemSO.maxCount)
                        {
                            slot.slotQuantity = itemSO.maxCount;
                            tempCount -= itemSO.maxCount;
                        }
                        else
                        {
                            slot.slotQuantity = tempCount;
                            tempCount -= slot.slotQuantity;
                        }

                        slot.UpdateUI();
                        if (tempCount <= 0)
                            return;

                    }
                }
            }



        }

     

        // BagManager.cs
        public bool IsBagFull(ItemSO itemSO)
        {
            // 1. 先检查是否有该物品的槽位，且数量未到上限
            bool hasAvailableSameItemSlot = false;
            foreach (var slot in slots)
            {
                if (slot.itemSO == itemSO && slot.slotQuantity < itemSO.maxCount)
                {
                    hasAvailableSameItemSlot = true;
                    break;
                }
            }
            if (hasAvailableSameItemSlot)
            {
                return false; // 该物品还有槽位可加，背包没满
            }

            // 2. 再检查是否有空槽可放该物品
            bool hasEmptySlot = false;
            foreach (var slot in slots)
            {
                if (slot.itemSO == null)
                {
                    hasEmptySlot = true;
                    break;
                }
            }
            if (hasEmptySlot)
            {
                return false; // 有空槽可放，背包没满
            }

            // 3. 既无该物品的可用槽位，也无空槽 → 该物品无法添加
            UIManager.Instance.questTipManager.ShowQuestTip(bagFullTip);
            return true;
        }


        public void UseItem(ItemSO itemSO,int quantity)
        {
           foreach(var slot in slots)
            {
                if(slot.itemSO == itemSO && slot.slotQuantity >= quantity)
                {
                    slot.slotQuantity -= quantity;
                    
                    slot.UpdateUI();
                    return;
                }
            }
        }

      

        public bool HasItem(ItemSO itemSO)
        {
            foreach (BagSlot bagSlot in slots)
            {
                if (bagSlot.itemSO == itemSO && bagSlot.slotQuantity > 0)
                {
                    return true;

                }
            }
            return false;
        }

        public int CheckitemSOCount(ItemSO itemSO)
        {
            int total = 0;
            foreach (BagSlot bagSlot in slots)
            {
                if (bagSlot.itemSO == itemSO)
                    total += bagSlot.slotQuantity;
            }
            return total;
        }

        public void ShowBag(float scroll)
        {
            UIManager.Instance.SharpShowAndFadeWithCor(canvasGroup, ref bagCoroutine ,lastTime);
            //为了不打扰背包本身的显示协程，就自己再去创建一个协程把
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                slots[index].nameGroup.alpha = 0f;
            }
            slots[index].CancelSelected();
            if (scroll > 0f)
            {
                // 向上滚：上一个
                index--;
                if (index < 0)
                {
                    index = slots.Length - 1;
                }
            }
            else if (scroll < 0f)
            {
                // 向下滚：下一个
                index++;
                if (index > slots.Length - 1)
                {
                    index = 0;
                }
            }
            else
            {
                index = 0;

            }
            if (slots[index].itemSO!=null)
                coroutine = StartCoroutine(UIManager.Instance.SharpShowAndFade(slots[index].nameGroup, 2, 0.3f));
            slots[index].Selected();

        }

        // 提取数据快照（毫秒级，主线程直接调用）
        public BagSaveData GetSaveData()
        {
            BagSaveData data = new BagSaveData
            {
                hasPickUpBattery = this.hasPickUpBattery,
                hasPickUpBox = this.hasPickUpBox,
                hasPickUpFusil = this.hasPickUpFusil
            };

            foreach (var slot in slots)
            {
                var sData = new SlotSaveData();
                if (slot != null && slot.itemSO != null && slot.slotQuantity > 0)
                {
                    // 记录 itemName（或 slot.itemSO.name）
                    sData.itemName = slot.itemSO.itemName;
                    sData.quantity = slot.slotQuantity;
                }
                else
                {
                    sData.itemName = string.Empty;
                    sData.quantity = 0;
                }
                data.slotDatas.Add(sData);
            }

            return data;
        }

        // 根据快照恢复数据并刷新 UI
        public void RestoreFromSaveData(BagSaveData data)
        {
            if (data == null) return;

            // 1. 恢复布尔变量
            this.hasPickUpBattery = data.hasPickUpBattery;
            this.hasPickUpBox = data.hasPickUpBox;
            this.hasPickUpFusil = data.hasPickUpFusil;

            // 2. 恢复每个格子并刷新 UI 显示
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null) continue;

                if (i < data.slotDatas.Count && !string.IsNullOrEmpty(data.slotDatas[i].itemName) && data.slotDatas[i].quantity > 0)
                {
                    string targetName = data.slotDatas[i].itemName;
                    ItemSO matchedSO = itemDatabase.Find(item => item != null && item.itemName == targetName);

                    slots[i].itemSO = matchedSO;
                    slots[i].slotQuantity = data.slotDatas[i].quantity;
                }
                else
                {
                    slots[i].itemSO = null;
                    slots[i].slotQuantity = 0;
                }

                // 刷新该格子的图标、文字显示
                slots[i].UpdateUI();
            }
        }
    }


    // 纯数据结构（专门用于 ES3 序列化，严禁挂载 UI 或 Component 引用）
    [System.Serializable]
    public class SlotSaveData
    {
        public string itemName; // 保存物品的名字
        public int quantity;    // 保存数量
    }

    [System.Serializable]
    public class BagSaveData
    {
        public bool hasPickUpBattery;
        public bool hasPickUpBox;
        public bool hasPickUpFusil;
        public List<SlotSaveData> slotDatas = new List<SlotSaveData>();
    }
}


