using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SwingState : State<Enemy> 
	{
		Transform attackPoint;
		Vector3 searchPoint;
		float attackSize = 0.75f;
		float attackTime = 1;
		bool initialized = false;

		public SwingState(StateMachine<Enemy> machine) : base(machine)
		{
			attackPoint = m_stateMachine.controller.transform.GetChild(1);
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.CreateStateIndicator("Attack");
			m_stateMachine.controller.stateIndicator.GetComponent<StateIndicator>().SetIndicator(m_stateMachine.controller);

			if (!initialized)
			{
				m_stateMachine.SetState(new WaitState(m_stateMachine, this, 0.3f));
			}
			else
			{
				searchPoint = m_stateMachine.controller.target.position;
				string name = m_stateMachine.controller.gameObject.name;
				m_stateMachine.controller.PlayAnimation("Attack");
				m_stateMachine.controller.m_sound.Play("Cry");
				attackTime = m_stateMachine.controller.m_animator.GetCurrentAnimatorClipInfo(0).Length;
			}
		}

		public override void OnExit()
		{
			if (initialized)
			{
				GameObject.Destroy(m_stateMachine.controller.stateIndicator);
			}
			else
			{
				initialized = true;
			}
		}

		public override void OnUpdate()
		{
			attackTime -= Time.deltaTime;
			if (attackTime <= 0)
			{
				Transform target = m_stateMachine.controller.m_sight.CheckForTargets();
				if (target != null)
				{
					m_stateMachine.SetState(new ChaseState(m_stateMachine, target));
				}
				else
				{
					m_stateMachine.SetState(new InvestigateState(m_stateMachine, m_stateMachine.controller.transform.position));
				}
			}
		}
	}
}
