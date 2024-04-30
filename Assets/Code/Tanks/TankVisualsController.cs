using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FormulaBoy.Player
{
    public class TankVisualsController : MonoBehaviour
    {
        [Header("Model References")]
        [SerializeField] private GameObject _tankHead;
        [SerializeField] private GameObject _tankBody;

        [Header("Effects References")]
        [SerializeField] private ParticleSystem[] _exhaustEffects;
        [SerializeField] private Light[] _headLights;

        [Header("Effect Settings")]
        [SerializeField] private AnimationCurve _exhaustEffectSpeedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _exhaustEffectSpeedMultiplier = 10;
        [SerializeField] private bool _flickerHeadlights = true;
        [SerializeField] private float _flickerSpeed = 1;
        [SerializeField] private float _flickerIntensity = 10;
        [SerializeField] private float _flickerRange = .5f;
        [SerializeField] private float _randomFlickerIntensity = 1;
        [SerializeField] private float _randomFlickerSpeed = 1;
        [SerializeField] private bool _initialHeadlightsState = true;

        // State Variables
        private Quaternion _initialHeadRotation;
        private Quaternion _initialBodyRotation;

        public void SetHeadlights(bool state)
        {
            foreach (var light in _headLights)
            {
                light.enabled = state;
            }
        }

        public void SetHeadLookAt(Vector3 position)
        {
            _tankHead?.transform.LookAt(position);
        }

        public void SetBodyVelocity(Vector3 velocity)
        {
            if (_tankBody == null)
            {
                return;
            }

            // rotate the body to face the direction of movement
            if (velocity.magnitude > 0)
            {
                _tankBody.transform.rotation = Quaternion.Slerp(_tankBody.transform.rotation, Quaternion.LookRotation(velocity) * _initialBodyRotation, Time.deltaTime * 5);
            }
        }

        public void SetMovementSpeed(float speed)
        {
            foreach (var effect in _exhaustEffects)
            {
                var emission = effect.emission;
                emission.rateOverTime = speed;
            }
        }

        private void Start()
        {
            SetHeadlights(_initialHeadlightsState);
            _initialHeadRotation = _tankHead.transform.rotation;
            _initialBodyRotation = _tankBody.transform.rotation;
        }

        private void Update()
        {
            if (_flickerHeadlights)
            {
                foreach (var light in _headLights)
                {
                    light.intensity = _flickerIntensity + Mathf.Sin(Time.time * _flickerSpeed) * _flickerRange + Mathf.PerlinNoise(Time.time * _randomFlickerSpeed, 0) * _randomFlickerIntensity;
                }
            }
        }

    }
}