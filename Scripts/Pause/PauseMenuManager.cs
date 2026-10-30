using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace ns
{
    public class PauseMenuManager : MonoBehaviour
    {
        public ButtonEffect reloadLastSavePointButtonEffect;
        public CanvasGroup canvasGroup;

        public string StartMenuSceneName = "StartMenu";
        public float interval = 1f;

        private float lastPauseTime = 0f;

        public bool isShowPauseMenu;

        // 新增：记录当前运行的协程
        private Coroutine pauseAnimCoroutine;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            EventCenter.GameFlowEvent.OnGamePauseButton += ShowPauseMenu;
        }

        private void OnDisable()
        {
            EventCenter.GameFlowEvent.OnGamePauseButton -= ShowPauseMenu;
            // 防止退出时残留协程
            if (pauseAnimCoroutine != null) StopCoroutine(pauseAnimCoroutine);
        }

        private void ShowPauseMenu()
        {
            if (Time.unscaledTime - lastPauseTime < interval) return;

            isShowPauseMenu = !isShowPauseMenu;
            if (isShowPauseMenu)
                PauseMenuOn();
            else
                PauseMenuOff();

            lastPauseTime = Time.unscaledTime;
        }

        private void PauseMenuOff()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 注意：继续游戏时，立即恢复时间，让游戏逻辑恢复
            Time.timeScale = 1f;

            // 使用带防冲突的协程管理方法
            UIManager.Instance.AlphaSmoothChangeWithCor(canvasGroup, ref pauseAnimCoroutine, 1, 0, 0.5f, () =>
            {
                canvasGroup.blocksRaycasts = false;
               
            });

            //修复一下选中的按钮，防止误触发！
            // 在加载完成或关闭菜单时调用：
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void UpdateButton()
        {
            // 保持原本的逻辑不变
            if (ES3.FileExists(UIManager.Instance.coverUIManager.savePoint.checkpointFileName))
            {
                reloadLastSavePointButtonEffect.Button.interactable = true;
                Color32 color32 = new Color32(255, 255, 255, 255);
                reloadLastSavePointButtonEffect.Image.color = color32;
                reloadLastSavePointButtonEffect.ButtonText.color = color32;
            }
            else
            {
                reloadLastSavePointButtonEffect.Button.interactable = false;
                Color32 color32 = new Color32(255, 255, 255, 120);
                Color32 color321 = new Color32(255, 255, 255, 0);
                reloadLastSavePointButtonEffect.ButtonText.color = color32;
                reloadLastSavePointButtonEffect.Image.color = color321;
            }
        }
        private void PauseMenuOn()
        {
            UpdateButton();
            canvasGroup.blocksRaycasts = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 使用带防冲突的协程管理方法
            UIManager.Instance.AlphaSmoothChangeWithCor(canvasGroup, ref pauseAnimCoroutine, 0, 1, 0.5f, () =>
            {
                Time.timeScale = 0; // 动画结束后才暂停游戏
            });
        }

        public void ContinueGameButton()
        {
            // 按钮调用时，同步更改状态，防止通过按钮关闭后，再按ESC触发状态混乱
            isShowPauseMenu = false;
            PauseMenuOff();
        }

        public void ReloadLastSavePointButton()
        {
            // 1. 强制停止暂停菜单自己的协程，防止和读档动画冲突
            if (pauseAnimCoroutine != null) StopCoroutine(pauseAnimCoroutine);

            // 2. 重置暂停菜单状态
            isShowPauseMenu = false;
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;

            // 3. 唤醒 coverUI
            UIManager.Instance.coverUIManager.canvasGroup.alpha = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;

            //切换BGM
            BGMManager.Instance.ChangeBGM(BGMManager.Instance.commonBGM);
            PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.commonBreath);
            UIManager.Instance.coverUIManager.CancelCover();

        }

        public void SettingsButton()
        {
            UIManager.Instance.settingsUIManager.ShowSettings();
        }

        public void ReturnStartMenuButton()
        {
            BGMManager.Instance.ChangeBGM(BGMManager.Instance.commonBGM);
            PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.commonBreath);
            UIManager.Instance.coverUIManager.CoverAndLoadScene(StartMenuSceneName);

        }
     
    }
}