using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class Enemy : Creature
    {
        public enum AttackType
        {
            Pounce,
            Bombard,
            Swing
        }
        [HideInInspector]
        public CircleCollider2D m_killBox;
        [HideInInspector]
        public EnemySightDetection m_sight;
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
            m_killBox = GetComponentInChildren<CircleCollider2D>();
            m_killBox.isTrigger = true;
            m_killBox.enabled = false;
            m_stateMachine = new StateMachine<Enemy>(this);
            m_stateMachine.SetState(new PatrolState(m_stateMachine));
            m_sight = GetComponentInChildren<EnemySightDetection>();
            canvas = GameObject.Find("Canvas").transform;
            NearPatrolPoint += m_boxCollider.bounds.extents.x;
            attackRange += m_boxCollider.bounds.extents.x;
        }

        public override void UpdateAnimatorMovement(float input, bool running)
        {
            string name = gameObject.name;
            if (input == 0)
            {
                PlayAnimation("Idle");
            }
            else if (running)
            {
                PlayAnimation("Run");
            }
            else
            {
                PlayAnimation("Walk");
            }
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
            if (Target == target) return;
            PlayerController player = Target.GetComponent<PlayerController>();
            if (juvenile && player)
            {
                m_stateMachine.SetState(new PanicState(m_stateMachine, Target));
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
                case AttackType.Pounce:
                {
                    m_stateMachine.SetState(new PounceState(m_stateMachine));
                    break;
                }
                case AttackType.Bombard:
                {
                    m_stateMachine.SetState(new BombardState(m_stateMachine));
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

                    float dir = (searchPoint - ((Vector3)m_boxCollider.offset + transform.position)).x;
                    if ((m_facingRight && dir < 0) || (!m_facingRight && dir >= 0)) SwitchOrientation();
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

        public void PlayAnimation(string animName)
        {
            m_animator.Play(animName, 0);
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
            player.m_rigidbody.AddForce((player.transform.position - transform.position).normalized * 1, ForceMode2D.Impulse);
        }

        public void Attack()
        {
            m_killBox.enabled = !m_killBox.enabled;
            if (m_stateMachine.currentState is BombardState)
            {
                BombardState b = m_stateMachine.currentState as BombardState;
                b.Bombard(transform.GetChild(2).position);

            }
        }

        public void TriggeredPlayer(Collider2D collider)
        {
            if (collider.GetComponent<PlayerController>() != null)
            {
                AlertEnemy(collider.transform.position);
            }
        }
        
        public bool IsNearPoint(Vector2 point)
        {
            return Mathf.Abs(point.x - transform.position.x) < NearPatrolPoint;
        }

        public Vector2 GetTargetDirection()
        {
            return target.position - (transform.position + (Vector3)m_boxCollider.offset);
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
                origin = m_boxCollider.offset + (Vector2)transform.position;
            }
            Vector2 y = new Vector2(0, m_boxCollider.bounds.extents.y);
            Vector2 pos = position - origin;
            RaycastHit2D upHit = Physics2D.Raycast(origin + y, Vector2.right*pos.x, pos.magnitude, LayerMask.GetMask("Ground"));
            RaycastHit2D downHit = Physics2D.Raycast(origin - y, Vector2.right*pos.x, pos.magnitude, LayerMask.GetMask("Ground"));
            if (upHit)
            {
                if (!downHit || Vector2.Distance(upHit.point, origin) > Vector2.Distance(upHit.point, origin))
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
    }
}