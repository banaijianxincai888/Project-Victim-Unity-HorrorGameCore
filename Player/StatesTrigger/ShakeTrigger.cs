using UnityEngine;

namespace ns
{
    ///summary
    ///
    ///summary 
    public class ShakeTrigger : PlayerStateTriggerBase
    {
        public ShakeTrigger(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override bool Trigger()
        {
            return playerMovement.isShaking;
        }
    }
}

