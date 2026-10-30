using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{   
	///summary
	///战利品事件
	///summary 
	public class LootEvent 
	{
		public Action<ItemSO> OnLootPickedUp;

		public Action OnBoxDrop;

  	}
}

