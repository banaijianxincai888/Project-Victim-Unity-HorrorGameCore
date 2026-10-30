using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ns
{   
    ///summary
    ///夜视仪控制器
    ///summary 
    public class NightVisionController : MonoBehaviour
	{
		
		public float interval = 0.5f;

        public float smoothTime = 0.3f;
		private float timer;

		public  UnityEngine.Rendering.Volume volume;

        public GameObject nightVisionSpotLight;

        public GameObject bodyLight;

        public Transform origionalParent;

        public Transform hideParent;


		private bool isOpenNightVision;


        private void OnEnable()
        {
            EventCenter.NightVisionEvent.OnNightVisionRequest += SetNightVision;
            EventCenter.NightVisionEvent.OnRecoverBatteryRequest += RecoverBattery;
        }


       

        private void OnDisable()
        {
            EventCenter.NightVisionEvent.OnNightVisionRequest -= SetNightVision;
            EventCenter.NightVisionEvent.OnRecoverBatteryRequest -= RecoverBattery;

        }



        private void SetNightVision()
        {
            if (Time.time - timer < interval) return;

            //如果没有电量，别开启
            if (UIManager.Instance.nightVisionManager.battery.value <= 0f) return;

            isOpenNightVision = !isOpenNightVision;

            if (isOpenNightVision)
            {
                SoundManager.Instance.nightVisionOpen.Post(gameObject);
                SoundManager.Instance.nightVisionVideo.Post(gameObject);
                // 防止之前有协程没跑完，直接停止并重置
                StopAllCoroutines();
                StartCoroutine(SmoothTurn(smoothTime, 1));
                UIManager.Instance.nightVisionManager.canvasGroup.alpha = 1f;
                UIManager.Instance.nightVisionManager.UseBattery(volume,TurnOffWhenPowerOff);
                bodyLight.SetActive(true);
                nightVisionSpotLight.SetActive(true);
            }
            else
            {
                SoundManager.Instance.nightVisionVideo.Stop(gameObject, 1500, AkCurveInterpolation.AkCurveInterpolation_Log3);
                SoundManager.Instance.nightVisionClose.Post(gameObject);
                StopAllCoroutines();
                StartCoroutine(SmoothTurn(smoothTime, 0));
                UIManager.Instance.nightVisionManager.canvasGroup.alpha = 0f;
                UIManager.Instance.nightVisionManager.StopCostBattery();
                bodyLight.SetActive(false);

                nightVisionSpotLight.SetActive(false);
            }

            timer = Time.time;
        }

       public void TurnOffWhenPowerOff()
        {
            SoundManager.Instance.nightVisionVideo.Stop(gameObject, 1500, AkCurveInterpolation.AkCurveInterpolation_Log3);
            SoundManager.Instance.nightVisionClose.Post(gameObject);
            isOpenNightVision = false;
            StopAllCoroutines();
            volume.weight = 0f;
            nightVisionSpotLight.SetActive(false);
            bodyLight.SetActive(false);
           
        }



        public void RecoverBattery()
        {
            UIManager.Instance.nightVisionManager.RecoverBattery();
        }
       
        public void CommonLight()
        {
            bodyLight.transform.SetParent(origionalParent);
            nightVisionSpotLight.transform.SetParent(origionalParent);
            bodyLight.transform.localPosition = Vector3.zero;
            bodyLight.transform.localRotation = Quaternion.identity;
            nightVisionSpotLight.transform.localPosition = Vector3.zero;
            nightVisionSpotLight.transform.localRotation = Quaternion.identity;
          



        }

        public void HideLight()
        {
            bodyLight.transform.SetParent(hideParent);
            nightVisionSpotLight.transform.SetParent(hideParent);
            bodyLight.transform.localPosition = Vector3.zero;
            bodyLight.transform.localRotation = Quaternion.identity;
            nightVisionSpotLight.transform.localPosition = Vector3.zero;
            nightVisionSpotLight.transform.localRotation = Quaternion.identity;
            




        }

        // 稍微优化一下协程，防止帧率波动
        IEnumerator SmoothTurn(float time, float target)
        {
            Debug.Log(
    $"NightVision Volume: {volume.name}, Weight: {volume.weight}, Camera.main: {Camera.main?.name}"
);
            float start = volume.weight;
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / time);
                volume.weight = Mathf.Lerp(start, target, t);
                yield return null;
            }
            volume.weight = target;
        }





    }
}

