using UnityEngine;

namespace ns
{
    ///summary
    ///解密交互对象
    ///summary 
    public class DecryptionItem : MonoBehaviour, IInteractable,ISolveDecryption
    {
        public DecryptionSO decryptionSO;

        public bool isSolved;

        public virtual void SolveDecryption( )
        {
            UIManager.Instance.bagManager.UseItem(decryptionSO.needItem, decryptionSO.needCounts);
        }

        public virtual void TryInteract()
        {
            if (!UIManager.Instance.bagManager.HasItem(decryptionSO.needItem) || 
                UIManager.Instance.bagManager.CheckitemSOCount(decryptionSO.needItem) < decryptionSO.needCounts) 
            {
                UIManager.Instance.decryptionManager.ActiveDecryption(this);
            }
            else
            {
                UIManager.Instance.decryptionManager.SolveDecryption(this);
            }
        }

        
      
    }
}

