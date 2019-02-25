using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class Enemy : Creature
    {
        protected EnemySightDetection m_sight;
        public EnemySightDetection sight
        {
            get
            {
                return m_sight;
            }
        }
        public List<Transform> patrolRoute;
        public float dectectionRange = 10, attackRange = 2, actionCooldown = 1, baitEatingSpeed = 0.2f;
        protected float NearPatrolPoint = 0.4f;
        public float NEAR_PATROL_POINT
        {
            get
            {
                return NearPatrolPoint;
            }
        }
        protected Transform m_target;
        public Transform target
        {
            get
            {
                return m_target;
            }
            set
            {
                m_target = value;
            }
        }

        protected StateMachine<Enemy> m_stateMachine;

        protected override void init()
        {
            base.init();
            m_stateMachine = new StateMachine<Enemy>(this);
            m_stateMachine.SetState(new PatrolState(m_stateMachine));
            m_sight = GetComponentInChildren<EnemySightDetection>();
            NearPatrolPoint += m_boxCollider.bounds.extents.x;
            attackRange += m_boxCollider.bounds.extents.x;
        }

        public virtual void TargetSpotted(Transform Target)
        {
            EnemyTarget possibleTarget = Target.GetComponent<EnemyTarget>();
            if (m_target == null || (possibleTarget != null && possibleTarget.GetPriority() > m_target.GetComponent<EnemyTarget>().GetPriority()))
            {
                m_stateMachine.SetState(new ChaseState(m_stateMachine, Target));
            }
        }

        public virtual void EnterAttackState()
        {
            //m_stateMachine.SetState(new AttackState<Enemy>(m_stateMachine));
        }

        public bool IsNearPoint(Vector2 point)
        {
            return Vector2.Distance(transform.position, point) < NearPatrolPoint;
        }

        public Vector2 GetTargetDirection()
        {
            return m_target.position - transform.position;
        }

        public int GetMoveDirection(Vector2 pos)
        {
            return (int)Mathf.Sign((pos - (Vector2)transform.position).x);
        }

        public bool IsNextToCliff(float direction)
        {
            return !Physics2D.Raycast(transform.position + new Vector3(m_boxCollider.bounds.extents.x*Mathf.Sign(direction), - m_boxCollider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        }

        public bool IsReachable(Vector3 targetPosition)
        {
            Vector3 dir = targetPosition - transform.position;
            return !Physics2D.Raycast(transform.position, dir, dir.magnitude, LayerMask.GetMask("Ground"));
        }

        public void AlertEnemy(Vector3 searchPoint)
        {
            if (m_stateMachine.currentState.CompareState("PatrolState") || m_stateMachine.currentState.CompareState("InvestigateState"))
            {
                m_stateMachine.SetState(new InvestigateState(m_stateMachine, searchPoint));
            }
        }

    }
}
