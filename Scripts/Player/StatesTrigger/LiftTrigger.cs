using UnityEngine;

namespace ns
{
    ///summary
    ///搬运条件
    ///summary 
    public class LiftTrigger : PlayerStateTriggerBase
    {
        public LiftTrigger(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override bool Trigger()
        {
            return playerMovement.isLifting;
        }
    }
}

