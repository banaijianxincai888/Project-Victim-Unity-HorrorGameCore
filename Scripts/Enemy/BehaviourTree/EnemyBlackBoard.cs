using BehaviorDesigner.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

namespace ns
{
    /// <summary>
    /// 敌人黑板变量（集中状态管理）
    /// </summary>
    public class EnemyBlackBoard :MonoBehaviour
    {


        [Header("Parameters")]
        public float rotateSpeed = 120f;

        [Header("References")]

        public Transform player;
        public NavMeshAgent agent;
        public Animator anim;
        public ZombieAnimEvent zombieAnimEvent;
        public BehaviorTree tree;

        public EnemySpawnManager spawnManager;

        public EnemySoundEmission soundEmission;

        [Header("前方视野配置")]
        public float viewRadius = 6f;
        [Range(0, 360)] public float viewAngle = 150f;
        public int rayCount = 5;

        [Header("后方警戒配置")]
        public float alarmRadius = 2f;// 背后射线的检测距离
        [Range(0, 360)] public float backViewAngle = 210f;// 背后扇形角度
        public int backRayCount = 3; // 背后发射的射线数量

        [Header("通用配置")]
        public float sectorYOffset = 1.3f;
        public float lostTime = 8f;
        public LayerMask layerMask;

        // 射线检测间隔配置与状态缓存
        public float raycastInterval = 1f;
        private float lastRaycastTime = -1f;
        private bool isPlayerVisibleCached = false;

        [Header("Detection State")]
        public Vector3 lastSeenPosition;       // 最后一次看见玩家的位置

        [Header("Loss & Search")]
        public bool hasLostPlayer;              // 当前是否处于丢失状态
        public bool searchCompleted;
        public bool hasMoveToLastPlayerPos;
        public bool hasSeen;
        [HideInInspector]
        public float timer;

        [Header("Hideout")]
        public HideoutInfo currentHideInfo;
    

        [Header("SetTrap")]
        public Transform setTrapTF;


        [Header("DoorBreak")]
        public bool isDoorBreaking;

        public float halfAngle = 30f;
        public Door door;

        [Header("Attack")]
        public float atk = 20f;
        public float atkDistance = 2f;

        public float coolTime = 5f;

        [Header("Patrol")]

        public Transform[] checkPoints;


        //重置黑板，供存档系统使用
        public void ResetBlackBoard()
        {
            isPlayerVisibleCached = false;
            lastSeenPosition = Vector3.zero;
            hasLostPlayer = false;
            searchCompleted = false;
            hasMoveToLastPlayerPos = false;
            hasSeen = false;
            currentHideInfo = null;
            isDoorBreaking = false;
            door = null;
            soundEmission.StopSound();
        }



        // ===== 核心：全局视野射线检测方法（供 isFinding 和 isDoorBreaking 共同调用） =====

        /// <summary>
        /// 重置射线冷却，确保节点刚启动时能立即检测一次
        /// </summary>
        public void ResetRaycastTimer()
        {
            lastRaycastTime = -raycastInterval;
            isPlayerVisibleCached = false;
        }

        /// <summary>
        /// 执行前后扇形射线检测（内置  冷却缓存，多处调用也不会重复耗性能）
        /// 如果直接看到玩家，会自动触发 OnPlayerDetected()、更新 timer 和 hasSeen，并取消破门状态
        /// </summary>
        public bool CheckPlayerVisibility()
        {
            if (player == null) return false;

            float dist = Vector3.Distance(player.position, transform.position);
            float maxDetectRadius = Mathf.Max(viewRadius, alarmRadius);

            // 只有在最大检测范围内才考虑发射射线
            if (dist <= maxDetectRadius)
            {
                // 到达指定检测周期（1）才重新发射物理射线
                if (Time.time - lastRaycastTime >= raycastInterval)
                {
                    lastRaycastTime = Time.time;
                    isPlayerVisibleCached = IsPlayerDetected(dist);
                }
            }
            else
            {
                // 超出半径直接置为不可见
                isPlayerVisibleCached = false;
            }

            // 如果当前直接看到了玩家，立即刷新发现状态并清除破门标志
            if (isPlayerVisibleCached)
            {
                OnPlayerDetected();
                timer = Time.time;
                hasSeen = true;
            }

            return isPlayerVisibleCached;
        }

        private bool IsPlayerDetected(float currentDist)
        {
            Vector3 center = transform.position + Vector3.up * sectorYOffset;

            // 1. 检测正前方扇形
            if (currentDist <= viewRadius)
            {
                if (CastSectorRays(center, transform.forward, viewAngle, viewRadius, rayCount))
                    return true;
            }

            // 2. 检测正后方扇形
            if (currentDist <= alarmRadius)
            {
                if (CastSectorRays(center, -transform.forward, backViewAngle, alarmRadius, backRayCount))
                    return true;
            }

            return false;
        }

