using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ns
{   
	///summary
	///条件黑板变量类
	///summary 
	public class ConditionalBase:Conditional
	{
        public SharedTransform sharedTransform;

        protected EnemyBlackBoard enemyBlackBoard;
        public override void OnAwake()
        {
            enemyBlackBoard = GetComponent<EnemyBlackBoard>();
        }

    }
}

