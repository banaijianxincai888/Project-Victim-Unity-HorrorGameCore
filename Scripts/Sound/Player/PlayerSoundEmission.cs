using ES3Types;
using UnityEngine;

namespace ns
{   
	///summary
	///玩家声音发射器
	///summary 
	public class PlayerSoundEmission : MonoBehaviour
	{

		public AK.Wwise.Event currentEvent;

		public AK.Wwise.Event hide;

		public AK.Wwise.Event isFound;

		public AK.Wwise.Event struggle;

		public AK.Wwise.Event isPursuit;

		public AK.Wwise.Event commonBreath;

		public AK.Wwise.Event runBreath;

		public AK.Wwise.Event walking;

		public AK.Wwise.Event running;

		public AK.Wwise.Event ahh;

		public AK.Wwise.Event earRing;

		public AK.Wwise.Event fallDown;

		public AK.Wwise.Event fallUp;

		public AK.Wwise.Event dead;


        public void SoundEmission(AK.Wwise.Event akEvent)
		{
			if (currentEvent != null) 
			{
				currentEvent.Stop(gameObject, 1500, AkCurveInterpolation.AkCurveInterpolation_Log1);
			}

			currentEvent = akEvent;
			currentEvent.Post(gameObject);
		}

		public void StopSound()
		{
			currentEvent.Stop(gameObject, 1500, AkCurveInterpolation.AkCurveInterpolation_Log1);
		}


  	}
}

