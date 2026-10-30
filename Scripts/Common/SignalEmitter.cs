using UnityEngine;

namespace ns
{   
	///summary
	///玩家信号发射器
	///summary 
	public class SignalEmitter : MonoBehaviour
	{
		public void StartAnim()
		{
			PlayerCore.Instance.isAniming = true;
		}

		public void StopAnim()
		{
			PlayerCore.Instance.isAniming = false;
        }

		public void StartHiding()
		{
			PlayerCore.Instance.playerMovement.isHiding = true;
		}

		public void StopHiding()
		{
			PlayerCore.Instance.playerMovement.isHiding = false;

        }

    }
}

