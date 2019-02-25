using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class PlayerController :  Creature
    {
        public static System.Action PlayerDied;
        public static System.Action<Vector3> UpdateMapPosition;
        GameObject bait;
        CircleCollider2D circleCollider;
        BoxCollider2D detectionBox;
        [SerializeField]
        float jumpFprce = 6, fallMultiplyer = 3, lowJumpMultiplyer = 2;
        int numBait = 5;
        bool crouching = false;
        bool climbing = false;
        bool canHide = false;
        bool hiding = false;

        protected override void init()
        {
            base.init();
            bait = Resources.Load<GameObject>("Prefabs/bait");
            circleCollider = GetComponent<CircleCollider2D>();
            detectionBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
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
            if (UpdateMapPosition != null)
            {
                UpdateMapPosition(transform.position);
            }
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
                    m_boxCollider.enabled = false;
                    m_spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Crouched");
                    if (canHide)
                    {
                        Hide(true);
                    }
                    crouching = true;
                }
            }
            else
            {
                if (crouching)
                {
                    m_boxCollider.enabled = true;
                    m_spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Standard");
                    Hide(false);
                    crouching = false;
                }
            }
        }

//================================================================================================================================================================================
        public bool IsCrouching()
        {
            return crouching;
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
        private void Hide(bool value)
        {
            Debug.Log("Hiding");
            detectionBox.enabled = !value;
            if (value)
            {
                m_spriteRenderer.color = Color.gray;
            }
            else
            {
                m_spriteRenderer.color = Color.white;
            }
            Physics2D.IgnoreLayerCollision(9, 12, value);
            hiding = value;
        }

//================================================================================================================================================================================
        private void DropBait()
        {
            if (Input.GetKeyDown(KeyCode.F) && numBait > 0)
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
            else if (collision.gameObject.GetComponent<Enemy>())
            {
                PlayerDied();
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
            if (collision.CompareTag("Climbable"))
            {
                m_rigidbody.gravityScale = 0;
                climbing = true;
            }
            else if (collision.CompareTag("Hideable"))
            {
                canHide = true;
                if (crouching)
                {
                    Hide(true);
                }
                Debug.Log("can hide is " + canHide);
            }
        }

//===============================================================================================================================================================================
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Climbable"))
            {
                m_rigidbody.gravityScale = 1;
                climbing = false;
            }
            else if (collision.CompareTag("Hideable") && !circleCollider.IsTouching(collision))
            {
                canHide = false;
                if (hiding)
                {
                    Hide(false);
                }
                Debug.Log("Can Hide is " + canHide);
            }
        }
    }
}
