using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bradley.AlienArk
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Creature : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody2D m_rigidbody;
        [HideInInspector]
        public BoxCollider2D m_boxCollider;
        [HideInInspector]
        public Animator m_animator;
        [HideInInspector]
        public SpriteRenderer m_spriteRenderer;
        [HideInInspector]
        public SFXManager m_sound;

        [SerializeField]
        protected float m_walkSpeed = 1, m_runSpeed = 2;
        protected float speedModifier = 1;
        protected bool m_facingRight = true;
        protected bool m_grounded = false;

        protected virtual void init()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_boxCollider = GetComponent<BoxCollider2D>();
            m_animator = GetComponent<Animator>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_sound = GetComponentInChildren<SFXManager>();
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
            transform.localScale = new Vector3(-1*transform.localScale.x, transform.localScale.y,0);
            m_facingRight = !m_facingRight;
        }

        public void SetSpeedModifier(float modifier)
        {
            speedModifier = modifier;
        }

        public float GetSpeedModifier()
        {
            return speedModifier;
        }

        
        public virtual void Move(float input, bool running)
        {
            float speed = m_walkSpeed;
            if (running)
            {
                speed = m_runSpeed;
            }

            UpdateAnimatorMovement(input, running);
            ApplyMovement(input, speed);
        }

        public virtual void ApplyMovement(float input, float speed)
        {
            CheckOrientation(input);

            if (input != 0)
            {
                m_rigidbody.velocity = new Vector2(Mathf.Sign(input)*speed*speedModifier, m_rigidbody.velocity.y);
            }
            else
            {
                ApplyDrag();
            }
        }

        public void ApplyDrag()
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x * (1 - 10 * Time.fixedDeltaTime), m_rigidbody.velocity.y);
            UpdateAnimatorMovement(0, false);
        }

        public void StopMovement()
        {
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
        }

        public virtual void UpdateAnimatorMovement(float input, bool running)
        {
            //Override Me
        }

        public void PlayAnimation(string animName)
        {
            m_animator.Play(animName, 0);
        }

        public void PlaySound(string soundName)
        {
            m_sound.Play(soundName);
        }

        public void StopSound(string soundName)
        {
            m_sound.Stop(soundName);
        }

        public virtual bool IsGrounded(Collider2D collider)
        {
            return Physics2D.Raycast((Vector2)transform.position + collider.offset - new Vector2(collider.bounds.extents.x, collider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground")) ||
                Physics2D.Raycast((Vector2)transform.position + collider.offset - new Vector2(-collider.bounds.extents.x, collider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        }
    }
}
