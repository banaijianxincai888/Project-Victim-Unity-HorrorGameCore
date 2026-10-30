using UnityEngine;

public class BreathingGlow : MonoBehaviour
{
    [Header("发光基础设置")]
    [SerializeField] private Color glowColor = Color.cyan; // 发光基础色
    [SerializeField] private float minIntensity = 0.2f;    // 呼吸暗部最低亮度
    [SerializeField] private float maxIntensity = 4.0f;    // 呼吸亮部最高亮度

    [Header("呼吸节奏")]
    [SerializeField, Tooltip("呼吸周期（秒）：一次吸气到呼气的总时长")]
    private float breathCycleDuration = 3.0f;

    private Renderer targetRenderer;
    private MaterialPropertyBlock propBlock;
    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        targetRenderer = GetComponentInChildren<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        // 避免除以 0
        if (breathCycleDuration <= 0f) return;

        // 1. 利用正弦波生成平滑的 0 到 1 归一化进度
        // Mathf.Sin 范围为 [-1, 1]，通过 * 0.5f + 0.5f 映射到 [0, 1]
        float t = Mathf.Sin((Time.time * Mathf.PI * 2f) / breathCycleDuration) * 0.5f + 0.5f;

        // 2. 将平滑进度应用为平滑缓动（SmoothStep 可让明暗顶点停留更柔和，更像呼吸）
        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        // 3. 在设定的最小与最大强度之间做插值
        float currentIntensity = Mathf.Lerp(minIntensity, maxIntensity, smoothT);

        // 4. HDRP 中发光颜色乘上线性能量值
        Color finalHdrColor = glowColor * Mathf.LinearToGammaSpace(currentIntensity);

        // 5. 应用到材质属性块
        targetRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor(EmissionColorId, finalHdrColor);
        targetRenderer.SetPropertyBlock(propBlock);
    }
}