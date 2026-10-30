using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///玩家行走姿态
    ///summary 
    public class PlayerWalkState : PlayerStateBase
    {
        private float smoothHorizontal;

        private float smoothVertical;

        

    
           public PlayerWalkState(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }



        public override void EnterState()
        {
            if (playerMovement.animFloatCor != null)
            {
                playerMovement.StopCoroutine(playerMovement.animFloatCor);
                playerMovement.animFloatCor = null;
            }
            playerMovement.SetCameraCommon();
            PlayerCore.Instance.playerCamera.noise.m_NoiseProfile = PlayerCore.Instance.playerCamera.walknoiseSettings;
            if(!playerMovement.enemyBlackBoard.hasSeen)
            {
                PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.commonBreath);
            }

            
        }

        public override void ExitState()
        {
            if (playerMovement.animFloatCor != null)
            {
                playerMovement.StopCoroutine(playerMovement.animFloatCor);
                playerMovement.animFloatCor = null;

            }
            playerMovement.animFloatCor = playerMovement.StartCoroutine(playerMovement.ChangeAnimFloat("Horizontal",0, "Vertical",0));

            PlayerCore.Instance.anim.applyRootMotion = true;




        }
        

        public override void UpdateState()
        {
            float currentHor= PlayerCore.Instance.anim.GetFloat("Horizontal");

            float currentVert= PlayerCore.Instance.anim.GetFloat("Vertical");


            //避免原地旋转时导致的视角粘滞
            PlayerCore.Instance.anim.applyRootMotion = Mathf.Abs(currentHor) > 0.1f || Mathf.Abs(currentVert) > 0.1f;

            smoothHorizontal = Mathf.Lerp(currentHor, playerMovement.horizontal,  playerMovement.animSmoothTime);
            smoothVertical = Mathf.Lerp(currentVert, playerMovement.vertical,  playerMovement.animSmoothTime);

            
          
            PlayerCore.Instance.anim.SetFloat("Horizontal", smoothHorizontal);
            PlayerCore.Instance.anim.SetFloat("Vertical", smoothVertical);



            playerMovement.SetCommonCameraPosition();
            
            playerMovement.RecoverStamina();
        }


       
    }
}

