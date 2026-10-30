using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace ns
{   
	///summary
	///夜视仪UI管理器
	///summary 
	public class NightVisionManager :MonoBehaviour
	{
		public Slider battery;

		public Image fillArea;

		public Color originalColor;

		public Color lowPowerColor;



		public ItemSO batteryItem;

		public float batteryCostTime = 5f;

		private Coroutine coroutine;

		private bool isColorChange;


		public CanvasGroup canvasGroup;

        private void Start()
        {
			canvasGroup = GetComponent<CanvasGroup>();
        }

        public void UseBattery(Volume volume, Action value0Method)
		{
			if (battery.value < 0) return;
			coroutine = StartCoroutine(CostBattery(volume,value0Method));
		}


		IEnumerator CostBattery(Volume volume,Action value0Method)
		{
			float t = 1 / batteryCostTime;
            while (battery.value > 0)
			{
				battery.value -= t * Time.deltaTime;
				if (battery.value <= 0.3f && !isColorChange)
				{
					fillArea.color = lowPowerColor;
					SoundManager.Instance.powerLow.Post(gameObject);
					isColorChange = true;

                }
                yield return null;
			}
			battery.value = 0;
			volume.weight = 0;

			canvasGroup.alpha = 0;
			value0Method?.Invoke();

		}

		public void StopCostBattery()
		{
			if(coroutine  != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
		}

		public void RecoverBattery()
		{
			if(UIManager.Instance.bagManager.HasItem(batteryItem))
			{
				SoundManager.Instance.reload.Post(gameObject);
                UIManager.Instance.bagManager.UseItem(batteryItem,1);
				fillArea.color = originalColor;
				isColorChange = false;
				battery.value = 1f;
			}
		}

		

  	}
}

