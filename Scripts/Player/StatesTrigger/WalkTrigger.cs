using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///走路条件
    ///summary 
    public class WalkTrigger : PlayerStateTriggerBase
    {
        public WalkTrigger(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }



        public override bool Trigger()
        {
           

            return PlayerCore.Instance.controller.isGrounded ;
        }
    }
}

