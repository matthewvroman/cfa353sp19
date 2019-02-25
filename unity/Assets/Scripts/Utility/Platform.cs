using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Platform : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
            GetComponent<BoxCollider2D>().size -= new Vector2(0, 0.2f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.1f);
        }
    }
}
