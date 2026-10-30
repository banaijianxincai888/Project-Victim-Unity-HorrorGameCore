using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace ns
{
    ///summary
    ///压力板解密
    ///summary 
    public class Decryption_Plate : DecryptionItem
    {
        public Transform contentObject;

        public Decryption_CBreaker cBreaker;

        public float changeTime = 0.5f;

        public float changeDistance = 0.05f;


        public Transform[] boxPoints;


        private Coroutine coroutine;

        public int index;

        private Vector3 originalPos;

        private void Start()
        {
            originalPos = contentObject.position;
        }



        private void OnTriggerEnter(Collider other)
        {
            if(isSolved) { return; }
            if (other.CompareTag("Player"))
            {
                SoundManager.Instance.pressUp.Post(gameObject);
                if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    StartCoroutine(SmoothChange(true));
              
                PlayerCore.Instance.playerMovement.canPlace = true;

            }


        }

        private void OnTriggerStay(Collider other)
        {
            if (isSolved) { return; }
            if (other.CompareTag("Player"))
            {
                if (index > boxPoints.Length - 1)
                {
                    if(cBreaker.isSolved)
                        UIManager.Instance.decryptionManager.SolveDecryption(this);
                }

            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (isSolved) { return; }
            if (other.CompareTag("Player"))
            {
                SoundManager.Instance.pressDown.Post(gameObject);
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                StartCoroutine(SmoothChange(false));
                PlayerCore.Instance.playerMovement.canPlace = false;

            }



        }





        IEnumerator SmoothChange(bool isOpen)
        {
            AK.Wwise.Event @event = isOpen? SoundManager.Instance.DrawerOpen: SoundManager.Instance.DrawerClose;
            @event.Post(gameObject);

            // 目标位置偏移方向：打开（玩家进入）为正X，关闭（玩家离开）为负X
            float direction = isOpen ? 1f : -1f;
            float targetOffset = direction * changeDistance;

            // 记录起始位置（保证移动的基准点不变）
            Vector3 startPos = contentObject.position;
            Vector3 targetPos = Vector3.zero;
            // 目标位置 = 起始位置 + X轴偏移
            if (isOpen)
                targetPos = new Vector3(originalPos.x + targetOffset, originalPos.y, originalPos.z);
            else
                targetPos = originalPos;

            float timer = 0f;
            while (timer < changeTime)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / changeTime); // 进度 = 已用时间 / 总时间
                // 从起始位置到目标位置线性插值
                contentObject.position = Vector3.Lerp(startPos, targetPos, progress);
                yield return null;
            }

            // 确保最终位置精准
            contentObject.position = targetPos;
            coroutine = null;

        }

        public override void SolveDecryption()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            StartCoroutine(SmoothChange(true));
        }

        public override void TryInteract()
        {
            if (isSolved) { return; }

            

            if (!UIManager.Instance.bagManager.HasItem(decryptionSO.needItem))
            {
                UIManager.Instance.decryptionManager.ActiveDecryption(this);
            }




        }


        public void PlaceBox()
        {

            PlayerCore.Instance.playerMovement.currentBox.tag = "Untagged";
            MaterialEmission materialEmission = PlayerCore.Instance.playerMovement.currentBox.GetComponent<MaterialEmission>();
            materialEmission.enableBlink = false;
            materialEmission.emissionColor = Color.black;

            PlayerCore.Instance.playerMovement.currentBox.position = boxPoints[index].position;
            PlayerCore.Instance.playerMovement.currentBox.rotation = boxPoints[index].rotation;
            index++;
           

        }
    }
}

