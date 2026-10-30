using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace ns
{   
	///summary
	///解密管理器
	///summary 
	public class DecryptionManager :MonoBehaviour
	{
		public float lastTime = 3f;
		public CanvasGroup canvasGroup;
		public TMP_Text text;

		public DecryptionItem currentDecryption;

		private Coroutine coroutine;



		public void ActiveDecryption(DecryptionItem decryption)
		{
			
			currentDecryption = decryption;
			if (currentDecryption.isSolved) return;
            UIManager.Instance.SmoothShowAndFadeWithCor(canvasGroup,ref coroutine, 0.3f, lastTime, 0.3f);
			text.text = currentDecryption.decryptionSO. needText;
		}

		public void SolveDecryption(DecryptionItem decryption)
		{
            currentDecryption = decryption;
            if (currentDecryption.isSolved) return;
            UIManager.Instance.SmoothShowAndFadeWithCor(canvasGroup,ref coroutine, 0.3f, lastTime, 0.3f);
            text.text = currentDecryption.decryptionSO.solveText;
            currentDecryption.transform.GetComponent<ISolveDecryption>().SolveDecryption();
			currentDecryption.isSolved = true;
		}


  	}
}

