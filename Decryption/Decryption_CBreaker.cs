using UnityEngine;

namespace ns
{   
	///summary
	///电闸解谜
	///summary 
	public class Decryption_CBreaker : DecryptionItem
	{
        
        public Light[] lights;

        public BoxCollider boxCollider;

        private void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }

        public override void SolveDecryption()
        {
            base.SolveDecryption();
            SoundManager.Instance.CBOpen.Post(gameObject);

            foreach (Light light in lights)
            {
                light.enabled = true;
            }

        }

        public void TurnOffLights()
        {
            SoundManager.Instance.CBClose.Post(gameObject);
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
        }

        public void TurnOnLights()
        {

            foreach (Light light in lights)
            {
                light.enabled = true;
            }
        }

    }
}

