using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class PatrolState : State<Enemy>
    {
        int patrolIndex = 0;
        int patrolDir = 1;
        int moveDir = 1;

        public PatrolState(StateMachine<Enemy> machine): base(machine) {}

        public override void OnEnter()
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

        public override void OnExit()
        {
            //Nothing to Do
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
            if (patrolIndex >= m_stateMachine.controller.patrolRoute.Count)
            {
                patrolDir = -1;
                patrolIndex += patrolDir*2;
            }
            else if (patrolIndex < 0)
            {
                patrolDir = 1;
                patrolIndex += patrolDir*2;
            }

            moveDir = m_stateMachine.controller.GetMoveDirection(m_stateMachine.controller.patrolRoute[patrolIndex].position);
        }
    }
}
