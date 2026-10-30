using System;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{   
	///summary
	///UI数据保存脚本
	///summary 
	public class SaveUIData : MonoBehaviour
	{
        [Serializable]
        public class SlotSaveData
        {
            public string itemSOName; // 记录道具SO的名字（用于还原）
            public int quantity;      // 道具数量
        }

        [Serializable]
        public class BagSaveData
        {
            // 1. 布尔状态
            public bool hasPickUpBattery;
            public bool hasPickUpBox;
            public bool hasPickUpFusil;

            // 2. 四个格子的纯数据
            public List<SlotSaveData> slotDatas = new List<SlotSaveData>();
        }

    }
}

