using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class State<T> 
	{
		protected StateMachine<T> m_stateMachine;

		public State(StateMachine<T> machine)
		{
			m_stateMachine = machine;
		}
		public virtual void OnEnter(){}

		public virtual void OnExit(){}

		public virtual void OnUpdate(){}

		public virtual void TriggerEntered(Collider2D collider) {}

		public virtual void TriggerStayed(Collider2D collider) {}

		public virtual void TriggerExited(Collider2D collider) {}

		public virtual void CollisionEntered(Collision2D collision) {}

		public virtual void CollisionStayed(Collision2D collision) {}

		public virtual void CollisionExited(Collision2D collision) {}

		public bool CompareState(string other)
		{
			string state = this.GetType().ToString();
			state = state.Substring(state.LastIndexOf('.') + 1);
			return state.Equals(other);
		}
	}
}
