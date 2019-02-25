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
			sightRange.OverlapCollider(filter, colliders);
			int highestPriority = -1;
			Transform newTarget = null;
			foreach(Collider2D col in colliders)
			{
				int priority = col.GetComponent<EnemyTarget>().GetPriority();
				if (priority > highestPriority)
				{
					highestPriority = priority;
					newTarget = col.transform;
				}
			}
			return newTarget;
		}

		/// <summary>
		/// Sent when another object enters a trigger collider attached to this
		/// object (2D physics only).
		/// </summary>
		/// <param name="other">The other Collider2D involved in this collision.</param>
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
