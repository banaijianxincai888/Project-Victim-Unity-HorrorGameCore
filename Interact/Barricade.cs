using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace ns
{   
	///summary
	///翻越障碍物
	///summary 
	public class Barricade : MonoBehaviour,IInteractable
	{
		public PlayableDirector vaultDirector;



		public  bool isIn;

        private void Start()
        {
            vaultDirector = GetComponent<PlayableDirector>();
        }

        public void TryInteract()
        {
            if(isIn)
            {
                PlayerCore.Instance.anim.SetTrigger(PlayerCore.Instance.animPara.vaultAnim);
                vaultDirector.Play();

            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
			if (other.CompareTag("Player")) 
			isIn = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                isIn = false;
            
        }

       



    }
}

