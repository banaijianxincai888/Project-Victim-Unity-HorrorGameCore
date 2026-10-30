using Cinemachine;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ns
{
    ///summary
    ///玩家摄像机
    ///summary 
    public class PlayerCamera : MonoBehaviour
    {
        public CinemachineVirtualCamera mainCamera;

        public bool canControl;

        [Header("Noise")]
        public CinemachineBasicMultiChannelPerlin noise;
        public NoiseSettings walknoiseSettings;
        public NoiseSettings runNoiseSettings;


        public float FOVChangeTime = 0.1f;
        public float commonFOV = 30f;
        public float runningFOV = 60f;
        public float checkDistance = 10f;


        public string interactTag;

        private Coroutine coroutineFOV;

        private Coroutine coroutineAngular;

        public LayerMask commonInteractLayer;


        public CinemachineVirtualCamera followCamera;

        public CinemachineVirtualCamera shakeCamera;


        [HideInInspector] public float verticalRotation = 0f;

       public float currentMaxUpAngle;

        public float maxDownAngle = -10f;

        public float commonUpAngle = 73f;
        public float runningUpAngle = 25f;
        public float crouchUpAngle = 20f;


        void Start()
        {
            noise = mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            // 锁定鼠标到屏幕中心，隐藏鼠标光标（提升操作体验）
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            EventCenter.CameraEvent.OnRayCheck += RaySent;
        }

        private void OnDisable()
        {
            EventCenter.CameraEvent.OnRayCheck -= RaySent;

        }

        void Update()
        {
            CameraCanControl();


            // 只有鼠标锁定时才响应旋转（避免误操作）
            if (canControl)
            {

                // 原始鼠标输入
                float rawMouseY = PlayerCore.Instance.playerMovement.mouse.y * PlayerCore.Instance.playerMovement.rotateSpeed * Time.deltaTime;


                // 计算旋转
                verticalRotation -= rawMouseY;

                // 限制角度
                verticalRotation = Mathf.Clamp(verticalRotation, maxDownAngle,currentMaxUpAngle);

                transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, transform.localEulerAngles.z);



            }
        }



        public bool CameraCanControl()
        {
            return canControl = Cursor.lockState == CursorLockMode.Locked && !PlayerCore.Instance.isAniming && !PlayerCore.Instance.playerMovement.isShaking;
        }





        public IInteractable[] RaySent()
        {

           

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, checkDistance,commonInteractLayer))
            {
                if (hit.collider.CompareTag(interactTag))
                {
                    print("GOT IT");
                    print (hit.collider.gameObject);
                    return  hit.transform.GetComponents<IInteractable>();

                }
            }

            // 如果处于躲藏状态，直接获取交互接口（此时没有 RaycastHit，可以设为 default）
            if (PlayerCore.Instance.playerMovement.currentState == PlayerStateType.Hide)
            {

                return PlayerCore.Instance.playerMovement.currentHideTransform
                    .GetComponents<IInteractable>();
            }


            print("NONE");
            return null;
        }





        public void SetFOV(float targetFOV)
        {
            if (coroutineFOV != null)
            {
                StopCoroutine(coroutineFOV);

            }
            coroutineFOV = StartCoroutine(ChangeFOV(targetFOV));

        }

        public void SetAngular(float targetEngular)
        {
            if (coroutineAngular != null)
            {
                StopCoroutine(coroutineAngular);

            }
            coroutineAngular = StartCoroutine(ChangeAngular(targetEngular));

        }



        public IEnumerator ChangeFOV(float targetFOV)
        {
            float duration = FOVChangeTime; // 固定0.1秒
            float startFOV = mainCamera.m_Lens.FieldOfView; // 起始FOV

            float elapsedTime = 0f; // 已用时间

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                // 计算进度（0~1），严格匀速
                float progress = Mathf.Clamp01(elapsedTime / duration);
                // 线性插值 = 绝对匀速
                mainCamera.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, progress);

                yield return null;

            }




            mainCamera.m_Lens.FieldOfView = targetFOV;


            coroutineFOV = null;

        }

        public IEnumerator ChangeAngular(float targetEngular)
        {
            float duration = FOVChangeTime;
            float startAngle =currentMaxUpAngle;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / duration);
               currentMaxUpAngle = Mathf.Lerp(startAngle, targetEngular, progress);
                yield return null;

            }


           currentMaxUpAngle = targetEngular;


            coroutineAngular = null;

        }

      
       
        public void ControlFollowCamera(bool isSolo)
        {
            followCamera.enabled = isSolo;
        }
    }
}

