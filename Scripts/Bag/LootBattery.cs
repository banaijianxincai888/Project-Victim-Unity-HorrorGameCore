using UnityEngine;

namespace ns
{   
	///summary
	///电池战利品
	///summary 
	public class LootBattery :Loot
	{
        public QuestTipSO firstTipForBattery;

       

        public override void AfterMethod()
        {
            if (UIManager.Instance.bagManager.hasPickUpBattery) return;
            UIManager.Instance.questTipManager.ShowQuestTip(firstTipForBattery);
            UIManager.Instance.bagManager.hasPickUpBattery = true;

        }
    }
}

