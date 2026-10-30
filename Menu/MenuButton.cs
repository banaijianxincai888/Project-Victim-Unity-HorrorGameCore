using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns
{   
	///summary
	///菜单按钮
	///summary 
	public class MenuButton : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler
	{
		public AK.Wwise.Event pointEnterEvent;

		public AK.Wwise.Event pointClickEvent;

		public Button button;

        private void Start()
        {
            button = GetComponent<Button>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(button.interactable)
			pointClickEvent.Post(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (button.interactable)
                pointEnterEvent.Post(gameObject);
        }
    }
}

