using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ns
{   
	///summary
	///设置陷阱
	///summary 
	public class SetTrap : ActionsBase
	{

        public override void OnStart()
        {
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.setTrapPara, true);

        }

        public override TaskStatus OnUpdate()
        {
            if(!enemyBlackBoard.anim.GetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.setTrapPara))
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.setTrapPara, false);

        }
    }
}

