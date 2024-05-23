using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.HealthSystems
{
    public class DestroyOnHit : MonoBehaviour
    {
        [SerializeField] private HitBox _hitBox;
        [SerializeField] private float _destroyDelay = 0;

        private void Start()
        {
            _hitBox ??= GetComponent<HitBox>();
            _hitBox.OnHit += DestroySelf;
        }

        private void DestroySelf(HurtBox hurtBox)
        {
            Destroy(gameObject, _destroyDelay);
        }
    }
}