using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.HealthSystems
{
    [RequireComponent(typeof(Collider))]
    public class HitBox : MonoBehaviour
    {
        [field: SerializeField] public float Damage = 1f;
        [field: SerializeField] public float InvincibilityTimeGranted = 1f;
        [Tooltip("Multiplier for the knockback force. If this hitbox has a rigidbody, multiplies velocity. Otherwise multiplies the unit vector from this to the hurtbox")]
        [field: SerializeField] public float KnockbackMultiplier = 100f;
        [field: SerializeField, Range(0, 1)] public float CriticalHitChance = 0;
        [field: SerializeField] public float CriticalHitMultiplier = 2;

        public bool IsCriticalHit => Random.value < CriticalHitChance;

        public delegate void OnHitHandler(HurtBox hurtBox);
        public OnHitHandler OnHit;

        public delegate Vector2 GetKnockbackFn(Transform other);
        public GetKnockbackFn GetKnockback;

        public LayerMask CollisionMask = 0;
        private bool _useMask = false;

        public Collider Collider { get; private set; }

        public void SetCollisionMask(LayerMask mask)
        {
            CollisionMask = mask;
            _useMask = true;
        }

        void Start()
        {
            Rigidbody Rigidbody = GetComponent<Rigidbody>();
            if (Rigidbody == null)
            {
                Rigidbody = GetComponentInParent<Rigidbody>();
            }
            // I'm trying out a new style here. Instead of branching in the function, have the function branch ahead of time!
            // I wish more languages had automatic currying like Haskell.
            if (Rigidbody != null)
            {
                GetKnockback = (Transform other) =>
                {
                    // Absolute velocity instead of relative velocity. I don't expect this to be a problem, but if it is, we'll have to do something
                    // like get the rigidbody from the other Transform.
                    return Rigidbody.velocity * KnockbackMultiplier;
                };
            }
            else
            {
                GetKnockback = (Transform other) =>
                {
                    Vector2 direction = (other.position - transform.position).normalized;
                    return direction * KnockbackMultiplier;
                };

            }

            Collider = GetComponent<Collider>();
            if (Collider.isTrigger == false)
            {
                Debug.LogWarning("HitBox: Collider is not a trigger. This may cause unexpected behavior. Setting to trigger.");
                Collider.isTrigger = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_useMask)
            {
                if ((CollisionMask & (1 << other.gameObject.layer)) == 0) return;
            }

            HurtBox hurtBox = other.GetComponent<HurtBox>();
            if (hurtBox == null) return;
            Hit(hurtBox);
        }

        private void Hit(HurtBox hurtBox)
        {
            OnHit?.Invoke(hurtBox);
            hurtBox.HurtBoxHealth.TakeHit(this, InvincibilityTimeGranted);
        }

    }
}
