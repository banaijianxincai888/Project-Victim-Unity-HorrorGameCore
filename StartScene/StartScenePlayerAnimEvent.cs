using UnityEngine;

namespace ns
{   
	///summary
	///开始场景玩家动画事件
	///summary 
	public class StartScenePlayerAnimEvent : MonoBehaviour
	{
		public AK.Wwise.Event hurtFootStep;

		public void HurtFootStep()
		{
			hurtFootStep.Post(gameObject);
		}

  	}
}

