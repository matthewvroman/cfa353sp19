using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bradley.AlienArk
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Creature : MonoBehaviour
    {
        protected Rigidbody2D m_rigidbody;
        public Rigidbody2D Rigidbody
        {
            get
            {
                return m_rigidbody;
            }
        }
        protected SpriteRenderer m_spriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                return m_spriteRenderer;
            }
        }
        protected BoxCollider2D m_boxCollider;
        public BoxCollider2D BoxCollider
        {
            get
            {
                return m_boxCollider;
            }
        }

        [SerializeField]
        protected float m_walkSpeed = 1, m_runSpeed = 2;
        protected bool m_facingRight = true;
        protected bool m_grounded = false;

        protected virtual void init()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_boxCollider = GetComponent<BoxCollider2D>();
        }

        public virtual void Move(float input, bool running)
        {
            float speed = m_walkSpeed;
            if (running)
            {
                speed = m_runSpeed;
            }

            ApplyMovement(input, speed);
        }

        public void CheckOrientation(float direction)
        {
            if ((m_facingRight && direction < 0) || (!m_facingRight && direction > 0))            
            {
                SwitchOrientation();
            }
        }

        public void SwitchOrientation()
        {
            transform.localScale = new Vector3(-1*transform.localScale.x,transform.localScale.y,0);
            m_facingRight = !m_facingRight;
        }

        public virtual void ApplyMovement(float input, float speed)
        {
            CheckOrientation(input);

            if (input != 0)
            {
                m_rigidbody.velocity = new Vector2(Mathf.Sign(input)*speed,m_rigidbody.velocity.y);
            }
            else
            {
                ApplyDrag();
            }
        }

        public void ApplyDrag()
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x * (1 - 10 * Time.fixedDeltaTime), m_rigidbody.velocity.y);
        }

        public void Stop()
        {
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
        }

        public virtual bool IsGrounded(Collider2D collider)
        {
            return Physics2D.Raycast((Vector2)transform.position + collider.offset - new Vector2(collider.bounds.extents.x, collider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground")) ||
                Physics2D.Raycast((Vector2)transform.position + collider.offset - new Vector2(-collider.bounds.extents.x, collider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        }
    }
}
