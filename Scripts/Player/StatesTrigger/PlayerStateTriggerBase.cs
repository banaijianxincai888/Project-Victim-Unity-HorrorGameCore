using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{   
	///summary
	///玩家状态触发条件基类
	///summary 
	public abstract class PlayerStateTriggerBase 
	{
		protected PlayerMovement playerMovement;

		public PlayerStateTriggerBase(PlayerMovement _playerMovement)
		{
			playerMovement = _playerMovement;
		}

		public abstract bool Trigger();



  	}
}

