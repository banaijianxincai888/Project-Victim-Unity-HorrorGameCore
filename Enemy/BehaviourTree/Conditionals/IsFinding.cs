using BehaviorDesigner.Runtime.Tasks;

namespace ns
{
    public class IsFinding : ConditionalBase
    {
        public override TaskStatus OnUpdate()
        {
            // 黑板的 Update 已经全自动更新 hasSeen 了，这里直接看结果就行！
            return enemyBlackBoard.hasSeen ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}