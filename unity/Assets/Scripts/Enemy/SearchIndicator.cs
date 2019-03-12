using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class SearchIndicator : StateIndicator
	{
		public Image m_SearchRing;
		float m_TotalSearchTime;
		float m_searchTimer;
		bool instantiated = false;

		public void StartSearch(Enemy target, float searchTime)
		{
			base.SetIndicator(target);
			m_TotalSearchTime = searchTime;
			m_searchTimer = m_TotalSearchTime;
			instantiated = true;
		}

		private void LateUpdate() 
		{
			if (instantiated)
			{
				UpdatePosition();
				UpdatePointer();
				UpdateSearchRing();
			}
		}

		private void UpdateSearchRing()
		{
			m_searchTimer -= Time.deltaTime;
			m_SearchRing.fillAmount = (m_searchTimer/m_TotalSearchTime);
			if (m_searchTimer <= 0)
			{
				m_target.SearchCompleted();
				Destroy(gameObject);
			}
		}
	}
}
