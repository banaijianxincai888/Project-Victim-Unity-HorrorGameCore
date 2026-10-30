using TMPro;
using UnityEngine;

namespace ns
{   
	///summary
	///任务提示管理器
	///summary 
	public class QuestTipManager : MonoBehaviour
	{
		public QuestTipSO currentQuestTip;

		public TMP_Text text;

		public CanvasGroup canvasGroup;

		private Coroutine coroutine;

		public AK.Wwise.Event tipEvent;


        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowQuestTip(QuestTipSO currentQuestTip)
		{
			tipEvent.Post(gameObject);
			text.text = currentQuestTip.description;
			UIManager.Instance.SharpShowAndFadeWithCor(canvasGroup,ref coroutine, 5,0.3f);
		}

  	}
}

