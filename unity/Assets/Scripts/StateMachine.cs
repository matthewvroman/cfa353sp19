using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class StateMachine<T> : MonoBehaviour 
	{
		State<T> m_currentState;
		public State<T> currentState
		{
			get
			{
				return m_currentState;
			}
		}


		public void SetState(State<T> newState)
		{
            if (m_currentState != null)
            {
                m_currentState.OnExit();
            }

            m_currentState = newState;
            m_currentState.OnEnter();
		}

        public void OnUpdate()
        {
            m_currentState.OnUpdate();
        }
	}
}
