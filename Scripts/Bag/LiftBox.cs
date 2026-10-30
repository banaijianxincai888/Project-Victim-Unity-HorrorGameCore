using System.Collections;
using UnityEngine;

namespace ns
{
    ///summary
    ///搬起箱子
    ///summary 
    public class LiftBox : MonoBehaviour, IInteractable
    {

        public ItemSO boxItemSo;

        public QuestTipSO firstLiftTipSO;

        public void TryInteract()
        {
            if(!UIManager.Instance.bagManager.hasPickUpBox)
            {
                UIManager.Instance.questTipManager.ShowQuestTip(firstLiftTipSO);
            }
            boxItemSo.pickUpSound.Post(gameObject);
            PlayerCore.Instance.playerMovement.isLifting = true;
            PlayerCore.Instance.anim.SetBool(PlayerCore.Instance.animPara.liftAnim, true);
            PlayerCore.Instance.playerMovement.currentBox = transform;
            UIManager.Instance.bagManager.AddItem(boxItemSo, 1);
            transform.gameObject.SetActive(false);
            PlayerCore.Instance.playerMovement.carryBoxTF.gameObject.SetActive(true);
            UIManager.Instance.bagManager.hasPickUpBox = true;

        }

      
    }
}

