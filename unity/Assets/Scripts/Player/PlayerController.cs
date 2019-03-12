﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class PlayerController :  Creature
    {
        public static System.Action PlayerDied;
        public static System.Action<Vector3> UpdateMapPosition;
        GameObject bait;

        StateMachine<PlayerController> stateMachine;
        [HideInInspector]
        public CircleCollider2D circleCollider;
        [HideInInspector]
        public BoxCollider2D detectionBox;
        [HideInInspector]
        public bool collectedEgg = false, canClimb = false, canHide = false;
        public LayerMask Detection;

        public float jumpFprce = 6, fallMultiplyer = 3, lowJumpMultiplyer = 2;
        int numBait = 5;

        protected override void init()
        {
            base.init();
            stateMachine = new StateMachine<PlayerController>(this);
            stateMachine.SetState(new BaseState(stateMachine));

            bait = Resources.Load<GameObject>("Prefabs/bait");
            circleCollider = GetComponent<CircleCollider2D>();
            detectionBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        }

        void OnEnable()
        {
            Egg.EggCollected += CollectedEgg;
        }

        void OnDisable()
        {
            Egg.EggCollected -= CollectedEgg;
        }

        // Use this for initialization
        void Start()
        {
            init();
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.OnUpdate();
            if (UpdateMapPosition != null)
            {
                UpdateMapPosition(transform.position);
            }
        }

//================================================================================================================================================================================
        public bool IsCrouching()
        {
            return stateMachine.currentState.CompareState("CrouchState");
        }

//================================================================================================================================================================================
        public void Climb(float input)
        {
            if (input > 0)
            {
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, Mathf.Clamp(input, 0, 1) * m_runSpeed);
            }
            else
            {
                m_rigidbody.velocity = new Vector2 (m_rigidbody.velocity.x, m_rigidbody.velocity.y * (1 - 10 * Time.deltaTime));
            }
        }

//================================================================================================================================================================================
        public void Hide(bool value)
        {
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
        }

//================================================================================================================================================================================

        public void SetupCrouching(bool value)
        {
            m_boxCollider.enabled = !value;
			detectionBox.offset = (m_spriteRenderer.bounds.center - transform.position);
            detectionBox.size = new Vector2(detectionBox.size.x, m_spriteRenderer.bounds.size.y);
        }

//================================================================================================================================================================================

        public bool CanUncrouch()
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Ground"));
            filter.useTriggers = false;
            Collider2D[] colliders = new Collider2D[1];
            return Physics2D.OverlapBox((Vector2)transform.position + m_boxCollider.offset, m_boxCollider.bounds.size, 0, filter, colliders) == 0;
        }

//================================================================================================================================================================================
        public bool IsSpotted()
        {
            if (detectionBox.IsTouchingLayers(LayerMask.GetMask("Enemy Detection")))
            {
                return true;
            }
            return false;
        }

//================================================================================================================================================================================
        public void DropBait()
        {
            if (Input.GetKeyDown(KeyCode.F) && numBait > 0)
            {
                CircleCollider2D collider = GetComponent<CircleCollider2D>();
                Instantiate(bait, (Vector2)transform.position + collider.offset + new Vector2(collider.radius, -collider.radius - 0.1f), Quaternion.identity, null);
                numBait--;
            }
        }

//================================================================================================================================================================================
        private void CollectedEgg()
        {
            collectedEgg = true;
            if (stateMachine.currentState.CompareState("CrouchState"))
            {
                m_spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Crouching_Egg");
            }
            else
            {
                m_spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Egg");
            }
        }

//================================================================================================================================================================================
        public void Climbable(bool compareTag, bool entered)
        {
            if (compareTag)
			{
				canClimb = entered;
			    m_rigidbody.gravityScale = (entered ? 0 : 1);
			}
        }

//================================================================================================================================================================================
        public void Hideable(Collider2D collider, bool entered)
        {
            if (collider.CompareTag("Hideable"))
            {
                if (!entered && circleCollider.IsTouching(collider)) return;
                canHide = entered;
            }
        }

//===============================================================================================================================================================================
        private void OnCollisionEnter2D(Collision2D collision)
        {
            stateMachine.CollisionEntered(collision);
        }

//===============================================================================================================================================================================
        private void OnCollisionExit2D(Collision2D collision)
        {
            stateMachine.CollisionExited(collision);
        }

//===============================================================================================================================================================================
        private void OnTriggerEnter2D(Collider2D collision)
        {
            stateMachine.TriggerEntered(collision);
        }

//===============================================================================================================================================================================
        private void OnTriggerExit2D(Collider2D collision)
        {
            stateMachine.TriggerExited(collision);
        }
    }
}