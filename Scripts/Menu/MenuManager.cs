using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace ns
{   
	///summary
	///菜单管理器
	///summary 
	public class MenuManager : MonoBehaviour
	{
		public ButtonEffect continueButtonEffect;

		public CanvasGroup coverUIGroup;
		public StartSceneManager startSceneManager;
		public PlayableDirector director;

		private CanvasGroup canvasGroup;

        public CanvasGroup thankUIGroup;

		public string saveFileName = "GameData/SaveData/checkpoint.es3";

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
           
			UpdateButton();
        }

        public void UpdateButton()
        {
            if (ES3.FileExists(saveFileName))
            {
                continueButtonEffect.Button.interactable = true;
				Color32 color32 = new Color32(255, 255, 255, 255);
                continueButtonEffect.Image.color = color32;
                continueButtonEffect.ButtonText.color = color32;
            }
			else
			{
                continueButtonEffect.Button.interactable = false;
                Color32 color32 = new Color32(255, 255, 255, 120);
				Color32 color321  = new Color32(255, 255, 255, 0);
                continueButtonEffect.ButtonText.color = color32;
                continueButtonEffect.Image.color = color321;

            }
        }

        public void ContinueButtonMethod()
		{
			StartCoroutine(AlphaSmoothChangeAndContinue(coverUIGroup, 0, 1, 0.5f,()=> startSceneManager.StartGame()));
		}

		public void NewGameButtonMethod()
		{
			if(ES3.FileExists(saveFileName))
				ES3.DeleteFile(saveFileName);
			canvasGroup.blocksRaycasts = false;
			director.Play();
		}

	

		public void ThankButtonMethod()
		{
			thankUIGroup.blocksRaycasts = true;
            StartCoroutine(AlphaSmoothChangeAndContinue(thankUIGroup, 0, 1, 0.5f));
		}

        public void ReturnMethod()
        {
            thankUIGroup.blocksRaycasts = false;
            StartCoroutine(AlphaSmoothChangeAndContinue(thankUIGroup, 1,0, 0.5f));
        }

        public void ExitGameButtonMethod()
		{
			Application.Quit();
		}

        public IEnumerator AlphaSmoothChangeAndContinue(CanvasGroup canvasGroup, float start, float end, float time,Action affterMethod = null)
        {
            float currentTime = 0f;
            while (currentTime < time)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / time;
                t = Mathf.Clamp01(t);
                canvasGroup.alpha = Mathf.Lerp(start, end, t);
                yield return null;

            }
            canvasGroup.alpha = end;
            affterMethod?.Invoke();

        }
    }
	[Serializable]
	public class ButtonEffect
	{
        public Button Button;
        public TMP_Text ButtonText;

        public Image Image;
    }
}

