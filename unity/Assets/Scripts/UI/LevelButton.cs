using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class LevelButton : MonoBehaviour
	{
		[SerializeField]
		Image lockIcon, checkmark;


		public void SetLevel(bool unlocked, bool completed)
		{
			GetComponent<Button>().enabled = unlocked;
			Color c = unlocked ? Color.white : Color.gray;
			lockIcon.gameObject.SetActive(!unlocked);
			GetComponent<Image>().color = c;
			checkmark.gameObject.SetActive(completed);
		}

	}
}
