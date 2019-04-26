using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class GameOverPopup : Popup 
	{
		GameScreenManager screenManager;

		void Start () 
		{
			init();
			header.text = "Game Over!";
			body.text = "You have meet with an unfortunate end while on your noble mission. Will you step up again and try to save these creatures from their horrible fate?";
			screenManager = transform.parent.GetComponent<GameScreenManager>();
		}

		protected override void OnYesClicked()
		{
			screenManager.ResetLevel();
		}

		protected override void OnNoClicked()
		{
			screenManager.LeaveLevel();
		}
	}
}
