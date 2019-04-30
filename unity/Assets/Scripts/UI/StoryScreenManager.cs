using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Bradley.AlienArk
{
	public class StoryScreenManager : MonoBehaviour 
	{
		public Sprite[] storyBackgrounds;
		public string[] storyText;
		[Range(5f, 100f)]
		public float characterSpeed = 15;
		public float fadeTime = 1;
		public float waitTime = 5;
		private float waitTimer;
		public Image background, textPanel;
		public Button skip;
		private Text text;
		private Image overlay;
		public enum StoryState
		{
			writing,
			waiting,
			fading
		}
		private StoryState state = StoryState.fading;
		private int stroyIndex = 0, characterIndex = 0;
		private float characterTimer;

		// Use this for initialization
		void Start () 
		{
			SetOverlay();
			background.sprite = storyBackgrounds[0];
			text = textPanel.GetComponentInChildren<Text>();
			skip.onClick.AddListener(EnterGame);
			characterTimer = characterSpeed;
			waitTimer = waitTime;
			overlay.CrossFadeAlpha(0, fadeTime, true);
			StartCoroutine("StartStory");
		}

		private void SetOverlay()
		{
			overlay = Instantiate(Resources.Load<GameObject>("UI/Overlay"), transform).GetComponent<Image>();
			overlay.transform.SetSiblingIndex(transform.childCount - 2);
			overlay.color = Color.black;
		}
		
		// Update is called once per frame
		void Update () 
		{
			switch (state)
			{
				case StoryState.waiting:
				{
					waitTimer -= Time.deltaTime;
					if (waitTimer <= 0 || (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
					{
						waitTimer = waitTime;
						StartCoroutine("Fade");
					}
					break;
				}
				case StoryState.writing:
				{
					if (characterIndex >= storyText[stroyIndex].Length || (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
					{
						text.text = storyText[stroyIndex];
						state = StoryState.waiting;
						return;
					}

					characterTimer += characterSpeed * Time.deltaTime;
					if (characterTimer >= 1)
					{
						text.text += storyText[stroyIndex].Substring(characterIndex, 1);
						characterTimer = 0;
						characterIndex++;
					}
					break;
				}
			}
		}

		private IEnumerator Fade()
		{
			state = StoryState.fading;
			overlay.CrossFadeAlpha(1, fadeTime, true);
			yield return new WaitForSeconds(fadeTime);
			UpdateStory();
			overlay.CrossFadeAlpha(0, fadeTime, true);
			yield return new WaitForSeconds(fadeTime);
			BeginWriting();
		}

		private IEnumerator StartStory()
		{
			Debug.Log("Waiting for fade");
			yield return new WaitForSeconds(fadeTime);
			BeginWriting();
			Debug.Log("Starting");
		}

		private void BeginWriting()
		{
			state = StoryState.writing;
			characterIndex = 0;
			Debug.Log("Begining writing");
		}

		private void UpdateStory()
		{
			Debug.Log("Updating Story");
			stroyIndex++;
			if (stroyIndex < storyBackgrounds.Length)
			{
				background.sprite = storyBackgrounds[stroyIndex];
				text.text = "";
			}
			else EnterGame();
		}

		private void EnterGame()
		{
			SceneManager.LoadScene("Level_1");
		}

	}
}
