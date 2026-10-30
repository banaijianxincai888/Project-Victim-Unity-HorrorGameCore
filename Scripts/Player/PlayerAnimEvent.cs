using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

namespace ns
{   
	///summary
	///玩家动画事件
	///summary 
	public class PlayerAnimEvent : MonoBehaviour
	{

		public Loot loot;

		public Cabinet cabinet;


		public TwoBoneIKConstraint lootRightRig;

		public float walkSoundInterval = 0.2f;

		private float lastWalkTime;


		public void PickUpLoot()
		{
			loot.itemSO.pickUpSound.Post(gameObject);
			loot.gameObject.SetActive(false);
		}



        public void CancelRigIK()
        {
           
            // 延迟一帧再打印，看是不是被动画系统改回去了
            StartCoroutine(SmoothCancel());
        }

        IEnumerator SmoothCancel()
        {
            float time = 0;
			float start = lootRightRig.weight;
			while(time < loot.ikFadeOutTime)
			{
				time += Time.deltaTime;
				float progress = Mathf.Clamp01(time / loot.ikFadeOutTime);
				lootRightRig.weight = Mathf.Lerp(start,0,progress);
				yield return null;
			}
			lootRightRig.weight = 0f;
        }

		public void EnterAnim()
		{
            PlayerCore.Instance.isAniming = true;
		}

		public void ExitAnim()
		{
            PlayerCore.Instance.isAniming = false;


        }

		public void OpenCabinetDoor()
		{
			if (PlayerCore.Instance.playerMovement.currentHideTransform != null)
				cabinet = PlayerCore.Instance.playerMovement.currentHideTransform.GetComponent<Cabinet>();
            StartCoroutine(cabinet.DoorControl(cabinet.openTime, true));
		}

        public void CloseCabinetDoor()
        {
            StartCoroutine(cabinet.DoorControl(cabinet.openTime, false));
        }

		public void ActiveHide()
		{
			EnterAnim();
			ActiveCameraFollow();
		}

		public void CancelHide()
		{
			ExitAnim();
			CancelCameraFollow();
		}

		
		public void ActiveCaught()
		{
			PlayerCore.Instance.playerMovement.isCaught = true;
			EnterAnim();
			ActiveCameraFollow();
            PlayerCore.Instance.playerMovement.currentHideTransform = null;

            PlayerCore.Instance.playerMovement.isHiding = false;
        }

		public void ExitCaught()
		{
			PlayerCore.Instance.playerMovement.isCaught = false;
			CancelCameraFollow();
			ExitAnim();
        }


        public void ActiveCameraFollow()
		{
            PlayerCore.Instance.playerCamera.ControlFollowCamera(true);
			PlayerCore.Instance.playerMovement.nightVisionController.HideLight();
        }
        public void CancelCameraFollow()
		{
			if (PlayerCore.Instance.playerMovement.isCaught || PlayerCore.Instance.playerMovement.isHiding) return;
            PlayerCore.Instance.playerCamera.ControlFollowCamera(false);
			PlayerCore.Instance.playerMovement.nightVisionController.CommonLight();
        }

		public void HideInSound()
		{
			SoundManager.Instance.cloth.Post(gameObject);
        }

		public void WalkSound()
		{
			if (PlayerCore.Instance.playerMovement.currentState == PlayerStateType.Walk || 
				PlayerCore.Instance.playerMovement.currentState == PlayerStateType.Lift)
			{

				float currentTime = Time.time;

				if (currentTime - lastWalkTime <= walkSoundInterval)
					return;
				lastWalkTime = Time.time;
				PlayerCore.Instance.playerSoundEmission.walking.Post(gameObject);
			}
		}

		public void RunSound()
		{
            PlayerCore.Instance.playerSoundEmission.running.Post(gameObject);

        }

		public void FallDownSound()
		{
            PlayerCore.Instance.playerSoundEmission.fallDown.Post(gameObject);
        }

		public void FallUpSound()
		{
            PlayerCore.Instance.playerSoundEmission.fallUp.Post(gameObject);

        }
    }

	
}

