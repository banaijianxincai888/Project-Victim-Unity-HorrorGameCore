using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI; // 新增引用

namespace ns
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public float spawnTime = 60f;
        public float readyTime = 10f;

        public Transform[] spawnPoints;
        public Transform[] readyCheckPoints;
        public ForceSpawnPoint[] forceSpawnPoiints;
        public ForceSpawnPoint currentForceSpawnPoint;

        public Transform enemy;

        private EnemyBlackBoard enemyBlackBoard;
        public int checkPointsCount;

        private void Start()
        {
            enemyBlackBoard = enemy.GetComponent<EnemyBlackBoard>();
        }

        public void ForceSpawn()
        {
            enemy.gameObject.SetActive(true);

            // 【修复1】先启用 Agent，再设置位置，否则 Warp 不生效
            enemyBlackBoard.agent.enabled = true;

            GetCloseSpawnPoint();
            enemyBlackBoard.tree.EnableBehavior();
        }

        private void GetCloseSpawnPoint()
        {
            float distance = 0;
            Transform player = PlayerCore.Instance.transform;
            float startDistance = Vector3.Distance(forceSpawnPoiints[0].triggerItem.position, player.position);
            currentForceSpawnPoint = forceSpawnPoiints[0];

            foreach (ForceSpawnPoint forceSpawnPoiint in forceSpawnPoiints)
            {
                distance = Vector3.Distance(forceSpawnPoiint.triggerItem.position, player.position);
                if (distance < startDistance)
                {
                    startDistance = distance; // 修复原代码这里没更新距离的BUG
                    currentForceSpawnPoint = forceSpawnPoiint;
                }
            }

            // 【修复2】用 Warp 代替直接修改 position
            Vector3 targetPos = currentForceSpawnPoint.forceSpawnPoint.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPos, out hit, 2f, NavMesh.AllAreas))
            {
                enemyBlackBoard.agent.Warp(hit.position);
            }
            else
            {
                enemyBlackBoard.agent.Warp(targetPos);
            }

            enemyBlackBoard.checkPoints = currentForceSpawnPoint.forceCheckPoints;
        }

        public void RandomSpawn(float spawnTime)
        {
            enemyBlackBoard.gameObject.SetActive(false);
            StartCoroutine(WaitSpawn(spawnTime));
        }

        IEnumerator WaitSpawn(float spawnTime)
        {
            yield return new WaitForSeconds(spawnTime);
            enemyBlackBoard.agent.enabled = false;
            enemyBlackBoard.tree.DisableBehavior();

            enemy.gameObject.SetActive(true);
            RandomSpawn();
            RandomCreateCheckPoints();
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.coldAnimPara, true);
            StartCoroutine(WaitingReady());
        }

        IEnumerator WaitingReady()
        {
            yield return new WaitForSeconds(readyTime);
            enemyBlackBoard.anim.SetBool(enemyBlackBoard.zombieAnimEvent.zombieAnimPara.coldAnimPara, false);
            enemyBlackBoard.agent.enabled = true;
            enemyBlackBoard.tree.EnableBehavior();
        }

        public void RandomSpawn()
        {
            System.Random rand = new System.Random();
            int index = rand.Next(0, spawnPoints.Length);
            Vector3 targetPos = spawnPoints[index].position;

            // 【修复3】判断 Agent 状态。如果在 WaitSpawn 里被禁用了，直接用 transform，否则用 Warp
            if (enemyBlackBoard.agent.isActiveAndEnabled)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(targetPos, out hit, 2f, NavMesh.AllAreas))
                {
                    enemyBlackBoard.agent.Warp(hit.position);
                }
                else
                {
                    enemyBlackBoard.agent.Warp(targetPos);
                }
            }
            else
            {
                enemy.transform.position = targetPos;
            }
        }

        public void RandomCreateCheckPoints()
        {
            enemyBlackBoard.checkPoints = GetRandomElements(readyCheckPoints, checkPointsCount);
        }

        public Transform[] GetRandomElements(Transform[] array, int count)
        {
            if (array == null || array.Length == 0) return new Transform[0];

            int resultCount = Mathf.Min(count, array.Length);
            Transform[] copy = (Transform[])array.Clone();

            for (int i = 0; i < resultCount; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, copy.Length);
                Transform temp = copy[i];
                copy[i] = copy[randomIndex];
                copy[randomIndex] = temp;
            }

            Transform[] result = new Transform[resultCount];
            for (int i = 0; i < resultCount; i++)
            {
                result[i] = copy[i];
            }
            return result;
        }
    }

    [Serializable]
    public class ForceSpawnPoint
    {
        public Transform triggerItem;
        public Transform forceSpawnPoint;
        public Transform[] forceCheckPoints;
    }
}