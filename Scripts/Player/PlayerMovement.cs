using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

namespace ns
{
    ///summary
    ///玩家移动脚本，控制玩家水平旋转
    ///summary 
    public class PlayerMovement : MonoBehaviour
    {
        public EnemyBlackBoard enemyBlackBoard;

        [Header("MoveSpeed & RotateSpeed")]

        private float currentMouseX;

        public float mouseTurnSensitivity = 0.01f;



        [Range(1, 10)] public float rotateSpeed;


        [Header("输入系统自动生成的脚本地图控制")]
        public InputSystemAutoGenerate autoGenerate;
        [Header("Heath")]
        public bool isDeath;


        [Header("Lift")]
        public bool isLifting;

        public bool canPlace;

        public Decryption_Plate plate;

        public Transform carryBoxTF;

        public Transform dropBoxTF;

        public Transform currentBox;


        [Header("Hide")]
        public bool isHiding;

        public Transform currentHideTransform;

        public NightVisionController nightVisionController;


        public bool isCaught;

        [Header("Crouch")]
        public float cameraOffsetY = 1f;

        public float cameraOffsetZ = 0.4f;

        [Header("Shake")]
        public bool isShaking;

        public GameObject currentTrap;

        [Range(0, 1)]
        public float progressRotate;

        [Header("CurrentState")]
        // 告诉 ES3：不要保存这个字段，也不要在读档时用空数据覆盖它
        public PlayerStateType currentState;

        [Header("AnimTime")]
        public float animSmoothTime = 0.05f;

        public float animRecoverTime = 0.5f;

        [Header("Movement & Gravity")]
        public float gravity = 20f;           // 重力加速度（正值，向下）
        private Vector3 velocity;             // 当前速度向量




        [HideInInspector]
        public float horizontal;

        [HideInInspector]
        public float vertical;

        private float mouseX;

        [HideInInspector]
        public Vector2 mouse;



        [HideInInspector]
        public float camerOrignalY;



        private Dictionary<PlayerStateTriggerBase, Dictionary<PlayerStateType, PlayerStateBase>> stateDic;


        public PlayerStateBase currentStateClass;

        public Coroutine animFloatCor;

        private void Awake()
        {
            camerOrignalY = PlayerCore.Instance.playerCamera.transform.localPosition.y;
            autoGenerate = new InputSystemAutoGenerate();
            InitStateDic();
        }



        private void OnEnable()
        {
            autoGenerate.Player.Enable();

            //EventCenter.PlayerInputEvent.OnPlayerMovement += MoveAndRotate;
        }

        private void OnDisable()
        {
            autoGenerate.Player.Disable();

            // EventCenter.PlayerInputEvent.OnPlayerMovement -= MoveAndRotate;


        }

        private void InitStateDic()
        {
            stateDic = new();
            foreach (PlayerStateType type in Enum.GetValues(typeof(PlayerStateType)))
            {
                string stateName = "Player" + type.ToString() + "State";
                string triggerName = type.ToString() + "Trigger";
                string fullStateName = "ns." + stateName;
                string fullTriggerName = "ns." + triggerName;
                Type stateType = Assembly.GetExecutingAssembly().GetType(fullStateName);
                Type triggerType = Assembly.GetExecutingAssembly().GetType(fullTriggerName);
                if (stateType != null && triggerType != null)
                {



                    // 【关键修复】显式创建参数数组，彻底避免重载歧义！
                    object[] constructorParams = new object[] { this }; // this = PlayerMovement实例
                    //传递this给构造函数
                    PlayerStateBase stateInstance = Activator.CreateInstance(stateType, constructorParams) as PlayerStateBase;
                    PlayerStateTriggerBase triggerInstance = Activator.CreateInstance(triggerType, constructorParams) as PlayerStateTriggerBase;
                    stateDic.Add(triggerInstance, new());
                    stateDic[triggerInstance].Add(type, stateInstance);
                }
            }
        }



        public void Movement()
        {

            CheckPlayerState();
            ExecutionState();
            if (PlayerCore.Instance.isAniming || isHiding || isDeath) return;
            horizontal = autoGenerate.Player.Horizontal.ReadValue<float>();
            vertical = autoGenerate.Player.Vertical.ReadValue<float>();

        }

        public void Rotation()
        {
            if (PlayerCore.Instance.isAniming || isHiding || isDeath || isShaking) return;
            mouse = autoGenerate.Player.Look.ReadValue<Vector2>();
            mouseX = mouse.x * rotateSpeed * Time.deltaTime;

            currentMouseX = Mathf.Lerp(currentMouseX, mouse.x,mouseTurnSensitivity );
            currentMouseX = Mathf.Clamp(currentMouseX, -1f, 1f);


            // 非躲藏状态：自由旋转
            PlayerCore.Instance.anim.SetFloat("Turn", currentMouseX);

            transform.Rotate(Vector3.up * mouseX);
        }

