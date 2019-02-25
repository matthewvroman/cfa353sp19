using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class PlayerController :  Creature
    {
        GameObject bait;
        CircleCollider2D circleCollider;
        [SerializeField]
        float jumpFprce = 6, fallMultiplyer = 3, lowJumpMultiplyer = 2;
        int numBait = 5;
        bool crouching = false;
        bool climbing = false;

        protected override void init()
        {
            base.init();
            bait = Resources.Load<GameObject>("Prefabs/bait");
            circleCollider = GetComponent<CircleCollider2D>();
        }

        // Use this for initialization
        void Start()
        {
            init();
        }

        // Update is called once per frame
        void Update()
        {
            DropBait();
            Jump();
            Crouch();
            Climb();
            Move(Input.GetAxis("Horizontal"), !crouching);
        }

//===============================================================================================================================================================================
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && (m_grounded || climbing))
            {
                m_rigidbody.gravityScale = 1;
                m_rigidbody.velocity += Vector2.up * jumpFprce;
                m_grounded = false;
                climbing = false;
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
                if (!crouching && m_grounded && !climbing)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    m_spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Crouched");
                    crouching = true;
                }
            }
            else
            {
                if (crouching)
                {
                    GetComponent<BoxCollider2D>().enabled = true;
                    m_spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Standard");
                    crouching = false;
                }
            }
        }

//================================================================================================================================================================================
        private void Climb()
        {
            if (climbing && !crouching)
            {
                float input = Input.GetAxis("Climb");
                if (input != 0)
                {
                    m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, Mathf.Clamp(input, 0, 1) * m_runSpeed);
                }
                else
                {
                    m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_rigidbody.velocity.y * (1 - 10 * Time.deltaTime));
                }
            }
        }

//================================================================================================================================================================================
        private void DropBait()
        {
            if (numBait > 0 && Input.GetKeyDown(KeyCode.F))
            {
                CircleCollider2D collider = GetComponent<CircleCollider2D>();
                Instantiate(bait, (Vector2)transform.position + collider.offset + new Vector2(collider.radius, -collider.radius - 0.1f), Quaternion.identity, null);
                numBait--;
            }
        }

//===============================================================================================================================================================================
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") && IsGrounded(circleCollider))
            {
                m_grounded = true;
            }
        }

//===============================================================================================================================================================================
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") && !IsGrounded(circleCollider))
            {
                m_grounded = false;
            }
        }

//===============================================================================================================================================================================
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Climbable"))
            {
                m_rigidbody.gravityScale = 0;
                climbing = true;
            }
        }

//===============================================================================================================================================================================
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Climbable"))
            {
                m_rigidbody.gravityScale = 1;
                climbing = false;
            }
        }
    }

}
