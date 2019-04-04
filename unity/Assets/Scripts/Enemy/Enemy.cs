using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class Enemy : Creature
    {
        public enum AttackType
        {
            Charge,
            Pounce,
            Shoot,
            Bombard,
            Peck,
            Swing
        }
        protected EnemySightDetection m_sight;
        public EnemySightDetection sight
        {
            get
            {
                return m_sight;
            }
        }
        public List<Transform> patrolRoute;
        public AttackType attackType;
        public bool juvenile = false;
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

//=====================================================================================================================================================================================
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

//=====================================================================================================================================================================================
        private void Start()
        {
            init();
        }

        private void Update()
        {
            m_stateMachine.OnUpdate();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            m_stateMachine.CollisionEntered(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            m_stateMachine.TriggerEntered(other);
        }
        
//=====================================================================================================================================================================================

        public virtual void TargetSpotted(Transform Target)
        {
            EnemyTarget possibleTarget = Target.GetComponent<EnemyTarget>();
            PlayerController player = Target.GetComponent<PlayerController>();
            if (juvenile && player)
            {
                m_stateMachine.SetState(new PanicState(m_stateMachine, Target.position));
            }
            else if (target == null || (possibleTarget != null && possibleTarget.GetPriority() > target.GetComponent<EnemyTarget>().GetPriority()))
            {
                m_stateMachine.SetState(new ChaseState(m_stateMachine, Target));
            }
        }

        public virtual void EnterAttackState()
        {
            switch (attackType)
            {
                case AttackType.Charge:
                {
                    m_stateMachine.SetState(new ChargeState(m_stateMachine));
                    break;
                }
                case AttackType.Pounce:
                {
                    m_stateMachine.SetState(new PounceState(m_stateMachine));
                    break;
                }
                case AttackType.Shoot:
                {
                    m_stateMachine.SetState(new ShootState(m_stateMachine));
                    break;
                }
                case AttackType.Bombard:
                {
                    m_stateMachine.SetState(new BombardState(m_stateMachine));
                    break;
                }
                case AttackType.Peck:
                {
                    m_stateMachine.SetState(new PeckState(m_stateMachine));
                    break;
                }
                case AttackType.Swing:
                {
                    m_stateMachine.SetState(new SwingState(m_stateMachine));
                    break;
                }
            }
        }

        public void AlertEnemy(Vector3 searchPoint)
        {
            string[] alertableStates = {"PatrolState", "WaitState", "SearchState", "InvestigateState"}; 
            foreach (string s in alertableStates)
            {
                if (m_stateMachine.currentState.CompareState(s))
                {
                    if (s.Equals("SearchState") && !((SearchState)m_stateMachine.currentState).IsAlertable(searchPoint)) return;
                    else if (s.Equals("WaitState") && !((WaitState)m_stateMachine.currentState).IsAlertable(searchPoint)) return;
                    else if (s.Equals("InvestigateState") && !((InvestigateState)m_stateMachine.currentState).IsAlertable(searchPoint)) return;
                    
                    m_stateMachine.SetState(new InvestigateState(m_stateMachine, searchPoint));
                    return;
                }
            }
        }

        public void CreateStateIndicator(string name)
        {
            if (stateIndicator != null)
            {
                Destroy(stateIndicator);
            }
            stateIndicator = Instantiate(Resources.Load<GameObject>("StateIndicators/" + name), Vector2.zero, Quaternion.identity, canvas);
            stateIndicator.transform.SetAsFirstSibling();
        }

        public void SearchCompleted()
        {
            m_stateMachine.SetState(new PatrolState(m_stateMachine));
        }

//============================================================================================================================================================================================================
        public bool CheckForTarget()
        {
            string[] layers = {"Ground","Player"};
            return target != null && Physics2D.Raycast(transform.position, target.position - transform.position, dectectionRange, LayerMask.GetMask(layers));
        }

        public void Knockback(Collision2D collision, PlayerController player)
        {
            AlertEnemy(collision.contacts[0].point);
            player.Rigidbody.AddForce((player.transform.position - transform.position).normalized * 3, ForceMode2D.Impulse);
        }

        public void TriggeredPlayer(Collider2D collider)
        {
            if (collider.GetComponent<PlayerController>() != null)
            {
                Debug.Log("Alerting Enemy");
                AlertEnemy(collider.transform.position);
            }
        }

        public void KillPlayer(GameObject player)
        {
            if (player.GetComponent<PlayerController>())
            {
                PlayerController.PlayerDied();
            }
        }
        
        public bool IsNearPoint(Vector2 point)
        {
            return Mathf.Abs(point.x - transform.position.x) < NearPatrolPoint;
        }

        public Vector2 GetTargetDirection()
        {
            return target.position - transform.position;
        }

        public Vector2 GetForwardPosition()
        {
            return ((Vector2)transform.position + m_boxCollider.offset + new Vector2(m_boxCollider.bounds.extents.x * (m_facingRight ? 1 : -1), 0));
        }

        public int GetMoveDirection(Vector2 pos)
        {
            return (int)Mathf.Sign((pos - (Vector2)transform.position).x);
        }

        public bool IsNextToCliff(float direction)
        {
            return  !Physics2D.Raycast((Vector2)transform.position + m_boxCollider.offset + new Vector2(m_boxCollider.bounds.extents.x * Mathf.Sign(direction),
                -m_boxCollider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        }

        public bool IsReachable(Vector3 targetPosition)
        {
            Vector3 dir = targetPosition - transform.position;
            return !CheckDistance(targetPosition) && !IsNextToCliff(dir.x);
        }

        public RaycastHit2D CheckDistance(Vector2 position, Vector2 origin = default(Vector2))
        {
            if (origin == default(Vector2))
            {
                origin = transform.position;
            }
            Vector2 y = new Vector2(0, m_boxCollider.bounds.extents.y);
            Vector2 dir = position - origin;
            RaycastHit2D upHit = Physics2D.Raycast(origin + y, dir, dir.magnitude, LayerMask.GetMask("Ground"));
            RaycastHit2D downHit = Physics2D.Raycast(origin - y, dir, dir.magnitude, LayerMask.GetMask("Ground"));
            if (upHit)
            {
                if (!downHit)
                {
                    return upHit;
                }
                else if (Vector2.Distance(upHit.point, origin) > Vector2.Distance(upHit.point, origin))
                {
                    return upHit;
                }
                return downHit;
            }
            else
            {
                return downHit;
            }
        }

        public bool HeadOnCollision(Collision2D collision)
        {
            Vector3 normal = collision.contacts[0].normal;
            return Vector3.Angle(normal, new Vector3(m_facingRight ? -1 : 1,0,0)) < 60;
        }

    }
}
