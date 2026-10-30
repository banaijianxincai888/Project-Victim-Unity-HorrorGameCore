using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace ns
{   
	///summary
	///发现玩家躲藏！！！
	///summary 
	public class FindPlayerHide :ActionsBase
	{
        private HideoutInfo hideoutInfo;
        private PlayableDirector playableDirector;

        public override void OnStart()
        {
            hideoutInfo = PlayerCore.Instance.playerMovement.currentHideTransform.GetComponent<ISearchHideout>().SearchHideoutPoint();
            enemyBlackBoard.soundEmission.SoundEmission(enemyBlackBoard.soundEmission.findHideDialogueSOs);


        }
        //public override TaskStatus OnUpdate()
        //{
        //    playableDirector = hideoutInfo.findDirector;
        //    playableDirector.Play();
        //    return TaskStatus.Success;
        //}



    }
}

