using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ns
{
    ///summary
    ///UI管理器单例
    ///summary 
    public class UIManager : MonoSingleton<UIManager>
    {
        public DecryptionManager decryptionManager;
        public BagManager bagManager;

        public QuestTipManager questTipManager;

        public DialogueManager dialogueManager;

        public StaminaManager staminaManager;

        public NightVisionManager nightVisionManager;

        public MouseUIManager mouseUIManager;

        public CoverUIManager coverUIManager;

        public SettingsUIManager settingsUIManager;

        public BloodManager bloodManager;

        public ExitTrapManager exitTrapManager;

        public DeathUIManager deathUIManager;
        public void SmoothShowAndFadeWithCor(CanvasGroup canvasGroup,ref Coroutine coroutine, float showTime = 0.3f, float lastTime = 1f, float fadeTime = 0.3f)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(SmoothShowAndFade(canvasGroup, showTime, lastTime, fadeTime));
        }



        public  IEnumerator SmoothShowAndFade(CanvasGroup canvasGroup, float showTime ,float lastTime, float fadeTime )
        {
            float currentTime = 0f;
            while (currentTime < showTime)
            {
                currentTime += Time.unscaledDeltaTime;
                float t = currentTime / showTime;
               t= Mathf.Clamp01(t);
                canvasGroup.alpha = Mathf.Lerp(0, 1, t);
                yield return null;

            }
            currentTime = 0f;
            canvasGroup.alpha = 1;

            yield return new WaitForSeconds(lastTime);

            while(currentTime < fadeTime)
            {
                currentTime += Time.unscaledDeltaTime;
                float t = currentTime / fadeTime;
                t = Mathf.Clamp01(t);
                canvasGroup.alpha = Mathf.Lerp(1,0, t);
                yield return null;
            }
            canvasGroup.alpha = 0;
        }

        public void AlphaSmoothChangeWithCor(CanvasGroup canvasGroup, ref Coroutine coroutine, float start, float end, float time, Action afterMethod = null)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine); // 停止上一个还没播完的动画
            }
            coroutine = StartCoroutine(AlphaSmoothChange(canvasGroup, start, end, time, afterMethod));
        }

        public IEnumerator AlphaSmoothChange(CanvasGroup canvasGroup,float start,float end,float time,Action afterMethod = null)
        {
            float currentTime = 0f;
            while (currentTime < time)
            {
                currentTime += Time.unscaledDeltaTime;
                float t = currentTime / time;
                t = Mathf.Clamp01(t);
                canvasGroup.alpha = Mathf.Lerp(start,end, t);
                yield return null;

            }
            canvasGroup.alpha = end;
            afterMethod?.Invoke();
        }

        public void SharpShowAndFadeWithCor(CanvasGroup canvasGroup,ref Coroutine coroutine, float lastTime = 1f, float fadeTime = 0.3f)
        {
            if(coroutine !=null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(SharpShowAndFade(canvasGroup, lastTime, fadeTime));
        }

       public  IEnumerator SharpShowAndFade( CanvasGroup canvasGroup,  float lastTime , float fadeTime )
        {
            canvasGroup.alpha = 1;
            float currentTime = 0f;
            yield return new WaitForSeconds(lastTime);

            while (currentTime < fadeTime)
            {
                currentTime += Time.unscaledDeltaTime;
                float t = currentTime / fadeTime;
                t = Mathf.Clamp01(t);
                canvasGroup.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }
            canvasGroup.alpha = 0;
        }


    }

    }



 

