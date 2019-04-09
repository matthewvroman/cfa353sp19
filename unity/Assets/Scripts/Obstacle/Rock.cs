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
			transform.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z + 30*Time.deltaTime);
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				Destroy(gameObject);
			}
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.CompareTag("Ground"))
			{
				Destroy(gameObject);
				return;
			}
			KillPlayer(other.gameObject);
		}
		
		public void SetSprite(int levelNum, int spriteNum)
		{
			if (!m_spriteRenderer)
			{
				m_spriteRenderer = GetComponent<SpriteRenderer>();
			}
			m_spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Obstacles/" + levelNum + "S Rock " + spriteNum);
		}
	}
}
