using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class Level
	{
		string m_name;
		bool m_unlocked = false;
		bool m_completed = false;

		public Level(string name)
		{
			m_name = name;
		}

		public string GetName()
		{
			return m_name;
		}

		public void Unlocked()
		{
			m_unlocked = true;
		}

		public bool IsUnlocked()
		{
			return m_unlocked;
		}

		public void Completed()
		{
			m_completed = true;
		}

		public bool IsCompleted()
		{
			return m_completed;
		}

	}
}
