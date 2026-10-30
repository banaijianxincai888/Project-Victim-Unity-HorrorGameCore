using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace ns
{
    ///summary
    ///门脚本（支持中途打断/平滑开关门）
    ///summary 
    public class Door : MonoBehaviour,IInteractable
    {
        public bool isAIInteract;

        public bool isOpen;

        public bool isLock;


        private float doorInteractTime = 1.5f;

        public float doorBreakTime = 0.2f;

        public float resetTime = 1f;


        public float openAngle = 90f;

        private Coroutine coroutine;

        public Transform center;

        public Transform kickPointFront;

        public Transform kickPointBack;

        public Transform kickPoint;

        public int kickCount = 3;

        public int count;


        [Header("抖动参数")]
        public float amplitude = 3f;          // 每次抖动的最大角度（度）
        public float frequency = 10f;         // 抖动频率（每秒完整来回次数）
        public int shakeTimes = 3;            // 抖动次数（一个来回算一次）

        private Quaternion initLocalRot;
        private bool isShaking = false;       // 防止重复触发

        private void Start()
        {
            initLocalRot = transform.localRotation;
        }

        //AI破门和射线检测的注意事项
      
        public void DoorInteract()
        {
            if(isAIInteract) { return; }
            if (isLock)
            {
                TriggerShake(transform);
                return;
            }

            if (coroutine != null) return;
            bool isRun = PlayerCore.Instance.playerMovement.currentState == PlayerStateType.Run;

            isOpen = !isOpen;
            if (isOpen)
            {
               if(isRun)
                {
                    coroutine = StartCoroutine(DoorControl(PlayerCore.Instance.transform, doorBreakTime,true));
                }
               else
                {
                    coroutine = StartCoroutine(DoorControl(PlayerCore.Instance.transform, doorInteractTime, true));
                    PlayerCore.Instance.anim.SetTrigger(PlayerCore.Instance.animPara.openDoorAnim);

                }

            }

            else
            {
                PlayerCore.Instance.anim.SetTrigger(PlayerCore.Instance.animPara.openDoorAnim);
                coroutine = StartCoroutine(DoorControl(PlayerCore.Instance.transform, doorInteractTime, false));
            }

        }

       public IEnumerator DoorControl(Transform subject, float controlTime,bool isOpen)
        {
            if(controlTime == doorBreakTime)
            {
                SoundManager.Instance.doorBreak.Post(gameObject);
            }
            else
            {
                AK.Wwise.Event @event = isOpen ? SoundManager.Instance.doorOpen : SoundManager.Instance.doorClose;
                @event.Post(gameObject);
            }

            kickPoint = SelectKickPoint(subject);
            float timer = 0;
            Quaternion start = transform.localRotation;
            float deltaAngle = 0;
           if(isOpen)
            {
                if(kickPoint == kickPointBack)
                {
                    deltaAngle = openAngle;
                }
                else
                {
                    deltaAngle = -openAngle;
                }
            }
           else
            {
                deltaAngle = 0;
            }

          Quaternion end = Quaternion.identity;
            if(deltaAngle !=0)
            {
                end = start* Quaternion.Euler(0,0,deltaAngle);
            }
           
            while (timer < controlTime)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / controlTime);
                transform.localRotation = Quaternion.Lerp(start,end
                    , progress);
                yield return null;
            }
            transform.localRotation = end;
            coroutine = null;
        }

        public  Transform SelectKickPoint(Transform subject)
        {
            return (Vector3.Distance(subject.position, kickPointFront.position) <
                Vector3.Distance(subject.position,kickPointBack.position)) ? kickPointFront :
                kickPointBack;
        }

        /// <summary>
        /// 外部调用：开始抖动（例如在碰撞或AI事件中调用）
        /// </summary>
        public void TriggerShake(Transform target)
        {
            if (!isShaking)
            {
                StartCoroutine(ShakeCoroutine(target));
            }
        }

        IEnumerator ShakeCoroutine(Transform target)
        {
            isShaking = true;

            if(target.CompareTag("AI"))
            {
                SoundManager.Instance.doorBreak.Post(gameObject);
            }
            else
            {
                SoundManager.Instance.doorLock.Post(gameObject);
            }
            // 计算每个抖动周期需要的时间（一个完整的正弦波周期 = 1/frequency 秒）
            float period = 1f / frequency;
            // 总抖动时间 = 次数 × 周期
            float totalTime = shakeTimes * period;
            float timer = 0f;

            while (timer < totalTime)
            {
                timer += Time.deltaTime;
                // 计算当前相位（0 到 shakeTimes*2π）
                float phase = timer * frequency * 2f * Mathf.PI;
                // 正弦值范围 [-1, 1]，乘以幅度得到角度
                float angle = Mathf.Sin(phase) * amplitude;

                // 应用旋转（局部Z轴）
                transform.localRotation = initLocalRot * Quaternion.Euler(0, 0, angle);

                yield return null;
            }
            transform.localRotation = initLocalRot;

            isShaking = false;
        }

        public void TryInteract()
        {
            //PlayerCore.Instance.playerIK.StartRightHandIK();
             DoorInteract();

        }

        private void OnTriggerEnter(Collider other)
        {
            if (isOpen || isLock) return;
            if (other.CompareTag("AI"))
            {
                EnemyBlackBoard enemyBlackBoard = other.GetComponent<EnemyBlackBoard>();
                if (enemyBlackBoard == null) return;

                Vector3 AIForward = other.transform.forward;
                AIForward.y = 0f;
                AIForward.Normalize();

                Vector3 toDoor = center.position - other.transform.position;
                toDoor.y = 0f;
                float distToDoor = toDoor.magnitude;
                toDoor.Normalize();

                // 【铁律2】：如果 AI 已经发现玩家了，而且玩家不在门那边（在身后），转身蹭到门时直接忽略！
                if (enemyBlackBoard.hasSeen && enemyBlackBoard.player != null)
                {
                    Vector3 toPlayer = enemyBlackBoard.player.position - other.transform.position;
                    toPlayer.y = 0f;
                    if (Vector3.Dot(toDoor, toPlayer.normalized) < 0.3f || toPlayer.magnitude < distToDoor)
                    {
                        return; // 直接退出，不设置 isDoorBreaking
                    }
                }

                float halfAngle = enemyBlackBoard.halfAngle;
                float dot = Vector3.Dot(AIForward, toDoor);
                float threshold = Mathf.Cos(halfAngle * Mathf.Deg2Rad);

                if (dot >= threshold && !isOpen)
                {
                    isAIInteract = true;
                    enemyBlackBoard.isDoorBreaking = true;
                    enemyBlackBoard.door = this;
                }
            }
        }

        // 【铁律3】：AI 离开门的范围时，自动擦掉踢门状态
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("AI"))
            {
                EnemyBlackBoard enemyBlackBoard = other.GetComponent<EnemyBlackBoard>();
                if (enemyBlackBoard != null && enemyBlackBoard.door == this)
                {
                    enemyBlackBoard.ClearDoorBreak();
                }
            }
        }
    }
}