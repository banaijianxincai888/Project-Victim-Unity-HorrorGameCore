using TMPEffects.Components;
using UnityEngine;
using UnityEngine.UI;

namespace ns
{   
	///summary
	///退出陷阱管理器
	///summary 
	public class ExitTrapManager : MonoBehaviour
	{

		public Slider progress;

		public CanvasGroup canvasGroup;

		public TMPAnimator tMPAnimator;

		[Range(0f, 0.1f)]
		public float pullRotate;

        private void Start()
        {
			canvasGroup = GetComponent<CanvasGroup>();
			tMPAnimator.StopAnimating();
        }

		public void ShowExitTrapTip()
		{
			canvasGroup.alpha = 1.0f;
			progress.value = 0.0f;
			tMPAnimator.StartAnimating();
		}

		public void PushProgress(float value)
		{
			progress.value += value;
		}

        public void PullProgress()
        {
            // 把每帧系数换算成考虑 deltaTime 的等效系数
            float t = 1f - Mathf.Pow(1f - Mathf.Clamp01(pullRotate), Time.deltaTime * 60f);
            progress.value = Mathf.Lerp(progress.value, 0f, t);
        }

        public void CancelExitTrapTip()
		{
			canvasGroup.alpha = 0f;
            tMPAnimator.StopAnimating ();
        }
    }
}

