using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bradley.AlienArk
{
	public class LevelSelectManager : MonoBehaviour 
	{
		[SerializeField]
		LevelButton[] levelButtons;

		void Start () 
		{
			for (int i = 0; i < levelButtons.Length; i++)
			{
				Level temp = GameManager.instance.GetLevelStatus(i);
				if (levelButtons[i] != null)
				{
					levelButtons[i].SetLevel(temp.IsUnlocked(), temp.IsCompleted());
				}
			}
		}

		public void OnLevelClicked(int levelIndex)
		{
			SceneManager.LoadScene(GameManager.instance.GetLevelStatus(levelIndex).GetName());
		}

		public void OnBackButtonClicked()
		{
			SceneManager.LoadScene("Menu");
		}
	}
}
