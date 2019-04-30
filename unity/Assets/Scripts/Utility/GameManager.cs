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

		bool[] eggsCollected = new bool[3];

		public GameManager()
		{
			ResetGame();
		}

		public void ResetGame()
		{
			for(int i = 0; i < eggsCollected.Length; i++)
			{
				eggsCollected[i] = false;
			}
		}

		public void TransferEgg(int index)
		{
			eggsCollected[index] = true;
		}

		public bool IsEggCollected(int index)
		{
			if (index < eggsCollected.Length)
			{
				return eggsCollected[index];
			}
			return false;
		}

		public bool[] GetEggsCollected()
		{
			return eggsCollected;
		}
	}
}