        private bool CastSectorRays(Vector3 center, Vector3 baseDirection, float angle, float radius, int count)
        {
            if (count <= 0 || radius <= 0f) return false;

            float half = angle * 0.5f;
            float startAngle = -half;
            float endAngle = half;

            for (int i = 0; i < count; i++)
            {
                float t = count == 1 ? 0.5f : (float)i / (count - 1);
                float currentAngle = Mathf.Lerp(startAngle, endAngle, t);
                Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * baseDirection;

                // 忽略触发器 (QueryTriggerInteraction.Ignore)
                if (Physics.Raycast(center, direction, out RaycastHit hit, radius, layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.CompareTag("Player"))
                        return true;
                }
            }
            return false;
        }




        // ===== 状态变更方法 =====
        public void OnPlayerDetected()
        {
            hasLostPlayer = false;
            searchCompleted = false;

            // 【新增】铁律1：既然射线直接看到了玩家，中间肯定没门挡着，立刻擦掉踢门指令！
            ClearDoorBreak();
        }

        // 【新增】专门用来擦掉踢门状态的辅助方法
        public void ClearDoorBreak()
        {
            isDoorBreaking = false;
            if (door != null)
            {
                door.isAIInteract = false;
                door = null;
            }
        }

        public void OnPlayerLost(Vector3 seenPos)
        {
            hasLostPlayer = true;
            // 重置移动到最后位置的标志（因为新的丢失需要重新移动）
            hasMoveToLastPlayerPos = false;
            
            // 重置搜索完成标志，允许重新搜索
            searchCompleted = false;
            lastSeenPosition = seenPos;
            soundEmission.hasPlayPursuit = false;
            BGMManager.Instance.ChangeBGM(BGMManager.Instance.commonBGM);
            PlayerCore.Instance.playerSoundEmission.StopSound();
            PlayerCore.Instance.playerHealth.StartRecoverHP();
           
        }

        public void OnSearchCompleted()
        {
            searchCompleted = true;
            hasLostPlayer = false;   // 搜索完成后不再处于丢失状态
            // 搜索完成回到巡逻，重置尖叫标志，以便下次看到重新尖叫
            // 重置移动到最后位置标志
            hasMoveToLastPlayerPos = false;
        }

        private void Update()
        {
            // 1. 黑板自己每帧跑视野检测（内部有 冷却锁，平时不耗性能）
            // 无论 AI 正在巡逻、正在踢门、正在追人、还是正在设陷阱，全天候都能感知玩家！
            if (CheckPlayerVisibility())
            {
                return; // 看到玩家了，CheckPlayerVisibility 内部已经把 hasSeen 设为 true 并清掉了踢门
            }

            // 2. 如果之前看到过玩家，但现在视线被挡住了，由黑板统一倒计时 
            if (hasSeen && Time.time - timer >= lostTime)
            {
                OnPlayerLost(player.position);
                hasSeen = false;
            }
        }

       

#if UNITY_EDITOR
        // 在 Scene 窗口选中敌人即可看到前后视野扇形与射线
        private void OnDrawGizmos()
        {
            Vector3 center = transform.position + Vector3.up * sectorYOffset;
            Vector3 forward = transform.forward;
            Vector3 backward = -forward;

            // 绘制前方扇形（淡红色）
            DrawSectorGizmo(center, forward, viewAngle, viewRadius, rayCount, new Color(1f, 0f, 0f, 0.2f), Color.red);

            // 绘制后方扇形（淡黄色）
            DrawSectorGizmo(center, backward, backViewAngle, alarmRadius, backRayCount, new Color(1f, 0.92f, 0.016f, 0.2f), Color.yellow);
        }

        private void DrawSectorGizmo(Vector3 center, Vector3 baseDirection, float angle, float radius, int count, Color arcColor, Color defaultRayColor)
        {
            if (count <= 0 || radius <= 0f) return;

            Vector3 leftBound = Quaternion.Euler(0, -angle / 2f, 0) * baseDirection;
            Vector3 rightBound = Quaternion.Euler(0, angle / 2f, 0) * baseDirection;

            Handles.color = arcColor;
            Handles.DrawSolidArc(center, Vector3.up, leftBound, angle, radius);
            Handles.DrawLine(center, center + leftBound * radius);
            Handles.DrawLine(center, center + rightBound * radius);

            if (Application.isPlaying && player != null)
            {
                float half = angle * 0.5f;
                float startAngle = -half;
                float endAngle = half;

                for (int i = 0; i < count; i++)
                {
                    float t = count == 1 ? 0.5f : (float)i / (count - 1);
                    float currentAngle = Mathf.Lerp(startAngle, endAngle, t);
                    Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * baseDirection;

                    bool hitPlayer = false;
                    float hitDistance = radius;
                    if (Physics.Raycast(center, direction, out RaycastHit hit, radius, layerMask, QueryTriggerInteraction.Ignore))
                    {
                        hitDistance = hit.distance;
                        if (hit.collider.CompareTag("Player"))
                            hitPlayer = true;
                    }
                    Gizmos.color = hitPlayer ? Color.green : defaultRayColor;
                    Gizmos.DrawLine(center, center + direction * hitDistance);
                }
            }
        }
#endif
    }




}
