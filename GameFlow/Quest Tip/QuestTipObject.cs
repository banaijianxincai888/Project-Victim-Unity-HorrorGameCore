using UnityEngine;

namespace ns
{   
	///summary
	///任务提示对象
	///summary 
	public class QuestTipObject : MonoBehaviour
	{

        public QuestTipSO questTipSO;


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                UIManager.Instance.questTipManager.ShowQuestTip(questTipSO);
                this.gameObject.SetActive(false);
            }
        }

    }
}

