using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bradley.AlienArk
{
	public class GameManager
	{
		static GameManager m_instance;
		public static GameManager instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new GameManager();
				}
				return m_instance;
			}
		}

		Level[] levels = new Level[3];

		public GameManager()
		{
			for (int i = 0; i < levels.Length; i++)
			{
				levels[i] = new Level("Sample_Level_" + (i + 1));
			}
			levels[0].Unlocked();
		}

		public void CompletedLevel()
		{
			string levelName = SceneManager.GetActiveScene().name;
			int levelNum = int.Parse(levelName.Substring(levelName.Length - 1));
			levels[levelNum - 1].Completed();
			if (levelNum < levels.Length)
			{
				levels[levelNum].Unlocked();
			}
		}

		public Level GetLevelStatus(int levelIndex)
		{
			if (levelIndex < levels.Length && levelIndex >= 0)
			{
				return levels[levelIndex];
			}
			Debug.Log("WARNING: Requested level status for level that didn't exist");
			return null;
		}
	}
}
