using UnityEngine;

namespace ns
{   
	///summary
	///大门解密
	///summary 
	public class Decryption_Gate : DecryptionItem
	{
        public Door door;

        private void Start()
        {
            door = GetComponent<Door>();
        }
        public override void SolveDecryption()
        {
            base.SolveDecryption();
            door.isLock = false;
            GameOver gameOver = FindAnyObjectByType<GameOver>();
            gameOver.PlayGameOverTimeline();
        }

  	}
}

