using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class EggManager : MonoBehaviour 
	{
		public GameObject[] eggs;
		bool[] collectedEggs, transferedEggs;
		int[] eggIds;
		Image[] images;
		int numEggs;

		private void OnEnable()
		{
			Egg.EggCollected += CollectedEgg;
			Checkpoint.Transfer += TransferEggs;
			GameScreenManager.RestartLevel += ResetEggs;
		}

		private void OnDisable()
		{
			Egg.EggCollected -= CollectedEgg;
			Checkpoint.Transfer -= TransferEggs;
			GameScreenManager.RestartLevel -= ResetEggs;
		}

		// Use this for initialization
		void Start () 
		{
			numEggs = eggs.Length;
			images = new Image[numEggs];
			collectedEggs = new bool[numEggs];
			transferedEggs = new bool[numEggs];
			eggIds = new int[numEggs];
			for (int i = 0; i < numEggs; i++)
			{
				if (i < transform.childCount) images[i] = transform.GetChild(i).GetComponent<Image>();
				images[i].sprite = eggs[i].GetComponent<SpriteRenderer>().sprite;
				images[i].color = Color.gray;
				collectedEggs[i] = false;
				transferedEggs[i] = false;
				eggIds[i] = eggs[i].GetInstanceID();
			}
		}

		public void CollectedEgg(int id)
		{
			for (int i = 0; i < images.Length; i++)
			{
				if (eggIds[i] == id)
				{
					collectedEggs[i] = true;
					images[i].color = Color.white;
					return;
				}
			}
		}

		public void TransferEggs()
		{
			for (int i = 0; i < collectedEggs.Length; i++)
			{
				if (collectedEggs[i])
				{
					transferedEggs[i] = true;
				}
			}
		}

		private void ResetEggs()
		{
			for(int i = 0; i < transferedEggs.Length; i++)
			{
				if (!transferedEggs[i])
				{
					Debug.Log("untransfered egg reset");
					eggs[i].SetActive(true);
					collectedEggs[i] = false;
					images[i].color = Color.gray;
				}
			}
		}
	}
}
