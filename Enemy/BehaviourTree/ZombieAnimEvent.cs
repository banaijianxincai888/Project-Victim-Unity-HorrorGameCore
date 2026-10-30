using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

namespace ns
{
	///summary
	///敌人动画事件
	///summary 
	public class ZombieAnimEvent : MonoBehaviour
	{
		public ZombieAnimPara zombieAnimPara;

		public EnemyBlackBoard enemyBlackBoard;

		public PlayerCore playerCore;

		public float hitBackTime = 0.5f;

		public float hitBackDistance = 2f;



        private void Start()
        {
            playerCore = enemyBlackBoard.player.GetComponent<PlayerCore>();
        }



		public void OnCancelAnim(string animPara)
		{
			enemyBlackBoard.anim.SetBool(animPara, false);
			print(animPara);
		}

		// 在翻越动画的最后一帧通过 Animation Event 调用这个函数

		

		public void BreakDoor()
		{
			Door door = enemyBlackBoard.door;
			door.count++;

			if (door.count >= door.kickCount)
			{
				door.StartCoroutine(door.DoorControl(transform, door.doorBreakTime, true));
				door.isOpen = true;
			}
			else
			{
				door.TriggerShake(transform);
			}
		}

		public void SetAIInteract()
		{
			enemyBlackBoard.currentHideInfo.isAIInteract = true;
            enemyBlackBoard.soundEmission.SoundEmission(enemyBlackBoard.soundEmission.findHideDialogueSOs);
            PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.isFound);
            Debug.Log("SetAIInteract");

        }

        public void CancelAIInteract()
		{
			enemyBlackBoard.currentHideInfo.isAIInteract = false;
			Debug.Log("CancelAIInteract");
           

        }

        public void OpenCabinet()
		{
			 playerCore.playerAnimEvent.OpenCabinetDoor();

        }

        public void CloseCabinet()
		{
			 playerCore.playerAnimEvent.CloseCabinetDoor();
		}

		public void CatchPlayer()
		{
			StartCoroutine(HitBackPlayer());
        }


		private IEnumerator HitBackPlayer()
		{
            float time = 0;
			Vector3 start = playerCore.transform.position;
			Vector3 end = playerCore.transform.position - playerCore.transform.forward * hitBackDistance;
			while(time < hitBackTime)
			{
				time += Time.deltaTime;
				float progress = Mathf.Clamp01(time / hitBackTime);
				playerCore.transform.position = Vector3.Lerp(start,end , progress);
				yield return null;
			}
			playerCore.transform.position = end;
			string para = enemyBlackBoard.currentHideInfo.hideoutType switch
			{
				HideoutType.Cabinet => playerCore.animPara.isFoundCabinetAnim,
				HideoutType.Desk => playerCore.animPara.isFoundDeskAnim,
				_ => string.Empty
			};
            playerCore.anim.SetTrigger(para);
			enemyBlackBoard.currentHideInfo.isInside = false;
        }

		public void AttackPlayer()
		{
			if(Vector3.Distance(enemyBlackBoard.player.position,transform.position) < enemyBlackBoard.atkDistance)
			playerCore.playerHealth.Hurt(enemyBlackBoard.atk,true);
		}

		public void AxeSound()
		{
            enemyBlackBoard.soundEmission.axe.Post(gameObject);
        }



        public void FootStep()
		{
			enemyBlackBoard.soundEmission.footStep.Post(gameObject);
		}

		public void SetTrap()
		{
			TrapManager.Instance.SetTrap(enemyBlackBoard.setTrapTF);
		}

    }

	[Serializable]
	public class ZombieAnimPara
	{
		public string coldAnimPara;

		public string pursuitPara;

		public string walkPara;

		public string atkPara;

		public string alarmPara;

		public string overPara;

		public List<string> searchPara;

		public string findLastPosPara;

		public string hearPara;

		public string kickDoorPara;

		public string setTrapPara;
	}

	
}

