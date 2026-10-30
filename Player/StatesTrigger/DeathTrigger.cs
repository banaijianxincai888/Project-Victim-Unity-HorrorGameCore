using UnityEngine;

namespace ns
{
    ///summary
    ///死亡触发条件
    ///summary 
    public class DeathTrigger : PlayerStateTriggerBase
    {
        public DeathTrigger(PlayerMovement _playerMovement) : base(_playerMovement)
        {
        }

        public override bool Trigger()
        {
            return playerMovement.isDeath;
        }
    }
}

