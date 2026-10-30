using UnityEngine;

namespace ns
{   
	///summary
	///灰色钥匙战利品
	///summary 
	public class LootGateKey : Loot
	{
        public Decryption_CBreaker cBreaker;

        public QuestTipSO questTipSO;

        public override void AfterMethod()
        {
            cBreaker.TurnOffLights();
            UIManager.Instance.questTipManager.ShowQuestTip(questTipSO);

        }

  	}
}

