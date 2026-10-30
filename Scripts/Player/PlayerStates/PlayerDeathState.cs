using UnityEngine;

namespace ns
{
    ///summary
    ///角色死亡状态
    ///summary 
    public class PlayerDeathState : PlayerStateBase
    {
        public PlayerDeathState(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override void EnterState()
        {
            System.Random random = new System.Random();
            int index = random.Next(0, PlayerCore.Instance.animPara.deathParas.Length);
            PlayerCore.Instance.playerAnimEvent.ActiveCameraFollow();
            PlayerCore.Instance.anim.SetTrigger(PlayerCore.Instance.animPara.deathParas[index]);
            BGMManager.Instance.ChangeBGM(BGMManager.Instance.deadBGM);
            PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.dead);
        }

        public override void ExitState()
        {
            PlayerCore.Instance.playerAnimEvent.CancelCameraFollow();
        }

        public override void UpdateState()
        {
        }
    }
}

