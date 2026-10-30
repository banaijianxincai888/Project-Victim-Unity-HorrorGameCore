using System;
using System.Collections;
using UnityEngine;

namespace ns
{   
	///summary
	///玩家生命脚本
	///summary 
	public class PlayerHealth : MonoBehaviour
	{
		public float currentHP = 100f;

		public float maxHP = 100f;

		public float recoverTime = 10f;

		public float startRecoverTime = 5f;

		private Coroutine coroutine;


		public void Hurt(float value,bool useEarRing)
		{
			StopAllCoroutines();
			if(currentHP-value <= 0)
			{
				Death();
				UIManager.Instance.deathUIManager.ShowDeathUI();
			}
			else
			{
				currentHP -= value;
                PlayerCore.Instance.playerPostProcess.SetHurtVolume(useEarRing);
				PlayerCore.Instance.playerSoundEmission.ahh.Post(gameObject);
				ActiveBloodUI();
            }
        }

		private void Death()
		{
			PlayerCore.Instance.playerMovement.isDeath = true;
		}

		public void ActiveBloodUI()
		{
			StopAllCoroutines();
			float rotate = currentHP / maxHP;
			UIManager.Instance.bloodManager.canvasGroup.alpha = 1;
			if(rotate==1)
			{
				UIManager.Instance.bloodManager.canvasGroup.alpha = 0;
            }
            else if (rotate >= 0.75 && rotate <1)
			{
				UIManager.Instance.bloodManager.SetBloodUI(UIManager.Instance.bloodManager.blood75);
			}
			else if(rotate >= 0.5 && rotate < 0.75)
			{
                UIManager.Instance.bloodManager.SetBloodUI(UIManager.Instance.bloodManager.blood50);

            }
			else if(rotate >=0.25 &&  rotate < 0.5)
			{
                UIManager.Instance.bloodManager.SetBloodUI(UIManager.Instance.bloodManager.blood25);

            }
			else
			{
                UIManager.Instance.bloodManager.SetBloodUI(UIManager.Instance.bloodManager.blood25);
            }
        }


		public void StartRecoverHP()
		{
			if (PlayerCore.Instance.playerMovement.isDeath) return;
			if (coroutine != null)
			{
				StopAllCoroutines();
			}
			coroutine = StartCoroutine(RecoverHP());
		}
    
		IEnumerator RecoverHP()
		{
			float Timer = 0f;
			yield return new WaitForSeconds(startRecoverTime);
			float startHP = currentHP;
			while(Timer < recoverTime )
			{
				Timer += Time.deltaTime;
				float progress = Mathf.Clamp01( Timer / recoverTime);
				currentHP = Mathf.Lerp(startHP, maxHP,progress);
                UIManager.Instance.bloodManager.canvasGroup.alpha = Mathf.Lerp(1,0,progress);
                yield return null;
			}
		}
    }
}

