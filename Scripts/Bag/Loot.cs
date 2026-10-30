using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ns
{
    public class Loot : MonoBehaviour, IInteractable
    {
        public ItemSO itemSO;
        public bool isGet;
        public int pickUpQuantity = 1;

     

        [HideInInspector]
        public float ikFadeOutTime = 0.2f;

      

        private void OnEnable()
        {
            isGet = false;

        }

       

        public virtual void AfterMethod() { }

        public void LootPickUp(ItemSO itemSO)
        {
            if (isGet) return;

            if (!UIManager.Instance.bagManager.IsBagFull(itemSO))
            {
                isGet = true;
                PlayerCore.Instance.playerAnimEvent.lootRightRig.weight = 0.7f;
                PlayerCore.Instance.anim.SetTrigger(PlayerCore.Instance.animPara.pickUpAnim);
                UIManager.Instance.bagManager.AddItem(itemSO, pickUpQuantity);
                AfterMethod();
            }
        }

        public void TryInteract()
        {
            LootPickUp(itemSO);
            PlayerCore.Instance.playerAnimEvent.loot = this;
        }
    }
}