        private void ApplyGravityAndMovement()
        {
            // 如果死亡，可能完全禁止移动（可根据需求调整）
            if (isDeath ) return;

            CharacterController controller = PlayerCore.Instance.controller;
            if (controller == null) return;
            if (PlayerCore.Instance.timeLineApplyRootMotion) return;
            if (!controller.enabled) return;
            // --- . 重力 ---
            if (controller.isGrounded && velocity.y < 0)
            {
                // 着地时给一个很小的负值，确保紧贴地面（防止“飘”）
                velocity.y = -2f;
            }

            // 每帧叠加向下的加速度
            velocity.y -= gravity * Time.deltaTime;

            // --- . 执行移动 ---
            controller.Move(velocity * Time.deltaTime);
        }

        private void Update()
        {
           
            Movement();
            Rotation();
            ApplyGravityAndMovement();

        }

    



       

        private void CheckPlayerState()
        {

            foreach (var trigger in stateDic.Keys)
            {
                if (trigger.Trigger())
                {
                    foreach (var state in stateDic[trigger].Keys)
                    {
                        SwitchState(state, stateDic[trigger][state]);
                        return;
                    }
                }
            }



        }

        public void SwitchState(PlayerStateType type, PlayerStateBase state)
        {
            if (currentState == type) return;

            if (currentStateClass != null)
            {
                currentStateClass.ExitState();
            }

            currentState = type;
            currentStateClass = state;

            currentStateClass.EnterState();

        }

        private void ExecutionState()
        {
            if (currentStateClass != null)
            {
                currentStateClass.UpdateState();
            }
        }

        public void ForceWalkState()
        {

            // 1. 遍历字典，找出 Walk 状态的实例
            foreach (var triggerDict in stateDic.Values)
            {
                if (triggerDict.TryGetValue(PlayerStateType.Walk, out PlayerStateBase walkState))
                {
                    // 2. 调用你原有的 SwitchState 方法
                    SwitchState(PlayerStateType.Walk, walkState);
                    break;
                }
            }
            //3.恢复所有的状态
            isCaught = false;
            isDeath = false;
            isHiding = false;
            isLifting = false;
            isShaking = false;
            canPlace = false;
        }


        public void SetCameraRun()
        {
            PlayerCore.Instance.playerCamera.SetAngular(PlayerCore.Instance.playerCamera.runningUpAngle);
            PlayerCore.Instance.playerCamera.SetFOV(PlayerCore.Instance.playerCamera.runningFOV);
        }

        public void CostStamina()
        {
            UIManager.Instance.staminaManager.StopRecoverStamina();
            UIManager.Instance.staminaManager.StartCostStamina();
        }

       
        public void RecoverStamina()
        {
            UIManager.Instance.staminaManager.StopCostStamina();
            UIManager.Instance.staminaManager.StartRecoverStamina();
        }

        public void SetCameraCommon()
        {
            PlayerCore.Instance.playerCamera.SetAngular(PlayerCore.Instance.playerCamera.commonUpAngle);
            PlayerCore.Instance.playerCamera.SetFOV(PlayerCore.Instance.playerCamera.commonFOV);
        }


        public void SetCommonCameraPosition()
        {

            PlayerCore.Instance.playerCamera.transform.localPosition = new Vector3(PlayerCore.Instance.playerCamera.transform.localPosition.x,
               Mathf.Lerp(PlayerCore.Instance.playerCamera.transform.localPosition.y, camerOrignalY, PlayerCore.Instance.playerCamera.FOVChangeTime),
               PlayerCore.Instance.playerCamera.transform.localPosition.z);
        }

        public void SetCrouchCameraPosition()
        {
            PlayerCore.Instance.playerCamera.transform.localPosition = new Vector3(PlayerCore.Instance.playerCamera.transform.localPosition.x,
                Mathf.Lerp(PlayerCore.Instance.playerCamera.transform.localPosition.y, cameraOffsetY, PlayerCore.Instance.playerCamera.FOVChangeTime),
               Mathf.Lerp(PlayerCore.Instance.playerCamera.transform.localPosition.z, cameraOffsetZ, PlayerCore.Instance.playerCamera.FOVChangeTime));
        }

       

        


        public IEnumerator ChangeAnimFloat(string Hor,float hor,string Ver,float ver )
        {

            float elapsedTime = 0f;
            while (elapsedTime < animRecoverTime)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / animRecoverTime);
                float currentHor = PlayerCore.Instance.anim.GetFloat(Hor);
                float currentVert = PlayerCore.Instance.anim.GetFloat(Ver);
                float smoothH = Mathf.Lerp(currentHor, hor, progress);
                float smoothV = Mathf.Lerp(currentVert, ver,progress);
                PlayerCore.Instance.anim.SetFloat(Hor, smoothH);
                PlayerCore.Instance.anim.SetFloat(Ver, smoothV);

                yield return null;

            }
          
            PlayerCore.Instance.anim.SetFloat("Horizontal",hor);
            PlayerCore.Instance.anim.SetFloat("Vertical",ver);
        }


      


    }


   
}

