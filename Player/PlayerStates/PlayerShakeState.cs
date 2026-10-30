using Unity.VisualScripting;
using UnityEngine;

namespace ns
{
    ///summary
    ///
    ///summary 
    public class PlayerShakeState : PlayerStateBase
    {
        private BearTrap bearTrap;
        public PlayerShakeState(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override void EnterState()
        {
            EventCenter.TrapEvent.OnActiveTrap += ExitTrap;
            UIManager.Instance.exitTrapManager.ShowExitTrapTip();
            bearTrap = playerMovement.currentTrap.GetComponent<BearTrap>();
            PlayerCore.Instance.playerCamera.shakeCamera.enabled = true;
            PlayerCore.Instance.anim.SetBool(PlayerCore.Instance.animPara.shakeAnim, true);
        }

        public override void ExitState()
        {
            EventCenter.TrapEvent.OnActiveTrap -= ExitTrap;
            UIManager.Instance.exitTrapManager.CancelExitTrapTip();
            PlayerCore.Instance.playerCamera.shakeCamera.enabled = false;
            PlayerCore.Instance.anim.SetBool(PlayerCore.Instance.animPara.shakeAnim, false);
        }

        public override void UpdateState()
        {
            UIManager.Instance.exitTrapManager.PullProgress();
        }

        private void ExitTrap()
        {

            UIManager.Instance.exitTrapManager.PushProgress(playerMovement.progressRotate);

            if (UIManager.Instance.exitTrapManager.progress.value >= 1)
            {
                playerMovement.isShaking = false;
            }
          
        }

        

    }
}

