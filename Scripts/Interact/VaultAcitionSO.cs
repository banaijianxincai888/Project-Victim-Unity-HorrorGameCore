using UnityEngine;

namespace ns
{
	[CreateAssetMenu(menuName = "VaultActionSO")]
	///summary
	///翻越行为可脚本化对象
	///summary 
	public class VaultAcitionSO : ScriptableObject
	{

		public AvatarTarget avatarTarget;

		[Range(0f, 1f)] 
		public float weight;

		public float startTime;

		public float targetTime;

		public Vector3 vaultPoint;

		public RaycastHit hit;

  	}
}

