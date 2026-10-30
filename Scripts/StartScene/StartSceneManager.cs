using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace ns
{   
	///summary
	///开始场景管理器
	///summary 
	public class StartSceneManager : MonoBehaviour
	{

        public string loadSceneName;
		public StartSoundManager soundManager;


     
        //singal信号

		public void StartGame()
		{
			soundManager.Cancel();
            StartCoroutine(LoadSceneAsync(loadSceneName));

		}

        IEnumerator LoadSceneAsync(string sceneName)
        {

            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
            asyncOp.allowSceneActivation = false;  // 先不激活，等进度条走完

            while (asyncOp.progress < 0.9f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            asyncOp.allowSceneActivation = true;
        }

      

    }
    

    
}

