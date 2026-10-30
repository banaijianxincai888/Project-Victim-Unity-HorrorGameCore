using UnityEngine;

namespace ns
{
	///summary
	///解密可脚本化对象
	///summary 
	[CreateAssetMenu(menuName = "Decryption/DecryptionSO")]
	public class DecryptionSO :ScriptableObject
	{

		[TextArea]public string needText;
		[TextArea] public string solveText; 
		public ItemSO needItem;

		public int needCounts;


  	}
}

