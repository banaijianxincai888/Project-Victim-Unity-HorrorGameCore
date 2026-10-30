using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace ns
{   
	///summary
	///躲藏点基类
	///summary 
	public class Hideout : MonoBehaviour
    {

        public HideoutInfo hideoutInfo;


        public void ActiveHideState()
        {



            if (PlayerCore.Instance.playerMovement.isHiding)
            {
                PlayerCore.Instance.playerMovement.isHiding = false;
                PlayerCore.Instance.playerMovement.currentHideTransform = null;
            }
            else
            {
                PlayerCore.Instance.playerMovement.isHiding = true;
                PlayerCore.Instance.playerMovement.currentHideTransform = transform;


            }
        }
      

        //private void OnDrawGizmos()
        //{
        //    Handles.color = Color.black;
        //}

    }
}

