﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class StateMachine<T>
    {
        T m_controller;
        public T controller
        {
            get
            {
                return m_controller;
            }
        }

        State<T> m_currentState = null;
        public State<T> currentState
        {
            get
            {
                return m_currentState;
            }
        }

        public StateMachine(T stateController)
        {
            m_controller = stateController;
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

        public void TriggerEntered(Collider2D collider) 
        {
            m_currentState.TriggerEntered(collider);
        }

		public void TriggerStayed(Collider2D collider) 
        {
            m_currentState.TriggerStayed(collider);
        }

		public void TriggerExited(Collider2D collider) 
        {
            m_currentState.TriggerExited(collider);
        }

		public void CollisionEntered(Collision2D collision) 
        {
            m_currentState.CollisionEntered(collision);
        }

		public void CollisionStayed(Collision2D collision) 
        {
            m_currentState.CollisionStayed(collision);
        }

		public void CollisionExited(Collision2D collision) 
        {
            m_currentState.CollisionExited(collision);
        }
	}
}
