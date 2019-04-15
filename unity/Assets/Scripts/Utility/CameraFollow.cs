using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class CameraFollow : MonoBehaviour
    {
        Camera cam;
        [SerializeField]
        Transform focus, startPoint, endPoint;
        CircleCollider2D circle;
        [Range(0, 1)]
        public float deadZoneX = 0, deadZoneY = 0, lookForward;
        [SerializeField]
        Vector3 offset = new Vector3(0,4,-10);
        float width, height;

        private void Start()
        {
            cam = GetComponent<Camera>();
            circle = focus.GetComponent<CircleCollider2D>();
            Vector3 rect = cam.ViewportToWorldPoint(new Vector3(1, 1, -offset.z)) - transform.position;
            width = Mathf.Abs(rect.x*2);
            height = Mathf.Abs(rect.y*2);
        }

        void FixedUpdate()
        {
            Vector3 pos =focus.position + (Vector3)(circle.offset + Vector2.up*circle.radius);
            Vector3 desiredPos = pos + offset;
            Vector3 deadZone = cam.ViewportToWorldPoint(DeadZone()) - transform.position;
            if (Mathf.Abs((desiredPos - transform.position).x) <= deadZone.x) desiredPos.x = transform.position.x;
            if (Mathf.Abs((pos - transform.position).y) <= deadZone.y) desiredPos.y = transform.position.y;
            desiredPos.x = Mathf.Clamp(desiredPos.x, startPoint.position.x + width/2, endPoint.position.x - width/2);
            transform.position = Vector3.Lerp(transform.position, desiredPos, 0.025f);
            //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, desiredPos.y, desiredPos.z), 0.02f);
        }

        private Vector3 DeadZone()
        {
            return new Vector3(0.5f + (deadZoneX*0.5f), 0.5f + (deadZoneY*0.5f), -offset.z);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            if (circle == null) circle = focus.GetComponent<CircleCollider2D>();
            Gizmos.DrawSphere((Vector2)focus.transform.position + circle.offset + Vector2.up*circle.radius + (Vector2)offset, 0.1f);
            Gizmos.color = Color.red;
            if (cam == null) cam = GetComponent<Camera>();
            Vector3 deadbox = cam.ViewportToWorldPoint(DeadZone());
            Gizmos.DrawCube(transform.position, deadbox);
        }
    }
}
