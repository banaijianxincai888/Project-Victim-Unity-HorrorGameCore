using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ns
{   
	///summary
	///玩家输入事件
	///summary 
	public class PlayerInputEvent : MonoBehaviour
	{



        public void NightVisionMethod()
        {

            EventCenter.NightVisionEvent.OnNightVisionRequest?.Invoke();
        }

        // 2. 修改方法签名，允许接收 InputAction.CallbackContext
        public void InteractMethod(InputAction.CallbackContext context)
        {
            // 3. 核心防御：如果不是“完成按下”阶段，直接拦截，不执行后续逻辑
            if (!context.performed) return;

            if (PlayerCore.Instance.isAniming) return;

            IInteractable[] interactables = EventCenter.CameraEvent.OnRayCheck?.Invoke();
            if (interactables != null)
            {
                foreach (var interactable in interactables)
                {
                    interactable.TryInteract();
                }
            }
            
            
        }

        public void ReloadBatteryMethod(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (PlayerCore.Instance.isAniming) return;

            EventCenter.NightVisionEvent.OnRecoverBatteryRequest?.Invoke();
        }

        

       

        public void OnMouseWheelMethod(InputAction.CallbackContext context)
        {
            float scroll =PlayerCore.Instance.playerMovement.autoGenerate.Player.MouseWheel.ReadValue<float>();
            // 过滤掉回弹的 0 值，只有滚轮真正滚动时才执行
            if (Mathf.Abs(scroll) > 0.01f)
            {
                UIManager.Instance.bagManager.ShowBag(scroll);
            }

        }

        public void DropBoxMethod(InputAction.CallbackContext context)
        {
            if(!PlayerCore.Instance.playerMovement.isLifting) return;
            if (!context.performed) return;

            if (PlayerCore.Instance.isAniming) return;

            EventCenter.LootEvent.OnBoxDrop?.Invoke();
        }

        public void GameStartMethod(InputAction.CallbackContext context)
        {
            if (ES3.FileExists(UIManager.Instance.coverUIManager.savePoint.checkpointFileName)) return;
            if (PlayerCore.Instance.isAniming) return;
            if (!context.performed) return;
            EventCenter.GameFlowEvent.OnGameStartButton?.Invoke();
        }

        public void GamePauseMethod(InputAction.CallbackContext context)
        {
            if (UIManager.Instance.coverUIManager.isCovering) return;
            if (PlayerCore.Instance.isAniming) return;
            if (!context.performed) return;
            EventCenter.GameFlowEvent.OnGamePauseButton?.Invoke();
        }

        public void ExitTrapMethod(InputAction.CallbackContext context)
        {
            if(!PlayerCore.Instance.playerMovement.isShaking) return;
            if (!context.performed) return;

            if (PlayerCore.Instance.isAniming) return;
            EventCenter.TrapEvent.OnActiveTrap?.Invoke();
        }
    }
}

