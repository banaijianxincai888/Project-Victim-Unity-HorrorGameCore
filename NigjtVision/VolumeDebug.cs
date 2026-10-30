using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VolumeDebug : MonoBehaviour
{
    private void Update()
    {
        Camera cam = Camera.main;

        if (cam == null)
            return;

        // 获取当前 Volume Stack
        VolumeStack stack = VolumeManager.instance.stack;

        ColorAdjustments ca = stack.GetComponent<ColorAdjustments>();

        if (ca == null)
        {
            Debug.LogError("当前 Volume Stack 没有 ColorAdjustments");
            return;
        }

        Debug.Log(
            $"Camera={cam.name} | " +
            $"ColorFilter={ca.colorFilter.value} | " +
            $"Override={ca.colorFilter.overrideState} | " +
            $"PostExposure={ca.postExposure.value} | " +
            $"Saturation={ca.saturation.value}"
        );
    }
}