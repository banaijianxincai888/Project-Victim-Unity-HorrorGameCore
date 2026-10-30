using UnityEngine;
using UnityEngine.UI;

namespace ns
{   
	///summary
	///血液管理器
	///summary 
	public class BloodManager : MonoBehaviour
	{

		public Image bloodImage;

		public Sprite blood75;

		public Sprite blood50;

		public Sprite blood25;

		public CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetBloodUI(Sprite sprite)
		{
			bloodImage.sprite = sprite;
		}

  	}
}

