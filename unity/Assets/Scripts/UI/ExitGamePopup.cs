using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ExitGamePopup : Popup
	{
		MainMenu MainMenu;

		// Use this for initialization
		void Start () 
		{
			init();
			MainMenu = GetComponentInParent<MainMenu>();
			header.text = "Exit Game?";
			body.text = "Are you sure you want to leave the game?";
		}

		protected override void OnYesClicked()
		{
			Application.Quit();
		}

		protected override void OnNoClicked()
		{
			MainMenu.ReturnToMenu();
			Destroy(gameObject);
		}
	}
}
