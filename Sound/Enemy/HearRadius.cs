using UnityEngine;

namespace ns
{   
	///summary
	///听力半径脚本
	///summary 
	public class HearRadius : MonoBehaviour
	{

        public CanvasGroup canvasGroup;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                canvasGroup.alpha = 1;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canvasGroup.alpha = 0;
            }
        }
    }
}

