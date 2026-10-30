using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns
{   
	///summary
	///死亡UI管理器
	///summary 
	public class DeathUIManager : MonoBehaviour
	{
		public CanvasGroup canvasGroup;

		private Coroutine coroutine;

		public float startTime = 5f;

		public string StartMenuSceneName = "StartMenu";

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

		public void ShowDeathUI()
		{
			StartCoroutine(WaitForUI());
          
		}

		IEnumerator WaitForUI()
		{
			yield return new WaitForSeconds(startTime);
            UIManager.Instance.mouseUIManager.canvasGroup.alpha = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Instance.AlphaSmoothChangeWithCor(canvasGroup, ref coroutine, 0, 1, 2f, () =>
            canvasGroup.blocksRaycasts = true);
        }

		public void CancelDeathUI()
		{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.alpha = 0;

            //修复一下选中的按钮，防止误触发！
            // 在加载完成或关闭菜单时调用：
            EventSystem.current.SetSelectedGameObject(null);

        }

		public void ReloadLastSavePoint()
		{
            //恢复玩家死亡状态
            PlayerCore.Instance.playerMovement.isDeath = false;
            PlayerCore.Instance.playerMovement.isShaking = false;
            PlayerCore.Instance.anim.SetTrigger(PlayerCore.Instance.animPara.resetAnim);

			CancelDeathUI();
            UIManager.Instance.mouseUIManager.canvasGroup.alpha = 1f;
            UIManager.Instance.coverUIManager.canvasGroup.alpha = 1f;
            UIManager.Instance.coverUIManager.CancelCover();

        }

        public void ReturnStartMenuButton()
        {
            UIManager.Instance.coverUIManager.CoverAndLoadScene(StartMenuSceneName);

        }

    }
}

