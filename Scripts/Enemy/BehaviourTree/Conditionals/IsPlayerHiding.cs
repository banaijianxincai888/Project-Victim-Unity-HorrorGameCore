using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ns
{   
	///summary
	///是否看到玩家躲藏条件
	///summary 
	public class IsPlayerHiding :ConditionalBase
	{
        public override TaskStatus OnUpdate()
        {
           if(PlayerCore.Instance.playerMovement.currentHideTransform!=null)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

  	}
}

