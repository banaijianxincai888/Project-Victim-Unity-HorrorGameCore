using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ns
{   
	///summary
	///是否在搜寻
	///summary 
	public class IsSearching : ConditionalBase
	{
       

        public override TaskStatus OnUpdate()
        {
            if (enemyBlackBoard.hasLostPlayer && ! enemyBlackBoard.searchCompleted) return TaskStatus.Success;
            return TaskStatus.Failure;
        }


       
    }
}

