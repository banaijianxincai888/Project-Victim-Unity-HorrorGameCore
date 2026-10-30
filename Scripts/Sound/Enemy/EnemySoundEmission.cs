using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    public class EnemySoundEmission : MonoBehaviour
    {
        public AK.Wwise.Event footStep;

        public AK.Wwise.Event axe;

        public DialogueSO[] pathcrolDialogueSOs;
        public DialogueSO[] killDialogueSOs;
        public DialogueSO[] findDialogueSOs;
        public DialogueSO[] findHideDialogueSOs;
        public DialogueSO[] lostDialogueSOs;
        public DialogueSO[] idleDialogueSOs;
        public DialogueSO[] pursuitDialogueSOs;
        public DialogueSO[] attackDialogueSOs;

        public bool hasPlayPursuit;

        public List<DialogueSO> _shuffledDialogueList = new List<DialogueSO>();
        public int _currentIndex = 0;
        private DialogueSO _lastPlayedSO = null;

        // 新增：上次传入数组的内容快照（做拷贝，避免外部数组被修改时比较失效）
        private DialogueSO[] _sourceSnapshot;

        

        public void SoundEmission(DialogueSO[] selectDialogueSO)
        {
            // 1. 防御校验
            if (selectDialogueSO == null || selectDialogueSO.Length == 0)
            {
                Debug.LogWarning($"[{gameObject.name}] SoundEmission: 传入的 DialogueSO 数组为空或长度为0！");
                return;
            }

            // 2. 只要内容跟上次不一样，就重新洗牌
            if (!IsSameSource(selectDialogueSO))
            {
                RebuildAndShuffle(selectDialogueSO);
            }

            // 3. 一轮全部播完后，重新洗牌开启新一轮
            if (_currentIndex >= _shuffledDialogueList.Count)
            {
                RebuildAndShuffle(selectDialogueSO);
            }

            // 4. 取当前台词
            DialogueSO chosenSO = _shuffledDialogueList[_currentIndex];
            _currentIndex++;
            _lastPlayedSO = chosenSO;

            // 5. 防御 null 槽位
            if (chosenSO == null)
            {
                Debug.LogWarning($"[{gameObject.name}] SoundEmission: 抽取的 DialogueSO 为空，请检查 Inspector 配置！");
                return;
            }

            UIManager.Instance.dialogueManager.PlayDialogue(chosenSO, gameObject);
        }

        /// <summary>
        /// 判断传入的数组内容是否与上次快照一致
        /// </summary>
        private bool IsSameSource(DialogueSO[] source)
        {
            if (_sourceSnapshot == null || _sourceSnapshot.Length != source.Length)
                return false;

            for (int i = 0; i < source.Length; i++)
            {
                if (_sourceSnapshot[i] != source[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// 核心：费雪-耶茨（Fisher-Yates）洗牌算法
        /// </summary>
        private void RebuildAndShuffle(DialogueSO[] sourceArray)
        {
            _shuffledDialogueList.Clear();
            _shuffledDialogueList.AddRange(sourceArray);
            _currentIndex = 0;

            // 保存内容快照（必须是拷贝，否则外部数组变了快照也跟着变，比较会失效）
            if (_sourceSnapshot == null || _sourceSnapshot.Length != sourceArray.Length)
                _sourceSnapshot = new DialogueSO[sourceArray.Length];
            System.Array.Copy(sourceArray, _sourceSnapshot, sourceArray.Length);

            if (_shuffledDialogueList.Count <= 1) return;

            // Fisher-Yates 乱序
            for (int i = _shuffledDialogueList.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                DialogueSO temp = _shuffledDialogueList[i];
                _shuffledDialogueList[i] = _shuffledDialogueList[randomIndex];
                _shuffledDialogueList[randomIndex] = temp;
            }

            // 防尴尬：避免新一轮第一句 = 上一轮最后一句
            if (_lastPlayedSO != null && _shuffledDialogueList[0] == _lastPlayedSO && _shuffledDialogueList.Count > 1)
            {
                int swapIndex = Random.Range(1, _shuffledDialogueList.Count);
                DialogueSO temp = _shuffledDialogueList[0];
                _shuffledDialogueList[0] = _shuffledDialogueList[swapIndex];
                _shuffledDialogueList[swapIndex] = temp;
            }
        }

        public void StopSound()
        {
            UIManager.Instance.dialogueManager.StopDialogue();
        }
      
    }
}