using System.Collections;
using System.IO;
using System.Runtime.InteropServices; // 【关键】用于获取内存指针 (GCHandle)
using UnityEngine;
using UnityEngine.Networking; // 【关键】用于在 WebGL 下通过 Web 请求读取文件

namespace ns
{
    /// <summary>
    /// 开始场景音频管理器
    /// 负责初始化音量设置、加载 SoundBank 并播放初始的 BGM 和 呼吸声
    /// </summary>
    public class StartSoundManager : MonoBehaviour
    {
        public AK.Wwise.RTPC GlobalVolume;
        public AK.Wwise.Event BGM;
        public AK.Wwise.Event breath;

        private void Start()
        {
            // 提前设置全局音量 RTPC（在 Bank 加载前设置，加载后生效）
            GlobalVolume.SetGlobalValue(100);
            // 启动加载与播放的协程
            StartCoroutine(WaitForWWiseAndLoadBank());
        }

        private IEnumerator WaitForWWiseAndLoadBank()
        {
            // 1. 等待 Wwise 底层音频引擎初始化完毕
            while (!AkUnitySoundEngine.IsInitialized())
            {
                yield return null;
            }

            // 2. 加载 SoundBank
            // 核心差异：WebGL 处于浏览器沙箱环境，无法直接读取本地文件，必须通过内存加载。
            // 而编辑器和桌面端依然可以使用传统的 LoadBank 方法。
#if UNITY_WEBGL && !UNITY_EDITOR
            // --- WebGL 平台专用加载逻辑 ---
            // 铁律：必须先加载 Init.bnk 初始化全局元数据，再加载 Main.bnk，否则会报 Event ID not found 错误
            Debug.Log("[Wwise] WebGL 环境，开始通过内存流加载 Bank...");
            yield return StartCoroutine(LoadBankFromBytes("Init.bnk"));
            yield return StartCoroutine(LoadBankFromBytes("Main.bnk"));
#else
            // --- 桌面端与编辑器加载逻辑 ---
            Debug.Log("[Wwise] 桌面端环境，使用常规 API 加载 Bank...");
            AkUnitySoundEngine.LoadBank("Init", out _);
            AkUnitySoundEngine.LoadBank("Main", out _);

            // 留出一帧的缓冲时间，确保加载回调有机会执行
            yield return null;
#endif

            // 3. Bank 确认加载完毕后，再设置 RTPC 并播放事件
            GlobalVolume.SetGlobalValue(100);
            BGM.Post(gameObject);
            breath.Post(gameObject);
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>
        /// 仅限 WebGL 平台使用：通过 UnityWebRequest 读取 bnk 文件的字节流，并直接加载到内存中
        /// </summary>
        private IEnumerator LoadBankFromBytes(string bankName)
        {
            // 拼接 StreamingAssets 路径，指向 Web 平台下的音频文件夹
            string path = Path.Combine(Application.streamingAssetsPath, "Audio", "GeneratedSoundBanks", "Web", bankName);

            // WebGL 下必须使用 UnityWebRequest 来读取 StreamingAssets 里的文件
            using (UnityWebRequest www = UnityWebRequest.Get(path))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[Wwise] 无法读取 Bank 文件: {path}, 错误: {www.error}");
                    yield break;
                }

                // 获取文件字节流
                byte[] bankData = www.downloadHandler.data;
                if (bankData == null || bankData.Length == 0)
                {
                    Debug.LogError($"[Wwise] Bank 文件为空: {bankName}");
                    yield break;
                }

                // 【关键步骤】将 C# 的 byte[] 数组固定在内存中，防止被垃圾回收器 (GC) 移动位置，从而获取安全的内存指针
                GCHandle pinnedHandle = GCHandle.Alloc(bankData, GCHandleType.Pinned);
                // 获取首地址指针
                System.IntPtr bankPtr = pinnedHandle.AddrOfPinnedObject(); 

                try
                {
                    // 使用最新的 API 从内存指针加载 Bank 数据
                    // 传入：内存指针、数据长度、输出 BankID
                    AKRESULT result = AkUnitySoundEngine.LoadBankMemoryCopy(
                        bankPtr, 
                        (uint)bankData.Length, 
                        out uint bankID
                    );

                    if (result != AKRESULT.AK_Success)
                    {
                        Debug.LogError($"[Wwise] 内存加载 Bank 失败: {bankName}, Result: {result}");
                    }
                    else
                    {
                        Debug.Log($"[Wwise] 成功从内存加载 Bank: {bankName}, ID: {bankID}");
                    }
                }
                finally
                {
                    // 无论加载成功还是失败，都必须释放句柄。
                    // 因为 Wwise 的 LoadBankMemoryCopy 内部已经拷贝了数据，释放指针不会影响已加载的音频
                    if (pinnedHandle.IsAllocated)
                    {
                        pinnedHandle.Free();
                    }
                }
            }
        }
#endif

        /// <summary>
        /// 停止播放背景音和呼吸声（通常是退出场景或进入游戏时调用）
        /// </summary>
        public void Cancel()
        {
            BGM.Stop(gameObject, 1500, AkCurveInterpolation.AkCurveInterpolation_Log1);
            breath.Stop(gameObject, 1500, AkCurveInterpolation.AkCurveInterpolation_Log1);
        }
    }
}