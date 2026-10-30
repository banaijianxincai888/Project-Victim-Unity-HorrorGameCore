using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///跑步条件
    ///summary 
    public class RunTrigger : PlayerStateTriggerBase
    {
        public RunTrigger(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override bool Trigger()
        {
            return playerMovement.autoGenerate.Player.Run.IsPressed()
                && UIManager.Instance.staminaManager.staminaImage.fillAmount > 0f &&
                PlayerCore.Instance.controller.isGrounded && PlayerCore.Instance.playerMovement.vertical > 0f;
        }
    }
}

