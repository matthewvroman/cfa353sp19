using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class PatrolState : State<Enemy>
    {
        int patrolIndex = 0, patrolDir = 1, moveDir;

        public PatrolState(StateMachine<Enemy> machine): base(machine) 
        {
            float distance = 1000;
            foreach (Transform t in m_stateMachine.controller.patrolRoute)
            {
                float dis = Vector3.Distance(t.position , m_stateMachine.controller.transform.position);
                if (dis < distance)
                {
                    distance = dis;
                    patrolIndex = m_stateMachine.controller.patrolRoute.IndexOf(t);
                }
            }
            moveDir = m_stateMachine.controller.GetMoveDirection(m_stateMachine.controller.patrolRoute[patrolIndex].position);
        }

        public override void OnEnter()
        {
            if (m_stateMachine.controller.stateIndicator != null)
            {
                GameObject.Destroy(m_stateMachine.controller.stateIndicator);
                m_stateMachine.controller.stateIndicator = null;
            }
        }

        public override void OnExit()
        {
            m_stateMachine.controller.StopSound("Walk");
        }

        public override void OnUpdate()
        {
            if (m_stateMachine.controller.IsNearPoint(m_stateMachine.controller.patrolRoute[patrolIndex].position))
            {
                UpdatePatrol();
            }
            else
            {
                m_stateMachine.controller.Move(moveDir, false);
            }
        }

        private void UpdatePatrol()
        {
            patrolIndex += patrolDir;
            if (patrolIndex >= m_stateMachine.controller.patrolRoute.Count || patrolIndex < 0)
            {
                patrolDir *= -1;
                patrolIndex += patrolDir*2;
            }

            moveDir = m_stateMachine.controller.GetMoveDirection(m_stateMachine.controller.patrolRoute[patrolIndex].position);

            m_stateMachine.SetState(new WaitState(m_stateMachine, this, 2));
        }

        public override void CollisionEntered(Collision2D collision)
		{
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                m_stateMachine.controller.Knockback(collision, player);
            }
		}

        public override void TriggerEntered(Collider2D collider)
        {
            m_stateMachine.controller.TriggeredPlayer(collider);
        }
    }
}
