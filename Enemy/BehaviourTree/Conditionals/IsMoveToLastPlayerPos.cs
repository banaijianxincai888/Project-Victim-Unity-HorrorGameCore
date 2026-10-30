using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ns
{   
	///summary
	///是否移动到玩家最后位置
	///summary 
	public class IsMoveToLastPlayerPos :ConditionalBase
	{

        public override TaskStatus OnUpdate()
        {
			if (!enemyBlackBoard.hasMoveToLastPlayerPos)
				return TaskStatus.Success;
			return TaskStatus.Failure;
        }

  	}
}

