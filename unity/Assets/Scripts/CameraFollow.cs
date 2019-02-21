using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        Transform focus;
        [SerializeField]
        Vector3 offset = new Vector3(0,1,-10);

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, focus.position + offset, 0.125f);
        }
    }
}
