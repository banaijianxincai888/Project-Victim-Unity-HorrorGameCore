using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace ns
{   
	///summary
	///补熊夹陷阱
	///summary 
	public class BearTrap :MonoBehaviour
	{
        public float hurtValue = 5f;

        public float hurtInterval = 0.4f;

        private float hurtTimer = 0;
        private Animator anim;

        public string activePara;

        public string lockPara;

        public bool isLock;

        private BoxCollider boxCollider;

        public AK.Wwise.Event activeSound;

        public AK.Wwise.Event lockSound;


        private void Start()
        {
            anim = GetComponent<Animator>();
            boxCollider  = GetComponent<BoxCollider>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                PlayerCore.Instance.playerMovement.isShaking = true;
                PlayerCore.Instance.playerMovement.currentTrap = gameObject;
                activeSound.Post(gameObject);
                anim.SetTrigger(activePara);
            }
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {


                if (isLock || PlayerCore.Instance.playerMovement.isDeath) return;

               if(!PlayerCore.Instance.playerMovement.isShaking)
                {
                    isLock = true;
                    anim.SetBool(lockPara,true);
                    lockSound.Post(gameObject);
                    PlayerCore.Instance.playerHealth.StartRecoverHP();
                    return;
                }
                if (Time.time - hurtTimer > hurtInterval)
                {
                    PlayerCore.Instance.playerSoundEmission.StopSound();
                    PlayerCore.Instance.playerHealth.Hurt(hurtValue, false);
                    hurtTimer = Time.time;
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
               boxCollider.enabled = false;
                StartCoroutine(HideTrap());
            }
        }

        IEnumerator HideTrap()
        {
            yield return new WaitForSeconds(TrapManager.Instance. collectTime);
            TrapManager.Instance.trapCollectios.Add(gameObject);
            gameObject.SetActive(false);
        }

        public  void ResetTrap( Transform resetTransform)
        {
            //控制显隐之前要把位置等先设置好，不然会受到动画影响从而发生位置错乱
            transform.SetPositionAndRotation(resetTransform.position, resetTransform.rotation);
            UIManager.Instance.exitTrapManager.progress.value = 0;
            boxCollider.enabled = true;
            isLock = false;
            TrapManager.Instance.trapCollectios.Remove(gameObject);

            anim.SetBool(activePara, false);
            anim.SetBool(lockPara, false);
            gameObject.SetActive(true);

        }

        public void RecoverTrap()
        {
            StopAllCoroutines();
            isLock = false;
            boxCollider.enabled = true;
            anim.SetBool(activePara, false);
            anim.SetBool(lockPara, false);
            gameObject.SetActive(true);

        }


    }
}

