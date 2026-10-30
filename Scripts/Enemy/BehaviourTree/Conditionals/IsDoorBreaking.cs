using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace ns
{
    public class IsDoorBreaking : ConditionalBase
    {
        //破门注意事项
        //     铁律 1：只要我的射线直接看到了玩家，我跟玩家之间就绝对没门！
        //- 道理很简单：你的视野射线是会被门挡住的。既然射线能直接射中玩家，说明你们俩之间是通畅的（比如玩家就在你身后），这时候根本不需要踢任何门！
        //- 写在哪里：写在 EnemyBlackBoard.cs 的 OnPlayerDetected()（发现玩家）方法里。一旦看到玩家，立刻把黑板上的“踢门指令”擦掉（isDoorBreaking = false）。
        //💡 铁律 2：在准备踢门前，先看一眼玩家在哪个方向！
        //- 道理很简单：如果 AI 正在追玩家（比如玩家刚跑过拐角，AI 还在 3 秒的追击记忆里），这时候 AI 路过一扇门。这扇门到底该不该踢？
        //  - 如果玩家在门的那一头（门挡住了去路） $\rightarrow$ 踢！
        //  - 如果玩家在 AI 身后，而门在 AI 面前（两个方向完全相反） $\rightarrow$ 不踢！
        //  - 如果玩家离 AI 只有 1 米，门离 AI 有 2 米（玩家比门还近，根本没隔着门） $\rightarrow$ 不踢！
        //- 写在哪里：写在 isDoorBreaking.cs（判断要不要踢门的条件节点）和 Door.cs（门的触发器）里。用向量点积（Vector3.Dot）对比一下【AI到门】和【AI到玩家】的方向即可。
        //💡 铁律 3：如果 AI 走开了，门就别再缠着 AI 了！
        //- 道理很简单：万一 AI 进了门的触发器，还没踢完门就被玩家吸引走了（离开了触发器），门必须把 isDoorBreaking 重新设回 false。
        //- 写在哪里：写在 Door.cs 新增的 OnTriggerExit（离开触发器）方法里。
        public override TaskStatus OnUpdate()
        {
            // 1. 如果没触发破门（或者刚被黑板因为看到玩家而自动清掉了），直接返回 Failure
            if (!enemyBlackBoard.isDoorBreaking || enemyBlackBoard.door == null || enemyBlackBoard.door.isOpen)
            {
                enemyBlackBoard.ClearDoorBreak();
                return TaskStatus.Failure;
            }

            // 2. 如果处于追击状态（比如玩家刚跑进拐角那 3 秒内），检查门是不是挡在去路上
            if (enemyBlackBoard.hasSeen && enemyBlackBoard.player != null)
            {
                Vector3 toDoor = enemyBlackBoard.door.center.position - transform.position;
                toDoor.y = 0f;

                Vector3 toPlayer = enemyBlackBoard.player.position - transform.position;
                toPlayer.y = 0f;

                if (Vector3.Dot(toDoor.normalized, toPlayer.normalized) < 0.3f || toPlayer.magnitude < toDoor.magnitude)
                {
                    enemyBlackBoard.ClearDoorBreak();
                    return TaskStatus.Failure;
                }
            }

            return TaskStatus.Success;
        }
    }
    }
