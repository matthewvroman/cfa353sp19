using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class Obstacle : MonoBehaviour 
	{
		protected Collider2D m_collider;
		protected SpriteRenderer m_spriteRenderer;

		public virtual void init()
		{
			if (!GetComponent<Collider2D>())
			{
				gameObject.AddComponent<BoxCollider2D>();
			}
			m_collider = GetComponent<Collider2D>();
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}
	}
}
