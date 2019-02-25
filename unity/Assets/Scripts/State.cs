using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class State<T> : MonoBehaviour 
	{
		protected StateMachine<T> m_stateMachine;

		public State(StateMachine<T> machine)
		{
			m_stateMachine = machine;
		}
		public virtual void OnEnter()
		{

		}

		public virtual void OnExit()
		{
			
		}

		public virtual void OnUpdate()
		{
			
		}
	}
}
