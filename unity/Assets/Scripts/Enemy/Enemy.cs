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
        public float dectectionRange = 10, attackRange = 2, baitEatingSpeed = 0.2f;
        protected float NearPatrolPoint = 0.4f;
        public float NEAR_PATROL_POINT
        {
            get
            {
                return NearPatrolPoint;
            }
        }
        [HideInInspector]
        public Transform target;
        [HideInInspector]
        public Transform canvas;
        [HideInInspector]
        public GameObject stateIndicator;

        protected StateMachine<Enemy> m_stateMachine;

        protected override void init()
        {
            base.init();
            m_stateMachine = new StateMachine<Enemy>(this);
            m_stateMachine.SetState(new PatrolState(m_stateMachine));
            m_sight = GetComponentInChildren<EnemySightDetection>();
            canvas = GameObject.Find("Canvas").transform;
            NearPatrolPoint += m_boxCollider.bounds.extents.x;
            attackRange += m_boxCollider.bounds.extents.x;
        }

        public virtual void TargetSpotted(Transform Target)
        {
            EnemyTarget possibleTarget = Target.GetComponent<EnemyTarget>();
            if (target == null || (possibleTarget != null && possibleTarget.GetPriority() > target.GetComponent<EnemyTarget>().GetPriority()))
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
            return Mathf.Abs(point.x - transform.position.x) < NearPatrolPoint;
        }

        public Vector2 GetTargetDirection()
        {
            return target.position - transform.position;
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
            return !Physics2D.Raycast(transform.position, dir, dir.magnitude, LayerMask.GetMask("Ground")) && !IsNextToCliff(dir.x);
        }

        public bool HeadOnCollision(Collision2D collision)
        {
            Vector3 normal = collision.contacts[0].normal;
            return Vector3.Angle(normal, new Vector3(m_facingRight ? -1 : 1,0,0)) < 60;
        }

        public void AlertEnemy(Vector3 searchPoint)
        {
            Debug.Log("Alert Enemy called");
            if (m_stateMachine.currentState.CompareState("PatrolState") || m_stateMachine.currentState.CompareState("WaitState") || m_stateMachine.currentState.CompareState("SearchState"))
            {
                Debug.Log("Moving towards " + searchPoint);
                m_stateMachine.SetState(new InvestigateState(m_stateMachine, searchPoint));
            }
        }

        public void CreateStateIndicator(string name)
        {
            if (stateIndicator != null)
            {
                Destroy(stateIndicator);
            }
            stateIndicator = Instantiate(Resources.Load<GameObject>("Prefabs/StateIndicators/" + name), canvas);
            stateIndicator.transform.SetAsFirstSibling();
        }

        public void SearchCompleted()
        {
            m_stateMachine.SetState(new PatrolState(m_stateMachine));
        }

    }
}
