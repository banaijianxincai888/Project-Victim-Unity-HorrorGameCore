using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{   
	///summary
	///玩家状态基类，构建有限状态机
	///summary 
	public abstract class PlayerStateBase
	{
		protected PlayerMovement playerMovement;


		public PlayerStateBase(PlayerMovement _playerMovement)
		{
			playerMovement = _playerMovement;
		}
		
		public abstract void EnterState() ;

		public abstract void UpdateState() ;
		public abstract void ExitState() ;

			
  	}
}

