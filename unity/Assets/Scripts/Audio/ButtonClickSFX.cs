using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class ButtonClickSFX : MonoBehaviour 
	{

		// Use this for initialization
		void Start () 
		{
			GetComponent<Button>().onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			AudioManager.Instance.PlaySound("Click");
		}
	}
}
