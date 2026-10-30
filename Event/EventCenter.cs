using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{   
	///summary
	///事件中心
	///summary 
	public static class EventCenter
	{

		public static LootEvent LootEvent = new();

		public static CameraEvent CameraEvent = new();

		public static NightVisionEvent NightVisionEvent = new();

		public static VaultEvent VaultEvent = new();

		public static GameFlowEvent GameFlowEvent = new();

		public static TrapEvent TrapEvent = new();

  	}
}

