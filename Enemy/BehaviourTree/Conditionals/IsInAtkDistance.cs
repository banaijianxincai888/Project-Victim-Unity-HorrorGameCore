using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ns
{   
	///summary
	///是否进入了攻击范围
	///summary 
	public class IsInAtkDistance : ConditionalBase
	{

		

        public override TaskStatus OnUpdate()
        {
            if (Vector3.Distance(enemyBlackBoard.player.position, transform.position) >enemyBlackBoard. atkDistance || 
                PlayerCore.Instance.playerMovement.currentHideTransform!=null || !enemyBlackBoard.hasSeen
                )
                return TaskStatus.Failure;

           
            return TaskStatus.Success;

        }

        public override void OnDrawGizmos()
        {
            //Handles.color = Color.yellow;
            //Handles.DrawWireDisc(sharedTransform.Value.position, Vector3.up, enemyBlackBoard. atkDistance);



        }


    }
}

