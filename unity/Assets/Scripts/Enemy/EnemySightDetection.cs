using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class EnemySightDetection : MonoBehaviour 
	{
		Enemy enemy;
		Collider2D sightRange;
		ContactFilter2D filter;

		void Start()
		{
			enemy = GetComponentInParent<Enemy>();
			if (GetComponent<Collider2D>() == null)
			{
				Collider2D col = gameObject.AddComponent<BoxCollider2D>();
				col.isTrigger = true;
				col.offset = new Vector2(3,0);
			}
			sightRange = GetComponent<Collider2D>();
			filter = new ContactFilter2D();
			string[] layerNames = {"Player", "Bait"};
			filter.layerMask = LayerMask.GetMask(layerNames);
			filter.useTriggers = true;
		}

		public Transform CheckForTargets()
		{
			Collider2D[] colliders = new Collider2D[6];
			int highestPriority = -1;
			Transform newTarget = null;
			int length = sightRange.OverlapCollider(filter, colliders);
			if (length > 0)
			{
				for(int i = 0; i < length; i++)
				{
					EnemyTarget target = colliders[i].GetComponent<EnemyTarget>();
					if (target != null && target.GetPriority() > highestPriority)
					{
						highestPriority = target.GetPriority();
						newTarget = colliders[i].transform;
					}
				}
			}
			return newTarget;
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			EnemyTarget target = other.GetComponentInParent<EnemyTarget>();
			if (target != null)
			{
				enemy.TargetSpotted(target.transform);
			}
		}
	}
}
