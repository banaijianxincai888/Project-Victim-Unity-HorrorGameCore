using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ns
{   
	///summary
	///
	///summary 
	public class TipManager : MonoBehaviour
	{
		public TMP_Text text;

		

		public void ShowTip(string tip)
		{
			text.text = tip;
			
		}

  	}
}

