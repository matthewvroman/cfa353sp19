using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class EnemyTarget : MonoBehaviour
    {
        [SerializeField]
        int m_targetPriority = 0;

        public int GetPriority()
        {
            return m_targetPriority;
        }

        public Vector3 GetPosition()
        {
            CircleCollider2D circle = GetComponent<CircleCollider2D>();
            Vector2 position = (Vector2)transform.position + circle.offset;
            if (m_targetPriority == 0) position += Vector2.up*circle.radius;
            return position;
        }
    }
}
