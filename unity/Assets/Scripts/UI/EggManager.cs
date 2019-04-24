using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class EggManager : MonoBehaviour 
	{
		Image[] images;
		int numEggs;
		// Use this for initialization
		void Start () 
		{
			numEggs = transform.childCount;
			images = new Image[numEggs];
			for (int i = 0; i < numEggs; i++)
			{
				images[i] = transform.GetChild(i).GetComponent<Image>();
			}
		}

		public void CollectedEgg()
		{
			//Do Something
		}
	}
}
