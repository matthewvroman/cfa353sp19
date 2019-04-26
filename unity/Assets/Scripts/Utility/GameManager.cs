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

		public GameManager()
		{
			//Do Nothing
		}
	}
}
