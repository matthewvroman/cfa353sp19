using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class TumbleSpike : DeadlyObstacle
	{
		public float speed = 3;
		int dir = 1;

		// Use this for initialization
		void Start () 
		{
			init();
		}
		
		// Update is called once per frame
		void Update () 
		{
			m_rigidbody.velocity = Vector2.right*dir*speed;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.CompareTag("Ground"))
			{
				ContactPoint2D[] contacts = new ContactPoint2D[1];
				if (other.GetContacts(contacts) > 0)
				{
					float angle = Vector2.Angle(contacts[0].point, Vector2.right);
					if (angle <= 30 || angle >= 150)
					{
						dir *= -1;
					}
				}
			}
			KillPlayer(other.gameObject);
		}
	}
}
