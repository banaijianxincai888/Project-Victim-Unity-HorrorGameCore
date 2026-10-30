using UnityEngine;

namespace ns
{
    ///summary
    ///躲藏进桌下
    ///summary 
    public class Desk : Hideout,IInteractable,ISearchHideout
    {

        public void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                hideoutInfo. isInside  = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                hideoutInfo. isInside   = false;
            }
        }

        public HideoutInfo SearchHideoutPoint()
        {
            return hideoutInfo;
        }

        public  void TryInteract()
        {
            if(!hideoutInfo. isInside) { return; }
            if(hideoutInfo.isAIInteract) { return; }
            ActiveHideState();
        }
    }
}

