using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///警觉行为
    ///summary 
    public class Alarm : ActionsBase
    {
        public override void OnStart()
        {
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.hearPara, true);
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            if (!enemyBlackBoard.anim.GetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.hearPara))
                return TaskStatus.Success;
            return TaskStatus.Running;

        }

        //public override void OnEnd()
        //{
        //    enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.hearPara, false);
        //    enemyBlackBoard.hasHeard = true;
        //}


    }
}

