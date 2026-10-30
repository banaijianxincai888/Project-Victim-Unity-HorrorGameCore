using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ns
{   
	///summary
	///玩家核心组件单例
	///summary 
	public class PlayerCore : MonoSingleton<PlayerCore>
	{
		public Animator anim;

		public CharacterController controller;

		public PlayerCamera playerCamera;

		public PlayerMovement playerMovement;


		public AnimPara animPara;


		public PlayerAnimEvent playerAnimEvent;

		public PlayerPostProcess playerPostProcess;

		public PlayerHealth playerHealth;

		public PlayerSoundEmission playerSoundEmission;


		public bool isAniming;

        public bool timeLineApplyRootMotion;


        private void Awake()
        {
            anim = GetComponent<Animator>();
			playerMovement = GetComponent<PlayerMovement>();
			playerAnimEvent = GetComponent<PlayerAnimEvent>();
			playerPostProcess = GetComponent<PlayerPostProcess>();
			playerHealth = GetComponent<PlayerHealth>();
			playerSoundEmission = GetComponent<PlayerSoundEmission>();
			timeLineApplyRootMotion = anim.applyRootMotion;
        }

		
		

		


    }


	[Serializable]
	public class AnimPara
	{
		public string runAnim;

		public string crouchAnim;

		public string jumpAnim;

		public string defenceAnim;

		public string vaultAnim;

		public string openDoorAnim;

		public string pickUpAnim;

		public string liftAnim;

		public string hideDeskAnim;

		public string hideCabinetAnim;

		public string isFoundCabinetAnim;

		public string isFoundDeskAnim;

		public string shakeAnim;

		public string resetAnim;

		public string[] deathParas;
	}

}

