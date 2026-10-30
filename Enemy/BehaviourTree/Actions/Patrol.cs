using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace ns
{
    public class Patrol : ActionsBase
    {
        public float speakTime = 30f;

        private float speakTimer;
        private int index = 0;
        public bool waitingForAlert = false;   // 是否正在等待警觉动画结束

        public override void OnStart()
        {
            waitingForAlert = false;
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, true);

            if(UIManager.Instance.dialogueManager.currentDialogueSO == null)
            {
                enemyBlackBoard.soundEmission.SoundEmission(enemyBlackBoard.soundEmission.pathcrolDialogueSOs);
                speakTimer = Time.time;
            }
           
        }



        public override TaskStatus OnUpdate()
        {
            if(Time.time - speakTimer > speakTime)
            {
                enemyBlackBoard.soundEmission.SoundEmission(enemyBlackBoard.soundEmission.pathcrolDialogueSOs);
                speakTimer = Time.time;
            }

            if (waitingForAlert)
            {
                // 等待警觉动画播放完毕（alarmPara 被动画状态机或事件重置为 false）
                if (!enemyBlackBoard.anim.GetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.alarmPara))
                {
                    // 检查是否最后一个点
                    if (index ==enemyBlackBoard.  checkPoints.Length - 1)
                    {
                        // 最后一个点：禁用并重生（根据你的逻辑）
                        index = 0;
                        enemyBlackBoard.spawnManager.RandomSpawn(enemyBlackBoard.spawnManager.spawnTime);
                        return TaskStatus.Success;   // 任务结束
                    }
                    else
                    {
                        // 走向下一个点
                        index = (index + 1) % enemyBlackBoard. checkPoints.Length;
                        enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, true);
                        waitingForAlert = false;   // 结束等待

                    }
                }
                return TaskStatus.Running;
            }
            else
            {
                // 正常巡逻移动
                if (Vector3.Distance(transform.position,enemyBlackBoard. checkPoints[index].position) <= 0.5f)
                {
                    // 到达点：触发警觉，进入等待
                    enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.alarmPara, true);
                    enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, false);
                    waitingForAlert = true;
                }
                else
                {
                    AgentRotate(enemyBlackBoard. checkPoints[index].position);   
                }
                return TaskStatus.Running;
            }
        }

        public override void OnEnd()
        {
             enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.walkPara, false);
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.alarmPara, false);

        }
    }


}