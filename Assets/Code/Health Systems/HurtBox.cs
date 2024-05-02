using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.HealthSystems
{
    [RequireComponent(typeof(Collider))]
    public class HurtBox : MonoBehaviour
    {
        public Collider Collider { get; private set; }
        [HideInInspector] public Health HurtBoxHealth;

        void Start()
        {
            Collider = GetComponent<Collider>();
        }

        void OnValidate()
        {
            Start();
        }

        public void DrawGizmos()
        {
            // Gizmos.color = Color.yellow;
            // if (Collider is BoxCollider)
            // {
            //     BoxCollider boxCollider = (BoxCollider)Collider;
            //     Gizmos.DrawWireCube(transform.position + (Vector3)boxCollider.offset, boxCollider.size);
            // }
            // // else if (Collider is CircleCollider)
            // // {
            // //     CircleCollider circleCollider = (CircleCollider)Collider;
            // //     Gizmos.DrawWireSphere(transform.position + (Vector3)circleCollider.offset, circleCollider.radius);
            // // }
            // else if (Collider is CapsuleCollider)
            // {
            //     CapsuleCollider capsuleCollider = (CapsuleCollider)Collider;
            //     // Draw the ends of the capsule
            //     Gizmos.DrawWireSphere(transform.position + (Vector3)capsuleCollider.offset + new Vector3(0, capsuleCollider.size.y / 2 - capsuleCollider.size.x / 2, 0), capsuleCollider.size.x / 2);
            //     Gizmos.DrawWireSphere(transform.position + (Vector3)capsuleCollider.offset - new Vector3(0, capsuleCollider.size.y / 2 - capsuleCollider.size.x / 2, 0), capsuleCollider.size.x / 2);
            //     // Draw the sides of the capsule as a rectangle
            //     Gizmos.DrawLine(transform.position + (Vector3)capsuleCollider.offset + new Vector3(capsuleCollider.size.x / 2, capsuleCollider.size.y / 2 - capsuleCollider.size.x / 2, 0), transform.position + (Vector3)capsuleCollider.offset + new Vector3(capsuleCollider.size.x / 2, -capsuleCollider.size.y / 2 + capsuleCollider.size.x / 2, 0));
            //     Gizmos.DrawLine(transform.position + (Vector3)capsuleCollider.offset - new Vector3(capsuleCollider.size.x / 2, capsuleCollider.size.y / 2 - capsuleCollider.size.x / 2, 0), transform.position + (Vector3)capsuleCollider.offset - new Vector3(capsuleCollider.size.x / 2, -capsuleCollider.size.y / 2 + capsuleCollider.size.x / 2, 0));
            // }
            // else
            // {
            //     Debug.LogWarning("Hurtbox has a collider that is not a box or circle collider");
            // }
        }

    }
}
