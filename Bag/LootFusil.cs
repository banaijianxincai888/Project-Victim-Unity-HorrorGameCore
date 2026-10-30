using UnityEngine;

namespace ns
{   
	///summary
	///保险丝战利品
	///summary 
	public class LootFusil :Loot
	{
        public Decryption_CBreaker decryption_CBreaker;

        public EnemySpawnManager enemySpawnManager;

        public QuestTipSO questTipSO;

        public Door door;
        public override void AfterMethod()
        {
            if (UIManager.Instance.bagManager.hasPickUpFusil) return;
            UIManager.Instance.bagManager.hasPickUpFusil = true;
            decryption_CBreaker.boxCollider.enabled = true;
            decryption_CBreaker.TurnOffLights();
            UIManager.Instance.questTipManager.ShowQuestTip(questTipSO);
            door.isAIInteract = true;
            door.isOpen = false;
            StartCoroutine(door.DoorControl(PlayerCore.Instance.transform, door.doorBreakTime, false));
            enemySpawnManager.ForceSpawn();

        }

  	}
}

