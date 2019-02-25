using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bradley.AlienArk
{
	public class ScreenManager : MonoBehaviour 
	{
		[SerializeField]
		GameObject pauseScreen, pauseButton, gameOverScreen, levelCompleteScreen, eggCollectionIcon, miniMap;


		void OnEnable()
		{
			Egg.EggCollected += OnEggCollected;
			PlayerController.PlayerDied += GameOver;
			Goal.LevelComplete += LevelComplete;
		}

		void OnDisable()
		{
			Egg.EggCollected -= OnEggCollected;
			PlayerController.PlayerDied -= GameOver;
			Goal.LevelComplete -= LevelComplete;
		}

		public void OnPauseButtonPressed()
		{
			ActivateHud(false);
			pauseScreen.SetActive(true);
		}


		public void RestartLevel()
		{
			PauseGame(1);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void ReturnToLevelSelect()
		{
			PauseGame(1);
			Debug.Log("Return to level select");
		}

		public void ReturnToGame(GameObject menu)
		{
			ActivateHud(true);
			menu.SetActive(false);
		}

		private void LevelComplete()
		{
			ActivateHud(false);
			levelCompleteScreen.SetActive(true);
		}

		private void OnEggCollected()
		{
			eggCollectionIcon.SetActive(true);
		}

		private void GameOver()
		{
			ActivateHud(false);
			gameOverScreen.SetActive(true);
		}

		private void ActivateHud(bool value)
		{
			PauseGame(value ? 1 : 0);
			pauseButton.SetActive(value);
			miniMap.SetActive(value);
			eggCollectionIcon.SetActive(value);
		}

		private void PauseGame(int value = 0)
		{
			Time.timeScale = value;
		}
	}
}
