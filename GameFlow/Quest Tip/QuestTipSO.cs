using UnityEngine;

namespace ns
{
	///summary
	///任务提示可脚本化对象
	///summary 
	///
	[CreateAssetMenu(menuName = "QuestTip/QuestTipSO")]
	public class QuestTipSO :ScriptableObject
	{
		[TextArea] public string description;

  	}
}

