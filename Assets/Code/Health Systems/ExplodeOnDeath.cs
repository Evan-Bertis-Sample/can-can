using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.HealthSystems
{
    public class ExplodeOnDeath : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private ParticleSystem _explosionParticles;
        [SerializeField] private float _explosionForce = 100;
        [SerializeField] private bool _destroyOnExplode = true;
        [SerializeField] private bool _destroyParticlesOnExplode = true;

        private void Start()
        {
            _health ??= GetComponent<Health>();
            _health.OnDeath += Explode;
        }

        private void Explode()
        {
            ParticleSystem explosion = Instantiate(_explosionParticles, transform.position, Quaternion.identity);
            Rigidbody[] rigidbodies = explosion.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.AddExplosionForce(_explosionForce, transform.position, 1);
            }
            if (_destroyOnExplode)
            {
                Destroy(gameObject);
            }

            if (_destroyParticlesOnExplode)
            {
                Destroy(explosion.gameObject, explosion.main.duration);
            }
        }
    }
}
