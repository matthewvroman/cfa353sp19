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
    }
}
