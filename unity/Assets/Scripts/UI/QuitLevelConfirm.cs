using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class QuitLevelConfirm : Popup 
	{
		[HideInInspector]
		public GameScreenManager screenManager;

		// Use this for initialization
		void Start () 
		{
			init();
			header.text = "Quit?";
			body.text = "Are you sure you want to leave the level? All progress will be lost.";
			screenManager = transform.parent.GetComponent<GameScreenManager>();
		}
		
		protected override void OnYesClicked()
		{
			screenManager.LeaveLevel();
		}

		protected override void OnNoClicked()
		{
			screenManager.ReturnToPauseMenu();
			Destroy(gameObject);
		}
	}
}
