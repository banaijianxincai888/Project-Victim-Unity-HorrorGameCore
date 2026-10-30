using UnityEngine;

namespace ns
{   
	///summary
	///战利品闪烁脚本
	///summary 
	public class MaterialEmission : MonoBehaviour
	{
        [Header("Emission Blink Settings")]
        public bool enableBlink = true;
        public float blinkSpeed = 5f;
        [ColorUsage(true, true)]
        public Color emissionColor = new Color(25f / 255f, 25f / 255f, 25f / 255f);
        public float minIntensity = 0f;
        public float maxIntensity = 5f;

        [Header("Player Detection")]
        public float detectionRadius = 5f;
        public Transform playerTransform;

        [Header("Renderer List (自动获取子物体)")]
        public Renderer[] renderers;           // 可手动拖拽，留空则自动获取所有子物体 Renderer

        private MaterialPropertyBlock[] mpbs;   // 每个 Renderer 对应一个属性块
        private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

        private bool isPlayerInRange = false;
        private bool wasPlayerInRange = false;

        private void Start()
        {

            // 获取所有需要控制的 Renderer
            if (renderers == null || renderers.Length == 0)
            {
                // 自动获取自身及所有子物体的 Renderer（包括隐藏的，可根据需求调整）
                renderers = GetComponentsInChildren<Renderer>(true);
            }

            if (renderers == null || renderers.Length == 0)
            {
                Debug.LogWarning("Loot: 未找到任何 Renderer，自发光闪烁功能禁用。", this);
                mpbs = null;
                return;
            }

            // 为每个 Renderer 创建独立的 MaterialPropertyBlock
            mpbs = new MaterialPropertyBlock[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    mpbs[i] = new MaterialPropertyBlock();
                    renderers[i].GetPropertyBlock(mpbs[i]);
                }
            }

            // 初始关闭发光
            SetEmission(Color.black);

            // 查找玩家
            if (playerTransform == null && PlayerCore.Instance != null)
            {
                playerTransform = PlayerCore.Instance.transform;
            }
            else if (playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTransform = player.transform;
            }

            isPlayerInRange = false;
            wasPlayerInRange = false;
        }

        private void Update()
        {
            // 缺少必要组件则跳过
            if (renderers == null || mpbs == null || playerTransform == null) return;

            // 检测玩家距离
            float sqrDistance = (playerTransform.position - transform.position).sqrMagnitude;
            isPlayerInRange = sqrDistance <= detectionRadius * detectionRadius;

            // 状态改变处理
            if (isPlayerInRange && !wasPlayerInRange)
            {
                // 进入范围（可加音效）
            }
            else if (!isPlayerInRange && wasPlayerInRange)
            {
                SetEmission(Color.black);
            }

            wasPlayerInRange = isPlayerInRange;

            // 更新发光
            if (isPlayerInRange && enableBlink)
            {
                float t = (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f;
                float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
                Color finalColor = emissionColor * intensity;
                SetEmission(finalColor);
            }
            else if (isPlayerInRange && !enableBlink)
            {
                SetEmission(emissionColor * maxIntensity);
            }
        }

        /// <summary>
        /// 对所有 Renderer 设置自发光颜色
        /// </summary>
        private void SetEmission(Color color)
        {
            if (renderers == null || mpbs == null) return;

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && mpbs[i] != null)
                {
                    mpbs[i].SetColor(EmissionColorID, color);
                    renderers[i].SetPropertyBlock(mpbs[i]);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawSphere(transform.position, detectionRadius);
        }

    }
}

