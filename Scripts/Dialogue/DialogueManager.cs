using AK.Wwise;
using TMPro;
using UnityEngine;

namespace ns
{
    /// <summary>
    /// 对话管理器
    /// </summary> 
    public class DialogueManager : MonoBehaviour
    {
        public TMP_Text text;
        public CanvasGroup canvasGroup;
        public DialogueSO currentDialogueSO;

        public void PlayDialogue(DialogueSO dialogueSO, GameObject soundEmitter)
        {
            if (dialogueSO == null) return;

            // 1. 停上一句（这会异步触发上一句的 EndOfEvent 回调）
            if (currentDialogueSO != null && currentDialogueSO.AKEvent != null)
            {
                currentDialogueSO.AKEvent.Stop(soundEmitter);
            }

            // 2. 更新当前对话对象
            currentDialogueSO = dialogueSO;

            // 3. Post，并把 dialogueSO 作为 cookie 传进去
            CallbackFlags flags = new CallbackFlags { value = (uint)AkCallbackType.AK_EndOfEvent };
            currentDialogueSO.AKEvent.Post(soundEmitter, flags, EndDialogue, dialogueSO);

            text.text = currentDialogueSO.content;
        }

        public void EndDialogue(object in_cookie, AkCallbackType type, AkCallbackInfo info)
        {
            // 只处理"当前这句"结束的情况
            // 如果 cookie 对应的对话已经不是 currentDialogueSO，说明是被 Stop 打断的旧对话，忽略
            if (in_cookie is DialogueSO finished && finished != currentDialogueSO)
                return;

            text.text = string.Empty;
            currentDialogueSO = null;
        }

        public void StopDialogue()
        {
            if(currentDialogueSO == null) return;
            text.text = string.Empty;
            currentDialogueSO.AKEvent.Stop(gameObject);
            currentDialogueSO = null;
        }
    }
}