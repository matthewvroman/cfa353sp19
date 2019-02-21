using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class PlayerController :  Creature
    {
        GameObject bait;
        [SerializeField]
        float jumpFprce = 6, fallMultiplyer = 3, lowJumpMultiplyer = 2;
        int numBait = 5;
        bool crouching = false;

        protected override void init()
        {
            base.init();
        }

        // Use this for initialization
        void Start()
        {
            init();
        }

        // Update is called once per frame
        void Update()
        {
            Jump();
            Crouch();
            Move(Input.GetAxis("Horizontal"), !crouching);
        }

        //===============================================================================================================================================================================
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && m_grounded)
            {
                m_rigidbody.velocity += Vector2.up * jumpFprce;
                m_grounded = false;
            }

            if (!m_grounded && m_rigidbody.velocity.y < 0)
            {
                m_rigidbody.velocity += Vector2.up * Physics.gravity.y * (fallMultiplyer - 1) * Time.deltaTime;
            }
            else if (m_rigidbody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                m_rigidbody.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplyer - 1) * Time.deltaTime;
            }
        }

//===============================================================================================================================================================================
        private void Crouch()
        {
            if (Input.GetAxis("Crouch") > 0)
            {
                if (!crouching)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player_Crouched");
                    crouching = true;
                }
            }
            else
            {
                if (crouching)
                {
                    GetComponent<BoxCollider2D>().enabled = true;
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player_Standard");
                    crouching = false;
                }
            }
        }

//===============================================================================================================================================================================
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") && IsGrounded())
            {
                m_grounded = true;
            }
        }

//===============================================================================================================================================================================
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") && !IsGrounded())
            {
                m_grounded = false;
            }
        }

        protected override bool IsGrounded()
        {
            CircleCollider2D boxCollider = GetComponent<CircleCollider2D>();
            return Physics2D.Raycast((Vector2)transform.position + boxCollider.offset - new Vector2(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground")) ||
                Physics2D.Raycast((Vector2)transform.position + boxCollider.offset - new Vector2(-boxCollider.bounds.extents.x, boxCollider.bounds.extents.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        }
    }
}
