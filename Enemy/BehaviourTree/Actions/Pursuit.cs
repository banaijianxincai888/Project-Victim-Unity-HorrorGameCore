using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ns
{   
	///summary
	///追逐
	///summary 
	public class Pursuit : ActionsBase
	{

      

        public override void OnStart()
        {
            base.OnStart();

            enemyBlackBoard. anim.SetBool(enemyBlackBoard. zombieAnimEvent.zombieAnimPara.pursuitPara, true);
            if(!enemyBlackBoard.soundEmission.hasPlayPursuit)
            {
                enemyBlackBoard.soundEmission.SoundEmission(enemyBlackBoard.soundEmission.findDialogueSOs);
                enemyBlackBoard.soundEmission.hasPlayPursuit = true;
                if (BGMManager.Instance.currentBGM != BGMManager.Instance.pursuitBGM)
                {
                    BGMManager.Instance.ChangeBGM(BGMManager.Instance.pursuitBGM);
                    PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.isFound);
                    StartCoroutine(WaitForPursuitSound());
                }
            }


        }


        public override TaskStatus OnUpdate()
        {

          

            AgentRotate(enemyBlackBoard.player.position);

            return TaskStatus.Running;


        }

        IEnumerator WaitForPursuitSound()
        {
            yield return new WaitForSeconds(1.5f);
            PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.isPursuit);

        }

        public override void OnEnd()
        {
            enemyBlackBoard. agent.ResetPath();
            enemyBlackBoard. anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.pursuitPara, false);

        }



    }
}

