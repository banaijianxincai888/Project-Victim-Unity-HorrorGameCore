using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
	///summary
	///陷阱管理器
	///summary 
	public class TrapManager : MonoSingleton<TrapManager>
	{
		public float collectTime = 10f;

		public List<GameObject> trapCollectios;

		public GameObject trapPrefab;


        private void Start()
        {
			trapCollectios = new List<GameObject>();
        }

        public void SetTrap(Transform trapTF)
		{
			GameObject GO = null;
			if (trapCollectios.Count == 0)
			{
				 GO = Instantiate(trapPrefab,transform);
			}
			else
			{
				GO = trapCollectios.Find(t => !t.activeInHierarchy);
				GO.SetActive(true);
            }
            BearTrap bearTrap = GO.GetComponent<BearTrap>();
            bearTrap.ResetTrap(trapTF);
        }
	}
}

