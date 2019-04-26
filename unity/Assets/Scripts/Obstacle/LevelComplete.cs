using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class LevelComplete : MonoBehaviour 
	{
		GameScreenManager screenManager;
		public Text header;
		public Image[] collectedEggs;
		public Button nextButton;

		private void Start()
		{
			screenManager = transform.parent.GetComponent<GameScreenManager>();
			header.text = "Good Job!";
			foreach(Image egg in collectedEggs)
			{
				egg.color = Color.gray;
			}
			nextButton.onClick.AddListener(OnNextButtonClicked);
		}

		private void OnNextButtonClicked()
		{
			screenManager.LeaveLevel();
		}

	}
}
