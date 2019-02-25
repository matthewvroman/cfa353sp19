using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class MiniMap : MonoBehaviour 
	{
		[SerializeField]
		RectTransform goal, egg, player;
		[SerializeField]
		Transform startOfLevel, endOfLevel;
		float levelLength;
		float miniMapLength;

		void OnEnable()
		{
			PlayerController.UpdateMapPosition += UpdatePlayerPosition;
			Egg.UpdateMapPosition += UpdateEggPosition;
			Goal.UpdateMapPosition += UpdateGoalPosition;
		}

		void OnDisable()
		{
			PlayerController.UpdateMapPosition -= UpdatePlayerPosition;
			Egg.UpdateMapPosition -= UpdateEggPosition;
			Goal.UpdateMapPosition -= UpdateGoalPosition;
		}

		// Use this for initialization
		void Awake() 
		{
			levelLength = Mathf.Abs(endOfLevel.position.x - startOfLevel.position.x);
			miniMapLength = transform.Find("Map Background").GetComponent<RectTransform>().sizeDelta.x - player.sizeDelta.x;
		}

		private void UpdatePlayerPosition(Vector3 pos)
		{
			player.anchoredPosition = UpdatePositionOnMap(pos);
		}

		private void UpdateEggPosition(bool active, Vector3 position = default(Vector3))
		{
			if (active)
			{
				egg.anchoredPosition = UpdatePositionOnMap(position);
			}
			else
			{
				Destroy(egg.gameObject);
			}
		}

		private void UpdateGoalPosition(bool active, Vector3 pos)
		{
			goal.gameObject.SetActive(active);
			goal.anchoredPosition = UpdatePositionOnMap(pos);
		}

		private Vector2 UpdatePositionOnMap(Vector3 pos)
		{
			float percentage = Mathf.Abs(pos.x - startOfLevel.position.x)/levelLength;
			return new Vector2(percentage*miniMapLength,0);
		}
	}
}
