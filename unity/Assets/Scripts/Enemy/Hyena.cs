using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class Hyena : Enemy 
	{
		// Use this for initialization
		void Start () 
		{
			init();
		}
		
		// Update is called once per frame
		void Update () 
		{
			m_stateMachine.OnUpdate();
		}

		public override void EnterAttackState()
		{
			m_stateMachine.SetState(new ChargeState(m_stateMachine));
		}

		private void OnCollisionEnter2D(Collision2D other) 
		{
			m_stateMachine.CollisionEntered(other);
		}
	}
}
