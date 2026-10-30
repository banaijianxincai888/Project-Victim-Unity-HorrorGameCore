using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace ns
{   
	///summary
	///玩家镜头后处理
	///summary 
	public class PlayerPostProcess : MonoBehaviour
	{
		[Header("Hurt")]
        public Volume hurtVolume;

		public CinemachineImpulseSource impulseSource;

		public float changeFloat = 0.7f;
		public float changeTime = 0.2f;

		public float recoverTime = 1f;

		private Coroutine coroutine;



        

        public void SetHurtVolume(bool isUseEarRing)
		{
			if(coroutine != null)
			{
				StopCoroutine(coroutine);
			}
            coroutine = StartCoroutine(SetHurtPostProcess(isUseEarRing));

        }

        private IEnumerator SetHurtPostProcess(bool isUseEarRing)
		{

			impulseSource.GenerateImpulse();
			float time = 0;
			float start = hurtVolume.weight;
            float progress = 0;
			if(isUseEarRing) PlayerCore.Instance.playerSoundEmission.earRing.Post(gameObject);
			while(time < changeTime)
			{
				time += Time.deltaTime;
				progress = Mathf.Clamp01(time / changeTime);
				hurtVolume.weight = Mathf.Lerp(start, changeFloat, progress);
				yield return null;
			}
			hurtVolume.weight = changeFloat;
			time = 0;
			start = hurtVolume.weight;
            while (time < recoverTime)
            {
                time += Time.deltaTime;
                progress = Mathf.Clamp01(time / recoverTime);
                hurtVolume.weight = Mathf.Lerp(start, 0, progress);
				yield return null;
            }
            hurtVolume.weight = 0;
            PlayerCore.Instance.playerSoundEmission.earRing.Stop(gameObject,1500,AkCurveInterpolation.AkCurveInterpolation_Log1);

        }
    }
}

