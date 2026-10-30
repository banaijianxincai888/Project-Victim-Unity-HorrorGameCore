using System.Collections;
using UnityEngine;

namespace ns
{
    ///summary
    ///丢下箱子
    ///summary 
    public class DropBox : MonoBehaviour
    {
        public AK.Wwise.Event dropSound;

        public ItemSO boxItemSO;
        private void Start()
        {
            EventCenter.LootEvent.OnBoxDrop += Drop;
        }

        private void OnDestroy()
        {
            EventCenter.LootEvent.OnBoxDrop -= Drop;
        }


        public void Drop()
        {
            if(!PlayerCore.Instance.playerMovement.isLifting) { return; }
            PlayerCore.Instance.playerMovement.isLifting = false;
            PlayerCore.Instance.anim.SetBool(PlayerCore.Instance.animPara.liftAnim, false);
            PlayerCore.Instance.playerMovement.currentBox.gameObject.SetActive(true);
            dropSound.Post(gameObject);
            if(PlayerCore.Instance.playerMovement.canPlace)
            {
                PlayerCore.Instance.playerMovement.plate.PlaceBox();
            }
            else
            {
                PlayerCore.Instance.playerMovement.currentBox.position = PlayerCore.Instance.playerMovement.dropBoxTF.position;
            }
            PlayerCore.Instance.playerMovement.currentBox = null;
            UIManager.Instance.bagManager.UseItem(boxItemSO, 1);
            transform.gameObject.SetActive(false);

        }



       
    }
}

