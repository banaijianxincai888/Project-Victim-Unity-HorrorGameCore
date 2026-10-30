using BehaviorDesigner.Runtime;
using Cinemachine;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace ns
{
    ///summary
    ///储物柜脚本
    ///summary 
    public class Cabinet :Hideout,IInteractable,ISearchHideout
    {

        public float openAngle = 90f;

        public float openTime = 0.5f;

        public Transform rightDoor;

        public Transform leftDoor;

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                hideoutInfo.isInside = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                hideoutInfo.isInside = false;
            }
        }

        public void TryInteract()
        {
            if(hideoutInfo .isAIInteract ) { return; }
            if (!hideoutInfo.isInside) { return; }
            ActiveHideState();

        }

        public IEnumerator DoorControl(float controlTime, bool isOpen)
        {
            // 重点看这里：到底是哪个柜子在执行开门动画！
            Debug.Log($"[正在转动门的柜子是]: {gameObject.name}，控制的门物体是: {leftDoor.name}, {rightDoor.name}", gameObject);
            AK.Wwise.Event @event = isOpen ? SoundManager.Instance.cabinetOpen : SoundManager.Instance.cabinetClose;
            @event.Post(gameObject);
            float timer = 0;
            Quaternion startRight = rightDoor.rotation;
            Quaternion startLeft = leftDoor.rotation;
            // 在 start 的基础上，绕局部 Z 轴旋转 +90° 或 -90°
            float deltaAngle = isOpen ? (-1 * openAngle) : openAngle;
            Quaternion endRight = startRight * Quaternion.Euler(0, 0, deltaAngle);
            Quaternion endLeft = startLeft * Quaternion.Euler(0,0, deltaAngle);
            while (timer < controlTime)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / controlTime);
                rightDoor.rotation = Quaternion.Lerp(startRight, endRight, progress);
                leftDoor.rotation = Quaternion.Lerp(startLeft, endLeft, progress);
                yield return null;
            }
            rightDoor.rotation = endRight;
            leftDoor.rotation = endLeft;
        }

        public HideoutInfo SearchHideoutPoint()
        {
            return hideoutInfo;
        }
    }
}

