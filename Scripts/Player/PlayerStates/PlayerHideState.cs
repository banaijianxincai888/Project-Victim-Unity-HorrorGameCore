using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///玩家躲藏姿态
    ///summary 
    public class PlayerHideState : PlayerStateBase
    {
        private string para;
        public PlayerHideState(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override void EnterState()
        {
            PlayerCore.Instance.controller.enabled = false;
            PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.hide);
            para = playerMovement.currentHideTransform.GetComponent<Hideout>().hideoutInfo.hideoutType
                switch
            {
                HideoutType.Desk => PlayerCore.Instance.animPara.hideDeskAnim,
                HideoutType.Cabinet => PlayerCore.Instance.animPara.hideCabinetAnim,
                _ => string.Empty
            };
                
            PlayerCore.Instance.anim.SetBool(para, true);


        }
        public override void ExitState()
        {
            PlayerCore.Instance.controller.enabled = true;
            PlayerCore.Instance.anim.SetBool(para,false);
            if(playerMovement.enemyBlackBoard.hasSeen)
            {
                PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.isPursuit);

            }

        }

        public override void UpdateState()
        {
           
        }
    }
}

