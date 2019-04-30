using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Bradley.AlienArk
{
	public class GameCompleteScreenManager : MonoBehaviour 
	{
		public GameObject[] backgrounds;
		public Image[] babies;
		public float fadeTime = 2;
		public float waitTime = 3;
		private float waitTimer;
		public Button quit, retry;
		private Image overlay;
		enum GameCompleteState
		{
			Pause,
			Wait
		}
		private GameCompleteState state = GameCompleteState.Wait;
		private int backgroundIndex;

		// Use this for initialization
		void Start () 
		{
			SetOverlay();
			quit.onClick.AddListener(OnQuitClicked);
			retry.onClick.AddListener(OnRetryClicked);
			ActivateButtons(false);
			waitTimer = waitTime;
			for(int i = 0; i < babies.Length; i++)
			{
				if (!GameManager.instance.IsEggCollected(i)) babies[i].enabled = false;
			}
			overlay.CrossFadeAlpha(0, fadeTime, true);
			StartCoroutine("Begin");
		}

		private void ActivateButtons(bool value)
		{
			quit.enabled = value;
			retry.enabled = value;
		}

		private void SetOverlay()
		{
			overlay = Instantiate(Resources.Load<GameObject>("UI/Overlay"), transform).GetComponent<Image>();
			overlay.transform.SetAsLastSibling();
			overlay.color = Color.black;
		}
		
		// Update is called once per frame
		void Update () 
		{
			switch (state)
			{
				case GameCompleteState.Pause:
				{
					waitTimer -= Time.deltaTime;
					if (waitTimer <= 0 || (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
					{
						StartCoroutine("Fade");
						waitTimer = waitTime;
					}
					break;
				}
			}
		}

		private IEnumerator Begin()
		{
			yield return new WaitForSeconds(fadeTime);
			state = GameCompleteState.Pause;
		}

		private IEnumerator Fade()
		{
			state = GameCompleteState.Wait;
			overlay.CrossFadeAlpha(1, fadeTime, true);
			yield return new WaitForSeconds(fadeTime);
			UpdateScreen();
			overlay.CrossFadeAlpha(0, fadeTime, true);
			yield return new WaitForSeconds(fadeTime);
			if (backgroundIndex < backgrounds.Length - 1) state = GameCompleteState.Pause;
			else 
			{
				Destroy(overlay);
				ActivateButtons(true);
			}
		}

		private void UpdateScreen()
		{
			backgroundIndex++;
			backgrounds[backgroundIndex - 1].SetActive(false);
			backgrounds[backgroundIndex].SetActive(true);
		}

		private void OnRetryClicked()
		{
			SceneManager.LoadScene("Level_1");
		}

		private void OnQuitClicked()
		{
			SceneManager.LoadScene("Menu");
		}
	}
}
