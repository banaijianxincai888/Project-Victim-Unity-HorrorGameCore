using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ns
{   
	///summary
	///踢门行为
	///summary 
	public class KickDoor :ActionsBase
	{




        public override void OnStart()
        {
            base.OnStart();
            enemyBlackBoard.door.count = 0;
            enemyBlackBoard. anim.SetBool(enemyBlackBoard. zombieAnimEvent.zombieAnimPara.walkPara, true);
            enemyBlackBoard.door.kickPoint = enemyBlackBoard.door.SelectKickPoint(transform);
        }

        public override TaskStatus OnUpdate()
        {
            if (enemyBlackBoard .door.count >= enemyBlackBoard .door.kickCount)
            {
                enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.kickDoorPara, false);

                return TaskStatus.Success;

            }

            if ((DistanceXZ(transform.position,enemyBlackBoard. door.kickPoint.position) < 0.5f ) )
            {
                enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, false);
                RotateToDirection(enemyBlackBoard. door.kickPoint.rotation);
                if (Vector3.Angle(transform.forward,enemyBlackBoard. door.kickPoint.forward) <5f)
                    enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.kickDoorPara, true);
                return TaskStatus.Running;

            }
            else
            {
                AgentRotate(enemyBlackBoard. door.kickPoint.position);
                return TaskStatus.Running;
            }
          
        }

      

        public override void OnEnd()
        {
            base.OnEnd();
            enemyBlackBoard.isDoorBreaking = false;
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, false);

            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.kickDoorPara, false);
        }

    }
}

