using UnityEngine;
using UnityEngine.Playables;

namespace ns
{   
	///summary
	///游戏结束
	///summary 
	public class GameOver : MonoBehaviour
	{
		public PlayableDirector director;

		

		public void PlayGameOverTimeline()
		{
			director.Play();
		}

		public void GameOverStart()
		{
			BGMManager.Instance.ChangeBGM(BGMManager.Instance.commonBGM);
			PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.isPursuit);

		}

		public void PlayerStruggle()
		{
            PlayerCore.Instance.playerSoundEmission.SoundEmission(PlayerCore.Instance.playerSoundEmission.struggle);
        }
        
		public void PlayerDead()
		{
			BGMManager.Instance.ChangeBGM(BGMManager.Instance.deadBGM); ;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

        }

       

    }
}

