using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
	///summary
	///对话可脚本化对象
	///summary 
	///
	[CreateAssetMenu(menuName = "Dialogue/DialogueSO")]
	public class DialogueSO :ScriptableObject
	{

		[TextArea] public string content;

		public AK.Wwise.Event AKEvent;

  	}
}

