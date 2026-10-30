using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace ns
{   
	///summary
	///关门陷阱
	///summary 
	public class CloseDoor : MonoBehaviour
	{
		public Door door;

		public DecryptionSO closeGate;

		private BoxCollider boxCollider;

		public Volume globalVolume;

		public Light directionLight;



        private void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
        }



        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
			{
				door.isLock  = true;
				Decryption_Gate decryption_Gate = door.transform.AddComponent<Decryption_Gate>();
				decryption_Gate.decryptionSO = closeGate;

                StartCoroutine(door.DoorControl(PlayerCore.Instance.transform, door.doorBreakTime, false));
				boxCollider.enabled = false;
				globalVolume .enabled = false;
				directionLight .enabled = false;
				this.enabled = false;
			}
        }

    }
}

