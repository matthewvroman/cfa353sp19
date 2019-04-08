using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class BaitManager : MonoBehaviour 
	{
		Image[] images;
		int numBait;
		// Use this for initialization
		void Start () 
		{
			numBait = transform.childCount;
			images = new Image[numBait];
			for (int i = 0; i < numBait; i++)
			{
				images[i] = transform.GetChild(i).GetComponent<Image>();
				images[i].sprite = Resources.Load<Sprite>("Sprites/Bait/Berry" + GameManager.instance.GetLevelNum());
			}
		}

		public void DroppedBait()
		{
			if (numBait > 0)
			{
				Destroy(images[numBait - 1].gameObject);
				numBait--;
			}
		}
	}
}
