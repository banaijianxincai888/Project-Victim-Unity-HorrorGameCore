using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///玩家下蹲姿态
    ///summary 
    public class PlayerCrouchState : PlayerStateBase
    {
        public PlayerCrouchState(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override void EnterState()
        {
            if (playerMovement.animFloatCor != null)
            {
                playerMovement.StopCoroutine(playerMovement.animFloatCor);
                playerMovement.animFloatCor = null;

            }
            PlayerCore.Instance.playerCamera.SetAngular(PlayerCore.Instance.playerCamera.crouchUpAngle);
            PlayerCore.Instance.playerCamera.SetFOV(PlayerCore.Instance.playerCamera.commonFOV);
            PlayerCore.Instance.anim.SetBool(PlayerCore.Instance.animPara.crouchAnim, true);
        }

        public override void ExitState()
        {
            if (playerMovement.animFloatCor != null)
            {
                playerMovement.StopCoroutine(playerMovement.animFloatCor);
                playerMovement.animFloatCor = null;

            }
            playerMovement.animFloatCor = playerMovement.StartCoroutine(playerMovement.ChangeAnimFloat("Horizontal", 0, "Vertical", 0));




            PlayerCore.Instance.anim.SetBool(PlayerCore.Instance.animPara.crouchAnim, false);

        }

        public override void UpdateState()
        {

            playerMovement.SetCrouchCameraPosition();

            playerMovement.RecoverStamina();

            float currentHor = PlayerCore.Instance.anim.GetFloat("Horizontal");

            float currentVert = PlayerCore.Instance.anim.GetFloat("Vertical");

            float smoothHorizontal = Mathf.Lerp(currentHor, playerMovement.horizontal, playerMovement.animSmoothTime);
            float smoothVertical = Mathf.Lerp(currentVert, playerMovement.vertical,  playerMovement.animSmoothTime);
            PlayerCore.Instance.anim.SetFloat("Horizontal", smoothHorizontal);
            PlayerCore.Instance.anim.SetFloat("Vertical", smoothVertical);
           
           

          
        }
    }
}

