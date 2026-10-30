using ns;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoverUIManager : MonoBehaviour
{
    public SavePoint savePoint;
    public CanvasGroup tipCanvasGroup;
    public CanvasGroup canvasGroup;
    public float showTime;
    public float fadeTime;

    public bool isCovering;

    private Coroutine coverAnimCoroutine; // 新增

    private Coroutine tipCoroutine;

    private void Start()
    {
        StartCoroutine(WaitForCover());
    }

    //在start重要方法加载保存点时，需要等待几帧再加载，因为start的执行时机不同，ES3可能还没有加载
    private IEnumerator WaitForCover()
    {
        yield return new WaitForSeconds(1f);
        if (!ES3.FileExists(savePoint.checkpointFileName))
            CoverWithTip();
        else
        {
            CancelCover();

        }
    }


    public void CancelCover()
    {
        // 使用 UIManager 的防冲突方法
        UIManager.Instance.AlphaSmoothChangeWithCor(canvasGroup, ref coverAnimCoroutine, 1, 0, 2f, () =>
        {
           CancelBlockRaycasts();
        });

        UIManager.Instance.mouseUIManager.canvasGroup.alpha = 1; // 如果之前是0，这里应该是1
        tipCanvasGroup.alpha = 0f;
        savePoint.LoadCheckpoint();


    }

    public void CoverWithTip()
    {
        isCovering = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        UIManager.Instance.AlphaSmoothChangeWithCor(tipCanvasGroup, ref tipCoroutine, 0, 1, showTime);
        
    }

    public void CoverAndLoadScene(string sceneName)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        UIManager.Instance.AlphaSmoothChangeWithCor(canvasGroup, ref coverAnimCoroutine, 0, 1, 2f, () => { 
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
        });
    }

    public void CancelBlockRaycasts()
    {
       canvasGroup.blocksRaycasts = false;
       canvasGroup.interactable = false;
       isCovering = false;
    }
}