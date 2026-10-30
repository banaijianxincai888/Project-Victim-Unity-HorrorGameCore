using UnityEngine;
using UnityEngine.UI;

namespace ns
{   
	///summary
	///鼠标指示器管理	
	///summary 
	public class MouseUIManager : MonoBehaviour
	{
		public Image image;

		public RectTransform rectTransform;
		public float checkInterval = 0.2f;

		public Sprite normolImage;

		public float normolScale;

		public Color normolColor;

		public Sprite interactImage;

        public float interactlScale;

        public Color interactlColor;

		private float lastTime = 0f;

		public CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }



        private void Update()
        {
			if (Time.time - lastTime > checkInterval)
			{
				IsInteract();
			}
        }

		private void IsInteract()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            lastTime = Time.time;
            if (Physics.Raycast(ray, out RaycastHit hit,PlayerCore.Instance.playerCamera.checkDistance,PlayerCore.Instance.playerCamera.commonInteractLayer))
			{
				if(hit.transform.CompareTag(PlayerCore.Instance.playerCamera.interactTag))
				{
					image.sprite = interactImage;
					image.color = interactlColor;
					rectTransform.localScale = new Vector3(interactlScale,interactlScale, interactlScale);
				}
				else
				{
					image.sprite = normolImage;
					image.color = normolColor;
					rectTransform.localScale = new Vector3(normolScale,normolScale,normolScale);
				}
			}
		}

    }
}

