using System;
using UnityEngine;

namespace ns
{   
	///summary
	///解密门
	///summary 
	public class Decryption_Door : DecryptionItem
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
        }
  	}
}

