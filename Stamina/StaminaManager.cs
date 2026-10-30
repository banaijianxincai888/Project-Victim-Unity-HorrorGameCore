using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ns
{
    /// <summary>
    /// 体力管理器：核心逻辑=跑步耗体力→停跑回体力→满体力后延迟淡出UI
    /// </summary> 
    public class StaminaManager :MonoBehaviour
    {
        public Image staminaImage; // 体力填充图
        public float staminaCostTime = 5f; // 体力消耗时长（满→空）
        public float staminaRecoverTime = 3f; // 体力恢复时长（空→满）
        public float coolTime = 1f; // 停跑后冷却多久开始恢复
        [Header("体力满后淡出延迟时间（秒）")]
        public float staminaFadeDelay = 2f; // 体力满后，延迟多久淡出UI
        public CanvasGroup canvasGroup; // 控制UI透明度的组件

        public bool isStart; // 是否正在消耗体力
        public bool isRecover; // 是否正在恢复体力

        // 协程管理：避免重复开启/内存泄漏
        private Coroutine coroutine; // 消耗体力协程
        private Coroutine coroutineRecover; // 恢复体力协程

        //[Header("跑步喘息音效设置")]

        ////public AK.Wwise.Event runBreathAudio;
        ////public AK.Wwise.RTPC runPara;



        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>(); // 获取UI透明度组件
        }

        /// <summary>
        /// 开始跑步：触发体力消耗
        /// </summary>
        public void StartCostStamina()
        {
            if (isStart) return; // 防止重复执行


            // 开启体力消耗协程
            coroutine = StartCoroutine(CostStamina());
        }

        /// <summary>
        /// 停止跑步：终止体力消耗
        /// </summary>
        public void StopCostStamina()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                isStart = false;
                coroutine = null;
            }
        }

        /// <summary>
        /// 体力消耗协程：逐帧减少体力，UI淡入
        /// </summary>
        private IEnumerator CostStamina()
        {
            if (isStart) yield break;
            isStart = true;
            float t = 1 / staminaCostTime; // 每帧消耗的比例（1/总时长）

            // UI淡入：如果当前透明度<1，0.3秒内淡到1
            if (canvasGroup.alpha < 0.9f)
            {

                StartCoroutine(UIManager.Instance.AlphaSmoothChange(canvasGroup,0,1,0.3f));
            }

            // 逐帧消耗体力，直到为空
            while (staminaImage.fillAmount > 0f)
            {
                staminaImage.fillAmount -= t * Time.deltaTime;
                staminaImage.fillAmount = Mathf.Max(staminaImage.fillAmount, 0f); // 防止小于0
                yield return null; // 等待下一帧
            }
            staminaImage.fillAmount = 0; // 强制置空

            isStart = false;
        }

       





        /// <summary>
        /// 开始恢复体力：停跑后调用
        /// </summary>
        public void StartRecoverStamina()
        {
            if (isRecover) return; // 防止重复执行


            // 开启体力恢复协程
            coroutineRecover = StartCoroutine(RecoverStamina());
        }

        /// <summary>
        /// 停止恢复体力：再次跑步时调用
        /// </summary>
        public void StopRecoverStamina()
        {
            // 停止恢复协程
            if (coroutineRecover != null)
            {
                StopCoroutine(coroutineRecover);
                isRecover = false;
                coroutineRecover = null;
            }


        }

        /// <summary>
        /// 体力恢复协程：冷却→恢复→延迟→淡出
        /// </summary>
        private IEnumerator RecoverStamina()
        {
            isRecover = true;

            // 第一步：等待冷却时间（停跑后不立刻恢复）
            yield return new WaitForSeconds(coolTime);

            // 第二步：逐帧恢复体力，直到满值
            float t = 1 / staminaRecoverTime; // 每帧恢复的比例（1/总时长）
            while (staminaImage.fillAmount < 1f)
            {
                staminaImage.fillAmount += t * Time.deltaTime;
                //BreathAudioPara();
                yield return null; // 等待下一帧
            }
            staminaImage.fillAmount = 1f; // 强制置满（避免浮点误差）





            if (canvasGroup.alpha > 0f)
            {
                // 第三步：体力满后，等待自定义延迟时间
                yield return new WaitForSeconds(staminaFadeDelay);
                // 第四步：延迟结束，UI淡出（1秒内从1→0）

                StartCoroutine(UIManager.Instance.AlphaSmoothChange(canvasGroup, 1f, 0,1f));
            }

            // 恢复完成
            isRecover = false;


            //StopBreathAudio(PlayerCore.Instance.gameObject);

        }

       
        //public void BreathAudioPara()
        //{
        //    float para = (1 - staminaImage.fillAmount) * 100;
        //    runPara.SetGlobalValue(para);
        //}

        //public void StopBreathAudio(GameObject gameObject)
        //{
        //    runBreathAudio.Stop(gameObject, 1);
        //}

       
    }
}