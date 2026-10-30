using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace ns
{
	///summary
	///游戏开始脚本
	///summary 
	public class GameStart : MonoBehaviour
	{
		public GameObject Player;
		public PlayableDirector startDirector;

		public Light[] lights;

		public AK.Wwise.Event lightOff;

		public QuestTipSO startTip;

		public bool isStart;

		private void Start()
		{
			startDirector = GetComponent<PlayableDirector>();
		}

        private void OnEnable()
        {
			EventCenter.GameFlowEvent.OnGameStartButton += GameStartButton;
        }

        private void OnDisable()
        {
			EventCenter.GameFlowEvent.OnGameStartButton -= GameStartButton;

        }

        public void StartGame()
		{
			foreach (var light in lights)
			{
				light.enabled = false;
			}
            UIManager.Instance.mouseUIManager.canvasGroup.alpha = 1;
			UIManager.Instance.coverUIManager.CancelBlockRaycasts();
            lightOff.Post(gameObject);
			UIManager.Instance.questTipManager.ShowQuestTip(startTip);
			StartCoroutine(SetParentNull());
		}

		IEnumerator SetParentNull()
		{
			yield return new WaitForSeconds(10f);
            Player.transform.SetParent(null, true);

        }

		public void GameStartButton()
		{
			if(!isStart)
			{
                startDirector.Play();
				isStart = true;
            }
        }
    }
}

