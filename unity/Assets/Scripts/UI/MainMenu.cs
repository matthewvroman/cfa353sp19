using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Bradley.AlienArk
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject menu;
        public Button play, sound, exit;
        private GameObject overlay, exitConfirm;
        Text soundText;

        private void Start()
        {
            AudioManager.Instance.PlaySound("Background Music");
            SetSoundButton();
            SetOverlay();
            exitConfirm = Resources.Load<GameObject>("UI/ExitConfirm");
            play.onClick.AddListener(OnPlayClicked);
            sound.onClick.AddListener(OnSoundButtonClicked);
            exit.onClick.AddListener(OnExitClicked);
        }

        private void SetOverlay()
		{
			overlay = Instantiate(Resources.Load<GameObject>("UI/Overlay"), transform);
			overlay.transform.SetSiblingIndex(2);
			overlay.SetActive(false);
		}

		private void SetSoundButton()
		{
			if (soundText == null) soundText = sound.GetComponentInChildren<Text>();
			string text = "Sound ";
			if (AudioListener.volume == 1) text += "On";
			else text += "Off";
			soundText.text = text;
		}

        private void ActivateOverlay(bool value)
        {
            menu.SetActive(!value);
            overlay.SetActive(value);
        }

        public void OnPlayClicked()
        {
            SceneManager.LoadScene("Level_1");
        }

                public void OnSoundButtonClicked()
        {
            if (AudioListener.volume == 1) AudioListener.volume = 0;
            else AudioListener.volume = 1;
            SetSoundButton();
        }
        
        public void OnExitClicked()
        {
            ActivateOverlay(true);
            Instantiate(exitConfirm, transform);
        }

        public void EnterMenu()
        {
            menu.SetActive(false);
        }

        public void ReturnToMenu()
        {
            if (overlay.activeSelf) ActivateOverlay(false);
            else menu.SetActive(true);
        }
    }
}
