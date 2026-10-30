using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns
{   
	///summary
	///设置UI管理器
	///summary 
	public class SettingsUIManager : MonoBehaviour
	{
		private CanvasGroup canvasGroup;

		private Coroutine coroutine;

        public AK.Wwise.RTPC globalVolumeRTPC;

		public Slider globalVolume;

		public Slider mouseSensitivity;
        [Header("分辨率设置")]
        public TMP_Dropdown resolutionDropdown; // 2. 将类型改为 TMP_Dropdown
        private Resolution[] resolutions;


        private void Start()
        {
			canvasGroup = GetComponent<CanvasGroup>();
			SetGlobalVolume();
			SetMouseSensitivity();
            InitResolutions();
        }

		public void ShowSettings()
		{
            canvasGroup.interactable = true;
            UIManager.Instance.AlphaSmoothChangeWithCor(canvasGroup, ref coroutine, 0, 1, 0.5f, () => {
				canvasGroup.blocksRaycasts = true;
			});
		}

		public void CancelSettings()
		{
            UIManager.Instance.AlphaSmoothChangeWithCor(canvasGroup, ref coroutine, 1,0, 0.5f, () =>
			{
				canvasGroup.blocksRaycasts = false;
				canvasGroup.interactable = false;
            });

        }

        public void SetGlobalVolume()
		{
			globalVolumeRTPC.SetGlobalValue(globalVolume.value);
		}

		public void SetMouseSensitivity()
		{
            PlayerCore.Instance.playerMovement.rotateSpeed = mouseSensitivity.value;
        }

        // 初始化分辨率选项
        private void InitResolutions()
        {
            if (resolutionDropdown == null) return;

            resolutionDropdown.ClearOptions();
            resolutions = Screen.resolutions;

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                // 匹配当前屏幕的分辨率作为默认选中项
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            // 绑定事件
            resolutionDropdown.onValueChanged.AddListener(SetResolutionByIndex);
        }

        // 对应 Dropdown 传递参数的方法
        public void SetResolutionByIndex(int resolutionIndex)
        {
            Resolution selectedResolution = resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
            // 打印日志确认
            Debug.Log($"已切换分辨率为: {selectedResolution.width} x {selectedResolution.height}，当前全屏状态: {Screen.fullScreen}");
        }

       


    }
}

