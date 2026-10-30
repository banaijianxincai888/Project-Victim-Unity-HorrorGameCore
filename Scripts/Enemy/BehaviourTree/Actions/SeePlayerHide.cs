using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace ns
{   
	///summary
	///看到玩家躲藏行为
	///summary 
	public class SeePlayerHide :ActionsBase
	{
        public override void OnStart()
        {
            enemyBlackBoard.  currentHideInfo = PlayerCore.Instance.playerMovement.currentHideTransform.GetComponent<ISearchHideout>().SearchHideoutPoint();
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, true);
            enemyBlackBoard.timer = Time.time;
        }



        public override TaskStatus OnUpdate()
        {
            enemyBlackBoard.currentHideInfo.target = enemyBlackBoard.player;
            if (DistanceXZ(transform.position, enemyBlackBoard.  currentHideInfo.target.position) >= 0.5f)
            {
                AgentRotate(enemyBlackBoard.  currentHideInfo.target.position);
                Debug.Log("ING");
                return TaskStatus.Running;
            }
            else
            {
                enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, false);
                string para = enemyBlackBoard.  currentHideInfo.hideoutType switch
                {
                    HideoutType.Desk => enemyBlackBoard.zombieAnimEvent.zombieAnimPara.searchPara[0],
                    HideoutType.Cabinet => enemyBlackBoard.zombieAnimEvent.zombieAnimPara.searchPara[1],
                    _ => string.Empty
                };
                enemyBlackBoard.anim.SetBool(para, true);

                return TaskStatus.Success;
            }
            
        }

      

       

    }
}

