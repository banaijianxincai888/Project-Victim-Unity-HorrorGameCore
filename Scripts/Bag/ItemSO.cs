using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
	[CreateAssetMenu(menuName = "Bag/ItemSO")]
	///summary
	///物品可脚本化对象
	///summary 
	public class ItemSO :ScriptableObject
	{
		public string itemName;

		public Sprite sprite;

		public AK.Wwise.Event pickUpSound;


		public int maxCount;

		

    }
}

