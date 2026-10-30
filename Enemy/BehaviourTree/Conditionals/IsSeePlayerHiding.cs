using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ns
{   
	///summary
	///如果玩家当面进柜子
	///summary 
	public class IsSeePlayerHiding : ConditionalBase
	{
        public override TaskStatus OnUpdate()
        {
            if (PlayerCore.Instance.playerMovement.currentHideTransform!=null && !enemyBlackBoard.hasLostPlayer)
                return TaskStatus.Success;
            return TaskStatus.Failure;
        }

  	}
}

