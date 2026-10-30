using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI; // 新增引用

namespace ns
{
    public class SavePoint : MonoBehaviour
    {
        [Header("UI引用")]
        public BagManager bagManager;

        public NightVisionManager nightVisionManager;

        [Header("电闸机关")]
        public Decryption_CBreaker decryption_cbreaker;

        [Header("门引用")]
        public Door[] doors;

        [Header("陷阱引用")]
        public BearTrap[] bearTraps;

        [Header("其他普通需要保存的游戏对象（如玩家、AI）")]
        public GameObject[] objectsToSave;

        public string checkpointFileName = "GameData/SaveData/checkpoint.es3";

        public void SaveCheckpoint()
        {
            //分帧保存数据，以免掉帧
            StartCoroutine(SaveCheckpointRoutine());
        }

        private IEnumerator SaveCheckpointRoutine()
        {
            BoxCollider box = transform.GetComponent<BoxCollider>();
            box.enabled = false;
            BagSaveData bagData = bagManager != null ? bagManager.GetSaveData() : null;
            var cacheSettings = new ES3Settings(checkpointFileName, ES3.Location.Cache);

            if (bagData != null) ES3.Save("BagSaveData", bagData, cacheSettings);

            if (nightVisionManager != null)
                ES3.Save("BatteryPower", nightVisionManager.battery.value, cacheSettings);

            foreach (GameObject obj in objectsToSave)
            {
                if (obj == null) continue;

                ES3.Save(obj.name, obj, cacheSettings);
                yield return null; // 关键：每帧只存一个
            }

            Task.Run(() => ES3.StoreCachedFile(checkpointFileName));

            yield return null;
            Debug.Log("✅ 检查点保存完成");
        }

        public void LoadCheckpoint()
        {
            foreach (GameObject obj in objectsToSave)
            {
                if (obj != null && ES3.KeyExists(obj.name, checkpointFileName))
                {
                    CharacterController cc = obj.GetComponent<CharacterController>();
                    PlayerMovement pm = obj.GetComponent<PlayerMovement>();
                    EnemyBlackBoard enemyBlackBoard = obj.GetComponent<EnemyBlackBoard>();//获取AI组件

                    // 1. 加载前先禁用碰撞/寻路组件，防止覆盖加载的位置
                    if (cc != null) cc.enabled = false;
                    if (enemyBlackBoard != null) enemyBlackBoard.agent.enabled = false;

                    // 2. 加载坐标、血量等数据
                    ES3.LoadInto(obj.name, checkpointFileName, obj);

                    // 3. 恢复玩家逻辑
                    if (pm != null)
                    {
                        //因为E3加载时使用无参数的反射，所以不能保存有参数的状态机构造函数，否则会空引用
                        pm.ForceWalkState();
                    }
                    if (cc != null) cc.enabled = true;

                    // 4. 恢复AI位置逻辑 (关键修复)
                    if (enemyBlackBoard != null)
                    {
                        // 重新启用 Agent，否则 Warp 无效
                        enemyBlackBoard.agent.enabled = true;

                        // 使用 Warp 强制传送，防止 Agent 内部缓存导致位置错乱
                        Vector3 savedPos = obj.transform.position;
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(savedPos, out hit, 2f, NavMesh.AllAreas))
                        {
                            enemyBlackBoard.agent.Warp(hit.position);
                        }
                        else
                        {
                            enemyBlackBoard.agent.Warp(savedPos);
                        }
                        //使用AI的重置方法
                        enemyBlackBoard.ResetBlackBoard();
                    }
                }
            }
            // 2. 恢复背包数据
            if (bagManager != null && ES3.KeyExists("BagSaveData", checkpointFileName))
            {
                BagSaveData bagData = ES3.Load<BagSaveData>("BagSaveData", checkpointFileName);
                bagManager.RestoreFromSaveData(bagData);
            }
            //恢复灯光状态
            if (!bagManager.hasPickUpFusil)
            {
                decryption_cbreaker.TurnOnLights();
            }
            else
            {
                decryption_cbreaker.TurnOffLights();
            }

            //恢复电池电量
            if (nightVisionManager != null)
            {
                nightVisionManager.battery.value = ES3.Load<float>("BatteryPower", checkpointFileName);
            }

            //不用保存门的状态，只是把AI干扰删除即可
            if (doors != null)
            {
                foreach (Door door in doors)
                {
                    door.isAIInteract = false;
                }
            }

            //恢复血液UI
            PlayerCore.Instance.playerHealth.ActiveBloodUI();

            //恢复捕熊夹陷阱
            foreach (BearTrap bearTrap in bearTraps)
            {
                bearTrap.RecoverTrap();
            }

            //修复一下选中的按钮，防止误触发！
            EventSystem.current.SetSelectedGameObject(null);

            Debug.Log("✅ 检查点已成功加载！");
        }

        public void DeleteSaveData()
        {
            if (ES3.FileExists(checkpointFileName))
            {
                ES3.DeleteFile(checkpointFileName);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SaveCheckpoint();
            }
        }
    }
}