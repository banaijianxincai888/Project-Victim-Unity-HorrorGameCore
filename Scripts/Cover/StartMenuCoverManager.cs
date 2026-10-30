using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace ns
{   
	///summary
	///开始菜单遮挡管理器
	///summary 
	public class StartMenuCoverManager : MonoBehaviour
	{
        CanvasGroup canvasGroup;


        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            StartCoroutine(AlphaSmoothChange(canvasGroup, 1, 0, 2f));
        }

        public IEnumerator AlphaSmoothChange(CanvasGroup canvasGroup, float start, float end, float time)
        {
            yield return new WaitForSeconds(1f);
            float currentTime = 0f;
            while (currentTime < time)
            {
                currentTime += Time.unscaledDeltaTime;
                float t = currentTime / time;
                t = Mathf.Clamp01(t);
                canvasGroup.alpha = Mathf.Lerp(start, end, t);
                yield return null;

            }
            canvasGroup.alpha = end;
        }
    }
}

