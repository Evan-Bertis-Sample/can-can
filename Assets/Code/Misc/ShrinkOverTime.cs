using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.Misc
{
    public class ShrinkOverTime : MonoBehaviour
    {
        [SerializeField] private float _shrinkDelay = 3.0f;
        [SerializeField] private float _shrinkTime = 1.0f;
        [SerializeField] private Vector3 _finalScale = Vector3.zero;
        [SerializeField] private AnimationCurve _shrinkCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private bool _destroyOnShrink = true;

        private float _timeElapsed = 0;
        private Vector3 _initialScale;

        private void Start()
        {
            _initialScale = transform.localScale;
        }

        private void Update()
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed < _shrinkDelay)
            {
                return;
            }

            float t = _shrinkCurve.Evaluate((_timeElapsed - _shrinkDelay) / _shrinkTime);
            transform.localScale = Vector3.Lerp(_initialScale, _finalScale, t);

            if (_timeElapsed >= _shrinkDelay + _shrinkTime && _destroyOnShrink)
            {
                Destroy(gameObject);
            }
        }

    }
}