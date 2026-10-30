using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///躲藏条件
    ///summary 
    public class HideTrigger : PlayerStateTriggerBase
    {
        public HideTrigger(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override bool Trigger()
        {
            return playerMovement.isHiding;
        }
    }
}

