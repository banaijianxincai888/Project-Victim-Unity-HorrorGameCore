using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ns
{
    /// <summary>
    /// 移动到玩家最后被看到的位置
    /// </summary>
    public class MoveToLastPlayerPos : ActionsBase
    {
        private bool waitAnimOver;

        public override void OnStart()
        {
            waitAnimOver = false;
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, true);
            enemyBlackBoard.soundEmission.SoundEmission(enemyBlackBoard.soundEmission.lostDialogueSOs);

        }

        public override TaskStatus OnUpdate()
        {
            // 1. 等待到达后的动画结束
            if (waitAnimOver)
            {
                if (!enemyBlackBoard.anim.GetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.findLastPosPara))
                {
                    return TaskStatus.Success;
                }
                return TaskStatus.Running;
            }

            // 2. 未到达时移动
            if (DistanceXZ(enemyBlackBoard.lastSeenPosition, transform.position) > 0.1f)
            {
                AgentRotate(enemyBlackBoard.lastSeenPosition);
                return TaskStatus.Running;
            }

            // 3. 到达：停止移动，播放动画
            enemyBlackBoard.hasMoveToLastPlayerPos = true;
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, false);
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.findLastPosPara, true);
            waitAnimOver = true;
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, false);
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.findLastPosPara, false);
        }
    }
}