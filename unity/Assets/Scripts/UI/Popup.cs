using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class Popup : MonoBehaviour 
	{
		public Text header, body;
		public Button yes, no;

		protected virtual void init()
		{
			yes.onClick.AddListener(OnYesClicked);
			no.onClick.AddListener(OnNoClicked);
			transform.SetAsLastSibling();
		}

		protected virtual void OnYesClicked()
		{
			//Override
		}

		protected virtual void OnNoClicked()
		{
			//Override
		}
	}
}
