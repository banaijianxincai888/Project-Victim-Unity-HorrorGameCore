using UnityEngine;

namespace ns
{   
	///summary
	///BGM管理器
	///summary 
	public class BGMManager : MonoSingleton<BGMManager>
	{
		public AK.Wwise.Event currentBGM;

		public AK.Wwise.Event pursuitBGM;

		public AK.Wwise.Event commonBGM;

		public AK.Wwise.Event deadBGM;


        private void Start()
        {
			currentBGM.Post(gameObject);
        }


        public void ChangeBGM( AK.Wwise.Event BGM )
		{
			if(currentBGM != null)
			{
				currentBGM.Stop(gameObject, 1500,AkCurveInterpolation.AkCurveInterpolation_Log1);
			}

			currentBGM = BGM;
            currentBGM.Post(gameObject);
           
		}

  	}
}

