using UnityEngine;

namespace ns
{
    ///summary
    ///搬运状态
    ///summary 
    public class PlayerLiftState : PlayerStateBase
    {

        private float smoothHorizontal;

        private float smoothVertical;
        public PlayerLiftState(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override void EnterState()
        {
            if (!playerMovement.enemyBlackBoard.hasSeen)
            {
                PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.commonBreath);
            }
        }

        public override void ExitState()
        {

            DropBox dropBox = playerMovement.carryBoxTF.GetComponent<DropBox>();
            dropBox.Drop();
            if (playerMovement.animFloatCor != null)
            {
                playerMovement.StopCoroutine(playerMovement.animFloatCor);
                playerMovement.animFloatCor = null;

            }
            playerMovement.animFloatCor = playerMovement.StartCoroutine(playerMovement.ChangeAnimFloat("Horizontal", 0, "Vertical", 0));
        }

        public override void UpdateState()
        {
           


            float currentHor = PlayerCore.Instance.anim.GetFloat("Horizontal");

            float currentVert = PlayerCore.Instance.anim.GetFloat("Vertical");


            //避免原地旋转时导致的视角粘滞
            PlayerCore.Instance.anim.applyRootMotion = Mathf.Abs(currentHor) > 0.1f || Mathf.Abs(currentVert) > 0.1f;

            smoothHorizontal = Mathf.Lerp(currentHor, playerMovement.horizontal, playerMovement.animSmoothTime);
            smoothVertical = Mathf.Lerp(currentVert, playerMovement.vertical, playerMovement.animSmoothTime);



            PlayerCore.Instance.anim.SetFloat("Horizontal", smoothHorizontal);
            PlayerCore.Instance.anim.SetFloat("Vertical", smoothVertical);



            playerMovement.SetCommonCameraPosition();

            playerMovement.RecoverStamina();
        }
    }
}

