using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class Rock : DeadlyObstacle 
	{
		float timer = 5;
		// Use this for initialization
		void Start () 
		{
			init();
		}
		
		// Update is called once per frame
		void Update () 
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				Destroy(gameObject);
			}
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			if (m_rigidbody.velocity.y < -0.1f)
			{
				Debug.Log("y velocity: " + m_rigidbody.velocity.y);
				KillPlayer(other.gameObject);
			}
		}
	}
}
