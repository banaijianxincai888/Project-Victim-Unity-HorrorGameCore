using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{   
	///summary
	///攻击行为
	///summary 
	public class Attack : ActionsBase
	{

        private float lastAtkTime = 0f;



        public override void OnStart()
        {
            if(BGMManager.Instance.currentBGM != BGMManager.Instance.pursuitBGM)
            {
                BGMManager.Instance.ChangeBGM(BGMManager.Instance.pursuitBGM);
                PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.isFound);
                StartCoroutine(WaitForPursuitSound());

            }
            if (PlayerCore.Instance.playerMovement.isCaught)
            {
                enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.atkPara, false);
            }
            else
            {
                enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.atkPara, true);

                lastAtkTime = Time.time;
                enemyBlackBoard.soundEmission.SoundEmission(enemyBlackBoard.soundEmission.attackDialogueSOs);

            }

        }

        IEnumerator WaitForPursuitSound()
        {
            yield return new WaitForSeconds(1.5f);
            PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.isPursuit);

        }

        public override TaskStatus OnUpdate()
        {
                    
          
            if(PlayerCore.Instance.playerMovement.isDeath)
            {

                enemyBlackBoard.soundEmission.SoundEmission(enemyBlackBoard.soundEmission.killDialogueSOs);
                enemyBlackBoard.tree.DisableBehavior();

                return TaskStatus.Success;
            }

            if (!enemyBlackBoard.anim.GetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.atkPara))
            {
                // 动画已结束 → 检查冷却是否完成
                if (Time.time - lastAtkTime >= enemyBlackBoard. coolTime)
                {
                    // 冷却完成，任务结束（成功）
                    return TaskStatus.Success;
                }
               
            }
            AgentRotate(enemyBlackBoard.player.position);
            return TaskStatus.Running;

        }

        public override void OnEnd()
        {
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.atkPara, false);

        }

     
    }
}

