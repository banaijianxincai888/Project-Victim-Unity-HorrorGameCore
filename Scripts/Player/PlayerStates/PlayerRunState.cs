using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///玩家跑步状态
    ///summary 
    public class PlayerRunState : PlayerStateBase
    {
        public PlayerRunState(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override void EnterState()
        {
            playerMovement.SetCameraRun();
            PlayerCore.Instance.playerCamera.noise.m_NoiseProfile = PlayerCore.Instance.playerCamera.runNoiseSettings;
            PlayerCore.Instance.anim.SetBool(PlayerCore.Instance.animPara.runAnim, true);

            if (!playerMovement.enemyBlackBoard.hasSeen)
            {
                PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.runBreath);
            }
        }

        public override void ExitState()
        {
            PlayerCore.Instance.anim.SetBool(PlayerCore.Instance.animPara.runAnim, false);
            PlayerCore.Instance.playerCamera.noise.m_NoiseProfile = PlayerCore.Instance.playerCamera.walknoiseSettings;
            playerMovement.RecoverStamina();

        }

        public override void UpdateState()
        {
           
            playerMovement.SetCommonCameraPosition();

            playerMovement. CostStamina();
        }


    }
}

