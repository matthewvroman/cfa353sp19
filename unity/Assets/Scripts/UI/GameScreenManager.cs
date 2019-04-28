using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Bradley.AlienArk
{
	public class GameScreenManager : MonoBehaviour 
	{
		[SerializeField]
		GameObject pauseScreen;
		public Button pauseButton, soundButton, quitButton, resumeButton, helpButton;
		private GameObject overlay, quitConfirm, helpScreen, gameOverScreen, levelCompleteScreen, hud = null;
		private Text soundButtonText;



		void OnEnable()
		{
			PlayerController.PlayerDied += GameOver;
		}

		void OnDisable()
		{
			PlayerController.PlayerDied -= GameOver;
		}

		private void Start()
		{
			pauseButton.onClick.AddListener(OnPauseButtonPressed);
			resumeButton.onClick.AddListener(OnResumeButtonPressed);
			soundButton.onClick.AddListener(OnSoundButtonPressed);
			helpButton.onClick.AddListener(OnHelpButtonPressed);
			quitButton.onClick.AddListener(OnQuitPressed);
			hud = transform.GetChild(0).gameObject;
			quitConfirm = Resources.Load<GameObject>("UI/QuitConfirm");
			helpScreen = Resources.Load<GameObject>("UI/Help");
			gameOverScreen = Resources.Load<GameObject>("UI/GameOver");
			levelCompleteScreen = Resources.Load<GameObject>("UI/LevelComplete");
			SetOverlay();
		}

		private void SetOverlay()
		{
			overlay = Instantiate(Resources.Load<GameObject>("UI/Overlay"), transform);
			overlay.transform.SetSiblingIndex(1);
			overlay.SetActive(false);
		}

		private void SetSoundButton()
		{
			if (soundButtonText == null) soundButtonText = soundButton.GetComponentInChildren<Text>();
			string text = "Sound ";
			if (AudioListener.volume == 1) text += "On";
			else text += "Off";
			soundButtonText.text = text;
		}

		private void ActivateOverlay(bool value)
		{
			ActivateHud(!value);
			overlay.SetActive(value);
		}

		
		private void ActivateHud(bool value)
		{
			PauseGame(value ? 1 : 0);
			hud.SetActive(value);
		}

		private void PauseGame(int value = 0)
		{
			Time.timeScale = value;
		}

//======================================================================================================================================================================

		private void OnPauseButtonPressed()
		{
			ActivateOverlay(true);
			SetSoundButton();
			pauseScreen.SetActive(true);
		}

		private void OnSoundButtonPressed()
		{
			if (AudioListener.volume == 1) AudioListener.volume = 0;
			else AudioListener.volume = 1;
			SetSoundButton();
		}

		private void OnHelpButtonPressed()
		{
			pauseScreen.SetActive(false);
			Instantiate(helpScreen, transform);
		}

		private void OnResumeButtonPressed()
		{
			pauseScreen.SetActive(false);
			ActivateOverlay(false);
		}

		private void OnQuitPressed()
		{
			pauseScreen.SetActive(false);
			Instantiate(quitConfirm, transform);
		}

//======================================================================================================================================================================

		public void ReturnToPauseMenu()
		{
			pauseScreen.SetActive(true);
		}

		public void LeaveLevel()
		{
			PauseGame(1);
			SceneManager.LoadScene("Menu");
		}

		public void LevelComplete()
		{
			PauseGame();
			ActivateOverlay(true);
			Instantiate(levelCompleteScreen, transform);
		}

		private void GameOver()
		{
			StartCoroutine(PlayDeathAnimation(2f));
		}

		public void ResetLevel()
		{
			PauseGame(1);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		private IEnumerator PlayDeathAnimation(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			PauseGame();
			ActivateOverlay(true);
			Instantiate(gameOverScreen, transform);
		}
	}
}
