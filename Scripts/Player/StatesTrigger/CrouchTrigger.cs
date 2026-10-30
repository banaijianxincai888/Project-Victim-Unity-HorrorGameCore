using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///蹲伏条件
    ///summary 
    public class CrouchTrigger : PlayerStateTriggerBase
    {
        public CrouchTrigger(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override bool Trigger()
        {

            return playerMovement.autoGenerate.Player.Crouch.IsPressed() && PlayerCore.Instance.controller.isGrounded;
        }
    }
}

