using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class HelpPopup : MonoBehaviour 
	{
		GameScreenManager screenManager;
		public Button back;
		// Use this for initialization
		void Start () {
			screenManager = transform.GetComponentInParent<GameScreenManager>();
			transform.SetAsLastSibling();
			back.onClick.AddListener(OnBackButtonClicked);
		}

		private void OnBackButtonClicked()
		{
			screenManager.ReturnToPauseMenu();
			Destroy(gameObject);
		}
	}
}
