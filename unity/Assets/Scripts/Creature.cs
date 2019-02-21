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
        [SerializeField]
        protected float m_walkSpeed = 1, m_runSpeed = 2;
        protected Rigidbody2D m_rigidbody;
        protected bool m_facingRight = true;
        protected bool m_grounded = false;

        protected virtual void init()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Move(float input, bool running)
        {
            float speed = m_walkSpeed;
            if (running)
            {
                speed = m_runSpeed;
            }

            if (m_facingRight && input < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                m_facingRight = false;
            }
            else if (!m_facingRight && input > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                m_facingRight = true;
            }

            if (input != 0)
            {
                m_rigidbody.velocity = new Vector2(Mathf.Sign(input)*speed,m_rigidbody.velocity.y);
            }
            else
            {
                ApplyDrag();
            }
        }

        protected void ApplyDrag()
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x * (1 - 10 * Time.fixedDeltaTime), m_rigidbody.velocity.y);
        }

        protected virtual bool IsGrounded()
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            return Physics2D.Raycast((Vector2)transform.position + boxCollider.offset - new Vector2(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground")) ||
                Physics2D.Raycast((Vector2)transform.position + boxCollider.offset - new Vector2(-boxCollider.bounds.extents.x, boxCollider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        }
    }
}
